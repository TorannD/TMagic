using RimWorld;
using RimWorld.BaseGen;
using System;
using Verse;

namespace TorannMagic
{
    class GenStep_ArcaneStash : GenStep_MagicAdventureGenerator
    {
        public override void Generate(Map map)
        {
            base.Generate(map);

            CellRect rect = new CellRect(Rand.RangeInclusive(this.adventureRegion.minX, this.adventureRegion.maxX - 60), Rand.RangeInclusive(this.adventureRegion.minZ + 15, this.adventureRegion.maxZ - 15), 40, 40);
            rect.ClipInsideMap(map);
            ResolveParams baseResolveParams = this.baseResolveParams;
            baseResolveParams.rect = rect;
            BaseGen.symbolStack.Push("arcaneTower", baseResolveParams);
            
            MapGenUtility.MakeDoors(new ResolveParams
            {
                wallStuff = ThingDefOf.Plasteel
            }, map);
            MapGenUtility.ResolveCustomGenSteps(map);
            BaseGen.Generate();
        }
    }
}
