using System;
using System.Collections.Generic;
using Verse;
using Harmony;

namespace TorannMagic.ModOptions
{
    public abstract class Constants
    {
        private static bool pawnInFlight = false;

        public static bool GetPawnInFlight()
        {
            return pawnInFlight;
        }

        public static bool SetPawnInFlight(bool inFlight)
        {
            pawnInFlight = inFlight;
            return true;
        }

        static List<IntVec3> growthCells = new List<IntVec3>();

        public static List<IntVec3> GetGrowthCells()
        {
            return growthCells;
        }

        public static List<IntVec3> SetGrowthCells(List<IntVec3> cells)
        {
            growthCells.AddRange(cells);
            return growthCells;
        }

        public static void RemoveGrowthCell(IntVec3 cell)
        {
            growthCells.Remove(cell);
        }

        private static int lastGrowthMoteTick;

        public static void SetLastGrowthMoteTick(int value)
        {
            lastGrowthMoteTick = value;
        }

        public static int GetLastGrowthMoteTick()
        {
            return lastGrowthMoteTick;
        }
    }
}
