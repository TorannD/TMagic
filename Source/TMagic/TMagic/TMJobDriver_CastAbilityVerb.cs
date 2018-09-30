using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using AbilityUser;
using UnityEngine;

namespace TorannMagic
{
    public class TMJobDriver_CastAbilityVerb : JobDriver_CastAbilityVerb
    { 

        private int duration;
        public AbilityContext context => job.count == 1 ? AbilityContext.Player : AbilityContext.AI;
        public Verb_UseAbility verb = new Verb_UseAbility(); // = this.pawn.CurJob.verbToUse as Verb_UseAbility;

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
            this.verb = this.pawn.CurJob.verbToUse as Verb_UseAbility;

            if (base.TargetA.HasThing)
            {
                if (!base.GetActor().IsFighting() ? true : !verb.UseAbilityProps.canCastInMelee)
                {
                    Toil toil = Toils_Combat.GotoCastPosition(TargetIndex.A, false);
                    yield return toil;
                    toil = null;
                }
            }
            if (this.Context == AbilityContext.Player)
            {
                Find.Targeter.targetingVerb = this.verb;
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
                //JobDriver curDriver = this.pawn.jobs.curDriver;
                combatToil.initAction = delegate
                {
                    this.verb = combatToil.actor.jobs.curJob.verbToUse as Verb_UseAbility;                    
                    this.duration = (int)((this.verb.verbProps.warmupTime * 60) * this.pawn.GetStatValue(StatDefOf.AimingDelayFactor, false));
                    
                    if (this.pawn.RaceProps.Humanlike)
                    {
                        if (this.pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                        {
                            RemoveMimicAbility(verb);
                        }

                        if (this.pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic))
                        {
                            PsionicEnergyCost(verb);                            
                        }

                        if(verb.Ability.CooldownTicksLeft != -1)
                        {
                            this.EndJobWith(JobCondition.Incompletable);
                        }
                    }                    
                    
                    LocalTargetInfo target = combatToil.actor.jobs.curJob.GetTarget(TargetIndex.A);
                    verb.TryStartCastOn(target, false, false);
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
                        verb.Ability.PostAbilityAttempt();                        
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
                if ((pawn.Position - TargetLocA).LengthHorizontal < verb.verbProps.range)
                {
                    if (TargetLocA.IsValid && TargetLocA.InBounds(pawn.Map) && !TargetLocA.Fogged(pawn.Map))  //&& TargetLocA.Walkable(pawn.Map)
                    {
                        ShootLine shootLine;
                        bool validTarg = verb.TryFindShootLineFromTo(pawn.Position, TargetLocA, out shootLine);
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
                            this.duration = (int)((verb.verbProps.warmupTime * 60) * this.pawn.GetStatValue(StatDefOf.AimingDelayFactor, false));
                            Toil toil = new Toil();
                            toil.initAction = delegate
                            {
                                this.verb = toil.actor.jobs.curJob.verbToUse as Verb_UseAbility;
                                if (this.pawn.RaceProps.Humanlike)
                                {
                                    if (this.pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                                    {
                                        RemoveMimicAbility(verb);                                        
                                    }

                                    if(this.pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic))
                                    {
                                        PsionicEnergyCost(verb);
                                    }

                                    if (verb.Ability.CooldownTicksLeft != -1)
                                    {
                                        this.EndJobWith(JobCondition.Incompletable);
                                    }

                                }
                                LocalTargetInfo target = toil.actor.jobs.curJob.GetTarget(TargetIndex.A); //TargetLocA;  //
                                bool canFreeIntercept2 = false;
                                verb.TryStartCastOn(target, false, canFreeIntercept2);
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
                                    verb.Ability.PostAbilityAttempt();
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
                            pawn.ClearAllReservations(false);
                        }
                    }
                    else
                    {
                        pawn.ClearAllReservations(false);
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

        private void PsionicEnergyCost(Verb_UseAbility verbCast)
        {
            if (verbCast.AbilityProjectileDef.defName == "TM_Projectile_PsionicBlast")
            {
                HealthUtility.AdjustSeverity(this.pawn, HediffDef.Named("TM_PsionicHD"), -20f);
            }
            else if (verbCast.AbilityProjectileDef.defName == "Projectile_PsionicDash")
            {
                float sevReduct = 8f - this.pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_PsionicDash.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicDash_eff").level;
                HealthUtility.AdjustSeverity(this.pawn, HediffDef.Named("TM_PsionicHD"), -sevReduct);
            }
            else if(verbCast.AbilityProjectileDef.defName == "Projectile_PsionicStorm")
            {
                //float sevReduct = 65 - (5 * this.pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_PsionicStorm.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicStorm_eff").level);
                HealthUtility.AdjustSeverity(this.pawn, HediffDef.Named("TM_PsionicHD"), -100);
            }
        }

        private void RemoveMimicAbility(Verb_UseAbility verbCast)
        {
            CompAbilityUserMight mightComp = this.pawn.GetComp<CompAbilityUserMight>();
            CompAbilityUserMagic magicComp = this.pawn.GetComp<CompAbilityUserMagic>();
            if (mightComp.mimicAbility != null && mightComp.mimicAbility.MainVerb.verbClass == verbCast.verbProps.verbClass)
            {
                mightComp.RemovePawnAbility(mightComp.mimicAbility);
            }
            if (magicComp.mimicAbility != null && magicComp.mimicAbility.MainVerb.verbClass == verbCast.verbProps.verbClass)
            {
                magicComp.RemovePawnAbility(magicComp.mimicAbility);
            }
        }
    }
}
