using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using AbilityUser;
using UnityEngine;

namespace TorannMagic
{
    public class TMJobDriver_CastAbilityVerb : JobDriver_CastAbilityVerb
    {

        private int duration;
        Verb arg_45_0;

        protected override IEnumerable<Toil> MakeNewToils()
        {
            bool flag;
            yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
            Verb_UseAbility curJob = this.pawn.CurJob.verbToUse as Verb_UseAbility;
            if (base.TargetA.HasThing)
            {
                flag = (!base.GetActor().IsFighting() ? true : !curJob.UseAbilityProps.canCastInMelee);
                if (flag)
                {
                    Toil toil = Toils_Combat.GotoCastPosition(TargetIndex.A, false);
                    yield return toil;
                    toil = null;
                }
            }
            if (this.Context == AbilityContext.Player)
            {
                Find.Targeter.targetingVerb = curJob;
            }
            Pawn targetPawn = null;
            if (this.TargetThingA != null)
            {
                targetPawn = TargetThingA as Pawn;
            }
            
            if (targetPawn != null)
            {
                //yield return Toils_Combat.CastVerb(TargetIndex.A, false);
                Toil combatToil = new Toil();
                combatToil.FailOnDestroyedOrNull(TargetIndex.A);
                combatToil.FailOnDespawnedOrNull(TargetIndex.A);
                //combatToil.FailOnDowned(TargetIndex.A);
                //CompAbilityUserMagic comp = this.pawn.GetComp<CompAbilityUserMagic>();
                this.duration = (int)((curJob.verbProps.warmupTime*60) * this.pawn.GetStatValue(StatDefOf.AimingDelayFactor, false));
                //JobDriver curDriver = this.pawn.jobs.curDriver;
                combatToil.initAction = delegate
                {
                    arg_45_0 = combatToil.actor.jobs.curJob.verbToUse;
                    if (this.pawn.RaceProps.Humanlike)
                    {
                        if (this.pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                        {
                            CompAbilityUserMight mightComp = this.pawn.GetComp<CompAbilityUserMight>();
                            CompAbilityUserMagic magicComp = this.pawn.GetComp<CompAbilityUserMagic>();
                            if (mightComp.mimicAbility != null && mightComp.mimicAbility.MainVerb.verbClass == arg_45_0.verbProps.verbClass)
                            {
                                mightComp.RemovePawnAbility(mightComp.mimicAbility);
                            }
                            if (magicComp.mimicAbility != null && magicComp.mimicAbility.MainVerb.verbClass == arg_45_0.verbProps.verbClass)
                            {
                                magicComp.RemovePawnAbility(magicComp.mimicAbility);
                            }
                        }
                    }
                    
                    LocalTargetInfo target = combatToil.actor.jobs.curJob.GetTarget(TargetIndex.A);
                    // bool canFreeIntercept2 = false;
                    arg_45_0.TryStartCastOn(target, false, false);
                    using (IEnumerator<Hediff> enumerator = this.pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Hediff rec = enumerator.Current;
                            if (rec.def == TorannMagicDefOf.TM_PossessionHD || rec.def == TorannMagicDefOf.TM_DisguiseHD || rec.def == TorannMagicDefOf.TM_DisguiseHD_I || rec.def == TorannMagicDefOf.TM_DisguiseHD_II || rec.def == TorannMagicDefOf.TM_DisguiseHD_III)
                            {
                                this.pawn.health.RemoveHediff(rec);
                            }
                        }
                    }
                };
                combatToil.tickAction = delegate
                {
                    if (Find.TickManager.TicksGame % 12 == 0)
                    {
                        TM_MoteMaker.ThrowCastingMote(pawn.DrawPos, pawn.Map, Rand.Range(1.2f, 2f));
                    }
                    
                    this.duration--;
                };
                combatToil.AddFinishAction(delegate
                {
                    if (this.duration <= 5)
                    {                        
                        curJob.Ability.PostAbilityAttempt();
                    }
                    
                });
                //if (combatToil.actor.CurJob != this.job)
                //{
                //    curDriver.ReadyForNextToil();
                //}
                combatToil.defaultCompleteMode = ToilCompleteMode.FinishedBusy;
                yield return combatToil;
                //Toil toil2 = new Toil()
                //{
                //    initAction = () =>
                //    {
                //        if (curJob.UseAbilityProps.isViolent)
                //        {
                //            JobDriver_CastAbilityVerb.CheckForAutoAttack(this.pawn);
                //        }
                //    },
                //    defaultCompleteMode = ToilCompleteMode.Instant
                //};
                //yield return toil2;
                //Toil toil1 = new Toil()
                //{
                //    initAction = () => curJob.Ability.PostAbilityAttempt(),
                //    defaultCompleteMode = ToilCompleteMode.Instant
                //};
                //yield return toil1;
            }
            else
            {
                if ((pawn.Position - TargetLocA).LengthHorizontal < curJob.verbProps.range)
                {
                    if (TargetLocA.IsValid && TargetLocA.InBounds(pawn.Map) && !TargetLocA.Fogged(pawn.Map) && TargetLocA.Walkable(pawn.Map))
                    {
                        ShootLine shootLine;
                        bool validTarg = curJob.TryFindShootLineFromTo(pawn.Position, TargetLocA, out shootLine);
                        if (validTarg)
                        {
                            //yield return Toils_Combat.CastVerb(TargetIndex.A, false);
                            //Toil toil2 = new Toil()
                            //{
                            //    initAction = () =>
                            //    {
                            //        if (curJob.UseAbilityProps.isViolent)
                            //        {
                            //            JobDriver_CastAbilityVerb.CheckForAutoAttack(this.pawn);
                            //        }

                            //    },
                            //    defaultCompleteMode = ToilCompleteMode.Instant
                            //};
                            //yield return toil2;
                            this.duration = (int)((curJob.verbProps.warmupTime * 60) * this.pawn.GetStatValue(StatDefOf.AimingDelayFactor, false));
                            Toil toil = new Toil();
                            toil.initAction = delegate
                            {
                                arg_45_0 = toil.actor.jobs.curJob.verbToUse;
                                if (this.pawn.RaceProps.Humanlike)
                                {
                                    if (this.pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                                    {
                                        CompAbilityUserMight mightComp = this.pawn.GetComp<CompAbilityUserMight>();
                                        CompAbilityUserMagic magicComp = this.pawn.GetComp<CompAbilityUserMagic>();
                                        if (mightComp.mimicAbility != null && mightComp.mimicAbility.MainVerb.verbClass == arg_45_0.verbProps.verbClass)
                                        {
                                            mightComp.RemovePawnAbility(mightComp.mimicAbility);
                                        }
                                        if (magicComp.mimicAbility != null && magicComp.mimicAbility.MainVerb.verbClass == arg_45_0.verbProps.verbClass)
                                        {
                                            magicComp.RemovePawnAbility(magicComp.mimicAbility);
                                        }
                                    }
                                }
                                
                                LocalTargetInfo target = toil.actor.jobs.curJob.GetTarget(TargetIndex.A);
                                bool canFreeIntercept2 = false;
                                arg_45_0.TryStartCastOn(target, false, canFreeIntercept2);

                                using (IEnumerator<Hediff> enumerator = this.pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        Hediff rec = enumerator.Current;
                                        if (rec.def == TorannMagicDefOf.TM_PossessionHD || rec.def == TorannMagicDefOf.TM_DisguiseHD || rec.def == TorannMagicDefOf.TM_DisguiseHD_I || rec.def == TorannMagicDefOf.TM_DisguiseHD_II || rec.def == TorannMagicDefOf.TM_DisguiseHD_III)
                                        {
                                            this.pawn.health.RemoveHediff(rec);
                                        }
                                    }
                                }
                            };
                            toil.tickAction = delegate
                            {
                                if (Find.TickManager.TicksGame % 12 == 0)
                                {
                                    TM_MoteMaker.ThrowCastingMote(pawn.DrawPos, pawn.Map, Rand.Range(1.2f, 2f));
                                }
                                this.duration--;
                            };
                            toil.AddFinishAction(delegate
                            {
                                if (this.duration <= 5)
                                {
                                    curJob.Ability.PostAbilityAttempt();
                                }                               
                            });
                            toil.defaultCompleteMode = ToilCompleteMode.FinishedBusy;
                            yield return toil;

                            //Toil toil1 = new Toil()
                            //{
                            //    initAction = () => curJob.Ability.PostAbilityAttempt(),
                            //    defaultCompleteMode = ToilCompleteMode.Instant
                            //};
                            //yield return toil1;
                        }
                        else
                        {
                            //No LoS
                            if (pawn.IsColonist)
                            {
                                Messages.Message("TM_OutOfLOS".Translate(new object[]
                                {
                                    pawn.LabelShort
                                }), MessageTypeDefOf.RejectInput);
                            }
                        }
                    }
                }
                else
                {
                    if (pawn.IsColonist)
                    {
                        //out of range
                        Messages.Message("TM_OutOfRange".Translate(), MessageTypeDefOf.RejectInput);
                    }
                }
            }
        }
    }
}
