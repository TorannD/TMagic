﻿using Harmony;
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
using Verse.AI;
using AbilityUserAI;
using System.Reflection.Emit;
using RimQuest;

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
            //harmonyInstance.Patch(AccessTools.Method(typeof(Verb), "get_TerrainDefSource", null, null), new HarmonyMethod(typeof(HarmonyPatches), "Get_TerrainDefSource", null), null);
            //harmonyInstance.Patch(AccessTools.Method(typeof(Verb), "get_EquipmentSource", null, null), new HarmonyMethod(typeof(HarmonyPatches), "Get_EquipmentSource", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Caravan),"get_NightResting", null, null), new HarmonyMethod(typeof(HarmonyPatches), "Get_NightResting_Undead", null), null);
            harmonyInstance.Patch(AccessTools.Method(typeof(Pawn_StanceTracker), "get_Staggered", null, null), new HarmonyMethod(typeof(HarmonyPatches), "Get_Staggered", null), null);
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
            //harmonyInstance.Patch(AccessTools.Method(typeof(CaravanPawnsNeedsUtility), "TrySatisfyPawnNeeds", new Type[]
            //    {
            //        typeof(Pawn),
            //        typeof(Caravan)
            //    }, null), new HarmonyMethod(typeof(HarmonyPatches), "TrySatisfyPawnNeeds_PrefixPatch", null), null, null);
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

            #region PrisonLabor
            {
                try
                {
                    ((Action)(() =>
                    {
                        if (ModOptions.ModCompatibilityCheck.PrisonLaborIsActive)
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

            //#region RimQuest
            //{
            //    try
            //    {
            //        ((Action)(() =>
            //        {
            //            if (ModOptions.ModCompatibilityCheck.RimQuestIsActive)
            //            {
            //                harmonyInstance.Patch(AccessTools.Method(typeof(RimQuest.QuestPawn), "GenerateQuests"), new HarmonyMethod(typeof(HarmonyPatches), "TM_RimQuest_GenerateQuestsPrefix", null), null, null);
            //                //harmonyInstance.Patch(AccessTools.Method(typeof(RimQuest.QuestPawn), "GenerateQuests"), null, new HarmonyMethod(typeof(HarmonyPatches), "TM_RimQuest_GenerateQuestsPostfix"));
            //            }
            //        }))();
            //    }
            //    catch (TypeLoadException) { }
            //}
            //#endregion RimQuest

            #region Children
            {
                try
                {
                    ((Action)(() =>
                    {
                        if (ModOptions.ModCompatibilityCheck.ChildrenIsActive)
                        {
                            harmonyInstance.Patch(AccessTools.Method(typeof(PawnUtility), "TrySpawnHatchedOrBornPawn"), null, new HarmonyMethod(typeof(HarmonyPatches), "TM_Children_TrySpawnHatchedOrBornPawn_Tweak"));
                        }
                    }))();
                }
                catch (TypeLoadException) { }
            }
            #endregion Children           

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
            for(int i =0; i < tempList.Count(); i++)
            {
                if(tempList[i].def.thingClass.ToString() == "TorannMagic.TMPawnSummoned")
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
                        pawnTraits[i].def == TorannMagicDefOf.InnerFire || pawnTraits[i].def == TorannMagicDefOf.HeartOfFrost || pawnTraits[i].def == TorannMagicDefOf.StormBorn ||
                        pawnTraits[i].def == TorannMagicDefOf.Paladin || pawnTraits[i].def == TorannMagicDefOf.Summoner || pawnTraits[i].def == TorannMagicDefOf.Druid || pawnTraits[i].def == TorannMagicDefOf.Necromancer ||
                        pawnTraits[i].def == TorannMagicDefOf.Lich || pawnTraits[i].def == TorannMagicDefOf.Priest || pawnTraits[i].def == TorannMagicDefOf.TM_Bard || pawnTraits[i].def == TorannMagicDefOf.Gifted)
                    {
                        pawnTraits.Remove(pawnTraits[i]);
                        i--;
                        hasMagicTrait = true;
                    }
                    if (pawnTraits[i].def == TorannMagicDefOf.Gladiator || pawnTraits[i].def == TorannMagicDefOf.Bladedancer || pawnTraits[i].def == TorannMagicDefOf.TM_Sniper || pawnTraits[i].def == TorannMagicDefOf.Ranger ||
                        pawnTraits[i].def == TorannMagicDefOf.TM_Psionic || pawnTraits[i].def == TorannMagicDefOf.Faceless || pawnTraits[i].def == TorannMagicDefOf.PhysicalProdigy)
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
                        pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Gifted"), 2, false));
                    }
                    else
                    {
                        pawn.story.traits.GainTrait(new Trait(TraitDef.Named("PhysicalProdigy"), 2, false));
                    }
                }
                else if (hasFighterTrait)
                {
                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("PhysicalProdigy"), 2, false));
                }
                else if (hasMagicTrait)
                {
                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Gifted"), 2, false));
                }
            }
        }


        //public static bool TM_RimQuest_GenerateQuestsPrefix(QuestPawn __instance)
        //{
        //    bool hasArcaneQuests = false;
        //    if (__instance.questGiverDef != null && __instance.questGiverDef.quests != null)
        //    {
        //        try
        //        {
        //            for (int q = 0; q < __instance.questGiverDef.quests.Count; q++)
        //            {
        //                if (__instance.questGiverDef.quests[q].def == IncidentDef.Named("QuestArcaneCamp") || __instance.questGiverDef.quests[q].def == IncidentDef.Named("MagicCaravanRequest"))
        //                {
        //                    hasArcaneQuests = true;
        //                }
        //            }
        //            if (!hasArcaneQuests)
        //            {
        //                RimQuest.QuestGenOption arcaneCaravanOption = new RimQuest.QuestGenOption();
        //                RimQuest.QuestGenOption arcaneStashOption = new RimQuest.QuestGenOption();
        //                if (__instance.pawn.Faction.def == FactionDef.Named("Seers"))
        //                {
        //                    arcaneStashOption.def = IncidentDef.Named("QuestArcaneCamp");
        //                    arcaneStashOption.selectionWeight = 10;
        //                    arcaneCaravanOption.def = IncidentDef.Named("MagicCaravanRequest");
        //                    arcaneCaravanOption.selectionWeight = 6;
        //                    __instance.questGiverDef.quests.Add(arcaneCaravanOption);
        //                    __instance.questGiverDef.quests.Add(arcaneStashOption);
        //                }
        //                else if (__instance.pawn.Faction.def.techLevel == TechLevel.Medieval) //cares about magic a little
        //                {
        //                    arcaneStashOption.def = IncidentDef.Named("QuestArcaneCamp");
        //                    arcaneStashOption.selectionWeight = 3;
        //                    arcaneCaravanOption.def = IncidentDef.Named("MagicCaravanRequest");
        //                    arcaneCaravanOption.selectionWeight = 2;
        //                    __instance.questGiverDef.quests.Add(arcaneCaravanOption);
        //                    __instance.questGiverDef.quests.Add(arcaneStashOption);
        //                }
        //                else if (__instance.pawn.Faction.def.techLevel == TechLevel.Neolithic) //cares about magic
        //                {
        //                    arcaneStashOption.def = IncidentDef.Named("QuestArcaneCamp");
        //                    arcaneStashOption.selectionWeight = 6;
        //                    arcaneCaravanOption.def = IncidentDef.Named("MagicCaravanRequest");
        //                    arcaneCaravanOption.selectionWeight = 2;
        //                    __instance.questGiverDef.quests.Add(arcaneCaravanOption);
        //                    __instance.questGiverDef.quests.Add(arcaneStashOption);
        //                }
        //            }
        //        }
        //        catch
        //        {
        //            Log.Message("An exception occurred while adding A RimWorld of Magic quests to a RimQuest questgiver - ending attempt to allow default behavior.");
        //            return true;
        //        }
        //    }
        //    return true;
        //}

        public static bool Get_Staggered(Pawn_StanceTracker __instance, ref bool __result)
        {
            if(__instance.pawn.def.defName == "TM_DemonR")
            {
                __result = false;
                return false;
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

        [HarmonyPatch(typeof(Pawn), "GetGizmos")]
        public class Pawn_GetGizmos_Patch
        {
            public static void Postfix(ref IEnumerable<Gizmo> __result, ref Pawn __instance)
            {
                
                if (__instance == null ||  !__instance.RaceProps.Humanlike)
                {
                    return;
                }
                if (__result == null || !__result.Any())
                {
                    return;
                }
                if(!__instance.Faction.Equals(Faction.OfPlayer) || __instance.story == null || __instance.story.traits.allTraits.Count < 1)
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

                        var energyGizmo = new Gizmo_EnergyStatus
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
                        string toggle = "psionicaugmentation";
                        string label = "TM_AugmentationsEnabled".Translate();
                        string desc = "TM_AugmentationsToggleDesc".Translate();
                        if(!compMight.usePsionicAugmentationToggle)
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
                    __result = gizmoList;
                }
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
                    if (dinfo.Value.Def == TMDamageDefOf.DamageDefOf.TM_Whirlwind || dinfo.Value.Def == TMDamageDefOf.DamageDefOf.TM_GrapplingHook || dinfo.Value.Def == TMDamageDefOf.DamageDefOf.TM_DisablingShot || dinfo.Value.Def == TMDamageDefOf.DamageDefOf.TM_Tranquilizer)
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

                        if (pawn.story.traits.HasTrait(TorannMagicDefOf.Druid) || pawn.story.traits.HasTrait(TorannMagicDefOf.Paladin) || pawn.story.traits.HasTrait(TorannMagicDefOf.Arcanist) || pawn.story.traits.HasTrait(TorannMagicDefOf.Summoner) || pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Lich) || pawn.story.traits.HasTrait(TorannMagicDefOf.InnerFire) || pawn.story.traits.HasTrait(TorannMagicDefOf.StormBorn) || pawn.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
                        {


                            if (pawn.Downed && !pawn.Dead)
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
                                    Effecter deathEffect = TorannMagicDefOf.GiantExplosion.Spawn();
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
                                                if(settingsRef.deathExplosionMin > settingsRef.deathExplosionMax)
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

                                    if(pawn != null && !pawn.Dead)
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

        [HarmonyPatch(typeof(Pawn_HealthTracker), "PreApplyDamage", null)]
        public class PreApplyDamage_Patch
        {
            public static FieldInfo pawn = typeof(Pawn_HealthTracker).GetField("pawn", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
            public static bool Prefix(Pawn __instance, DamageInfo dinfo, out bool absorbed)
            {
                Traverse traverse = Traverse.Create(__instance);
                Pawn pawn = (Pawn)PreApplyDamage_Patch.pawn.GetValue(__instance);
                if (dinfo.Def != null && pawn != null)
                {
                    if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_HediffTimedInvulnerable")))
                    {
                        absorbed = true;
                        return false;
                    }
                    if(pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_StoneskinHD"), false))
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
                    if (dinfo.Instigator != null && !pawn.Dead)
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
                            
                            if(instigator.equipment.Primary == null && dinfo.Def != TMDamageDefOf.DamageDefOf.TM_PsionicInjury && dinfo.Def != DamageDefOf.Stun)
                            {
                                CompAbilityUserMight comp = instigator.GetComp<CompAbilityUserMight>();
                                MightPowerSkill pwr = comp.MightData.MightPowerSkill_PsionicAugmentation.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicAugmentation_pwr");
                                float dmgNum = dinfo.Amount;
                                float pawnDPS = instigator.GetStatValue(StatDefOf.MeleeDPS, false);
                                float psiEnergy = instigator.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_PsionicHD"), false).Severity;
                                if (psiEnergy > 20f && Rand.Chance(.3f + (.05f * pwr.level)) && !pawn.Downed)
                                {
                                    DamageInfo dinfo2 = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_PsionicInjury, (dmgNum + pawnDPS) + 2*pwr.level, dinfo.ArmorPenetrationInt, dinfo.Angle, instigator, dinfo.HitPart, dinfo.Weapon, dinfo.Category, dinfo.intendedTargetInt);
                                    TM_MoteMaker.MakePowerBeamMotePsionic(pawn.DrawPos.ToIntVec3(), pawn.Map, 2f, 2f, .7f, .1f, .6f);
                                    pawn.TakeDamage(dinfo2);
                                    HealthUtility.AdjustSeverity(instigator, HediffDef.Named("TM_PsionicHD"), -2f);
                                    comp.Stamina.CurLevel -= .02f;
                                    comp.MightUserXP += Rand.Range(2, 4);
                                    if (psiEnergy > 60f && Rand.Chance(.2f + (.03f * pwr.level)))
                                    {
                                        for (int i = 0; i < 6; i++)
                                        {
                                            float moteDirection = Rand.Range(0, 360);
                                            TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Psi"), instigator.DrawPos, instigator.Map, Rand.Range(.1f, .3f), 0.2f, .02f, .1f, 0, Rand.Range(6, 8), moteDirection, moteDirection);
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
                                    else if (psiEnergy > 40f && Rand.Chance(.4f + (.05f * pwr.level)))
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
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Recipe_Surgery), "CheckSurgeryFail", null)]
        public static class CheckSurgeryFail_Base_Patch
        {
            public static bool Prefix(Recipe_Surgery __instance, Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, ref bool __result)
            {

                if (patient.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD")) || patient.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadAnimalHD")) )
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
                    __instance.verbProps.verbClass.ToString() == "TorannMagic.Verb_AdvancedHeal")
                {
                    //Ignores line of sight
                    //                    
                    if (__instance.CasterPawn.RaceProps.Humanlike)
                    {
                        Pawn pawn = __instance.CasterPawn;
                        if(pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                        {
                            CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();
                            MightPowerSkill ver = comp.MightData.MightPowerSkill_Transpose.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Transpose_ver");
                            if(ver.level < 3)
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
                if(thingDef.thingClass.ToString() == "TorannMagic.TMPawnSummoned")
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
                if(ModOptions.Constants.GetGrowthCells().Count > 0)
                {
                    List<IntVec3> growthCells = ModOptions.Constants.GetGrowthCells();
                    for (int i = 0; i < growthCells.Count; i++)
                    {
                        if(loc == growthCells[i])
                        {
                            Traverse traverse = Traverse.Create(__instance);
                            Map map = (Map)FertilityGrid_Patch.map.GetValue(__instance);
                            __result *= 2f;
                            if(Rand.Chance(.6f) && (ModOptions.Constants.GetLastGrowthMoteTick() + 5) < Find.TickManager.TicksGame )
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
                if (pawn.story.traits.HasTrait(TorannMagicDefOf.Geomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic) || pawn.story.traits.HasTrait(TorannMagicDefOf.Warlock) || pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus) || pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless) || pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) || pawn.story.traits.HasTrait(TorannMagicDefOf.Priest) || pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator) || pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper) || pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Lich) || pawn.story.traits.HasTrait(TorannMagicDefOf.InnerFire) || pawn.story.traits.HasTrait(TorannMagicDefOf.StormBorn) || pawn.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
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
                if (pawn.story.traits.HasTrait(TorannMagicDefOf.Geomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic) || pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus) || pawn.story.traits.HasTrait(TorannMagicDefOf.Warlock) || pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless) || pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) || pawn.story.traits.HasTrait(TorannMagicDefOf.Priest) || pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator) || pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper) || pawn.story.traits.HasTrait(TorannMagicDefOf.Druid) || pawn.story.traits.HasTrait(TorannMagicDefOf.Paladin) || pawn.story.traits.HasTrait(TorannMagicDefOf.Arcanist) || pawn.story.traits.HasTrait(TorannMagicDefOf.Summoner) || pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Lich) || pawn.story.traits.HasTrait(TorannMagicDefOf.InnerFire) || pawn.story.traits.HasTrait(TorannMagicDefOf.StormBorn) || pawn.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
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
                            if (pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) || pawn.story.traits.HasTrait(TorannMagicDefOf.Priest) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Bard) || pawn.story.traits.HasTrait(TorannMagicDefOf.Geomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless) || pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator) || pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Druid) || pawn.story.traits.HasTrait(TorannMagicDefOf.Summoner))
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
                            if(__instance.ability.defName == "TM_ArrowStorm" && !caster.equipment.Primary.def.weaponTags.Contains("Neolithic"))
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
                bool flag = false;

                if (!flag)
                {

                    if (Rand.Chance(((settingsRef.baseFighterChance * 4) + (settingsRef.baseMageChance * 4) + (5 * settingsRef.advFighterChance) + (12 * settingsRef.advMageChance)) / (allTraits.Count - 17)))
                    {
                        pawnTraits.Remove(pawnTraits[pawnTraits.Count - 1]);
                        float rnd = Rand.Range(0, 4 * (settingsRef.baseFighterChance + settingsRef.baseMageChance) + (5 * settingsRef.advFighterChance) + (12 * settingsRef.advMageChance));
                        if (rnd < (4 * settingsRef.baseMageChance))
                        {
                            pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Gifted"), 2, false));
                        }
                        else if (rnd >= 4 * settingsRef.baseMageChance && rnd < (4 * (settingsRef.baseFighterChance + settingsRef.baseMageChance)))
                        {
                            pawn.story.traits.GainTrait(new Trait(TraitDef.Named("PhysicalProdigy"), 2, false));
                        }
                        else if (rnd >= (4 * (settingsRef.baseFighterChance + settingsRef.baseMageChance)) && rnd < (4 * (settingsRef.baseFighterChance + settingsRef.baseMageChance) + (5 * settingsRef.advFighterChance)))
                        {
                            int rndF = Rand.RangeInclusive(1, 6);
                            switch (rndF)
                            {
                                case 1:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Gladiator"), 4, false));
                                    break;
                                case 2:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("TM_Sniper"), 0, false));
                                    break;
                                case 3:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Bladedancer"), 0, false));
                                    break;
                                case 4:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Ranger"), 0, false));
                                    break;
                                case 5:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Faceless"), 4, false));
                                    break;
                                case 6:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("TM_Psionic"), 4, false));
                                    break;
                            }
                        }
                        else
                        {
                            int rndM = Rand.RangeInclusive(1, 13);
                            switch (rndM)
                            {
                                case 1:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("InnerFire"), 4, false));
                                    break;
                                case 2:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("HeartOfFrost"), 4, false));
                                    break;
                                case 3:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("StormBorn"), 4, false));
                                    break;
                                case 4:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Arcanist"), 4, false));
                                    break;
                                case 5:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Druid"), 4, false));
                                    break;
                                case 6:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Paladin"), 4, false));
                                    break;
                                case 7:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Summoner"), 4, false));
                                    break;
                                case 8:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Necromancer"), 4, false));
                                    break;
                                case 9:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Priest"), 4, false));
                                    break;
                                case 10:
                                    if (pawn.gender != Gender.Female)
                                    {
                                        pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Warlock"), 4, false));
                                    }
                                    else
                                    {
                                        pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Succubus"), 4, false));
                                    }
                                    break;
                                case 11:
                                    if (pawn.gender != Gender.Male)
                                    {
                                        pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Succubus"), 4, false));
                                    }
                                    else
                                    {
                                        pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Warlock"), 4, false));
                                    }
                                    break;
                                case 12:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("TM_Bard"), 0, false));
                                    break;
                                case 13:
                                    pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Geomancer"), 4, false));
                                    break;
                            }
                        }
                    }
                }
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
                if (__instance.abilityClass.FullName == "TorannMagic.MagicAbility" || __instance.abilityClass.FullName == "TorannMagic.MightAbility")
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
                if(Rand.Chance(settingsRef.magicyteChance))
                {
                    Thing thing = null;
                    thing = ThingMaker.MakeThing(TorannMagicDefOf.RawMagicyte);
                    thing.stackCount = Rand.Range(5, 12);
                    if(thing != null)
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
                    bool flag2 = __result == null || !__result.Any<Gizmo>();
                    if (!flag2)
                    {
                        List<Gizmo> list = __result.ToList<Gizmo>();
                        ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                        if (Find.Selector.SelectedObjects.Count >= 2 && !settingsRef.showIconsMultiSelect)
                        {
                            for (int z = 0; z < 5; z++)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    if (list[i].ToString().Contains("label=Attack") || list[i].ToString().Contains("Desc=Toggle") || list[i].ToString().Contains("label=Draft"))
                                    {
                                        //yes, filter 5 time                                   
                                    }
                                    else
                                    {
                                        list.Remove(list[i]);
                                    }
                                }
                            }                            
                            __result = list;
                        }                        
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
                if (comp != null && pawnComp.IsMagicUser)
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
                                opts.Add(new FloatMenuOption("TM_CannotDrop".Translate(new object[]
                                {
                                    comp.enchantingContainer[0].Label
                                }) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
                            }
                            else
                            {
                                opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TM_DropGem".Translate(new object[]
                                {
                                comp.enchantingContainer.ContentsString
                                }), delegate
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
                                opts.Add(new FloatMenuOption("CannotPickUp".Translate(new object[]
                                {
                                t.Label
                                }) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
                            }
                            else if (MassUtility.WillBeOverEncumberedAfterPickingUp(pawn, t, 1))
                            {
                                opts.Add(new FloatMenuOption("CannotPickUp".Translate(new object[]
                                {
                                t.Label
                                }) + " (" + "TooHeavy".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
                            }
                            else// if (item.stackCount == 1)
                            {
                                opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TM_PickupGem".Translate(new object[]
                                {
                                t.Label
                                }), delegate
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
                                opts.Add(new FloatMenuOption("TM_CannotReach".Translate(new object[]
                                {
                                t.Label
                                }) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null));
                            }
                            else if (pawnComp.Mana.CurLevel < .5f)
                            {
                                opts.Add(new FloatMenuOption("TM_NeedManaForEnchant".Translate(new object[]
                                {
                                pawnComp.Mana.CurLevel.ToString("0.000")
                                }), null, MenuOptionPriority.Default, null, null, 0f, null, null));
                            }
                            else// if (item.stackCount == 1)
                            {
                                if (current.stackCount == 1)
                                {
                                    opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TM_EnchantItem".Translate(new object[]
                                    {
                                        t.Label
                                    }), delegate
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

        [HarmonyPatch(typeof(PawnAbility), "PostAbilityAttempt", null)]
        public class PawnAbility_Patch
        {
            public static bool Prefix(PawnAbility __instance)
            {
                CompAbilityUserMagic comp = __instance.Pawn.GetComp<CompAbilityUserMagic>();
                __instance.CooldownTicksLeft = Mathf.RoundToInt((float)__instance.MaxCastingTicks * comp.coolDown);
                if(!__instance.Pawn.IsColonist)
                {
                    __instance.CooldownTicksLeft = (int)(__instance.CooldownTicksLeft / 2f);
                }
                return false;
            }
        }        

        [HarmonyPatch(typeof(GenGrid), "Standable", null)]
        public class Standable_Patch
        {
            public static bool Prefix(ref IntVec3 c, ref Map map, ref bool __result)
            {
                if(map != null && c != default(IntVec3))
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
                    if ( targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_I, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_II, false) || targetPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_III, false))
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
                if(pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadAnimalHD")))
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
                if(__instance.parent.def.defName == "Poppi")
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

        [HarmonyPatch(typeof(ITab_Pawn_Gear), "TryDrawOverallArmor", null)]
        public class ITab_Pawn_GearFillTab_Patch
        {
            //public static FieldInfo pawn = typeof(ITab_Pawn_Gear).GetField("SelPawnForGear", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);

            public static void Postfix(ITab_Pawn_Gear __instance, ref float curY, float width, StatDef stat, string label)
            {
                if(stat.defName == "ArmorRating_Heat")
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
                if(initiator.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper) || recipient.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper))
                {
                    __result *= 1.2f;
                }
                if(recipient.story.traits.HasTrait(TorannMagicDefOf.TM_Bard))
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
                if(__instance.pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD")) || __instance.pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_LichHD")))
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
                        if(pawn.Inspired && pawn.InspirationDef.defName == "ID_Outgoing")
                        {
                            if ((Find.TickManager.TicksGame - lastInteractionTime) > (1800))
                            {
                                Log.Message("faster interactions occuring");
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
    }
}
