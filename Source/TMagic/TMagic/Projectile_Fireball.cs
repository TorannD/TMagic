using AbilityUser;
using RimWorld;
using Verse;
using System.Linq;
using UnityEngine;

namespace TorannMagic
{
	public class Projectile_Fireball : Projectile_AbilityBase
	{
        private int verVal;
        private int pwrVal;
        private float arcaneDmg = 1;

		protected override void Impact(Thing hitThing)
		{
            
            Map map = base.Map;
			base.Impact(hitThing);
			ThingDef def = this.def;
            //GenExplosion.DoExplosion(base.Position, map, this.def.projectile.explosionRadius, DamageDefOf.Bomb, this.launcher, SoundDefOf.PlanetkillerImpact, def, this.equipmentDef, null, 0f, 1, false, null, 0f, 1);
            GenExplosion.DoExplosion(base.Position, map, this.def.projectile.explosionRadius, DamageDefOf.Bomb, this.launcher, Mathf.RoundToInt(Rand.Range(this.def.projectile.damageAmountBase/2, this.def.projectile.damageAmountBase) * this.arcaneDmg), SoundDefOf.PlanetkillerImpact, def, this.equipmentDef, null, 0f, 1, false, null, 0f, 1, 0.1f, true);
            CellRect cellRect = CellRect.CenteredOn(base.Position, 5);
			cellRect.ClipInsideMap(map);
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            Pawn pawn = this.launcher as Pawn;
            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            MagicPowerSkill pwr = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Fireball.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Fireball_pwr");
            MagicPowerSkill ver = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Fireball.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Fireball_ver");
            pwrVal = pwr.level;
            verVal = ver.level;
            this.arcaneDmg = comp.arcaneDmg;
            if(settingsRef.AIHardMode && !pawn.IsColonist)
            {
                pwrVal = 3;
                verVal = 3;
            }
            for (int i = 0; i < (pwrVal * 3); i++)
			{
				IntVec3 randomCell = cellRect.RandomCell;
                if(randomCell.IsValid && randomCell.InBounds(map) && !randomCell.Fogged(map))
                {
                    this.FireExplosion(randomCell, map, 2.2f, ver);
                }
                else
                {
                    i--;
                }
				
			}
		}

		protected void FireExplosion(IntVec3 pos, Map map, float radius, MagicPowerSkill ver)
		{
            ThingDef def = this.def;
            if (verVal == 0)
            {
                Explosion(pos, map, radius, DamageDefOf.Flame, this.launcher, null, def, this.equipmentDef, ThingDefOf.Mote_ExplosionFlash, 0.3f, 1, false, null, 0f, 1);
            }
            else if (verVal == 1)
            {
                Explosion(pos, map, radius, TMDamageDefOf.DamageDefOf.TM_Fireball_I, this.launcher, null, def, this.equipmentDef, ThingDefOf.Mote_ExplosionFlash, 0.5f, 1, false, null, 0f, 1);
            }
            else if (verVal == 2)
            {
                Explosion(pos, map, radius, TMDamageDefOf.DamageDefOf.TM_Fireball_II, this.launcher, null, def, this.equipmentDef, ThingDefOf.Mote_ExplosionFlash, 0.8f, 1, false, null, 0f, 1);
            }
            else if (verVal == 3)
            {
                Explosion(pos, map, radius, TMDamageDefOf.DamageDefOf.TM_Fireball_III, this.launcher, null, def, this.equipmentDef, ThingDefOf.Mote_ExplosionFlash, 1.1f, 1, false, null, 0f, 1);
            }
            else
            {
                Log.Message("Fireball Versatility level not recognized: " + verVal);
            }
        }

		public void Explosion(IntVec3 center, Map map, float radius, DamageDef damType, Thing instigator, SoundDef explosionSound = null, ThingDef projectile = null, ThingDef source = null, ThingDef postExplosionSpawnThingDef = null, float postExplosionSpawnChance = 0f, int postExplosionSpawnThingCount = 1, bool applyDamageToExplosionCellsNeighbors = false, ThingDef preExplosionSpawnThingDef = null, float preExplosionSpawnChance = 0f, int preExplosionSpawnThingCount = 1)
		{
            
            System.Random rnd = new System.Random();
			int modDamAmountRand = (int)GenMath.RoundRandom(rnd.Next(3, projectile.projectile.damageAmountBase / 2));
            modDamAmountRand *= Mathf.RoundToInt(this.arcaneDmg);
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
			explosion.damAmount = ((projectile == null) ? GenMath.RoundRandom((float)damType.explosionDamage) : modDamAmountRand);
			explosion.weapon = source;
			explosion.preExplosionSpawnThingDef = preExplosionSpawnThingDef;
			explosion.preExplosionSpawnChance = preExplosionSpawnChance;
			explosion.preExplosionSpawnThingCount = preExplosionSpawnThingCount;
			explosion.postExplosionSpawnThingDef = postExplosionSpawnThingDef;
			explosion.postExplosionSpawnChance = postExplosionSpawnChance;
			explosion.postExplosionSpawnThingCount = postExplosionSpawnThingCount;
			explosion.applyDamageToExplosionCellsNeighbors = applyDamageToExplosionCellsNeighbors;
            explosion.dealMoreDamageAtCenter = true;
            explosion.chanceToStartFire = 0.05f;
            explosion.StartExplosion(explosionSound);
            
		}
		
	}	
}


