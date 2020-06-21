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
    public class CustomAbility : PawnAbility
    {

        public TMAbilityDef TMDef
        {
            get
            {
                return base.Def as TMAbilityDef;
            }
        }
        TMAbilityCost cost;
        TMAbilityCost Cost
        {
            get
            {
                if (cost == null) // TODO: handle when the Cost cannot be created
                {
                    cost = TMDef.customCost?.ForPawn(this.Pawn)
                        ?? (TMDef.manaCost > 0 ? new TMAbilityNeedCost(this.Pawn.needs.TryGetNeed(TorannMagicDefOf.TM_Mana), TMDef.manaCost * 100f)
                        : (TMDef.staminaCost > 0 ? new TMAbilityNeedCost(this.Pawn.needs.TryGetNeed(TorannMagicDefOf.TM_Stamina), TMDef.staminaCost * 100f)
                        : new TMAbilityBadCost() as TMAbilityCost));
                }
                return cost;
            }
        }
        public CustomAbility()
        {
        }

        public CustomAbility(AbilityData data) : base(data)
        {
        }

        public CustomAbility(CompAbilityUser abilityUser) : base(abilityUser)
		{
            this.abilityUser = (abilityUser as CompAbilityUserMagic);
        }

        public CustomAbility(Pawn user, AbilityUser.AbilityDef pdef) : base(user, pdef)
		{
            this.cost = (pdef as TMAbilityDef)?.customCost.ForPawn(user);
        }

        public override void PostAbilityAttempt()  //commented out in CompAbilityUserMagic
        {
            base.PostAbilityAttempt();

            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (!this.Pawn.IsColonist && settingsRef.AIAggressiveCasting)// for AI
            {
                this.CooldownTicksLeft = Mathf.RoundToInt(this.MaxCastingTicks/2f);
            }
            else
            {
                this.CooldownTicksLeft = Mathf.RoundToInt(this.MaxCastingTicks); //TODO: integrate cooldown reduction effects, Arcalleum
            }
            float cost;
            Cost.PayCost(out cost);
            CompAbilityUserCustom custom = this.AbilityUser.Pawn.TryGetComp<CompAbilityUserCustom>();
            if (custom != null && custom.Data.ReturnMatchingPower(this.Def, true)?.learned == true)
            {
                custom.Data.UserXP += Mathf.RoundToInt(cost * settingsRef.xpMultiplier); // TODO: XP modifiers (custom.Data.GainXP ? )
            }
        }

        public override string PostAbilityVerbCompDesc(VerbProperties_Ability verbDef)
        {
            return "Costs " + Cost.Description;
            //CompAbilityUserCustom custom = this.AbilityUser.Pawn.TryGetComp<CompAbilityUserCustom>();
            //BasePower power = custom.Data.ReturnMatchingPower(Def);
            //StringBuilder stringBuilder = new StringBuilder();
            //TMAbilityDef magicAbilityDef = (TMAbilityDef)verbDef.abilityDef;
            //bool flag = magicAbilityDef != null;
            //if (flag)
            //{
            //    string text = "";
            //    string text2 = "";
            //    string text3 = "";
            //    float num = 0;
            //    float num2 = 0;
                //if (magicAbilityDef == TorannMagicDefOf.TM_Teleport)
                //{
                //    num = this.MagicUser.ActualManaCost(magicDef)*100;
                //    MagicPowerSkill mps2 = this.MagicUser.MagicData.MagicPowerSkill_Teleport.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Teleport_ver");
                //    MagicPowerSkill mps1 = this.MagicUser.MagicData.MagicPowerSkill_Teleport.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Teleport_pwr");
                //    num2 = 80 + (mps1.level * 20) + (mps2.level * 20);
                //    text2 = "TM_AbilityDescPortalTime".Translate(
                //        num2.ToString()
                //    );
                //}
                //else
                //{
                //    num = this.MagicUser.ActualManaCost(magicDef) * 100;
                //}

                //if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                //{                    
                //    text = "TM_AbilityDescBaseStaminaCost".Translate(
                //        (magicAbilityDef.manaCost * 100).ToString("n1")
                //    ) + "\n" + "TM_AbilityDescAdjustedStaminaCost".Translate(
                //        (magicDef.manaCost * 100).ToString("n1")
                //    );
                //}
                //else
                //{
                //    text = "TM_AbilityDescBaseManaCost".Translate(
                //        (magicAbilityDef.manaCost * 100).ToString("n1")
                //    ) + "\n" + "TM_AbilityDescAdjustedManaCost".Translate(
                //        num.ToString("n1")
                //    );
                //}
                //if(this.MagicUser.coolDown != 1f)
                //{
                //    text3 = "TM_AdjustedCooldown".Translate(
                //        ((this.MaxCastingTicks * this.MagicUser.coolDown) / 60).ToString("0.00")
                //    );
                //}
                //bool flag4 = text3 != "";
                //if(flag4)
                //{
                //    stringBuilder.AppendLine(text3);
                //}
            //}
            //return stringBuilder.ToString();
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
                    float actualCost;
                    if (Cost.CanPayCost(out actualCost))
                    {
                        bool flagMute = this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_MuteHD);
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
                        bool flag7 = mightComp != null && mightComp.Stamina != null && actualCost < mightComp.Stamina.CurLevel;
                        if (flag7)
                        {
                            reason = "TM_NotEnoughStamina".Translate(
                            base.Pawn.LabelShort
                            );
                            result = false;
                            return result;
                        }
                    }
                    else
                    {
                        reason = "Cannot pay " + Cost.Description;
                        return false;
                    }
                }
                //List<Apparel> wornApparel = base.Pawn.apparel.WornApparel;
                //for (int i = 0; i < wornApparel.Count; i++)
                //{
                //    if (!wornApparel[i].AllowVerbCast(base.Pawn.Position, base.Pawn.Map, base.abilityUser.Pawn.TargetCurrentlyAimingAt, this.Verb) &&
                //        (this.magicDef.defName == "TM_LightningCloud" || this.magicDef.defName == "Laser_LightningBolt" || this.magicDef.defName == "TM_LightningStorm" || this.magicDef.defName == "TM_EyeOfTheStorm" ||
                //        this.magicDef.defName.Contains("Laser_FrostRay") || this.magicDef.defName == "TM_Blizzard" || this.magicDef.defName == "TM_Snowball" || this.magicDef.defName == "TM_Icebolt" ||
                //        this.magicDef.defName == "TM_Firestorm" || this.magicDef.defName == "TM_Fireball" || this.magicDef.defName == "TM_Fireclaw" || this.magicDef.defName == "TM_Firebolt" ||
                //        this.magicDef.defName.Contains("TM_MagicMissile") ||
                //        this.magicDef.defName.Contains("TM_DeathBolt") ||
                //        this.magicDef.defName.Contains("TM_ShadowBolt") ||
                //        this.magicDef.defName == "TM_BloodForBlood" || this.magicDef.defName == "TM_IgniteBlood" ||
                //        this.magicDef.defName == "TM_Poison" ||
                //        this.magicDef == TorannMagicDefOf.TM_ArcaneBolt) )
                //    {
                //        reason = "TM_ShieldBlockingPowers".Translate(
                //            base.Pawn.Label,
                //            wornApparel[i].Label
                //        );
                //        return false;
                //    }
                //}
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
