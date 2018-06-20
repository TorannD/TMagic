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
                if (curCell.InBounds(map) && curCell.IsValid)
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
                                Pawn newUndeadPawn = new Pawn();
                                
                                if (undeadPawn.RaceProps.IsFlesh && undeadPawn.Dead && undeadPawn.def.thingClass.FullName != "TorannMagic.TMPawnSummoned")
                                {
                                    undeadPawn.SetFaction(pawn.Faction);
                                    ResurrectionUtility.Resurrect(undeadPawn);
                                    raisedPawns++;
                                    if (!undeadPawn.kindDef.RaceProps.Animal && undeadPawn.kindDef.RaceProps.Humanlike)
                                    {
                                        HealthUtility.AdjustSeverity(undeadPawn, TorannMagicDefOf.TM_UndeadHD, -4f);
                                        HealthUtility.AdjustSeverity(undeadPawn, TorannMagicDefOf.TM_UndeadHD, .5f + ver.level);
                                        RedoSkills(undeadPawn);
                                        RemoveTraits(undeadPawn, undeadPawn.story.traits.allTraits);                                        
                                        undeadPawn.story.traits.GainTrait(new Trait(TraitDef.Named("Undead"), 0, false));
                                        undeadPawn.story.traits.GainTrait(new Trait(TraitDef.Named("Psychopath"), 0, false));
                                        undeadPawn.needs.AddOrRemoveNeedsAsAppropriate();
                                        Color undeadColor = new Color(.2f, .4f, 0);
                                        undeadPawn.story.hairColor = undeadColor;                                        
                                        CompAbilityUserMagic undeadComp = undeadPawn.GetComp<CompAbilityUserMagic>();
                                        if (undeadComp.IsMagicUser)
                                        {
                                            undeadComp.ClearPowers();
                                        }                                       
                                                                              
                                        List<SkillRecord> skills = undeadPawn.skills.skills;
                                        for (int j = 0; j < skills.Count; j++)
                                        {
                                            skills[j].passion = Passion.None;
                                        }
                                        undeadPawn.playerSettings.hostilityResponse = HostilityResponseMode.Attack;
                                        undeadPawn.playerSettings.medCare = MedicalCareCategory.NoMeds;
                                        for(int h = 0; h < 24; h++ )
                                        {
                                            undeadPawn.timetable.SetAssignment(h, TimeAssignmentDefOf.Work);
                                        }                                        

                                    }
                                    if (undeadPawn.kindDef.RaceProps.Animal)
                                    {
                                        HealthUtility.AdjustSeverity(undeadPawn, TorannMagicDefOf.TM_UndeadAnimalHD, -4f);
                                        HealthUtility.AdjustSeverity(undeadPawn, TorannMagicDefOf.TM_UndeadAnimalHD, .5f + ver.level);

                                        if (undeadPawn.RaceProps.TrainableIntelligence == TrainableIntelligenceDefOf.Intermediate)
                                        {
                                            while (!undeadPawn.training.IsCompleted(TrainableDefOf.Obedience))
                                            {
                                                undeadPawn.training.Train(TrainableDefOf.Obedience, pawn);
                                            }
                                            while (!undeadPawn.training.IsCompleted(TrainableDefOf.Release))
                                            {
                                                undeadPawn.training.Train(TrainableDefOf.Release, pawn);
                                            }
                                        }

                                        if (undeadPawn.RaceProps.TrainableIntelligence == TrainableIntelligenceDefOf.Advanced)
                                        {
                                            while (!undeadPawn.training.IsCompleted(TrainableDefOf.Obedience))
                                            {
                                                undeadPawn.training.Train(TrainableDefOf.Obedience, pawn);
                                            }
                                            while (!undeadPawn.training.IsCompleted(TrainableDefOf.Release))
                                            {
                                                undeadPawn.training.Train(TrainableDefOf.Release, pawn);
                                            }
                                            if (undeadPawn.BodySize > .4)
                                            {
                                                while (!undeadPawn.training.IsCompleted(TorannMagicDefOf.Haul))
                                                {
                                                    undeadPawn.training.Train(TorannMagicDefOf.Haul, pawn);
                                                }
                                            }
                                        }
                                        undeadPawn.playerSettings.medCare = MedicalCareCategory.NoMeds;
                                        undeadPawn.def.tradeability = Tradeability.Never;
                                        
                                        
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
            undeadPawn.skills.Learn(SkillDefOf.Growing, -10000000, true);
            undeadPawn.skills.Learn(SkillDefOf.Growing, Rand.Range(40000, 60000), true);
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
    }
}


