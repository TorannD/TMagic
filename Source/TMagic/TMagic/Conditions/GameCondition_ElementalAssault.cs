using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Verse;
using RimWorld;

namespace TorannMagic.Conditions
{
    class GameCondition_ElementalAssault : GameCondition
    {
        public IntVec2 centerLocation;
        private int areaRadius = 2;
        bool initialized = false;
        bool disabled = false;

        public override void ExposeData()
        {
            base.ExposeData();
            //Scribe_Deep.Look<Thing>(ref this.thing, "thing", new object[0]);
            Scribe_Values.Look<bool>(ref this.initialized, "initialized", true, false);
            Scribe_Values.Look<IntVec2>(ref this.centerLocation, "centerLocation", default(IntVec2), false);
            Scribe_Values.Look<bool>(ref this.disabled, "disabled", false, false);
        }

        public override void Init()
        {           
            base.Init();
            this.disabled = false;
            this.FindGoodCenterLocation();
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (settingsRef.riftChallenge > 0)
            {
                Thing thing = ThingMaker.MakeThing(ThingDef.Named("TM_ElementalRift"), ThingDefOf.BlocksGranite);
                GenSpawn.Spawn(thing, centerLocation.ToIntVec3, this.Map, Rot4.North, false);
                Faction faction = Find.FactionManager.FirstFactionOfDef(FactionDef.Named("TM_ElementalFaction"));
                if (!faction.HostileTo(Faction.OfPlayer))
                {
                    thing.SetFaction(Faction.OfMechanoids, null);
                }
                else
                {
                    thing.SetFaction(faction, null);
                }
            }
            else
            {
                this.disabled = true;
                Log.Message("Rift spawning disabled.");
            }
        }

        public override void End()
        {
            IntVec3 thingLoc = centerLocation.ToIntVec3;
            List<Thing> thingList;
            Thing destroyable = null;
            thingList = thingLoc.GetThingList(base.Map);
            int z = 0;
            while (z < thingList.Count)
            {
                destroyable = thingList[z];
                if (destroyable != null)
                {
                    bool validator = destroyable is Building;
                    if (validator)
                    {
                        if(destroyable.def == ThingDef.Named("TM_ElementalRift"))
                        {
                            destroyable.Destroy(DestroyMode.Vanish);
                        }
                    }
                }
                z++;
            }
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (!this.disabled)
            {
                Thing thing = null;
                thing = ThingMaker.MakeThing(ThingDef.Named("Jade"));
                thing.stackCount = Rand.Range(35 * (int)settingsRef.riftChallenge, 60 * (int)settingsRef.riftChallenge);
                if (thing != null)
                {
                    GenPlace.TryPlaceThing(thing, thingLoc, this.Map, ThingPlaceMode.Near, null);
                }
                ItemCollectionGeneratorParams parms = default(ItemCollectionGeneratorParams);
                parms.techLevel = TechLevel.Neolithic;
                parms.totalMarketValue = 1000f * (settingsRef.riftChallenge * settingsRef.riftChallenge);
                List<Thing> list = new List<Thing>();
                ItemCollectionGenerator_Gemstones itc_g = new ItemCollectionGenerator_Gemstones();
                list = itc_g.Generate(parms, list);
                for (int i = 0; i < list.Count; i++)
                {
                    thing = list[i];
                    thing.stackCount = list[i].stackCount;
                    GenPlace.TryPlaceThing(thing, thingLoc, this.Map, ThingPlaceMode.Near, null);
                }
            }
            base.End();
        }

        private void FindGoodCenterLocation()
        {
            if (base.Map.Size.x <= 16 || base.Map.Size.z <= 16)
            {
                throw new Exception("Map too small for elemental assault");
            }
            for (int i = 0; i < 10; i++)
            {
                this.centerLocation = new IntVec2(Rand.Range(8, base.Map.Size.x - 8), Rand.Range(8, base.Map.Size.z - 8));
                if (this.IsGoodCenterLocation(this.centerLocation))
                {
                    break;
                }
            }
        }

        private bool IsGoodLocationForSpawn(IntVec3 loc)
        {
            return loc.InBounds(base.Map) && !loc.Roofed(base.Map) && loc.Standable(base.Map) && loc.IsValid && !loc.Fogged(base.Map) && loc.Walkable(base.Map);
        }

        private bool IsGoodCenterLocation(IntVec2 loc)
        {
            int num = 0;
            int num2 = (int)(3.14159274f * (float)this.areaRadius * (float)this.areaRadius / 2f);
            foreach (IntVec3 current in this.GetPotentiallyAffectedCells(loc))
            {
                if (this.IsGoodLocationForSpawn(current))
                {
                    num++;
                }
                if (num >= num2)
                {
                    break;
                }
            }
            
            return num >= num2 && (IsGoodLocationForSpawn(loc.ToIntVec3));
        }

        [DebuggerHidden]
        private IEnumerable<IntVec3> GetPotentiallyAffectedCells(IntVec2 center)
        {
            for (int x = center.x - this.areaRadius; x <= center.x + this.areaRadius; x++)
            {
                for (int z = center.z - this.areaRadius; z <= center.z + this.areaRadius; z++)
                {
                    if ((center.x - x) * (center.x - x) + (center.z - z) * (center.z - z) <= this.areaRadius * this.areaRadius)
                    {
                        yield return new IntVec3(x, 0, z);
                    }
                }
            }
        }
    }
}
