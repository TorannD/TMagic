using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using AbilityUser;

namespace TorannMagic
{
    public class TMJobDriver_CastAbilitySelf : JobDriver_CastAbilityVerb
    {
        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Misc.ThrowColonistAttackingMote(TargetIndex.A);
            Verb_UseAbility curJob = this.pawn.CurJob.verbToUse as Verb_UseAbility;
            Find.Targeter.targetingVerb = curJob;            
            yield return Toils_Combat.CastVerb(TargetIndex.A, false);
            Toil toil1 = new Toil()
            {
                initAction = () => {
                    if (curJob.UseAbilityProps.isViolent)
                    {
                        JobDriver_CastAbilityVerb.CheckForAutoAttack(this.pawn);
                    }
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
            yield return toil1;
            Toil toil = new Toil()
            {
                initAction = () => curJob.Ability.PostAbilityAttempt(),
                defaultCompleteMode = ToilCompleteMode.Instant
            };
            yield return toil;
        }
    }
}
