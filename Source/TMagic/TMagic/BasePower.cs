using AbilityUser;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace TorannMagic
{
    public abstract class BasePower : IExposable
    {
        private List<AbilityDef> abilityDefs;
        private List<string> upgrades;

        public int ticksUntilNextCast = -1;

        public int level;

        public int learnCost = 2;
        public int upgradeCost = 1;
        public int maxLevel = 3;

        public bool learned = true;
        public bool autocast = false;
        protected int interactionTick = 0;

        public bool AutoCast
        {
            get => autocast;
            set
            {
                if (interactionTick < Find.TickManager.TicksGame)
                {
                    autocast = value;
                    interactionTick = Find.TickManager.TicksGame + 30;
                }
            }
        }

        public AbilityDef NextLevelAbilityDef
        {
            get => level + 1 < this.Abilities.Count ? Abilities[level + 1] : Abilities.Last();
        }

        public AbilityDef AbilityDef
        {
            get => level < this.Abilities.Count ? Abilities[level] : Abilities.Last();
        }

        public List<AbilityDef> Abilities
        {
            get => abilityDefs;
            protected set => abilityDefs = value;
        }

        public List<string> Upgrades
        {
            get => upgrades;
            protected set => upgrades = value;
        }

        public Texture2D Icon
        {
            get => this.AbilityDef.uiIcon;
        }

        public BasePower()
        {
            abilityDefs = new List<AbilityDef>();
            upgrades = new List<string>();
        }

        public virtual void ExposeData()
        {
            Scribe_Values.Look<bool>(ref this.learned, "learned", true, false);
            Scribe_Values.Look<bool>(ref this.autocast, "autocast", false, false);
            Scribe_Values.Look<int>(ref this.learnCost, "learnCost", 2, false);
            Scribe_Values.Look<int>(ref this.level, "level", 0, false);
            Scribe_Values.Look<int>(ref this.maxLevel, "maxLevel", 3, false);
            Scribe_Values.Look<int>(ref this.ticksUntilNextCast, "ticksUntilNextCast", -1, false);
            Scribe_Collections.Look<AbilityDef>(ref this.abilityDefs, "abilityDefs", LookMode.Def, null);
            Scribe_Collections.Look<string>(ref this.upgrades, "upgrades", LookMode.Value, null);
        }
    }
}
