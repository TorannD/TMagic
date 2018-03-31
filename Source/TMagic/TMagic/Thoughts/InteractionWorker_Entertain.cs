using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace TorannMagic.Thoughts
{
    public class InteractionWorker_Entertain : InteractionWorker
    {
        public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks)
        {
            CompAbilityUserMagic compInit = initiator.GetComp<CompAbilityUserMagic>();
            base.Interacted(initiator, recipient, extraSentencePacks);
            int num =  Rand.Range(50, 100);
            compInit.MagicUserXP += num;
            MoteMaker.ThrowText(initiator.DrawPos, initiator.MapHeld, "XP +" + num, -1f);
        }

    }
}
