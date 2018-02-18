using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace TorannMagic
{
    public class GenStep_ArcaneStashTreasure : GenStep
    {
        public override void Generate(Map map)
        {
            System.Random random = new System.Random();
            float num = Mathf.Min(10000f, (float)GenMath.RoundRandom(random.Next(10000, 12000)));
            ItemCollectionGenerator_Arcane itemCollectionGenerator_Arcane = new ItemCollectionGenerator_Arcane();
            ItemCollectionGeneratorParams itemCollectionGeneratorParams = default(ItemCollectionGeneratorParams);
            itemCollectionGeneratorParams.techLevel = (TechLevel)5;
            itemCollectionGeneratorParams.totalMarketValue = num;
            itemCollectionGeneratorParams.count = Rand.RangeInclusive(10, 20);
            if (num > 12000f)
            {
                itemCollectionGeneratorParams.count = 1;
            }
            itemCollectionGeneratorParams.validator = ((ThingDef t) => t.defName != "Silver");
            List<Thing> list = new List<Thing>();
            Thing item = new Thing();
            //random = new System.Random();
            //int rnd = GenMath.RoundRandom(random.Next(0, 10));
            //if (rnd < 2)
            //{
            //    item.def = TorannMagicDefOf.BookOfInnerFire;
            //}
            //else if (rnd >= 2 && rnd < 4)
            //{
            //    item.def = TorannMagicDefOf.BookOfHeartOfFrost;
            //}
            //else if (rnd >= 4 && rnd < 6)
            //{
            //    item.def = TorannMagicDefOf.BookOfStormBorn;
            //}
            //else if (rnd >= 6 && rnd < 8)
            //{
            //    item.def = TorannMagicDefOf.BookOfArcanist;
            //}
            //else
            //{
            //    item.def = TorannMagicDefOf.BookOfValiant;
            //}
            //item.stackCount = 1;

            //list.Add(item);
            foreach (Thing current in list)
            {
                if (current.stackCount > current.def.stackLimit)
                {
                    current.stackCount = current.def.stackLimit;
                }
                IntVec3 intVec;
                if (CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => GenGrid.Standable(x, map) && GridsUtility.Fogged(x, map) && GridsUtility.GetRoom(x, map, (RegionType)6).CellCount >= 2, map, 1000, out intVec))
                {
                    
                    GenSpawn.Spawn(current, intVec, map, Rot4.Random, false);
                }
            }
            
            
            list = itemCollectionGenerator_Arcane.Generate(itemCollectionGeneratorParams);
            foreach (Thing current in list)
            {
                if (current.stackCount > current.def.stackLimit)
                {
                    current.stackCount = current.def.stackLimit;
                }
                IntVec3 intVec;
                if (CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => GenGrid.Standable(x, map) && GridsUtility.Fogged(x, map) && GridsUtility.GetRoom(x, map, (RegionType)6).CellCount >= 2, map, 1000, out intVec))
                {
                    
                    GenSpawn.Spawn(current, intVec, map, Rot4.North, false);
                }
            }
        }
    }
}
