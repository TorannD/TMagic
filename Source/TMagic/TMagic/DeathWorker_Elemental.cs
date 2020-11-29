using RimWorld;
using System;
using Verse;

namespace TorannMagic
{
    public class DeathWorker_Elemental : DeathActionWorker
    {
        public override void PawnDied(Corpse corpse)
        {
            if (corpse.InnerPawn.Faction == Faction.OfPlayer)
            {
                for (int i = 0; i < 3; i++)
                {
                    MoteMaker.ThrowSmoke(corpse.DrawPos, corpse.Map, Rand.Range(.5f, 1.1f));
                }
                MoteMaker.ThrowHeatGlow(corpse.Position, corpse.Map, 1f);
                corpse.Destroy();
            }
        }
    }
}
