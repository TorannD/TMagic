using AbilityUser;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using UnityEngine;

namespace TorannMagic
{
    public class MagicAbility : PawnAbility
    {
       
        public CompAbilityUserMagic MagicUser
        {
            get
            {
                return MagicUserUtility.GetMagicUser(base.Pawn);
            }
        }

        public TMAbilityDef magicDef
        {
            get
            {
                return base.Def as TMAbilityDef;
            }
        }

        private float ActualBloodCost
        {
            get
            {
                float num = 1;
                if(magicDef != null)
                {
                    num *= 1f - (magicDef.efficiencyReductionPercent * this.MagicUser.MagicData.GetSkill_Efficiency(magicDef).level);
                    num *= (1f - (TorannMagicDefOf.TM_BloodGift.efficiencyReductionPercent /2f) * this.MagicUser.MagicData.MagicPowerSkill_BloodGift.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodGift_eff").level);
                }
                return magicDef.bloodCost * num;
            }
        }

        private float ActualManaCost
        {
            get
            {
                if (magicDef != null)
                {
                    return this.MagicUser.ActualManaCost(magicDef);
                }
                return magicDef.manaCost;         
            }
        }

        public MagicAbility()
        {
        }

        public MagicAbility(CompAbilityUser abilityUser) : base(abilityUser)
		{
            this.abilityUser = (abilityUser as CompAbilityUserMagic);
        }

        public MagicAbility(Pawn user, AbilityUser.AbilityDef pdef) : base(user, pdef)
		{

        }

        public override void PostAbilityAttempt()  //commented out in CompAbilityUserMagic
        {
            //base.PostAbilityAttempt();
            
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (!this.Pawn.IsColonist && settingsRef.AIAggressiveCasting)// for AI
            {
                this.CooldownTicksLeft = Mathf.RoundToInt(this.MaxCastingTicks/2f);
            }
            else
            {
                this.CooldownTicksLeft = Mathf.RoundToInt(this.MaxCastingTicks * this.MagicUser.coolDown);
            }
            if(Rand.Chance(MagicUser.arcalleumCooldown))
            {
                this.CooldownTicksLeft = 4;
            }
            bool flag = this.magicDef != null;
            if (flag)
            {
                bool flag3 = this.MagicUser.Mana != null;
                if (flag3)
                {
                    if(!this.Pawn.IsColonist && settingsRef.AIAggressiveCasting)// for AI
                    {
                        this.MagicUser.Mana.UseMagicPower(this.MagicUser.ActualManaCost(magicDef)/2f);
                    }
                    else
                    {                       
                        this.MagicUser.Mana.UseMagicPower(this.MagicUser.ActualManaCost(magicDef));
                    }
                                       
                    if(this.magicDef != TorannMagicDefOf.TM_TransferMana && magicDef.abilityHediff == null)
                    {                        
                        this.MagicUser.MagicUserXP += (int)((magicDef.manaCost * 300) * this.MagicUser.xpGain * settingsRef.xpMultiplier);
                    }                    
                }
                else if (this.MagicUser.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                {
                    CompAbilityUserMight mightComp = this.MagicUser.Pawn.GetComp<CompAbilityUserMight>();
                    mightComp.Stamina.UseMightPower(magicDef.manaCost);
                    mightComp.MightUserXP += (int)((magicDef.manaCost * 180) * mightComp.xpGain * settingsRef.xpMultiplier);
                }

                if (this.magicDef.bloodCost != 0)
                {
                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_BloodHD"), -100 * this.ActualBloodCost);
                }
            }                       
        }

        public override string PostAbilityVerbCompDesc(VerbProperties_Ability verbDef)
        {
            string result = "";
            StringBuilder stringBuilder = new StringBuilder();
            TMAbilityDef magicAbilityDef = (TMAbilityDef)verbDef.abilityDef;
            bool flag = magicAbilityDef != null;
            if (flag)
            {
                string text = "";
                string text2 = "";
                string text3 = "";
                float num = 0;
                float num2 = 0;
                
               
                if (magicAbilityDef == TorannMagicDefOf.TM_Teleport)
                {
                    num = this.MagicUser.ActualManaCost(magicDef)*100;
                    MagicPowerSkill mps2 = this.MagicUser.MagicData.MagicPowerSkill_Teleport.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Teleport_ver");
                    MagicPowerSkill mps1 = this.MagicUser.MagicData.MagicPowerSkill_Teleport.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Teleport_pwr");
                    num2 = 80 + (mps1.level * 20) + (mps2.level * 20);
                    text2 = "TM_AbilityDescPortalTime".Translate(
                        num2.ToString()
                    );
                }
                else if (magicAbilityDef == TorannMagicDefOf.TM_SummonMinion)
                {
                    num = this.MagicUser.ActualManaCost(magicDef)*100;
                    MagicPowerSkill mps1 = this.MagicUser.MagicData.MagicPowerSkill_SummonMinion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonMinion_ver");
                    num2 = 1200 + (600 * mps1.level);
                    text2 = "TM_AbilityDescSummonDuration".Translate(
                        num2.ToString()
                    );
                }
                else if (magicAbilityDef == TorannMagicDefOf.TM_SummonPylon)
                {
                    num = this.MagicUser.ActualManaCost(magicDef)*100;
                    MagicPowerSkill mps1 = this.MagicUser.MagicData.MagicPowerSkill_SummonPylon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonPylon_ver");
                    num2 = 240 + (120 * mps1.level);
                    text2 = "TM_AbilityDescSummonDuration".Translate(
                        num2.ToString()
                    );
                }
                else if (magicAbilityDef == TorannMagicDefOf.TM_SummonExplosive)
                {
                    num = this.MagicUser.ActualManaCost(magicDef) * 100;
                    MagicPowerSkill mps1 = this.MagicUser.MagicData.MagicPowerSkill_SummonExplosive.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonExplosive_ver");
                    num2 = 240 + (120 * mps1.level);
                    text2 = "TM_AbilityDescSummonDuration".Translate(
                        num2.ToString()
                    );
                }
                else if (magicAbilityDef == TorannMagicDefOf.TM_SummonElemental)
                {
                    num = this.MagicUser.ActualManaCost(magicDef) * 100;
                    MagicPowerSkill mps1 = this.MagicUser.MagicData.MagicPowerSkill_SummonElemental.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonElemental_ver");
                    num2 = 30 + (15 * mps1.level);
                    text2 = "TM_AbilityDescSummonDuration".Translate(
                        num2.ToString()
                    );
                }
                else if (magicAbilityDef == TorannMagicDefOf.TM_PsychicShock)
                {
                    num = this.MagicUser.ActualManaCost(magicDef) * 100;
                    num2 = this.MagicUser.Pawn.GetStatValue(StatDefOf.PsychicSensitivity, false);
                    text3 = "TM_PsychicSensitivity".Translate(
                        num2.ToString()
                    );
                }
                else
                {
                    num = this.MagicUser.ActualManaCost(magicDef) * 100;
                }

                if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                {                    
                    text = "TM_AbilityDescBaseStaminaCost".Translate(
                        (magicAbilityDef.manaCost * 100).ToString("n1")
                    ) + "\n" + "TM_AbilityDescAdjustedStaminaCost".Translate(
                        (magicDef.manaCost * 100).ToString("n1")
                    );
                }
                else
                {
                    text = "TM_AbilityDescBaseManaCost".Translate(
                        (magicAbilityDef.manaCost * 100).ToString("n1")
                    ) + "\n" + "TM_AbilityDescAdjustedManaCost".Translate(
                        num.ToString("n1")
                    );
                }

                if(magicAbilityDef == TorannMagicDefOf.TM_IgniteBlood || magicAbilityDef == TorannMagicDefOf.TM_BloodShield || magicAbilityDef == TorannMagicDefOf.TM_BloodForBlood || 
                    magicAbilityDef == TorannMagicDefOf.TM_Rend || magicAbilityDef == TorannMagicDefOf.TM_Rend_I || magicAbilityDef == TorannMagicDefOf.TM_Rend_II || magicAbilityDef == TorannMagicDefOf.TM_Rend_III ||
                    magicAbilityDef == TorannMagicDefOf.TM_BloodMoon || magicAbilityDef == TorannMagicDefOf.TM_BloodMoon_I || magicAbilityDef == TorannMagicDefOf.TM_BloodMoon_II || magicAbilityDef == TorannMagicDefOf.TM_BloodMoon_III)
                {
                    num = this.ActualBloodCost * 100;
                    text = "TM_AbilityDescBaseBloodCost".Translate(
                    (magicAbilityDef.bloodCost * 100).ToString("n1")
                    ) + "\n" + "TM_AbilityDescAdjustedBloodCost".Translate(
                        num.ToString("n1")
                    );
                }

                if(this.MagicUser.coolDown != 1f)
                {
                    text3 = "TM_AdjustedCooldown".Translate(
                        ((this.MaxCastingTicks * this.MagicUser.coolDown)/60).ToString("0.00")
                    );
                }

                bool flag2 = text != "";
                if (flag2)
                {
                    stringBuilder.AppendLine(text);
                }
                bool flag3 = text2 != "";
                if (flag3)
                {
                    stringBuilder.AppendLine(text2);
                }
                bool flag4 = text3 != "";
                if(flag4)
                {
                    stringBuilder.AppendLine(text3);
                }
                result = stringBuilder.ToString();
            }
            return result;
        }

        public override bool CanCastPowerCheck(AbilityContext context, out string reason)
        {
            bool flag = base.CanCastPowerCheck(context, out reason);
            bool result;
            if (flag)
            {
                reason = "";
                TMAbilityDef tmAbilityDef;
                bool flag1 = base.Def != null && (tmAbilityDef = (base.Def as TMAbilityDef)) != null;
                if (flag1)
                {
                    bool flag4 = this.MagicUser.Mana != null;
                    if (flag4)
                    {
                        bool flag5 = magicDef.manaCost > 0f && this.ActualManaCost > this.MagicUser.Mana.CurLevel;
                        if (flag5)
                        {
                            reason = "TM_NotEnoughMana".Translate(
                                base.Pawn.LabelShort
                            );
                            result = false;
                            return result;
                        }
                        bool flag6 = magicDef.bloodCost > 0f && this.MagicUser.Pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_BloodHD"),false) && (this.ActualBloodCost * 100) > this.MagicUser.Pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_BloodHD"), false).Severity;
                        if (flag6)
                        {
                            reason = "TM_NotEnoughBlood".Translate(
                                base.Pawn.LabelShort
                            );
                            result = false;
                            return result;
                        }
                        bool flagMute = this.MagicUser.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_MuteHD);
                        if(flagMute)
                        {
                            reason = "TM_CasterMute".Translate(
                                base.Pawn.LabelShort
                            );
                            result = false;
                            return result;
                        }
                    }
                    else if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                    {
                        CompAbilityUserMight mightComp = this.Pawn.GetComp<CompAbilityUserMight>();
                        bool flag7 = mightComp != null && mightComp.Stamina != null && magicDef.manaCost > 0f && this.magicDef.manaCost > mightComp.Stamina.CurLevel;
                        if (flag7)
                        {
                            reason = "TM_NotEnoughStamina".Translate(
                            base.Pawn.LabelShort
                            );
                            result = false;
                            return result;
                        }
                    }
                }
                List<Apparel> wornApparel = base.Pawn.apparel.WornApparel;
                for (int i = 0; i < wornApparel.Count; i++)
                {
                    if (!wornApparel[i].AllowVerbCast(base.Pawn.Position, base.Pawn.Map, base.abilityUser.Pawn.TargetCurrentlyAimingAt, this.Verb) &&
                        (this.magicDef.defName == "TM_LightningCloud" || this.magicDef.defName == "Laser_LightningBolt" || this.magicDef.defName == "TM_LightningStorm" || this.magicDef.defName == "TM_EyeOfTheStorm" ||
                        this.magicDef.defName.Contains("Laser_FrostRay") || this.magicDef.defName == "TM_Blizzard" || this.magicDef.defName == "TM_Snowball" || this.magicDef.defName == "TM_Icebolt" ||
                        this.magicDef.defName == "TM_Firestorm" || this.magicDef.defName == "TM_Fireball" || this.magicDef.defName == "TM_Fireclaw" || this.magicDef.defName == "TM_Firebolt" ||
                        this.magicDef.defName.Contains("TM_MagicMissile") ||
                        this.magicDef.defName.Contains("TM_DeathBolt") ||
                        this.magicDef.defName.Contains("TM_ShadowBolt") ||
                        this.magicDef.defName == "TM_BloodForBlood" || this.magicDef.defName == "TM_IgniteBlood" ||
                        this.magicDef.defName == "TM_Poison" ||
                        this.magicDef == TorannMagicDefOf.TM_ArcaneBolt) )
                    {
                        reason = "TM_ShieldBlockingPowers".Translate(
                            base.Pawn.Label,
                            wornApparel[i].Label
                        );
                        return false;
                    }
                }
                result = true;
                
            }
            else
            {
                result = false;
            }
            return result;

        }
    }
}
