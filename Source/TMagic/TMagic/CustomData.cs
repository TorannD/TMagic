using AbilityUser;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TorannMagic
{
    public class CustomData : IExposable
    {
        private int userLevel = 0;
        private int abilityPoints = 0;
        private int userXP = 1;
        private int ticksToLearnXP = -1;
        private int ticksAffiliation = 0;

        private List<CustomPower> powers;
        private Dictionary<string, MagicPowerSkill> upgrades;

        public List<BasePower> Powers
        {
            get => this.powers.ConvertAll((CustomPower p) => p as BasePower);
        }

        public Dictionary<string, MagicPowerSkill> Upgrades
        {
            get => upgrades;
        }

        public int UserLevel
        {
            get => this.userLevel;
            set
            {
                if (value > this.userLevel)
                {
                    this.AbilityPoints += value - this.userLevel;
                }
                int minXp = XPForLevel(value);
                if (this.userXP < minXp)
                {
                    this.userXP = XPForLevel(value);
                }
                this.userLevel = value;
            }
        }

        public int UserXP
        {
            get => this.userXP;
            set => this.userXP = value;
        }

        public virtual int XPForLevel(int level)
        {
            return level * 500;
        }

        public int AbilityPoints
        {
            get => this.abilityPoints;
            set => this.abilityPoints = value;
        }

        public int TicksToLearnXP
        {
            get => this.ticksToLearnXP;
            set => this.ticksToLearnXP = value;
        }

        public int TicksAffiliation
        {
            get => this.ticksAffiliation;
            set => this.ticksAffiliation = value;
        }

        public int GetUniquePowersWithSkillsCount()
        {
            return this.powers.Count;
        }

        public int GetUpgradeLevel(string upgradeLabel)
        {
            return Upgrades.TryGetValue(upgradeLabel)?.level ?? 0; //FIXME: when not found should we return -1 ?
        }

        public BasePower ReturnMatchingPower(AbilityUser.AbilityDef def, bool isCurrentAbility = false)
        {
            if (def == TorannMagicDefOf.TM_SoulBond || def == TorannMagicDefOf.TM_ShadowBolt || def == TorannMagicDefOf.TM_Dominate)
            {
                return null;
            }
            return powers.Find((CustomPower p) => p.Abilities.Contains(def) && (!isCurrentAbility || p.AbilityDef == def));
        }

        public List<BasePower> AllPowersForChaosMage
        {
            get
            {
                List<AbilityUser.AbilityDef> tmpList = new List<AbilityUser.AbilityDef>();
                List<CustomPower> list = new List<CustomPower>();
                list.Clear();
                list.AddRange(this.powers.FindAll((CustomPower p) =>
                {
                    TMAbilityDef def = p.AbilityDef as TMAbilityDef;
                    return def != null && def.canCopy
                        && def.staminaCost < float.Epsilon && def.chiCost < float.Epsilon && def.bloodCost < float.Epsilon;
                }));
                return list.ConvertAll((CustomPower p) => p as BasePower); ;
            }
        }

        public void ResetAllSkills()
        {
            this.Upgrades.Values.ToList().ForEach((MagicPowerSkill m) => m.level = 0);
        }

        public CustomData()
        {
        }

        public CustomData(List<CustomPower> powers)
        {
            this.userLevel = 0;
            this.userXP = 0;
            this.abilityPoints = 0;
            this.powers = powers ?? new List<CustomPower>();
            this.upgrades = this.powers.SelectMany((CustomPower p) =>
                    p.localDef.upgrades.ConvertAll((UpgradeDef u) => u.asMagicPowerSkill())
                ).ToDictionary((MagicPowerSkill s) => s.label);
            //this.upgrades_labels = upgrades.Keys.ToList();
            //this.upgrades_values = upgrades.Values.ToList();
            Log.Message("Creating new Data with " + this.powers.Count + " powers and " + this.upgrades.Count + " upgrades");
        }

        public void ExposeData()
        {
            //Scribe_References.Look<Pawn>(ref this.pawn, "magicPawn", false);
            Scribe_Values.Look<int>(ref this.userLevel, "c_userLevel", 0, false);
            Scribe_Values.Look<int>(ref this.userXP, "c_userXP", 0, false);
            Scribe_Values.Look<int>(ref this.abilityPoints, "c_magicAbilityPoints", 0, false);
            Scribe_Values.Look<int>(ref this.ticksToLearnXP, "c_ticksToLearnMagicXP", -1, false);
            Scribe_Values.Look<int>(ref this.ticksAffiliation, "c_ticksAffiliation", -1, false);
            Scribe_Collections.Look<CustomPower>(ref this.powers, "c_powers", LookMode.Deep, new object[0]);

            //this.upgrades_labels = upgrades.Keys.ToList();
            //this.upgrades_values = upgrades.Values.ToList();
            Scribe_Collections.Look<string, MagicPowerSkill>(ref this.upgrades, "c_upgrades", LookMode.Value, LookMode.Deep);
            //LookMode.Value, LookMode.Deep, ref this.upgrades_labels, ref this.upgrades_values);
        }
    }
}
