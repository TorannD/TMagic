using HarmonyLib;
using RimWorld;
using AbilityUser;
using RimWorld.Planet;
using System;
using System.Text;
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
using TorannMagic.Conditions;
using RimWorld.QuestGen;

namespace TorannMagic
{
    //[StaticConstructorOnStartup]
    //internal class HarmonyPatches
    //{
    //    private static readonly Type patchType = typeof(HarmonyPatches);

    //    static HarmonyPatches()
    //    {

        //*notes, replacing HarmonyPatches with TorannMagicMod
    public class TorannMagicMod : Mod
    {
        private static readonly Type patchType = typeof(TorannMagicMod);

        public TorannMagicMod(ModContentPack content) : base(content)
        { 
            var harmonyInstance = new Harmony("rimworld.torann.tmagic");

            harmonyInstance.Patch(AccessTools.Method(typeof(IncidentWorker_SelfTame), "Candidates"), null,
                 new HarmonyMethod(patchType, nameof(SelfTame_Candidates_Patch)), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(IncidentWorker_DiseaseHuman), "PotentialVictimCandidates"), null,
                 new HarmonyMethod(patchType, nameof(DiseaseHuman_Candidates_Patch)), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(IncidentWorker_DiseaseAnimal), "PotentialVictimCandidates"), null,  //calls the same patch as human, which includes hediff for undead animals
                 new HarmonyMethod(patchType, nameof(DiseaseHuman_Candidates_Patch)), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Pawn), "GetGizmos"), null,
                 new HarmonyMethod(patchType, nameof(Pawn_Gizmo_TogglePatch)), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Pawn), "GetGizmos"), null,
                 new HarmonyMethod(patchType, nameof(Pawn_Gizmo_ActionPatch)), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(CompPowerPlant), "CompTick"), null,
                 new HarmonyMethod(patchType, nameof(PowerCompTick_Overdrive_Postfix)), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Building_TurretGun), "Tick"), null,
                 new HarmonyMethod(patchType, nameof(TurretGunTick_Overdrive_Postfix)), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(CompRefuelable), "PostDraw"), new HarmonyMethod(patchType, nameof(CompRefuelable_DrawBar_Prefix)),
                 null, null);
            harmonyInstance.Patch(AccessTools.Method(typeof(AutoUndrafter), "AutoUndraftTick"), new HarmonyMethod(patchType, nameof(AutoUndrafter_Undead_Prefix)),
                 null, null);
            harmonyInstance.Patch(AccessTools.Method(typeof(PawnUtility), "IsTravelingInTransportPodWorldObject"),
                 new HarmonyMethod(patchType, nameof(IsTravelingInTeleportPod_Prefix)), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(FloatMenuMakerMap), "AddHumanlikeOrders"), null,
                 new HarmonyMethod(patchType, nameof(AddHumanLikeOrders_RestrictEquipmentPatch)), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(CompAbilityItem), "PostDrawExtraSelectionOverlays"), new HarmonyMethod(patchType, nameof(CompAbilityItem_Overlay_Prefix)),
                 null, null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Verb), "CanHitCellFromCellIgnoringRange"), new HarmonyMethod(patchType, nameof(RimmuNation_CHCFCIR_Patch)),
                 null, null);
            harmonyInstance.Patch(AccessTools.Method(typeof(WealthWatcher), "ForceRecount"), null,
                 new HarmonyMethod(patchType, nameof(WealthWatcher_ClassAdjustment_Postfix)), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Pawn_EquipmentTracker), "TryDropEquipment"), null,
                 new HarmonyMethod(patchType, nameof(PawnEquipment_Drop_Postfix)), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Pawn_EquipmentTracker), "AddEquipment"), null,
                 new HarmonyMethod(patchType, nameof(PawnEquipment_Add_Postfix)), null);

            harmonyInstance.Patch(AccessTools.Method(typeof(Pawn), "get_IsColonist", null, null), new HarmonyMethod(typeof(TorannMagicMod), "Get_IsColonist_Polymorphed", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Caravan), "get_NightResting", null, null), new HarmonyMethod(typeof(TorannMagicMod), "Get_NightResting_Undead", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Pawn_StanceTracker), "get_Staggered", null, null), new HarmonyMethod(typeof(TorannMagicMod), "Get_Staggered", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Verb_LaunchProjectile), "get_Projectile", null, null), new HarmonyMethod(typeof(TorannMagicMod), "Get_Projectile_ES", null), null);
            //harmonyInstance.Patch(AccessTools.Method(typeof(GenRadial), "get_MaxRadialPatternRadius", null, null), null, new HarmonyMethod(typeof(TorannMagicMod), "Get_MaxDrawRadius_Patch", null));

            harmonyInstance.Patch(AccessTools.Method(typeof(GenDraw), "DrawRadiusRing", new Type[]
                {
                    typeof(IntVec3),
                    typeof(float),
                    typeof(Color),
                    typeof(Func<IntVec3, bool>)
                }, null), new HarmonyMethod(typeof(TorannMagicMod), "DrawRadiusRing_Patch"), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Pawn_PathFollower), "CostToMoveIntoCell", new Type[]
                {
                    typeof(Pawn),
                    typeof(IntVec3)
                }, null), new HarmonyMethod(typeof(TorannMagicMod), "Pawn_PathFollower_Pathfinder_Prefix", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Pawn_StanceTracker), "StaggerFor", new Type[]
                {
                    typeof(int)
                }, null), new HarmonyMethod(typeof(TorannMagicMod), "StaggerFor_Patch", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(MemoryThoughtHandler), "TryGainMemory", new Type[]
                {
                    typeof(ThoughtDef),
                    typeof(Pawn)
                }, null), new HarmonyMethod(typeof(TorannMagicMod), "MemoryThoughtHandler_PreventDisturbedRest_Prefix", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(PawnRenderer), "RenderPawnInternal", new Type[]
                {
                    typeof(Vector3),
                    typeof(float),
                    typeof(bool),
                    typeof(Rot4),
                    typeof(Rot4),
                    typeof(RotDrawMode),
                    typeof(bool),
                    typeof(bool),
                    typeof(bool)
                }, null), new HarmonyMethod(typeof(TorannMagicMod), "PawnRenderer_UndeadInternal_Prefix", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(PawnRenderer), "RenderPawnAt", new Type[]
                {
                    typeof(Vector3),
                    typeof(RotDrawMode),
                    typeof(bool),
                    typeof(bool)
                }, null), new HarmonyMethod(typeof(TorannMagicMod), "PawnRenderer_Undead_Prefix", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(PawnRenderer), "RenderPawnAt", new Type[]
                {
                    typeof(Vector3),
                    typeof(RotDrawMode),
                    typeof(bool),
                    typeof(bool)
                }, null), new HarmonyMethod(typeof(TorannMagicMod), "PawnRenderer_Blur_Prefix", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(PawnDiedOrDownedThoughtsUtility), "AppendThoughts_Relations", new Type[]
                {
                    typeof(Pawn),
                    typeof(DamageInfo?),
                    typeof(PawnDiedOrDownedThoughtsKind),
                    typeof(List<IndividualThoughtToAdd>),
                    typeof(List<ThoughtToAddToAll>)
                }, null), new HarmonyMethod(typeof(TorannMagicMod), "AppendThoughts_Relations_PrefixPatch", null), null, null);
            harmonyInstance.Patch(AccessTools.Method(typeof(PawnDiedOrDownedThoughtsUtility), "AppendThoughts_ForHumanlike", new Type[]
                {
                    typeof(Pawn),
                    typeof(DamageInfo?),
                    typeof(PawnDiedOrDownedThoughtsKind),
                    typeof(List<IndividualThoughtToAdd>),
                    typeof(List<ThoughtToAddToAll>)
                }, null), new HarmonyMethod(typeof(TorannMagicMod), "AppendThoughts_ForHumanlike_PrefixPatch", null), null, null);
            harmonyInstance.Patch(AccessTools.Method(typeof(PawnDiedOrDownedThoughtsUtility), "TryGiveThoughts", new Type[]
                {
                    typeof(Pawn),
                    typeof(DamageInfo?),
                    typeof(PawnDiedOrDownedThoughtsKind)
                }, null), new HarmonyMethod(typeof(TorannMagicMod), "TryGiveThoughts_PrefixPatch", null), null, null);
            harmonyInstance.Patch(AccessTools.Method(typeof(DaysWorthOfFoodCalculator), "ApproxDaysWorthOfFood", new Type[]
                {
                    typeof(List<Pawn>),
                    typeof(List<ThingDefCount>),
                    typeof(int),
                    typeof(IgnorePawnsInventoryMode),
                    typeof(Faction),
                    typeof(WorldPath),
                    typeof(float),
                    typeof(int),
                    typeof(bool)
                }, null), null, new HarmonyMethod(typeof(TorannMagicMod), "DaysWorthOfFoodCalc_Undead_Postfix", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Targeter), "TargeterOnGUI", new Type[]
                {
                }, null), null, new HarmonyMethod(typeof(TorannMagicMod), "Targeter_Casting_Postfix", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(QuestPart_LendColonistsToFaction), "Enable", new Type[]
                {
                    typeof(SignalArgs)
                }, null), null, new HarmonyMethod(typeof(TorannMagicMod), "QuestPart_LendColonists_Enable_NoUndead"));
            harmonyInstance.Patch(AccessTools.Method(typeof(HealthUtility), "AdjustSeverity", new Type[]
                {
                    typeof(Pawn),
                    typeof(HediffDef),
                    typeof(float)
                }, null), new HarmonyMethod(typeof(TorannMagicMod), "HealthUtility_HeatCold_HediffGiverForUndead"), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(ThingFilter), "SetFromPreset", new Type[]
                {
                    typeof(StorageSettingsPreset)
                }, null), new HarmonyMethod(typeof(TorannMagicMod), "DefaultStorageSettings_IncludeMagicItems"), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(AreaManager), "AddStartingAreas", new Type[]
                {
                }, null), null, new HarmonyMethod(typeof(TorannMagicMod), "AreaManager_AddMagicZonesToStartingAreas"));
            //harmonyInstance.Patch(AccessTools.Method(typeof(QuestNode_RaceProperty), "Matches", new Type[]
            //    {
            //        typeof(object)
            //    }, null), null, new HarmonyMethod(typeof(TorannMagicMod), "QuestNode_RaceProperties_ExcludeSummoned"));

            //harmonyInstance.Patch(original: AccessTools.Method(type: typeof(Pawn_DraftController), name: "Notify_PrimaryWeaponChanged"), prefix: null,
            //    postfix: new HarmonyMethod(methodType: patchType, methodName: nameof(PawnEquipment_Change_Postfix)), transpiler: null);
            //harmonyInstance.Patch(original: AccessTools.Method(type: typeof(Pawn_EquipmentTracker), name: "TryTransferEquipmentToContainer"), prefix: null,
            //    postfix: new HarmonyMethod(methodType: patchType, methodName: nameof(PawnEquipment_Transfer_Postfix)), transpiler: null);
            //harmonyInstance.Patch(AccessTools.Method(typeof(Thing), "get_Suspended", null, null), new HarmonyMethod(typeof(TorannMagicMod), "Get_Suspended_Polymorphed", null), null);
            //harmonyInstance.Patch(AccessTools.Method(typeof(Toils_Recipe), "DoRecipeWork", new Type[]
            //    {
            //    }, null), new HarmonyMethod(typeof(TorannMagicMod), "DoMagicRecipeWork", null), null);
            //harmonyInstance.Patch(AccessTools.Method(typeof(CaravanArrivalTimeEstimator), "EstimatedTicksToArrive", new Type[]
            //    {
            //        typeof(int),
            //        typeof(int),
            //        typeof(WorldPath),
            //        typeof(float),
            //        typeof(int),
            //        typeof(int)
            //    }, null), null, new HarmonyMethod(typeof(TorannMagicMod), "EstimatedTicksToArrive_Wayfarer_Postfix", null), null);


            #region PrisonLabor
            {
                try
                {
                    ((Action)(() =>
                    {
                        if (ModCheck.Validate.PrisonLaborOutdated.IsInitialized())
                        {
                            harmonyInstance.Patch(AccessTools.Method(typeof(PrisonLabor.JobDriver_Mine_Tweak), "ResetTicksToPickHit"), null, new HarmonyMethod(typeof(TorannMagicMod), "TM_PrisonLabor_JobDriver_Mine_Tweak"));
                        }
                    }))();
                }
                catch (TypeLoadException) { }

                try
                {
                    ((Action)(() =>
                    {
                        if (ModCheck.Validate.PrisonLabor.IsInitialized())
                        {
                            harmonyInstance.Patch(AccessTools.Method(typeof(PrisonLabor.Tweaks.JobDriver_Mine_Tweak), "ResetTicksToPickHit"), null, new HarmonyMethod(typeof(TorannMagicMod), "TM_PrisonLabor_JobDriver_Mine_Tweak"));
                        }
                    }))();
                }
                catch (TypeLoadException) { }

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
                            harmonyInstance.Patch(AccessTools.Method(typeof(PawnUtility), "TrySpawnHatchedOrBornPawn"), null, new HarmonyMethod(typeof(TorannMagicMod), "TM_Children_TrySpawnHatchedOrBornPawn_Tweak"));
                        }
                    }))();
                }
                catch (TypeLoadException) { }
            }
            #endregion Children

        }

        [HarmonyPatch(typeof(Reward_Items), "InitFromValue", null)]
        public class Reward_Items_TMQuests_Patch
        {
            private static bool Prefix(Reward_Items __instance, float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
            {
                float num = 0f;
                if (parms.chosenPawnSignal != null && parms.chosenPawnSignal == "TM_ArcaneRewards")
                {
                    int value = Mathf.RoundToInt(rewardValue * Rand.Range(0.7f, 1.3f));
                    __instance.items.Clear();
                    __instance.items.AddRange(ItemCollectionGenerator_Internal_Arcane.Generate(value));                    
                    for (int i = 0; i < __instance.items.Count; i++)
                    {
                        num += __instance.items[i].MarketValue * (float)__instance.items[i].stackCount;
                    }
                    valueActuallyUsed = num;
                    return false;
                }
                valueActuallyUsed = num;
                return true;
            }
        }

        public static void AreaManager_AddMagicZonesToStartingAreas(AreaManager __instance)
        {
            TM_Calc.GetSpriteArea(__instance.map);
            TM_Calc.GetTransmutateArea(__instance.map);
            TM_Calc.GetSeedOfRegrowthArea(__instance.map);
        }

        public static bool DefaultStorageSettings_IncludeMagicItems(ThingFilter __instance, StorageSettingsPreset preset)
        {
            MethodBase SetAllow = AccessTools.Method(typeof(ThingFilter), "SetAllow", new Type[]
                {
                    typeof(ThingCategoryDef),
                    typeof(bool),
                    typeof(IEnumerable<ThingDef>),
                    typeof(IEnumerable<SpecialThingFilterDef>)
                }, null);
            if (preset == StorageSettingsPreset.DefaultStockpile)
            {
                SetAllow.Invoke(__instance, new object[]
                {
                    TorannMagicDefOf.TM_MagicItems,
                    true,
                    null,
                    null
                });
            }
            return true;
        }

        public static bool HealthUtility_HeatCold_HediffGiverForUndead(Pawn pawn, ref HediffDef hdDef, float sevOffset)
        {
            if (hdDef != null && hdDef == HediffDefOf.Heatstroke)
            {
                if (TM_Calc.IsUndead(pawn))
                {
                    hdDef = TorannMagicDefOf.TM_SlaggedHD;
                }
            }
            if (hdDef != null && hdDef == HediffDefOf.Hypothermia)
            {
                if (TM_Calc.IsUndead(pawn))
                {
                    hdDef = TorannMagicDefOf.TM_BrittleBonesHD;
                }
            }
            return true;
        }

        public static void QuestPart_LendColonists_Enable_NoUndead(QuestPart_LendColonistsToFaction __instance, ref SignalArgs receivedArgs)
        {
            //MethodBase Complete = AccessTools.Method(typeof(QuestPart_LendColonistsToFaction), "Complete", null, null);

            if (__instance.LentColonistsListForReading != null && __instance.LentColonistsListForReading.Count > 0)
            {
                bool undeadSent = false;
                for(int i = 0; i < __instance.LentColonistsListForReading.Count; i++)
                {
                    Thing lentColonist = __instance.LentColonistsListForReading[i];
                    if(lentColonist is Pawn && ((Pawn)lentColonist).health != null && ((Pawn)lentColonist).health.hediffSet != null && (((Pawn)lentColonist).health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadHD) || ((Pawn)lentColonist).health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadAnimalHD)))
                    {
                        undeadSent = true;
                        break;
                    }
                }
                if(undeadSent)
                {
                    for (int i = 0; i < __instance.LentColonistsListForReading.Count; i++)
                    {
                        Thing t = __instance.LentColonistsListForReading[i];
                        GenPlace.TryPlaceThing(t, __instance.shuttle.Position, __instance.shuttle.Map, ThingPlaceMode.Near);
                        if(t is Pawn)
                        {
                            Pawn p = t as Pawn;
                            if(p.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadHD) || p.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadAnimalHD))
                            {
                                p.Kill(null, null);
                            }
                        }
                    }
                    Messages.Message("TM_LendColonist_UndeadFail".Translate(), MessageTypeDefOf.SilentInput, false);
                    __instance.quest.End(QuestEndOutcome.Fail, true);
                }
            }
        }

        //public static void QuestNode_RaceProperties_ExcludeSummoned(object obj, ref bool __result)
        //{
        //    Log.Message("triggered race properites Match object");
        //    if(obj != null)
        //    {
        //        if(obj is PawnKindDef)
        //        {

        //            Log.Message("object pkd is " + ((PawnKindDef)obj).defName);
        //        }
        //        if (obj is ThingDef)
        //        {

        //            Log.Message("object td is " + ((ThingDef)obj).defName);
        //        }
        //        if (obj is Pawn)
        //        {

        //            Log.Message("object pawn is " + ((Pawn)obj).def.defName);
        //        }
        //        if (obj is Faction)
        //        {
        //            Log.Message("object faction ");
        //        }
        //        if (obj is IEnumerable<Pawn>)
        //        {
        //            Log.Message("object ie pawn ");
        //        }
        //        if (obj is IEnumerable<Thing>)
        //        {
        //            Log.Message("object ie thing");
        //        }
        //        if (obj is IEnumerable<object>)
        //        {
        //            Log.Message("object ie object ");
        //            if (((IEnumerable<object>)obj).Any())
        //            {
        //                //foreach (ThingDef current in enumerable)
        //                foreach (object current in (IEnumerable<object>)obj)
        //                {
        //                    if(current is Pawn)
        //                    {
        //                        Log.Message("current obj is " + ((Pawn)current).def.defName);
        //                        if(((Pawn)current).def.defName.Contains("TM_"))
        //                        {
        //                            Log.Message("returning false");
        //                            __result = false;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        if (obj is string && !((string)obj).NullOrEmpty())
        //        {
        //            Log.Message("object string");
        //        }
        //    }
        //}


        public static void PawnEquipment_Drop_Postfix(Pawn_EquipmentTracker __instance, ThingWithComps eq, ref bool __result)
        {
            Pawn p = __instance.pawn;
            CompAbilityUserMight comp = p.TryGetComp<CompAbilityUserMight>();
            if (p != null && comp != null && p.story.traits.HasTrait(TorannMagicDefOf.TM_SuperSoldier) && comp.equipmentContainer != null && __result)
            {
                if (comp.equipmentContainer.Count > 0)
                {
                    for(int i =0; i < comp.equipmentContainer.Count; i++)
                    {
                        if(eq.def.label.Contains(comp.equipmentContainer[i].def.label))
                        {
                            //Log.Message("dropping specialized weapon base: " + eq.def.defName);
                            Thing outThing = null;
                            comp.equipmentContainer[i].HitPoints = eq.HitPoints;
                            comp.equipmentContainer.TryDrop(comp.equipmentContainer[i], p.Position, p.Map, ThingPlaceMode.Near, out outThing);
                            //eq.Destroy(DestroyMode.Vanish);
                            comp.equipmentContainer.Clear();
                        }
                    }                    
                }
            }
        }

        //public static void PawnEquipment_Transfer_Postfix(Pawn_EquipmentTracker __instance, ThingWithComps eq, ref bool __result)
        //{
        //    Pawn p = __instance.pawn;
        //    CompAbilityUserMight comp = p.TryGetComp<CompAbilityUserMight>();
        //    if (p != null && comp != null && p.story.traits.HasTrait(TorannMagicDefOf.TM_SuperSoldier) && __result)
        //    {

        //    }
        //}

        public static void PawnEquipment_Add_Postfix(Pawn_EquipmentTracker __instance, ThingWithComps newEq)
        {
            if (!newEq.def.defName.Contains("Spec_Base"))
            {
                Pawn p = __instance.pawn;
                CompAbilityUserMight comp = p.TryGetComp<CompAbilityUserMight>();
                if (p != null && comp != null && p.story.traits.HasTrait(TorannMagicDefOf.TM_SuperSoldier))
                {
                    if (comp.equipmentContainer == null)
                    {
                        comp.equipmentContainer = new ThingOwner<ThingWithComps>();
                        comp.equipmentContainer.Clear();
                    }

                    if (newEq == p.equipment.Primary)
                    {
                        //Log.Message("adding primary weapon");
                        if (comp.MightData.MightPowersSS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_PistolSpec).learned && TM_Calc.IsUsingPistol(p))
                        {
                            //Log.Message("weapon is pistol specialized: " + newEq.def.defName);                            
                            __instance.TryTransferEquipmentToContainer(newEq, comp.equipmentContainer);
                            TM_Action.DoAction_PistolSpecCopy(p, newEq);
                        }
                        else if (comp.MightData.MightPowersSS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_RifleSpec).learned && TM_Calc.IsUsingRifle(p))
                        {
                            //Log.Message("weapon is rifle specialized: " + newEq.def.defName);
                            __instance.TryTransferEquipmentToContainer(newEq, comp.equipmentContainer);
                            TM_Action.DoAction_RifleSpecCopy(p, newEq);
                        }
                        else if (comp.MightData.MightPowersSS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ShotgunSpec).learned && TM_Calc.IsUsingShotgun(p))
                        {
                            //Log.Message("weapon is shotgun specialized: " + newEq.def.defName);
                            __instance.TryTransferEquipmentToContainer(newEq, comp.equipmentContainer);
                            TM_Action.DoAction_ShotgunSpecCopy(p, newEq);
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Caravan_PathFollower), "CostToPayThisTick", null)]
        public class CostToPayThisTick_Base_Patch
        {
            private static void Postfix(Caravan_PathFollower __instance, ref float __result)
            {
                Caravan caravan = Traverse.Create(root: __instance).Field(name: "caravan").GetValue<Caravan>();
                if (caravan != null)
                {
                    IEnumerable<Pawn> pList = caravan.PlayerPawnsForStoryteller;
                    bool hasWayfarer = false;
                    if (pList != null && pList.Count() > 0)
                    {
                        foreach (Pawn p in pList)
                        {
                            if (p.story != null && p.story.traits != null && p.story.traits.HasTrait(TorannMagicDefOf.TM_Wayfarer))
                            {
                                hasWayfarer = true;
                                break;
                            }
                        }
                    }
                    if (hasWayfarer)
                    {
                        __result *= 1.25f;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Caravan_PathFollower), "CostToMove", new Type[]
        {
            typeof(Caravan),
            typeof(int),
            typeof(int),
            typeof(int?)
        })]
        public static class CostToMove_Caravan_Patch
        {
            [HarmonyPostfix]
            public static void CostToMove_Caravan_Postfix(Caravan_PathFollower __instance, Caravan caravan, int start, int end, ref int __result, int? ticksAbs = default(int?))
            {
                if (caravan != null)
                {
                    IEnumerable<Pawn> pList = caravan.PlayerPawnsForStoryteller;
                    bool hasWanderer = false;
                    if (pList != null && pList.Count() > 0)
                    {
                        foreach (Pawn p in pList)
                        {
                            if (p.story != null && p.story.traits != null && p.story.traits.HasTrait(TorannMagicDefOf.TM_Wanderer))
                            {
                                hasWanderer = true;
                                break;
                            }
                        }
                    }
                    if (hasWanderer)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        float num = WorldPathGrid.CalculatedMovementDifficultyAt(end, false, ticksAbs, stringBuilder);
                        __result = (int)(__result/num);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(ApparelUtility), "HasPartsToWear", null)]
        public class BracersOfPacifist_Wear_Prevention
        {
            public static void Postfix(Pawn p, ThingDef apparel, ref bool __result)
            {
                if(p != null && p.story != null && p.story.DisabledWorkTagsBackstoryAndTraits != WorkTags.Violent && apparel == TorannMagicDefOf.TM_Artifact_BracersOfThePacifist)
                {
                    __result = false;
                }
            }
        }

        [HarmonyPatch(typeof(WeatherEvent_LightningStrike), "FireEvent", null)]
        public class LightningStrike_DarkThunderstorm_Patch
        {
            public static bool Prefix(ref WeatherEvent_LightningStrike __instance)
            {
                //Log.Message("thunder strike");
                Map map = Traverse.Create(root: __instance).Field(name: "map").GetValue<Map>();
                if(map != null && map.GameConditionManager != null)
                {
                    //Log.Message("checking condition");
                    GameCondition_DarkThunderstorm gcdt = map.GameConditionManager.GetActiveCondition(TorannMagicDefOf.DarkThunderstorm) as GameCondition_DarkThunderstorm;
                    if(gcdt != null && gcdt.enemyPawns != null && gcdt.enemyPawns.Count > 0)
                    {
                        //Log.Message("setting strike loc to...");
                        Pawn e = gcdt.enemyPawns.RandomElement();
                        IntVec3 rndLoc = e.Position;
                        rndLoc.x += (Rand.Range(-2, 2));
                        rndLoc.z += (Rand.Range(-2, 2));
                        //Log.Message("setting rndloc " + rndLoc + " to strike loc...");
                        Traverse.Create(root: __instance).Field(name: "strikeLoc").SetValue(rndLoc);
                        //IntVec3 loc = Traverse.Create(root: __instance).Field(name: "strikeLoc").GetValue<IntVec3>();
                        //Log.Message("" + loc);
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(WeatherManager), "TransitionTo", null)]
        public class WeatherManager_RemoveDarkThunderstorm_Postfix
        {
            public static void Postfix(WeatherManager __instance, WeatherDef newWeather)
            {
                Map map = Traverse.Create(root: __instance).Field(name: "map").GetValue<Map>();
                if (map != null && map.GameConditionManager != null)
                {
                    GameCondition_DarkThunderstorm gcdt = map.GameConditionManager.GetActiveCondition(TorannMagicDefOf.DarkThunderstorm) as GameCondition_DarkThunderstorm;
                    if (gcdt != null)
                    {
                        //Log.Message("ending thunderstorm");
                        gcdt.End();
                    }
                }
            }
        }

        [HarmonyPatch(typeof(InteractionWorker), "Interacted", null)]
        public class GearRepair_InteractionWorker_Postfix
        {
            public static void Postfix(InteractionWorker __instance, Pawn initiator, Pawn recipient)
            {
                if (__instance.interaction == InteractionDefOf.Chitchat)
                {
                    if (initiator.story != null && initiator.story.traits != null && initiator.story.traits.HasTrait(TorannMagicDefOf.TM_Wayfarer))
                    {
                        if (initiator.health != null && initiator.health.hediffSet != null && initiator.health.hediffSet.HasHediff(TorannMagicDefOf.TM_HediffGearRepair))
                        {
                            CompAbilityUserMight comp = initiator.GetComp<CompAbilityUserMight>();
                            if (comp != null && comp.MightData.MightPowerSkill_FieldTraining.FirstOrDefault((MightPowerSkill x) => x.label == "TM_FieldTraining_ver").level >= 10)
                            {
                                if (recipient.equipment != null)
                                {
                                    Thing weapon = recipient.equipment.Primary;
                                    if (weapon != null && (weapon.def.IsRangedWeapon || weapon.def.IsMeleeWeapon))
                                    {
                                        if (weapon.HitPoints < weapon.MaxHitPoints)
                                        {
                                            weapon.HitPoints++;
                                        }
                                    }
                                }
                                if (recipient.apparel != null)
                                {
                                    List<Apparel> gear = recipient.apparel.WornApparel;
                                    for (int i = 0; i < gear.Count; i++)
                                    {
                                        if (gear[i].HitPoints < gear[i].MaxHitPoints)
                                        {
                                            gear[i].HitPoints++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (recipient.story != null && recipient.story.traits != null && recipient.story.traits.HasTrait(TorannMagicDefOf.TM_Wayfarer))
                    {
                        if (recipient.health != null && recipient.health.hediffSet != null && recipient.health.hediffSet.HasHediff(TorannMagicDefOf.TM_HediffGearRepair))
                        {
                            CompAbilityUserMight comp = recipient.GetComp<CompAbilityUserMight>();
                            if (comp != null && comp.MightData.MightPowerSkill_FieldTraining.FirstOrDefault((MightPowerSkill x) => x.label == "TM_FieldTraining_ver").level >= 10)
                            {
                                if (initiator.equipment != null)
                                {
                                    Thing weapon = initiator.equipment.Primary;
                                    if (weapon != null && (weapon.def.IsRangedWeapon || weapon.def.IsMeleeWeapon))
                                    {
                                        if (weapon.HitPoints < weapon.MaxHitPoints)
                                        {
                                            weapon.HitPoints++;
                                        }
                                    }
                                }
                                if (initiator.apparel != null)
                                {
                                    List<Apparel> gear = initiator.apparel.WornApparel;
                                    for (int i = 0; i < gear.Count; i++)
                                    {
                                        if (gear[i].HitPoints < gear[i].MaxHitPoints)
                                        {
                                            gear[i].HitPoints++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if(__instance.interaction == InteractionDefOf.DeepTalk)
                {
                    if (initiator.story != null && initiator.story.traits != null && initiator.story.traits.HasTrait(TorannMagicDefOf.TM_Wayfarer))
                    {
                        if (initiator.health != null && initiator.health.hediffSet != null && initiator.health.hediffSet.HasHediff(TorannMagicDefOf.TM_HediffGearRepair))
                        {
                            CompAbilityUserMight comp = initiator.GetComp<CompAbilityUserMight>();
                            if (comp != null && comp.MightData.MightPowerSkill_FieldTraining.FirstOrDefault((MightPowerSkill x) => x.label == "TM_FieldTraining_ver").level >= 10)
                            {
                                if (recipient.equipment != null)
                                {
                                    Thing weapon = recipient.equipment.Primary;
                                    if (weapon != null && (weapon.def.IsRangedWeapon || weapon.def.IsMeleeWeapon))
                                    {
                                        if (weapon.HitPoints < weapon.MaxHitPoints)
                                        {
                                            weapon.HitPoints++;
                                        }
                                        if (weapon.HitPoints < weapon.MaxHitPoints)
                                        {
                                            weapon.HitPoints++;
                                        }
                                        if (weapon.HitPoints < weapon.MaxHitPoints)
                                        {
                                            weapon.HitPoints++;
                                        }
                                    }
                                }
                                if (recipient.apparel != null)
                                {
                                    List<Apparel> gear = recipient.apparel.WornApparel;
                                    for (int i = 0; i < gear.Count; i++)
                                    {
                                        if (gear[i].HitPoints < gear[i].MaxHitPoints)
                                        {
                                            gear[i].HitPoints++;
                                        }
                                        if (gear[i].HitPoints < gear[i].MaxHitPoints)
                                        {
                                            gear[i].HitPoints++;
                                        }
                                        if (gear[i].HitPoints < gear[i].MaxHitPoints)
                                        {
                                            gear[i].HitPoints++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (recipient.story != null && recipient.story.traits != null && recipient.story.traits.HasTrait(TorannMagicDefOf.TM_Wayfarer))
                    {
                        if (recipient.health != null && recipient.health.hediffSet != null && recipient.health.hediffSet.HasHediff(TorannMagicDefOf.TM_HediffGearRepair))
                        {
                            CompAbilityUserMight comp = recipient.GetComp<CompAbilityUserMight>();
                            if (comp != null && comp.MightData.MightPowerSkill_FieldTraining.FirstOrDefault((MightPowerSkill x) => x.label == "TM_FieldTraining_ver").level >= 10)
                            {
                                if (initiator.equipment != null)
                                {
                                    Thing weapon = initiator.equipment.Primary;
                                    if (weapon != null && (weapon.def.IsRangedWeapon || weapon.def.IsMeleeWeapon))
                                    {
                                        if (weapon.HitPoints < weapon.MaxHitPoints)
                                        {
                                            weapon.HitPoints++;
                                        }
                                    }
                                }
                                if (initiator.apparel != null)
                                {
                                    List<Apparel> gear = initiator.apparel.WornApparel;
                                    for (int i = 0; i < gear.Count; i++)
                                    {
                                        if (gear[i].HitPoints < gear[i].MaxHitPoints)
                                        {
                                            gear[i].HitPoints++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //[HarmonyPatch(typeof(JobGiver_GetJoy), "TryGiveJob", null)]
        //public class Monk_GetJoyByMeditate_Joy_Patch
        //{
        //    public static void Postfix(JobGiver_GetJoy __instance, Pawn pawn, ref Job __result)
        //    {
        //        if (__result != null && pawn != null && pawn.health != null && pawn.health.hediffSet != null && pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_ChiHD, false))
        //        {
        //            Hediff chi = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_ChiHD);
        //            if(chi != null && chi.Severity < 70)
        //            {
        //                __result = new Job(TorannMagicDefOf.JobDriver_TM_Meditate, __result.targetA.Cell);
        //            }
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(JobGiver_Wander), "TryGiveJob", null)]
        //public class Monk_GetJoyByMeditate_Wander_Patch
        //{
        //    public static void Postfix(JobGiver_Wander __instance, Pawn pawn, ref Job __result)
        //    {
        //        if (__result != null && pawn != null && pawn.health != null && pawn.health.hediffSet != null && pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_ChiHD, false))
        //        {
        //            Hediff chi = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_ChiHD);
        //            if (chi != null && chi.Severity < 90)
        //            {
        //                __result = new Job(TorannMagicDefOf.JobDriver_TM_Meditate, __result.targetA.Cell);
        //            }
        //        }
        //    }
        //}

        [HarmonyPatch(typeof(Thing), "IDNumberFromThingID", null)]
        public class TechnoWeapon_ThingID
        {
            public static void Postfix(string thingID, ref int __result)
            {
                if(thingID.Contains("TM_TechnoWeapon") || thingID.Contains("Spec_Base"))
                {                    
                    __result = Rand.Range(0, 50000);
                    //Log.Message("changing thing id to " + __result);
                }
            }
        }

        [HarmonyPatch(typeof(Pawn), "VerifyReservations", null)]
        public class VerifyReservations_Prefix_Patch
        {
            public static void Prefix(Pawn __instance)
            {
                if (__instance.jobs != null && __instance.CurJob == null && __instance.jobs.jobQueue.Count <= 0 && !__instance.jobs.startingNewJob)
                {
                    List<Map> maps = Find.Maps;
                    for (int i = 0; i < maps.Count; i++)
                    {
                        IntVec3 obj3 = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationFor(__instance);
                        if (obj3.IsValid)
                        {
                            Job job = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationJobFor(__instance);
                            __instance.ClearAllReservations();
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(IncidentWorker), "TryExecute", null)]
        public class IncidentWorker_TryExecute_Prefix_Patch
        {
            public static bool Prefix(IncidentWorker __instance, ref IncidentParms parms, ref bool __result)
            {
                if (__instance != null && __instance.def.defName != "VisitorGroup" && __instance.def.defName != "VisitorGroupMax")
                {
                    try
                    {
                        List<Map> allMaps = Find.Maps;
                        if (allMaps != null && allMaps.Count > 0)
                        {
                            for (int i = 0; i < allMaps.Count; i++)
                            {
                                if (parms.target != null && allMaps[i].Tile == parms.target.Tile)
                                {
                                    List<Pawn> mapPawns = allMaps[i].mapPawns.AllPawnsSpawned;
                                    if (mapPawns != null && mapPawns.Count > 0)
                                    {
                                        for (int j = 0; j < mapPawns.Count; j++)
                                        {
                                            if (mapPawns[j].health != null && mapPawns[j].health.hediffSet != null && mapPawns[j].health.hediffSet.HasHediff(TorannMagicDefOf.TM_PredictionHD, false) && mapPawns[j].IsColonist)
                                            {
                                                CompAbilityUserMagic comp = mapPawns[j].GetComp<CompAbilityUserMagic>();
                                                if (comp != null && comp.MagicData != null)
                                                {
                                                    MagicPowerSkill ver = comp.MagicData.MagicPowerSkill_Prediction.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Prediction_ver");
                                                    if (comp.predictionIncidentDef != null)
                                                    {
                                                        try
                                                        {
                                                            //Log.Message("attempt to execute prediction " + comp.predictionIncidentDef.defName);
                                                            if (comp.predictionIncidentDef == __instance.def)
                                                            {
                                                                //Log.Message("executing prediction" + __instance.def.defName);
                                                                parms.forced = true;
                                                                comp.predictionIncidentDef = null;
                                                                return true;
                                                            }
                                                        }
                                                        catch (NullReferenceException ex)
                                                        {
                                                            parms.forced = true;
                                                            return true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                    
                                                        if (__instance.CanFireNow(parms, false) && !ModOptions.Constants.GetBypassPrediction() && Rand.Chance(.25f + (.05f * ver.level))) //up to 40% chance to predict, per chronomancer
                                                        {
                                                            if (__instance.def.category != null && (__instance.def.category == IncidentCategoryDefOf.ThreatBig || __instance.def.category == IncidentCategoryDefOf.ThreatSmall || __instance.def.category == IncidentCategoryDefOf.DeepDrillInfestation ||
                                                                __instance.def.category == IncidentCategoryDefOf.DiseaseAnimal || __instance.def.category == IncidentCategoryDefOf.DiseaseHuman || __instance.def.category == IncidentCategoryDefOf.Misc))
                                                            {
                                                                //Log.Message("prediction is " + __instance.def.defName + " and can fire now: " + __instance.CanFireNow(parms, false));
                                                                int ticksTillIncident = Mathf.RoundToInt((Rand.Range(1800, 3600) * (1 + (.15f * ver.level))));  // from .72 to 1.44 hours, plus bonus (1.05 - 2.1)
                                                                                                                                                                //Log.Message("pushing " + __instance.def.defName + " to iq for " + ticksTillIncident  + " ticks");
                                                                comp.predictionIncidentDef = __instance.def;
                                                                comp.predictionTick = Find.TickManager.TicksGame + ticksTillIncident;
                                                                QueuedIncident iq = new QueuedIncident(new FiringIncident(__instance.def, null, parms), comp.predictionTick);
                                                                Find.Storyteller.incidentQueue.Add(iq);
                                                                string labelText = "TM_PredictionLetter".Translate(__instance.def.label);
                                                                string text = "TM_PredictionText".Translate(mapPawns[j].LabelShort, __instance.def.label, Mathf.RoundToInt(ticksTillIncident / 2500));
                                                                //Log.Message("attempting to push letter");
                                                                Find.LetterStack.ReceiveLetter(labelText, text, LetterDefOf.NeutralEvent, null);
                                                                int xpNum = Rand.Range(60, 120);
                                                                comp.MagicUserXP += xpNum;
                                                                MoteMaker.ThrowText(comp.Pawn.DrawPos, comp.Pawn.Map, "XP +" + xpNum, -1f);
                                                                __result = true;
                                                                return false;
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (NullReferenceException ex)
                    {
                        return true;
                    }
                }
                return true;
            }
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

        [HarmonyPatch(typeof(HealthAIUtility), "ShouldSeekMedicalRest", null)]
        public class Arcane_ManaWeakness_BedRest_Patch
        {
            public static void Postfix(Pawn pawn, ref bool __result)
            {
                if (pawn.DestroyedOrNull() && pawn.health != null && pawn.health.hediffSet != null)
                {
                    if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_ArcaneWeakness, false))
                    {
                        Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_ArcaneWeakness);
                        if(hediff.Severity >= 10)
                        {
                            __result = true;
                        }
                    }
                    if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_ManaSickness, false))
                    {
                        Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_ManaSickness);
                        if (hediff.Severity >= 4)
                        {
                            __result = true;
                        }
                    }
                }
            }
        }

        //public static bool DaysWorthOfFoodCalc_Undead_Prefix(ref List<Pawn> pawns, List<ThingDefCount> extraFood, int tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction, ref float __result, WorldPath path = null, float nextTileCostLeft = 0f, int caravanTicksPerMove = 3300, bool assumeCaravanMoving = true)
        //{
        //    for(int i = 0; i < pawns.Count; i++)
        //    {
        //        if (TM_Calc.IsUndead(pawns[i]))
        //        {
        //            pawns.Remove(pawns[i]);
        //            i--;
        //        }
        //    }
        //    return true;
        //}

        public static void Targeter_Casting_Postfix(Targeter __instance)
        {
            if (__instance.targetingSource != null && __instance.targetingSource.CasterIsPawn)
            {
                Pawn caster = __instance.targetingSource.CasterPawn;
                if (caster != null)
                {
                    IntVec3 targ = UI.MouseMapPosition().ToIntVec3();
                    if(targ != null && __instance.targetingSource.GetVerb != null && __instance.targetingSource.GetVerb.EquipmentSource == null && __instance.targetingSource.GetVerb.loadID == null) // && __instance.targetingSource.GetVerb.EquipmentSource == null)
                    {
                        if ((caster.Position - targ).LengthHorizontal > __instance.targetingSource.GetVerb.verbProps.range)
                        {
                            //Log.Message("drawing icon " + __instance.targetingVerb.verbProps.range + " is less than target distance of " + (caster.Position - targ).LengthHorizontal);
                            Texture2D icon = TexCommand.CannotShoot; // TM_RenderQueue.losIcon;
                            GenUI.DrawMouseAttachment(icon);
                        }                        
                        if(__instance.targetingSource.GetVerb.verbProps.requireLineOfSight && !__instance.targetingSource.GetVerb.TryFindShootLineFromTo(caster.Position, targ, out ShootLine resultingLine))
                        {
                            Texture2D icon = TM_RenderQueue.losIcon;
                            GenUI.DrawMouseAttachment(icon);
                        }
                    }
                }
            }            
        }

        public static void DaysWorthOfFoodCalc_Undead_Postfix(List<Pawn> pawns, List<ThingDefCount> extraFood, int tile, IgnorePawnsInventoryMode ignoreInventory, Faction faction, ref float __result, WorldPath path = null, float nextTileCostLeft = 0f, int caravanTicksPerMove = 3300, bool assumeCaravanMoving = true)
        {
            if (pawns.Count != 0)
            {
                float undeadCount = 0;
                float undeadRatio = 0;
                for (int i = 0; i < pawns.Count; i++)
                {
                    if (TM_Calc.IsUndead(pawns[i]))
                    {
                        undeadCount++;
                    }
                }
                undeadRatio = undeadCount / pawns.Count;
                if (undeadRatio != 0)
                {
                    __result = __result / (1 - undeadRatio);
                }
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

        public static bool Get_IsColonist_Polymorphed(Pawn __instance, ref bool __result)
        {
            if (__instance.GetComp<CompPolymorph>() != null && __instance.GetComp<CompPolymorph>().Original != null && __instance.GetComp<CompPolymorph>().Original.RaceProps.Humanlike)
            {
                __result = __instance.Faction != null && __instance.Faction.IsPlayer;
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(FloatMenuMakerMap), "AddJobGiverWorkOrders", null)]
        public class SkipPolymorph_UndraftedOrders_Patch
        {
            public static bool Prefix(IntVec3 clickCell, Pawn pawn, List<FloatMenuOption> opts, bool drafted)
            {
                if (pawn.GetComp<CompPolymorph>() != null && pawn.GetComp<CompPolymorph>().Original != null)
                {
                    return false;
                }
                return true;
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

        [HarmonyPatch(typeof(GenDraw), "DrawMeshNowOrLater", null)]
        public class DrawMesh_Cloaks_Patch
        {
            public static bool Prefix(Mesh mesh, Vector3 loc, Quaternion quat, Material mat, bool drawNow)
            {
                if (mesh != null && loc != null && quat != null && mat != null)
                {
                    if (mat.mainTexture != null && mat.mainTexture.name != null && mat.mainTexture.ToString() != null && (mat.mainTexture.ToString().Contains("demonlordcloak") || mat.mainTexture.name.Contains("opencloak")))
                    {
                        //Log.Message("item is " + mat.mainTexture.ToString() + " at y: " + loc.y);
                        loc.y = 8.205f;
                        //loc.y += .010f; //was 0.015f
                        if (mat.name.ToString().Contains("_north"))
                        {
                            //loc.y += .00175f; //was 0.006f
                            loc.y = 8.209f; //7.9961f;
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
                    //if(mat.mainTexture != null && mat.mainTexture.name != null && loc.y > 7.9961f)
                    //{
                    //    Log.Message("thing: " + mat.mainTexture.name + " at loc.y:" + loc.y);
                    //}
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

        [HarmonyPatch(typeof(Pawn), "CheckAcceptArrest", null)]
        public class CheckArrest_Undead_Patch
        {
            public static bool Prefix(Pawn __instance, Pawn arrester, ref bool __result)
            {
                if (TM_Calc.IsUndead(__instance))
                {
                    __result = false;
                    return false;
                }
                return true;
            }
        }

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

        public static bool PawnRenderer_Blur_Prefix(PawnRenderer __instance, ref Vector3 drawLoc, ref RotDrawMode bodyDrawType, bool headStump, bool invisible)
        {
            Pawn pawn = Traverse.Create(root: __instance).Field(name: "pawn").GetValue<Pawn>();
            if (!pawn.DestroyedOrNull() && !pawn.Dead && !pawn.Downed)
            {
                if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_BlurHD))
                {
                    int blurTick = 0;
                    try
                    {
                        blurTick = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_BlurHD).TryGetComp<HediffComp_Blur>().blurTick;
                    }
                    catch(NullReferenceException ex)
                    {
                        return true;
                    }
                    if (blurTick > Find.TickManager.TicksGame - 10)
                    {
                        float blurMagnitude = (10 / (Find.TickManager.TicksGame - blurTick + 1)) + 5f;
                        Vector3 blurLoc = drawLoc;
                        blurLoc.x += Rand.Range(-.03f, .03f) * blurMagnitude;
                        //blurLoc.z += Rand.Range(-.01f, .01f) * blurMagnitude;
                        drawLoc = blurLoc;
                    }
                }

                if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PredictionHD))
                {
                    int blurTick = 0;
                    try
                    {
                        blurTick = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_PredictionHD).TryGetComp<HediffComp_Prediction>().blurTick;
                    }
                    catch(NullReferenceException ex)
                    {
                        return true;
                    }
                    if (blurTick > Find.TickManager.TicksGame - 10)
                    {
                        float blurMagnitude = (10 / (Find.TickManager.TicksGame - blurTick + 1)) + 5f;
                        Vector3 blurLoc = drawLoc;
                        blurLoc.x += Rand.Range(-.03f, .03f) * blurMagnitude;
                        //blurLoc.z += Rand.Range(-.01f, .01f) * blurMagnitude;
                        drawLoc = blurLoc;
                    }
                }

                if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_InvisibilityHD))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool PawnRenderer_Undead_Prefix(PawnRenderer __instance, Vector3 drawLoc, ref RotDrawMode bodyDrawType, bool headStump, bool invisible)
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

        public static bool PawnRenderer_UndeadInternal_Prefix(PawnRenderer __instance, Vector3 rootLoc, float angle, bool renderBody, Rot4 bodyFacing, Rot4 headFacing, ref RotDrawMode bodyDrawType, bool portrait, bool headStump, bool invisible)
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
                List<Pawn> mapPawns = ModOptions.Constants.GetOverdrivePawnList();
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    Pawn pawn = mapPawns[i];
                    if (!pawn.DestroyedOrNull() && pawn.RaceProps.Humanlike && pawn.story != null)
                    {
                        CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
                        if (comp.IsMagicUser && comp.overdriveBuilding != null)
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
                List<Pawn> mapPawns = ModOptions.Constants.GetOverdrivePawnList();
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    Pawn pawn = mapPawns[i];
                    if (!pawn.DestroyedOrNull() && pawn.RaceProps.Humanlike && pawn.story != null)
                    {
                        CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
                        if (comp.IsMagicUser && comp.overdriveBuilding != null)
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

        //public static void Get_MaxDrawRadius_Patch(ref float __result)
        //{
        //    __result = 250f;
        //}

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

        [HarmonyPriority(10)]
        public static void TM_Children_TrySpawnHatchedOrBornPawn_Tweak(ref Pawn pawn, Thing motherOrEgg, ref bool __result)
        {
            if (pawn.story != null && pawn.story.traits != null)
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
                        pawnTraits[i].def == TorannMagicDefOf.Technomancer || pawnTraits[i].def == TorannMagicDefOf.BloodMage || pawnTraits[i].def == TorannMagicDefOf.Enchanter || pawnTraits[i].def == TorannMagicDefOf.Chronomancer ||
                        pawnTraits[i].def == TorannMagicDefOf.TM_Wanderer || pawnTraits[i].def == TorannMagicDefOf.ChaosMage)
                    {
                        pawnTraits.Remove(pawnTraits[i]);
                        i--;
                        hasMagicTrait = true;
                    }
                    if (pawnTraits[i].def == TorannMagicDefOf.Gladiator || pawnTraits[i].def == TorannMagicDefOf.Bladedancer || pawnTraits[i].def == TorannMagicDefOf.TM_Sniper || pawnTraits[i].def == TorannMagicDefOf.Ranger ||
                        pawnTraits[i].def == TorannMagicDefOf.TM_Psionic || pawnTraits[i].def == TorannMagicDefOf.Faceless || pawnTraits[i].def == TorannMagicDefOf.DeathKnight || pawnTraits[i].def == TorannMagicDefOf.PhysicalProdigy ||
                        pawnTraits[i].def == TorannMagicDefOf.TM_Monk || pawnTraits[i].def == TorannMagicDefOf.TM_Wayfarer || pawnTraits[i].def == TorannMagicDefOf.TM_Commander || pawnTraits[i].def == TorannMagicDefOf.TM_SuperSoldier)
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
            if (__instance.pawn.def == TorannMagicDefOf.TM_DemonR)
            {
                __result = false;
                return false;
            }
            if(__instance.pawn.health != null && __instance.pawn.health.hediffSet != null && (__instance.pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_BurningFuryHD) || __instance.pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_MoveOutHD)))
            {
                __result = false;
                return false;
            }
            return true;
        }

        public static bool StaggerFor_Patch(Pawn_StanceTracker __instance, int ticks)
        {
            if (__instance.pawn.def == TorannMagicDefOf.TM_DemonR)
            {
                return false;
            }
            if (__instance.pawn.health != null && __instance.pawn.health.hediffSet != null && __instance.pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_BurningFuryHD))
            {
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
                if(comp != null && pawn.RaceProps.Humanlike && pawn.GetPosture() == PawnPosture.Standing && comp.HasTechnoWeapon && (pawn.story != null && pawn.story.traits != null && (pawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.ChaosMage))) && comp.useElementalShotToggle && pawn.equipment.Primary.def.IsRangedWeapon && pawn.equipment.Primary.def.techLevel >= TechLevel.Industrial)
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

        [HarmonyPriority(2000)]
        public static void Pawn_Gizmo_ActionPatch(ref IEnumerable<Gizmo> __result, ref Pawn __instance)
        {
            if (Find.Selector.NumSelected == 1)
            {
                if (__instance == null || !__instance.RaceProps.Humanlike)
                {
                    return;
                }
                if ((__instance.Faction != null && !__instance.Faction.Equals(Faction.OfPlayer)) || __instance.story == null || __instance.story.traits == null || __instance.story.traits.allTraits.Count < 1)
                {
                    return;
                }
                if (__instance.IsColonist)
                {
                    ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();

                    var gizmoList = __result.ToList();
                    if (settingsRef.Wanderer && __instance.story.traits.HasTrait(TorannMagicDefOf.Gifted))
                    {
                        Pawn p = __instance;
                        Command_Action itemWanderer = new Command_Action
                        {
                            action = new Action(delegate
                            {
                                TM_Action.PromoteWanderer(p);
                            }),
                            order = 51,
                            defaultLabel = TM_TextPool.TM_PromoteWanderer,
                            defaultDesc = TM_TextPool.TM_PromoteWandererDesc,
                            icon = ContentFinder<Texture2D>.Get("UI/wanderer", true),
                        };
                        gizmoList.Add(itemWanderer);
                    }
                    if (settingsRef.Wayfarer && __instance.story.traits.HasTrait(TorannMagicDefOf.PhysicalProdigy))
                    {
                        Pawn p = __instance;
                        Command_Action itemWayfarer = new Command_Action
                        {

                            action = new Action(delegate
                            {
                                TM_Action.PromoteWayfarer(p);
                            }),
                            order = 52,
                            defaultLabel = TM_TextPool.TM_PromoteWayfarer,
                            defaultDesc = TM_TextPool.TM_PromoteWayfarerDesc,
                            icon = ContentFinder<Texture2D>.Get("UI/wayfarer", true),
                        };
                        gizmoList.Add(itemWayfarer);
                    }
                    __result = gizmoList;
                }
            }
        }

        public static void Pawn_Gizmo_TogglePatch(ref IEnumerable<Gizmo> __result, ref Pawn __instance)
        {
            if (__instance == null || !__instance.RaceProps.Humanlike)
            {
                return;
            }            
            if ((__instance.Faction != null && !__instance.Faction.Equals(Faction.OfPlayer)) || __instance.story == null || __instance.story.traits == null || __instance.story.traits.allTraits.Count < 1)
            {
                return;
            }
            if (__result == null || !__result.Any())
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
                    Enchantment.CompEnchantedItem itemComp = null;
                    if(__instance.apparel != null && __instance.apparel.WornApparel != null)
                    {
                        for(int i = 0; i < __instance.apparel.WornApparel.Count; i++)
                        {
                            if(__instance.apparel.WornApparel[i].def == TorannMagicDefOf.TM_Artifact_NecroticOrb)
                            {
                                itemComp = __instance.apparel.WornApparel[i].TryGetComp<Enchantment.CompEnchantedItem>();
                            }
                        }
                    }
                    if (compMagic == null && compMight == null && itemComp == null)
                    {
                        return;
                    }
                    if (!compMagic.IsMagicUser && !compMight.IsMightUser && itemComp == null)
                    {
                        return;
                    }

                    Gizmo_EnergyStatus energyGizmo = new Gizmo_EnergyStatus
                    {
                        //All gizmo properties done in Gizmo_EnergyStatus
                        //Make it the first thing you see
                        pawn = __instance,
                        iComp = itemComp,
                        order = -101f
                    };

                    gizmoList.Add(energyGizmo);

                }
                if (__instance.story.traits.HasTrait(TorannMagicDefOf.Gladiator))
                {
                    String toggle = "cleave";
                    String label = "TM_CleaveEnabled".Translate();
                    String desc = "TM_CleaveToggleDesc".Translate();
                    if (!compMight.useCleaveToggle)
                    {
                        toggle = "cleavetoggle_off";
                        label = "TM_CleaveDisabled".Translate();
                    }
                    Command_Toggle item = new Command_Toggle
                    {
                        defaultLabel = label,
                        defaultDesc = desc,
                        order = -90,
                        icon = ContentFinder<Texture2D>.Get("UI/" + toggle, true),
                        isActive = (() => compMight.useCleaveToggle),
                        toggleAction = delegate
                        {
                            compMight.useCleaveToggle = !compMight.useCleaveToggle;
                        }
                    };
                    gizmoList.Add(item);
                }
                if (__instance.story.traits.HasTrait(TorannMagicDefOf.TM_SuperSoldier))
                {
                    String toggle = "cqc";
                    String label = "TM_CQCEnabled".Translate();
                    String desc = "TM_CQCToggleDesc".Translate();
                    if (!compMight.useCleaveToggle)
                    {
                        //toggle = "cqc_off";
                        label = "TM_CQCDisabled".Translate();
                    }
                    Command_Toggle item = new Command_Toggle
                    {
                        defaultLabel = label,
                        defaultDesc = desc,
                        order = -90,
                        icon = ContentFinder<Texture2D>.Get("UI/" + toggle, true),
                        isActive = (() => compMight.useCleaveToggle),
                        toggleAction = delegate
                        {
                            compMight.useCleaveToggle = !compMight.useCleaveToggle;
                        }
                    };
                    gizmoList.Add(item);
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

                    String toggle2 = "psionicmindattack";
                    String label2 = "TM_MindAttackEnabled".Translate();
                    String desc2 = "TM_MindAttackToggleDesc".Translate();
                    if (!compMight.usePsionicMindAttackToggle)
                    {
                        toggle2 = "psionicmindattack_off";
                        label2 = "TM_MindAttackDisabled".Translate();
                    }
                    Command_Toggle item2 = new Command_Toggle
                    {
                        defaultLabel = label2,
                        defaultDesc = desc2,
                        order = -89,
                        icon = ContentFinder<Texture2D>.Get("UI/" + toggle2, true),
                        isActive = (() => compMight.usePsionicMindAttackToggle),
                        toggleAction = delegate
                        {
                            compMight.usePsionicMindAttackToggle = !compMight.usePsionicMindAttackToggle;
                        }
                    };
                    gizmoList.Add(item2);
                }
                if ((__instance.story.traits.HasTrait(TorannMagicDefOf.Technomancer) || __instance.story.traits.HasTrait(TorannMagicDefOf.ChaosMage)) && compMagic.HasTechnoBit)
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

                if ((__instance.story.traits.HasTrait(TorannMagicDefOf.Technomancer) || __instance.story.traits.HasTrait(TorannMagicDefOf.ChaosMage)) && compMagic.HasTechnoWeapon)
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

        [HarmonyPatch(typeof(Pawn), "Kill", null)]
        public static class Undead_Kill_Prefix
        {
            public static bool Prefix(ref Pawn __instance)
            {
                if (__instance != null && __instance.health != null && (__instance.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadHD) || __instance.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadAnimalHD)))
                {
                    __instance.SetFaction(null, null);
                }

                return true;

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
                    CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
                    bool flagChrono = comp != null && comp.IsMagicUser && comp.recallSet;
                    if (flagChrono || (dinfo.Value.Def == TMDamageDefOf.DamageDefOf.TM_DisablingBlow || dinfo.Value.Def == TMDamageDefOf.DamageDefOf.TM_Whirlwind || dinfo.Value.Def == TMDamageDefOf.DamageDefOf.TM_GrapplingHook || dinfo.Value.Def == TMDamageDefOf.DamageDefOf.TM_DisablingShot || dinfo.Value.Def == TMDamageDefOf.DamageDefOf.TM_Tranquilizer) || TM_Calc.IsUndeadNotVamp(pawn))
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
                                    if(comp != null && comp.IsMagicUser && comp.recallSet)
                                    {
                                        int pwrVal = comp.MagicData.MagicPowerSkill_Recall.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Recall_pwr").level;
                                        if(pwrVal == 3 || (pwrVal >= 1 && Rand.Chance(.5f)))
                                        {
                                            TM_Action.DoRecall(pawn, comp, true);
                                            result = false;
                                            return false;
                                        }
                                    }
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
                                    if (comp != null && comp.IsMagicUser && comp.recallSet)
                                    {
                                        int pwrVal = comp.MagicData.MagicPowerSkill_Recall.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Recall_pwr").level;
                                        if (pwrVal == 3 || (pwrVal >= 2 && Rand.Chance(.5f)))
                                        {
                                            TM_Action.DoRecall(pawn, comp, false);
                                            result = false;
                                            return false;
                                        }
                                    }
                                    float num = (!pawn.RaceProps.Animal) ? 0f : 0f;
                                    bool flag7 = !__instance.forceIncap && dinfo.HasValue && dinfo.Value.Def.ExternalViolenceFor(pawn) && (pawn.Faction == null || !pawn.Faction.IsPlayer) && !pawn.IsPrisonerOfColony && pawn.RaceProps.IsFlesh && Rand.Value < num;
                                    if (flag7)
                                    {                                        
                                        pawn.Kill(dinfo, null);
                                        result = false;
                                        return result;
                                    }
                                    bool flagUndead = dinfo.HasValue && TM_Calc.IsUndeadNotVamp(pawn) && !pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_LichHD);
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
                        if (pawn.Downed && !pawn.Dead && !pawn.IsPrisoner)
                        {
                            if (pawn.Map == null)
                            {
                                Log.Warning("Tried to do death retaliation in a null map.");
                                return;
                            }
                            float chc = 1f * settingsRef.deathRetaliationChance;
                            if (Rand.Chance(chc))
                            {
                                CompAbilityUserMagic compMagic = pawn.GetComp<CompAbilityUserMagic>();
                                CompAbilityUserMight compMight = pawn.GetComp<CompAbilityUserMight>();
                                if (compMagic != null && compMagic.IsMagicUser)
                                {
                                    compMagic.canDeathRetaliate = true;                                    
                                }
                                else if(compMight != null && compMight.IsMightUser)
                                {
                                    compMight.canDeathRetaliate = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Hediff_Injury), "PostAdd", null)]
        public class Hediff_Injury_RemoveError_Prefix
        {
            public static bool Prefix(Hediff_Injury __instance, DamageInfo? dinfo)
            {
                if(__instance.Part != null && __instance.Part.coverageAbs <= 0f)
                {
                    __instance.PostAdd(dinfo);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(Pawn), "PreApplyDamage", null)]
        public class Pawn_PreApplyDamage
        {
            public static bool Prefix(Pawn __instance, ref DamageInfo dinfo, out bool absorbed)
            {
                Thing instigator = dinfo.Instigator as Thing;
                absorbed = false;
                if(instigator != null && !absorbed)
                {
                    if(__instance.health != null && __instance.health.hediffSet != null && __instance.health.hediffSet.HasHediff(TorannMagicDefOf.TM_BurningFuryHD, false))
                    {
                        dinfo.SetAmount(dinfo.Amount * 0.65f);
                    }
                    if (dinfo.Def != null && dinfo.Instigator != null && dinfo.Instigator.Map != null && dinfo.Instigator is Pawn)
                    {
                        Pawn monk = dinfo.Instigator as Pawn;
                        if (monk.health != null && monk.health.hediffSet != null && monk.health.hediffSet.HasHediff(TorannMagicDefOf.TM_MindOverBodyHD) && dinfo.Def == DamageDefOf.Blunt && dinfo.Weapon.defName == "Human")
                        {
                            Hediff hediff = monk.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_MindOverBodyHD);
                            dinfo.SetAmount(Mathf.RoundToInt(dinfo.Amount + hediff.Severity + Rand.Range(0f, 3f)));
                            dinfo.Def = TMDamageDefOf.DamageDefOf.TM_ChiFist;
                        }

                        Pawn ranger = dinfo.Instigator as Pawn;
                        if (ranger != null)
                        {
                            if (ranger.health.hediffSet.HasHediff(TorannMagicDefOf.TM_BowTrainingHD))
                            {
                                if (ranger.equipment.Primary != null)
                                {
                                    Thing wpn = ranger.equipment.Primary;
                                    if (wpn.def.IsRangedWeapon)
                                    {
                                        if (wpn.def.Verbs.FirstOrDefault<VerbProperties>().defaultProjectile.projectile.damageDef.defName == "Arrow" || wpn.def.defName.Contains("Bow") || wpn.def.defName.Contains("bow") || wpn.def.Verbs.FirstOrDefault<VerbProperties>().defaultProjectile.projectile.damageDef.defName.Contains("Arrow") || wpn.def.Verbs.FirstOrDefault<VerbProperties>().defaultProjectile.projectile.damageDef.defName.Contains("arrow"))
                                        {
                                            Hediff hediff = ranger.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_BowTrainingHD);
                                            float amt;
                                            if (hediff.Severity >= 1)
                                            {
                                                amt = dinfo.Amount * 1.4f;
                                            }
                                            else if (hediff.Severity >= 2)
                                            {
                                                amt = dinfo.Amount * 1.6f;
                                            }
                                            else if (hediff.Severity >= 3)
                                            {
                                                amt = dinfo.Amount * 1.8f;
                                            }
                                            else
                                            {
                                                amt = dinfo.Amount * 1.2f;
                                            }
                                            dinfo.SetAmount(amt);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }

        }

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
                    if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_HTLShieldHD, false))
                    {
                        HealthUtility.AdjustSeverity(pawn, TorannMagicDefOf.TM_HTLShieldHD, -dinfo.Amount);
                        TM_Action.DisplayShieldHit(pawn, dinfo);
                        absorbed = true;
                        return false;
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
                    if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_BlurHD, false) && !dinfo.Def.isExplosive)
                    {
                        float blurVal = .2f;
                        if (pawn.TryGetComp<CompAbilityUserMagic>()?.MagicData.MagicPowerSkill_Cantrips.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Cantrips_ver").level >= 11)
                        {
                            blurVal = .3f;
                        }
                        if (Rand.Chance(blurVal))
                        {
                            Hediff blur = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_BlurHD);
                            blur.TryGetComp<HediffComp_Blur>().blurTick = Find.TickManager.TicksGame;
                            absorbed = true;
                            return false;
                        }
                    }
                    if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PredictionHD, false) && !dinfo.Def.isExplosive)
                    {
                        Hediff prediction = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_PredictionHD);
                        if (Rand.Chance(prediction.Severity / 10f))
                        {
                            prediction.TryGetComp<HediffComp_Prediction>().blurTick = Find.TickManager.TicksGame;
                            absorbed = true;
                            return false;
                        }
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

                            TM_Action.DoAction_HealPawn(attacker, attacker, 1, dinfo.Amount * (.20f + .04f * lifestealPwr));
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
                                    TM_Action.DoAction_HealPawn(attacker, ally, 1, dinfo.Amount * (.20f * lifestealVer));
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
                            //Log.Message("undead taking damage");
                            if (dinfo.Def != null && dinfo.Def.armorCategory != null && dinfo.Def.armorCategory.defName == "Light" && Rand.Chance(.25f))
                            {
                                //Log.Message("taking light damage");
                                dinfo.SetAmount(dinfo.Amount * .7f);
                                pawn.TakeDamage(dinfo);
                            }
                        }

                        if (instigator != null && dinfo.Def != TMDamageDefOf.DamageDefOf.TM_Cleave && dinfo.Def != TMDamageDefOf.DamageDefOf.TM_DragonStrike && dinfo.Def != TMDamageDefOf.DamageDefOf.TM_ChiBurn && dinfo.Def != DamageDefOf.Stun && dinfo.Def != TMDamageDefOf.DamageDefOf.TM_CQC)
                        {
                            if (instigator.RaceProps.Humanlike && instigator.story != null)
                            {
                                //Log.Message("checking class bonus damage");
                                if (instigator.story.traits.HasTrait(TorannMagicDefOf.Gladiator) && instigator.equipment.Primary != null && instigator.equipment.Primary.def.IsMeleeWeapon)
                                {
                                    float cleaveChance = Mathf.Min(instigator.equipment.Primary.def.BaseMass * .4f, .75f);
                                    CompAbilityUserMight comp = instigator.GetComp<CompAbilityUserMight>();
                                    if (comp.useCleaveToggle && Rand.Chance(cleaveChance) && comp.Stamina.CurLevel >= comp.ActualStaminaCost(TorannMagicDefOf.TM_Cleave) && (pawn.Position - instigator.Position).LengthHorizontal <= 1.6f)
                                    {
                                        MightPowerSkill pwr = comp.MightData.MightPowerSkill_Cleave.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Cleave_pwr");
                                        MightPowerSkill str = comp.MightData.MightPowerSkill_global_strength.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_strength_pwr");
                                        MightPowerSkill ver = comp.MightData.MightPowerSkill_Cleave.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Cleave_ver");
                                        int dmgNum = Mathf.RoundToInt(dinfo.Amount * (.35f + (.05f * pwr.level)));
                                        DamageInfo dinfo2 = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_Cleave, dmgNum, 0, (float)-1, instigator, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
                                        Verb_Cleave.ApplyCleaveDamage(dinfo2, instigator, pawn, pawn.Map, ver.level);
                                        comp.Stamina.CurLevel -= comp.ActualStaminaCost(TorannMagicDefOf.TM_Cleave);
                                        comp.MightUserXP += Rand.Range(10, 15);
                                    }
                                }
                                if (instigator.story.traits.HasTrait(TorannMagicDefOf.TM_SuperSoldier))
                                {                                    
                                    CompAbilityUserMight comp = instigator.GetComp<CompAbilityUserMight>();
                                    if (comp != null && comp.useCleaveToggle && comp.Stamina.CurLevel >= comp.ActualStaminaCost(TorannMagicDefOf.TM_CQC) && (pawn.Position - instigator.Position).LengthHorizontal <= 1.6f)
                                    {
                                        MightPowerSkill pwr = comp.MightData.MightPowerSkill_CQC.FirstOrDefault((MightPowerSkill x) => x.label == "TM_CQC_pwr");
                                        MightPowerSkill str = comp.MightData.MightPowerSkill_global_strength.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_strength_pwr");
                                        int verVal = comp.MightData.MightPowerSkill_CQC.FirstOrDefault((MightPowerSkill x) => x.label == "TM_CQC_ver").level;
                                        float cqcChance = .2f;
                                        if(verVal == 1)
                                        {
                                            cqcChance = .25f;
                                        }
                                        else if(verVal == 2)
                                        {
                                            cqcChance = .28f;
                                        }
                                        else if( verVal == 3)
                                        {
                                            cqcChance = .3f;
                                        }                                        
                                        if (Rand.Chance(cqcChance))
                                        {
                                            int dmgNum = Mathf.RoundToInt(Rand.Range(10, 14)) + (2 * pwr.level);
                                            Vector3 strikeEndVec = pawn.DrawPos;
                                            strikeEndVec.x += Rand.Range(-.2f, .2f);
                                            strikeEndVec.z += Rand.Range(-.2f, .2f);
                                            Vector3 strikeStartVec = instigator.DrawPos;
                                            strikeStartVec.z += Rand.Range(-.2f, .2f);
                                            strikeStartVec.x += Rand.Range(-.2f, .2f);
                                            Vector3 angle = TM_Calc.GetVector(strikeStartVec, strikeEndVec);
                                            TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_CQC, strikeStartVec, instigator.Map, .35f, .08f, .03f, .05f, 0, 8f, (Quaternion.AngleAxis(90, Vector3.up) * angle).ToAngleFlat(), (Quaternion.AngleAxis(90, Vector3.up) * angle).ToAngleFlat());
                                            TM_Action.DamageEntities(pawn, dinfo.HitPart, dmgNum, TMDamageDefOf.DamageDefOf.TM_CQC, instigator);
                                            comp.Stamina.CurLevel -= comp.ActualStaminaCost(TorannMagicDefOf.TM_CQC);
                                            comp.MightUserXP += Rand.Range(10, 15);
                                        } 
                                    }
                                }
                            }

                            if(instigator.RaceProps.Humanlike && instigator.health.hediffSet.HasHediff(TorannMagicDefOf.TM_MindOverBodyHD) && instigator.equipment.Primary == null)
                            {
                                CompAbilityUserMight comp = instigator.GetComp<CompAbilityUserMight>();
                                MightPowerSkill ver = comp.MightData.MightPowerSkill_DragonStrike.FirstOrDefault((MightPowerSkill x) => x.label == "TM_DragonStrike_ver");
                                if (Rand.Chance(.3f + (.05f * ver.level)) && comp != null)
                                {
                                    MightPowerSkill pwr = comp.MightData.MightPowerSkill_DragonStrike.FirstOrDefault((MightPowerSkill x) => x.label == "TM_DragonStrike_pwr");
                                    MightPowerSkill str = comp.MightData.MightPowerSkill_global_strength.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_strength_pwr");
                                    int dmgNum = Mathf.RoundToInt(Rand.Range(6f, 10f) * (1 + (.1f * pwr.level) + (.05f * str.level)));
                                    DamageInfo dinfo2 = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_DragonStrike, dmgNum, 0, (float)-1, instigator, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
                                    TM_Action.DoAction_ApplySplashDamage(dinfo2, instigator, pawn, instigator.Map, 0);                                    
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

                        if(instigator != null && instigator.equipment != null && instigator.equipment.Primary != null && instigator.equipment.Primary.def.IsMeleeWeapon)
                        {
                            //Log.Message("checking instigator melee bonus ");
                            if (instigator.health.hediffSet.HasHediff(TorannMagicDefOf.TM_HediffFightersFocus) && Rand.Chance(.2f))
                            {
                                CompAbilityUserMight comp = instigator.GetComp<CompAbilityUserMight>();
                                if (comp != null && comp.MightData.MightPowerSkill_FieldTraining.FirstOrDefault((MightPowerSkill x) => x.label == "TM_FieldTraining_pwr").level >= 7)
                                {
                                    if (pawn.equipment != null && pawn.equipment.Primary != null && (pawn.equipment.Primary.def.IsRangedWeapon || pawn.equipment.Primary.def.IsMeleeWeapon))
                                    {
                                        ThingWithComps outThing = new ThingWithComps();
                                        pawn.equipment.TryDropEquipment(pawn.equipment.Primary, out outThing, pawn.Position, false);
                                        MoteMaker.ThrowText(pawn.DrawPos, pawn.MapHeld, "disarmed!", -1);
                                    }
                                }
                            }
                            if(pawn.health != null && pawn.health.hediffSet != null && pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_HediffThickSkin))
                            {
                                Hediff hd = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_HediffThickSkin);
                                if(hd.Severity >= 3)
                                {
                                    bool flagDmg = false;
                                    if (dinfo.WeaponBodyPartGroup != null)
                                    {
                                        List<BodyPartRecord> bpr = new List<BodyPartRecord>();
                                        bpr.Clear();
                                        bpr.Add(instigator.RaceProps.body.AllParts.FirstOrDefault<BodyPartRecord>((BodyPartRecord x) => x.def.tags.Contains(BodyPartTagDefOf.ManipulationLimbSegment)));
                                        bpr.Add(instigator.RaceProps.body.AllParts.FirstOrDefault<BodyPartRecord>((BodyPartRecord x) => x.def.tags.Contains(BodyPartTagDefOf.ManipulationLimbCore)));
                                        bpr.Add(instigator.RaceProps.body.AllParts.FirstOrDefault<BodyPartRecord>((BodyPartRecord x) => x.def.tags.Contains(BodyPartTagDefOf.ManipulationLimbDigit)));
                                        if (bpr != null && bpr.Count > 0)
                                        {
                                            TM_Action.DamageEntities(instigator, bpr.RandomElement(), Rand.Range(1f, 4f), DamageDefOf.Scratch, pawn);
                                            flagDmg = true;
                                        }
                                    }
                                    if(!flagDmg)
                                    {
                                        TM_Action.DamageEntities(instigator, null, Rand.Range(1f, 4f), DamageDefOf.Scratch, pawn);
                                    }
                                }
                            }
                        }

                        if (instigator != null)
                        {
                            //Log.Message("checking enchantment damage");
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

        [HarmonyPatch(typeof(Verb), "TryCastNextBurstShot", null)]
        public static class TryCastNextBurstShot_Monk_Patch
        {
            public static void Postfix(Verb __instance)
            {
                if (__instance.CasterIsPawn && __instance.CasterPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_MindOverBodyHD, false))
                {
                    LocalTargetInfo currentTarget = Traverse.Create(root: __instance).Field(name: "currentTarget").GetValue<LocalTargetInfo>();
                    int burstShotsLeft = Traverse.Create(root: __instance).Field(name: "burstShotsLeft").GetValue<int>();

                    if (__instance.CasterPawn.equipment.Primary == null && burstShotsLeft <= 0)
                    {
                        CompAbilityUserMight comp = __instance.CasterPawn.GetComp<CompAbilityUserMight>();
                        MightPowerSkill pwr = comp.MightData.MightPowerSkill_TigerStrike.FirstOrDefault((MightPowerSkill x) => x.label == "TM_TigerStrike_pwr");
                        MightPowerSkill eff = comp.MightData.MightPowerSkill_TigerStrike.FirstOrDefault((MightPowerSkill x) => x.label == "TM_TigerStrike_eff");
                        MightPowerSkill globalSkill = comp.MightData.MightPowerSkill_global_seff.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_seff_pwr");
                        float actualStaminaCost = .06f * (1 - (.1f * eff.level) * (1 - (.03f * globalSkill.level)));
                        if (comp != null && comp.Stamina.CurLevel >= actualStaminaCost && Rand.Chance(.3f + (.05f * pwr.level)))
                        {
                            Vector3 strikeEndVec = currentTarget.CenterVector3;
                            strikeEndVec.x += Rand.Range(-.2f, .2f);
                            strikeEndVec.z += Rand.Range(-.2f, .2f);
                            Vector3 strikeStartVec = __instance.CasterPawn.DrawPos;
                            strikeStartVec.z += Rand.Range(-.2f, .2f);
                            strikeStartVec.x += Rand.Range(-.2f, .2f);
                            Vector3 angle = TM_Calc.GetVector(strikeStartVec, strikeEndVec);
                            TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_Strike, strikeStartVec, __instance.CasterPawn.Map, .2f, .08f, .03f, .05f, 0, 8f, (Quaternion.AngleAxis(90, Vector3.up) * angle).ToAngleFlat(), (Quaternion.AngleAxis(90, Vector3.up) * angle).ToAngleFlat());
                            __instance.CasterPawn.stances.SetStance(new Stance_Cooldown(5, currentTarget, __instance));
                            comp.Stamina.CurLevel -= actualStaminaCost;
                            comp.MightUserXP += (int)(.06f * 180);
                        }
                    }
                }

                if (__instance.CasterIsPawn && __instance.CasterPawn.story != null && __instance.CasterPawn.story.traits != null && __instance.CasterPawn.story.traits.HasTrait(TorannMagicDefOf.TM_SuperSoldier))
                {
                    LocalTargetInfo currentTarget = Traverse.Create(root: __instance).Field(name: "currentTarget").GetValue<LocalTargetInfo>();
                    int burstShotsLeft = Traverse.Create(root: __instance).Field(name: "burstShotsLeft").GetValue<int>();
                    if (__instance.CasterPawn.equipment.Primary != null && burstShotsLeft <= 0)
                    {
                        CompAbilityUserMight comp = __instance.CasterPawn.TryGetComp<CompAbilityUserMight>();
                        if (comp.specWpnRegNum != -1 && comp.MightData.MightPowersSS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_PistolSpec).learned)
                        {
                            int doubleTapPwr = comp.MightData.MightPowerSkill_PistolSpec.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PistolSpec_eff").level;
                            MightPowerSkill globalSkill = comp.MightData.MightPowerSkill_global_seff.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_seff_pwr");
                            float actualStaminaCost = .03f * (1 - (.1f * doubleTapPwr) * (1 - (.03f * globalSkill.level)));
                            if (comp != null && comp.Stamina.CurLevel >= actualStaminaCost && Rand.Chance(.25f + (.05f * doubleTapPwr)))
                            {
                                __instance.CasterPawn.stances.SetStance(new Stance_Cooldown(5, currentTarget, __instance));
                                comp.Stamina.CurLevel -= actualStaminaCost;
                                comp.MightUserXP += (int)(.03f * 180);
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
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_BlankMind" ||
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_MechaniteReprogramming" ||
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

            private static bool Prefix(CastPositionRequest newReq, out IntVec3 dest) //, ref IntVec3 __result)
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
                    //__result = dest;
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
                if (thingDef != null && thingDef.thingClass != null && thingDef.thingClass.ToString() == "TorannMagic.TMPawnSummoned")
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
                if (pawn.story.traits.HasTrait(TorannMagicDefOf.TM_SuperSoldier) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Commander) || pawn.story.traits.HasTrait(TorannMagicDefOf.ChaosMage) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Wayfarer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Wanderer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Chronomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Monk) || pawn.story.traits.HasTrait(TorannMagicDefOf.DeathKnight) || pawn.story.traits.HasTrait(TorannMagicDefOf.BloodMage) || pawn.story.traits.HasTrait(TorannMagicDefOf.Enchanter) || pawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Geomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic) || pawn.story.traits.HasTrait(TorannMagicDefOf.Warlock) || pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus) || pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless) || pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) || pawn.story.traits.HasTrait(TorannMagicDefOf.Priest) || pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator) || pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper) || pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Lich) || pawn.story.traits.HasTrait(TorannMagicDefOf.InnerFire) || pawn.story.traits.HasTrait(TorannMagicDefOf.StormBorn) || pawn.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
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
                if (pawn.story != null && (pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Commander) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_SuperSoldier) || pawn.story.traits.HasTrait(TorannMagicDefOf.ChaosMage) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Wayfarer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Wanderer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Chronomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Monk) || pawn.story.traits.HasTrait(TorannMagicDefOf.DeathKnight) || pawn.story.traits.HasTrait(TorannMagicDefOf.BloodMage) || pawn.story.traits.HasTrait(TorannMagicDefOf.Enchanter) || pawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Geomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic) || pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus) || pawn.story.traits.HasTrait(TorannMagicDefOf.Warlock) || pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless) || pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) || pawn.story.traits.HasTrait(TorannMagicDefOf.Priest) || pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator) || pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper) || pawn.story.traits.HasTrait(TorannMagicDefOf.Druid) || pawn.story.traits.HasTrait(TorannMagicDefOf.Paladin) || pawn.story.traits.HasTrait(TorannMagicDefOf.Arcanist) || pawn.story.traits.HasTrait(TorannMagicDefOf.Summoner) || pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Lich) || pawn.story.traits.HasTrait(TorannMagicDefOf.InnerFire) || pawn.story.traits.HasTrait(TorannMagicDefOf.StormBorn) || pawn.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost)))
                {
                    ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                    if (!settingsRef.AICasting)
                    {
                        __result = false;
                        return false;
                    }
                    if(pawn.IsPrisoner || pawn.Downed)
                    {
                        __result = false;
                        return false;
                    }
                    bool hasThing = target != null && target.HasThing;
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
                            if (pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Commander) || pawn.story.traits.HasTrait(TorannMagicDefOf.Chronomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Monk) || pawn.story.traits.HasTrait(TorannMagicDefOf.DeathKnight) || pawn.story.traits.HasTrait(TorannMagicDefOf.BloodMage) || pawn.story.traits.HasTrait(TorannMagicDefOf.Enchanter) || pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) || pawn.story.traits.HasTrait(TorannMagicDefOf.Priest) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Bard) || pawn.story.traits.HasTrait(TorannMagicDefOf.Geomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless) || pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator) || pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Druid) || pawn.story.traits.HasTrait(TorannMagicDefOf.Summoner))
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
                            __result = true; //!corpse2.IsNotFresh();
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
                if (pawn != null && pawn.story != null && pawn.story.traits != null && pawn.story.traits.HasTrait(TorannMagicDefOf.Undead))
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

        //[HarmonyPatch(typeof(ShotReport), "HitReportFor", null)]
        //public static class ShotReport_Patch
        //{
        //    private static bool Prefix(Thing caster, Verb verb, LocalTargetInfo target, ref ShotReport __result)
        //    {
        //        if(false) //verb != null && verb.verbProps != null && verb.verbProps.verbClass != null && (verb.verbProps.verbClass.ToString() == "TorannMagic.Verb_SB" || verb.verbProps.verbClass.ToString() == "TorannMagic.Verb_BLOS"))
        //        {
        //            //ShotReport shotReport = default(ShotReport);
                    
        //            //__result = result;
        //            //return false;
        //        }
        //        return true;
        //    }
        //}

        [HarmonyPatch(typeof(LordToil_Siege), "CanBeBuilder", null)]
        public static class CanBeBuilder_Patch
        {
            private static bool Prefix(Pawn p, ref bool __result)
            {
                if (p?.def?.thingClass?.ToString() == "TorannMagic.TMPawnSummoned")
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
                if (victim?.def?.thingClass?.ToString() == "TorannMagic.TMPawnSummoned")
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
                int baseCount = 6;
                int mageCount = 18;
                int fighterCount = 11;
                int supportingFighterCount = 1;
                int supportingMageCount = 2;
                if (settingsRef.Gladiator || settingsRef.Bladedancer || settingsRef.Ranger || settingsRef.Sniper || settingsRef.Faceless || settingsRef.DeathKnight || settingsRef.Psionic || settingsRef.Monk || settingsRef.Wayfarer || settingsRef.Commander || settingsRef.SuperSoldier)
                {
                    anyFightersEnabled = true;
                }
                if (settingsRef.Arcanist || settingsRef.FireMage || settingsRef.IceMage || settingsRef.LitMage || settingsRef.Druid || settingsRef.Paladin || settingsRef.Summoner || settingsRef.Priest || settingsRef.Necromancer || settingsRef.Bard || settingsRef.Demonkin || settingsRef.Geomancer || settingsRef.Technomancer || settingsRef.BloodMage || settingsRef.Enchanter || settingsRef.Chronomancer || settingsRef.Wanderer || settingsRef.ChaosMage)
                {
                    anyMagesEnabled = true;
                }
                if (flag)
                {
                    if (ModCheck.Validate.AlienHumanoidRaces.IsInitialized())
                    {
                        if (Rand.Chance(((settingsRef.baseFighterChance * baseCount) + (settingsRef.baseMageChance * baseCount) + (fighterCount * settingsRef.advFighterChance) + (mageCount * settingsRef.advMageChance)) / (allTraits.Count)))
                        {
                            if (pawnTraits.Count > 0)
                            {
                                pawnTraits.Remove(pawnTraits[pawnTraits.Count - 1]);
                            }
                            float rnd = Rand.Range(0, baseCount * (settingsRef.baseFighterChance + settingsRef.baseMageChance) + (fighterCount * settingsRef.advFighterChance) + (mageCount * settingsRef.advMageChance));
                            if (rnd < (baseCount * settingsRef.baseMageChance) && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Gifted, 2)) && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Gifted.ToString()))
                            {
                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Gifted, 2, false));
                            }
                            else if (rnd >= baseCount * settingsRef.baseMageChance && rnd < (baseCount * (settingsRef.baseFighterChance + settingsRef.baseMageChance)) && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.PhysicalProdigy, 2))   && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.PhysicalProdigy.ToString()))
                            {
                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.PhysicalProdigy, 2, false));
                            }
                            else if (rnd >= (baseCount * (settingsRef.baseFighterChance + settingsRef.baseMageChance)) && rnd < (baseCount * (settingsRef.baseFighterChance + settingsRef.baseMageChance) + (fighterCount * settingsRef.advFighterChance)))
                            {
                                if (anyFightersEnabled)
                                {
                                    int rndF = Rand.RangeInclusive(1, fighterCount);
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
                                            //    goto Monk;
                                            //}
                                            break;
                                        case 8:
                                        Monk:;
                                            if (settingsRef.Monk && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_Monk, 4)) && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.TM_Monk.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Monk, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Wayfarer;
                                            //}
                                            break;
                                        case 9:
                                        Wayfarer:;
                                            if (settingsRef.Wayfarer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_Wayfarer, 4)) && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.TM_Wayfarer.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Wayfarer, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Commander;
                                            //}
                                            break;
                                        case 10:
                                            Commander:;
                                            if (settingsRef.Wayfarer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_Commander, 4)) && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.TM_Commander.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Commander, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto SuperSoldier;
                                            //}
                                            break;
                                        case 11:
                                            SuperSoldier:;
                                            if (settingsRef.Wayfarer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_SuperSoldier, 4)) && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.TM_SuperSoldier.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_SuperSoldier, 4, false));
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
                                    int rndM = Rand.RangeInclusive(1, (mageCount+1));
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
                                            //    goto Chronomancer;
                                            //}
                                            break;
                                        case 17:
                                            Chronomancer:;
                                            if (settingsRef.Chronomancer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Chronomancer, 4)) && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.Chronomancer.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Chronomancer, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Wanderer;
                                            //}
                                            break;
                                        case 18:
                                            Wanderer:;
                                            if (settingsRef.Wanderer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_Wanderer, 4)) && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.TM_Wanderer.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Wanderer, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto ChaosMage;
                                            //}
                                            break;
                                        case 19:
                                            ChaosMage:;
                                            if (settingsRef.ChaosMage && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.ChaosMage, 4)) && ModCheck.AlienHumanoidRaces.TryGetBackstory_DisallowedTrait(pawn.def, pawn, TorannMagicDefOf.ChaosMage.ToString()))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.ChaosMage, 4, false));
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
                        if (Rand.Chance(((settingsRef.baseFighterChance * baseCount) + (settingsRef.baseMageChance * baseCount) + (fighterCount * settingsRef.advFighterChance) + (mageCount * settingsRef.advMageChance)) / (allTraits.Count)))
                        {
                            if (pawnTraits.Count > 0)
                            {
                                pawnTraits.Remove(pawnTraits[pawnTraits.Count - 1]);
                            }
                            float rnd = Rand.Range(0, baseCount * (settingsRef.baseFighterChance + settingsRef.baseMageChance) + (fighterCount * settingsRef.advFighterChance) + (mageCount * settingsRef.advMageChance));
                            if (rnd < (baseCount * settingsRef.baseMageChance) && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Gifted, 2)))
                            {
                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Gifted, 2, false));
                            }
                            else if (rnd >= baseCount * settingsRef.baseMageChance && rnd < (baseCount * (settingsRef.baseFighterChance + settingsRef.baseMageChance)) && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.PhysicalProdigy, 2)))
                            {
                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.PhysicalProdigy, 2, false));
                            }
                            else if (rnd >= (baseCount * (settingsRef.baseFighterChance + settingsRef.baseMageChance)) && rnd < (baseCount * (settingsRef.baseFighterChance + settingsRef.baseMageChance) + (fighterCount * settingsRef.advFighterChance)))
                            {
                                if (anyFightersEnabled)
                                {
                                    int rndF = Rand.RangeInclusive(1, fighterCount);
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
                                            //    goto Monk;
                                            //}
                                            break;
                                        case 8:
                                            Monk:;
                                            if (settingsRef.Monk && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_Monk, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Monk, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Wayfarer;
                                            //}
                                            break;
                                        case 9:
                                            Wayfarer:;
                                            if (settingsRef.Wayfarer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_Wayfarer, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Wayfarer, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Commander;
                                            //}
                                            break;
                                        case 10:
                                            Commander:;
                                            if (settingsRef.Commander && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_Commander, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Commander, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto SuperSoldier;
                                            //}
                                            break;
                                        case 11:
                                            SuperSoldier:;
                                            if (settingsRef.SuperSoldier && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_SuperSoldier, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_SuperSoldier, 4, false));
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
                                    int rndM = Rand.RangeInclusive(1, (mageCount+1));
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
                                            //    goto Chronomancer;
                                            //}
                                            break;
                                        case 17:
                                            Chronomancer:;
                                            if (settingsRef.Chronomancer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.Chronomancer, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.Chronomancer, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto Wanderer;
                                            //}
                                            break;
                                        case 18:
                                            Wanderer:;
                                            if (settingsRef.Wanderer && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_Wanderer, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Wanderer, 4, false));
                                            }
                                            //else
                                            //{
                                            //    goto ChaosMage;
                                            //}
                                            break;
                                        case 19:
                                            ChaosMage:;
                                            if (settingsRef.ChaosMage && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.ChaosMage, 4)))
                                            {
                                                pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.ChaosMage, 4, false));
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

                    if (Rand.Chance(settingsRef.supportTraitChance))
                    {
                        if (TM_Calc.IsMagicUser(pawn) || pawn.story.traits.HasTrait(TorannMagicDefOf.Gifted))
                        {
                            int rndS = Rand.RangeInclusive(1, supportingMageCount);
                            switch (rndS)
                            {
                                case 1:
                                    if (settingsRef.ArcaneConduit && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_ArcaneConduitTD, 0)))
                                    {
                                        pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_ArcaneConduitTD, 0, false));
                                    }
                                    break;
                                case 2:
                                    if (settingsRef.ManaWell && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_ManaWellTD, 0)))
                                    {
                                        pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_ManaWellTD, 0, false));
                                    }
                                    break;
                            }                                
                        }
                        else if(TM_Calc.IsMightUser(pawn) || pawn.story.traits.HasTrait(TorannMagicDefOf.PhysicalProdigy))
                        {
                            int rndS = Rand.RangeInclusive(1, supportingFighterCount);
                            switch (rndS)
                            {
                                case 1:
                                    if (settingsRef.Boundless && !pawn.story.AllBackstories.Any(bs => bs.DisallowsTrait(TorannMagicDefOf.TM_BoundlessTD, 0)))
                                    {
                                        pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_BoundlessTD, 0, false));
                                    }
                                    break;
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

        [HarmonyPatch(typeof(JobGiver_Haul), "TryGiveJob", null)]
        public static class JobGiver_MinionHaul_Patch
        {
            public static bool Prefix(Pawn pawn, ref Job __result)
            {
                if (pawn != null && (pawn.def == TorannMagicDefOf.TM_MinionR || pawn.def == TorannMagicDefOf.TM_GreaterMinionR))
                {
                    if(pawn.jobs != null && pawn.CurJob != null && (pawn.CurJob.def == JobDefOf.HaulToCell || pawn.CurJob.def == JobDefOf.HaulToContainer || pawn.CurJob.def == JobDefOf.HaulToTransporter))
                    {
                        __result = null;
                        return false;
                    }
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(AbilityUser.AbilityDef), "GetJob", null)]
        public static class AbilityDef_Patch
        {
            private static bool Prefix(AbilityUser.AbilityDef __instance, AbilityTargetCategory cat, LocalTargetInfo target, ref Job __result)
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
                    thing.stackCount = Rand.Range(9, 25);
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
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (!(list[i].ToString().Contains("label=Attack") || list[i].ToString().Contains("label=Melee attack") || list[i].ToString().Contains("Desc=Toggle") || list[i].ToString().Contains("label=Draft")))
                                {
                                    list.Remove(list[i]);
                                    i--;
                                }
                            }                          
                        }                       
                    }
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
                    if(!(pawn.story.traits.HasTrait(TorannMagicDefOf.Priest) || pawn.story.DisabledWorkTagsBackstoryAndTraits == WorkTags.Violent))
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
                if (pawn == null)
                {
                    return;
                }
                IntVec3 c = IntVec3.FromVector3(clickPos);
                Enchantment.CompEnchant comp = pawn.TryGetComp<Enchantment.CompEnchant>();
                CompAbilityUserMagic pawnComp = pawn.TryGetComp<CompAbilityUserMagic>();
                if (comp != null && pawnComp != null && pawnComp.IsMagicUser)
                {
                    if (comp.enchantingContainer == null)
                    {
                        Log.Warning($"Enchanting container is null for {pawn}, initializing.");
                        comp.enchantingContainer = new ThingOwner<Thing>();
                        //comp.enchantingContainer = new ThingOwner<Thing>(comp);
                    }
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
                        if (comp.enchantingContainer?.Count > 0)
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
                        else if ((current.def.IsApparel || current.def.IsWeapon || current.def.IsRangedWeapon) && comp.enchantingContainer?.Count > 0)
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

        [HarmonyPatch(typeof(FloatMenuMakerMap), "AddJobGiverWorkOrders", null)]
        public static class FloatMenuMakerMap_MagicJobGiver_Patch
        {
            public static void Postfix(IntVec3 clickCell, Pawn pawn, List<FloatMenuOption> opts, bool drafted)
            {
                JobGiver_Work jobGiver_Work = pawn.thinker.TryGetMainTreeThinkNode<JobGiver_Work>();
                if (jobGiver_Work != null)
                {
                    foreach (Thing item in pawn.Map.thingGrid.ThingsAt(clickCell))
                    {
                        if(item is Building && (item.def == TorannMagicDefOf.TableArcaneForge))
                        {
                            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
                            if(comp != null && comp.Mana != null && comp.Mana.CurLevel < .5f)
                            {
                                string text = null;
                                Action action = null;
                                text = "TM_InsufficientManaForJob".Translate((comp.Mana.CurLevel * 100).ToString("0.##"));
                                FloatMenuOption menuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, action), pawn, item);
                                if (!opts.Any((FloatMenuOption op) => op.Label == menuOption.Label))
                                {
                                    menuOption.Disabled = true;
                                    opts.Add(menuOption);                                    
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
            if(pawn.IsColonist || (pawn.Faction != null && pawn.Faction.IsPlayer))
            {
                __result = pawn.IsWorldPawn() && ThingOwnerUtility.AnyParentIs<ActiveDropPodInfo>(pawn);
                return false;
            }
            return true;
        }
        

        //[HarmonyPatch(typeof(PawnAbility), "PostAbilityAttempt", null)]
        //public class PawnAbility_Patch
        //{
        //    public static bool Prefix(PawnAbility __instance)
        //    {
        //        if (__instance.Def.defName.Contains("TM_"))
        //        {
        //            CompAbilityUserMagic comp = __instance.Pawn.GetComp<CompAbilityUserMagic>();
        //            CompAbilityUserMight mightComp = __instance.Pawn.GetComp<CompAbilityUserMight>();
        //            if (comp.IsMagicUser && !__instance.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
        //            {
        //                __instance.CooldownTicksLeft = Mathf.RoundToInt((float)__instance.MaxCastingTicks * comp.coolDown);
        //                if (!__instance.Pawn.IsColonist)
        //                {
        //                    __instance.CooldownTicksLeft = (int)(__instance.CooldownTicksLeft / 2f);
        //                }
        //            }
        //            else if (mightComp.IsMightUser)
        //            {
        //                __instance.CooldownTicksLeft = Mathf.RoundToInt((float)__instance.MaxCastingTicks * mightComp.coolDown);
        //            }
        //            return false;
        //        }
        //        return true;
        //    }
        //}

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
                    if (targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_I, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_II, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_III, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_InvisibilityHD, false))
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
                    if (targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_I, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_II, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_III, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_InvisibilityHD, false))
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
            public static void Postfix(ColonistBarColonistDrawer __instance, ref Rect rect, Pawn colonist)
            {
                if (!colonist.Dead)
                {                    
                    ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                    if (colonist.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD")))
                    {
                        float num = 20f * Find.ColonistBar.Scale * settingsRef.classIconSize;
                        Vector2 vector = new Vector2(rect.x + 1f, rect.yMin + 1f);
                        rect = new Rect(vector.x, vector.y, num, num);
                        GUI.DrawTexture(rect, TM_MatPool.Icon_Undead);
                        TooltipHandler.TipRegion(rect, "TM_Icon_Undead".Translate());
                        vector.x += num;
                        //rect = new Rect(vector.x, vector.y, num, num);
                    }
                    else if(settingsRef.showClassIconOnColonistBar && colonist.story != null)
                    {
                        float num = 20f * Find.ColonistBar.Scale * settingsRef.classIconSize;
                        Vector2 vector = new Vector2(rect.x + 1f, rect.yMin + 1f);
                        rect = new Rect(vector.x, vector.y, num, num);
                        if (colonist.story.traits.HasTrait(TorannMagicDefOf.InnerFire))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.fireIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.iceIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.StormBorn))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.lightningIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.Arcanist))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.arcanistIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.Paladin))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.paladinIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.Summoner))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.summonerIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.Druid))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.druidIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || colonist.story.traits.HasTrait(TorannMagicDefOf.Lich))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.necroIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.Priest))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.priestIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.TM_Bard))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.bardIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.Succubus) || colonist.story.traits.HasTrait(TorannMagicDefOf.Warlock))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.demonkinIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.Geomancer))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.earthIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.Technomancer))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.technoIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.BloodMage))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.bloodmageIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.Enchanter))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.enchanterIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.Chronomancer))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.chronoIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.Gladiator))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.gladiatorIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Fighter".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.sniperIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Fighter".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.Bladedancer))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.bladedancerIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Fighter".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.Ranger))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.rangerIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Fighter".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.facelessIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Fighter".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.psiIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Fighter".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.DeathKnight))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.deathknightIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Fighter".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.TM_Monk))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.monkIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Fighter".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.TM_Wanderer))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.wandererIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.TM_Wayfarer))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.wayfarerIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Fighter".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.ChaosMage))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.chaosIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Mage".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.TM_Commander))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.commanderIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Fighter".Translate());
                            vector.x += num;
                        }
                        else if (colonist.story.traits.HasTrait(TorannMagicDefOf.TM_SuperSoldier))
                        {
                            GUI.DrawTexture(rect, TM_MatPool.SSIcon);
                            TooltipHandler.TipRegion(rect, "TM_Icon_Fighter".Translate());
                            vector.x += num;
                        }
                        //rect = new Rect(vector.x, vector.y, num, num);
                    }
                }
                //return true;
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
                            if ((Find.TickManager.TicksGame - lastInteractionTime) > (3000 - (450 * pwr.level)))
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
                        if(pawn.health != null && pawn.health.hediffSet != null && pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_TaskMasterHD))
                        {
                            if((Find.TickManager.TicksGame - lastInteractionTime) < 30000)
                            {
                                InteractionsTrackerTick_Patch.wantsRandomInteract.SetValue(__instance, false);
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
                    if(nd == TorannMagicDefOf.TM_Travel)// && pawn.story != null && pawn.story.traits != null)
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
                        List<Thing> thingList = cellList[i].GetThingList(pawn.Map);
                        if (thingList != null && thingList.Count > 0)
                        {
                            for (int j = 0; j < thingList.Count; j++)
                            {
                                if (thingList[j] != null && thingList[j] is Building)
                                {
                                    Building bldg = thingList[j] as Building;
                                    if (bldg.def == TorannMagicDefOf.TableArcaneForge)
                                    {
                                        forgeNearby = true;
                                        break;
                                    }
                                }
                            }
                            if(forgeNearby)
                            {
                                break;
                            }
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
                        Text.Font = GameFont.Tiny;
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
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Entertain)
                        {
                            magicPower = comp.MagicData.MagicPowersB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Entertain);
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_EnchantedAura)
                        {
                            magicPower = comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchantedAura);
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_EnchantedBody)
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
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Prediction)
                        {
                            magicPower = comp.MagicData.MagicPowersC.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Prediction);
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
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Regenerate)
                        {
                            magicPower = comp.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Regenerate);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_CureDisease)
                        {
                            magicPower = comp.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_CureDisease);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Heal)
                        {
                            magicPower = comp.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Heal);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Shield)
                        {
                            magicPower = comp.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Shield_I)
                        {
                            magicPower = comp.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Shield_II)
                        {
                            magicPower = comp.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield_I);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Shield_III)
                        {
                            magicPower = comp.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield_II);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_AdvancedHeal)
                        {
                            magicPower = comp.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_AdvancedHeal);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_TransferMana)
                        {
                            magicPower = comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TransferMana);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_SiphonMana)
                        {
                            magicPower = comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SiphonMana);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_CauterizeWound)
                        {
                            magicPower = comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_CauterizeWound);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_SpellMending)
                        {
                            magicPower = comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SpellMending);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_TeachMagic)
                        {
                            magicPower = comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TeachMagic);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_ShadowBolt)
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
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_ShadowBolt_I)
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
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_ShadowBolt_II)
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
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_ShadowBolt_III)
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
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Purify)
                        {
                            magicPower = comp.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Purify);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_SummonMinion)
                        {
                            magicPower = comp.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonMinion);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_MechaniteReprogramming)
                        {
                            magicPower = comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_MechaniteReprogramming);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_DirtDevil)
                        {
                            magicPower = comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_DirtDevil);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_ArcaneBolt)
                        {
                            magicPower = comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ArcaneBolt);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_TimeMark)
                        {
                            magicPower = comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TimeMark);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Transmutate)
                        {
                            magicPower = comp.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Transmutate);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                magicPower.AutoCast = !magicPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_RegrowLimb)
                        {
                            magicPower = comp.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_RegrowLimb);

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
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Grapple)
                        {
                            mightPower = mightComp.MightData.MightPowersG.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Grapple);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Grapple_I)
                        {
                            mightPower = mightComp.MightData.MightPowersG.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Grapple);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Grapple_II)
                        {
                            mightPower = mightComp.MightData.MightPowersG.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Grapple_I);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Grapple_III)
                        {
                            mightPower = mightComp.MightData.MightPowersG.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Grapple_II);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_BladeSpin)
                        {
                            mightPower = mightComp.MightData.MightPowersB.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_BladeSpin);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_PhaseStrike)
                        {
                            mightPower = mightComp.MightData.MightPowersB.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_PhaseStrike);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_PhaseStrike_I)
                        {
                            mightPower = mightComp.MightData.MightPowersB.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_PhaseStrike);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_PhaseStrike_II)
                        {
                            mightPower = mightComp.MightData.MightPowersB.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_PhaseStrike_I);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_PhaseStrike_III)
                        {
                            mightPower = mightComp.MightData.MightPowersB.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_PhaseStrike_II);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_ArrowStorm)
                        {
                            mightPower = mightComp.MightData.MightPowersR.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ArrowStorm);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_ArrowStorm_I)
                        {
                            mightPower = mightComp.MightData.MightPowersR.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ArrowStorm);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_ArrowStorm_II)
                        {
                            mightPower = mightComp.MightData.MightPowersR.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ArrowStorm_I);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_ArrowStorm_III)
                        {
                            mightPower = mightComp.MightData.MightPowersR.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ArrowStorm_II);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_DisablingShot)
                        {
                            mightPower = mightComp.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_DisablingShot);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_DisablingShot_I)
                        {
                            mightPower = mightComp.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_DisablingShot);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_DisablingShot_II)
                        {
                            mightPower = mightComp.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_DisablingShot_I);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_DisablingShot_III)
                        {
                            mightPower = mightComp.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_DisablingShot_II);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Spite)
                        {
                            mightPower = mightComp.MightData.MightPowersDK.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Spite);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Spite_I)
                        {
                            mightPower = mightComp.MightData.MightPowersDK.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Spite);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Spite_II)
                        {
                            mightPower = mightComp.MightData.MightPowersDK.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Spite_I);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Spite_III)
                        {
                            mightPower = mightComp.MightData.MightPowersDK.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Spite_II);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Headshot)
                        {
                            mightPower = mightComp.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Headshot);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_AntiArmor)
                        {
                            mightPower = mightComp.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_AntiArmor);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_ThrowingKnife)
                        {
                            mightPower = mightComp.MightData.MightPowersStandalone.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ThrowingKnife);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_PommelStrike)
                        {
                            mightPower = mightComp.MightData.MightPowersStandalone.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_PommelStrike);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_TempestStrike)
                        {
                            mightPower = mightComp.MightData.MightPowersStandalone.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_TempestStrike);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_TigerStrike)
                        {
                            mightPower = mightComp.MightData.MightPowersM.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_TigerStrike);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_ThunderStrike)
                        {
                            mightPower = mightComp.MightData.MightPowersM.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ThunderStrike);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_ProvisionerAura)
                        {
                            mightPower = mightComp.MightData.MightPowersC.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ProvisionerAura);
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_TaskMasterAura)
                        {
                            mightPower = mightComp.MightData.MightPowersC.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_TaskMasterAura);
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_CommanderAura)
                        {
                            mightPower = mightComp.MightData.MightPowersC.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_CommanderAura);
                        }
                        if(__instance.pawnAbility.Def == TorannMagicDefOf.TM_PistolWhip)
                        {
                            mightPower = mightComp.MightData.MightPowersSS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_PistolWhip);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_SuppressingFire)
                        {
                            mightPower = mightComp.MightData.MightPowersSS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_SuppressingFire);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Buckshot)
                        {
                            mightPower = mightComp.MightData.MightPowersSS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Buckshot);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_FirstAid)
                        {
                            mightPower = mightComp.MightData.MightPowersSS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_FirstAid);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        //if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_FightersFocus)
                        //{
                        //    mightPower = mightComp.MightData.MightPowersStandalone.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_FightersFocus);
                        //}
                        //if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_ThickSkin)
                        //{
                        //    mightPower = mightComp.MightData.MightPowersStandalone.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ThickSkin);
                        //}
                        //if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_StrongBack)
                        //{
                        //    mightPower = mightComp.MightData.MightPowersStandalone.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_StrongBack);
                        //}
                        //if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_HeavyBlow)
                        //{
                        //    mightPower = mightComp.MightData.MightPowersStandalone.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_HeavyBlow);
                        //}
                        //if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_InnerHealing)
                        //{
                        //    mightPower = mightComp.MightData.MightPowersStandalone.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_InnerHealing);
                        //}
                        //if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_GearRepair)
                        //{
                        //    mightPower = mightComp.MightData.MightPowersStandalone.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_GearRepair);
                        //}
                        //if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_BurningFury)
                        //{
                        //    mightPower = mightComp.MightData.MightPowersStandalone.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_BurningFury);
                        //}
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_Meditate)
                        {
                            mightPower = mightComp.MightData.MightPowersM.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Meditate);

                            if (Input.GetMouseButtonDown(1) && Mouse.IsOver(rect))
                            {
                                mightPower.AutoCast = !mightPower.AutoCast;
                                __result = new GizmoResult(GizmoState.Mouseover, null);
                                return false;
                            }
                        }
                        if (__instance.pawnAbility.Def == TorannMagicDefOf.TM_TeachMight)
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
                    if (magicPower != null && comp != null && comp.Mana != null)
                    {
                        //Rect rect = new Rect(topLeft.x, topLeft.y, __instance.GetWidth(maxWidth), 75f);
                        Rect position = new Rect(rect.x + rect.width - 24f, rect.y, 24f, 24f);
                        Texture2D image = (!magicPower.AutoCast) ? Widgets.CheckboxOffTex : Widgets.CheckboxOnTex;
                        GUI.DrawTexture(position, image);
                    }
                    if (mightPower != null && mightComp != null && mightComp.Stamina != null)
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
       
    }
}
