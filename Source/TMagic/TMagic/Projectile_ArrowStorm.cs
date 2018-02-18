using System.Linq;
using RimWorld;
using Verse;
using AbilityUser;
using UnityEngine;

namespace TorannMagic
{
	public class Projectile_ArrowStorm : Projectile_AbilityBase
	{

        private bool initialized = false;
        Pawn pawn;
        MightPowerSkill pwr;
        MightPowerSkill ver;
        MightPowerSkill str;

        public void Initialize(Map map)
        {
            pawn = this.launcher as Pawn;
            CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();
            pwr = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_ArrowStorm.FirstOrDefault((MightPowerSkill x) => x.label == "TM_ArrowStorm_pwr");
            ver = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_ArrowStorm.FirstOrDefault((MightPowerSkill x) => x.label == "TM_ArrowStorm_ver");
            str = comp.MightData.MightPowerSkill_global_strength.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_strength_pwr");
            initialized = true;
        }

        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            base.Impact(hitThing);
            ThingDef def = this.def;

            Pawn victim = hitThing as Pawn;
            if (!initialized)
            {
                Initialize(map);
            }

            ThingWithComps arg_3C_0;
            int value = 0;
            if (pawn == null)
            {
                arg_3C_0 = null;
            }
            else
            {
                Pawn_EquipmentTracker expr_eq = pawn.equipment;
                arg_3C_0 = ((expr_eq != null) ? expr_eq.Primary : null);
            }
            ThingWithComps thing;
            bool flag31 = (thing = arg_3C_0) != null;
            if (flag31)
            {
                value = Mathf.RoundToInt(thing.GetStatValue(StatDefOf.MarketValue));
            }

            int dmg;
            if (value > 1000)
            {
                value -= 1000;
                dmg = (this.def.projectile.damageAmountBase) + (int)((20 + (value / 120)) * (1 + (.1f * pwr.level) + (.05f * str.level)));
            }
            else
            {
                dmg = (this.def.projectile.damageAmountBase) + (int)((value / 50) * (1 + (.1f * pwr.level) + (.05f * str.level)));
            }
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (!pawn.IsColonistPlayerControlled && settingsRef.AIHardMode)
            {
                dmg += 12;
            }

            if (victim != null && Rand.Chance(pawn.equipment.Primary.GetStatValue(StatDefOf.AccuracyMedium, true)))
            {
                damageEntities(victim, null, dmg, DamageDefOf.Arrow);
                TM_MoteMaker.ThrowBloodSquirt(victim.DrawPos, victim.Map, 1f);
            }
        }

        public void damageEntities(Pawn victim, BodyPartRecord hitPart, int amt, DamageDef type)
        {
            DamageInfo dinfo;
            amt = (int)((float)amt * Rand.Range(.7f, 1.3f));
            dinfo = new DamageInfo(type, amt, (float)-1, pawn, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown);
            dinfo.SetAllowDamagePropagation(false);
            victim.TakeDamage(dinfo);
        }
    }
}
