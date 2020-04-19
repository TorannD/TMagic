using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;
using AbilityUser;
using Verse.AI.Group;
using HarmonyLib;

namespace TorannMagic.Conditions
{
    class GameCondition_DemonAssault : GameCondition
    {
        public IntVec2 centerLocation;
        public IntVec2 edgeLocation;
        private int areaRadius = 4;
        bool initialized = false;
        bool disabled = false;
        public Thing thing;
        private int nextEventTick = 0;
        private int ticksBetweenEvents = 4000;
        IntVec3 rndTarg = default(IntVec3);
        private bool doEventAction = false;
        private int eventActionCount = 0;

        public override void GameConditionTick()
        {
            base.GameConditionTick();            
            if(Find.TickManager.TicksGame % 60 == 0)
            {
                if(this.thing.DestroyedOrNull())
                {
                    this.End();
                }

                if (this.nextEventTick <= Find.TickManager.TicksGame)
                {
                    this.nextEventTick = Find.TickManager.TicksGame + this.ticksBetweenEvents;                    
                }
            }
            if(Find.TickManager.TicksGame % 10 == 0 && doEventAction)
            {
                eventActionCount--;
                IntVec3 rndPos = rndTarg;
                rndPos.x += Rand.Range(-4, 4);
                rndPos.z += Rand.Range(-4, 4);
                SkyfallerMaker.SpawnSkyfaller(TorannMagicDefOf.TM_Firestorm_Small, rndPos, this.SingleMap);
                if(eventActionCount <= 0)
                {
                    doEventAction = false;
                }
            }
        }

        private void DoEvent()
        {
            IntVec3 rndTarg = FindEnemyPawnOrBuilding();
            doEventAction = true;
            eventActionCount = Rand.RangeInclusive(4, 6);            
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look<Thing>(ref this.thing, "thing", false);
            Scribe_Values.Look<bool>(ref this.initialized, "initialized", true, false);
            Scribe_Values.Look<IntVec2>(ref this.centerLocation, "centerLocation", default(IntVec2), false);
            Scribe_Values.Look<IntVec2>(ref this.edgeLocation, "edgeLocation", default(IntVec2), false);
            Scribe_Values.Look<bool>(ref this.disabled, "disabled", false, false);
            Scribe_Values.Look<int>(ref this.ticksBetweenEvents, "ticksBetweenEvents", 4000, false);
            Scribe_Values.Look<int>(ref this.nextEventTick, "nextEventTick", 0, false);
        }

        public override void Init()
        {           
            base.Init();
            this.disabled = false;
            this.FindGoodEdgeLocation();
            this.SpawnDemon();
            this.SetEventParameters();
            InitializeVolcanicWinter();
            InitializeDeathSkies();
        }

        private void InitializeVolcanicWinter()
        {
            GameConditionManager gameConditionManager = this.gameConditionManager;
            int duration = Mathf.RoundToInt(this.Duration);
            GameCondition cond = GameConditionMaker.MakeCondition(GameConditionDefOf.VolcanicWinter, duration);
            gameConditionManager.RegisterCondition(cond);
        }

        private void InitializeDeathSkies()
        {
            GameConditionManager gameConditionManager = this.gameConditionManager;
            int duration = Mathf.RoundToInt(this.Duration);
            GameCondition cond2 = GameConditionMaker.MakeCondition(TorannMagicDefOf.DarkClouds, duration);
            gameConditionManager.RegisterCondition(cond2);
        }

        private void SetEventParameters()
        {
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            float mult = Rand.Range(2f, 4f) + settingsRef.demonAssaultChallenge + Find.Storyteller.difficulty.difficulty;
            this.nextEventTick = Find.TickManager.TicksGame + 200;
            this.ticksBetweenEvents = Mathf.RoundToInt((float)this.Duration / mult);
        }

        public void SpawnDemon()
        {
            AbilityUser.SpawnThings spawnables = new SpawnThings();
            spawnables.def = TorannMagicDefOf.TM_DemonR;
            spawnables.kindDef = PawnKindDef.Named("TM_Demon");
            spawnables.temporary = false;            
            Faction faction = Find.FactionManager.FirstFactionOfDef(TorannMagicDefOf.TM_SkeletalFaction);
            if (faction != null)
            {
                if (!faction.HostileTo(Faction.OfPlayer))
                {
                    faction.TryAffectGoodwillWith(Faction.OfPlayerSilentFail, -200, false, false, null, null);
                }
            }
            else
            {
                faction = Find.FactionManager.RandomEnemyFaction(true, true, true, TechLevel.Undefined);
            }
            this.thing = TM_Action.SingleSpawnLoop(null, spawnables, edgeLocation.ToIntVec3, this.SingleMap, 0, false, false, faction);
        }

