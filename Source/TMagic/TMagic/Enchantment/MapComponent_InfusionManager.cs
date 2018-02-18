using System;
using Verse;

namespace TorannMagic.Enchantment
{
    public class MapComponent_InfusionManager : MapComponent
    {
        public MapComponent_InfusionManager(Map map) : base(map)
        {
            InfusionLabelManager.ReInit();
        }

        public override void MapComponentOnGUI()
        {
            if (Find.CameraDriver.CurrentZoom != null)
            {
                return;
            }
            if (InfusionLabelManager.Drawee.Count == 0)
            {
                return;
            }
            foreach (CompInfusion current in InfusionLabelManager.Drawee)
            {
                if (current.parent.Map == this.map)
                {
                    if (!this.map.fogGrid.IsFogged(current.parent.Position))
                    {
                        GenMapUI.DrawThingLabel(GenMapUI.LabelDrawPosFor(current.parent, -0.66f), current.InfusedLabel, current.InfusedLabelColor);
                    }
                }
            }
        }
    }
}
