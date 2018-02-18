using RimWorld;
using RimWorld.BaseGen;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TorannMagic
{
    public class MapGenUtility
    {
        private static List<IntVec3> DoorPosList = new List<IntVec3>();

        private static List<ThingStuffPair> weapons;

        private static List<GenStepDef> customGenSteps = new List<GenStepDef>();

        public static void AddGenStep(GenStepDef step)
        {
            MapGenUtility.customGenSteps.Add(step);
        }

        public static void ResolveCustomGenSteps(Map map)
        {
            foreach (GenStepDef current in MapGenUtility.customGenSteps)
            {
                current.genStep.Generate(map);
            }
            MapGenUtility.customGenSteps.Clear();
        }

        public static bool TryFindRandomCellWhere(IEnumerable<IntVec3> candidates, Predicate<IntVec3> validator, out IntVec3 loc)
        {
            loc = default(IntVec3);
            IEnumerable<IntVec3> enumerable = from v in candidates
                                              where validator(v)
                                              select v;
            bool result;
            if (enumerable.Count<IntVec3>() > 0)
            {
                loc = GenCollection.RandomElement<IntVec3>(enumerable);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public static void MakeRoom(Map map, CellRect rect, RoomStructure rs)
        {
            if (rs.wallMaterial == null)
            {
                rs.wallMaterial = BaseGenUtility.RandomCheapWallStuff(Find.FactionManager.FirstFactionOfDef(FactionDefOf.Spacer), false);
            }
            if (rs.floorMaterial == null)
            {
                rs.floorMaterial = BaseGenUtility.CorrespondingTerrainDef(rs.wallMaterial, true);
            }
            if (rs.floorMaterial == null)
            {
                rs.floorMaterial = BaseGenUtility.RandomBasicFloorDef(Faction.OfMechanoids, false);
            }
            MapGenUtility.TileArea(map, rect, rs.floorMaterial, rs.floorChance);
            MapGenUtility.MakeLongWall(new IntVec3(rect.minX, 1, rect.minZ), map, rect.Width, true, rs.wallS, rs.wallMaterial);
            MapGenUtility.MakeLongWall(new IntVec3(rect.minX, 1, rect.maxZ), map, rect.Width, true, rs.wallN, rs.wallMaterial);
            MapGenUtility.MakeLongWall(new IntVec3(rect.minX, 1, rect.minZ), map, rect.Height, false, rs.wallW, rs.wallMaterial);
            MapGenUtility.MakeLongWall(new IntVec3(rect.maxX, 1, rect.minZ), map, rect.Height, false, rs.wallE, rs.wallMaterial);
            for (int i = 0; i < rs.doorN; i++)
            {
                MapGenUtility.RandomAddDoor(new IntVec3(rect.minX + 2, 1, rect.maxZ), map, rect.Width - 3, true, rs.wallMaterial);
            }
            for (int j = 0; j < rs.doorS; j++)
            {
                MapGenUtility.RandomAddDoor(new IntVec3(rect.minX + 2, 1, rect.minZ), map, rect.Width - 3, true, rs.wallMaterial);
            }
            for (int k = 0; k < rs.doorE; k++)
            {
                MapGenUtility.RandomAddDoor(new IntVec3(rect.minX, 1, rect.minZ + 2), map, rect.Height - 3, false, rs.wallMaterial);
            }
            for (int l = 0; l < rs.doorW; l++)
            {
                MapGenUtility.RandomAddDoor(new IntVec3(rect.maxX, 1, rect.minZ + 2), map, rect.Height - 3, false, rs.wallMaterial);
            }
            map.MapUpdate();
        }

        public static List<ThingStuffPair> GetWeapons(Predicate<ThingDef> validator)
        {
            List<ThingStuffPair> list = new List<ThingStuffPair>();
            if (GenList.NullOrEmpty<ThingStuffPair>(MapGenUtility.weapons))
            {
                MapGenUtility.FillPossibleObjectLists();
            }
            foreach (ThingStuffPair current in MapGenUtility.weapons)
            {
                if (validator(current.thing))
                {
                    list.Add(current);
                }
            }
            return list;
        }

        public static void MakeTriangularRoom(Map map, ResolveParams rp)
        {
            if (rp.wallStuff == null)
            {
                rp.wallStuff = BaseGenUtility.RandomCheapWallStuff(Find.FactionManager.FirstFactionOfDef(FactionDefOf.Spacer), false);
            }
            if (rp.floorDef == null)
            {
                rp.floorDef = BaseGenUtility.CorrespondingTerrainDef(rp.wallStuff, true);
            }
            if (rp.floorDef == null)
            {
                rp.floorDef = BaseGenUtility.RandomBasicFloorDef(Faction.OfMechanoids, false);
            }
            ResolveParams resolveParams = rp;
            resolveParams.rect = new CellRect(rp.rect.minX, rp.rect.minZ, 1, rp.rect.Height);
            BaseGen.symbolStack.Push("edgeWalls", resolveParams);
            for (int i = 0; i <= rp.rect.Width; i++)
            {
                int num = rp.rect.minX + i;
                int num2 = (int)Math.Floor((double)(0.5f * (float)i));
                int num3 = (int)Math.Ceiling((double)(0.5f * (float)i));
                for (int j = rp.rect.minZ + num2; j < rp.rect.minZ + rp.rect.Width - num2; j++)
                {
                    foreach (Thing current in map.thingGrid.ThingsAt(new IntVec3(num, 1, j)))
                    {
                        current.TakeDamage(new DamageInfo(DamageDefOf.Blunt, 10000, -1f, null, null, null, 0));
                    }
                    MapGenUtility.TryToSetFloorTile(new IntVec3(num, 1, j), map, rp.floorDef);
                    if (j == rp.rect.minZ + num2 || j == rp.rect.minZ + num3 || j == rp.rect.minZ + rp.rect.Width - (num2 + 1) || j == rp.rect.minZ + rp.rect.Width - (num3 + 1))
                    {
                        ResolveParams resolveParams2 = rp;
                        resolveParams2.rect = new CellRect(num, j, 1, 1);
                        BaseGen.symbolStack.Push("edgeWalls", resolveParams2);
                    }
                }
            }
            map.MapUpdate();
            RoofGrid roofGrid = BaseGen.globalSettings.map.roofGrid;
            RoofDef roofDef = rp.roofDef ?? RoofDefOf.RoofConstructed;
            for (int k = 0; k <= rp.rect.Width; k++)
            {
                int num4 = rp.rect.minX + k;
                int num5 = (int)Math.Floor((double)(0.5f * (float)k));
                for (int l = rp.rect.minZ + num5; l < rp.rect.minZ + rp.rect.Width - num5; l++)
                {
                    IntVec3 intVec = new IntVec3(num4, 1, l);
                    if (!roofGrid.Roofed(intVec))
                    {
                        roofGrid.SetRoof(intVec, roofDef);
                    }
                }
            }
        }

        public static void DestroyAllInArea(Map map, CellRect rect)
        {
            for (int i = rect.minX; i <= rect.maxX; i++)
            {
                for (int j = rect.minZ; j <= rect.maxZ; j++)
                {
                    IntVec3 current = new IntVec3(i, 1, j);
                    MapGenUtility.DestroyAllAtLocation(current, map);
                    rect.GetIterator().MoveNext();
                    current = rect.GetIterator().Current;
                }
            }
        }

        public static void TileArea(Map map, CellRect rect, TerrainDef floorMaterial = null, float floorIntegrity = 1f)
        {
            if (floorMaterial == null)
            {
                floorMaterial = BaseGenUtility.RandomBasicFloorDef(Faction.OfMechanoids, false);
            }
            for (int i = rect.minX; i <= rect.maxX; i++)
            {
                for (int j = rect.minZ; j <= rect.maxZ; j++)
                {
                    IntVec3 current = new IntVec3(i, 1, j);
                    if (Rand.Value <= floorIntegrity)
                    {
                        MapGenUtility.TryToSetFloorTile(current, map, floorMaterial);
                        rect.GetIterator().MoveNext();
                        current = rect.GetIterator().Current;
                    }
                }
            }
        }

        public static void RoofArea(Map map, CellRect rect, float roofIntegrity = 1f)
        {
            for (int i = rect.minX; i <= rect.maxX; i++)
            {
                for (int j = rect.minZ; j <= rect.maxZ; j++)
                {
                    IntVec3 intVec = new IntVec3(i, 1, j);
                    if (Rand.Value <= roofIntegrity)
                    {
                        if (!map.roofGrid.Roofed(intVec))
                        {
                            map.roofGrid.SetRoof(intVec, RoofDefOf.RoofConstructed);
                        }
                    }
                }
            }
        }

        public static void RandomAddDoor(IntVec3 start, Map map, int extent, bool horizontal, ThingDef material = null)
        {
            if (material == null)
            {
                material = BaseGenUtility.RandomCheapWallStuff(Find.FactionManager.FirstFactionOfDef(FactionDefOf.Spacer), false);
            }
            int num = Rand.RangeInclusive(0, extent);
            if (horizontal)
            {
                start.x += num;
            }
            else
            {
                start.z += num;
            }
            MapGenUtility.TryMakeDoor(start, map, material);
        }

        public static void FillPossibleObjectLists()
        {
            if (GenList.NullOrEmpty<ThingStuffPair>(MapGenUtility.weapons))
            {
                MapGenUtility.weapons = ThingStuffPair.AllWith((ThingDef td) => td.IsWeapon && !GenList.NullOrEmpty<string>(td.weaponTags));
            }
        }

        public static void ScatterWeaponsWhere(CellRect within, int num, Map map, Predicate<ThingDef> validator)
        {
            List<ThingStuffPair> list = MapGenUtility.GetWeapons(validator);
            for (int i = 0; i < num; i++)
            {
                ThingStuffPair thingStuffPair = GenCollection.RandomElement<ThingStuffPair>(list);
                Thing thing = ThingMaker.MakeThing(thingStuffPair.thing, thingStuffPair.stuff);
                CompQuality compQuality = ThingCompUtility.TryGetComp<CompQuality>(thing);
                if (compQuality != null)
                {
                    compQuality.SetQuality(QualityUtility.RandomCreationQuality(12), 0);
                }
                IntVec3 intVec;
                if (thing != null && CellFinder.TryFindRandomCellInsideWith(within, (IntVec3 loc) => GenGrid.Standable(loc, map), out intVec))
                {
                    GenSpawn.Spawn(thing, intVec, map);
                }
            }
        }

        public static void MakeLongWall(IntVec3 start, Map map, int extendDist, bool horizontal, float integrity, ThingDef stuffDef)
        {
            IntVec3 intVec = start;
            for (int i = 0; i < extendDist; i++)
            {
                if (!GenGrid.InBounds(intVec, map))
                {
                    break;
                }
                if (Rand.Value < integrity)
                {
                    MapGenUtility.TrySetCellAsWall(intVec, map, stuffDef);
                }
                if (horizontal)
                {
                    intVec.x++;
                }
                else
                {
                    intVec.z++;
                }
            }
        }

        private static void TrySetCellAsWall(IntVec3 c, Map map, ThingDef stuffDef)
        {
            List<Thing> thingList = GridsUtility.GetThingList(c, map);
            for (int i = 0; i < thingList.Count; i++)
            {
                if (!thingList[i].def.destroyable)
                {
                    return;
                }
            }
            for (int j = thingList.Count - 1; j >= 0; j--)
            {
                thingList[j].Destroy(0);
            }
            map.terrainGrid.SetTerrain(c, BaseGenUtility.CorrespondingTerrainDef(stuffDef, true));
            Thing thing = ThingMaker.MakeThing(ThingDefOf.Wall, stuffDef);
            GenSpawn.Spawn(thing, c, map);
        }

        public static void DestroyAllAtLocation(IntVec3 c, Map map)
        {
            List<Thing> thingList = GridsUtility.GetThingList(c, map);
            for (int i = 0; i < thingList.Count; i++)
            {
                if (!thingList[i].def.destroyable)
                {
                    return;
                }
            }
            for (int j = thingList.Count - 1; j >= 0; j--)
            {
                thingList[j].Destroy(0);
            }
        }

        private static void TryToSetFloorTile(IntVec3 c, Map map, TerrainDef floorDef)
        {
            List<Thing> thingList = GridsUtility.GetThingList(c, map);
            for (int i = 0; i < thingList.Count; i++)
            {
                if (!thingList[i].def.destroyable)
                {
                    return;
                }
            }
            for (int j = thingList.Count - 1; j >= 0; j--)
            {
                thingList[j].Destroy(0);
            }
            map.terrainGrid.SetTerrain(c, floorDef);
        }

        private static void TryMakeDoor(IntVec3 c, Map map, ThingDef doorStuff = null)
        {
            if (doorStuff == null)
            {
                doorStuff = BaseGenUtility.RandomCheapWallStuff(Find.FactionManager.FirstFactionOfDef(FactionDefOf.Spacer), false);
            }
            List<Thing> thingList = GridsUtility.GetThingList(c, map);
            for (int i = 0; i < thingList.Count; i++)
            {
                if (!thingList[i].def.destroyable)
                {
                    return;
                }
            }
            for (int j = thingList.Count - 1; j >= 0; j--)
            {
                thingList[j].Destroy(0);
            }
            Thing thing = ThingMaker.MakeThing(ThingDefOf.Door, doorStuff);
            GenSpawn.Spawn(thing, c, map);
        }

        public static void UnfogFromRandomEdge(Map map)
        {
            MapGenUtility.FogAll(map);
            MapGenerator.rootsToUnfog.Clear();
            foreach (Pawn current in map.mapPawns.FreeColonists)
            {
                MapGenerator.rootsToUnfog.Add(current.Position);
            }
            MapGenerator.rootsToUnfog.Add(CellFinderLoose.RandomCellWith((IntVec3 loc) => GenGrid.Standable(loc, map) && (loc.x < 4 || loc.z < 4 || loc.x > map.Size.x - 5 || loc.z > map.Size.z - 5), map, 1000));
            foreach (IntVec3 current2 in MapGenerator.rootsToUnfog)
            {
                FloodFillerFog.FloodUnfog(current2, map);
            }
        }

        public static void FogAll(Map map)
        {
            FogGrid fogGrid = map.fogGrid;
            if (fogGrid != null)
            {
                CellIndices cellIndices = map.cellIndices;
                if (fogGrid.fogGrid == null)
                {
                    fogGrid.fogGrid = new bool[cellIndices.NumGridCells];
                }
                foreach (IntVec3 current in map.AllCells)
                {
                    fogGrid.fogGrid[cellIndices.CellToIndex(current)] = true;
                }
                if (Current.ProgramState == (ProgramState)2)
                {
                    map.roofGrid.Drawer.SetDirty();
                }
            }
        }

        public static ThingDef RandomExpensiveWallStuff(Faction faction, bool notVeryFlammable = true)
        {
            ThingDef result;
            if (faction != null && TechLevelUtility.IsNeolithicOrWorse(faction.def.techLevel))
            {
                result = ThingDefOf.WoodLog;
            }
            else
            {
                result = GenCollection.RandomElement<ThingDef>(from d in DefDatabase<ThingDef>.AllDefsListForReading
                                                               where d.IsStuff && d.stuffProps.CanMake(ThingDefOf.Wall) && (!notVeryFlammable || d.BaseFlammability < 0.5f) && d.BaseMarketValue / d.VolumePerUnit > 8f
                                                               select d);
            }
            return result;
        }

        public static void PushDoor(IntVec3 loc)
        {
            MapGenUtility.DoorPosList.Add(loc);
        }

        public static void MakeDoors(ResolveParams rp, Map map)
        {
            foreach (IntVec3 current in MapGenUtility.DoorPosList)
            {
                MapGenUtility.DestroyAllAtLocation(current, map);
                Thing thing = ThingMaker.MakeThing(ThingDefOf.Door, rp.wallStuff);
                if (map.ParentFaction != null)
                {
                    thing.SetFaction(map.ParentFaction, null);
                }
                else
                {
                    thing.SetFaction(Find.FactionManager.FirstFactionOfDef(FactionDefOf.SpacerHostile), null);
                }
                GenSpawn.Spawn(thing, current, map);
            }
            MapGenUtility.DoorPosList.Clear();
        }
    }
}
