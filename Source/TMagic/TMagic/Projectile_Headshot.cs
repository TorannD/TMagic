using RimWorld;
using Verse;
using AbilityUser;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;

namespace TorannMagic
{
    public class Projectile_Headshot : Projectile_AbilityBase
    {

        public float destroyParentPartPctTo = 0.50f;
        Pawn pawn;

        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            base.Impact(hitThing);
            ThingDef def = this.def;

            pawn = this.launcher as Pawn;
            Pawn victim = hitThing as Pawn;
            CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();
            MightPowerSkill ver = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Headshot.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Headshot_ver");

            CellRect cellRect = CellRect.CenteredOn(base.Position, 1);
            cellRect.ClipInsideMap(map);

            int dmg = GetWeaponDmg(pawn, this.def);

            if (victim != null && Rand.Chance(this.launcher.GetStatValue(StatDefOf.ShootingAccuracy, true)))
            {                
                this.PenetratingShot(victim, dmg, DamageDefOf.Bullet);

                if (victim.Dead)
                {
                    comp.Stamina.CurLevel += (.1f * ver.level);
                }
            }
        }

        public static int GetWeaponDmg(Pawn pawn, ThingDef projectileDef)
        {
            CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();
            MightPowerSkill pwr = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Headshot.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Headshot_pwr");            
            MightPowerSkill str = comp.MightData.MightPowerSkill_global_strength.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_strength_pwr");
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
                dmg = (projectileDef.projectile.damageAmountBase) + (int)((20 + (value / 120)) * (1 + (.1f * pwr.level) + (.05f * str.level)));
            }
            else
            {
                dmg = (projectileDef.projectile.damageAmountBase) + (int)((value / 50) * (1 + (.1f * pwr.level) + (.05f * str.level)));
            }
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (!pawn.IsColonist && settingsRef.AIHardMode)
            {
                dmg += 8;
            }

            return dmg;
        }

        public void PenetratingShot(Pawn victim, int dmg, DamageDef dmgType)
        {
            BodyPartRecord vitalPart = null;
            if (victim != null && !victim.Dead)
            {
                IEnumerable<BodyPartRecord> partSearch = victim.def.race.body.AllParts;
                vitalPart = partSearch.FirstOrDefault<BodyPartRecord>((BodyPartRecord x) => x.def.tags.Contains("ConsciousnessSource"));
                if (vitalPart != null)
                {
                    this.HitBodyPartOrParent(victim, dmg, dmgType, vitalPart, 0);
                }
                else
                {
                    vitalPart = partSearch.FirstOrDefault<BodyPartRecord>((BodyPartRecord x) => x.def.tags.Contains("BloodPumpingSource"));
                    if (vitalPart != null)
                    {
                        this.HitBodyPartOrParent(victim, dmg, dmgType, vitalPart, 0);
                    }
                    else
                    {
                        vitalPart = partSearch.FirstOrDefault<BodyPartRecord>((BodyPartRecord x) => x.def.tags.Contains("Spine"));
                        if (vitalPart != null)
                        {
                            this.HitBodyPartOrParent(victim, dmg, dmgType, vitalPart, 0);
                        }
                        else
                        {
                            Log.Message("did not find a vital organ, no extra damage applied");
                        }
                    }
                }
            }
        }

        public int HitBodyPartOrParent(Pawn victim, int dmg, DamageDef dmgType, BodyPartRecord hitPart, int penetratedParts)
        {
            BodyPartRecord parentPart = null;
            if (hitPart.parent != null && hitPart.depth == BodyPartDepth.Inside)
            {
                parentPart = hitPart.parent;
                dmg = this.HitBodyPartOrParent(victim, dmg, dmgType, parentPart, penetratedParts + 1);
                if (penetratedParts == 0)
                {
                    damageEntities(victim, hitPart, dmg, dmgType, penetratedParts);
                    dmg = 0;
                }
                else
                {
                    float maxDmg = this.destroyParentPartPctTo * hitPart.def.GetMaxHealth(victim);
                    if (dmg > maxDmg)
                    {
                        damageEntities(victim, hitPart, (int)maxDmg , dmgType, penetratedParts);
                        dmg = dmg - (int)maxDmg;
                    }
                    else
                    {
                        damageEntities(victim, hitPart, dmg , dmgType, penetratedParts);
                        dmg = 0;
                    }                    
                }
                return dmg;
            }
            else
            {
                float maxDmg = (this.destroyParentPartPctTo * hitPart.def.GetMaxHealth(victim));
                if ( dmg > maxDmg )
                {
                    damageEntities(victim, hitPart, (int)maxDmg, dmgType, penetratedParts);
                    dmg = dmg - (int)maxDmg;                 
                }
                else
                {
                    damageEntities(victim, hitPart, dmg, dmgType, penetratedParts);
                    
                    dmg = 0;
                }                
                return dmg;
            }            
        }

        public void damageEntities(Pawn victim, BodyPartRecord hitPart, int amt, DamageDef type, int penetratedParts)
        {
            DamageInfo dinfo;
            amt = (int)((float)amt * Rand.Range(.5f, 1.5f));
            
            if (hitPart.def.GetMaxHealth(victim) > amt)
            {
                //Very large animals or creatures
                dinfo = new DamageInfo(type, amt, (float)-1, pawn, hitPart, pawn.equipment.Primary.def, DamageInfo.SourceCategory.ThingOrUnknown);
            }
            else
            {
                amt = (int)(amt / (1 + penetratedParts));
                dinfo = new DamageInfo(type, amt, (float)-1, pawn, hitPart, pawn.equipment.Primary.def, DamageInfo.SourceCategory.ThingOrUnknown);
            }
            dinfo.SetAllowDamagePropagation(false);
            //DamageWorker_AddInjury inj = new DamageWorker_AddInjury();
            //inj.Apply(dinfo, victim);
            victim.TakeDamage(dinfo);
            if (!victim.IsColonist && !victim.IsPrisoner && victim.Faction != null && !victim.Faction.HostileTo(pawn.Faction))
            {
                Faction faction = victim.Faction;
                faction.SetHostileTo(pawn.Faction, true);
            }
        }
    }
}
