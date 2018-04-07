using RimWorld;
using Verse;
using AbilityUser;
using System.Linq;
using UnityEngine;

namespace TorannMagic
{
    public class Projectile_AntiArmor : Projectile_AbilityBase
    {

        float xProb;
        IntVec3 newPos;
        bool xflag = false;
        bool zflag = false;
        int value = 0;

        private int verVal;
        private int pwrVal;

        private void Initialize(IntVec3 target, Pawn pawn)
        {
            newPos = target;
            XProb(target, pawn);
        }

        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            base.Impact(hitThing);
            ThingDef def = this.def;

            Pawn pawn = this.launcher as Pawn;
            Pawn victim = hitThing as Pawn;

            CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();
            MightPowerSkill pwr = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_AntiArmor.FirstOrDefault((MightPowerSkill x) => x.label == "TM_AntiArmor_pwr");
            MightPowerSkill ver = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_AntiArmor.FirstOrDefault((MightPowerSkill x) => x.label == "TM_AntiArmor_ver");
            MightPowerSkill str = comp.MightData.MightPowerSkill_global_strength.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_strength_pwr");
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            pwrVal = pwr.level;
            verVal = ver.level;
            if (settingsRef.AIHardMode && !pawn.IsColonist)
            {
                pwrVal = 3;
                verVal = 3;
            }
            this.Initialize(base.Position, pawn);

            if (victim != null && !victim.Dead && Rand.Chance(this.launcher.GetStatValue(StatDefOf.ShootingAccuracy, true)))
            {
                int dmg = GetWeaponDmg(pawn, this.def);
                if (!victim.RaceProps.IsFlesh)
                {
                    MoteMaker.ThrowMicroSparks(victim.Position.ToVector3(), map);
                    damageEntities(victim, null, dmg, DamageDefOf.Bullet);
                    MoteMaker.MakeStaticMote(victim.Position, pawn.Map, ThingDefOf.Mote_ExplosionFlash, 4f);
                    damageEntities(victim, null, GetWeaponDmgMech(pawn, dmg), DamageDefOf.Bullet);
                    MoteMaker.ThrowMicroSparks(victim.Position.ToVector3(), map);
                    for (int i = 0; i < 1 + verVal; i++)
                    {
                        GenExplosion.DoExplosion(newPos, map, Rand.Range((.1f) * (1 + verVal), (.3f) * (1 + verVal)), DamageDefOf.Bomb, this.launcher, (this.def.projectile.damageAmountBase / 4) * (1 + verVal), SoundDefOf.BulletImpactMetal, def, this.equipmentDef, null, 0f, 1, false, null, 0f, 1, 0f, true);
                        GenExplosion.DoExplosion(newPos, map, Rand.Range((.2f)*(1+verVal), (.4f)*(1+verVal)), DamageDefOf.Stun, this.launcher, (this.def.projectile.damageAmountBase / 2) * (1+ verVal), SoundDefOf.BulletImpactMetal, def, this.equipmentDef, null, 0f, 1, false, null, 0f, 1, 0f, true);
                        newPos = GetNewPos(newPos, pawn.Position.x <= victim.Position.x, pawn.Position.z <= victim.Position.z, false, 0, 0, xProb, 1 - xProb);                        
                        MoteMaker.ThrowMicroSparks(victim.Position.ToVector3(), base.Map);
                        MoteMaker.ThrowDustPuff(newPos, map, Rand.Range(1.2f, 2.4f));
                    }
                }
                else
                {
                    damageEntities(victim, null, dmg, DamageDefOf.Bullet);
                }
            }
            else
            {
                Log.Message("No valid target for anti armor shot or missed");
            }
        }

