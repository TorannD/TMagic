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
        protected override bool TryCastShot()
        {
            bool flag = false;

            Map map = base.CasterPawn.Map;

            Pawn hitPawn = (Pawn)this.currentTarget;
            Pawn caster = base.CasterPawn;

            MagicPowerSkill pwr = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Regenerate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Regenerate_pwr");
            MagicPowerSkill ver = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Regenerate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Regenerate_ver");

            if (hitPawn != null & !hitPawn.Dead)
            {
                if(pwr.level == 3)
                {
                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_Regeneration_III, Rand.Range(1f + ver.level, 3f + (ver.level * 3)));
                    TM_MoteMaker.ThrowRegenMote(hitPawn.Position.ToVector3(), map, 1f + (.2f * (ver.level + pwr.level)));
                }
                else if (pwr.level == 2)
                {
                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_Regeneration_II, Rand.Range(1f + ver.level, 3f + (ver.level * 3)));
                    TM_MoteMaker.ThrowRegenMote(hitPawn.Position.ToVector3(), map, 1f + (.2f * (ver.level + pwr.level)));
                }
                else if (pwr.level == 1)
                {
                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_Regeneration_I, Rand.Range(1f+ver.level, 3f+(ver.level*3)));
                    TM_MoteMaker.ThrowRegenMote(hitPawn.Position.ToVector3(), map, 1f + (.2f * (ver.level + pwr.level)));
                }
                else
                {
                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_Regeneration, Rand.Range(1f + ver.level, 3f + (ver.level * 3)));
                    TM_MoteMaker.ThrowRegenMote(hitPawn.Position.ToVector3(), map, 1f + (.2f * (ver.level + pwr.level)));
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
