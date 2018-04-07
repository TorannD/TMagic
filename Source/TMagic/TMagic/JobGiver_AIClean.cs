using System;
using Verse;
using Verse.AI;
using RimWorld;
using System.Collections.Generic;

namespace TorannMagic
{
    public class JobGiver_AIClean : ThinkNode_JobGiver //Code by Mehni from Penguin Mod
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            Predicate<Thing> filth = f => f.def.category == ThingCategory.Filth;

            //Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Filth), PathEndMode.ClosestTouch,
            //    TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 300f, filth, null, 0, -1, false, RegionType.Set_Passable, false);

            List<Thing> filthList = pawn.Map.listerFilthInHomeArea.FilthInHomeArea;
            for(int i = 0; i < filthList.Count; i++)
            {
                if(pawn.CanReserve(filthList[i], 1, -1, ReservationLayerDefOf.Floor, false))
                {
                    Thing thing = filthList[i];
                    if (thing != null && pawn.CanReserve(thing))
                    {
                        Job job = new Job(JobDefOf.Clean);
                        job.AddQueuedTarget(TargetIndex.A, thing);
                        return job;
                    }
                }
            }
            return null;
        }
    }
}