        public static int GetWeaponDmg(Pawn pawn, ThingDef projectileDef)
        {
            MightPowerSkill str = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_global_strength.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_strength_pwr");
            int value = 0;
            ThingWithComps arg_3C_0;
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
                dmg = (projectileDef.projectile.damageAmountBase) + (int)((16.5f + (value / 150)) * (1 + .05f * str.level));
            }
            else
            {
                dmg = (projectileDef.projectile.damageAmountBase) + (int)((value / 60) * (1 + .05f * str.level));
            }
            return dmg;
        }

        public static int GetWeaponDmgMech(Pawn pawn, int dmg)
        {
            
            MightPowerSkill pwr = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_AntiArmor.FirstOrDefault((MightPowerSkill x) => x.label == "TM_AntiArmor_pwr");
            int mechDmg = dmg + Mathf.RoundToInt(dmg * (1 + .5f * pwr.level));
            return mechDmg;
        }

        public void damageEntities(Pawn victim, BodyPartRecord hitPart, int amt, DamageDef type)
        {
            DamageInfo dinfo;
            amt = (int)((float)amt * Rand.Range(.5f, 1.5f));
            if ( hitPart != null)
            {
                dinfo = new DamageInfo(type, amt, (float)-1, this.launcher as Pawn, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown);
            }
            else
            {
                dinfo = new DamageInfo(type, amt, this.ExactRotation.eulerAngles.y, this.launcher as Pawn, null, this.equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown);                
            }
            victim.TakeDamage(dinfo);
        }

        private void XProb(IntVec3 target, Pawn pawn)
        {
            float hyp = 0;
            float angleRad = 0;
            float angleDeg = 0;

            hyp = Mathf.Sqrt((Mathf.Pow(pawn.Position.x - target.x, 2)) + (Mathf.Pow(pawn.Position.z - target.z, 2)));
            angleRad = Mathf.Asin(Mathf.Abs(pawn.Position.x - target.x) / hyp);
            angleDeg = Mathf.Rad2Deg * angleRad;
            xProb = angleDeg / 90;
        }

        private IntVec3 GetNewPos(IntVec3 curPos, bool xdir, bool zdir, bool halfway, float zvar, float xvar, float xguide, float zguide)
        {
            float rand = (float)Rand.Range(0, 100);
            bool flagx = rand <= ((xguide + Mathf.Abs(xvar)) * 100) && !xflag;
            bool flagz = rand <= ((zguide + Mathf.Abs(zvar)) * 100) && !zflag;

            if (halfway)
            {
                xvar = (-1 * xvar);
                zvar = (-1 * zvar);
            }

            if (xdir && zdir)
            {
                //top right
                if (flagx)
                {
                    if (xguide + xvar >= 0) { curPos.x++; }
                    else { curPos.x--; }
                }
                if (flagz)
                {
                    if (zguide + zvar >= 0) { curPos.z++; }
                    else { curPos.z--; }
                }
            }
            if (xdir && !zdir)
            {
                //bottom right
                if (flagx)
                {
                    if (xguide + xvar >= 0) { curPos.x++; }
                    else { curPos.x--; }
                }
                if (flagz)
                {
                    if ((-1 * zguide) + zvar >= 0) { curPos.z++; }
                    else { curPos.z--; }
                }
            }
            if (!xdir && zdir)
            {
                //top left
                if (flagx)
                {
                    if ((-1 * xguide) + xvar >= 0) { curPos.x++; }
                    else { curPos.x--; }
                }
                if (flagz)
                {
                    if (zguide + zvar >= 0) { curPos.z++; }
                    else { curPos.z--; }
                }
            }
            if (!xdir && !zdir)
            {
                //bottom left
                if (flagx)
                {
                    if ((-1 * xguide) + xvar >= 0) { curPos.x++; }
                    else { curPos.x--; }
                }
                if (flagz)
                {
                    if ((-1 * zguide) + zvar >= 0) { curPos.z++; }
                    else { curPos.z--; }
                }
            }
            else
            {
                //no direction identified
            }
            return curPos;
            //return curPos;
        }
    }
}