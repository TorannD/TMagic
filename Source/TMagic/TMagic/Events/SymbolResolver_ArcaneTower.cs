using RimWorld;
using RimWorld.BaseGen;
using System;
using System.Collections.Generic;
using Verse;

namespace TorannMagic
{
    class SymbolResolver_ArcaneTower : SymbolResolver
    {
        public override bool CanResolve(ResolveParams rp)
        {
            return base.CanResolve(rp);
        }

        public override void Resolve(ResolveParams rp)
        {
            ResolveParams resolveParams = rp;
            resolveParams.rect = rp.rect.ContractedBy(1);
            resolveParams.wallStuff = ThingDefOf.BlocksGranite;
            rp.wallStuff = ThingDefOf.Steel;
            resolveParams.SetCustom<int>("minRoomDimension", 6, false);
            BaseGen.globalSettings.minBuildings = 3;
            BaseGen.globalSettings.minBarracks = 1;
            BaseGen.symbolStack.Push("factionBase", resolveParams);
            
            
            
            //BaseGen.symbolStack.Push("edgeStreet", resolveParams);
            //BaseGen.symbolStack.Push("edgeDefense", resolveParams);
            BaseGen.symbolStack.Push("ancientCryptosleepCasket", rp);
            BaseGen.symbolStack.Push("ancientCryptosleepCasket", rp);
            BaseGen.symbolStack.Push("ancientCryptosleepCasket", rp);
            

            CellRect field = rp.rect;
            field.minX = Rand.RangeInclusive(rp.rect.minX, rp.rect.maxX - 15);
            field.minZ = Rand.RangeInclusive(rp.rect.minZ + 15, rp.rect.maxZ - 15);
            field.Width = 5;
            field.Height = 4;
            //BaseGen.symbolStack.Push("cultivatedPlants", field);
            BaseGen.symbolStack.Push("wireOutline", rp);
            //field.minX = Rand.RangeInclusive(rp.rect.minX, rp.rect.maxX);
            //field.minZ = Rand.RangeInclusive(rp.rect.minZ, rp.rect.maxZ);
            //field.Width = 4;
            //field.Height = 4;
            //BaseGen.symbolStack.Push("batteryRoom", field);
            //field.Width = 2;
            //field.Height = 2;
            //BaseGen.symbolStack.Push("wireOutline", field);
            BaseGen.symbolStack.Push("chargeBatteries", resolveParams);

            BaseGen.symbolStack.Push("clear", rp);

        }
    }
}
