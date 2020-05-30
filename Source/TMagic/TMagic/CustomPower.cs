using AbilityUser;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace TorannMagic
{
    public class CustomPower : BasePower
    {
        public PowerDef localDef;
        public CustomPower()
        {
        }
        public CustomPower(PowerDef def) : base()
        {
            this.localDef = def;
            updateBasePower();
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look<PowerDef>(ref localDef, "powerDef");
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                updateBasePower();
            }
        }
        private void updateBasePower()
        {
            this.Abilities = localDef.abilityLine.ConvertAll((TMAbilityDef a) => a as AbilityDef);
            this.Upgrades = localDef.upgrades.ConvertAll((UpgradeDef u) => u.UpgradeId);
            this.upgradeCost = localDef.upgradeCost;
            this.learnCost = localDef.learnCost;
            this.maxLevel = localDef.abilityLine.Count - 1;
        }
    }

    public class PowerDef : Def
    {
        public int upgradeCost = 1;
        public int learnCost = 1;
        public List<TMAbilityDef> abilityLine;
        public List<UpgradeDef> upgrades = new List<UpgradeDef>();
    }

    public class UpgradeDef : Def
    {
        public string title = null;
        public string upgradeId = null;
        public int upgradeCost = 1;
        public string upgradeDescription = null;
        public int levelMax = 3;

        public string Title
        {
            get => title ?? this.label ?? upgradeId?? this.defName;
        }

        public string UpgradeId
        {
            get => upgradeId ?? this.defName;
        }

        public string Description
        {
            get => upgradeDescription ?? this.description ?? title ?? this.label ?? this.defName;
        }

        public MagicPowerSkill asMagicPowerSkill()
        {
            MagicPowerSkill mps = new MagicPowerSkill(UpgradeId, Description);
            mps.costToLevel = upgradeCost;
            mps.levelMax = levelMax;
            return mps;
        }
    }
}
