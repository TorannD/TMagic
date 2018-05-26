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

        protected override bool TryCastShot()
        {
            Pawn caster = base.CasterPawn;
            Pawn pawn = this.currentTarget.Thing as Pawn;

            MagicPowerSkill pwr = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Purify.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Purify_pwr");
            MagicPowerSkill ver = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Purify.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Purify_ver");
            pwrVal = pwr.level;
            verVal = ver.level;
            if (pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
            {
                MightPowerSkill mpwr = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_pwr");
                MightPowerSkill mver = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_ver");
                pwrVal = mpwr.level;
                verVal = mver.level;
            }
            bool flag = pawn != null && !pawn.Dead;
            if (flag)
            {
                int num = Mathf.RoundToInt(1f + (.4f * verVal));
                using (IEnumerator<BodyPartRecord> enumerator = pawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        BodyPartRecord rec = enumerator.Current;
                        bool flag2 = num > 0;
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
                                    bool flag5 = !current.CanHealNaturally() && current.IsOld();
                                    if (flag5)
                                    {
                                        if (rec.def.tags.Contains("ConsciousnessSource"))
                                        {
                                            if (pwrVal >= 1)
                                            {
                                                current.Heal(pwrVal);
                                                num--;
                                                num2--;
                                            }
                                        }
                                        else
                                        {
                                            current.Heal(2f + pwrVal * 2);
                                            //current.Heal(5.0f + (float)pwrVal * 3f); // power affects how much to heal
                                            num--;
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
                using (IEnumerator<Hediff> enumerator = pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Hediff rec = enumerator.Current;
                        bool flag2 = num > 0;
                        if (flag2)
                        {
                            if (rec.def.defName == "Cataract" || rec.def.defName == "HearingLoss" || rec.def.defName.Contains("ToxicBuildup"))
                            {
                                rec.Heal(.4f + .3f * pwrVal);
                                num--;
                            }
                            if ((rec.def.defName == "Blindness" || rec.def.defName.Contains("Asthma") || rec.def.defName == "Cirrhosis" || rec.def.defName == "ChemicalDamageModerate") && verVal >= 1)
                            {
                                rec.Heal(.3f + .2f * pwrVal);
                                if(rec.def.defName.Contains("Asthma"))
                                {
                                    pawn.health.RemoveHediff(rec);
                                }
                                num--;
                            }
                            if ((rec.def.defName == "Frail" || rec.def.defName == "BadBack" || rec.def.defName.Contains("Carcinoma") || rec.def.defName == "ChemicalDamageSevere") && verVal >= 2)
                            {
                                rec.Heal(.2f + .15f * pwrVal);
                                num--;
                            }
                            if ((rec.def.defName.Contains("Alzheimers") || rec.def.defName == "Dementia" || rec.def.defName.Contains("HeartArteryBlockage") || rec.def.defName == "PsychicShock" || rec.def.defName == "CatatonicBreakdown") && verVal >= 3)
                            {
                                rec.Heal(.1f + .1f * pwrVal);
                                num--;
                            }
                            TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3Shifted(), pawn.Map, .6f);
                            TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3Shifted(), pawn.Map, .4f);
                        }
                    }
                }
                using (IEnumerator<Hediff_Addiction> enumerator = pawn.health.hediffSet.GetHediffs<Hediff_Addiction>().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Hediff_Addiction rec = enumerator.Current;
                        bool flag2 = num > 0;
                        if (flag2)
                        {
                            if (rec.Chemical.defName == "Alcohol" || rec.Chemical.defName == "Smokeleaf")
                            {
                                rec.Severity -= (.3f + .3f * pwrVal);
                                num--;
                            }
                            if ((rec.Chemical.defName == "GoJuice" || rec.Chemical.defName == "WakeUp") && verVal >= 1)
                            {
                                rec.Severity -= (.25f + .25f * pwrVal);
                                num--;
                            }
                            if (rec.Chemical.defName == "Psychite" && verVal >= 2)
                            {
                                rec.Severity -= (.25f + .25f * pwrVal);
                                num--;
                            }
                            if (verVal >= 3)
                            {
                                rec.Severity -= (.15f + .15f * pwrVal);
                                num--;
                            }
                            TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3Shifted(), pawn.Map, .6f);
                            TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3Shifted(), pawn.Map, .4f);
                        }
                    }
                }
            }
            return true;
        }
    }
}
