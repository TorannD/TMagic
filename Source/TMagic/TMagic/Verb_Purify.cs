using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using AbilityUser;
using Verse;
using UnityEngine;


namespace TorannMagic
{
    public class Verb_Purify : Verb_UseAbility
    {
        private int verVal;
        private int pwrVal;
        float arcaneDmg = 1f;

        public VerbProperties_Purify Props => (VerbProperties_Purify)UseAbilityProps;

        bool validTarg;
        //Used for non-unique abilities that can be used with shieldbelt
        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            if (targ.Thing != null && targ.Thing == this.caster)
            {
                return this.verbProps.targetParams.canTargetSelf;
            }
            if (targ.IsValid && targ.CenterVector3.InBounds(base.CasterPawn.Map) && !targ.Cell.Fogged(base.CasterPawn.Map) && targ.Cell.Walkable(base.CasterPawn.Map))
            {
                if ((root - targ.Cell).LengthHorizontal < this.verbProps.range)
                {
                    ShootLine shootLine;
                    validTarg = this.TryFindShootLineFromTo(root, targ, out shootLine);
                }
                else
                {
                    validTarg = false;
                }
            }
            else
            {
                validTarg = false;
            }
            return validTarg;
        }

        protected override bool TryCastShot()
        {
            Pawn caster = this.CasterPawn;
            Pawn pawn = this.currentTarget.Thing as Pawn;
            CompAbilityUserMagic comp = caster.GetComp<CompAbilityUserMagic>();
            if (comp != null && !caster.story.traits.HasTrait(TorannMagicDefOf.Faceless))
            {
                MagicPowerSkill pwr = comp.MagicData.MagicPowerSkill_Purify.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Purify_pwr");
                MagicPowerSkill ver = comp.MagicData.MagicPowerSkill_Purify.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Purify_ver");
                pwrVal = pwr.level;
                verVal = ver.level;
                arcaneDmg = comp.arcaneDmg;
            }
            else if (caster.story.traits.HasTrait(TorannMagicDefOf.Faceless))
            {

                MightPowerSkill mpwr = caster.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_pwr");
                MightPowerSkill mver = caster.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_ver");
                pwrVal = mpwr.level;
                verVal = mver.level;
                arcaneDmg = caster.GetComp<CompAbilityUserMight>().mightPwr;

            }
            bool flag = pawn != null && !pawn.Dead;
            if (flag)
            {
                int numberOfThingsToHeal = Mathf.RoundToInt(1f + (.4f * verVal));
                using (IEnumerator<BodyPartRecord> enumerator = pawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        BodyPartRecord rec = enumerator.Current;
                        bool flag2 = numberOfThingsToHeal > 0;
                        if (flag2)
                        {
                            int num2 = 1 + verVal;
                            IEnumerable<Hediff_Injury> arg_BB_0 = pawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                            Func<Hediff_Injury, bool> arg_BB_1;

                            arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                            foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                            {
                                bool flag4 = num2 > 0;
                                if (flag4)
                                {
                                    bool flag5 = !current.CanHealNaturally() && current.IsPermanent();
                                    if (flag5)
                                    {
                                        if (rec.def.tags.Contains(BodyPartTagDefOf.ConsciousnessSource))
                                        {
                                            if (pwrVal >= 1)
                                            {
                                                current.Heal(pwrVal * arcaneDmg);
                                                numberOfThingsToHeal--;
                                                num2--;
                                            }
                                        }
                                        else
                                        {
                                            current.Heal((2f + pwrVal * 2) * arcaneDmg);
                                            //current.Heal(5.0f + (float)pwrVal * 3f); // power affects how much to heal
                                            numberOfThingsToHeal--;
                                            num2--;
                                        }
                                        TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3Shifted(), pawn.Map, .6f);
                                        TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3Shifted(), pawn.Map, .4f);
                                        
                                    }
                                }
                            }
                        }
                    }
                }

                if (numberOfThingsToHeal > 0)
                {
                    foreach (Hediff hediff in pawn.health.hediffSet.GetHediffs<Hediff>())
                    {
                        if (numberOfThingsToHeal <= 0)
                            break;

                        string hediffName = hediff.def.defName;

                        // Look up healable parameters and see if there is an entry for this hediff
                        VerbProperties_Purify.HealableHediffParameters healableParameters = Props.healableHediffs.Find(hh => hh.hediffs.Contains(hediffName));
                        if (healableParameters == null)
                            continue;

                        // Make sure we are capable of healing it
                        if (verVal < healableParameters.minLevel)
                            continue;

                        float healAmount = (healableParameters.baseAmount + (healableParameters.amountPerLevel * pwrVal));
                        if (healableParameters.useArcaneDamage)
                            healAmount *= arcaneDmg;

                        // Is the heal amount just a chance to fully remove?
                        if (healableParameters.isRemovalChance)
                        {
                            Log.Message(String.Format("{0}: {1} heal chance {2}", pawn, hediff.def.defName, healAmount));
                            if (Rand.Chance(healAmount))
                            {
                                Log.Message(String.Format("{0}: {1} heal successful!", pawn, hediff.def.defName));
                                pawn.health.RemoveHediff(hediff);
                            }
                        }
                        // Otherwise lower the severity by the heal amount
                        else
                        {
                            // If something else should be removed on a full heal
                            if (!string.IsNullOrEmpty(healableParameters.alsoRemoveOnFullHeal) && hediff.Severity <= healAmount)
                            {
                                Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named(healableParameters.alsoRemoveOnFullHeal));
                                if (firstHediffOfDef != null)
                                {
                                    Log.Message(String.Format("{0}: {1} removed", pawn, healableParameters.alsoRemoveOnFullHeal));
                                    pawn.health.RemoveHediff(firstHediffOfDef);
                                }
                            }

                            Log.Message(String.Format("{0}: {1} being healed by {2}", pawn, hediff.def.defName, healAmount));
                            hediff.Heal(healAmount);
                        }

                        TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3Shifted(), pawn.Map, 0.6f);
                        TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3Shifted(), pawn.Map, 0.4f);

                        --numberOfThingsToHeal;
                    }
                }
                //if (pawn.RaceProps.Humanlike)
                //{
                //using (IEnumerator<Hediff> enumerator = pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                //{
                //    while (enumerator.MoveNext())
                //    {
                //        Hediff rec = enumerator.Current;
                //        bool flag2 = num > 0;
                //        if (flag2)
                //        {
                //            if (rec.def.defName == "Cataract" || rec.def.defName == "HearingLoss" || rec.def.defName.Contains("ToxicBuildup"))
                //            {
                //                rec.Heal(.4f + .3f * pwrVal);
                //                num--;
                //            }
                //            if ((rec.def.defName == "Blindness" || rec.def.defName.Contains("Asthma") || rec.def.defName == "Cirrhosis" || rec.def.defName == "ChemicalDamageModerate") && verVal >= 1)
                //            {
                //                rec.Heal(.3f + .2f * pwrVal);
                //                if (rec.def.defName.Contains("Asthma"))
                //                {
                //                    pawn.health.RemoveHediff(rec);
                //                }
                //                num--;
                //            }
                //            if ((rec.def.defName == "Frail" || rec.def.defName == "BadBack" || rec.def.defName.Contains("Carcinoma") || rec.def.defName == "ChemicalDamageSevere") && verVal >= 2)
                //            {
                //                rec.Heal(.25f + .2f * pwrVal);
                //                num--;
                //            }
                //            if ((rec.def.defName.Contains("Alzheimers") || rec.def.defName == "Dementia" || rec.def.defName.Contains("HeartArteryBlockage") || rec.def.defName == "PsychicShock" || rec.def.defName == "CatatonicBreakdown") && verVal >= 3)
                //            {
                //                rec.Heal(.15f + .15f * pwrVal);
                //                num--;
                //            }
                //            if(rec.def.defName.Contains("Abasia") && verVal >= 3)
                //            {
                //                if(Rand.Chance(.25f + (.05f * pwrVal)))
                //                {
                //                    pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("Abasia")));
                //                    num--;
                //                }
                //                else
                //                {
                //                    MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, "Failed to remove Abasia...");
                //                }
                //            }
                //            TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3Shifted(), pawn.Map, .6f);
                //            TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3Shifted(), pawn.Map, .4f);
                //        }
                //    }
                //}
                ////}
                //using (IEnumerator<Hediff_Addiction> enumerator = pawn.health.hediffSet.GetHediffs<Hediff_Addiction>().GetEnumerator())
                //{
                //    while (enumerator.MoveNext())
                //    {
                //        Hediff_Addiction rec = enumerator.Current;
                //        bool flag2 = num > 0;
                //        if (flag2)
                //        {
                //            if (rec.Chemical.defName == "Alcohol" || rec.Chemical.defName == "Smokeleaf")
                //            {
                //                rec.Severity -= ((.3f + .3f * pwrVal)*arcaneDmg);
                //                num--;
                //            }
                //            if ((rec.Chemical.defName == "GoJuice" || rec.Chemical.defName == "WakeUp") && verVal >= 1)
                //            {
                //                rec.Severity -= ((.25f + .25f * pwrVal)*arcaneDmg);
                //                num--;
                //            }
                //            if (rec.Chemical.defName == "Psychite" && verVal >= 2)
                //            {
                //                rec.Severity -= ((.25f + .25f * pwrVal)*arcaneDmg);
                //                num--;
                //            }
                //            if (verVal >= 3)
                //            {
                //                if (rec.Chemical.defName == "Luciferium" && (rec.Severity - ((.15f + .15f * pwrVal)*arcaneDmg) < 0))
                //                {
                //                    Hediff luciHigh = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("LuciferiumHigh"), false);
                //                    pawn.health.RemoveHediff(luciHigh);
                //                }
                //                rec.Severity -= ((.15f + .15f * pwrVal) * arcaneDmg);
                //                num--;                                
                //            }
                //            TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3Shifted(), pawn.Map, .6f);
                //            TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3Shifted(), pawn.Map, .4f);
                //        }
                //    }
                //}
            }
            return true;
        }
    }
}
