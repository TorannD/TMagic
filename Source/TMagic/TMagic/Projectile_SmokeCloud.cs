using AbilityUser;
using RimWorld;
using Verse;

namespace TorannMagic
{
	public class Projectile_SmokeCloud : Projectile_AbilityBase
	{
		protected override void Impact(Thing hitThing)
		{            
            Map map = base.Map;
			base.Impact(hitThing);
			ThingDef def = this.def;
            GenExplosion.DoExplosion(base.Position, map, this.def.projectile.explosionRadius, this.def.projectile.damageDef, this.launcher, this.def.projectile.damageAmountBase, SoundDefOf.ArtilleryShellLoaded, def, this.equipmentDef, ThingDefOf.Gas_Smoke, 1f, 1, false, null, 0f, 1, 0f, false);
            CellRect cellRect = CellRect.CenteredOn(base.Position, 6);
			cellRect.ClipInsideMap(map);
		}		
	}	
}


