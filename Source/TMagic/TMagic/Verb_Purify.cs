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
        protected override bool TryCastShot()
        {
            Pawn caster = base.CasterPawn;
            Pawn pawn = this.currentTarget.Thing as Pawn;

            MagicPowerSkill pwr = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Purify.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Purify_pwr");
            MagicPowerSkill ver = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Purify.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Purify_ver");

            bool flag = pawn != null && !pawn.Dead;
            if (flag)
            {
                int num = Mathf.RoundToInt(1f + (.4f * ver.level));
                using (IEnumerator<BodyPartRecord> enumerator = pawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        BodyPartRecord rec = enumerator.Current;
                        bool flag2 = num > 0;
                        if (flag2)
                        {
                            int num2 = 1 + ver.level;
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
                                            if (pwr.level >= 1)
                                            {
                                                current.Heal(pwr.level);
                                                num--;
                                                num2--;
                                            }
                                        }
                                        else
                                        {
                                            current.Heal(2f + pwr.level * 2);
                                            //current.Heal(5.0f + (float)pwr.level * 3f); // power affects how much to heal
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
                                rec.Heal(.4f + .3f * pwr.level);
                                num--;
                            }
                            if ((rec.def.defName == "Blindness" || rec.def.defName.Contains("Asthma") || rec.def.defName == "Cirrhosis" || rec.def.defName == "ChemicalDamageModerate") && ver.level >= 1)
                            {
                                rec.Heal(.3f + .2f * pwr.level);
                                if(rec.def.defName.Contains("Asthma"))
                                {
                                    pawn.health.RemoveHediff(rec);
                                }
                                num--;
                            }
                            if ((rec.def.defName == "Frail" || rec.def.defName == "BadBack" || rec.def.defName.Contains("Carcinoma") || rec.def.defName == "ChemicalDamageSevere") && ver.level >= 2)
                            {
                                rec.Heal(.2f + .15f * pwr.level);
                                num--;
                            }
                            if ((rec.def.defName.Contains("Alzheimers") || rec.def.defName == "Dementia" || rec.def.defName.Contains("HeartArteryBlockage") || rec.def.defName == "PsychicShock" || rec.def.defName == "CatatonicBreakdown") && ver.level >= 3)
                            {
                                rec.Heal(.1f + .1f * pwr.level);
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
                                rec.Severity -= (.2f + .2f * pwr.level);
                                num--;
                            }
                            if ((rec.Chemical.defName == "GoJuice" || rec.Chemical.defName == "WakeUp") && ver.level >= 1)
                            {
                                rec.Severity -= (.15f + .15f * pwr.level);
                                num--;
                            }
                            if (rec.Chemical.defName == "Psychite" && ver.level >= 2)
                            {
                                rec.Severity -= (.1f + .1f * pwr.level);
                                num--;
                            }
                            if (ver.level >= 3)
                            {
                                rec.Severity -= (.05f + .05f * pwr.level);
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
