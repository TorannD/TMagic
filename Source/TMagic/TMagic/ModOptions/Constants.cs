using System;
using Harmony;
using System.Collections.Generic;
using Verse;

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

        private static int technoWeaponCount;

        public static void SetTechnoWeaponCount(int value)
        {
            technoWeaponCount = value;
        }

        public static int GetTechnoWeaponCount()
        {
            return technoWeaponCount;
        }

        private static bool bypassPrediction = false;

        public static bool GetBypassPrediction()
        {
            return bypassPrediction;
        }

        public static bool SetBypassPrediction(bool value)
        {
            bypassPrediction = value;
            return bypassPrediction;
        }

        static List<Pawn> overdrivePawns = new List<Pawn>();

        public static List<Pawn> SetOverdrivePawnList(List<Pawn> value)
        {            
            if(overdrivePawns == null)
            {
                overdrivePawns = new List<Pawn>();
            }
            for(int i = 0; i < value.Count; i++)
            {
                overdrivePawns.AddDistinct<Pawn>(value[i]);
            }
            return overdrivePawns;
        }

        public static List<Pawn> GetOverdrivePawnList()
        {
            if(overdrivePawns == null)
            {
                overdrivePawns = new List<Pawn>();
                overdrivePawns.Clear();
            }
            return overdrivePawns;
        }
    }
}
