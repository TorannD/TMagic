using Verse;

namespace TorannMagic
{
    public class MightPowerSkill : IExposable
    {
        public string label;

        public string desc;

        public int level;

        public int levelMax;

        public MightPowerSkill()
        {
        }

        public MightPowerSkill(string newLabel, string newDesc)
        {
            this.label = newLabel;
            this.desc = newDesc;
            this.level = 0;

            if (newLabel == "TM_global_endurance_pwr")
            {
                this.levelMax = 50;
            }
            else if (newLabel == "TM_FieldTraining_pwr" || newLabel == "TM_FieldTraining_eff" || newLabel == "TM_FieldTraining_ver")
            {
                this.levelMax = 15;
            }
            else if (newLabel == "TM_WayfarerCraft_pwr" || newLabel == "TM_WayfarerCraft_eff" || newLabel == "TM_WayfarerCraft_ver")
            {
                this.levelMax = 30;
            }
            else if (newLabel == "TM_global_refresh_pwr" || newLabel == "TM_global_seff_pwr" || newLabel == "TM_global_strength_pwr" || newLabel == "TM_Shroud_pwr" || newLabel == "TM_Shroud_ver" || newLabel == "TM_Shroud_eff")
            {
                this.levelMax = 5;
            }
            else if (false)
            {
                this.levelMax = 4;
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
