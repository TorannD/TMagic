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

        public MagicAbility(Pawn user, AbilityDef pdef) : base(user, pdef)
		{

        }

        public override void PostAbilityAttempt()  //commented out in CompAbilityUserMagic
        {
            base.PostAbilityAttempt();
            bool flag = this.magicDef != null;
            if (flag)
            {
                bool flag3 = this.MagicUser.Mana != null;
                if (flag3)
                {
                    this.MagicUser.Mana.UseMagicPower(this.MagicUser.ActualManaCost(magicDef));                    
                    if(this.magicDef != TorannMagicDefOf.TM_TransferMana)
                    {
                        ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                        this.MagicUser.MagicUserXP += (int)((magicDef.manaCost * 300) * this.MagicUser.xpGain * settingsRef.xpMultiplier);
                    }                    
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
                    num = this.MagicUser.ActualManaCost(magicDef);
                    MagicPowerSkill mps2 = this.MagicUser.MagicData.MagicPowerSkill_Teleport.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Teleport_ver");
                    MagicPowerSkill mps1 = this.MagicUser.MagicData.MagicPowerSkill_Teleport.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Teleport_pwr");
                    num2 = 60 + (mps1.level * 15) + (mps2.level * 15);
                    text2 = "TM_AbilityDescPortalTime".Translate(new object[]
                    {
                        num2.ToString()
                    });
                }
                else if (magicAbilityDef == TorannMagicDefOf.TM_SummonMinion)
                {
                    num = this.MagicUser.ActualManaCost(magicDef);
                    MagicPowerSkill mps1 = this.MagicUser.MagicData.MagicPowerSkill_SummonMinion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonMinion_ver");
                    num2 = 600 + (300 * mps1.level);
                    text2 = "TM_AbilityDescSummonDuration".Translate(new object[]
                    {
                        num2.ToString()
                    });
                }
                else if (magicAbilityDef == TorannMagicDefOf.TM_SummonPylon)
                {
                    num = this.MagicUser.ActualManaCost(magicDef);
                    MagicPowerSkill mps1 = this.MagicUser.MagicData.MagicPowerSkill_SummonPylon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonPylon_ver");
                    num2 = 120 + (60 * mps1.level);
                    text2 = "TM_AbilityDescSummonDuration".Translate(new object[]
                    {
                        num2.ToString()
                    });
                }
                else if (magicAbilityDef == TorannMagicDefOf.TM_SummonExplosive)
                {
                    num = this.MagicUser.ActualManaCost(magicDef);
                    MagicPowerSkill mps1 = this.MagicUser.MagicData.MagicPowerSkill_SummonExplosive.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonExplosive_ver");
                    num2 = 120 + (60 * mps1.level);
                    text2 = "TM_AbilityDescSummonDuration".Translate(new object[]
                    {
                        num2.ToString()
                    });
                }
                else if (magicAbilityDef == TorannMagicDefOf.TM_SummonElemental)
                {
                    num = this.MagicUser.ActualManaCost(magicDef);
                    MagicPowerSkill mps1 = this.MagicUser.MagicData.MagicPowerSkill_SummonElemental.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonElemental_ver");
                    num2 = 30 + (15 * mps1.level);
                    text2 = "TM_AbilityDescSummonDuration".Translate(new object[]
                    {
                        num2.ToString()
                    });
                }
                else if (magicAbilityDef == TorannMagicDefOf.TM_PsychicShock)
                {
                    num = this.MagicUser.ActualManaCost(magicDef);
                    num2 = this.MagicUser.Pawn.GetStatValue(StatDefOf.PsychicSensitivity, false);
                    text3 = "TM_PsychicSensitivity".Translate(new object[]
                    {
                        num2.ToString()
                    });
                }
                else
                {
                    num = this.MagicUser.ActualManaCost(magicDef);
                }

                text = "TM_AbilityDescBaseManaCost".Translate(new object[]
                {
                    magicAbilityDef.manaCost.ToString("p1")
                }) + "\n" + "TM_AbilityDescAdjustedManaCost".Translate(new object[]
                {
                    num.ToString("p1")
                });

                if(this.MagicUser.coolDown != 1f)
                {
                    text3 = "TM_AdjustedCooldown".Translate(new object[]
                    {
                        ((this.MaxCastingTicks * this.MagicUser.coolDown)/60).ToString("0.00")
                    });
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
                            reason = "TM_NotEnoughMana".Translate(new object[]
                            {
                                base.Pawn.LabelShort
                            });
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
                        this.magicDef.defName == "TM_Poison" ) )
                    {
                        reason = "TM_ShieldBlockingPowers".Translate(new object[]
                        {
                            base.Pawn.Label,
                            wornApparel[i].Label
                        });
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
