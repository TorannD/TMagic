using Verse;
using RimWorld;
using AbilityUser;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace TorannMagic
{
    class Projectile_RaiseUndead : Projectile_AbilityBase
    {
        MagicPowerSkill pwr;
        MagicPowerSkill ver;

        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            base.Impact(hitThing);
            ThingDef def = this.def;
            int raisedPawns = 0;

            Pawn pawn = this.launcher as Pawn;
            Pawn victim = hitThing as Pawn;
            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            pwr = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_RaiseUndead.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_RaiseUndead_pwr");
            ver = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_RaiseUndead.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_RaiseUndead_ver");

            Thing corpseThing = null;
            
            IntVec3 curCell;
            IEnumerable<IntVec3> targets = GenRadial.RadialCellsAround(base.Position, this.def.projectile.explosionRadius, true);
            for (int i = 0; i < targets.Count(); i++)
            {
                curCell = targets.ToArray<IntVec3>()[i];

                TM_MoteMaker.ThrowPoisonMote(curCell.ToVector3Shifted(), map, .3f);
                if (curCell.InBounds(map))
                { 
                    Corpse corpse = null;
                    List<Thing> thingList;
                    thingList = curCell.GetThingList(map);
                    int z = 0;
                    while (z < thingList.Count)
                    {
                        corpseThing = thingList[z];
                        if (corpseThing != null)
                        {
                            bool validator = corpseThing is Corpse;
                            if (validator)
                            {
                                corpse = corpseThing as Corpse;
                                Pawn undeadPawn = corpse.InnerPawn;
                                CompRottable compRottable = corpse.GetComp<CompRottable>();
                                float rotStage = 0;
                                if (compRottable != null && compRottable.Stage == RotStage.Dessicated)
                                {
                                    rotStage = 1f;
                                }
                                if (compRottable != null)
                                {
                                    rotStage += compRottable.RotProgressPct;
                                }
                                bool flag_SL = false;
                                if (undeadPawn.def.defName == "SL_Runner" || undeadPawn.def.defName == "SL_Peon" || undeadPawn.def.defName == "SL_Archer" || undeadPawn.def.defName == "SL_Hero")
                                {                                    
                                    PawnGenerationRequest pgr = new PawnGenerationRequest(PawnKindDef.Named("Tribesperson"), pawn.Faction, PawnGenerationContext.NonPlayer, -1, true, false, false, false, false, true, 0, false, false, false, false, false, false, false, null, null, null, null, null, null, null, "Undead");
                                    Pawn newUndeadPawn = PawnGenerator.GeneratePawn(pgr);
                                    GenSpawn.Spawn(newUndeadPawn, corpse.Position, corpse.Map, WipeMode.Vanish);
                                    corpse.Strip();
                                    corpse.Destroy(DestroyMode.Vanish);
                                    rotStage = 1f;
                                    flag_SL = true;
                                    undeadPawn = newUndeadPawn;
                                }                                                              
                                if (!undeadPawn.def.defName.Contains("ROM_") && undeadPawn.RaceProps.IsFlesh && (undeadPawn.Dead || flag_SL) && undeadPawn.def.thingClass.FullName != "TorannMagic.TMPawnSummoned")
                                {
                                    bool wasVampire = false;

                                    IEnumerable<ThingDef> enumerable = from hd in DefDatabase<HediffDef>.AllDefs
                                                                       where (def.defName == "ROM_Vampirism")
                                                                       select def;
                                    if (enumerable.Count() > 0)
                                    {
                                        bool hasVampHediff = undeadPawn.health.hediffSet.HasHediff(HediffDef.Named("ROM_Vampirism")) || undeadPawn.health.hediffSet.HasHediff(HediffDef.Named("ROM_GhoulHediff"));
                                        if (hasVampHediff)
                                        {
                                            wasVampire = true;
                                        }
                                    }

                                    if (!wasVampire)
                                    {                                        
                                        undeadPawn.SetFaction(pawn.Faction);
                                        if (undeadPawn.Dead)
                                        {
                                            ResurrectionUtility.Resurrect(undeadPawn);
                                        }
                                        raisedPawns++;
                                        if (!undeadPawn.kindDef.RaceProps.Animal && undeadPawn.kindDef.RaceProps.Humanlike)
                                        {
                                            CompAbilityUserMagic compMagic = undeadPawn.GetComp<CompAbilityUserMagic>();
                                            if(TM_Calc.IsMagicUser(undeadPawn)) //(compMagic.IsMagicUser && !undeadPawn.story.traits.HasTrait(TorannMagicDefOf.Faceless)) || 
                                            {
                                                compMagic.Initialize();
                                                compMagic.RemovePowers();
                                            }
                                            CompAbilityUserMight compMight = undeadPawn.GetComp<CompAbilityUserMight>();
                                            if (TM_Calc.IsMightUser(undeadPawn)) //compMight.IsMightUser || 
                                            {
                                                compMight.Initialize();
                                                compMight.RemovePowers();
                                            }
                                            HealthUtility.AdjustSeverity(undeadPawn, TorannMagicDefOf.TM_UndeadHD, -4f);
                                            HealthUtility.AdjustSeverity(undeadPawn, TorannMagicDefOf.TM_UndeadHD, .5f + ver.level);                                            
                                            HealthUtility.AdjustSeverity(undeadPawn, HediffDef.Named("TM_UndeadStageHD"), -2f);
                                            HealthUtility.AdjustSeverity(undeadPawn, HediffDef.Named("TM_UndeadStageHD"), rotStage);
                                            RedoSkills(undeadPawn);
                                            RemoveTraits(undeadPawn, undeadPawn.story.traits.allTraits);
                                            undeadPawn.story.traits.GainTrait(new Trait(TraitDef.Named("Undead"), 0, false));
                                            undeadPawn.story.traits.GainTrait(new Trait(TraitDef.Named("Psychopath"), 0, false));                                            
                                            undeadPawn.needs.AddOrRemoveNeedsAsAppropriate();
                                            RemoveClassHediff(undeadPawn);
                                            //Color undeadColor = new Color(.2f, .4f, 0);
                                            //undeadPawn.story.hairColor = undeadColor;
                                            //CompAbilityUserMagic undeadComp = undeadPawn.GetComp<CompAbilityUserMagic>();
                                            //if (undeadComp.IsMagicUser)
                                            //{
                                            //    undeadComp.ClearPowers();
                                            //}

                                            List<SkillRecord> skills = undeadPawn.skills.skills;
                                            for (int j = 0; j < skills.Count; j++)
                                            {
                                                skills[j].passion = Passion.None;
                                            }
                                            undeadPawn.playerSettings.hostilityResponse = HostilityResponseMode.Attack;
                                            undeadPawn.playerSettings.medCare = MedicalCareCategory.NoMeds;
                                            for (int h = 0; h < 24; h++)
                                            {
                                                undeadPawn.timetable.SetAssignment(h, TimeAssignmentDefOf.Work);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        Messages.Message("Vampiric powers have prevented undead reanimation of " + undeadPawn.LabelShort, MessageTypeDefOf.RejectInput);
                                    }

                                    if (undeadPawn.kindDef.RaceProps.Animal)
                                    {
                                        HealthUtility.AdjustSeverity(undeadPawn, TorannMagicDefOf.TM_UndeadAnimalHD, -4f);
                                        HealthUtility.AdjustSeverity(undeadPawn, TorannMagicDefOf.TM_UndeadAnimalHD, .5f + ver.level);
                                        HealthUtility.AdjustSeverity(undeadPawn, HediffDef.Named("TM_UndeadStageHD"), -2f);
                                        HealthUtility.AdjustSeverity(undeadPawn, HediffDef.Named("TM_UndeadStageHD"), rotStage);

                                        if (undeadPawn.training.CanAssignToTrain(TrainableDefOf.Tameness).Accepted)
                                        {
                                            while (!undeadPawn.training.HasLearned(TrainableDefOf.Tameness))
                                            {
                                                undeadPawn.training.Train(TrainableDefOf.Tameness, pawn);
                                            }
                                        }
                                        
                                        if (undeadPawn.training.CanAssignToTrain(TrainableDefOf.Obedience).Accepted) 
                                        {
                                            while (!undeadPawn.training.HasLearned(TrainableDefOf.Obedience)) 
                                            {
                                                undeadPawn.training.Train(TrainableDefOf.Obedience, pawn);
                                            }                                            
                                        }

                                        if(undeadPawn.training.CanAssignToTrain(TrainableDefOf.Release).Accepted)
                                        {
                                            while (!undeadPawn.training.HasLearned(TrainableDefOf.Release))
                                            {
                                                undeadPawn.training.Train(TrainableDefOf.Release, pawn);
                                            }
                                        }

                                        if (undeadPawn.training.CanAssignToTrain(TorannMagicDefOf.Haul).Accepted)
                                        {
                                            while (!undeadPawn.training.HasLearned(TorannMagicDefOf.Haul))
                                            {
                                                undeadPawn.training.Train(TorannMagicDefOf.Haul, pawn);
                                            }
                                        }

                                        if (undeadPawn.training.CanAssignToTrain(TorannMagicDefOf.Rescue).Accepted)
                                        {
                                            while (!undeadPawn.training.HasLearned(TorannMagicDefOf.Rescue))
                                            {
                                                undeadPawn.training.Train(TorannMagicDefOf.Rescue, pawn);
                                            }
                                        }
                                        undeadPawn.playerSettings.medCare = MedicalCareCategory.NoMeds;
                                        undeadPawn.def.tradeability = Tradeability.None;
                                        
                                        
                                    }
                                }
                            }
                        }
                        z++;
                    }
                }
                if (raisedPawns > pwr.level + 1)
                {
                    i = targets.Count();
                }
            }
        }

        private void RedoSkills(Pawn undeadPawn)
        {
            undeadPawn.story.childhood = null;
            undeadPawn.story.adulthood = null;
            undeadPawn.story.DisabledWorkTypes.Clear();
            //undeadPawn.story.WorkTypeIsDisabled(WorkTypeDefOf.Warden);
            //undeadPawn.story.WorkTypeIsDisabled(WorkTypeDefOf.Hunting);
            //undeadPawn.story.WorkTypeIsDisabled(WorkTypeDefOf.Handling);
            //undeadPawn.story.WorkTypeIsDisabled(WorkTypeDefOf.Doctor);            

            undeadPawn.skills.Learn(SkillDefOf.Shooting, -10000000, true);
            undeadPawn.skills.Learn(SkillDefOf.Animals, -10000000, true);
            undeadPawn.skills.Learn(SkillDefOf.Artistic, -10000000, true);
            undeadPawn.skills.Learn(SkillDefOf.Cooking, -10000000, true);
            undeadPawn.skills.Learn(SkillDefOf.Cooking, Rand.Range(10000, 20000), true);
            undeadPawn.skills.Learn(SkillDefOf.Crafting, -10000000, true);
            undeadPawn.skills.Learn(SkillDefOf.Crafting, Rand.Range(10000, 50000), true);
            undeadPawn.skills.Learn(SkillDefOf.Plants, -10000000, true);
            undeadPawn.skills.Learn(SkillDefOf.Plants, Rand.Range(40000, 60000), true);
            undeadPawn.skills.Learn(SkillDefOf.Intellectual, -10000000, true);
            undeadPawn.skills.Learn(SkillDefOf.Medicine, -10000000, true);
            undeadPawn.skills.Learn(SkillDefOf.Melee, -10000000, true);
            undeadPawn.skills.Learn(SkillDefOf.Melee, Rand.Range(60000, 100000), true);
            undeadPawn.skills.Learn(SkillDefOf.Mining, -10000000, true);
            undeadPawn.skills.Learn(SkillDefOf.Mining, Rand.Range(40000, 80000), true);
            undeadPawn.skills.Learn(SkillDefOf.Social, -10000000, true);
            undeadPawn.skills.Learn(SkillDefOf.Construction, -10000000, true);
            undeadPawn.skills.Learn(SkillDefOf.Construction, Rand.Range(40000, 60000), true);
            undeadPawn.workSettings.SetPriority(WorkTypeDefOf.Doctor, 0);
            undeadPawn.workSettings.SetPriority(WorkTypeDefOf.Warden, 0);
            undeadPawn.workSettings.SetPriority(WorkTypeDefOf.Handling, 0);
            undeadPawn.workSettings.SetPriority(TorannMagicDefOf.Research, 0);
            undeadPawn.workSettings.SetPriority(TorannMagicDefOf.Art, 0);

            //SkillRecord skill;
            //skill = undeadPawn.skills.GetSkill(SkillDefOf.Animals);
            //skill.passion = Passion.None;
            //skill = undeadPawn.skills.GetSkill(SkillDefOf.Artistic);
            //skill.passion = Passion.None;
            //skill = undeadPawn.skills.GetSkill(SkillDefOf.Construction);
            //skill.passion = Passion.None;
            //skill = undeadPawn.skills.GetSkill(SkillDefOf.Cooking);
            //skill.passion = Passion.None;
            //skill = undeadPawn.skills.GetSkill(SkillDefOf.Crafting);
            //skill.passion = Passion.None;
            //skill = undeadPawn.skills.GetSkill(SkillDefOf.Growing);
            //skill.passion = Passion.None;
            //skill = undeadPawn.skills.GetSkill(SkillDefOf.Medicine);
            //skill.passion = Passion.None;
            //skill = undeadPawn.skills.GetSkill(SkillDefOf.Mining);
            //skill.passion = Passion.None;
            //skill = undeadPawn.skills.GetSkill(SkillDefOf.Social);
            //skill.passion = Passion.None;
            //skill = undeadPawn.skills.GetSkill(SkillDefOf.Intellectual);
            //skill.passion = Passion.None;
            //skill = undeadPawn.skills.GetSkill(SkillDefOf.Shooting);
            //skill.passion = Passion.None;
            //skill = undeadPawn.skills.GetSkill(SkillDefOf.Melee);
            //skill.passion = Passion.None;
        }

        private void RemoveTraits(Pawn pawn, List<Trait> traits)
        {
            for (int i = 0; i < traits.Count; i++)
            {
                traits.Remove(traits[i]);
                i--;
            }
        }

        private void RemoveClassHediff(Pawn pawn)
        {
            if (pawn.health != null && pawn.health.hediffSet != null && pawn.health.hediffSet.hediffs != null && pawn.health.hediffSet.hediffs.Count > 0)
            {
                for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
                {
                    Hediff hediff = pawn.health.hediffSet.hediffs[i];
                    if(hediff.def == TorannMagicDefOf.TM_MagicUserHD)
                    {
                        pawn.health.RemoveHediff(hediff);
                    }
                    if(hediff.def == TorannMagicDefOf.TM_MightUserHD)
                    {
                        pawn.health.RemoveHediff(hediff);
                    }
                }
            }
        }
    }
}


