using Verse;
using UnityEngine;

namespace TorannMagic
{
    public class Building_TMBarrier : Building
    {

        private bool initialized = false;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            //LessonAutoActivator.TeachOpportunity(ConceptDef.Named("TM_Portals"), OpportunityType.GoodToKnow);
        }
                
        public override void Tick()
        {
            if(!initialized)
            {
                initialized = true;
            }
            if(Find.TickManager.TicksGame % 4 == 0)
            {
                TM_MoteMaker.ThrowBarrierMote(this.DrawPos, this.Map, .7f);
            }
            base.Tick();
        }
    }
}
