using Verse;

namespace TorannMagic
{
    class PlaceWorker_ShowPortalRadius : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot)
        {
            Map visibleMap = Find.VisibleMap;
            GenDraw.DrawFieldEdges(Building_TMPortal.PortableCellsAround(center, visibleMap));
        }
    }
}