        public override void End()
        {
            
            if(!thing.DestroyedOrNull())
            {
                if (thing.Map != null)
                {
                    thing.Map.weatherManager.eventHandler.AddEvent(new TM_WeatherEvent_MeshFlash(thing.Map, thing.Position, TM_MatPool.redLightning));
                }
                thing.Destroy(DestroyMode.Vanish);
            }
            List<GameCondition> gcs = new List<GameCondition>();
            GameCondition gcClouds = null;
            GameCondition gcVW = null;
            gcs = this.SingleMap.GameConditionManager.ActiveConditions;
            for(int i = 0; i < gcs.Count; i++)
            {
                if(gcs[i].def == TorannMagicDefOf.DarkClouds)
                {
                    gcClouds = gcs[i];
                }
                else if(gcs[i].def == GameConditionDefOf.VolcanicWinter)
                {
                    gcVW = gcs[i];
                }
            }
            if(gcClouds != null)
            {
                gcClouds.End();
            }
            if(gcVW != null)
            {
                gcVW.End();
            }
            MagicMapComponent mmc = this.SingleMap.GetComponent<MagicMapComponent>();
            if(mmc != null)
            {
                mmc.allowAllIncidents = false;
            }
            base.End();
        }

        private IntVec3 FindEnemyPawnOrBuilding()
        {
            List<Thing> list = new List<Thing>();
            list.Clear();
            list = (from x in this.SingleMap.listerThings.AllThings
                    where x.Faction != null && x.Faction.HostileTo(thing.Faction)
                    select x).ToList<Thing>();
            return list.RandomElement().Position;
        }

        private void FindGoodEdgeLocation()
        {
            bool centerLocFound = false;
            if (this.SingleMap.Size.x <= 32 || this.SingleMap.Size.z <= 32)
            {
                throw new Exception("Map too small for wandering lich");
            }
            for (int i = 0; i < 20; i++)
            {
                int xVar = 0;
                int zVar = 0;
                if (Rand.Chance(.5f)) 
                {
                    xVar = Rand.Range(8, base.SingleMap.Size.x - 8);
                    zVar = Rand.Chance(.5f) ? Rand.Range(8, 16) : Rand.Range(base.SingleMap.Size.z - 16, base.SingleMap.Size.z - 8);
                }
                else
                {
                    xVar = Rand.Chance(.5f) ? Rand.Range(8, 16) : Rand.Range(base.SingleMap.Size.x - 16, base.SingleMap.Size.x - 8);
                    zVar = Rand.Range(8, base.SingleMap.Size.z - 8);
                }
                this.edgeLocation = new IntVec2(xVar, zVar);
                if (this.IsGoodCenterLocation(this.edgeLocation))
                {
                    this.centerLocation = this.edgeLocation;
                    centerLocFound = true;
                    break;
                }
            }
            if(!centerLocFound)
            {
                FindGoodCenterLocation();
            }
        }

        private void FindGoodCenterLocation()
        {
            if (this.SingleMap.Size.x <= 16 || this.SingleMap.Size.z <= 16)
            {
                throw new Exception("Map too small for wandering lich");
            }
            for (int i = 0; i < 10; i++)
            {
                this.centerLocation = new IntVec2(Rand.Range(8, base.SingleMap.Size.x - 8), Rand.Range(8, base.SingleMap.Size.z - 8));
                if (this.IsGoodCenterLocation(this.centerLocation))
                {
                    break;
                }
            }
        }

        private bool IsGoodLocationForSpawn(IntVec3 loc)
        {
            return loc.InBounds(base.SingleMap) && !loc.Roofed(base.SingleMap) && loc.Standable(base.SingleMap) && loc.IsValid && !loc.Fogged(base.SingleMap) && loc.Walkable(base.SingleMap);
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

        public void CalculateWealthModifier()
        {
            float wealthMultiplier = .7f;
            float wealth = this.SingleMap.PlayerWealthForStoryteller;
            if (wealth > 20000)
            {
                wealthMultiplier = .8f;
            }
            if (wealth > 50000)
            {
                wealthMultiplier = 1f;
            }
            if (wealth > 100000)
            {
                wealthMultiplier = 1.25f;
            }
            if (wealth > 250000)
            {
                wealthMultiplier = 1.5f;
            }
            if (wealth > 500000)
            {
                wealthMultiplier = 2.5f;
            }
        }
    }
}
