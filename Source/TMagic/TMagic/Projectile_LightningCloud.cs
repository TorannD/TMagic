using Verse;
using RimWorld;
using AbilityUser;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace TorannMagic
{
    class Projectile_LightningCloud : Projectile_AbilityBase
    {

        private IntVec3 strikeLoc = IntVec3.Invalid;

        private int age = -1;

        private bool primed = true;

        private int duration = 900;

        private int shockDelay = 0;

        private int lastStrike = 0;

        private int strikeInt = 0;

        private int radius;

        private List<Pawn> insideCloud = new List<Pawn>();

        MagicPowerSkill pwr;
        MagicPowerSkill ver;
        private int verVal;
        private int pwrVal;
        private float arcaneDmg = 1;

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            bool flag = this.age < duration;
            if (!flag)
            {
                base.Destroy(mode);
            }
        }

        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            
            base.Impact(hitThing);
            ThingDef def = this.def;
            Pawn victim = hitThing as Pawn;

            Pawn pawn = this.launcher as Pawn;
            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            pwr = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_LightningCloud.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_LightningCloud_pwr");
            ver = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_LightningCloud.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_LightningCloud_ver");
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            pwrVal = pwr.level;
            verVal = ver.level;
            this.arcaneDmg = comp.arcaneDmg;
            if (settingsRef.AIHardMode && !pawn.IsColonist)
            {
                pwrVal = 3;
                verVal = 3;
            }
            radius = (int)this.def.projectile.explosionRadius + (1 * verVal);

            CellRect cellRect = CellRect.CenteredOn(base.Position, radius - 3);
            cellRect.ClipInsideMap(map);
            IntVec3 randomCell = cellRect.RandomCell;

            duration = 900 + (verVal * 120);

            if (this.primed == true)
            {
                if (((this.shockDelay + this.lastStrike) < this.age))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        randomCell = cellRect.RandomCell;
                        if (randomCell.InBounds(map))
                        {
                            victim = randomCell.GetFirstPawn(map);
                            if (victim != null)
                            {
                                damageEntities(victim, Mathf.RoundToInt((this.def.projectile.damageAmountBase + pwrVal) * this.arcaneDmg));
                            }
                        }
                    }

                    Vector3 loc2 = base.Position.ToVector3Shifted();
                    Vector3 loc = randomCell.ToVector3Shifted();

                    bool rand1 = Rand.Range(0, 100) < 3;
                    bool rand2 = Rand.Range(0, 100) < 16;
                    if (rand1)
                    {
                        MoteMaker.ThrowSmoke(loc2, map, radius);
                        SoundDefOf.AmbientAltitudeWind.sustainFadeoutTime.Equals(30.0f);
                    }
                    if (rand2)
                    {
                        MoteMaker.ThrowSmoke(loc, map, 4f);
                    }

                    MoteMaker.ThrowMicroSparks(loc, map);
                    MoteMaker.ThrowLightningGlow(loc, map, 2f);

                    strikeInt++;
                    this.lastStrike = this.age;
                    this.shockDelay = Rand.Range(1, 3);

                    bool flag1 = this.age <= duration;
                    if (!flag1)
                    {
                        this.primed = false;
                    }
                }
            }
        }

        public void damageEntities(Pawn e, int amt)
        {
            
            DamageInfo dinfo2 = new DamageInfo(DamageDefOf.Stun, amt, (float)-1, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
            if(Rand.Chance(.35f))
            {
                amt = 0;
            }
            DamageInfo dinfo = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_LightningCloud, amt, (float)-1, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown);

            bool flag = e != null;
            if (flag)
            {
                e.TakeDamage(dinfo);
                e.TakeDamage(dinfo2);
            }

        }
        
        protected void LightningBlast(IntVec3 pos, Map map, float radius)
        {
            ThingDef def = this.def;
            Explosion(pos, map, radius, DamageDefOf.EMP, this.launcher, null, def, this.equipmentDef, ThingDefOf.Spark, 3f, 1, false, null, 0f, 1);
            Explosion(pos, map, radius, DamageDefOf.Stun, this.launcher, null, def, this.equipmentDef, ThingDefOf.Mote_Stun, 2f, 1, false, null, 0f, 1);
            Explosion(pos, map, radius, TMDamageDefOf.DamageDefOf.TM_LightningCloud, this.launcher, null, def, this.equipmentDef, ThingDefOf.Mote_Smoke, 0.4f, 1, false, null, 0f, 1);

        }

        public static void Explosion(IntVec3 center, Map map, float radius, DamageDef damType, Thing instigator, SoundDef explosionSound = null, ThingDef projectile = null, ThingDef source = null, ThingDef postExplosionSpawnThingDef = null, float postExplosionSpawnChance = 0f, int postExplosionSpawnThingCount = 1, bool applyDamageToExplosionCellsNeighbors = false, ThingDef preExplosionSpawnThingDef = null, float preExplosionSpawnChance = 0f, int preExplosionSpawnThingCount = 1)
        {
            System.Random rnd = new System.Random();
            int modDamAmountRand = GenMath.RoundRandom(rnd.Next(1, projectile.projectile.damageAmountBase));

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
            explosion.damAmount = ((projectile == null) ? GenMath.RoundRandom((float)damType.explosionDamage) : modDamAmountRand);
            explosion.weapon = source;
            explosion.preExplosionSpawnThingDef = preExplosionSpawnThingDef;
            explosion.preExplosionSpawnChance = preExplosionSpawnChance;
            explosion.preExplosionSpawnThingCount = preExplosionSpawnThingCount;
            explosion.postExplosionSpawnThingDef = postExplosionSpawnThingDef;
            explosion.postExplosionSpawnChance = postExplosionSpawnChance;
            explosion.postExplosionSpawnThingCount = postExplosionSpawnThingCount;
            explosion.applyDamageToExplosionCellsNeighbors = applyDamageToExplosionCellsNeighbors;
            //map.GetComponent<ExplosionManager>().StartExplosion(explosion, explosionSound);
            explosion.StartExplosion(explosionSound);
        }

        public override void Tick()
        {
            base.Tick();
            this.age++;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.primed, "primed", true, false);
            Scribe_Values.Look<int>(ref this.age, "age", -1, false);
            Scribe_Values.Look<int>(ref this.duration, "duration", 900, false);
            Scribe_Values.Look<int>(ref this.shockDelay, "shockDelay", 0, false);
            Scribe_Values.Look<int>(ref this.lastStrike, "lastStrike", 0, false);
            Scribe_Values.Look<int>(ref this.strikeInt, "strikeInt", 0, false);
            Scribe_Values.Look<int>(ref this.radius, "radius", 6, false);

        }
    }
}
