using Verse;
using RimWorld;
using System.Linq;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace TorannMagic.Enchantment
{
    public class HediffComp_EnchantedItem : HediffComp
    {
        public bool initialized = false;
        public bool removeNow = false;
        public Apparel enchantedItem;

        public int checkActiveRate = 60;
        public int hediffActionRate = 1;

        public override void CompExposeData()
        {            
            Scribe_Values.Look<bool>(ref this.initialized, "initialized", false, false);
            Scribe_Values.Look<int>(ref this.checkActiveRate, "checkActiveRate", 60, false);
            Scribe_Values.Look<int>(ref this.hediffActionRate, "hediffActionRate", 1, false);
            base.CompExposeData();
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

        public override string CompLabelInBracketsExtra => this.enchantedItem.def.label;

        private void Initialize()
        {
            bool spawned = base.Pawn.Spawned;
            if (spawned)
            {
                PostInitialize();
            }
        }

        public virtual void PostInitialize()
        {
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
            if(Find.TickManager.TicksGame % this.checkActiveRate == 0)
            {
                CheckActiveApparel();
            }
            if(this.hediffActionRate != 0 && Find.TickManager.TicksGame % this.hediffActionRate == 0)
            {
                HediffActionTick();
            }
        }
        
        public void CheckActiveApparel()
        {
            bool remove = true;
            List<Apparel> apparel = this.Pawn.apparel.WornApparel;
            if (apparel != null)
            {
                if(apparel.Contains(this.enchantedItem))
                {
                    remove = false;
                }
            }

            this.removeNow = remove;
        }

        public override bool CompShouldRemove => base.CompShouldRemove || this.removeNow;

        public virtual void HediffActionTick()
        {
        }
    }
}
