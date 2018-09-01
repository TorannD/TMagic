using System;
using Verse;

namespace TorannMagic
{
    public class MagicPowerSkill : IExposable
    {
        public string label;

        public string desc;

        public int level;

        public int levelMax;

        public MagicPowerSkill()
        {
        }

        public MagicPowerSkill(string newLabel, string newDesc)
        {
            this.label = newLabel;
            this.desc = newDesc;
            this.level = 0;

            if (newLabel == "TM_Firebolt_pwr")
            {
                this.levelMax = 6;
            }
            else if (newLabel == "TM_global_regen_pwr" || newLabel == "TM_global_eff_pwr" || newLabel == "TM_EarthSprites_pwr")
            {
                this.levelMax = 5;
            }
            else if (newLabel == "TM_Blink_eff" || newLabel == "TM_Summon_eff" || newLabel == "TM_AdvancedHeal_pwr" || newLabel == "TM_AdvancedHeal_ver" || newLabel == "TM_HealingCircle_pwr")
            {
                this.levelMax = 4;
            }
            else if (newLabel == "TM_global_spirit_pwr")
            {
                this.levelMax = 50;
            }
            else if (newLabel == "TM_Sentinel_pwr")
            {
                this.levelMax = 2;
            }
            else
            {
                this.levelMax = 3;
            }
        }

        public void ExposeData()
        {
            Scribe_Values.Look<string>(ref this.label, "label", "default", false);
            Scribe_Values.Look<string>(ref this.desc, "desc", "default", false);
            Scribe_Values.Look<int>(ref this.level, "level", 0, false);
            Scribe_Values.Look<int>(ref this.levelMax, "levelMax", 0, false);
        }

    }
}
