using AbilityUser;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace TorannMagic 
{
    public class MightPower : IExposable
    {
        public List<AbilityDef> TMabilityDefs;

        public int ticksUntilNextCast = -1;

        public int level;

        public bool learned = true;

        public int learnCost = 2;

        public AbilityDef abilityDescDef
        {
            get
            {
                AbilityDef result = null;
                bool flag = this.TMabilityDefs != null && this.TMabilityDefs.Count > 0;
                if (flag)
                {
                    result = this.TMabilityDefs[0];
                    int num = this.level - 0; 
                    bool flag2 = num > -1 && num < this.TMabilityDefs.Count;
                    if (flag2)
                    {
                        result = this.TMabilityDefs[num];
                    }
                    else
                    {
                        bool flag3 = num >= this.TMabilityDefs.Count;
                        if (flag3)
                        {
                            result = this.TMabilityDefs[this.TMabilityDefs.Count - 1];
                        }
                    }
                }
                return result;
            }
        }

        public AbilityDef nextLevelAbilityDescDef
        {
            get
            {
                AbilityDef result = null;
                bool flag = this.abilityDef != null && this.TMabilityDefs.Count > 0;
                if (flag)
                {
                    result = this.TMabilityDefs[0];
                    int num = this.level + 1;
                    bool flag2 = num > -1 && num <= this.TMabilityDefs.Count;
                    if (flag2)
                    {
                        result = this.TMabilityDefs[num];
                    }
                    else
                    {
                        bool flag3 = num >= this.TMabilityDefs.Count;
                        if (flag3)
                        {
                            result = this.TMabilityDefs[this.TMabilityDefs.Count - 1];
                        }
                    }
                }
                return result;
            }
        }

        public AbilityDef abilityDef
        {
            get
            {
                AbilityDef result = null;
                bool flag = this.TMabilityDefs != null && this.TMabilityDefs.Count > 0;
                if (flag)
                {
                    result = this.TMabilityDefs[0];
                    int num = this.level - 1; 
                    bool flag2 = num > -1 && num < this.TMabilityDefs.Count;
                    if (flag2)
                    {
                        result = this.TMabilityDefs[num];
                    }
                    else
                    {
                        bool flag3 = num >= this.TMabilityDefs.Count;
                        if (flag3)
                        {
                            result = this.TMabilityDefs[this.TMabilityDefs.Count - 1];
                        }
                    }
                }
                return result;
            }
        }

        public AbilityDef nextLevelAbilityDef
        {
            get
            {
                AbilityDef result = null;
                bool flag = this.abilityDef != null && this.TMabilityDefs.Count > 0;
                if (flag)
                {
                    result = this.TMabilityDefs[0];
                    int num = this.level; 
                    bool flag2 = num > -1 && num <= this.TMabilityDefs.Count;
                    if (flag2)
                    {
                        result = this.TMabilityDefs[num];
                    }
                    else
                    {
                        bool flag3 = num >= this.TMabilityDefs.Count;
                        if (flag3)
                        {
                            result = this.TMabilityDefs[this.TMabilityDefs.Count - 1];
                        }
                    }
                }
                return result;
            }
        }

        public Texture2D Icon
        {
            get
            {
                return this.abilityDef.uiIcon;
            }
        }

        public AbilityDef GetAbilityDef(int index)
        {
            AbilityDef result = null;
            bool flag = this.TMabilityDefs != null && this.TMabilityDefs.Count > 0;
            if (flag)
            {
                result = this.TMabilityDefs[0];
                bool flag2 = index > -1 && index < this.TMabilityDefs.Count;
                if (flag2)
                {
                    result = this.TMabilityDefs[index];
                }
                else
                {
                    bool flag3 = index >= this.TMabilityDefs.Count;
                    if (flag3)
                    {
                        result = this.TMabilityDefs[this.TMabilityDefs.Count - 1];
                    }
                }
            }
            return result;
        }

        public AbilityDef HasAbilityDef(AbilityDef defToFind)
        {
            return this.TMabilityDefs.FirstOrDefault((AbilityDef x) => x == defToFind);
        }

        public MightPower()
        {
        }

        public MightPower(List<AbilityDef> newAbilityDefs)
        {
            this.level = 0;
            this.TMabilityDefs = newAbilityDefs;
        }

        public void ExposeData()
        {
            Scribe_Values.Look<bool>(ref this.learned, "learned", true, false);
            Scribe_Values.Look<int>(ref this.learnCost, "learnCost", 2, false);
            Scribe_Values.Look<int>(ref this.level, "level", 0, false);
            Scribe_Values.Look<int>(ref this.ticksUntilNextCast, "ticksUntilNextCast", -1, false);
            Scribe_Collections.Look<AbilityDef>(ref this.TMabilityDefs, "TMabilityDefs", (LookMode)4, null);
        }
    }
}
