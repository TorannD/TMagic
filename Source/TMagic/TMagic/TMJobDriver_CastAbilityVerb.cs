using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using AbilityUser;

namespace TorannMagic
{
    public class TMJobDriver_CastAbilityVerb : JobDriver_CastAbilityVerb
    {
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
                yield return Toils_Combat.CastVerb(TargetIndex.A, false);
                Toil toil2 = new Toil()
                {
                    initAction = () =>
                    {
                        if (curJob.UseAbilityProps.isViolent)
                        {
                            JobDriver_CastAbilityVerb.CheckForAutoAttack(this.pawn);
                        }
                    },
                    defaultCompleteMode = ToilCompleteMode.Instant
                };
                yield return toil2;
                Toil toil1 = new Toil()
                {
                    initAction = () => curJob.Ability.PostAbilityAttempt(),
                    defaultCompleteMode = ToilCompleteMode.Instant
                };
                yield return toil1;
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
                            yield return Toils_Combat.CastVerb(TargetIndex.A, false);
                            Toil toil2 = new Toil()
                            {
                                initAction = () =>
                                {
                                    if (curJob.UseAbilityProps.isViolent)
                                    {
                                        JobDriver_CastAbilityVerb.CheckForAutoAttack(this.pawn);
                                    }
                                },
                                defaultCompleteMode = ToilCompleteMode.Instant
                            };
                            yield return toil2;
                            Toil toil1 = new Toil()
                            {
                                initAction = () => curJob.Ability.PostAbilityAttempt(),
                                defaultCompleteMode = ToilCompleteMode.Instant
                            };
                            yield return toil1;
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
