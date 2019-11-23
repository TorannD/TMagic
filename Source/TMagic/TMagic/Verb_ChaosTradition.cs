using RimWorld;
using System;
using Verse;
using AbilityUser;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Harmony;

namespace TorannMagic
{
    public class Verb_ChaosTradition : Verb_UseAbility  
    {
        private int verVal;
        private int pwrVal;
        private int effVal;

        private int gRegen;
        private int gEff;
        private int gSpirit;

        protected override bool TryCastShot()
        {
            bool result = false;
            Map map = this.CasterPawn.Map;
            CompAbilityUserMagic comp = this.CasterPawn.GetComp<CompAbilityUserMagic>();
            pwrVal = comp.MagicData.MagicPowerSkill_ChaosTradition.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ChaosTradition_pwr").level;
            verVal = comp.MagicData.MagicPowerSkill_ChaosTradition.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ChaosTradition_ver").level;
            effVal = comp.MagicData.MagicPowerSkill_ChaosTradition.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ChaosTradition_eff").level;

            gRegen = comp.MagicData.MagicPowerSkill_global_regen.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_regen_pwr").level;
            gEff = comp.MagicData.MagicPowerSkill_global_eff.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_eff_pwr").level;
            gSpirit = comp.MagicData.MagicPowerSkill_global_spirit.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_spirit_pwr").level;

            if (this.CasterPawn != null && !this.CasterPawn.Downed && comp != null)
            {
                ClearSustainedMagicHediffs(comp);
                TM_Calc.AssignChaosMagicPowers(comp);

                if(effVal >= 3)
                {
                    HealthUtility.AdjustSeverity(this.CasterPawn, TorannMagicDefOf.TM_ChaosTraditionHD, 8f);
                }
                if(effVal >= 2)
                {
                    comp.Mana.CurLevel += .25f * comp.mpRegenRate;
                }
                if(effVal >= 1)
                { 
                    HealthUtility.AdjustSeverity(this.CasterPawn, TorannMagicDefOf.TM_ChaoticMindHD, 24f);
                }

                comp.MagicData.MagicAbilityPoints -= ((2*(pwrVal + verVal + effVal)) + gSpirit + gRegen + gEff);
                comp.MagicData.MagicPowerSkill_ChaosTradition.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ChaosTradition_pwr").level = pwrVal;
                comp.MagicData.MagicPowerSkill_ChaosTradition.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ChaosTradition_ver").level = verVal;
                comp.MagicData.MagicPowerSkill_ChaosTradition.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ChaosTradition_eff").level = effVal;

                comp.MagicData.MagicPowerSkill_global_regen.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_regen_pwr").level = gRegen;
                comp.MagicData.MagicPowerSkill_global_eff.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_eff_pwr").level = gEff;
                comp.MagicData.MagicPowerSkill_global_spirit.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_spirit_pwr").level = gSpirit;

                if(comp.MagicData.MagicAbilityPoints < 0)
                {
                    comp.MagicData.MagicAbilityPoints = 0;
                }

            }
            else
            {
                Log.Warning("failed to TryCastShot");
            }

            this.burstShotsLeft = 0;
            return result;
        }

        public void ClearSustainedMagicHediffs(CompAbilityUserMagic comp)
        {
            if(comp != null)
            {
                Pawn p = comp.Pawn;
                if(p != null && p.health != null && p.health.hediffSet != null)
                {
                    List<Hediff> recList = new List<Hediff>();
                    recList.Clear();
                    List<Hediff> hds = p.health.hediffSet.GetHediffs<Hediff>().ToList();
                    if (hds != null && hds.Count > 0)
                    {
                        for (int i = 0; i < hds.Count; i++)
                        {
                            if (hds[i].def == TorannMagicDefOf.TM_RayOfHope_AuraHD || hds[i].def == TorannMagicDefOf.TM_SoothingBreeze_AuraHD || hds[i].def == TorannMagicDefOf.TM_Shadow_AuraHD ||
                                hds[i].def == TorannMagicDefOf.TM_TechnoBitHD || hds[i].def == TorannMagicDefOf.TM_EnchantedAuraHD || hds[i].def == TorannMagicDefOf.TM_EnchantedBodyHD || 
                                hds[i].def == TorannMagicDefOf.TM_PredictionHD)                                
                            {
                                p.health.RemoveHediff(hds[i]);
                            }
                        }
                    }
                }
            }
        }        
    }
}
