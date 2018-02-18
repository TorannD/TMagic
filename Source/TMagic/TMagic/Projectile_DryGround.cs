using AbilityUser;
using RimWorld;
using Verse;

namespace TorannMagic
{
    public class Projectile_DryGround : Projectile_AbilityBase
    {
        protected override void Impact(Thing hitThing)
        {

            Map map = base.Map;
            base.Impact(hitThing);
            ThingDef def = this.def;
            CellRect cellRect = CellRect.CenteredOn(base.Position, 1);
            cellRect.ClipInsideMap(map);

            IntVec3 c = cellRect.CenterCell;
            TerrainDef terrain = c.GetTerrain(map);
            if (terrain.driesTo != null && map.Biome != BiomeDefOf.SeaIce)
            {
                if (terrain.defName == "MarshyTerrain" || terrain.defName == "Mud" || terrain.defName == "Marsh")
                {
                    map.terrainGrid.SetTerrain(c, terrain.driesTo);
                }
                else if( terrain.defName == "WaterShallow")
                {
                    map.terrainGrid.SetTerrain(c, TerrainDef.Named("Marsh"));
                }
                else if( terrain.defName == "Ice")
                {
                    map.terrainGrid.SetTerrain(c, TerrainDef.Named("Mud"));
                }
                else
                {
                    Messages.Message("TerraformFailed".Translate(), MessageTypeDefOf.RejectInput);
                }
            }
            else
            {
                Messages.Message("NotRightTerrainType".Translate(), MessageTypeDefOf.RejectInput);
            }

        }
    }
}
