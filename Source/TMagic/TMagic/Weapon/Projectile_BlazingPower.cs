using System;
using System.Collections.Generic;
using Verse;
using RimWorld;
using AbilityUser;
using System.Linq;
using UnityEngine;

namespace TorannMagic.Weapon
{
    public class Projectile_BlazingPower : Projectile_AbilityBase
    {
        private float arcaneDmg = 1;

        protected override void Impact(Thing hitThing)
        {
            Pawn pawn = this.launcher as Pawn;
            Map map = base.Map;
            base.Impact(hitThing);
            ThingDef def = this.def;
            if (pawn != null)
            {
                CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
                if (comp.IsMagicUser)
                {
                    this.arcaneDmg = comp.arcaneDmg;
                }
                try
                {
                    //TM_MoteMaker.MakePowerBeamMotePsionic(base.Position, map, 12f, 2f, .7f, .1f, .6f);
                    //List<Thing> thingList = base.Position.GetThingList(map);
                    //for(int i = 0; i < thingList.Count; i++)
                    //{
                    //    DamageEntities(thingList[i], null, this.def.projectile.GetDamageAmount(1, null), TMDamageDefOf.DamageDefOf.TM_BlazingPower, pawn);
                    //}
                    
                    GenExplosion.DoExplosion(base.Position, map, this.def.projectile.explosionRadius, TMDamageDefOf.DamageDefOf.TM_BlazingPower, this.launcher, Mathf.RoundToInt(this.def.projectile.GetDamageAmount(1, null) * this.arcaneDmg), 2, SoundDefOf.Crunch, def, this.equipmentDef, null, null, 0f, 1, false, null, 0f, 1, 0.0f, true);
                }
                catch
                {
                    //don't care
                }
            }
        }

        protected void FireExplosion(IntVec3 pos, Map map, float radius)
        {
            ThingDef def = this.def;
            Explosion(pos, map, radius, DamageDefOf.Flame, this.launcher, null, def, this.equipmentDef, ThingDefOf.Mote_ExplosionFlash, 0.6f, 1, false, null, 0f, 1);            
        }

        public static void Explosion(IntVec3 center, Map map, float radius, DamageDef damType, Thing instigator, SoundDef explosionSound = null, ThingDef projectile = null, ThingDef source = null, ThingDef postExplosionSpawnThingDef = null, float postExplosionSpawnChance = 0f, int postExplosionSpawnThingCount = 1, bool applyDamageToExplosionCellsNeighbors = false, ThingDef preExplosionSpawnThingDef = null, float preExplosionSpawnChance = 0f, int preExplosionSpawnThingCount = 1)
        {
            System.Random rnd = new System.Random();
            int modDamAmountRand = (int)GenMath.RoundRandom(rnd.Next(3, projectile.projectile.GetDamageAmount(1,null)/2));
            if (map == null)
            {
                Log.Warning("Tried to do explosion in a null map.");
                return;
            }
            Explosion explosion = (Explosion)GenSpawn.Spawn(ThingDefOf.Explosion, center, map);
            explosion.Position = center;
            explosion.radius = radius;
            explosion.damType = damType;
            explosion.instigator = instigator;
            explosion.damAmount = ((projectile == null) ? GenMath.RoundRandom((float)damType.defaultDamage) : modDamAmountRand);
            explosion.weapon = source;
            explosion.preExplosionSpawnThingDef = preExplosionSpawnThingDef;
            explosion.preExplosionSpawnChance = preExplosionSpawnChance;
            explosion.preExplosionSpawnThingCount = preExplosionSpawnThingCount;
            explosion.postExplosionSpawnThingDef = postExplosionSpawnThingDef;
            explosion.postExplosionSpawnChance = postExplosionSpawnChance;
            explosion.postExplosionSpawnThingCount = postExplosionSpawnThingCount;
            explosion.applyDamageToExplosionCellsNeighbors = applyDamageToExplosionCellsNeighbors;
            explosion.damageFalloff = true;
            explosion.chanceToStartFire = 0.05f;
            explosion.StartExplosion(explosionSound);

        }

        public void DamageEntities(Thing victim, BodyPartRecord hitPart, int amt, DamageDef type, Pawn instigator)
        {
            amt = (int)((float)amt * Rand.Range(.75f, 1.25f));
            DamageInfo dinfo = new DamageInfo(type, amt, 0, (float)-1, instigator, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown);
            dinfo.SetAllowDamagePropagation(false);
            victim.TakeDamage(dinfo);
        }
    }
}
