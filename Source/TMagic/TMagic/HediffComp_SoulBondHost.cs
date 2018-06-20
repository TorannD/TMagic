using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    class HediffComp_SoulBondHost : HediffComp
    {
        private bool initialized = false;
        private bool soulPawnRemove = false;

        Pawn bonderPawn;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_References.Look<Pawn>(ref this.bonderPawn, "bonderPawn", false);
        }

        public Pawn BonderPawn
        {
            get
            {
                return this.bonderPawn;
            }
            set
            {
                this.bonderPawn = value;
            }
        }

        public string labelCap
        {
            get
            {
                return base.Def.LabelCap;
            }
        }

        public string label
        {
            get
            {
                return base.Def.label;
            }
        }

        private void Initialize()
        {
            bool spawned = base.Pawn.Spawned;
            if (spawned)
            {
                for (int i = 0; i < 2; i++)
                {
                    TM_MoteMaker.ThrowArcaneMote(this.Pawn.DrawPos, this.Pawn.Map, Rand.Range(.6f, 1f));
                }
            }            
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);            
            bool flag = base.Pawn != null;
            if (flag)
            {
                if (!initialized)
                {
                    initialized = true;
                    this.Initialize();
                }
            }
            bool flag4 = Find.TickManager.TicksGame % 600 == 0;
            if (flag4)
            {
                if(bonderPawn != null && !bonderPawn.Dead && !bonderPawn.Destroyed)
                {
                    //do nothing
                }
                else
                {
                    this.soulPawnRemove = true;
                }
            }
        }

        public override bool CompShouldRemove
        {
            get
            {
                return base.CompShouldRemove || this.soulPawnRemove;
            }
        }
    }
}
