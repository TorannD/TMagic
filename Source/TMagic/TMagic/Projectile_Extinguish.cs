using AbilityUser;
using RimWorld;
using Verse;

namespace TorannMagic
{
	public class Projectile_Extinguish : Projectile_AbilityBase
	{
        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            base.Impact(hitThing);
            ThingDef def = this.def;
            GenExplosion.DoExplosion(base.Position, map, this.def.projectile.explosionRadius, this.def.projectile.damageDef, this.launcher, this.def.projectile.GetDamageAmount(1,null), 0, SoundDefOf.Artillery_ShellLoaded, def, this.equipmentDef, null, null, 0f, 1, false, null, 0f, 1, 0f, false);

        }
		
	}	
}


