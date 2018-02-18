using AbilityUser;
using RimWorld;
using Verse;

namespace TorannMagic
{
    public class Projectile_ChargeBattery : Projectile_AbilityBase
    {
        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            base.Impact(hitThing);
            ThingDef def = this.def;
            CellRect cellRect = CellRect.CenteredOn(base.Position, 1);
            cellRect.ClipInsideMap(map);
            Building bldg = new Building();

            IntVec3 c = cellRect.CenterCell;

            bldg = cellRect.CenterCell.GetFirstBuilding(map);
            if (bldg != null)
            {
                
                if (bldg.GetComp<CompPowerBattery>() != null)
                {
                    bldg.GetComp<CompPowerBattery>().AddEnergy(400f);
                    if (400f > bldg.GetComp<CompPowerBattery>().AmountCanAccept)
                    {
                        bldg.GetComp<CompPowerBattery>().AddEnergy(bldg.GetComp<CompPowerBattery>().AmountCanAccept);
                    }
                    else
                    {
                        bldg.GetComp<CompPowerBattery>().AddEnergy(400f);
                    }
                }
                else
                {
                    Messages.Message("InvalidBattery".Translate(), MessageTypeDefOf.NegativeEvent);
                }
                    
            }
            else
            {
                Messages.Message("NotABuilding".Translate(), MessageTypeDefOf.NegativeEvent);
            }            

        }
    }
}
