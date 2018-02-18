using AbilityUser;
using RimWorld;
using Verse;

namespace TorannMagic
{
    public class Projectile_WetGround : Projectile_AbilityBase
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

            if (terrain.defName == "Sand" || terrain.defName == "Gravel")
            {
                map.terrainGrid.SetTerrain(c, TerrainDef.Named("Soil"));
            }
            else
            {
                Messages.Message("TerraformNotSandOrGravel".Translate(), MessageTypeDefOf.RejectInput);
            }


        }
    }
}
