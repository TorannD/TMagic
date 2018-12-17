using Verse;
using AbilityUser;
using System.Linq;
using RimWorld;


namespace TorannMagic
{
    public class Projectile_SiphonMana : Projectile_AbilityBase
    {
        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            base.Impact(hitThing);
            ThingDef def = this.def;

            Pawn hitPawn = hitThing as Pawn;
            Pawn caster = this.launcher as Pawn;
            CompAbilityUserMagic compHitPawn = hitPawn.GetComp<CompAbilityUserMagic>();            
            CompAbilityUserMagic compCaster = caster.GetComp<CompAbilityUserMagic>();

            if (hitPawn != null && !hitPawn.Dead && !caster.Dead && !caster.Downed && caster != null)
            {
                if (Rand.Chance(TM_Calc.GetSpellSuccessChance(caster, hitPawn, true)))
                {
                    if (compHitPawn.IsMagicUser)
                    {
                        MagicPowerSkill regen = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_global_regen.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_regen_pwr");
                        float manaDrained = compHitPawn.Mana.CurLevel;
                        if (manaDrained > .5f)
                        {
                            manaDrained = .5f;
                        }
                        compHitPawn.Mana.CurLevel -= manaDrained;
                        compCaster.Mana.CurLevel += (manaDrained * .6f) * (1 + regen.level * .05f);
                        TM_MoteMaker.ThrowSiphonMote(hitPawn.Position.ToVector3(), hitPawn.Map, 1f);
                        TM_MoteMaker.ThrowManaPuff(caster.Position.ToVector3(), caster.Map, 1f);
                    }
                    else
                    {
                        float sev = Rand.Range(0, 10);
                        HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_AntiManipulation, sev);
                        sev = Rand.Range(0, 10);
                        HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_AntiMovement, sev);
                        sev = Rand.Range(0, 10);
                        HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_AntiBreathing, sev);
                        sev = Rand.Range(0, 10);
                        HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_AntiSight, sev);
                        TM_MoteMaker.ThrowSiphonMote(hitPawn.Position.ToVector3(), hitPawn.Map, 1f);
                        TM_MoteMaker.ThrowSiphonMote(hitPawn.Position.ToVector3(), hitPawn.Map, 1f);
                    }
                }
                else
                {
                    MoteMaker.ThrowText(hitPawn.DrawPos, hitPawn.Map, "TM_ResistedSpell".Translate(), -1);
                }
            }
        }
    }
}
