using RimWorld;
using Verse;
using AbilityUser;
using System.Linq;
using UnityEngine;

namespace TorannMagic
{
    class Projectile_Overwhelm : Projectile_AbilityBase
    {

        IntVec3 pos;
        MagicPowerSkill pwr;
        MagicPowerSkill ver;
        private int verVal;
        private int pwrVal;
        private float arcaneDmg = 1;

        protected override void Impact(Thing hitThing)
        {
            base.Impact(hitThing);
            Pawn pawn = this.launcher as Pawn;
            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            pwr = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Overwhelm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Overwhelm_pwr");
            ver = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Overwhelm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Overwhelm_ver");
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            pwrVal = pwr.level;
            verVal = ver.level;
            if (pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
            {
                MightPowerSkill mpwr = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_pwr");
                MightPowerSkill mver = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_ver");
                pwrVal = mpwr.level;
                verVal = mver.level;
            }
            this.arcaneDmg = comp.arcaneDmg;
            if (settingsRef.AIHardMode&& !pawn.IsColonist)
            {
                pwrVal = 3;
                verVal = 3;
            }
            Map map = pawn.Map;

            if (pawn != null)
            {
                pos = pawn.Position;

                pos.x++;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);
                pos.z--;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);
                pos.x--;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);
                pos.x--;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);
                pos.z++;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);
                pos.z++;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);
                pos.x++;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);
                pos.x++;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);


                pos.x++;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                pos.x++;
                pos.z--;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                pos.x++;
                pos.z--;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                pos.z--;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                pos.x -= 3;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                pos.x--;
                pos.z--;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                pos.x--;
                pos.z--;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);
                
                pos.x--;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                pos.z += 3;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                pos.x--;
                pos.z++;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                pos.x--;
                pos.z++;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                pos.z++;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                pos.x += 3;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                pos.x++;
                pos.z++;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                pos.x++;
                pos.z++;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                pos.x++;
                HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                if (verVal >= 1)
                {
                    pos.x++;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.z--;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x -= 3;
                    pos.z--;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x -= 3;
                    pos.z++;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x--;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.z -= 3;
                    pos.x += 2;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.z -= 3;
                    pos.x--;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.z--;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x += 3;
                    pos.z += 2;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x += 3;
                    pos.z--;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x++;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.z += 3;
                    pos.x -= 2;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x += 3;
                    pos.z++;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x -= 6;
                    pos.z += 4;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x -= 4;
                    pos.z -= 6;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x += 6;
                    pos.z -= 4;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                }

                if (verVal >= 3)
                {
                    pos.x++;
                    pos.z++;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x += 2;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x--;
                    pos.z += 3;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x++;
                    pos.z += 3;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.z += 2;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x -= 3;
                    pos.z--;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x -= 3;
                    pos.z++;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x -= 2;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x++;
                    pos.z -= 3;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x--;
                    pos.z -= 3;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.z -= 2;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x += 3;
                    pos.z++;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x += 6;
                    pos.z -= 2;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.z += 10;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.x -= 10;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                    pos.z -= 10;
                    HolyExplosion(pwrVal, verVal, pos, map, 0.4f);

                }
            }
            else
            {
                Log.Warning("failed to cast");
            }

        }

        protected void HolyExplosion(int pwr, int ver, IntVec3 pos, Map map, float radius)
        {
            ThingDef def = this.def;
            Explosion(pwr, pos, map, radius, TMDamageDefOf.DamageDefOf.TM_Overwhelm, this.launcher, null, def, this.equipmentDef, ThingDefOf.Mote_ExplosionFlash, 0.4f, 1, false, null, 0f, 1);
            
            if (ver >= 2)
            {
                int rnd = Rand.Range(1, 10);
                if (rnd >= 5)
                {
                    Explosion(pwr, pos, map, radius, DamageDefOf.Stun, this.launcher, null, def, this.equipmentDef, ThingDefOf.Mote_HeatGlow, 0.4f, 1, false, null, 0f, 1);
                }
            }
            MoteMaker.MakeStaticMote(pos, map, ThingDefOf.Mote_HeatGlow, 2f);

        }

        public void Explosion(int pwr, IntVec3 center, Map map, float radius, DamageDef damType, Thing instigator, SoundDef explosionSound = null, ThingDef projectile = null, ThingDef source = null, ThingDef postExplosionSpawnThingDef = null, float postExplosionSpawnChance = 0f, int postExplosionSpawnThingCount = 1, bool applyDamageToExplosionCellsNeighbors = false, ThingDef preExplosionSpawnThingDef = null, float preExplosionSpawnChance = 0f, int preExplosionSpawnThingCount = 1)
        {
            System.Random rnd = new System.Random();
            int modDamAmountRand = (pwr * 3) + GenMath.RoundRandom(rnd.Next(3, projectile.projectile.damageAmountBase));
            modDamAmountRand = Mathf.RoundToInt(this.arcaneDmg);
            if (map == null)
            {
                Log.Warning("Tried to do explosion in a null map.");
                return;
            }
            Explosion explosion = (Explosion)GenSpawn.Spawn(ThingDefOf.Explosion, center, map);
            explosion.dealMoreDamageAtCenter = false;
            explosion.chanceToStartFire = 0.0f;
            explosion.Position = center;
            explosion.radius = radius;
            explosion.damType = damType;
            explosion.instigator = instigator;
            explosion.damAmount =  ((projectile == null) ? GenMath.RoundRandom((float)damType.explosionDamage) : modDamAmountRand);
            explosion.weapon = source;
            explosion.preExplosionSpawnThingDef = preExplosionSpawnThingDef;
            explosion.preExplosionSpawnChance = preExplosionSpawnChance;
            explosion.preExplosionSpawnThingCount = preExplosionSpawnThingCount;
            explosion.postExplosionSpawnThingDef = postExplosionSpawnThingDef;
            explosion.postExplosionSpawnChance = postExplosionSpawnChance;
            explosion.postExplosionSpawnThingCount = postExplosionSpawnThingCount;
            explosion.applyDamageToExplosionCellsNeighbors = applyDamageToExplosionCellsNeighbors;
            explosion.StartExplosion(explosionSound);
        }
    }
}
