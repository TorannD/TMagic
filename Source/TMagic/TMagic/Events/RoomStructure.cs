using RimWorld;
using RimWorld.BaseGen;
using System;
using Verse;

namespace TorannMagic
{
    public class RoomStructure
    {
        public float wallN = 1f;

        public float wallS = 1f;

        public float wallE = 1f;

        public float wallW = 1f;

        public float floorChance = 1f;

        public float roofChance = 1f;

        public int doorN = 0;

        public int doorS = 0;

        public int doorE = 0;

        public int doorW = 0;

        public ThingDef wallMaterial = BaseGenUtility.RandomCheapWallStuff(Find.FactionManager.FirstFactionOfDef(FactionDefOf.Spacer), false);

        public TerrainDef floorMaterial = BaseGenUtility.RandomBasicFloorDef(Faction.OfMechanoids, false);

        public void damage()
        {
            float value = Rand.Value;
            if ((double)value < 0.25)
            {
                this.wallN = 0f;
            }
            else if ((double)value < 0.5)
            {
                this.wallS = 0f;
            }
            else if ((double)value < 0.75)
            {
                this.wallS = 0f;
            }
            else
            {
                this.wallS = 0f;
            }
        }

        public void delapidate()
        {
            float num = 0.2f + 0.6f * Rand.Value;
            this.wallN = num - 0.2f + 0.4f * Rand.Value;
            this.wallS = num - 0.2f + 0.4f * Rand.Value;
            this.wallE = num - 0.2f + 0.4f * Rand.Value;
            this.wallW = num - 0.2f + 0.4f * Rand.Value;
        }
    }
}
