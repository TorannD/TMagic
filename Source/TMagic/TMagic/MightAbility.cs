using AbilityUser;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using UnityEngine;

namespace TorannMagic
{
    public class MightAbility : PawnAbility
    {

        public int CastingTicks
        {
            get
            {
                return this.MaxCastingTicks;
            }
        }
       
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
                    this.MightUser.MightUserXP += (int)((mightDef.staminaCost * 180) * this.MightUser.xpGain * settingsRef.xpMultiplier);
                    
                }
            }
        }

        public override string PostAbilityVerbCompDesc(VerbProperties_Ability verbDef)
        {
            TMAbilityDef mightAbilityDef = (TMAbilityDef)verbDef.abilityDef;            
            return PostAbilityDesc(mightAbilityDef, this.MightUser, this.MaxCastingTicks);
        }

        public static string PostAbilityDesc(TMAbilityDef mightAbilityDef, CompAbilityUserMight mightUser, int maxCastingTicks)
        {
            string result = "";
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = mightAbilityDef != null;
            if (flag)
            {
                string text = "";
                string text2 = "";
                string text3 = "";
                float num = 0;
                float num2 = 0;


                if (mightAbilityDef == TorannMagicDefOf.TM_Whirlwind)//mightAbilityDef == TorannMagicDefOf.)
                {
                    num = mightUser.ActualStaminaCost(mightAbilityDef);

                    num2 = FlyingObject_Whirlwind.GetWeaponDmg(mightUser.Pawn);
                    text2 = "TM_WhirlwindDamage".Translate(
                        num2.ToString()
                    );

                }
                else if (mightAbilityDef == TorannMagicDefOf.TM_Cleave)
                {
                    num = mightUser.ActualStaminaCost(mightAbilityDef);
                    if (mightUser.Pawn.equipment.Primary != null && !mightUser.Pawn.equipment.Primary.def.IsRangedWeapon)
                    {
                        num2 = Mathf.Min((mightUser.Pawn.equipment.Primary.def.BaseMass * .3f) * 100f, 65f);
                        text2 = "TM_CleaveChance".Translate(
                            num2.ToString()
                        );
                    }
                    else
                    {
                        text2 = "TM_CleaveChance".Translate(
                            num2.ToString()
                        );
                    }

                }
                else if (mightUser.Pawn.equipment.Primary != null && mightUser.Pawn.equipment.Primary.def.IsRangedWeapon)
                {
                    num = mightUser.ActualStaminaCost(mightAbilityDef);
                    if (mightAbilityDef == TorannMagicDefOf.TM_Headshot)
                    {
                        num2 = Projectile_Headshot.GetWeaponDmg(mightUser.Pawn, ThingDef.Named("Projectile_Headshot"));
                        text2 = "TM_WeaponDamage".Translate(
                        mightAbilityDef.label,
                        num2.ToString()
                        );
                    }
                    if (mightAbilityDef == TorannMagicDefOf.TM_AntiArmor)
                    {
                        num2 = Projectile_AntiArmor.GetWeaponDmg(mightUser.Pawn, ThingDef.Named("Projectile_AntiArmor"));
                        float num3 = Projectile_AntiArmor.GetWeaponDmgMech(mightUser.Pawn, Mathf.RoundToInt(num2));
                        text2 = "TM_AntiArmorDamage".Translate(
                        mightAbilityDef.label,
                        num2.ToString(),
                        num3.ToString()
                        );
                    }
                    if (mightAbilityDef == TorannMagicDefOf.TM_ArrowStorm || mightAbilityDef == TorannMagicDefOf.TM_ArrowStorm_I || mightAbilityDef == TorannMagicDefOf.TM_ArrowStorm_II || mightAbilityDef == TorannMagicDefOf.TM_ArrowStorm_III)
                    {
                        num2 = Projectile_ArrowStorm.GetWeaponDmg(mightUser.Pawn, ThingDef.Named("Projectile_ArrowStorm"));
                        int num3 = Mathf.RoundToInt(Projectile_ArrowStorm.GetWeaponAccuracy(mightUser.Pawn) * 100f);
                        text2 = "TM_ArrowStormDamage".Translate(
                        mightAbilityDef.label,
                        num2.ToString(),
                        num3.ToString()
                        );
                    }

                }
                else if (mightUser.Pawn.equipment.Primary != null && !mightUser.Pawn.equipment.Primary.def.IsRangedWeapon)
                {
                    num = mightUser.ActualStaminaCost(mightAbilityDef);
                    if (mightAbilityDef == TorannMagicDefOf.TM_PhaseStrike || mightAbilityDef == TorannMagicDefOf.TM_PhaseStrike_I || mightAbilityDef == TorannMagicDefOf.TM_PhaseStrike_II || mightAbilityDef == TorannMagicDefOf.TM_PhaseStrike_III)
                    {
                        num2 = Verb_PhaseStrike.GetWeaponDmg(mightUser.Pawn);
                        text2 = "TM_WeaponDamage".Translate(
                        mightAbilityDef.label,
                        num2.ToString()
                        );
                    }
                    if (mightAbilityDef == TorannMagicDefOf.TM_BladeSpin)
                    {
                        num = mightUser.ActualStaminaCost(mightAbilityDef);
                        num2 = Verb_BladeSpin.GetWeaponDmg(mightUser.Pawn);
                        text2 = "TM_WeaponDamage".Translate(
                        mightAbilityDef.label,
                        num2.ToString()
                        );
                    }
                    if (mightAbilityDef == TorannMagicDefOf.TM_SeismicSlash)
                    {
                        num = mightUser.ActualStaminaCost(mightAbilityDef);
                        num2 = Verb_SeismicSlash.GetWeaponDmg(mightUser.Pawn);
                        text2 = "TM_WeaponDamage".Translate(
                        mightAbilityDef.label,
                        num2.ToString()
                        );
                    }
                }
                else if (mightUser.Pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_PsionicHD"), false)) 
                {
                    num = mightUser.ActualStaminaCost(mightAbilityDef);
                    if (mightAbilityDef == TorannMagicDefOf.TM_PsionicBlast || mightAbilityDef == TorannMagicDefOf.TM_PsionicBlast_I || mightAbilityDef == TorannMagicDefOf.TM_PsionicBlast_II || mightAbilityDef == TorannMagicDefOf.TM_PsionicBlast_III)
                    {
                        num2 = 4 - (mightUser.MightData.MightPowerSkill_PsionicBlast.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicBlast_ver").level);                     
                        text2 = "TM_PsionicInitialCost".Translate(
                            20
                        ) + "\n" + "TM_PsionicBlastAddCost".Translate(
                            num2
                        );
                    }
                    if (mightAbilityDef == TorannMagicDefOf.TM_PsionicDash)
                    {
                        num2 = 8 - (mightUser.MightData.MightPowerSkill_PsionicDash.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicDash_eff").level);
                        text2 = "TM_PsionicInitialCost".Translate(
                            num2
                        );
                    }
                    if (mightAbilityDef == TorannMagicDefOf.TM_PsionicBarrier)
                    {
                        num2 = 8 - (mightUser.MightData.MightPowerSkill_PsionicDash.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicDash_eff").level);
                        text2 = "TM_PsionicBarrierMaintenanceCost".Translate(
                            20
                        ) + "\n" + "TM_PsionicBarrierConversionRate".Translate(
                            num2
                        );
                    }
                    if (mightAbilityDef == TorannMagicDefOf.TM_PsionicStorm)
                    {
                        num2 = 65 - (mightUser.MightData.MightPowerSkill_PsionicStorm.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicStorm_eff").level);
                        text2 = "TM_PsionicInitialCost".Translate(
                            num2
                        );
                    }
                }
                else
                {
                    num = mightUser.ActualStaminaCost(mightAbilityDef);
                }

                text = "TM_AbilityDescBaseStaminaCost".Translate(
                    mightAbilityDef.staminaCost.ToString("p1")
                ) + "\n" + "TM_AbilityDescAdjustedStaminaCost".Translate(
                    num.ToString("p1")
                );

                if (mightUser.coolDown != 1f && maxCastingTicks != 0)
                {
                    text3 = "TM_AdjustedCooldown".Translate(
                        ((maxCastingTicks * mightUser.coolDown)/60).ToString("0.00")
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
                result = stringBuilder.ToString();
                bool flag4 = text3 != "";
                if (flag4)
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
                    bool flag4 = this.MightUser.Stamina != null;
                    if (flag4)
                    {
                        bool flag5 = mightDef.staminaCost > 0f && this.ActualStaminaCost > this.MightUser.Stamina.CurLevel;
                        if (flag5)
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
                        (this.mightDef.defName == "TM_Headshot" || 
                        this.mightDef.defName == "TM_DisablingShot" || this.mightDef.defName == "TM_DisablingShot_I" || this.mightDef.defName == "TM_DisablingShot_II" || this.mightDef.defName == "TM_DisablingShot_III" || 
                        this.mightDef.defName == "TM_AntiArmor" || 
                        this.mightDef.defName == "TM_ArrowStorm" || this.mightDef.defName == "TM_ArrowStorm_I" || this.mightDef.defName == "TM_ArrowStorm_II" || this.mightDef.defName == "TM_ArrowStorm_III" ||
                        this.mightDef.defName == "TM_PsionicStorm" ||
                        this.mightDef.defName == "TM_PsionicBlast" || this.mightDef.defName == "TM_PsionicBlast_I" || this.mightDef.defName == "TM_PsionicBlast_II" || this.mightDef.defName == "TM_PsionicBlast_III" || 
                        this.mightDef.defName == "TM_Mimic"))
                    {
                        reason = "TM_ShieldBlockingPowers".Translate(
                            base.Pawn.Label,
                            wornApparel[i].Label
                        );
                        return false;
                    }
                }
                if (this.MightUser.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic))
                {
                    if (this.MightUser.Pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_PsionicHD"), false))
                    {
                        float psiEnergy = this.MightUser.Pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_PsionicHD"), false).Severity;
                        if ((this.mightDef.defName == "TM_PsionicBlast" || this.mightDef.defName == "TM_PsionicBlast_I" || this.mightDef.defName == "TM_PsionicBlast_II" || this.mightDef.defName == "TM_PsionicBlast_III") && psiEnergy < 20f)
                        {
                            reason = "TM_NotEnoughPsionicEnergy".Translate(
                            base.Pawn.Label,
                            "Psionic Blast"
                            );
                            return false;
                        }
                        if ((this.mightDef.defName == "TM_PsionicDash" && psiEnergy < 8f))
                        {
                            reason = "TM_NotEnoughPsionicEnergy".Translate(
                            base.Pawn.Label,
                            "Psionic Dash"
                            );
                            return false;
                        }
                        int stormCost = 65 - (this.MightUser.MightData.MightPowerSkill_PsionicStorm.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicStorm_eff").level);
                        if ((this.mightDef.defName == "TM_PsionicStorm" && psiEnergy < stormCost))
                        {
                            reason = "TM_NotEnoughPsionicEnergy".Translate(
                            base.Pawn.Label,
                            "Psionic Storm"
                            );
                            return false;
                        }
                    }
                    else
                    {
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
