using AbilityUser;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

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
            TerrainDef terrain;
            float radius = this.def.projectile.explosionRadius;
            IntVec3 curCell;
            if(map.Biome != BiomeDefOf.SeaIce)
            { 
                IEnumerable<IntVec3> cells = GenRadial.RadialCellsAround(c, radius, true);
                for (int i = 0; i < cells.Count(); i++)
                {
                    curCell = cells.ToArray<IntVec3>()[i];
                    terrain = curCell.GetTerrain(map);
                    if (curCell.InBounds(map) && curCell.IsValid && terrain.driesTo != null)
                    {
                        if (terrain.defName == "MarshyTerrain" || terrain.defName == "Mud" || terrain.defName == "Marsh")
                        {
                            map.terrainGrid.SetTerrain(curCell, terrain.driesTo);
                        }
                        else if (terrain.defName == "WaterShallow")
                        {
                            map.terrainGrid.SetTerrain(curCell, TerrainDef.Named("Marsh"));
                        }
                        else if (terrain.defName == "Ice")
                        {
                            map.terrainGrid.SetTerrain(curCell, TerrainDef.Named("Mud"));
                        }
                        else
                        {
                            //Messages.Message("TerraformFailed".Translate(), MessageTypeDefOf.RejectInput);
                        }
                    }
                }
            }
            else
            {
                Messages.Message("NotRightTerrainType".Translate(), MessageTypeDefOf.RejectInput);
            }

        }
    }
}
