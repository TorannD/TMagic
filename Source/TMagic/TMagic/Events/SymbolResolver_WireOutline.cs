using RimWorld;
using RimWorld.BaseGen;
using System;
using Verse;


namespace TorannMagic
{
    public class SymbolResolver_WireOutline : SymbolResolver
    {

        public override bool CanResolve(ResolveParams rp)
        {
            return base.CanResolve(rp);
        }

        public override void Resolve(ResolveParams rp)
        {
            float? chanceToSkipWallBlock = rp.chanceToSkipWallBlock;
            float num = (!chanceToSkipWallBlock.HasValue) ? 0f : chanceToSkipWallBlock.Value;
            foreach (IntVec3 current in rp.rect.EdgeCells)
            {
                if (!Rand.Chance(num))
                {
                    ThingDef powerConduit = ThingDefOf.PowerConduit;
                    Thing thing = ThingMaker.MakeThing(powerConduit, null);
                    GenSpawn.Spawn(thing, current, BaseGen.globalSettings.map);
                }
            }
            }
        
    }
}
