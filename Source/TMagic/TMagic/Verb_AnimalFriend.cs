using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using AbilityUser;
using Verse;


namespace TorannMagic
{
    public class Verb_AnimalFriend : Verb_UseAbility
    {

        bool validTarg;
        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            if (targ.IsValid && targ.CenterVector3.InBounds(base.CasterPawn.Map) && !targ.Cell.Fogged(base.CasterPawn.Map))
            {
                if ((root - targ.Cell).LengthHorizontal < this.verbProps.range)
                {
                    validTarg = true;
                }
                else
                {
                    //out of range
                    validTarg = false;
                }
            }
            else
            {
                validTarg = false;
            }
            return validTarg;
        }

        protected override bool TryCastShot()
        {
            bool flag = false;
            CompAbilityUserMight comp = base.CasterPawn.GetComp<CompAbilityUserMight>();
            MightPowerSkill pwr = base.CasterPawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_AnimalFriend.FirstOrDefault((MightPowerSkill x) => x.label == "TM_AnimalFriend_pwr");
            MightPowerSkill ver = base.CasterPawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_AnimalFriend.FirstOrDefault((MightPowerSkill x) => x.label == "TM_AnimalFriend_ver");
            Pawn pawn = this.CasterPawn;
            Pawn animal = this.currentTarget.Thing as Pawn;

            if(animal !=null && animal.RaceProps.Animal && animal.RaceProps.IsFlesh)
            {
                Pawn oldbond = comp.bondedPet;
                if (animal == comp.bondedPet)
                {
                    comp.bondedPet = null;
                    Messages.Message("TM_BondedAnimalRelease".Translate(
                                            oldbond.LabelShort,
                                            pawn.LabelShort
                                        ), MessageTypeDefOf.NeutralEvent);
                    MoteMaker.ThrowSmoke(oldbond.DrawPos, oldbond.Map, 3f);
                    oldbond.Destroy();
                }
                else if(animal.Faction != null)
                {
                    Messages.Message("TM_AnimalHasAllegience".Translate(
                                        ), MessageTypeDefOf.RejectInput);
                }
                else
                {
                    if (animal.RaceProps.intelligence == Intelligence.Animal) // == TrainableIntelligenceDefOf.Intermediate || animal.RaceProps.TrainableIntelligence == TrainableIntelligenceDefOf.Advanced)
                    {
                        if ((animal.RaceProps.wildness <= .7f) || (animal.RaceProps.wildness <= .8f && pwr.level == 1) || (animal.RaceProps.wildness <= .9f && pwr.level == 2) || pwr.level == 3)
                        {
                            if (Rand.Chance((1 - animal.RaceProps.wildness) * 10))
                            {
                                if (comp.bondedPet != null && comp.bondedPet != animal)
                                {                                    
                                    if (!oldbond.Destroyed)
                                    {
                                        if (!comp.bondedPet.Dead)
                                        {
                                            //bonding with another pet without first pet being dead or destroyed
                                            comp.bondedPet = null;
                                            Messages.Message("TM_BondedAnimalRelease".Translate(
                                            oldbond.LabelShort,
                                            pawn.LabelShort
                                            ), MessageTypeDefOf.NeutralEvent);
                                            if (oldbond.Map != null)
                                            {
                                                MoteMaker.ThrowSmoke(oldbond.DrawPos, oldbond.Map, 3f);
                                            }
                                            else
                                            {
                                                oldbond.ParentHolder.GetDirectlyHeldThings().Remove(oldbond);
                                            }
                                            oldbond.Destroy();
                                        }
                                    }
                                }
                                animal.SetFaction(pawn.Faction);
                                HealthUtility.AdjustSeverity(animal, TorannMagicDefOf.TM_RangerBondHD, -4f);
                                HealthUtility.AdjustSeverity(animal, TorannMagicDefOf.TM_RangerBondHD, .5f + ver.level);
                                comp.bondedPet = animal;

                                if (animal.training.CanBeTrained(TrainableDefOf.Tameness))
                                {
                                    while (!animal.training.HasLearned(TrainableDefOf.Tameness))
                                    {
                                        animal.training.Train(TrainableDefOf.Tameness, pawn);
                                    }
                                }

                                if (animal.training.CanBeTrained(TrainableDefOf.Obedience)) 
                                {
                                    while (!animal.training.HasLearned(TrainableDefOf.Obedience)) 
                                    {
                                        animal.training.Train(TrainableDefOf.Obedience, pawn);
                                    }
                                }

                                if (animal.training.CanBeTrained(TrainableDefOf.Release))
                                {
                                    while (!animal.training.HasLearned(TrainableDefOf.Release))
                                    {
                                        animal.training.Train(TrainableDefOf.Release, pawn);
                                    }
                                }

                                if (animal.training.CanBeTrained(TorannMagicDefOf.Haul))
                                {
                                    while (!animal.training.HasLearned(TorannMagicDefOf.Haul))
                                    {
                                        animal.training.Train(TorannMagicDefOf.Haul, pawn);
                                    }
                                }

                                if (animal.training.CanBeTrained(TorannMagicDefOf.Rescue))
                                {
                                    while (!animal.training.HasLearned(TorannMagicDefOf.Rescue))
                                    {
                                        animal.training.Train(TorannMagicDefOf.Rescue, pawn);
                                    }
                                }                               
                            }
                            else
                            {
                                Messages.Message("TM_FailedRangerBond".Translate(
                                animal.LabelShort,
                                pawn.LabelShort,
                                ((1 - animal.RaceProps.wildness) * 100f)
                                ), MessageTypeDefOf.NeutralEvent);
                            }
                        }
                        else
                        {
                            Messages.Message("TM_RangerNotExperienced".Translate(
                                animal.LabelShort,
                                pawn.LabelShort,
                                (animal.RaceProps.wildness * 100).ToString("F"),
                                (.7f + .1f*pwr.level) * 100
                            ), MessageTypeDefOf.NeutralEvent);
                        }
                    }
                    else
                    {
                        Messages.Message("TM_AnimalIncapableOfBond".Translate(
                            animal.LabelShort,
                            pawn.LabelShort
                            ), MessageTypeDefOf.NeutralEvent);
                    }
                }

            }
            this.PostCastShot(flag, out flag);
            return flag;
        }
    }
}
