using AbilityUser;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TorannMagic
{
    class MightAbility : PawnAbility
    {
       
        public CompAbilityUserMight MightUser
        {
            get
            {
                return MightUserUtility.GetMightUser(base.Pawn);
            }
        }

        public TMAbilityDef mightDef
        {
            get
            {
                return base.Def as TMAbilityDef;
            }
        }

        private float ActualStaminaCost
        {
            get
            {
                if (mightDef != null)
                {
                    return this.MightUser.ActualStaminaCost(mightDef);
                }
                return mightDef.staminaCost;         
            }
        }

        public MightAbility()
        {
        }

        public MightAbility(CompAbilityUser abilityUser) : base(abilityUser)
		{
            this.abilityUser = (abilityUser as CompAbilityUserMight);
        }

        public MightAbility(AbilityData abilityData) : base(abilityData)
		{
            this.abilityUser = (abilityData.Pawn.AllComps.FirstOrDefault((ThingComp x) => x.GetType() == abilityData.AbilityClass) as CompAbilityUserMight);
        }

        public MightAbility(Pawn user, AbilityDef pdef) : base(user, pdef)
		{

        }

        public override void PostAbilityAttempt()  
        {
            base.PostAbilityAttempt();
            bool flag = this.mightDef != null;
            if (flag)
            {
                bool flag3 = this.MightUser.Stamina != null;
                if (flag3)
                {
                    ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                    this.MightUser.Stamina.UseMightPower(this.MightUser.ActualStaminaCost(mightDef));
                    this.MightUser.MightUserXP += (int)((mightDef.staminaCost * 120) * settingsRef.xpMultiplier);
                    
                }
            }
        }

        public override string PostAbilityVerbCompDesc(VerbProperties_Ability verbDef)
        {
            string result = "";
            StringBuilder stringBuilder = new StringBuilder();
            TMAbilityDef mightAbilityDef = (TMAbilityDef)verbDef.abilityDef;
            bool flag = mightAbilityDef != null;
            if (flag)
            {
                string text = "";
                string text2 = "";
                float num = 0;
                float num2 = 0;
                
               
                if (false )//mightAbilityDef == TorannMagicDefOf.)
                {
                    num = this.MightUser.ActualStaminaCost(mightDef);

                    //num2 = 
                    //text2 = "TM_AbilityDescPortalTime".Translate(new object[]
                    //{
                    //    num2.ToString()
                    //});
                }
                else if (false)
                {
                }
                else
                {
                    num = this.MightUser.ActualStaminaCost(mightDef);
                }

                text = "TM_AbilityDescBaseStaminaCost".Translate(new object[]
                {
                    mightAbilityDef.staminaCost.ToString("p1")
                }) + "\n" + "TM_AbilityDescAdjustedStaminaCost".Translate(new object[]
                {
                    num.ToString("p1")
                });

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
                    bool flag4 = this.MightUser.Stamina != null;
                    if (flag4)
                    {
                        bool flag5 = mightDef.staminaCost > 0f && this.ActualStaminaCost > this.MightUser.Stamina.CurLevel;
                        if (flag5)
                        {
                            reason = "TM_NotEnoughStamina".Translate(new object[]
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
                    if (!wornApparel[i].AllowVerbCast(base.Pawn.Position, base.Pawn.Map, base.abilityUser.Pawn.TargetCurrentlyAimingAt) && 
                        (this.mightDef.defName == "TM_Headshot" || this.mightDef.defName == "TM_DisablingShot" || this.mightDef.defName == "TM_DisablingShot_I" || this.mightDef.defName == "TM_DisablingShot_II" || this.mightDef.defName == "TM_DisablingShot_III" || this.mightDef.defName == "TM_AntiArmor" || 
                        this.mightDef.defName == "TM_ArrowStorm" || this.mightDef.defName == "TM_ArrowStorm_I" || this.mightDef.defName == "TM_ArrowStorm_II"|| this.mightDef.defName == "TM_ArrowStorm_III"))
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
