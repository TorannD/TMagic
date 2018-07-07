using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace TorannMagic
{
    public class TMPawnSummoned : Pawn
    {
        private Effecter effecter;

        private bool initialized;

        private bool temporary;

        private int ticksLeft;

        private int ticksToDestroy = 1800;

        CompAbilityUserMagic compSummoner;
        Pawn spawner;

        public Pawn Spawner
        {
            get => this.spawner;
            set => spawner = value;
        }

        public CompAbilityUserMagic CompSummoner
        {
            get
            {
                return spawner.GetComp<CompAbilityUserMagic>();
            }
        }

        public bool Temporary
        {
            get
            {
                return this.temporary;
            }
            set
            {
                this.temporary = value;
            }
        }

        public int TicksToDestroy
        {
            get
            {
                return this.ticksToDestroy;
            }
            set
            {
                ticksToDestroy = value;
            }
        }

        public int TicksLeft
        {
            get
            {
                return this.ticksLeft;
            }
            set
            {
                this.ticksLeft = value;
            }
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            this.ticksLeft = this.ticksToDestroy;
            base.SpawnSetup(map, respawningAfterLoad);
        }

        public virtual void PostSummonSetup()
        {

        }

        public void CheckPawnState()
        {
            if (this.def.race.body.ToString() == "Minion")
            {
                try
                {
                    if (this.Downed && !this.Destroyed && this != null)
                    {
                        Messages.Message("MinionFled".Translate(), MessageTypeDefOf.NeutralEvent);
                        MoteMaker.ThrowSmoke(this.Position.ToVector3(), base.Map, 1);
                        MoteMaker.ThrowHeatGlow(this.Position, base.Map, 1);
                        if (CompSummoner != null)
                        {
                            CompSummoner.summonedMinions.Remove(this);
                        }
                        this.Destroy(DestroyMode.Vanish);
                    }
                }
                catch
                {
                    Log.Message("TM_ExceptionTick".Translate(new object[]
                    {
                        this.def.defName
                    }));
                    this.Destroy(DestroyMode.Vanish);
                }
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (Find.TickManager.TicksGame % 10 == 0)
            {
                if (!this.initialized)
                {
                    this.initialized = true;
                    this.PostSummonSetup();
                }
                bool flag2 = this.temporary;
                if (flag2)
                {
                    this.ticksLeft -= 10;
                    bool flag3 = this.ticksLeft <= 0;
                    if (flag3)
                    {
                        this.PreDestroy();
                        this.Destroy(DestroyMode.Vanish);
                    }
                    CheckPawnState();
                    bool spawned = base.Spawned;
                    if (spawned)
                    {
                        bool flag4 = this.effecter == null;
                        if (flag4)
                        {
                            EffecterDef progressBar = EffecterDefOf.ProgressBar;
                            this.effecter = progressBar.Spawn();
                        }
                        else
                        {
                            LocalTargetInfo localTargetInfo = this;
                            bool spawned2 = base.Spawned;
                            if (spawned2)
                            {
                                this.effecter.EffectTick(this, TargetInfo.Invalid);
                            }
                            MoteProgressBar mote = ((SubEffecter_ProgressBar)this.effecter.children[0]).mote;
                            bool flag5 = mote != null;
                            if (flag5)
                            {
                                float value = 1f - (float)(this.TicksToDestroy - this.ticksLeft) / (float)this.TicksToDestroy;
                                mote.progress = Mathf.Clamp01(value);
                                mote.offsetZ = -0.5f;
                            }
                        }
                    }
                }
            }
        }

        public void PreDestroy()
        {
            if (this.def.defName == "TM_MinionR" || this.def.defName == "TM_GreaterMinionR")
            {
                try
                {
                    MoteMaker.ThrowSmoke(this.Position.ToVector3(), base.Map, 3);
                    if (CompSummoner != null)
                    {
                        CompSummoner.summonedMinions.Remove(this);
                    }
                }
                catch
                {
                    Log.Message("TM_ExceptionClose".Translate(new object[]
                    {
                            this.def.defName
                    }));
                }
            }
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            bool flag = this.effecter != null;
            if (flag)
            {
                this.effecter.Cleanup();
            }
            base.DeSpawn(mode);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.temporary, "temporary", false, false);
            Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
            Scribe_Values.Look<int>(ref this.ticksToDestroy, "ticksToDestroy", 1800, false);
            Scribe_Values.Look<CompAbilityUserMagic>(ref this.compSummoner, "compSummoner", null, false);
            Scribe_References.Look<Pawn>(ref this.spawner, "spawner", false);
        }

        public TMPawnSummoned()
        {

        }
    }
}
