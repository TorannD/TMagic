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
                if (animal.RaceProps.TrainableIntelligence == TrainableIntelligenceDefOf.Intermediate || animal.RaceProps.TrainableIntelligence == TrainableIntelligenceDefOf.Advanced)
                {
                    if ((animal.RaceProps.wildness <= .7f) || (animal.RaceProps.wildness <= .8f && pwr.level == 1) || (animal.RaceProps.wildness <= .9f && pwr.level == 2) || pwr.level == 3)
                    {
                        if (Rand.Chance((1 - animal.RaceProps.wildness) * 10))
                        {
                            if (comp.bondedPet != null && comp.bondedPet != animal)
                            {
                                Pawn oldbond = comp.bondedPet;
                                if (!oldbond.Destroyed)
                                {
                                    if (!comp.bondedPet.Dead)
                                    {
                                        //bonding with another pet without first pet being dead or destroyed
                                        Messages.Message("TM_BondedAnimalRelease".Translate(new object[]
                                        {
                                            oldbond.LabelShort,
                                            pawn.LabelShort
                                        }), MessageTypeDefOf.NeutralEvent);
                                        MoteMaker.ThrowSmoke(oldbond.DrawPos, oldbond.Map, 3f);
                                        oldbond.Destroy();
                                    }                                    
                                }
                            }
                            animal.SetFaction(pawn.Faction);
                            HealthUtility.AdjustSeverity(animal, TorannMagicDefOf.TM_RangerBondHD, -4f);
                            HealthUtility.AdjustSeverity(animal, TorannMagicDefOf.TM_RangerBondHD, .5f + ver.level);
                            comp.bondedPet = animal;
                            if (animal.RaceProps.TrainableIntelligence == TrainableIntelligenceDefOf.Intermediate)
                            {
                                while (!animal.training.IsCompleted(TrainableDefOf.Obedience))
                                {
                                    animal.training.Train(TrainableDefOf.Obedience, pawn);
                                }
                                while (!animal.training.IsCompleted(TrainableDefOf.Release))
                                {
                                    animal.training.Train(TrainableDefOf.Release, pawn);
                                }
                            }

                            if (animal.RaceProps.TrainableIntelligence == TrainableIntelligenceDefOf.Advanced)
                            {
                                while (!animal.training.IsCompleted(TrainableDefOf.Obedience))
                                {
                                    animal.training.Train(TrainableDefOf.Obedience, pawn);
                                }
                                while (!animal.training.IsCompleted(TrainableDefOf.Release))
                                {
                                    animal.training.Train(TrainableDefOf.Release, pawn);
                                }
                                while (!animal.training.IsCompleted(TorannMagicDefOf.Rescue))
                                {
                                    animal.training.Train(TorannMagicDefOf.Rescue, pawn);
                                }
                                if (animal.BodySize > .4)
                                {
                                    while (!animal.training.IsCompleted(TorannMagicDefOf.Haul))
                                    {
                                        animal.training.Train(TorannMagicDefOf.Haul, pawn);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Messages.Message("TM_FailedRangerBond".Translate(new object[]
                            {
                                animal.LabelShort,
                                pawn.LabelShort,
                                ((1 - animal.RaceProps.wildness) * 10f)
                            }), MessageTypeDefOf.NeutralEvent);
                        }
                    }
                    else
                    {
                        Messages.Message("TM_RangerNotExperienced".Translate(new object[]
                        {
                                animal.LabelShort,
                                pawn.LabelShort
                        }), MessageTypeDefOf.NeutralEvent);
                    }
                }
                else
                {
                    Messages.Message("TM_AnimalIncapableOfBond".Translate(new object[]
                        {
                            animal.LabelShort,
                            pawn.LabelShort
                        }), MessageTypeDefOf.NeutralEvent);
                }

            }
            this.PostCastShot(flag, out flag);
            return flag;
        }
    }
}
