using System;
using Verse;
using RimWorld;

namespace TorannMagic.Thoughts
{
    public class ThoughtWorker_TM_HolierThanThou : ThoughtWorker
    {
        protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
        {
            if (pawn != null && other != null)
            {
                if ((other.story.traits.HasTrait(TorannMagicDefOf.Paladin) || other.story.traits.HasTrait(TorannMagicDefOf.Druid) || other.story.traits.HasTrait(TorannMagicDefOf.Priest)) && pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
