using Harmony;
using RimWorld;
using AbilityUser;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.AI;
using AbilityUserAI;
using System.Reflection.Emit;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    internal class HarmonyPatches
    {
        private static readonly Type patchType = typeof(HarmonyPatches);

        static HarmonyPatches()
        {
            HarmonyInstance harmonyInstance = HarmonyInstance.Create(id: "rimworld.torann.tmagic");

            harmonyInstance.Patch(original: AccessTools.Method(type: typeof(GenDraw), name: "DrawRadiusRing"),
                prefix: new HarmonyMethod(type: patchType, name: nameof(DrawRadiusRing_Patch)));
            harmonyInstance.Patch(original: AccessTools.Method(type: typeof(IncidentWorker_SelfTame), name: "Candidates"), prefix: null,
                postfix: new HarmonyMethod(type: patchType, name: nameof(SelfTame_Candidates_Patch)), transpiler: null);
            harmonyInstance.Patch(original: AccessTools.Method(type: typeof(IncidentWorker_DiseaseHuman), name: "PotentialVictimCandidates"), prefix: null,
                postfix: new HarmonyMethod(type: patchType, name: nameof(DiseaseHuman_Candidates_Patch)), transpiler: null);
            harmonyInstance.Patch(original: AccessTools.Method(type: typeof(IncidentWorker_DiseaseAnimal), name: "PotentialVictimCandidates"), prefix: null,  //calls the same patch as human, which includes hediff for undead animals
                postfix: new HarmonyMethod(type: patchType, name: nameof(DiseaseHuman_Candidates_Patch)), transpiler: null);
            harmonyInstance.Patch(original: AccessTools.Method(type: typeof(Pawn), name: "GetGizmos"), prefix: null,
                postfix: new HarmonyMethod(type: patchType, name: nameof(Pawn_Gizmo_TogglePatch)), transpiler: null);
            harmonyInstance.Patch(original: AccessTools.Method(type: typeof(CompPowerPlant), name: "CompTick"), prefix: null,
                postfix: new HarmonyMethod(type: patchType, name: nameof(PowerCompTick_Overdrive_Postfix)), transpiler: null);
            harmonyInstance.Patch(original: AccessTools.Method(type: typeof(Building_TurretGun), name: "Tick"), prefix: null,
                postfix: new HarmonyMethod(type: patchType, name: nameof(TurretGunTick_Overdrive_Postfix)), transpiler: null);
            harmonyInstance.Patch(original: AccessTools.Method(type: typeof(CompRefuelable), name: "PostDraw"), prefix: new HarmonyMethod(type: patchType, name: nameof(CompRefuelable_DrawBar_Prefix)),
                postfix: null, transpiler: null);
            harmonyInstance.Patch(original: AccessTools.Method(type: typeof(AutoUndrafter), name: "AutoUndraftTick"), prefix: new HarmonyMethod(type: patchType, name: nameof(AutoUndrafter_Undead_Prefix)),
                postfix: null, transpiler: null);
            harmonyInstance.Patch(original: AccessTools.Method(type: typeof(PawnUtility), name: "IsTravelingInTransportPodWorldObject"),
                prefix: new HarmonyMethod(type: patchType, name: nameof(IsTravelingInTeleportPod_Prefix)), postfix: null);
            harmonyInstance.Patch(original: AccessTools.Method(type: typeof(FloatMenuMakerMap), name: "AddHumanlikeOrders"), prefix: null,
                postfix: new HarmonyMethod(type: patchType, name: nameof(AddHumanLikeOrders_RestrictEquipmentPatch)), transpiler: null);
            harmonyInstance.Patch(original: AccessTools.Method(type: typeof(CompAbilityItem), name: "PostDrawExtraSelectionOverlays"), prefix: new HarmonyMethod(type: patchType, name: nameof(CompAbilityItem_Overlay_Prefix)),
                postfix: null, transpiler: null);
            harmonyInstance.Patch(original: AccessTools.Method(type: typeof(Verb), name: "CanHitCellFromCellIgnoringRange"), prefix: new HarmonyMethod(type: patchType, name: nameof(RimmuNation_CHCFCIR_Patch)),
                postfix: null, transpiler: null);
            harmonyInstance.Patch(original: AccessTools.Method(type: typeof(WealthWatcher), name: "ForceRecount"), prefix: null,
                postfix: new HarmonyMethod(type: patchType, name: nameof(WealthWatcher_ClassAdjustment_Postfix)), transpiler: null);

            //harmonyInstance.Patch(AccessTools.Method(typeof(Thing), "get_Suspended", null, null), new HarmonyMethod(typeof(HarmonyPatches), "Get_Suspended_Polymorphed", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Pawn), "get_IsColonist", null, null), new HarmonyMethod(typeof(HarmonyPatches), "Get_IsColonist_Polymorphed", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Caravan), "get_NightResting", null, null), new HarmonyMethod(typeof(HarmonyPatches), "Get_NightResting_Undead", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Pawn_StanceTracker), "get_Staggered", null, null), new HarmonyMethod(typeof(HarmonyPatches), "Get_Staggered", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Verb_LaunchProjectile), "get_Projectile", null, null), new HarmonyMethod(typeof(HarmonyPatches), "Get_Projectile_ES", null), null);

            harmonyInstance.Patch(AccessTools.Method(typeof(Pawn_PathFollower), "CostToMoveIntoCell", new Type[]
                {
                    typeof(Pawn),
                    typeof(IntVec3)
                }, null), new HarmonyMethod(typeof(HarmonyPatches), "Pawn_PathFollower_Pathfinder_Prefix", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(MemoryThoughtHandler), "TryGainMemory", new Type[]
                {
                    typeof(ThoughtDef),
                    typeof(Pawn)
                }, null), new HarmonyMethod(typeof(HarmonyPatches), "MemoryThoughtHandler_PreventDisturbedRest_Prefix", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(PawnRenderer), "RenderPawnInternal", new Type[]
                {
                    typeof(Vector3),
                    typeof(float),
                    typeof(bool),
                    typeof(Rot4),
                    typeof(Rot4),
                    typeof(RotDrawMode),
                    typeof(bool),
                    typeof(bool)
                }, null), new HarmonyMethod(typeof(HarmonyPatches), "PawnRenderer_UndeadInternal_Prefix", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(PawnRenderer), "RenderPawnAt", new Type[]
                {
                    typeof(Vector3),
                    typeof(RotDrawMode),
                    typeof(bool)
                }, null), new HarmonyMethod(typeof(HarmonyPatches), "PawnRenderer_Undead_Prefix", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(PawnRenderer), "RenderPawnAt", new Type[]
                {
                    typeof(Vector3),
                    typeof(RotDrawMode),
                    typeof(bool)
                }, null), new HarmonyMethod(typeof(HarmonyPatches), "PawnRenderer_Blur_Prefix", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(PawnDiedOrDownedThoughtsUtility), "AppendThoughts_Relations", new Type[]
                {
                    typeof(Pawn),
                    typeof(DamageInfo?),
                    typeof(PawnDiedOrDownedThoughtsKind),
                    typeof(List<IndividualThoughtToAdd>),
                    typeof(List<ThoughtDef>)
                }, null), new HarmonyMethod(typeof(HarmonyPatches), "AppendThoughts_Relations_PrefixPatch", null), null, null);
            harmonyInstance.Patch(AccessTools.Method(typeof(PawnDiedOrDownedThoughtsUtility), "AppendThoughts_ForHumanlike", new Type[]
                {
                    typeof(Pawn),
                    typeof(DamageInfo?),
                    typeof(PawnDiedOrDownedThoughtsKind),
                    typeof(List<IndividualThoughtToAdd>),
                    typeof(List<ThoughtDef>)
                }, null), new HarmonyMethod(typeof(HarmonyPatches), "AppendThoughts_ForHumanlike_PrefixPatch", null), null, null);
            harmonyInstance.Patch(AccessTools.Method(typeof(PawnDiedOrDownedThoughtsUtility), "TryGiveThoughts", new Type[]
                {
                    typeof(Pawn),
                    typeof(DamageInfo?),
                    typeof(PawnDiedOrDownedThoughtsKind)
                }, null), new HarmonyMethod(typeof(HarmonyPatches), "TryGiveThoughts_PrefixPatch", null), null, null);


            #region PrisonLabor
            {
                try
                {
                    ((Action)(() =>
                    {
                        if (ModCheck.Validate.PrisonLabor.IsInitialized())
                        {
                            harmonyInstance.Patch(AccessTools.Method(typeof(PrisonLabor.JobDriver_Mine_Tweak), "ResetTicksToPickHit"), null, new HarmonyMethod(typeof(HarmonyPatches), "TM_PrisonLabor_JobDriver_Mine_Tweak"));
                        }
                    }))();
                }
                catch (TypeLoadException) { }

                //try
                //{
                //    ((Action)(() =>
                //    {
                //        if (AccessTools.Method(typeof(PrisonLabor.JobDriver_Mine_Tweak), nameof(PrisonLabor.JobDriver_Mine_Tweak.ExposeData)) != null)
                //        {
                //            harmonyInstance.Patch(AccessTools.Method(typeof(PrisonLabor.JobDriver_Mine_Tweak), "ResetTicksToPickHit"), null, new HarmonyMethod(typeof(HarmonyPatches), "TM_PrisonLabor_JobDriver_Mine_Tweak"));
                //        }
                //    }))();
                //}
                //catch (TypeLoadException) { }
            }
            #endregion PrisonLabor

            #region Children
            {
                try
                {
                    ((Action)(() =>
                    {
                        if (ModCheck.Validate.ChildrenSchoolLearning.IsInitialized())
                        {
                            harmonyInstance.Patch(AccessTools.Method(typeof(PawnUtility), "TrySpawnHatchedOrBornPawn"), null, new HarmonyMethod(typeof(HarmonyPatches), "TM_Children_TrySpawnHatchedOrBornPawn_Tweak"));
                        }
                    }))();
                }
                catch (TypeLoadException) { }
            }
            #endregion Children           

        }

        [HarmonyPatch(typeof(AbilityDecisionConditionalNode_CasterHealth), "CanContinueTraversing", null)]
        public class CasterHealth_Check_Patch
        {
            public static bool Prefix(AbilityDecisionConditionalNode_CasterHealth __instance, Pawn caster, ref bool __result)
            {
                bool flag = caster.health.summaryHealth.SummaryHealthPercent >= __instance.minHealth && caster.health.summaryHealth.SummaryHealthPercent <= __instance.maxHealth;
                if(__instance.invert)
                {
                    __result = !flag;
                }
                __result = flag;
                return false;
                //Log.Message("caster healthscale is " + caster.HealthScale + " and summary health pct is " + caster.health.summaryHealth.SummaryHealthPercent);
                //return true;
            }
        }

        public static void WealthWatcher_ClassAdjustment_Postfix(WealthWatcher __instance, bool allowDuringInit = false)
        {
            float wealthPawns = Traverse.Create(root: __instance).Field(name: "wealthPawns").GetValue<float>();
            Map map = Traverse.Create(root: __instance).Field(name: "map").GetValue<Map>();
            if (wealthPawns != 0 && map != null)
            {
                foreach (Pawn item in map.mapPawns.PawnsInFaction(Faction.OfPlayer))
                {
                    CompAbilityUserMagic compMagic = item.GetComp<CompAbilityUserMagic>();
                    CompAbilityUserMight compMight = item.GetComp<CompAbilityUserMight>();                    
                    if(compMight != null && compMight.IsMightUser)
                    {
                        wealthPawns += 400 + (compMight.MightUserLevel * 20);
                    }
                    else if (compMagic != null && compMagic.IsMagicUser)
                    {
                        wealthPawns += 500 + (compMagic.MagicUserLevel * 15);                        
                    }
                }
                Traverse.Create(root: __instance).Field(name: "wealthPawns").SetValue(wealthPawns);
            }
        }

        [HarmonyPatch(typeof(CaravanFormingUtility), "AllSendablePawns", null)]
        public class AddNullException_Alerts_Patch
        {
            public static void Postfix(ref List<Pawn> __result)
            {
                if (__result != null)
                {
                    for (int i = 0; i < __result.Count; i++)
                    {
                        CompPolymorph comp = __result[i].GetComp<CompPolymorph>();
                        if (comp != null && comp.Original != null)
                        {
                            __result.Remove(__result[i]);
                            i--;                            
                        }
                    }
                }
            }
        }

        [HarmonyPriority(2000)] 
        public static bool RimmuNation_CHCFCIR_Patch(Verb __instance, IntVec3 sourceSq, IntVec3 targetLoc, bool includeCorners, ref bool __result)
        {
            if (__instance != null && (__instance.verbProps.verbClass.ToString().Contains("AbilityUser") || __instance.verbProps.verbClass.ToString().Contains("TorannMagic")))
            {
                __result = true;
                VerbProperties verbProps = Traverse.Create(root: __instance).Field(name: "verbProps").GetValue<VerbProperties>();
                Thing caster = Traverse.Create(root: __instance).Field(name: "caster").GetValue<Thing>();
                if (verbProps.mustCastOnOpenGround && (!targetLoc.Standable(caster.Map) || caster.Map.thingGrid.CellContains(targetLoc, ThingCategory.Pawn)))
                {
                    __result = false;
                }
                if (verbProps.requireLineOfSight)
                {
                    if (!includeCorners)
                    {
                        if (!GenSight.LineOfSight(sourceSq, targetLoc, caster.Map, skipFirstCell: true))
                        {
                            __result = false;
                        }
                    }
                    else if (!GenSight.LineOfSightToEdges(sourceSq, targetLoc, caster.Map, skipFirstCell: true))
                    {
                        __result = false;
                    }
                }
                return false;
            }
            return true;
        }

        public static bool Pawn_PathFollower_Pathfinder_Prefix(Pawn pawn, IntVec3 c, ref int __result)
        {
            if (pawn != null && pawn.health != null && pawn.health.hediffSet != null && pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_ArtifactPathfindingHD))
            {
                int x = c.x;
                IntVec3 position = pawn.Position;
                int num;
                if (x != position.x)
                {
                    int z = c.z;
                    IntVec3 position2 = pawn.Position;
                    if (z != position2.z)
                    {
                        num = pawn.TicksPerMoveDiagonal;
                        goto IL_0047;
                    }
                }
                num = pawn.TicksPerMoveCardinal;
                goto IL_0047;
                IL_0047:
                if (num > 450)
                {
                    num = 450;
                }
                if (pawn.CurJob != null)
                {
                    switch (pawn.jobs.curJob.locomotionUrgency)
                    {
                        case LocomotionUrgency.Amble:
                            num *= 3;
                            if (num < 60)
                            {
                                num = 60;
                            }
                            break;
                        case LocomotionUrgency.Walk:
                            num *= 2;
                            if (num < 50)
                            {
                                num = 50;
                            }
                            break;
                        case LocomotionUrgency.Jog:
                            num = num;
                            break;
                        case LocomotionUrgency.Sprint:
                            num = Mathf.RoundToInt((float)num * 0.75f);
                            break;
                    }
                }
                __result = num;
                return false;
            }
            return true;
        }
    

        public static bool MemoryThoughtHandler_PreventDisturbedRest_Prefix(MemoryThoughtHandler __instance, ThoughtDef def, Pawn otherPawn = null)
        {
            Pawn pawn = Traverse.Create(root: __instance).Field(name: "pawn").GetValue<Pawn>();
            if (pawn != null && pawn.health != null && pawn.health.hediffSet != null && pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_ArtifactRenewalHD"), false))
            {
                return false;
            }
            return true;
        }

        public static bool Get_IsColonist_Polymorphed(Pawn __instance, ref bool __result)
        {
            if (__instance.GetComp<CompPolymorph>() != null)
            {
                __result = __instance.Faction != null && __instance.Faction.IsPlayer;
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(GenDraw), "DrawMeshNowOrLater", null)]
        public class DrawMesh_Cloaks_Patch
        {
            public static bool Prefix(Mesh mesh, Vector3 loc, Quaternion quat, Material mat, bool drawNow)
            {
                if(mat != null && mat.mainTexture != null && mat.mainTexture.name != null && (mat.mainTexture.ToString().Contains("demonlordcloak") || mat.mainTexture.name.Contains("opencloak")))
                {
                    loc.y += .015f;
                    if(mat.name.ToString().Contains("_north"))
                    {
                        loc.y += .006f;
                    }
                    if (drawNow)
                    {
                        mat.SetPass(0);
                        Graphics.DrawMeshNow(mesh, loc, quat);
                    }
                    else
                    {
                        Graphics.DrawMesh(mesh, loc, quat, mat, 0);
                    }
                    return false;
                }
                return true;
            }
        }

        //code crashes linux and mac when a pawn dies
        //[HarmonyPatch(typeof(DeathActionWorker_Simple), "PawnDied", null)]
        //public class Undead_DeathActionWorker_Patch
        //{
        //    public static bool Prefix(Corpse corpse)
        //    {
        //        if(corpse != null && corpse.InnerPawn != null)
        //        {
        //            Pawn innerPawn = corpse.InnerPawn;
        //            if(innerPawn.health != null && innerPawn.health.hediffSet != null && (innerPawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD"), false) || innerPawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadAnimalHD"), false)))
        //            {
        //                Faction faction = new Faction();
        //                faction.def = TorannMagicDefOf.TM_SummonedFaction;
        //                innerPawn.SetFactionDirect(faction);
        //            }
        //        }
        //        return true;
        //    }
        //}

        [HarmonyPatch(typeof(Corpse), "PostMake", null)]
        public class Corpse_UndeadStage_Patch
        {
            public static void Postfix(Corpse __instance)
            {
                CompRottable compRottable = __instance.GetComp<CompRottable>();
                Pawn undeadPawn = __instance.InnerPawn;
                if(compRottable != null && undeadPawn != null)
                {
                    Hediff hediff = undeadPawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_UndeadStageHD"), false);
                    if (hediff != null)
                    {
                        compRottable.RotProgress = hediff.Severity;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(FloatMenuMakerMap), "AddJobGiverWorkOrders", null)]
        public class SkipPolymorph_UndraftedOrders_Patch
        {
            public static bool Prefix(IntVec3 clickCell, Pawn pawn, List<FloatMenuOption> opts, bool drafted)
            {
                if(pawn.GetComp<CompPolymorph>() != null && pawn.GetComp<CompPolymorph>().Original != null)
                {
                    return false;
                }
                return true;
            }
        }

        public static bool CompAbilityItem_Overlay_Prefix(CompAbilityItem __instance)
        {
            Graphic Overlay = Traverse.Create(root: __instance).Field(name: "Overlay").GetValue<Graphic>();
            if(Overlay != null)
            {
                return true;
            }
            return false;
        }


        public static bool CompRefuelable_DrawBar_Prefix(CompRefuelable __instance)
        {
            if(__instance.parent.def.defName == "TM_ArcaneCapacitor")
            {
                if (!__instance.HasFuel && __instance.Props.drawOutOfFuelOverlay)
                {
                    __instance.parent.Map.overlayDrawer.DrawOverlay(__instance.parent, OverlayTypes.OutOfFuel);
                }
                if (__instance.Props.drawFuelGaugeInMap)
                {
                    GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
                    r.center = __instance.parent.DrawPos + Vector3.up * 0.1f;
                    r.center.z -= .2f;
                    r.size = new Vector2(.5f, .15f);
                    r.fillPercent = __instance.FuelPercentOfMax;
                    r.filledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.6f, 0.0f, 0.6f));
                    r.unfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f));
                    r.margin = 0.15f;
                    Rot4 rotation = __instance.parent.Rotation;
                    if (!rotation.IsHorizontal)
                    {
                        rotation.Rotate(RotationDirection.Clockwise);
                    }
                    r.rotation = rotation;
                    GenDraw.DrawFillableBar(r);
                }
                return false;
            }
            if (__instance.parent.def.defName == "TM_DimensionalManaPocket")
            {
                if (!__instance.HasFuel && __instance.Props.drawOutOfFuelOverlay)
                {
                    __instance.parent.Map.overlayDrawer.DrawOverlay(__instance.parent, OverlayTypes.OutOfFuel);
                }
                if (__instance.Props.drawFuelGaugeInMap)
                {
                    GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
                    r.center = __instance.parent.DrawPos + Vector3.up * 0.1f;
                    r.center.z -= .6f;
                    r.size = new Vector2(.5f, .15f);
                    r.fillPercent = __instance.FuelPercentOfMax;
                    r.filledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.6f, 0.0f, 0.6f));
                    r.unfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f));
                    r.margin = 0.15f;
                    Rot4 rotation = __instance.parent.Rotation;
                    if (rotation.IsHorizontal)
                    {
                        rotation.Rotate(RotationDirection.Clockwise);
                    }
                    r.rotation = rotation;
                    GenDraw.DrawFillableBar(r);
                }
                return false;
            }
            return true;
        }

        public static bool AutoUndrafter_Undead_Prefix(AutoUndrafter __instance)
        {
            Pawn pawn = Traverse.Create(root: __instance).Field(name: "pawn").GetValue<Pawn>();
            if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD")))
            {
                return false;
            }
            return true;
        }

        public static bool PawnRenderer_Blur_Prefix(PawnRenderer __instance, ref Vector3 drawLoc, ref RotDrawMode bodyDrawType, bool headStump)
        {
            Pawn pawn = Traverse.Create(root: __instance).Field(name: "pawn").GetValue<Pawn>();
            if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_BlurHD))
            {
                int blurTick = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_BlurHD).TryGetComp<HediffComp_Blur>().blurTick;
                if (blurTick >  Find.TickManager.TicksGame - 10)
                {
                    float blurMagnitude = (10 / (Find.TickManager.TicksGame - blurTick + 1)) + 5f;
                    Vector3 blurLoc = drawLoc;
                    blurLoc.x += Rand.Range(-.03f, .03f) * blurMagnitude;
                    //blurLoc.z += Rand.Range(-.01f, .01f) * blurMagnitude;
                    drawLoc = blurLoc;
                }
            }            
            return true;
        }

        public static bool PawnRenderer_Undead_Prefix(PawnRenderer __instance, Vector3 drawLoc, ref RotDrawMode bodyDrawType, bool headStump)
        {
            Pawn pawn = Traverse.Create(root: __instance).Field(name: "pawn").GetValue<Pawn>();
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadStageHD")))
            {
                if (settingsRef.changeUndeadPawnAppearance && pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD")))
                {
                    Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_UndeadStageHD"));
                    if (hediff.Severity < 1)
                    {
                        bodyDrawType = RotDrawMode.Rotting;
                    }
                    else
                    {
                        bodyDrawType = RotDrawMode.Dessicated;
                    }
                }
                if (settingsRef.changeUndeadAnimalAppearance && pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadAnimalHD")))
                {
                    Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_UndeadStageHD"));
                    if (hediff.Severity < 1)
                    {
                        bodyDrawType = RotDrawMode.Rotting;
                    }
                    else
                    {
                        bodyDrawType = RotDrawMode.Dessicated;
                    }
                }
            }
            return true;
        }

        public static bool PawnRenderer_UndeadInternal_Prefix(PawnRenderer __instance, Vector3 rootLoc, float angle, bool renderBody, Rot4 bodyFacing, Rot4 headFacing, ref RotDrawMode bodyDrawType, bool portrait, bool headStump)
        {
            Pawn pawn = Traverse.Create(root: __instance).Field(name: "pawn").GetValue<Pawn>();
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadStageHD")))
            {
                if (settingsRef.changeUndeadPawnAppearance && pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD")))
                {
                    Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_UndeadStageHD"));
                    if (hediff.Severity < 1)
                    {
                        bodyDrawType = RotDrawMode.Rotting;
                    }
                    else
                    {
                        bodyDrawType = RotDrawMode.Dessicated;
                    }
                }
                if (settingsRef.changeUndeadAnimalAppearance && pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadAnimalHD")))
                {
                    Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_UndeadStageHD"));
                    if (hediff.Severity < 1)
                    {
                        bodyDrawType = RotDrawMode.Rotting;
                    }
                    else
                    {
                        bodyDrawType = RotDrawMode.Dessicated;
                    }
                }
            }
            return true;
        }

        public static void TurretGunTick_Overdrive_Postfix(Building_TurretGun __instance)
        {            
            Thing overdriveThing = __instance;
            if(!overdriveThing.DestroyedOrNull() && overdriveThing.Map != null)
            {
                int burstCooldownTicksLeft = Traverse.Create(root: __instance).Field(name: "burstCooldownTicksLeft").GetValue<int>();
                int burstWarmupTicksLeft = Traverse.Create(root: __instance).Field(name: "burstWarmupTicksLeft").GetValue<int>();
                List<Pawn> mapPawns = overdriveThing.Map.mapPawns.AllPawnsSpawned;
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    Pawn pawn = mapPawns[i];
                    if (!pawn.DestroyedOrNull() && pawn.RaceProps.Humanlike && pawn.story != null)
                    {
                        CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
                        if (pawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer) && comp.IsMagicUser && comp.overdriveBuilding != null)
                        {
                            if (overdriveThing == comp.overdriveBuilding)
                            {
                                if (burstCooldownTicksLeft >= 5)
                                {
                                    Traverse.Create(root: __instance).Field(name: "burstCooldownTicksLeft").SetValue(burstCooldownTicksLeft -= 1 + Rand.Range(0, comp.MagicData.MagicPowerSkill_Overdrive.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Overdrive_pwr").level));
                                }
                                if (burstWarmupTicksLeft >= 5)
                                {
                                    Traverse.Create(root: __instance).Field(name: "burstWarmupTicksLeft").SetValue(burstCooldownTicksLeft -= 5);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void PowerCompTick_Overdrive_Postfix(CompPowerPlant __instance)
        {
            float DesiredPowerOutput = Traverse.Create(root: __instance).Field(name: "DesiredPowerOutput").GetValue<float>();

            Thing overdriveThing = __instance.parent;
            if (overdriveThing != null && __instance.PowerOn && __instance.powerOutputInt != 0)
            {
                List<Pawn> mapPawns = overdriveThing.Map.mapPawns.AllPawnsSpawned;
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    Pawn pawn = mapPawns[i];
                    if (!pawn.DestroyedOrNull() && pawn.RaceProps.Humanlike && pawn.story != null)
                    {
                        CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
                        if (pawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer) && comp.IsMagicUser && comp.overdriveBuilding != null)
                        {
                            if (overdriveThing == comp.overdriveBuilding)
                            {                                
                                __instance.powerOutputInt = comp.overdrivePowerOutput;
                               
                            }
                        }
                    }
                }
            }
        }

        public static void DiseaseHuman_Candidates_Patch(ref IEnumerable<Pawn> __result)
        {
            List<Pawn> tempList = __result.ToList();
            List<Pawn> removalList = new List<Pawn>();
            removalList.Clear();
            for (int i = 0; i < tempList.Count(); i++)
            {
                if (tempList[i].health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD")) || tempList[i].health.hediffSet.HasHediff(HediffDef.Named("TM_LichHD")) || tempList[i].health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadAnimalHD")))
                {
                    removalList.Add(tempList[i]);
                }
            }
            __result = tempList.Except(removalList);
        }

        public static void SelfTame_Candidates_Patch(Map map, ref IEnumerable<Pawn> __result)
        {
            List<Pawn> tempList = __result.ToList();
            List<Pawn> removalList = new List<Pawn>();
            removalList.Clear();
            for (int i = 0; i < tempList.Count(); i++)
            {
                if (tempList[i].def.thingClass.ToString() == "TorannMagic.TMPawnSummoned")
                {
                    removalList.Add(tempList[i]);
                }
            }
            __result = tempList.Except(removalList);
        }

        public static bool DrawRadiusRing_Patch(IntVec3 center, float radius)
        {
            if (radius > GenRadial.MaxRadialPatternRadius)
            {
                return false;
            }
            return true;
        }

        public static bool TryGiveThoughts_PrefixPatch(ref Pawn victim)
        {
            if (victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadHD, false) || victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD) || victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD_I) || victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD_II) || victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD_III))
            {
                return false;
            }
            return true;
        }

        public static bool AppendThoughts_ForHumanlike_PrefixPatch(ref Pawn victim)
        {
            if (victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadHD, false) || victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD) || victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD_I) || victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD_II) || victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD_III))
            {
                return false;
            }
            return true;
        }

        public static bool AppendThoughts_Relations_PrefixPatch(ref Pawn victim)
        {
            if (victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadHD, false) || victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD) || victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD_I) || victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD_II) || victim.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD_III))
            {
                return false;
            }
            return true;
        }

        public static void TM_PrisonLabor_JobDriver_Mine_Tweak(JobDriver __instance)
        {
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (Rand.Chance(settingsRef.magicyteChance))
            {
                Thing thing = null;
                thing = ThingMaker.MakeThing(TorannMagicDefOf.RawMagicyte);
                thing.stackCount = Rand.Range(5, 12);
                if (thing != null)
                {
                    GenPlace.TryPlaceThing(thing, __instance.pawn.Position, __instance.pawn.Map, ThingPlaceMode.Near, null);
                }
            }
        }

        [HarmonyPriority(100)]
        public static void TM_Children_TrySpawnHatchedOrBornPawn_Tweak(ref Pawn pawn, Thing motherOrEgg, ref bool __result)
        {
            if (pawn.RaceProps.Humanlike && pawn.story != null)
            {
                bool hasMagicTrait = false;
                bool hasFighterTrait = false;
                List<Trait> pawnTraits = pawn.story.traits.allTraits;
                for (int i = 0; i < pawnTraits.Count(); i++)
                {
                    if (pawnTraits[i].def == TorannMagicDefOf.Arcanist || pawnTraits[i].def == TorannMagicDefOf.Geomancer || pawnTraits[i].def == TorannMagicDefOf.Warlock || pawnTraits[i].def == TorannMagicDefOf.Succubus ||
                        pawnTraits[i].def == TorannMagicDefOf.InnerFire || pawnTraits[i].def == TorannMagicDefOf.HeartOfFrost || pawnTraits[i].def == TorannMagicDefOf.StormBorn || pawnTraits[i].def == TorannMagicDefOf.Technomancer ||
                        pawnTraits[i].def == TorannMagicDefOf.Paladin || pawnTraits[i].def == TorannMagicDefOf.Summoner || pawnTraits[i].def == TorannMagicDefOf.Druid || pawnTraits[i].def == TorannMagicDefOf.Necromancer ||
                        pawnTraits[i].def == TorannMagicDefOf.Lich || pawnTraits[i].def == TorannMagicDefOf.Priest || pawnTraits[i].def == TorannMagicDefOf.TM_Bard || pawnTraits[i].def == TorannMagicDefOf.Gifted ||
                        pawnTraits[i].def == TorannMagicDefOf.Technomancer || pawnTraits[i].def == TorannMagicDefOf.BloodMage || pawnTraits[i].def == TorannMagicDefOf.Enchanter)
                    {
                        pawnTraits.Remove(pawnTraits[i]);
                        i--;
                        hasMagicTrait = true;
                    }
                    if (pawnTraits[i].def == TorannMagicDefOf.Gladiator || pawnTraits[i].def == TorannMagicDefOf.Bladedancer || pawnTraits[i].def == TorannMagicDefOf.TM_Sniper || pawnTraits[i].def == TorannMagicDefOf.Ranger ||
                        pawnTraits[i].def == TorannMagicDefOf.TM_Psionic || pawnTraits[i].def == TorannMagicDefOf.Faceless || pawnTraits[i].def == TorannMagicDefOf.DeathKnight || pawnTraits[i].def == TorannMagicDefOf.PhysicalProdigy)
                    {
                        pawnTraits.Remove(pawnTraits[i]);
                        i--;
                        hasFighterTrait = true;
                    }
                }
                if (hasFighterTrait && hasMagicTrait)
                {
                    if (Rand.Chance(.5f))
                    {
                        pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Gifted, 2, false));
                    }
                    else
                    {
                        pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.PhysicalProdigy, 2, false));
                    }
                }
                else if (hasFighterTrait)
                {
                    pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.PhysicalProdigy, 2, false));
                }
                else if (hasMagicTrait)
                {
                    pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Gifted, 2, false));
                }
            }
        }

        public static bool Get_Staggered(Pawn_StanceTracker __instance, ref bool __result)
        {
            if (__instance.pawn.def.defName == "TM_DemonR")
            {
                __result = false;
                return false;
            }
            return true;
        }

        public static bool Get_Projectile_ES(Verb_LaunchProjectile __instance, ref ThingDef __result)
        {
            if(__instance.caster != null && __instance.caster is Pawn && __instance.Bursting)
            {
                Pawn pawn = __instance.caster as Pawn;
                CompAbilityUserMagic comp = pawn.TryGetComp<CompAbilityUserMagic>();
                if(pawn.RaceProps.Humanlike && pawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer) && comp.HasTechnoWeapon && comp.useElementalShotToggle && pawn.equipment.Primary.def.IsRangedWeapon && pawn.equipment.Primary.def.techLevel >= TechLevel.Industrial)
                {
                    int verVal = comp.MagicData.MagicPowerSkill_TechnoWeapon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoWeapon_ver").level;
                    if (Rand.Chance(.2f + .01f * verVal) && comp.Mana.CurLevel >= .02f)
                    {
                        ThingDef projectile = null;
                        float rnd = Rand.Range(0f, 1f);
                        if(rnd <= .33f) //fire
                        {
                            projectile = ThingDef.Named("Bullet_ES_Fire");
                            projectile.projectile.explosionRadius = __instance.verbProps.defaultProjectile.projectile.explosionRadius + (1 + .05f * verVal);
                            comp.Mana.CurLevel -= (.02f - .0008f * verVal);
                            comp.MagicUserXP += 4;
                        }
                        else if(rnd <= .66f) //ice
                        {
                            projectile = ThingDef.Named("Bullet_ES_Ice");
                            comp.Mana.CurLevel -= (.01f - .0004f * verVal);
                            comp.MagicUserXP += 2;
                        }
                        else //stun
                        {
                            projectile = ThingDef.Named("Bullet_ES_Lit");
                            comp.Mana.CurLevel -= (.015f - .0006f * verVal);
                            comp.MagicUserXP += 3;
                        }
                        __result = projectile;
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool Get_NightResting_Undead(Caravan __instance, ref bool __result)
        {
            List<Pawn> undeadCaravan = __instance.PawnsListForReading;
            bool allUndead = true;
            for (int i = 0; i < undeadCaravan.Count; i++)
            {
                if (undeadCaravan[i].IsColonist && !(undeadCaravan[i].health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD")) || undeadCaravan[i].health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadAnimalHD")) || undeadCaravan[i].health.hediffSet.HasHediff(HediffDef.Named("TM_LichHD"))))
                {
                    allUndead = false;
                }
            }
            return !allUndead;
        }

        public static void Pawn_Gizmo_TogglePatch(ref IEnumerable<Gizmo> __result, ref Pawn __instance)
        {

            if (__instance == null || !__instance.RaceProps.Humanlike)
            {
                return;
            }
            if (__result == null || !__result.Any())
            {
                return;
            }
            if (!__instance.Faction.Equals(Faction.OfPlayer) || __instance.story == null || __instance.story.traits.allTraits.Count < 1)
            {
                return;
            }
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();

            if (Find.Selector.NumSelected == 1)
            {
                CompAbilityUserMagic compMagic = __instance.GetComp<CompAbilityUserMagic>();
                CompAbilityUserMight compMight = __instance.GetComp<CompAbilityUserMight>();
                var gizmoList = __result.ToList();
                if (settingsRef.showGizmo)
                {
                    if (compMagic == null && compMight == null)
                    {
                        return;
                    }
                    if (!compMagic.IsMagicUser && !compMight.IsMightUser)
                    {
                        return;
                    }

                    Gizmo_EnergyStatus energyGizmo = new Gizmo_EnergyStatus
                    {
                        //All gizmo properties done in Gizmo_EnergyStatus
                        //Make it the first thing you see
                        pawn = __instance,
                        order = -101f
                    };

                    gizmoList.Add(energyGizmo);

                }
                if (__instance.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic))
                {
                    String toggle = "psionicaugmentation";
                    String label = "TM_AugmentationsEnabled".Translate();
                    String desc = "TM_AugmentationsToggleDesc".Translate();
                    if (!compMight.usePsionicAugmentationToggle)
                    {
                        toggle = "psionicaugmentation_off";
                        label = "TM_AugmentationsDisabled".Translate();
                    }
                    Command_Toggle item = new Command_Toggle
                    {
                        defaultLabel = label,
                        defaultDesc = desc,
                        order = -90,
                        icon = ContentFinder<Texture2D>.Get("UI/" + toggle, true),
                        isActive = (() => compMight.usePsionicAugmentationToggle),
                        toggleAction = delegate
                        {
                            compMight.usePsionicAugmentationToggle = !compMight.usePsionicAugmentationToggle;
                        }
                    };
                    gizmoList.Add(item);
                }
                if (__instance.story.traits.HasTrait(TorannMagicDefOf.Technomancer) && compMagic.HasTechnoBit)
                {
                    String toggle = "bit_c";
                    String label = "TM_TechnoBitEnabled".Translate();
                    String desc = "TM_TechnoBitToggleDesc".Translate();
                    if (!compMagic.useTechnoBitToggle)
                    {
                        toggle = "bit_off";
                        label = "TM_TechnoBitDisabled".Translate();
                    }
                    var item = new Command_Toggle
                    {
                        isActive = () => compMagic.useTechnoBitToggle,
                        toggleAction = () =>
                        {
                            compMagic.useTechnoBitToggle = !compMagic.useTechnoBitToggle;
                        },
                        defaultLabel = label,
                        defaultDesc = desc,
                        order = -89,
                        icon = ContentFinder<Texture2D>.Get("UI/" + toggle, true)                            
                    };
                    gizmoList.Add(item);

                    String toggle_repair = "bit_repairon";
                    String label_repair = "TM_TechnoBitRepair".Translate();
                    String desc_repair = "TM_TechnoBitRepairDesc".Translate();
                    if (!compMagic.useTechnoBitRepairToggle)
                    {
                        toggle_repair = "bit_repairoff";
                    }
                    var item_repair = new Command_Toggle
                    {
                        isActive = () => compMagic.useTechnoBitRepairToggle,
                        toggleAction = () =>
                        {
                            compMagic.useTechnoBitRepairToggle = !compMagic.useTechnoBitRepairToggle;
                        },
                        defaultLabel = label_repair,
                        defaultDesc = desc_repair,
                        order = -88,
                        icon = ContentFinder<Texture2D>.Get("UI/" + toggle_repair, true)
                    };
                    gizmoList.Add(item_repair);
                }

                if (__instance.story.traits.HasTrait(TorannMagicDefOf.Technomancer) && compMagic.HasTechnoWeapon)
                {
                    String toggle = "elementalshot";
                    String label = "TM_TechnoWeapon_ver".Translate();
                    String desc = "TM_ElementalShotToggleDesc".Translate();
                    if (!compMagic.useElementalShotToggle)
                    {
                        toggle = "elementalshot_off";
                    }
                    var item = new Command_Toggle
                    {
                        isActive = () => compMagic.useElementalShotToggle,
                        toggleAction = () =>
                        {
                            compMagic.useElementalShotToggle = !compMagic.useElementalShotToggle;
                        },
                        defaultLabel = label,
                        defaultDesc = desc,
                        order = -88,
                        icon = ContentFinder<Texture2D>.Get("UI/" + toggle, true)
                    };
                    gizmoList.Add(item);
                }
                __result = gizmoList;
            }
        }


        [HarmonyPatch(typeof(Pawn_HealthTracker), "CheckForStateChange", null)]
        public static class CheckForStateChange_Patch
        {
            public static FieldInfo pawn = typeof(Pawn_HealthTracker).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
            public static MethodBase MakeDowned = typeof(Pawn_HealthTracker).GetMethod("MakeDowned", BindingFlags.Instance | BindingFlags.NonPublic);
            public static MethodBase MakeUnDowned = typeof(Pawn_HealthTracker).GetMethod("MakeUnDowned", BindingFlags.Instance | BindingFlags.NonPublic);

            public static bool Prefix(Pawn_HealthTracker __instance, DamageInfo? dinfo, Hediff hediff) //CheckForStateChange_
            {
                Traverse traverse = Traverse.Create(__instance);
                Pawn pawn = (Pawn)CheckForStateChange_Patch.pawn.GetValue(__instance);

                bool flag = pawn != null && dinfo.HasValue && hediff != null;
                bool result;
                if (flag)
                {
                    if ((dinfo.Value.Def == TMDamageDefOf.DamageDefOf.TM_Whirlwind || dinfo.Value.Def == TMDamageDefOf.DamageDefOf.TM_GrapplingHook || dinfo.Value.Def == TMDamageDefOf.DamageDefOf.TM_DisablingShot || dinfo.Value.Def == TMDamageDefOf.DamageDefOf.TM_Tranquilizer) || TM_Calc.IsUndeadNotVamp(pawn))
                    {
                        bool flag2 = !__instance.Dead;
                        if (flag2)
                        {
                            bool flag3 = traverse.Method("ShouldBeDead", new object[0]).GetValue<bool>() && CheckForStateChange_Patch.pawn != null;
                            if (flag3)
                            {
                                bool flag4 = !pawn.Destroyed;
                                if (flag4)
                                {
                                    pawn.Kill(dinfo, hediff);
                                }
                                result = false;
                                return result;
                            }
                            bool flag5 = !__instance.Downed;
                            if (flag5)
                            {
                                bool flag6 = traverse.Method("ShouldBeDowned", new object[0]).GetValue<bool>() && CheckForStateChange_Patch.pawn != null;
                                if (flag6)
                                {
                                    float num = (!pawn.RaceProps.Animal) ? 0f : 0f;
                                    bool flag7 = !__instance.forceIncap && dinfo.HasValue && dinfo.Value.Def.ExternalViolenceFor(pawn) && (pawn.Faction == null || !pawn.Faction.IsPlayer) && !pawn.IsPrisonerOfColony && pawn.RaceProps.IsFlesh && Rand.Value < num;
                                    if (flag7)
                                    {
                                        pawn.Kill(dinfo, null);
                                        result = false;
                                        return result;
                                    }
                                    bool flagUndead = !__instance.forceIncap && dinfo.HasValue && dinfo.Value.Def.ExternalViolenceFor(pawn) && TM_Calc.IsUndeadNotVamp(pawn) && !pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_LichHD"));
                                    if (flagUndead)
                                    {
                                        TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Ghost"), pawn.DrawPos, pawn.Map, .65f, .05f, .05f, .4f, 0, Rand.Range(3, 4), Rand.Range(-15, 15), 0);
                                        pawn.Kill(dinfo, null);
                                        result = false;
                                        return result;
                                    }
                                    __instance.forceIncap = false;
                                    CheckForStateChange_Patch.MakeDowned.Invoke(__instance, new object[]
                                    {
                                    dinfo,
                                    hediff
                                    });
                                    result = false;
                                    return result;
                                }
                                else
                                {
                                    bool flag8 = !__instance.capacities.CapableOf(PawnCapacityDefOf.Manipulation);
                                    if (flag8)
                                    {
                                        bool flag9 = pawn.carryTracker != null && pawn.carryTracker.CarriedThing != null && pawn.jobs != null && pawn.CurJob != null;
                                        if (flag9)
                                        {
                                            pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
                                        }
                                        bool flag10 = pawn.equipment != null && pawn.equipment.Primary != null;
                                        if (flag10)
                                        {
                                            bool inContainerEnclosed = pawn.InContainerEnclosed;
                                            if (inContainerEnclosed)
                                            {
                                                pawn.equipment.TryTransferEquipmentToContainer(pawn.equipment.Primary, pawn.holdingOwner);
                                            }
                                            else
                                            {
                                                bool spawnedOrAnyParentSpawned = pawn.SpawnedOrAnyParentSpawned;
                                                if (spawnedOrAnyParentSpawned)
                                                {
                                                    ThingWithComps thingWithComps;
                                                    pawn.equipment.TryDropEquipment(pawn.equipment.Primary, out thingWithComps, pawn.PositionHeld, true);
                                                }
                                                else
                                                {
                                                    pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                bool flag11 = !traverse.Method("ShouldBeDowned", new object[0]).GetValue<bool>() && CheckForStateChange_Patch.pawn != null;
                                if (flag11)
                                {
                                    CheckForStateChange_Patch.MakeUnDowned.Invoke(__instance, null);
                                    result = false;
                                    return result;
                                }
                            }
                        }
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
                else
                {
                    result = true;
                }
                return result;
            }

            private static void Postfix(Pawn_HealthTracker __instance, DamageInfo? dinfo, Hediff hediff)
            {
                Traverse traverse = Traverse.Create(__instance);
                Pawn pawn = (Pawn)CheckForStateChange_Patch.pawn.GetValue(__instance);
                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();


                bool flag = pawn != null && dinfo.HasValue && hediff != null;
                if (flag)
                {
                    if (pawn != null && !pawn.IsColonist && pawn.RaceProps.Humanlike)
                    {

                        if (pawn.story.traits.HasTrait(TorannMagicDefOf.BloodMage) || pawn.story.traits.HasTrait(TorannMagicDefOf.Enchanter) || pawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Geomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Warlock) || pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus) || pawn.story.traits.HasTrait(TorannMagicDefOf.Druid) || pawn.story.traits.HasTrait(TorannMagicDefOf.Paladin) || pawn.story.traits.HasTrait(TorannMagicDefOf.Arcanist) || pawn.story.traits.HasTrait(TorannMagicDefOf.Summoner) || pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Lich) || pawn.story.traits.HasTrait(TorannMagicDefOf.InnerFire) || pawn.story.traits.HasTrait(TorannMagicDefOf.StormBorn) || pawn.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
                        {


                            if (pawn.Downed && !pawn.Dead && !pawn.IsPrisoner)
                            {
                                float chc = 0f;
                                if (settingsRef.AICasting)
                                {
                                    chc = .4f;
                                    if (settingsRef.AIHardMode)
                                    {
                                        chc = 1f;
                                    }
                                }
                                if (Rand.Chance(chc))
                                {
                                    IntVec3 curCell;
                                    Pawn victim = new Pawn();

                                    float radius = 2f;
                                    if (settingsRef.AIHardMode)
                                    {
                                        radius = settingsRef.deathExplosionRadius * 2;
                                    }
                                    else
                                    {
                                        radius = settingsRef.deathExplosionRadius;
                                    }

                                    if (pawn.Map == null || !pawn.Position.IsValid)
                                    {
                                        Log.Warning("Tried to do explosion in a null map.");
                                        return;
                                    }

                                    Faction faction = pawn.Faction;
                                    Pawn p = new Pawn();
                                    p = pawn;
                                    Map map = p.Map;
                                    GenExplosion.DoExplosion(p.Position, p.Map, 0f, DamageDefOf.Burn, p as Thing, 0, 0, SoundDefOf.Thunder_OnMap, null, null, null, null, 0f, 0, false, null, 0f, 0, 0.0f, false);
                                    Effecter deathEffect = TorannMagicDefOf.TM_DeathExplosion.Spawn();
                                    deathEffect.Trigger(new TargetInfo(p.Position, p.Map, false), new TargetInfo(p.Position, p.Map, false));
                                    deathEffect.Cleanup();

                                    IEnumerable<IntVec3> targets = GenRadial.RadialCellsAround(p.Position, radius, true);
                                    for (int i = 0; i < targets.Count(); i++)
                                    {
                                        curCell = targets.ToArray<IntVec3>()[i];
                                        if (curCell.InBounds(map) && curCell.IsValid)
                                        {
                                            victim = curCell.GetFirstPawn(map);
                                        }
                                        if (victim != null)
                                        {
                                            if (victim.Faction != faction || victim == pawn)
                                            {
                                                DamageInfo dinfo1;
                                                int amt = 0;
                                                if (settingsRef.deathExplosionMin > settingsRef.deathExplosionMax)
                                                {
                                                    amt = Mathf.RoundToInt(Rand.Range(settingsRef.deathExplosionMin, settingsRef.deathExplosionMin));
                                                }
                                                else
                                                {
                                                    amt = Mathf.RoundToInt(Rand.Range(settingsRef.deathExplosionMin, settingsRef.deathExplosionMax));
                                                }
                                                dinfo1 = new DamageInfo(DamageDefOf.Burn, amt, 0, (float)-1, p as Thing, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
                                                dinfo1.SetAllowDamagePropagation(false);
                                                victim.TakeDamage(dinfo1);
                                            }
                                        }
                                        victim = null;
                                        targets.GetEnumerator().MoveNext();
                                    }

                                    if (pawn != null && !pawn.Dead)
                                    {
                                        DamageInfo dinfo2;
                                        BodyPartRecord vitalPart = null;
                                        int amt = 30;
                                        IEnumerable<BodyPartRecord> partSearch = pawn.def.race.body.AllParts;
                                        vitalPart = partSearch.FirstOrDefault<BodyPartRecord>((BodyPartRecord x) => x.def.tags.Contains(BodyPartTagDefOf.ConsciousnessSource));
                                        dinfo2 = new DamageInfo(DamageDefOf.Burn, amt, 0, (float)-1, p as Thing, vitalPart, null, DamageInfo.SourceCategory.ThingOrUnknown);
                                        dinfo2.SetAllowDamagePropagation(false);
                                        pawn.TakeDamage(dinfo2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //[HarmonyPatch(typeof(DamageWorker), "Apply", null)]
        //public class DamageWorker_Patch
        //{
        //    public static bool Prefix(ref DamageInfo dinfo, Thing victim)
        //    {
        //        Log.Message("apply damage patch");
        //        Pawn pawn = victim as Pawn;
        //        Log.Message("pawn is " + pawn + " and is dead " + pawn.Dead);
        //        Log.Message("pawn is undead " + TM_Calc.IsUndead(pawn));
        //        Log.Message("dinfo def is " + dinfo.Def);

                
        //        return true;
        //    }
        //}

        [HarmonyPatch(typeof(Pawn_HealthTracker), "PreApplyDamage", null)]
        public class PreApplyDamage_Patch
        {
            public static FieldInfo pawn = typeof(Pawn_HealthTracker).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
            public static bool Prefix(Pawn __instance, ref DamageInfo dinfo, out bool absorbed)
            {
                Traverse traverse = Traverse.Create(__instance);
                Pawn pawn = (Pawn)PreApplyDamage_Patch.pawn.GetValue(__instance);
                if (dinfo.Def != null && pawn != null && !pawn.Downed)
                {
                    if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_HediffTimedInvulnerable")))
                    {
                        absorbed = true;
                        return false;
                    }
                    if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_ArtifactBlockHD) && Rand.Chance(.4f))
                    {
                        Thing instigator = dinfo.Instigator as Thing;
                        if (instigator != null)
                        {
                            if ((dinfo.Weapon != null && !dinfo.Def.isExplosive) || dinfo.WeaponBodyPartGroup != null)
                            {
                                Vector3 drawPos = pawn.DrawPos;
                                float drawAngle = (instigator.DrawPos - drawPos).AngleFlat();
                                drawPos.x += Mathf.Clamp(((instigator.DrawPos.x - drawPos.x) / 5f) + Rand.Range(-.1f, .1f), -.45f, .45f);
                                drawPos.z += Mathf.Clamp(((instigator.DrawPos.z - drawPos.z) / 5f) + Rand.Range(-.1f, .1f), -.45f, .45f);
                                TM_MoteMaker.ThrowSparkFlashMote(drawPos, pawn.Map, 1f);                                
                                TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_BracerBlock_NoFlash, drawPos, pawn.Map, .45f, .23f, 0f, .07f, 0, 0, 0, drawAngle);
                                SoundInfo info = SoundInfo.InMap(new TargetInfo(pawn.Position, pawn.Map, false), MaintenanceType.None);
                                TorannMagicDefOf.TM_MetalImpact.PlayOneShot(info);
                                dinfo.SetAmount(0);
                                absorbed = true;
                                return false;
                            }
                        }
                    }
                    if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_ArtifactDeflectHD) && Rand.Chance(.3f))
                    {
                        Thing instigator = dinfo.Instigator as Thing;
                        if (instigator != null)
                        {
                            if ((dinfo.Weapon != null && !dinfo.Def.isExplosive) || dinfo.WeaponBodyPartGroup != null)
                            {
                                Vector3 drawPos = pawn.DrawPos;
                                float drawAngle = (instigator.DrawPos - drawPos).AngleFlat();
                                drawPos.x += Mathf.Clamp(((instigator.DrawPos.x - drawPos.x) / 5f) + Rand.Range(-.1f, .1f), -.45f, .45f);
                                drawPos.z += Mathf.Clamp(((instigator.DrawPos.z - drawPos.z) / 5f) + Rand.Range(-.1f, .1f), -.45f, .45f);
                                TM_MoteMaker.ThrowSparkFlashMote(drawPos, pawn.Map, 1f);
                                TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_BracerBlock, drawPos, pawn.Map, .45f, .23f, 0f, .07f, 0, 0, 0, drawAngle);
                                SoundInfo info = SoundInfo.InMap(new TargetInfo(pawn.Position, pawn.Map, false), MaintenanceType.None);
                                TM_Action.DoReversalRandomTarget(dinfo, pawn, 0, 8f);
                                TorannMagicDefOf.TM_MetalImpact.PlayOneShot(info);
                                dinfo.SetAmount(0);
                                absorbed = true;
                                return false;
                            }
                        }
                    }
                    if ((pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_TechnoShieldHD) && dinfo.Amount <= 10) || (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_TechnoShieldHD_I) && dinfo.Amount <= 13) || (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_TechnoShieldHD_II) && dinfo.Amount <= 18) || (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_TechnoShieldHD_III) && dinfo.Amount <= 30))
                    {
                        Thing instigator = dinfo.Instigator as Thing;
                        if (instigator != null && dinfo.Weapon != null && dinfo.Weapon.IsRangedWeapon)
                        {
                            Vector3 drawPos = pawn.DrawPos;
                            drawPos.x += ((instigator.DrawPos.x - drawPos.x) / 20f) + Rand.Range(-.2f, .2f);
                            drawPos.z += ((instigator.DrawPos.z - drawPos.z) / 20f) + Rand.Range(-.2f, .2f);
                            TM_MoteMaker.ThrowSparkFlashMote(drawPos, pawn.Map, 2f);
                            TM_Action.DoReversal(dinfo, pawn);
                            TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_TechnoShield"), pawn.DrawPos, pawn.Map, .9f, .1f, 0f, .05f, Rand.Range(-500, 500), 0, 0, Rand.Range(0, 360));
                            HealthUtility.AdjustSeverity(pawn, TorannMagicDefOf.TM_TechnoShieldHD, -dinfo.Amount);
                            HealthUtility.AdjustSeverity(pawn, TorannMagicDefOf.TM_TechnoShieldHD_I, -dinfo.Amount);
                            HealthUtility.AdjustSeverity(pawn, TorannMagicDefOf.TM_TechnoShieldHD_II, -dinfo.Amount);
                            HealthUtility.AdjustSeverity(pawn, TorannMagicDefOf.TM_TechnoShieldHD_III, -dinfo.Amount);
                            dinfo.SetAmount(0);                                                                      
                            absorbed = true;
                            return false;

                        }
                    }
                    if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_StoneskinHD"), false))
                    {
                        HealthUtility.AdjustSeverity(pawn, HediffDef.Named("TM_StoneskinHD"), -1);
                        for (int m = 0; m < 4; m++)
                        {
                            Vector3 vectorOffset = pawn.DrawPos;
                            vectorOffset.x += (Rand.Range(-.3f, .3f));
                            vectorOffset.z += Rand.Range(-.3f, .3f);
                            TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_ThickDust"), vectorOffset, pawn.Map, Rand.Range(.15f, .35f), Rand.Range(.1f, .15f), 0, Rand.Range(.1f, .2f), Rand.Range(-20, 20), Rand.Range(.3f, .5f), Rand.Range(0, 360), Rand.Range(0, 360));
                        }
                        absorbed = true;
                        return false;
                    }
                    if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_BloodShieldHD"), false))
                    {
                        HealthUtility.AdjustSeverity(pawn, HediffDef.Named("TM_BloodShieldHD"), -dinfo.Amount);
                        for (int m = 0; m < 4; m++)
                        {
                            Effecter BloodShieldEffect = TorannMagicDefOf.TM_BloodShieldEffecter.Spawn();
                            BloodShieldEffect.Trigger(new TargetInfo(pawn.Position, pawn.Map, false), new TargetInfo(pawn.Position, pawn.Map, false));
                            BloodShieldEffect.Cleanup();
                        }
                        absorbed = true;
                        return false;
                    }
                    if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_BlurHD, false) && Rand.Chance(.2f) && !dinfo.Def.isExplosive)
                    {
                        Hediff blur = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_BlurHD);
                        blur.TryGetComp<HediffComp_Blur>().blurTick = Find.TickManager.TicksGame;
                        absorbed = true;
                        return false;
                    }
                    if (TM_Calc.HasHateHediff(pawn) && dinfo.Amount > 0)
                    {
                        int hatePwr = 0;
                        int hateVer = 0;
                        int hateEff = 0;

                        CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();
                        if (comp != null && comp.IsMightUser)
                        {
                           hatePwr = comp.MightData.MightPowerSkill_Shroud.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Shroud_pwr").level;
                           hateVer = comp.MightData.MightPowerSkill_Shroud.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Shroud_ver").level;
                           hateEff = comp.MightData.MightPowerSkill_Shroud.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Shroud_eff").level;
                        }

                        Hediff hediff = null;
                        for (int h = 0; h < pawn.health.hediffSet.hediffs.Count; h++)
                        {
                            if (pawn.health.hediffSet.hediffs[h].def.defName.Contains("TM_HateHD"))
                            {
                                hediff = pawn.health.hediffSet.hediffs[h];
                            }
                        }

                        HealthUtility.AdjustSeverity(pawn, hediff.def, (dinfo.Amount * (1 + (.1f * hateEff))));
                        if (hediff != null && hediff.Severity >= 20 && Rand.Chance(.1f * hateVer) && dinfo.Instigator != null && dinfo.Instigator is Pawn && (dinfo.Instigator.Position - pawn.Position).LengthHorizontal < 2)
                        {
                            TM_Action.DamageEntities(dinfo.Instigator, null, (dinfo.Amount * (1 + .2f * hatePwr)), TMDamageDefOf.DamageDefOf.TM_Spirit, pawn);
                            TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_SpiritRetaliation"), pawn.DrawPos, pawn.Map, Rand.Range(1f, 1.2f), Rand.Range(.1f, .15f), 0, Rand.Range(.1f, .2f), -600, 0, 0, Rand.Range(0, 360));
                            HealthUtility.AdjustSeverity(pawn, hediff.def, -(dinfo.Amount * (.8f - (.1f * hateEff))));
                        }
                                              
                    }
                    if(dinfo.Instigator != null && dinfo.Instigator is Pawn)
                    {
                        Pawn attacker = dinfo.Instigator as Pawn;
                        if(attacker != null && !attacker.Destroyed && !attacker.Downed && !attacker.Dead && attacker != pawn && TM_Calc.HasHateHediff(attacker) && dinfo.Weapon != null && dinfo.Weapon.IsMeleeWeapon)
                        {
                            CompAbilityUserMight comp = attacker.GetComp<CompAbilityUserMight>();
                            Hediff hediff = null;
                            for (int h = 0; h < attacker.health.hediffSet.hediffs.Count; h++)
                            {
                                if (attacker.health.hediffSet.hediffs[h].def.defName.Contains("TM_HateHD"))
                                {
                                    hediff = attacker.health.hediffSet.hediffs[h];
                                }
                            }
                            int lifestealPwr = comp.MightData.MightPowerSkill_LifeSteal.FirstOrDefault((MightPowerSkill x) => x.label == "TM_LifeSteal_pwr").level;
                            int lifestealEff = comp.MightData.MightPowerSkill_LifeSteal.FirstOrDefault((MightPowerSkill x) => x.label == "TM_LifeSteal_eff").level;
                            int lifestealVer = comp.MightData.MightPowerSkill_LifeSteal.FirstOrDefault((MightPowerSkill x) => x.label == "TM_LifeSteal_ver").level;

                            TM_Action.DoAction_HealPawn(attacker, attacker, 1, dinfo.Amount * (.25f + .05f * lifestealPwr));
                            TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Siphon"), attacker.DrawPos, attacker.Map, 1f, .1f, .15f, .5f, 600, 0, 0, Rand.Range(0, 360));
                            TM_MoteMaker.ThrowSiphonMote(attacker.DrawPos, attacker.Map, 1f);

                            if (hediff != null && lifestealEff > 0)
                            {
                                HealthUtility.AdjustSeverity(attacker, hediff.def, dinfo.Amount * (.25f + .05f * lifestealEff));
                                comp.Stamina.CurLevel += (dinfo.Amount * (float)(.1f * lifestealEff)) / 100;
                            }
                            if(hediff != null && lifestealVer > 0)
                            {
                                Pawn ally = TM_Calc.FindNearbyInjuredPawnOther(attacker, 3, 0);
                                if (ally != null)
                                {
                                    TM_Action.DoAction_HealPawn(attacker, ally, 1, dinfo.Amount * (.2f * lifestealVer));
                                    TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Siphon"), ally.DrawPos, ally.Map, 1f, .1f, .15f, .5f, 600, 0, 0, Rand.Range(0, 360));
                                }
                            }
                        }
                    }
                    //concept damage mitigation from psychic sensitivity - completely mitigates some damage types
                    //if (pawn.RaceProps.Humanlike && pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) < 1)
                    //{
                    //    if ((dinfo.Def.defName.Contains("TM_") || dinfo.Def.defName == "FrostRay" || dinfo.Def.defName == "Snowball" || dinfo.Def.defName == "Iceshard" || dinfo.Def.defName == "Firebolt") && Rand.Chance(1 - pawn.GetStatValue(StatDefOf.PsychicSensitivity, true)))
                    //    {
                    //        absorbed = true;
                    //        return false;
                    //    }
                    //}
                }
                absorbed = false;
                return true;
            }
        }

        [HarmonyPatch(typeof(Pawn_HealthTracker), "PostApplyDamage", null)]
        public static class PostApplyDamage_Patch
        {
            public static FieldInfo pawn = typeof(Pawn_HealthTracker).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);

            public static void Postfix(Pawn_HealthTracker __instance, DamageInfo dinfo)
            {
                Traverse traverse = Traverse.Create(__instance);
                Pawn pawn = (Pawn)PostApplyDamage_Patch.pawn.GetValue(__instance);
                if (dinfo.Def != null)
                {                    
                    if (dinfo.Instigator != null && pawn != null && dinfo.Instigator != pawn && !pawn.Destroyed && !pawn.Dead && pawn.Map != null)
                    {
                        Pawn instigator = dinfo.Instigator as Pawn;
                        if (instigator != null && dinfo.Def != TMDamageDefOf.DamageDefOf.TM_Shrapnel)
                        {
                            if (instigator.health.hediffSet.HasHediff(TorannMagicDefOf.TM_BowTrainingHD))
                            {
                                if (instigator.equipment.Primary != null)
                                {
                                    Thing wpn = instigator.equipment.Primary;
                                    if (wpn.def.IsRangedWeapon)
                                    {
                                        if (wpn.def.Verbs.FirstOrDefault<VerbProperties>().defaultProjectile.projectile.damageDef.defName == "Arrow" || wpn.def.defName.Contains("Bow") || wpn.def.defName.Contains("bow") || wpn.def.Verbs.FirstOrDefault<VerbProperties>().defaultProjectile.projectile.damageDef.defName.Contains("Arrow") || wpn.def.Verbs.FirstOrDefault<VerbProperties>().defaultProjectile.projectile.damageDef.defName.Contains("arrow"))
                                        {
                                            Hediff hediff = instigator.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_BowTrainingHD);
                                            DamageInfo dinfo2;
                                            float amt;
                                            if (hediff.Severity >= 1)
                                            {
                                                amt = dinfo.Amount * .4f;
                                            }
                                            else if (hediff.Severity >= 2)
                                            {
                                                amt = dinfo.Amount * .6f;
                                            }
                                            else if (hediff.Severity >= 3)
                                            {
                                                amt = dinfo.Amount * .8f;
                                            }
                                            else
                                            {
                                                amt = dinfo.Amount * .2f;
                                            }
                                            dinfo2 = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_Shrapnel, (int)amt, 0, (float)-1, instigator, dinfo.HitPart, null, DamageInfo.SourceCategory.ThingOrUnknown);
                                            dinfo2.SetAllowDamagePropagation(false);
                                            pawn.TakeDamage(dinfo2);
                                        }
                                    }
                                }
                            }
                        }

                        if (instigator != null && dinfo.Def != TMDamageDefOf.DamageDefOf.TM_ArcaneSpectre)
                        {
                            if (instigator.health.hediffSet.HasHediff(TorannMagicDefOf.TM_HediffEnchantment_arcaneSpectre) && Rand.Chance(.5f))
                            {
                                DamageInfo dinfo2;
                                float amt;
                                amt = dinfo.Amount * .2f;
                                dinfo2 = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_ArcaneSpectre, (int)amt, 0, (float)-1, instigator, dinfo.HitPart, null, DamageInfo.SourceCategory.ThingOrUnknown);
                                dinfo2.SetAllowDamagePropagation(false);
                                pawn.TakeDamage(dinfo2);
                                Vector3 displayVec = pawn.Position.ToVector3Shifted();
                                displayVec.x += Rand.Range(-.2f, .2f);
                                displayVec.z += Rand.Range(-.2f, .2f);
                                TM_MoteMaker.ThrowArcaneDaggers(displayVec, pawn.Map, .7f);
                            }
                        }

                        if (TM_Calc.IsUndead(pawn))
                        {
                            if (dinfo.Def.armorCategory != null && dinfo.Def.armorCategory.defName == "Light" && Rand.Chance(.25f))
                            {
                                dinfo.SetAmount(dinfo.Amount * .7f);
                                pawn.TakeDamage(dinfo);
                            }
                        }

                        if (instigator != null && dinfo.Def != TMDamageDefOf.DamageDefOf.TM_Cleave)
                        {
                            if (instigator.RaceProps.Humanlike && instigator.story != null && instigator.story.traits.HasTrait(TorannMagicDefOf.Gladiator))
                            {
                                if (instigator.equipment.Primary != null && !instigator.equipment.Primary.def.IsRangedWeapon)
                                {
                                    float cleaveChance = Mathf.Min(instigator.equipment.Primary.def.BaseMass * .3f, .65f);
                                    CompAbilityUserMight comp = instigator.GetComp<CompAbilityUserMight>();
                                    if (Rand.Chance(cleaveChance) && comp.Stamina.CurLevel >= comp.ActualStaminaCost(TorannMagicDefOf.TM_Cleave))
                                    {
                                        MightPowerSkill pwr = comp.MightData.MightPowerSkill_Cleave.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Cleave_pwr");
                                        MightPowerSkill str = comp.MightData.MightPowerSkill_global_strength.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_strength_pwr");
                                        MightPowerSkill ver = comp.MightData.MightPowerSkill_Cleave.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Cleave_ver");
                                        int dmgNum = Mathf.RoundToInt(dinfo.Amount * (.35f + (.05f * pwr.level)));
                                        DamageInfo dinfo2 = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_Cleave, dmgNum, 0, (float)-1, instigator, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
                                        Verb_Cleave.ApplyCleaveDamage(dinfo2, instigator, pawn, pawn.Map, ver.level);
                                        comp.Stamina.CurLevel -= comp.ActualStaminaCost(TorannMagicDefOf.TM_Cleave);
                                    }
                                }
                            }
                        }

                        if (instigator != null && instigator.health.hediffSet.HasHediff(HediffDef.Named("TM_PsionicHD"), false))
                        {
                            if (instigator.equipment.Primary == null && dinfo.Def != TMDamageDefOf.DamageDefOf.TM_PsionicInjury && dinfo.Def != DamageDefOf.Stun)
                            {
                                CompAbilityUserMight comp = instigator.GetComp<CompAbilityUserMight>();
                                MightPowerSkill pwr = comp.MightData.MightPowerSkill_PsionicAugmentation.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicAugmentation_pwr");
                                float dmgNum = dinfo.Amount;
                                float pawnDPS = instigator.GetStatValue(StatDefOf.MeleeDPS, false);
                                float psiEnergy = instigator.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_PsionicHD"), false).Severity;
                                if (psiEnergy > 20f && Rand.Chance(.3f + (.05f * pwr.level)) && !pawn.Downed)
                                {
                                    DamageInfo dinfo2 = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_PsionicInjury, (dmgNum + pawnDPS) + 2 * pwr.level, dinfo.ArmorPenetrationInt, dinfo.Angle, instigator, dinfo.HitPart, dinfo.Weapon, dinfo.Category, dinfo.intendedTargetInt);
                                    TM_MoteMaker.MakePowerBeamMotePsionic(pawn.DrawPos.ToIntVec3(), pawn.Map, 2.5f, 2f, .7f, .1f, .6f);
                                    pawn.TakeDamage(dinfo2);
                                    HealthUtility.AdjustSeverity(instigator, HediffDef.Named("TM_PsionicHD"), -2f);
                                    comp.Stamina.CurLevel -= .02f;
                                    comp.MightUserXP += Rand.Range(2, 4);
                                    if (psiEnergy > 60f && !pawn.Dead && Rand.Chance(.2f + (.03f * pwr.level)))
                                    {
                                        for (int i = 0; i < 6; i++)
                                        {
                                            float moteDirection = Rand.Range(0, 360);
                                            TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Psi"), instigator.DrawPos, instigator.Map, Rand.Range(.3f, .5f), 0.25f, .05f, .1f, 0, Rand.Range(6, 8), moteDirection, moteDirection);
                                        }
                                        Vector3 heading = (pawn.Position - instigator.Position).ToVector3();
                                        float distance = heading.magnitude;
                                        Vector3 direction = heading / distance;
                                        IntVec3 destinationCell = pawn.Position + (direction * (Rand.Range(5, 8) + (2 * pwr.level))).ToIntVec3();
                                        FlyingObject_Spinning flyingObject = (FlyingObject_Spinning)GenSpawn.Spawn(ThingDef.Named("FlyingObject_Spinning"), pawn.Position, pawn.Map);
                                        flyingObject.speed = 35;
                                        flyingObject.Launch(instigator, destinationCell, pawn);
                                        HealthUtility.AdjustSeverity(instigator, HediffDef.Named("TM_PsionicHD"), -2f);
                                        comp.Stamina.CurLevel -= .02f;
                                        comp.MightUserXP += Rand.Range(3, 5);
                                    }
                                    else if (psiEnergy > 40f && !pawn.Dead && Rand.Chance(.4f + (.05f * pwr.level)))
                                    {
                                        DamageInfo dinfo3 = new DamageInfo(DamageDefOf.Stun, dmgNum / 2, dinfo.ArmorPenetrationInt, dinfo.Angle, instigator, dinfo.HitPart, dinfo.Weapon, dinfo.Category, dinfo.intendedTargetInt);
                                        pawn.TakeDamage(dinfo3);
                                        HealthUtility.AdjustSeverity(instigator, HediffDef.Named("TM_PsionicHD"), -2f);
                                        comp.Stamina.CurLevel -= .01f;
                                        comp.MightUserXP += Rand.Range(2, 3);
                                    }
                                }
                            }
                        }

                        if (instigator != null)
                        {
                            if (instigator.health.hediffSet.HasHediff(TorannMagicDefOf.TM_WeaponEnchantment_FireHD) && dinfo.Def != TMDamageDefOf.DamageDefOf.TM_Enchanted_Fire && Rand.Chance(.5f))
                            {
                                float sev = instigator.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_WeaponEnchantment_FireHD).Severity;
                                DamageInfo dinfo3 = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_Enchanted_Fire, Rand.Range(1f + sev, 5f + sev), 1, -1, instigator, dinfo.HitPart, dinfo.Weapon, dinfo.Category, dinfo.intendedTargetInt);
                                pawn.TakeDamage(dinfo3);
                            }
                            if (instigator.health.hediffSet.HasHediff(TorannMagicDefOf.TM_WeaponEnchantment_IceHD) && dinfo.Def != TMDamageDefOf.DamageDefOf.TM_Enchanted_Ice && Rand.Chance(.4f))
                            {
                                float sev = instigator.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_WeaponEnchantment_IceHD).Severity;
                                DamageInfo dinfo3 = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_Enchanted_Ice, Mathf.RoundToInt(Rand.Range(3f + sev, 5f + sev) / 2), 1, -1, instigator, dinfo.HitPart, dinfo.Weapon, dinfo.Category, dinfo.intendedTargetInt);
                                pawn.TakeDamage(dinfo3);
                            }
                            if (instigator.health.hediffSet.HasHediff(TorannMagicDefOf.TM_WeaponEnchantment_LitHD) && dinfo.Def != DamageDefOf.Stun && Rand.Chance(.3f))
                            {
                                float sev = instigator.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_WeaponEnchantment_LitHD).Severity;
                                DamageInfo dinfo3 = new DamageInfo(DamageDefOf.Stun, Rand.Range(1f + (.5f * sev), 3f + (.5f * sev)), 1, -1, instigator, dinfo.HitPart, dinfo.Weapon, dinfo.Category, dinfo.intendedTargetInt);
                                pawn.TakeDamage(dinfo3);
                            }
                            if (instigator.health.hediffSet.HasHediff(TorannMagicDefOf.TM_WeaponEnchantment_DarkHD))
                            {
                                float sev = instigator.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_WeaponEnchantment_DarkHD).Severity;
                                if (Rand.Chance(.3f + (.1f * sev)))
                                {
                                    HealthUtility.AdjustSeverity(pawn, TorannMagicDefOf.TM_Blind, Rand.Range(.05f, .2f));
                                }
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Recipe_Surgery), "CheckSurgeryFail", null)]
        public static class CheckSurgeryFail_Base_Patch
        {
            public static bool Prefix(Recipe_Surgery __instance, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, ref bool __result)
            {
                if (patient.health.hediffSet.HasHediff(HediffDef.Named("TM_StoneskinHD")))
                {
                    Hediff hediff = patient.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_StoneskinHD"), false);
                    patient.health.RemoveHediff(hediff);
                }

                if (patient.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD")) || patient.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadAnimalHD")))
                {
                    Messages.Message("Something went horribly wrong while trying to perform a surgery on " + patient.LabelShort + ", perhaps it's best to leave the bodies of the living dead alone.", MessageTypeDefOf.NegativeHealthEvent);
                    GenExplosion.DoExplosion(surgeon.Position, surgeon.Map, 2f, TMDamageDefOf.DamageDefOf.TM_CorpseExplosion, patient, Rand.Range(6, 12), 10, TMDamageDefOf.DamageDefOf.TM_CorpseExplosion.soundExplosion, null, null, null, null, 0, 0, false, null, 0, 0, 0, false);
                    __result = true;
                    return false;
                }

                return true;

            }
        }

        [HarmonyPatch(typeof(Verb), "TryFindShootLineFromTo", null)]
        public static class TryFindShootLineFromTo_Base_Patch
        {
            public static bool Prefix(Verb __instance, IntVec3 root, LocalTargetInfo targ, out ShootLine resultingLine, ref bool __result)
            {
                if (__instance.verbProps.IsMeleeAttack)
                {
                    resultingLine = new ShootLine(root, targ.Cell);
                    __result = ReachabilityImmediate.CanReachImmediate(root, targ, __instance.caster.Map, PathEndMode.Touch, null);
                    return false;
                }
                if (__instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_Blink" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_BLOS" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_Summon" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_SootheAnimal" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Effect_EyeOfTheStorm" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_PhaseStrike" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Effect_Flight" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_Regenerate" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_SpellMending" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_CauterizeWound" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_Transpose" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_Disguise" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_SoulBond" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_SummonDemon" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_EarthSprites" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_Overdrive" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_Heal" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_BlankMind" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_AdvancedHeal")
                {
                    //Ignores line of sight
                    //                    
                    if (__instance.CasterPawn.RaceProps.Humanlike)
                    {
                        Pawn pawn = __instance.CasterPawn;
                        if (pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                        {
                            CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();
                            MightPowerSkill ver = comp.MightData.MightPowerSkill_Transpose.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Transpose_ver");
                            if (ver.level < 3)
                            {
                                __result = true;
                                resultingLine = default(ShootLine);
                                return true;
                            }
                        }
                    }

                    resultingLine = new ShootLine(root, targ.Cell);
                    __result = true;
                    return false;
                }
                resultingLine = default(ShootLine);
                __result = true;
                return true;
            }
        }

        [HarmonyPatch(typeof(CastPositionFinder), "TryFindCastPosition", null)]
        public static class TryFindCastPosition_Base_Patch
        {

            private static bool Prefix(CastPositionRequest newReq, out IntVec3 dest, ref IntVec3 __result)
            {
                CastPositionRequest req = newReq;
                IntVec3 casterLoc = req.caster.Position;
                IntVec3 targetLoc = req.target.Position;
                Verb verb = req.verb;
                dest = IntVec3.Invalid;
                bool isTMAbility = verb.verbProps.verbClass.ToString().Contains("TorannMagic") || verb.verbProps.verbClass.ToString().Contains("AbilityUser");


                if (verb.CanHitTargetFrom(casterLoc, req.target) && (req.caster.Position - req.target.Position).LengthHorizontal < verb.verbProps.range && isTMAbility)
                {
                    //If in range and in los, cast immediately
                    dest = casterLoc;
                    __result = dest;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(Pawn_SkillTracker), "Learn", null)]
        public static class Pawn_SkillTracker_Base_Patch
        {
            public static FieldInfo pawn = typeof(Pawn_SkillTracker).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
            private static bool Prefix(Pawn_SkillTracker __instance)
            {
                Traverse traverse = Traverse.Create(__instance);
                Pawn pawn = (Pawn)Pawn_SkillTracker_Base_Patch.pawn.GetValue(__instance);
                if (pawn != null)
                {
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.Undead))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(SkillRecord), "Learn", null)]
        public static class SkillRecord_Patch
        {
            public static FieldInfo pawn = typeof(SkillRecord).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
            private static bool Prefix(SkillRecord __instance)
            {
                Traverse traverse = Traverse.Create(__instance);
                Pawn pawn = (Pawn)SkillRecord_Patch.pawn.GetValue(__instance);

                if (pawn != null)
                {
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.Undead))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(StockGenerator_Animals), "HandlesThingDef", null)]
        public static class StockGenerator_Animals_Patch
        {
            private static bool Prefix(ThingDef thingDef, ref bool __result)
            {
                if (thingDef.thingClass.ToString() == "TorannMagic.TMPawnSummoned")
                {
                    __result = false;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(FertilityGrid), "CalculateFertilityAt", null)]
        public static class FertilityGrid_Patch
        {
            public static FieldInfo map = typeof(FertilityGrid).GetField("map", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);

            private static void Postfix(FertilityGrid __instance, IntVec3 loc, ref float __result)
            {
                if (ModOptions.Constants.GetGrowthCells().Count > 0)
                {
                    List<IntVec3> growthCells = ModOptions.Constants.GetGrowthCells();
                    for (int i = 0; i < growthCells.Count; i++)
                    {
                        if (loc == growthCells[i])
                        {
                            Traverse traverse = Traverse.Create(__instance);
                            Map map = (Map)FertilityGrid_Patch.map.GetValue(__instance);
                            __result *= 2f;
                            if (Rand.Chance(.6f) && (ModOptions.Constants.GetLastGrowthMoteTick() + 5) < Find.TickManager.TicksGame)
                            {
                                TM_MoteMaker.ThrowTwinkle(growthCells[i].ToVector3Shifted(), map, Rand.Range(.3f, .7f), Rand.Range(100, 300), Rand.Range(.5f, 1.5f), Rand.Range(.1f, .5f), .05f, Rand.Range(.8f, 1.8f));
                                ModOptions.Constants.SetLastGrowthMoteTick(Find.TickManager.TicksGame);
                            }

                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(AbilityWorker), "TargetAbilityFor", null)]
        public static class AbilityWorker_TargetAbilityFor_Patch
        {
            public static bool Prefix(AbilityAIDef abilityDef, Pawn pawn, ref LocalTargetInfo __result)
            {
                if (pawn.story.traits.HasTrait(TorannMagicDefOf.DeathKnight) || pawn.story.traits.HasTrait(TorannMagicDefOf.BloodMage) || pawn.story.traits.HasTrait(TorannMagicDefOf.Enchanter) || pawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Geomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic) || pawn.story.traits.HasTrait(TorannMagicDefOf.Warlock) || pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus) || pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless) || pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) || pawn.story.traits.HasTrait(TorannMagicDefOf.Priest) || pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator) || pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper) || pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Lich) || pawn.story.traits.HasTrait(TorannMagicDefOf.InnerFire) || pawn.story.traits.HasTrait(TorannMagicDefOf.StormBorn) || pawn.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
                {
                    bool usedOnCaster = abilityDef.usedOnCaster;
                    if (usedOnCaster)
                    {
                        __result = pawn;
                    }
                    else
                    {
                        bool canTargetAlly = abilityDef.canTargetAlly;
                        if (canTargetAlly)
                        {
                            __result = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), abilityDef.maxRange, (Thing thing) => AbilityUtility.AreAllies(pawn, thing), null, 0, -1, false, RegionType.Set_Passable, false);
                        }
                        else
                        {

                            Pawn pawn2 = pawn.mindState.enemyTarget as Pawn;
                            Building bldg = pawn.mindState.enemyTarget as Building;
                            Corpse corpse = pawn.mindState.enemyTarget as Corpse;
                            bool flag = pawn.mindState.enemyTarget != null && pawn2 != null;
                            bool flag1 = pawn.mindState.enemyTarget != null && bldg != null;
                            bool flag11 = pawn.mindState.enemyTarget != null && corpse != null;
                            if (flag)
                            {
                                bool flag2 = !pawn2.Dead;
                                if (flag2)
                                {
                                    __result = pawn.mindState.enemyTarget;
                                    return false;
                                }
                            }
                            else if (flag1)
                            {
                                bool flag2 = !bldg.Destroyed;
                                if (flag2)
                                {
                                    __result = pawn.mindState.enemyTarget;
                                    return false;
                                }
                            }
                            else if (flag11)
                            {
                                bool flag2 = !corpse.IsNotFresh();
                                if (flag2)
                                {
                                    __result = pawn.mindState.enemyTarget;
                                    return false;
                                }
                            }
                            else
                            {
                                bool flag3 = pawn.mindState.enemyTarget != null && !(pawn.mindState.enemyTarget is Corpse);
                                if (flag3)
                                {
                                    __result = pawn.mindState.enemyTarget;
                                    return false;
                                }
                            }
                            __result = null;
                        }
                    }
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }

        [HarmonyPatch(typeof(AbilityWorker), "CanPawnUseThisAbility", null)]
        public static class AbilityWorker_CanPawnUseThisAbility_Patch
        {
            public static bool Prefix(AbilityAIDef abilityDef, Pawn pawn, LocalTargetInfo target, ref bool __result)
            {
                if (pawn.story.traits.HasTrait(TorannMagicDefOf.DeathKnight) || pawn.story.traits.HasTrait(TorannMagicDefOf.BloodMage) || pawn.story.traits.HasTrait(TorannMagicDefOf.Enchanter) || pawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Geomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic) || pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus) || pawn.story.traits.HasTrait(TorannMagicDefOf.Warlock) || pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless) || pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) || pawn.story.traits.HasTrait(TorannMagicDefOf.Priest) || pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator) || pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper) || pawn.story.traits.HasTrait(TorannMagicDefOf.Druid) || pawn.story.traits.HasTrait(TorannMagicDefOf.Paladin) || pawn.story.traits.HasTrait(TorannMagicDefOf.Arcanist) || pawn.story.traits.HasTrait(TorannMagicDefOf.Summoner) || pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Lich) || pawn.story.traits.HasTrait(TorannMagicDefOf.InnerFire) || pawn.story.traits.HasTrait(TorannMagicDefOf.StormBorn) || pawn.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
                {
                    ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                    if (!settingsRef.AICasting)
                    {
                        __result = false;
                        return false;
                    }
                    bool hasThing = target.HasThing;
                    if (hasThing)
                    {
                        Pawn pawn2 = target.Thing as Pawn;
                        if (pawn2 != null)
                        {
                            bool flag = !abilityDef.canTargetAlly;
                            if (flag)
                            {
                                __result = !pawn2.Downed;
                                return false;
                            }
                        }
                        Building bldg2 = target.Thing as Building;
                        if (bldg2 != null)
                        {
                            if (pawn.story.traits.HasTrait(TorannMagicDefOf.DeathKnight) || pawn.story.traits.HasTrait(TorannMagicDefOf.BloodMage) || pawn.story.traits.HasTrait(TorannMagicDefOf.Enchanter) || pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) || pawn.story.traits.HasTrait(TorannMagicDefOf.Priest) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Bard) || pawn.story.traits.HasTrait(TorannMagicDefOf.Geomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless) || pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator) || pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Druid) || pawn.story.traits.HasTrait(TorannMagicDefOf.Summoner))
                            {
                                __result = false;
                                return false;
                            }
                            __result = !bldg2.Destroyed;
                            return false;
                        }
                        Corpse corpse2 = target.Thing as Corpse;
                        if (corpse2 != null)
                        {
                            __result = true;//!corpse2.IsNotFresh();
                            return false;
                        }
                    }
                    __result = true;
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        [HarmonyPatch(typeof(WorkGiver_Researcher), "ShouldSkip", null)]
        public static class WorkGiver_Researcher_Patch
        {
            private static bool Prefix(Pawn pawn, ref bool __result)
            {
                if (pawn.story.traits.HasTrait(TorannMagicDefOf.Undead))
                {
                    __result = true;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(WorkGiver_Tend), "HasJobOnThing", null)]
        public static class WorkGiver_Tend_Patch
        {
            private static void Postfix(Pawn pawn, ref bool __result)
            {
                if (pawn.story.traits.HasTrait(TorannMagicDefOf.Undead))
                {
                    __result = false;
                }
            }
        }

        [HarmonyPatch(typeof(JobGiver_SocialFighting), "TryGiveJob", null)]
        public static class JobGiver_SocialFighting_Patch
        {
            private static void Postfix(Pawn pawn, ref Job __result)
            {
                if (pawn.story.traits.HasTrait(TorannMagicDefOf.Undead))
                {
                    __result = null;
                }
            }
        }

        [HarmonyPatch(typeof(ShotReport), "HitReportFor", null)]
        public static class ShotReport_Patch
        {
            private static bool Prefix(Thing caster, Verb verb, LocalTargetInfo target, ref ShotReport __result)
            {
                if (verb.verbProps.verbClass.ToString() == "TorannMagic.Verb_SB" || verb.verbProps.verbClass.ToString() == "TorannMagic.Verb_BLOS")
                {
                    ShotReport result = default(ShotReport);
                    __result = result;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(LordToil_Siege), "CanBeBuilder", null)]
        public static class CanBeBuilder_Patch
        {
            private static bool Prefix(Pawn p, ref bool __result)
            {
                if (p.def.thingClass.ToString() == "TorannMagic.TMPawnSummoned")
                {
                    __result = false;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(LordToil_Siege), "Notify_PawnLost", null)]
        public static class Notify_PawnLost_Patch
        {
            private static bool Prefix(Pawn victim)
            {
                if (victim.def.thingClass.ToString() == "TorannMagic.TMPawnSummoned")
                {
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(AbilityAIDef), "CanPawnUseThisAbility", null)]
        public static class CanPawnUseThisAbility_Patch
        {
            private static bool Prefix(AbilityAIDef __instance, Pawn caster, LocalTargetInfo target, ref bool __result)
            {
                bool flag = __instance.appliedHediffs.Count > 0 && __instance.appliedHediffs.Any((HediffDef hediffDef) => caster.health.hediffSet.HasHediff(hediffDef, false));
                //bool result;
                if (flag)
                {
                    __result = false;
                }
                else
                {
                    bool flag2 = !__instance.Worker.CanPawnUseThisAbility(__instance, caster, target);
                    if (flag2)
                    {
                        __result = false;
                    }
                    else
                    {
                        bool flag3 = !__instance.needEnemyTarget;
                        if (flag3)
                        {
                            __result = true;
                        }
                        else
                        {
                            bool flag4 = !__instance.usedOnCaster && target.IsValid;
                            if (flag4)
                            {
                                float num = Math.Abs(caster.Position.DistanceTo(target.Cell));
                                bool flag5 = num < __instance.minRange || num > __instance.maxRange;
                                if (flag5)
                                {
                                    __result = false;
                                    return false;
                                }
                                bool flag6 = __instance.needSeeingTarget && !AbilityUtility.LineOfSightLocalTarget(caster, target, true, null);
                                if (flag6)
                                {
                                    __result = false;
                                    return false;
                                }
                            }
                            //Log.Message("caster " + caster.LabelShort + " attempting to case " + __instance.ability.defName + " on target " + target.Thing.LabelShort);
                            if (__instance.ability.defName == "TM_ArrowStorm" && !caster.equipment.Primary.def.weaponTags.Contains("Neolithic"))
                            {
                                __result = false;
                                return false;
                            }
                            if (__instance.ability.defName == "TM_DisablingShot" || __instance.ability.defName == "TM_Headshot" && caster.equipment.Primary.def.weaponTags.Contains("Neolithic"))
                            {
                                __result = false;
                                return false;
                            }

                            if (target.IsValid && !target.Thing.Destroyed && target.Thing.Map == caster.Map && target.Thing.Spawned)
                            {
                                Pawn targetPawn = target.Thing as Pawn;
                                if (targetPawn != null && targetPawn.Dead)
                                {
                                    __result = false;
                                    return false;
                                }
                                else
                                {
                                    __result = true;
                                }
                            }
                            else
                            {
                                __result = false;
                            }
                        }
                    }
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(PawnGenerator), "GenerateTraits", null)]
        public static class PawnGenerator_Patch
        {
            private static void Postfix(Pawn pawn)
            {
                List<TraitDef> allTraits = DefDatabase<TraitDef>.AllDefsListForReading;
                List<Trait> pawnTraits = pawn.story.traits.allTraits;
                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                bool flag = pawnTraits != null;
                bool anyFightersEnabled = false;
                bool anyMagesEnabled = false;
                if (settingsRef.Gladiator || settingsRef.Bladedancer || settingsRef.Ranger || settingsRef.Sniper || settingsRef.Faceless || settingsRef.DeathKnight || settingsRef.Psionic)
                {
                    anyFightersEnabled = true;
                }
                if (settingsRef.Arcanist || settingsRef.FireMage || settingsRef.IceMage || settingsRef.LitMage || settingsRef.Druid || settingsRef.Paladin || settingsRef.Summoner || settingsRef.Priest || settingsRef.Necromancer || settingsRef.Bard || settingsRef.Demonkin || settingsRef.Geomancer || settingsRef.Technomancer || settingsRef.BloodMage || settingsRef.Enchanter)
                {
                    anyMagesEnabled = true;
                }
                if (flag)
                {
                    if (ModCheck.Validate.AlienHumanoidRaces.IsInitialized())
                    {
                        if (Rand.Chance(((settingsRef.baseFighterChance * 4) + (settingsRef.baseMageChance * 4) + (7 * settingsRef.advFighterChance) + (15 * settingsRef.advMageChance)) / (allTraits.Count - 22)))
                        {
                            if (pawnTraits.Count > 0)
                            {
                                pawnTraits.Remove(pawnTraits[pawnTraits.Count - 1]);
                            }
                            float rnd = Rand.Range(0, 4 * (settingsRef.baseFighterChance + settingsRef.baseMageChance) + (7 * settingsRef.advFighterChance) + (15 * settingsRef.advMageChance));
                            if (rnd < (4 * settingsRef.baseMageChance) && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Gifted, 2)) && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Gifted.ToString()))
                            {
                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Gifted, 2, false));
                            }
                            else if (rnd >= 4 * settingsRef.baseMageChance && rnd < (4 * (settingsRef.baseFighterChance + settingsRef.baseMageChance)) && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.PhysicalProdigy, 2))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.PhysicalProdigy.ToString()))
                            {
                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.PhysicalProdigy, 2, false));
                            }
                            else if (rnd >= (4 * (settingsRef.baseFighterChance + settingsRef.baseMageChance)) && rnd < (4 * (settingsRef.baseFighterChance + settingsRef.baseMageChance) + (7 * settingsRef.advFighterChance)))
                            {
                                if (anyFightersEnabled)
                                {
                                    int rndF = Rand.RangeInclusive(1, 7);
                                    switch (rndF)
                                    {
                                        case 1:
                                            //Gladiator:;
                                            if (settingsRef.Gladiator && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Gladiator, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Gladiator.defName.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Gladiator, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Sniper;
                                            //}
                                            break;
                                        case 2:
                                            //Sniper:;
                                            if (settingsRef.Sniper && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_Sniper, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.TM_Sniper.defName.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Sniper, 0, false));
                                            }
                                            //else
                                            //{
                                            //    goto Bladedancer;
                                            //}
                                            break;
                                        case 3:
                                        Bladedancer:;
                                            if (settingsRef.Bladedancer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Bladedancer, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Bladedancer.defName.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Bladedancer, 0, false));
                                            }
                                            //else
                                            //{
                                            //    goto Ranger;
                                            //}
                                            break;
                                        case 4:
                                        Ranger:;
                                            if (settingsRef.Ranger && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Ranger, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Ranger.defName.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Ranger, 0, false));
                                            }
                                            //else
                                            //{
                                            //    goto Faceless;
                                            //}
                                            break;
                                        case 5:
                                        Faceless:;
                                            if (settingsRef.Faceless && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Faceless, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Faceless.defName.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Faceless, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Psionic;
                                            //}
                                            break;
                                        case 6:
                                        Psionic:;
                                            if (settingsRef.Psionic && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_Psionic, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.TM_Psionic.defName.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Psionic, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto DeathKnight;
                                            //}
                                            break;
                                        case 7:
                                        DeathKnight:;
                                            if (settingsRef.DeathKnight && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.DeathKnight, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.DeathKnight.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.DeathKnight, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Gladiator;
                                            //}
                                            break;
                                    }
                                }
                                else
                                {
                                    goto TraitEnd;
                                }
                            }
                            else
                            {
                                if (anyMagesEnabled)
                                {
                                    int rndM = Rand.RangeInclusive(1, 16);
                                    switch (rndM)
                                    {
                                        case 1:
                                        FireMage:;
                                            if (settingsRef.FireMage && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.InnerFire, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.InnerFire.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.InnerFire, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto IceMage;
                                            //}
                                            break;
                                        case 2:
                                        IceMage:;
                                            if (settingsRef.IceMage && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.HeartOfFrost, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.HeartOfFrost.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.HeartOfFrost, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto LitMage;
                                            //}
                                            break;
                                        case 3:
                                        LitMage:;
                                            if (settingsRef.LitMage && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.StormBorn, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.StormBorn.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.StormBorn, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Arcanist;
                                            //}
                                            break;
                                        case 4:
                                        Arcanist:;
                                            if (settingsRef.Arcanist && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Arcanist, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Arcanist.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Arcanist, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Druid;
                                            //}
                                            break;
                                        case 5:
                                        Druid:;
                                            if (settingsRef.Druid && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Druid, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Druid.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Druid, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Paladin;
                                            //}
                                            break;
                                        case 6:
                                        Paladin:;
                                            if (settingsRef.Paladin && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Paladin, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Paladin.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Paladin, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Summoner;
                                            //}
                                            break;
                                        case 7:
                                        Summoner:;
                                            if (settingsRef.Summoner && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Summoner, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Summoner.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Summoner, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Necromancer;
                                            //}
                                            break;
                                        case 8:
                                        Necromancer:;
                                            if (settingsRef.Necromancer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Necromancer, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Necromancer.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Necromancer, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Priest;
                                            //}
                                            break;
                                        case 9:
                                        Priest:;
                                            if (settingsRef.Priest && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Priest, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Priest.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Priest, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Demonkin;
                                            //}
                                            break;
                                        case 10:
                                        Demonkin:;
                                            if (settingsRef.Demonkin && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Warlock, 4)) && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Succubus, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Succubus.ToString())   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Warlock.ToString()))
                                            {
                                                if (pawn.gender != Gender.Female)
                                                {
                                                    pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Warlock, 4, false));
                                                }
                                                else
                                                {
                                                    pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Succubus, 4, false));
                                                }
                                            }
                                            //else
                                            //{
                                            //    goto Bard;
                                            //}
                                            break;
                                        case 11:
                                            if (settingsRef.Demonkin && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Warlock, 4)) && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Succubus, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Succubus.ToString())   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Warlock.ToString()))
                                            {
                                                if (pawn.gender != Gender.Male)
                                                {
                                                    pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Succubus, 4, false));
                                                }
                                                else
                                                {
                                                    pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Warlock, 4, false));
                                                }
                                            }
                                            //else
                                            //{
                                            //    goto Bard;
                                            //}
                                            break;
                                        case 12:
                                        Bard:;
                                            if (settingsRef.Bard && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_Bard, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.TM_Bard.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Bard, 0, false));
                                            }
                                            //else
                                            //{
                                            //    goto Geomancer;
                                            //}
                                            break;
                                        case 13:
                                        Geomancer:;
                                            if (settingsRef.Geomancer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Geomancer, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Geomancer.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Geomancer, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Technomancer;
                                            //}
                                            break;
                                        case 14:
                                        Technomancer:;
                                            if (settingsRef.Technomancer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Technomancer, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Technomancer.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Technomancer, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto BloodMage;
                                            //}
                                            break;
                                        case 15:
                                        BloodMage:;
                                            if (settingsRef.Technomancer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.BloodMage, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.BloodMage.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.BloodMage, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Enchanter;
                                            //}
                                            break;
                                        case 16:
                                        Enchanter:;
                                            if (settingsRef.Technomancer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Enchanter, 4))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Enchanter.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Enchanter, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto FireMage;
                                            //}
                                            break;
                                    }
                                }
                                else
                                {
                                    goto TraitEnd;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Rand.Chance(((settingsRef.baseFighterChance * 4) + (settingsRef.baseMageChance * 4) + (7 * settingsRef.advFighterChance) + (15 * settingsRef.advMageChance)) / (allTraits.Count - 22)))
                        {
                            if (pawnTraits.Count > 0)
                            {
                                pawnTraits.Remove(pawnTraits[pawnTraits.Count - 1]);
                            }
                            float rnd = Rand.Range(0, 4 * (settingsRef.baseFighterChance + settingsRef.baseMageChance) + (7 * settingsRef.advFighterChance) + (15 * settingsRef.advMageChance));
                            if (rnd < (4 * settingsRef.baseMageChance) && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Gifted, 2)))
                            {
                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Gifted, 2, false));
                            }
                            else if (rnd >= 4 * settingsRef.baseMageChance && rnd < (4 * (settingsRef.baseFighterChance + settingsRef.baseMageChance)) && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.PhysicalProdigy, 2)))
                            {
                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.PhysicalProdigy, 2, false));
                            }
                            else if (rnd >= (4 * (settingsRef.baseFighterChance + settingsRef.baseMageChance)) && rnd < (4 * (settingsRef.baseFighterChance + settingsRef.baseMageChance) + (7 * settingsRef.advFighterChance)))
                            {
                                if (anyFightersEnabled)
                                {
                                    int rndF = Rand.RangeInclusive(1, 7);
                                    switch (rndF)
                                    {
                                        case 1:
                                            //Gladiator:;
                                            if (settingsRef.Gladiator && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Gladiator, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Gladiator, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Sniper;
                                            //}
                                            break;
                                        case 2:
                                            //Sniper:;
                                            if (settingsRef.Sniper && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_Sniper, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Sniper, 0, false));
                                            }
                                            //else
                                            //{
                                            //    goto Bladedancer;
                                            //}
                                            break;
                                        case 3:
                                        Bladedancer:;
                                            if (settingsRef.Bladedancer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Bladedancer, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Bladedancer, 0, false));
                                            }
                                            //else
                                            //{
                                            //    goto Ranger;
                                            //}
                                            break;
                                        case 4:
                                        Ranger:;
                                            if (settingsRef.Ranger && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Ranger, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Ranger, 0, false));
                                            }
                                            //else
                                            //{
                                            //    goto Faceless;
                                            //}
                                            break;
                                        case 5:
                                        Faceless:;
                                            if (settingsRef.Faceless && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Faceless, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Faceless, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Psionic;
                                            //}
                                            break;
                                        case 6:
                                        Psionic:;
                                            if (settingsRef.Psionic && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_Psionic, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Psionic, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto DeathKnight;
                                            //}
                                            break;
                                        case 7:
                                        DeathKnight:;
                                            if (settingsRef.DeathKnight && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.DeathKnight, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.DeathKnight, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Gladiator;
                                            //}
                                            break;
                                    }
                                }
                                else
                                {
                                    goto TraitEnd;
                                }
                            }
                            else
                            {
                                if (anyMagesEnabled)
                                {
                                    int rndM = Rand.RangeInclusive(1, 16);
                                    switch (rndM)
                                    {
                                        case 1:
                                        FireMage:;
                                            if (settingsRef.FireMage && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.InnerFire, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.InnerFire, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto IceMage;
                                            //}
                                            break;
                                        case 2:
                                        IceMage:;
                                            if (settingsRef.IceMage && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.HeartOfFrost, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.HeartOfFrost, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto LitMage;
                                            //}
                                            break;
                                        case 3:
                                        LitMage:;
                                            if (settingsRef.LitMage && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.StormBorn, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.StormBorn, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Arcanist;
                                            //}
                                            break;
                                        case 4:
                                        Arcanist:;
                                            if (settingsRef.Arcanist && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Arcanist, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Arcanist, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Druid;
                                            //}
                                            break;
                                        case 5:
                                        Druid:;
                                            if (settingsRef.Druid && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Druid, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Druid, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Paladin;
                                            //}
                                            break;
                                        case 6:
                                        Paladin:;
                                            if (settingsRef.Paladin && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Paladin, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Paladin, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Summoner;
                                            //}
                                            break;
                                        case 7:
                                        Summoner:;
                                            if (settingsRef.Summoner && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Summoner, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Summoner, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Necromancer;
                                            //}
                                            break;
                                        case 8:
                                        Necromancer:;
                                            if (settingsRef.Necromancer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Necromancer, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Necromancer, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Priest;
                                            //}
                                            break;
                                        case 9:
                                        Priest:;
                                            if (settingsRef.Priest && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Priest, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Priest, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Demonkin;
                                            //}
                                            break;
                                        case 10:
                                        Demonkin:;
                                            if (settingsRef.Demonkin && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Warlock, 4)) && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Succubus, 4)))
                                            {
                                                if (pawn.gender != Gender.Female)
                                                {
                                                    pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Warlock, 4, false));
                                                }
                                                else
                                                {
                                                    pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Succubus, 4, false));
                                                }
                                            }
                                            //else
                                            //{
                                            //    goto Bard;
                                            //}
                                            break;
                                        case 11:
                                            if (settingsRef.Demonkin && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Warlock, 4)) && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Succubus, 4)))
                                            {
                                                if (pawn.gender != Gender.Male)
                                                {
                                                    pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Succubus, 4, false));
                                                }
                                                else
                                                {
                                                    pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Warlock, 4, false));
                                                }
                                            }
                                            //else
                                            //{
                                            //    goto Bard;
                                            //}
                                            break;
                                        case 12:
                                        Bard:;
                                            if (settingsRef.Bard && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_Bard, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Bard, 0, false));
                                            }
                                            //else
                                            //{
                                            //    goto Geomancer;
                                            //}
                                            break;
                                        case 13:
                                        Geomancer:;
                                            if (settingsRef.Geomancer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Geomancer, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Geomancer, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Technomancer;
                                            //}
                                            break;
                                        case 14:
                                        Technomancer:;
                                            if (settingsRef.Technomancer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Technomancer, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Technomancer, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto BloodMage;
                                            //}
                                            break;
                                        case 15:
                                        BloodMage:;
                                            if (settingsRef.Technomancer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.BloodMage, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.BloodMage, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Enchanter;
                                            //}
                                            break;
                                        case 16:
                                        Enchanter:;
                                            if (settingsRef.Technomancer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Enchanter, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Enchanter, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto FireMage;
                                            //}
                                            break;
                                    }
                                }
                                else
                                {
                                    goto TraitEnd;
                                }
                            }
                        }
                    }
                }
                TraitEnd:;
            }
        }

        [HarmonyPatch(typeof(IncidentParmsUtility), "GetDefaultPawnGroupMakerParms", null)]
        public static class GetDefaultPawnGroupMakerParms_Patch
        {
            public static void Postfix(ref PawnGroupMakerParms __result)
            {
                if (__result.faction != null && __result.faction.def.defName == "Seers")
                {
                    __result.points *= 1.65f;
                }

            }
        }

        [HarmonyPatch(typeof(JobGiver_GetFood), "TryGiveJob", null)]
        public static class JobGiver_GetFood_Patch
        {
            public static bool Prefix(Pawn pawn, ref Job __result)
            {
                if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadHD) || pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadAnimalHD))
                {
                    __result = null;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(JobGiver_EatRandom), "TryGiveJob", null)]
        public static class JobGiver_EatRandom_Patch
        {
            public static bool Prefix(Pawn pawn, ref Job __result)
            {
                if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadHD) || pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadAnimalHD))
                {
                    __result = null;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(AbilityDef), "GetJob", null)]
        public static class AbilityDef_Patch
        {
            private static bool Prefix(AbilityDef __instance, AbilityTargetCategory cat, LocalTargetInfo target, ref Job __result)
            {
                if (__instance.abilityClass.FullName == "TorannMagic.MagicAbility" || __instance.abilityClass.FullName == "TorannMagic.MightAbility" || __instance.defName.Contains("TM_Artifact"))
                {
                    Job result;
                    switch (cat)
                    {
                        case AbilityTargetCategory.TargetSelf:
                            result = new Job(TorannMagicDefOf.TMCastAbilitySelf, target);
                            __result = result;
                            return false;
                        case AbilityTargetCategory.TargetThing:
                            result = new Job(TorannMagicDefOf.TMCastAbilityVerb, target);
                            __result = result;
                            return false;
                        case AbilityTargetCategory.TargetAoE:
                            result = new Job(TorannMagicDefOf.TMCastAbilityVerb, target);
                            __result = result;
                            return false;
                    }
                    result = new Job(TorannMagicDefOf.TMCastAbilityVerb, target);
                    __result = result;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(JobDriver_Mine), "ResetTicksToPickHit", null)]
        public static class JobDriver_Mine_Patch
        {
            private static void Postfix(JobDriver_Mine __instance)
            {
                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                if (Rand.Chance(settingsRef.magicyteChance))
                {
                    Thing thing = null;
                    thing = ThingMaker.MakeThing(TorannMagicDefOf.RawMagicyte);
                    thing.stackCount = Rand.Range(5, 12);
                    if (thing != null)
                    {
                        GenPlace.TryPlaceThing(thing, __instance.pawn.Position, __instance.pawn.Map, ThingPlaceMode.Near, null);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Pawn), "GetGizmos", null)]
        public class Pawn_DraftController_GetGizmos_Patch
        {
            public static void Postfix(ref IEnumerable<Gizmo> __result, ref Pawn __instance)
            {
                Pawn pawn = __instance;
                bool flag = __instance != null || __instance.Faction.Equals(Faction.OfPlayer);
                if (flag)
                {
                    List<Gizmo> list = __result.ToList<Gizmo>();
                    bool flag2 = __result == null || !__result.Any<Gizmo>();
                    if (!flag2)
                    {                        
                        ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                        if (Find.Selector.SelectedObjects.Count >= 2 && !settingsRef.showIconsMultiSelect)
                        {
                            for (int z = 0; z < 5; z++)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    if (list[i].ToString().Contains("label=Attack") || list[i].ToString().Contains("label=Melee attack") || list[i].ToString().Contains("Desc=Toggle") || list[i].ToString().Contains("label=Draft"))
                                    {
                                        //yes, filter 5 time                                   
                                    }
                                    else
                                    {
                                        list.Remove(list[i]);
                                    }
                                }
                            }                            
                        }                       
                    }

                    //if (pawn.Faction == Faction.OfPlayer)
                    //{
                    //    if (pawn.drafter != null)
                    //    {
                    //        Command_Action TM_PolymorphDrafter = new Command_Action();
                    //        TM_PolymorphDrafter.action = delegate
                    //        {
                    //            pawn.drafter.Drafted = !pawn.drafter.Drafted;
                    //        };
                    //        TM_PolymorphDrafter.defaultLabel = "CommandDraftLabel".Translate();
                    //        TM_PolymorphDrafter.defaultDesc = "CommandToggleDraftDesc".Translate();
                    //        TM_PolymorphDrafter.icon = TexCommand.Draft;
                    //        TM_PolymorphDrafter.activateSound = SoundDefOf.DraftOn;
                    //        list.Insert(0, TM_PolymorphDrafter);
                    //    }
                    //}

                    __result = list;
                }
            }
        }

        //[HarmonyPatch(typeof(FloatMenuMakerMap))]
        //[HarmonyPatch("CanTakeOrder")]
        //public static class FloatMenuMakerMap_CanTakeOrder_Patch
        //{
        //    [HarmonyPostfix]
        //    public static void MakePawnControllable(Pawn pawn, ref bool __result)

        //    {
        //        bool flagIsCreatureMine = pawn.Faction != null && pawn.Faction.IsPlayer;
        //        bool flagIsCreatureDraftable = (pawn.TryGetComp<CompPolymorph>() != null);

        //        if (flagIsCreatureDraftable && flagIsCreatureMine)
        //        {
        //            //Log.Message("You should be controllable now");
        //            __result = true;
        //        }

        //    }
        //}

        [HarmonyPriority(100)] //Go last
        public static void AddHumanLikeOrders_RestrictEquipmentPatch(Vector3 clickPos, Pawn pawn, ref List<FloatMenuOption> opts)
        {
            IntVec3 c = IntVec3.FromVector3(clickPos);
            if (pawn.equipment != null)
            {
                ThingWithComps equipment = null;
                List<Thing> thingList = c.GetThingList(pawn.Map);
                for (int i = 0; i < thingList.Count; i++)
                {
                    if (thingList[i].def == TorannMagicDefOf.TM_Artifact_BracersOfThePacifist)
                    {
                        equipment = (ThingWithComps)thingList[i];
                        break;
                    }
                }
                if (equipment != null)
                {
                    string labelShort = equipment.LabelShort;
                    FloatMenuOption nve_option;
                    if(!pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                    {
                        for(int j = 0; j< opts.Count; j++)
                        {
                            if(opts[j].Label.Contains("wear"))
                            {
                                opts.Remove(opts[j]);
                            }
                        }
                        nve_option = new FloatMenuOption("TM_ViolentCannotEquip".Translate(pawn.LabelShort, labelShort), null);
                        opts.Add(nve_option);
                    }                    
                }
            }
        }

        [HarmonyPatch(typeof(FloatMenuMakerMap), "AddHumanlikeOrders", null)]
        public static class FloatMenuMakerMap_Patch
        {
            public static void Postfix(Vector3 clickPos, Pawn pawn, ref List<FloatMenuOption> opts)
            {
                IntVec3 c = IntVec3.FromVector3(clickPos);
                Enchantment.CompEnchant comp = pawn.TryGetComp<Enchantment.CompEnchant>();
                CompAbilityUserMagic pawnComp = pawn.TryGetComp<CompAbilityUserMagic>();
                if (comp != null && pawnComp != null && pawnComp.IsMagicUser)
                {
                    bool emptyGround = true;
                    foreach (Thing current in c.GetThingList(pawn.Map))
                    {
                        if (current != null && current.def.EverHaulable)
                        {
                            emptyGround = false;
                        }
                    }
                    if (emptyGround && !pawn.Drafted) //c.GetThingList(pawn.Map).Count == 0 &&
                    {
                        if (comp.enchantingContainer.Count > 0)
                        {
                            if (!pawn.CanReach(c, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
                            {
                                opts.Add(new FloatMenuOption("TM_CannotDrop".Translate(
                                    comp.enchantingContainer[0].Label
                                ) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
                            }
                            else
                            {
                                opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TM_DropGem".Translate(
                                comp.enchantingContainer.ContentsString
                                ), delegate
                                {
                                    Job job = new Job(TorannMagicDefOf.JobDriver_RemoveEnchantingGem, c);
                                    pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                                }, MenuOptionPriority.High, null, null, 0f, null, null), pawn, c, "ReservedBy"));
                            }
                        }

                    }
                    foreach (Thing current in c.GetThingList(pawn.Map))
                    {
                        Thing t = current;
                        if (t != null && t.def.EverHaulable && t.def.defName.ToString().Contains("TM_EStone_"))
                        {
                            if (!pawn.CanReach(t, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
                            {
                                opts.Add(new FloatMenuOption("CannotPickUp".Translate(
                                t.Label
                                ) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
                            }
                            else if (MassUtility.WillBeOverEncumberedAfterPickingUp(pawn, t, 1))
                            {
                                opts.Add(new FloatMenuOption("CannotPickUp".Translate(
                                t.Label
                                ) + " (" + "TooHeavy".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
                            }
                            else// if (item.stackCount == 1)
                            {
                                opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TM_PickupGem".Translate(
                                t.Label
                                ), delegate
                                {
                                    t.SetForbidden(false, false);
                                    Job job = new Job(TorannMagicDefOf.JobDriver_AddEnchantingGem, t);
                                    job.count = 1;
                                    pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                                }, MenuOptionPriority.High, null, null, 0f, null, null), pawn, t, "ReservedBy"));
                            }
                        }
                        else if ((current.def.IsApparel || current.def.IsWeapon || current.def.IsRangedWeapon) && comp.enchantingContainer.Count > 0)
                        {
                            if (!pawn.CanReach(t, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
                            {
                                opts.Add(new FloatMenuOption("TM_CannotReach".Translate(
                                t.Label
                                ) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
                            }
                            else if (pawnComp.Mana.CurLevel < .5f)
                            {
                                opts.Add(new FloatMenuOption("TM_NeedManaForEnchant".Translate(
                                pawnComp.Mana.CurLevel.ToString("0.000")
                                ), null, MenuOptionPriority.Default, null, null, 0f, null, null));
                            }
                            else// if (item.stackCount == 1)
                            {
                                if (current.stackCount == 1)
                                {
                                    opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TM_EnchantItem".Translate(
                                        t.Label
                                    ), delegate
                                    {
                                        t.SetForbidden(true, false);
                                        Job job = new Job(TorannMagicDefOf.JobDriver_EnchantItem, t);
                                        job.count = 1;
                                        pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                                    }, MenuOptionPriority.High, null, null, 0f, null, null), pawn, t, "ReservedBy"));
                                }
                            }
                        }
                    }
                }
            }
        }

        //[HarmonyPatch(typeof(PawnUtility), "IsTravelingInTransportPodWorldObject", null), HarmonyPriority(1000)]
        //[HarmonyBefore(new string[] { "TheThirdAge.RemoveModernStuffHarmony.IsTravelingInTransportPodWorldObject", "rimworld.PawnUtility.IsTravelingInTransportPodWorldObject" })]        
        //[HarmonyPatch(typeof(PawnUtility), "IsTravelingInTransportPodWorldObject", null)]

        [HarmonyPriority(2000)]
        public static bool IsTravelingInTeleportPod_Prefix(Pawn pawn, ref bool __result)
        {
            if(pawn.IsColonist)
            {
                __result = pawn.IsWorldPawn() && ThingOwnerUtility.AnyParentIs<ActiveDropPodInfo>(pawn);
                return false;
            }
            return true;
        }
        

        [HarmonyPatch(typeof(PawnAbility), "PostAbilityAttempt", null)]
        public class PawnAbility_Patch
        {
            public static bool Prefix(PawnAbility __instance)
            {
                if (__instance.Def.defName.Contains("TM_"))
                {
                    CompAbilityUserMagic comp = __instance.Pawn.GetComp<CompAbilityUserMagic>();
                    CompAbilityUserMight mightComp = __instance.Pawn.GetComp<CompAbilityUserMight>();
                    if (comp.IsMagicUser && !__instance.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                    {
                        __instance.CooldownTicksLeft = Mathf.RoundToInt((float)__instance.MaxCastingTicks * comp.coolDown);
                        if (!__instance.Pawn.IsColonist)
                        {
                            __instance.CooldownTicksLeft = (int)(__instance.CooldownTicksLeft / 2f);
                        }
                    }
                    else if (mightComp.IsMightUser)
                    {
                        __instance.CooldownTicksLeft = Mathf.RoundToInt((float)__instance.MaxCastingTicks * mightComp.coolDown);
                    }
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(GenGrid), "Standable", null)]
        public class Standable_Patch
        {
            public static bool Prefix(ref IntVec3 c, ref Map map, ref bool __result)
            {
                if (map != null && c != default(IntVec3))
                {
                    return true;
                }
                __result = false;
                return false;
            }
        }

        [HarmonyPatch(typeof(AttackTargetFinder), "CanSee", null)]
        public class AttackTargetFinder_CanSee_Patch
        {
            public static bool Prefix(Thing target, ref bool __result)
            {
                if (target is Pawn)
                {
                    Pawn targetPawn = target as Pawn;
                    if (targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_I, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_II, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_III, false))
                    {
                        __result = false;
                        return false;
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(AttackTargetFinder), "CanReach", null)]
        public class AttackTargetFinder_CanReach_Patch
        {
            public static bool Prefix(Thing target, ref bool __result)
            {
                if (target is Pawn)
                {
                    Pawn targetPawn = target as Pawn;
                    if (targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_I, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_II, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_III, false))
                    {
                        __result = false;
                        return false;
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(JobGiver_Mate), "TryGiveJob", null)]
        public class JobGiver_Mate_Patch
        {
            public static void Postfix(Pawn pawn, ref Job __result)
            {
                if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadAnimalHD")))
                {
                    __result = null;
                }
            }
        }

        [HarmonyPatch(typeof(MapParent), "CheckRemoveMapNow", null)]
        public class CheckRemoveMapNow_Patch
        {
            public static bool Prefix()
            {
                bool inFlight = ModOptions.Constants.GetPawnInFlight();
                if (inFlight)
                {
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(CompMilkable), "CompInspectStringExtra", null)]
        public class CompMilkable_Patch
        {
            public static void Postfix(CompMilkable __instance, ref string __result)
            {
                if (__instance.parent.def.defName == "Poppi")
                {
                    __result = "Poppi_fuelGrowth".Translate() + ": " + __instance.Fullness.ToStringPercent();
                }
            }
        }

        [HarmonyPatch(typeof(JobGiver_Kidnap), "TryGiveJob", null)]
        public class JobGiver_Kidnap_Patch
        {
            public static void Postfix(Pawn pawn, ref Job __result)
            {
                if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD")))
                {
                    __result = null;
                }               
            }
        }

        [HarmonyPatch(typeof(ITab_Pawn_Gear), "DrawThingRow", null)]
        public class ITab_Pawn_Gear_Patch
        {
            public static Rect GetRowRect(Rect inRect, int row)
            {
                float y = 20f * (float)row;
                Rect result = new Rect(inRect.x, y, inRect.width, 18f);
                return result;
            }

            public static void Postfix(ref float y, float width, Thing thing)
            {
                bool valid = !thing.DestroyedOrNull() && thing.TryGetQuality(out QualityCategory qc);
                if (valid)
                {
                    if (((thing.def.thingClass != null && thing.def.thingClass.ToString() == "RimWorld.Apparel") || thing.TryGetComp<CompEquippable>() != null) && thing.TryGetComp<Enchantment.CompEnchantedItem>() != null)
                    {
                        if (thing.TryGetComp<Enchantment.CompEnchantedItem>().HasEnchantment)
                        {
                            Text.Font = GameFont.Tiny;
                            string str1 = "-- Enchanted (";
                            string str2 = "Enchanted \n\n";

                            Enchantment.CompEnchantedItem enchantedItem = thing.TryGetComp<Enchantment.CompEnchantedItem>();
                            if (enchantedItem.maxMP != 0)
                            {
                                GUI.color = Enchantment.GenEnchantmentColor.EnchantmentColor(enchantedItem.maxMPTier);
                                str1 += "M";
                                str2 += enchantedItem.MaxMPLabel + "\n";
                            }
                            if (enchantedItem.mpCost != 0)
                            {
                                GUI.color = Enchantment.GenEnchantmentColor.EnchantmentColor(enchantedItem.mpCostTier);
                                str1 += "C";
                                str2 += enchantedItem.MPCostLabel + "\n";
                            }
                            if (enchantedItem.mpRegenRate != 0)
                            {
                                GUI.color = Enchantment.GenEnchantmentColor.EnchantmentColor(enchantedItem.mpRegenRateTier);
                                str1 += "R";
                                str2 += enchantedItem.MPRegenRateLabel + "\n";
                            }
                            if (enchantedItem.coolDown != 0)
                            {
                                GUI.color = Enchantment.GenEnchantmentColor.EnchantmentColor(enchantedItem.coolDownTier);
                                str1 += "D";
                                str2 += enchantedItem.CoolDownLabel + "\n";
                            }
                            if (enchantedItem.xpGain != 0)
                            {
                                GUI.color = Enchantment.GenEnchantmentColor.EnchantmentColor(enchantedItem.xpGainTier);
                                str1 += "G";
                                str2 += enchantedItem.XPGainLabel + "\n";
                            }
                            if (enchantedItem.arcaneRes != 0)
                            {
                                GUI.color = Enchantment.GenEnchantmentColor.EnchantmentColor(enchantedItem.arcaneResTier);
                                str1 += "X";
                                str2 += enchantedItem.ArcaneResLabel + "\n";
                            }
                            if (enchantedItem.arcaneDmg != 0)
                            {
                                GUI.color = Enchantment.GenEnchantmentColor.EnchantmentColor(enchantedItem.arcaneDmgTier);
                                str1 += "Z";
                                str2 += enchantedItem.ArcaneDmgLabel + "\n";
                            }
                            if (enchantedItem.arcaneSpectre != false)
                            {
                                GUI.color = Enchantment.GenEnchantmentColor.EnchantmentColor(enchantedItem.skillTier);
                                str1 += "*S";
                                str2 += enchantedItem.ArcaneSpectreLabel + "\n";
                            }
                            if (enchantedItem.phantomShift != false)
                            {
                                GUI.color = Enchantment.GenEnchantmentColor.EnchantmentColor(enchantedItem.skillTier);
                                str1 += "*P";
                                str2 += enchantedItem.PhantomShiftLabel + "\n";
                            }
                            str1 += ")";
                            y -= 6f;
                            Rect rect = new Rect(48f, y, width - 36f, 28f);
                            Widgets.Label(rect, str1);

                            TooltipHandler.TipRegion(rect, () => string.Concat(new string[]
                            {
                            str2,
                            }), 398512);

                            y += 28f;
                            GUI.color = Color.white;
                            Text.Font = GameFont.Small;
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ITab_Pawn_Gear), "TryDrawOverallArmor", null)]
        public class ITab_Pawn_GearFillTab_Patch
        {
            //public static FieldInfo pawn = typeof(ITab_Pawn_Gear).GetField("SelPawnForGear", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);

            public static void Postfix(ITab_Pawn_Gear __instance, ref float curY, float width, StatDef stat, string label)
            {
                if (stat.defName == "ArmorRating_Heat")
                {
                    //Traverse traverse = Traverse.Create(__instance);
                    Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
                    if (!pawn.DestroyedOrNull() && !pawn.Dead)
                    {
                        stat = StatDef.Named("ArmorRating_Alignment");
                        label = "TM_ArmorHarmony".Translate();
                        float num = 0f;
                        float num2 = Mathf.Clamp01(pawn.GetStatValue(stat, true) / 2f);
                        List<BodyPartRecord> allParts = pawn.RaceProps.body.AllParts;
                        List<Apparel> list = (pawn.apparel == null) ? null : pawn.apparel.WornApparel;
                        for (int i = 0; i < allParts.Count; i++)
                        {
                            float num3 = 1f - num2;
                            if (list != null)
                            {
                                for (int j = 0; j < list.Count; j++)
                                {
                                    if (list[j].def.apparel.CoversBodyPart(allParts[i]))
                                    {
                                        float num4 = Mathf.Clamp01(list[j].GetStatValue(stat, true) / 2f);
                                        num3 *= 1f - num4;
                                    }
                                }
                            }
                            num += allParts[i].coverageAbs * (1f - num3);
                        }
                        num = Mathf.Clamp(num * 2f, 0f, 2f);
                        Rect rect = new Rect(0f, curY, width, 100f);
                        Widgets.Label(rect, label.Truncate(120f, null));
                        rect.xMin += 120f;
                        Widgets.Label(rect, num.ToStringPercent());
                        curY += 22f;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(NegativeInteractionUtility), "NegativeInteractionChanceFactor", null)]
        public class NegativeInteractionChanceFactor_Patch
        {
            public static void Postfix(Pawn initiator, Pawn recipient, ref float __result)
            {
                if (initiator.story.traits.HasTrait(TorannMagicDefOf.TM_Bard))
                {
                    MagicPowerSkill ver = initiator.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Entertain.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Entertain_ver");
                    __result = __result / (1 + ver.level);

                }
                if (initiator.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper) || recipient.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper))
                {
                    __result *= 1.2f;
                }
                if (recipient.story.traits.HasTrait(TorannMagicDefOf.TM_Bard))
                {
                    MagicPowerSkill ver = recipient.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Entertain.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Entertain_ver");
                    __result = __result / (1 + ver.level);
                }
                if (initiator.Inspired && initiator.InspirationDef.defName == "Outgoing")
                {
                    __result = __result * .5f;
                }
            }
        }

        [HarmonyPatch(typeof(InspirationHandler), "TryStartInspiration", null)]
        public class InspirationHandler_Patch
        {
            public static bool Prefix(InspirationHandler __instance, ref bool __result)
            {
                if (__instance.pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD")) || __instance.pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_LichHD")))
                {
                    __result = false;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(ColonistBarColonistDrawer), "DrawIcons", null)]
        public class ColonistBarColonistDrawer_Patch
        {
            public static bool Prefix(ColonistBarColonistDrawer __instance, ref Rect rect, Pawn colonist)
            {
                if (!colonist.Dead)
                {
                    float num = 20f * Find.ColonistBar.Scale;
                    Vector2 vector = new Vector2(rect.x + 1f, rect.yMax - num - 1f);
                    if (colonist.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD")))
                    {
                        rect = new Rect(vector.x, vector.y, num, num);
                        GUI.DrawTexture(rect, TM_MatPool.Icon_Undead);
                        TooltipHandler.TipRegion(rect, "TM_Icon_Undead".Translate());
                        vector.x += num;
                    }
                }
                return true;
            }
        }



        [HarmonyPatch(typeof(Pawn_InteractionsTracker), "InteractionsTrackerTick", null)]
        public class InteractionsTrackerTick_Patch
        {
            public static FieldInfo pawn = typeof(Pawn_InteractionsTracker).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
            public static FieldInfo wantsRandomInteract = typeof(Pawn_InteractionsTracker).GetField("wantsRandomInteract", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
            public static FieldInfo lastInteractionTime = typeof(Pawn_InteractionsTracker).GetField("lastInteractionTime", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);

            public static void Postfix(Pawn_InteractionsTracker __instance)
            {
                if (Find.TickManager.TicksGame % 1200 == 0)
                {
                    Traverse traverse = Traverse.Create(__instance);
                    Pawn pawn = (Pawn)InteractionsTrackerTick_Patch.pawn.GetValue(__instance);
                    if (pawn.IsColonist && !pawn.Downed && !pawn.Dead && pawn.RaceProps.Humanlike)
                    {
                        CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
                        int lastInteractionTime = (int)InteractionsTrackerTick_Patch.lastInteractionTime.GetValue(__instance);
                        if (comp != null && comp.IsMagicUser && comp.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Bard))
                        {
                            MagicPowerSkill pwr = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Entertain.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Entertain_pwr");
                            if ((Find.TickManager.TicksGame - lastInteractionTime) > (3000 - (360 * pwr.level)))
                            {
                                InteractionsTrackerTick_Patch.wantsRandomInteract.SetValue(__instance, true);
                            }
                        }
                        if (pawn.Inspired && pawn.InspirationDef.defName == "ID_Outgoing")
                        {
                            if ((Find.TickManager.TicksGame - lastInteractionTime) > (1800))
                            {
                                InteractionsTrackerTick_Patch.wantsRandomInteract.SetValue(__instance, true);
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(MentalStateHandler), "TryStartMentalState", null)]
        public class MentalStateHandler_Patch
        {
            public static FieldInfo pawn = typeof(MentalStateHandler).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);

            public static bool Prefix(MentalStateHandler __instance, MentalStateDef stateDef, Pawn otherPawn, ref bool __result)
            {
                Traverse traverse = Traverse.Create(__instance);
                Pawn pawn = (Pawn)MentalStateHandler_Patch.pawn.GetValue(__instance);

                if (pawn.RaceProps.Humanlike && pawn.story.traits.HasTrait(TorannMagicDefOf.Undead))
                {
                    __result = false;
                    return false;
                }
                return true;
            }

            //public static void Postfix(MentalStateHandler __instance, MentalStateDef stateDef, ref bool __result)
            //{
            //    Traverse traverse = Traverse.Create(__instance);
            //    Pawn pawn = (Pawn)MentalStateHandler_Patch.pawn.GetValue(__instance);

            //    Log.Message("pawn in postfix is " + pawn.LabelShort);
            //    int necroCount = 0;
            //    List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            //    List<Pawn> undeadPawns = new List<Pawn>();
            //    for (int i = 0; i < mapPawns.Count; i++)
            //    {
            //        if (mapPawns[i].RaceProps.Humanlike)
            //        {
            //            if (mapPawns[i].story.traits.HasTrait(TorannMagicDefOf.Undead))
            //            {
            //                undeadPawns.Add(mapPawns[i]);
            //            }
            //            if (mapPawns[i].story.traits.HasTrait(TorannMagicDefOf.Necromancer))
            //            {
            //                necroCount++;
            //            }
            //        }
            //    }

            //    Log.Message("necro count is " + necroCount);
            //    Log.Message("undead count is " + undeadPawns.Count);
            //    if (pawn.RaceProps.Humanlike && pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer))
            //    {
            //        for (int j = 0; j < undeadPawns.Count; j++)
            //        {
            //            if (Rand.Chance(1 / necroCount))
            //            {
            //                Log.Message("applying mental state " + stateDef.LabelCap + " to " + undeadPawns[j].Label);
            //                undeadPawns[j].mindState.mentalStateHandler.TryStartMentalState(stateDef, "", false, false, pawn);
            //            }
            //        }
            //    }
            //}
        }

        [HarmonyPriority(100)]
        [HarmonyPatch(typeof(Pawn_NeedsTracker), "ShouldHaveNeed", null)]
        public static class Pawn_NeedsTracker_Patch
        {
            public static FieldInfo pawn = typeof(Pawn_NeedsTracker).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
            public static bool Prefix(Pawn_NeedsTracker __instance, NeedDef nd, ref bool __result)
            {
                Traverse traverse = Traverse.Create(__instance);
                Pawn pawn = (Pawn)Pawn_NeedsTracker_Patch.pawn.GetValue(__instance);
                if (pawn != null)
                {
                    if (nd.defName == "ROMV_Blood" && (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD")) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadAnimalHD")) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_LichHD"))))
                    {
                        bool hasVampHediff = pawn.health.hediffSet.HasHediff(HediffDef.Named("ROM_Vampirism"));
                        if (hasVampHediff)
                        {
                            return true;
                        }
                        __result = false;
                        return false;
                    }
                    if ((nd.defName == "TM_Mana" || nd.defName == "TM_Stamina") && pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD")))
                    {
                        __result = false;
                        return false;
                    }
                    //if(pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD")) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadAnimalHD")))
                    //{
                    //    __result = false;
                    //    return false;
                    //}
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(FloatMenuMakerMap), "ChoicesAtFor", null), HarmonyPriority(100)]
        public static class FloatMenuMakerMap_ROMV_Undead_Patch
        {
            public static void Postfix(Vector3 clickPos, Pawn pawn, ref List<FloatMenuOption> __result)
            {
                IntVec3 c = IntVec3.FromVector3(clickPos);
                Pawn target = c.GetFirstPawn(pawn.Map);
                if (target != null)
                {
                    if ((target.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD")) || target.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadAnimalHD")) || target.health.hediffSet.HasHediff(HediffDef.Named("TM_LichHD"))))
                    {
                        for (int i = 0; i < __result.Count(); i++)
                        {
                            string name = target.LabelShort;
                            if (__result[i].Label.Contains("Feed on") || __result[i].Label.Contains("Sip") || __result[i].Label.Contains("Embrace") || __result[i].Label.Contains("Give vampirism") || __result[i].Label.Contains("Create Ghoul") || __result[i].Label.Contains("Give vitae") || __result[i].Label == "Embrace " + name + " (Give vampirism)")
                            {
                                __result.Remove(__result[i]);
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(DamageWorker), "ExplosionStart", null)]
        public static class ExplosionNoShaker_Patch
        {
            public static bool Prefix(DamageWorker __instance, Explosion explosion)
            {
                if(explosion.damType == TMDamageDefOf.DamageDefOf.TM_BlazingPower || explosion.damType == TMDamageDefOf.DamageDefOf.TM_BloodBurn)
                {
                    MoteMaker.MakeStaticMote(explosion.Position, explosion.Map, ThingDefOf.Mote_ExplosionFlash, explosion.radius * 6f);
                    for (int i = 0; i < 4; i++)
                    {
                        if (explosion.damType == TMDamageDefOf.DamageDefOf.TM_BloodBurn)
                        {
                            if (i < 1)
                            {
                                TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_BloodMist"), explosion.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(explosion.radius * 0.7f), explosion.Map, Rand.Range(1f, 1.5f), .2f, 0.6f, 2f, Rand.Range(-30, 30), Rand.Range(.5f, .7f), Rand.Range(30f, 40f), Rand.Range(0, 360));
                            }
                        }
                        else
                        {
                            MoteMaker.ThrowSmoke(explosion.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(explosion.radius * 0.7f), explosion.Map, explosion.radius * 0.6f);
                        }
                    }
                    if (__instance.def.explosionInteriorMote != null)
                    {
                        int num = Mathf.RoundToInt(3.14159274f * explosion.radius * explosion.radius / 6f);
                        for (int j = 0; j < num; j++)
                        {
                            MoteMaker.ThrowExplosionInteriorMote(explosion.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(explosion.radius * 0.7f), explosion.Map, __instance.def.explosionInteriorMote);
                        }
                    }
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(QualityUtility), "GenerateQualityCreatedByPawn", null)]
        [HarmonyPatch(new Type[]
        {
            typeof(Pawn),
            typeof(SkillDef)
        })]
        public static class ArcaneForge_Quality_Patch
        {
            public static void Postfix(Pawn pawn, SkillDef relevantSkill, ref QualityCategory __result)
            {
                CompAbilityUserMagic comp = pawn.TryGetComp<CompAbilityUserMagic>();
                if(comp != null && comp.IsMagicUser && pawn.story.traits != null && !pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless) && comp.ArcaneForging)
                {
                    List<IntVec3> cellList = GenRadial.RadialCellsAround(pawn.Position, 2, true).ToList();
                    bool forgeNearby = false;
                    for(int i = 0; i < cellList.Count; i++)
                    {
                        Building bldg = cellList[i].GetFirstBuilding(pawn.Map);
                        if(bldg != null && bldg.def == TorannMagicDefOf.TableArcaneForge)
                        {
                            forgeNearby = true;
                        }
                    }
                    if (forgeNearby)
                    {
                        int mageLevel = Rand.Range(0, Mathf.RoundToInt(comp.MagicUserLevel / 15));
                        __result = (QualityCategory)Mathf.Min((int)__result + mageLevel, 6);
                        SoundInfo info = SoundInfo.InMap(new TargetInfo(pawn.Position, pawn.Map, false), MaintenanceType.None);
                        info.pitchFactor = .6f;
                        info.volumeFactor = 1.6f;
                        TorannMagicDefOf.TM_Gong.PlayOneShot(info);
                        cellList.Clear();
                        cellList = GenRadial.RadialCellsAround(pawn.Position, (int)__result, false).ToList<IntVec3>();
                        for (int i = 0; i < cellList.Count; i++)
                        {
                            IntVec3 curCell = cellList[i];
                            Vector3 angle = TM_Calc.GetVector(pawn.Position, curCell);
                            TM_MoteMaker.ThrowArcaneWaveMote(curCell.ToVector3(), pawn.Map, .4f * (curCell - pawn.Position).LengthHorizontal, .1f, .05f, .1f, 0, 3, (Quaternion.AngleAxis(90, Vector3.up) * angle).ToAngleFlat(), (Quaternion.AngleAxis(90, Vector3.up) * angle).ToAngleFlat());
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(WorkGiver_DoBill), "JobOnThing", null)]
        public static class RegrowthSurgery_WorkGiver_Patch
        {
            public static void Postfix(ref Job __result, Pawn pawn, Thing thing, bool forced = false)
            {
                if (!((__result == null || pawn == null || thing == null) | forced))
                {
                    IBillGiver billGiver = thing as IBillGiver;
                    if (billGiver != null)
                    {
                        Bill bill = __result.bill;
                        if (bill != null)
                        {
                            RecipeDef recipe = bill.recipe;
                            if (recipe != null && ConfirmRegrowthSurgery(pawn, billGiver, recipe))
                            {
                                __result = null;
                            }
                        }
                    }
                }
            }

            private static Pawn GiverPawn(IBillGiver billGiver)
            {
                Pawn pawn = null;
                if (billGiver is Corpse)
                {
                    Corpse corpse = billGiver as Corpse;
                    pawn = corpse.InnerPawn;
                }
                if (billGiver is Pawn)
                {
                    pawn = billGiver as Pawn;
                }
                return pawn;
            }

            private static bool ConfirmRegrowthSurgery(Pawn pawn, IBillGiver billGiver, RecipeDef recipe)
            {
                if (recipe == null || !recipe.IsSurgery)
                {
                    return false;
                }                
                if(recipe.defName != "Regrowth")
                {
                    return false;
                }
                //Pawn giverPawn = GiverPawn(billGiver);
                //if (giverPawn.RaceProps.IsMechanoid)
                //{
                //    return false;
                //}
                return !IsCapableDruid(pawn, recipe);
            }            

            private static bool IsCapableDruid(Pawn p, RecipeDef recipe)
            {
                if (p.RaceProps.Humanlike && p.skills != null)
                {
                    if(p.workSettings.WorkIsActive(WorkTypeDefOf.Doctor) && p.story.traits.HasTrait(TorannMagicDefOf.Druid))
                    {
                        if (recipe.PawnSatisfiesSkillRequirements(p) && p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && p.health.capacities.CapableOf(PawnCapacityDefOf.Moving))
                        {
                            CompAbilityUserMagic comp = p.GetComp<CompAbilityUserMagic>();
                            if(comp.Mana.CurLevel > .9f)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        [HarmonyPatch(typeof(Command_PawnAbility), "GizmoOnGUI", null)]
        public static class GizmoOnGUI_Prefix_Patch
        {
            public static bool Prefix(Command_PawnAbility __instance, Vector2 topLeft, float maxWidth, ref GizmoResult __result)
            {
                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                if (settingsRef.autocastEnabled && __instance.pawnAbility.Def.defName.Contains("TM_"))
                {
                    CompAbilityUserMagic comp = __instance.pawnAbility.Pawn.GetComp<CompAbilityUserMagic>();
                    CompAbilityUserMight mightComp = __instance.pawnAbility.Pawn.GetComp<CompAbilityUserMight>();
                    MagicPower magicPower = null;
                    MightPower mightPower = null;

                    Rect rect = new Rect(topLeft.x, topLeft.y, __instance.GetWidth(maxWidth), 75f);
                    bool flag = false;
                    if (Mouse.IsOver(rect))
                    {
                        flag = true;
                        GUI.color = GenUI.MouseoverColor;
                    }
                    Texture2D texture2D = __instance.icon;
                    if (texture2D == null)
                    {
                        texture2D = BaseContent.BadTex;
                    }
                    GUI.DrawTexture(rect, Command.BGTex);
                    MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Command);
                    GUI.color = __instance.IconDrawColor;
                    Widgets.DrawTextureFitted(new Rect(rect), texture2D, __instance.iconDrawScale * 0.85f, __instance.iconProportions, __instance.iconTexCoords);
                    GUI.color = Color.white;
                    bool flag2 = false;
                    KeyCode keyCode = (__instance.hotKey != null) ? __instance.hotKey.MainKey : KeyCode.None;
                    if (keyCode != 0 && !GizmoGridDrawer.drawnHotKeys.Contains(keyCode))
                    {
                        Rect rect2 = new Rect(rect.x + 5f, rect.y + 5f, rect.width - 10f, 18f);
                        Widgets.Label(rect2, keyCode.ToStringReadable());
                        GizmoGridDrawer.drawnHotKeys.Add(keyCode);
                        if (__instance.hotKey.KeyDownEvent)
                        {
                            flag2 = true;
                            Event.current.Use();
                        }
                    }
                    if (Widgets.ButtonInvisible(rect))
                    {
                        flag2 = true;
                    }
                    string labelCap = __instance.LabelCap;
                    if (!labelCap.NullOrEmpty())
                    {
                        float num = Text.CalcHeight(labelCap, rect.width);
                        num -= 2f;
                        Rect rect3 = new Rect(rect.x, rect.yMax - num + 12f, rect.width, num);
                        GUI.DrawTexture(rect3, TexUI.GrayTextBG);
                        GUI.color = Color.white;
                        Text.Anchor = TextAnchor.UpperCenter;
                        Widgets.Label(rect3, labelCap);
                        Text.Anchor = TextAnchor.UpperLeft;
                        GUI.color = Color.white;
                    }
                    GUI.color = Color.white;
                    if (true)
                    {
                        TipSignal tipSignal = __instance.Desc;
                        if (__instance.disabled && !__instance.disabledReason.NullOrEmpty())
                        {
                            tipSignal.text = tipSignal.text + "\n" + StringsToTranslate.AU_DISABLED + ": " + __instance.disabledReason;
                        }
                        TooltipHandler.TipRegion(rect, tipSignal);
                    }
                    if (__instance.pawnAbility.CooldownTicksLeft != -1 && __instance.pawnAbility.CooldownTicksLeft < __instance.pawnAbility.MaxCastingTicks)
                    {
                        float fillPercent = (float)__instance.curTicks / (float)__instance.pawnAbility.MaxCastingTicks;
                        Widgets.FillableBar(rect, fillPercent, AbilityButtons.FullTex, AbilityButtons.EmptyTex, doBorder: false);
                    }
                    if (!__instance.HighlightTag.NullOrEmpty() && (Find.WindowStack.FloatMenu == null || !Find.WindowStack.FloatMenu.windowRect.Overlaps(rect)))
                    {
                        UIHighlighter.HighlightOpportunity(rect, __instance.HighlightTag);
                    }
                    if (comp != null && __instance.pawnAbility.Def != null)
                    {
                        if (__instance.pawnAbility.Def.defName == "TM_Blink")
                        {
                            magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Blink);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect)) //__result.State == GizmoState.Mouseover)
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Blink_I")
                        {
                            magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Blink);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Blink_II")
                        {
                            magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Blink_I);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Blink_III")
                        {
                            magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Blink_II);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Summon")
                        {
                            magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Summon);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Summon_I")
                        {
                            magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Summon);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Summon_II")
                        {
                            magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Summon_I);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Summon_III")
                        {
                            magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Summon_II);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Firebolt")
                        {
                            magicPower = comp.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Firebolt);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Icebolt")
                        {
                            magicPower = comp.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Icebolt);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_LightningBolt")
                        {
                            magicPower = comp.MagicData.MagicPowersSB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_LightningBolt);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_FrostRay")
                        {
                            magicPower = comp.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_FrostRay);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_FrostRay_I")
                        {
                            magicPower = comp.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_FrostRay);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_FrostRay_II")
                        {
                            magicPower = comp.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_FrostRay_I);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_FrostRay_III")
                        {
                            magicPower = comp.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_FrostRay_II);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_MagicMissile")
                        {
                            magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_MagicMissile);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_MagicMissile_I")
                        {
                            magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_MagicMissile);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_MagicMissile_II")
                        {
                            magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_MagicMissile_I);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_MagicMissile_III")
                        {
                            magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_MagicMissile_II);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Entertain")
                        {
                            magicPower = comp.MagicData.MagicPowersB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Entertain);
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_EnchantedAura")
                        {
                            magicPower = comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchantedAura);
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_EnchantedBody")
                        {
                            magicPower = comp.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchantedBody);
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Shadow)
                        {
                            magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shadow);
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Shadow_I)
                        {
                            magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shadow);
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Shadow_II)
                        {
                            magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shadow_I);
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Shadow_III)
                        {
                            magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shadow_II);
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_RayofHope)
                        {
                            magicPower = comp.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_RayofHope);
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_RayofHope_I)
                        {
                            magicPower = comp.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_RayofHope);
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_RayofHope_II)
                        {
                            magicPower = comp.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_RayofHope_I);
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_RayofHope_III)
                        {
                            magicPower = comp.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_RayofHope_II);
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Soothe)
                        {
                            magicPower = comp.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Soothe);
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Soothe_I)
                        {
                            magicPower = comp.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Soothe);
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Soothe_II)
                        {
                            magicPower = comp.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Soothe_I);
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Soothe_III)
                        {
                            magicPower = comp.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Soothe_II);
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Poison")
                        {
                            magicPower = comp.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Poison);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Regenerate")
                        {
                            magicPower = comp.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Regenerate);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_CureDisease")
                        {
                            magicPower = comp.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_CureDisease);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Heal")
                        {
                            magicPower = comp.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Heal);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Shield")
                        {
                            magicPower = comp.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Shield_I")
                        {
                            magicPower = comp.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Shield_II")
                        {
                            magicPower = comp.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield_I);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Shield_III")
                        {
                            magicPower = comp.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield_II);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_AdvancedHeal")
                        {
                            magicPower = comp.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_AdvancedHeal);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_TransferMana")
                        {
                            magicPower = comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TransferMana);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_SiphonMana")
                        {
                            magicPower = comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SiphonMana);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_CauterizeWound")
                        {
                            magicPower = comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_CauterizeWound);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_SpellMending")
                        {
                            magicPower = comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SpellMending);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_TeachMagic")
                        {
                            magicPower = comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TeachMagic);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_ShadowBolt")
                        {
                            if (comp.Pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus))
                            {
                                magicPower = comp.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt);
                            }
                            else
                            {
                                magicPower = comp.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt);
                            }

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_ShadowBolt_I")
                        {
                            if (comp.Pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus))
                            {
                                magicPower = comp.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt);
                            }
                            else
                            {
                                magicPower = comp.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt);
                            }

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_ShadowBolt_II")
                        {
                            if (comp.Pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus))
                            {
                                magicPower = comp.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt_I);
                            }
                            else
                            {
                                magicPower = comp.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt_I);
                            }

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_ShadowBolt_III")
                        {
                            if (comp.Pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus))
                            {
                                magicPower = comp.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt_II);
                            }
                            else
                            {
                                magicPower = comp.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt_II);
                            }

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Purify")
                        {
                            magicPower = comp.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Purify);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_SummonMinion")
                        {
                            magicPower = comp.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonMinion);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                    }
                    if (mightComp != null && __instance.pawnAbility.Def != null)
                    {
                        //might abilities
                        if (__instance.pawnAbility.Def.defName == "TM_Grapple")
                        {
                            mightPower = mightComp.MightData.MightPowersG.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Grapple);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Grapple_I")
                        {
                            mightPower = mightComp.MightData.MightPowersG.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Grapple);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Grapple_II")
                        {
                            mightPower = mightComp.MightData.MightPowersG.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Grapple_I);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Grapple_III")
                        {
                            mightPower = mightComp.MightData.MightPowersG.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Grapple_II);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_BladeSpin")
                        {
                            mightPower = mightComp.MightData.MightPowersB.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_BladeSpin);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_PhaseStrike")
                        {
                            mightPower = mightComp.MightData.MightPowersB.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_PhaseStrike);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_PhaseStrike_I")
                        {
                            mightPower = mightComp.MightData.MightPowersB.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_PhaseStrike);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_PhaseStrike_II")
                        {
                            mightPower = mightComp.MightData.MightPowersB.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_PhaseStrike_I);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_PhaseStrike_III")
                        {
                            mightPower = mightComp.MightData.MightPowersB.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_PhaseStrike_II);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_ArrowStorm")
                        {
                            mightPower = mightComp.MightData.MightPowersR.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ArrowStorm);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_ArrowStorm_I")
                        {
                            mightPower = mightComp.MightData.MightPowersR.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ArrowStorm);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_ArrowStorm_II")
                        {
                            mightPower = mightComp.MightData.MightPowersR.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ArrowStorm_I);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_ArrowStorm_III")
                        {
                            mightPower = mightComp.MightData.MightPowersR.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ArrowStorm_II);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_DisablingShot")
                        {
                            mightPower = mightComp.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_DisablingShot);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_DisablingShot_I")
                        {
                            mightPower = mightComp.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_DisablingShot);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_DisablingShot_II")
                        {
                            mightPower = mightComp.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_DisablingShot_I);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_DisablingShot_III")
                        {
                            mightPower = mightComp.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_DisablingShot_II);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Spite")
                        {
                            mightPower = mightComp.MightData.MightPowersDK.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Spite);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Spite_I")
                        {
                            mightPower = mightComp.MightData.MightPowersDK.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Spite);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Spite_II")
                        {
                            mightPower = mightComp.MightData.MightPowersDK.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Spite_I);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Spite_III")
                        {
                            mightPower = mightComp.MightData.MightPowersDK.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Spite_II);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_Headshot")
                        {
                            mightPower = mightComp.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Headshot);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_AntiArmor")
                        {
                            mightPower = mightComp.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_AntiArmor);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def.defName == "TM_TeachMight")
                        {
                            mightPower = mightComp.MightData.MightPowersStandalone.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_TeachMight);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                    }
                    if (magicPower != null)
                    {
                        //Rect rect = new Rect(topLeft.x, topLeft.y, __instance.GetWidth(maxWidth), 75f);
                        Rect position = new Rect(rect.x + rect.width - 24f, rect.y, 24f, 24f);
                        Texture2D image = (!magicPower.AutoCast) ? Widgets.CheckboxOffTex : Widgets.CheckboxOnTex;
                        GUI.DrawTexture(position, image);
                    }
                    if (mightPower != null)
                    {
                        //Rect rect = new Rect(topLeft.x, topLeft.y, __instance.GetWidth(maxWidth), 75f);
                        Rect position = new Rect(rect.x + rect.width - 24f, rect.y, 24f, 24f);
                        Texture2D image = (!mightPower.AutoCast) ? Widgets.CheckboxOffTex : Widgets.CheckboxOnTex;
                        GUI.DrawTexture(position, image);
                    }
                    if (flag2 && !Input.GetMouseButtonDown(1) && !Input.GetMouseButtonUp(1))
                    {
                        if (__instance.disabled)
                        {
                            if (!__instance.disabledReason.NullOrEmpty())
                            {
                                Messages.Message(__instance.disabledReason, MessageTypeDefOf.RejectInput);
                            }
                            __result = new GizmoResult(GizmoState.Mouseover, null);
                            return false;
                        }
                        if (!TutorSystem.AllowAction(__instance.TutorTagSelect))
                        {
                            __result = new GizmoResult(GizmoState.Mouseover, null);
                            return false;
                        }
                        __result = new GizmoResult(GizmoState.Interacted, Event.current);
                        return false;
                    }

                    if (flag)
                    {
                        __result = new GizmoResult(GizmoState.Mouseover, null);
                        return false;
                    }
                    __result = new GizmoResult(GizmoState.Clear, null);
                    return false;
                }
                return true;
            }
        }

        //[HarmonyPatch(typeof(Pawn_ApparelTracker), "Wear", null)]
        //public class Pawn_ApparelTracker_Wear_Patch
        //{
        //    //public static FieldInfo pawn = typeof(Pawn_ApparelTracker).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);

        //    public static bool Prefix(Apparel newApparel)
        //    {

        //        if(newApparel != null)
        //        {
        //            Log.Message("wearing apparel " + newApparel.def.defName);
        //            Log.Message("apparel properties last layer " + newApparel.def.apparel.LastLayer);
        //            Log.Message("apparel draw order " + newApparel.def.apparel.LastLayer.drawOrder);

        //        }
        //        return true;
        //    }
        //}        
    }
}
