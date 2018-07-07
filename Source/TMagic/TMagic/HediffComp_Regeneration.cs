using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    class HediffComp_Regeneration : HediffComp
    {

        private bool initialized = false;
        private int age = 0;
        private int regenRate = 300;
        private int lastRegen = 0;
        private int hediffPwr = 0;

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
                MoteMaker.ThrowLightningGlow(base.Pawn.TrueCenter(), base.Pawn.Map, 1f);
                if (this.Def.defName == "TM_Regeneration_III")
                {
                    hediffPwr = 3;
                }
                else if (this.Def.defName == "TM_Regeneration_II")
                {
                    hediffPwr = 2;
                }
                else if (this.Def.defName == "TM_Regeneration_I")
                {
                    hediffPwr = 1;
                }
                else
                {
                    hediffPwr = 0;
                }
            }
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (base.Pawn != null & base.parent != null)
            {
                if (!initialized)
                {
                    initialized = true;
                    this.Initialize();
                }
            }
            this.age++;
            
            if (age > lastRegen + regenRate)
            {
                HealthUtility.AdjustSeverity(base.Pawn, this.Def, -0.3f);
                this.lastRegen = this.age;
                Pawn pawn = base.Pawn as Pawn;
                
                TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3(), pawn.Map, 1f);
                bool flag = pawn != null;
                if (flag)
                {
                    ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                    int num = 1; // + ver.level;
                    if(settingsRef.AIHardMode && !pawn.IsColonist)
                    {
                        num = 2;
                    }
  
                    using (IEnumerator<BodyPartRecord> enumerator = pawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            BodyPartRecord rec = enumerator.Current;
                            bool flag2 = num > 0;

                            if (flag2)
                            {
                                
                                int num2 = 1; // + ver.level;
                                if(settingsRef.AIHardMode && !pawn.IsColonist)
                                {
                                    num2 = 2;
                                }
                                IEnumerable<Hediff_Injury> arg_BB_0 = pawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                                Func<Hediff_Injury, bool> arg_BB_1;

                                arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                                foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                                {
                                    bool flag4 = num2 > 0;
                                    if (flag4)
                                    {
                                        bool flag5 = current.CanHealNaturally() && !current.IsPermanent();
                                        if (flag5)
                                        {
                                            
                                            if(!pawn.IsColonist)
                                            {
                                                current.Heal(10f + (1.5f * hediffPwr));
                                            }
                                            else
                                            {
                                                current.Heal(2f + (1f * hediffPwr));
                                            }
                                            num--;
                                            num2--;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look<bool>(ref this.initialized, "initialized", false, false);
            Scribe_Values.Look<int>(ref this.age, "age", 0, false);
            Scribe_Values.Look<int>(ref this.regenRate, "regenRate", 300, false);
            Scribe_Values.Look<int>(ref this.lastRegen, "lastRegen", 0, false);
            Scribe_Values.Look<int>(ref this.hediffPwr, "hediffPwr", 0, false);
        }

    }
}
