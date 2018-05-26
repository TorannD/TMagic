using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using AbilityUser;
using Verse;


namespace TorannMagic
{
    public class Verb_Regenerate : Verb_UseAbility
    {
        bool validTarg;
        private int verVal;
        private int pwrVal;
        //Used specifically for non-unique verbs that ignore LOS (can be used with shield belt)
        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            if (targ.IsValid && targ.CenterVector3.InBounds(base.CasterPawn.Map) && !targ.Cell.Fogged(base.CasterPawn.Map) && targ.Cell.Walkable(base.CasterPawn.Map))
            {
                if ((root - targ.Cell).LengthHorizontal < this.verbProps.range)
                {
                    validTarg = true;
                }
                else
                {
                    //out of range
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
            bool flag = false;

            Map map = base.CasterPawn.Map;

            Pawn hitPawn = (Pawn)this.currentTarget;
            Pawn caster = base.CasterPawn;

            MagicPowerSkill pwr = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Regenerate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Regenerate_pwr");
            MagicPowerSkill ver = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Regenerate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Regenerate_ver");
            verVal = ver.level;
            pwrVal = pwr.level;
            if (caster.story.traits.HasTrait(TorannMagicDefOf.Faceless))
            {
                MightPowerSkill mpwr = caster.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_pwr");
                MightPowerSkill mver = caster.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_ver");
                pwrVal = mpwr.level;
                verVal = mver.level;
            }
            if (hitPawn != null & !hitPawn.Dead)
            {
                if(pwrVal == 3)
                {
                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_Regeneration_III, Rand.Range(1f + verVal, 3f + (verVal * 3)));
                    TM_MoteMaker.ThrowRegenMote(hitPawn.Position.ToVector3(), map, 1f + (.2f * (verVal + pwrVal)));
                }
                else if (pwrVal == 2)
                {
                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_Regeneration_II, Rand.Range(1f + verVal, 3f + (verVal * 3)));
                    TM_MoteMaker.ThrowRegenMote(hitPawn.Position.ToVector3(), map, 1f + (.2f * (verVal + pwrVal)));
                }
                else if (pwrVal == 1)
                {
                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_Regeneration_I, Rand.Range(1f+verVal, 3f+(verVal*3)));
                    TM_MoteMaker.ThrowRegenMote(hitPawn.Position.ToVector3(), map, 1f + (.2f * (verVal + pwrVal)));
                }
                else
                {
                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_Regeneration, Rand.Range(1f + verVal, 3f + (verVal * 3)));
                    TM_MoteMaker.ThrowRegenMote(hitPawn.Position.ToVector3(), map, 1f + (.2f * (verVal + pwrVal)));
                }                
            }
            else
            {
                Messages.Message("TM_NothingToRegenerate".Translate(), MessageTypeDefOf.NeutralEvent);
            }
            this.PostCastShot(flag, out flag);
            return flag;
        }
    }
}
