using RimWorld;
using System;
using System.Linq;
using Verse;

namespace TorannMagic
{
    class SitePartWorker_EnemyRaidOnArrival : SitePartWorker
    {
        public override void PostMapGenerate(Map map)
        {
            IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(Find.Storyteller.def, (IncidentCategory)3, map);
            incidentParms.forced = true;
            IntVec3 spawnCenter;
            if (RCellFinder.TryFindRandomPawnEntryCell(out spawnCenter, map, 0f, (IntVec3 v) => GenGrid.Standable(v, map)))
            {
                incidentParms.spawnCenter = spawnCenter;
            }
            Faction faction;
            if (GenCollection.TryRandomElement<Faction>(from f in Find.FactionManager.AllFactions
                                                        where !f.def.hidden && FactionUtility.HostileTo(f, Faction.OfPlayer)
                                                        select f, out faction))
            {
                IntVec3 spawnCenter2;
                if (CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, 0f, out spawnCenter2))
                {
                    incidentParms.faction = Faction.OfMechanoids;
                    incidentParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
                    incidentParms.generateFightersOnly = true;
                    incidentParms.raidNeverFleeIndividual = true;
                    incidentParms.raidArrivalMode = PawnsArriveMode.CenterDrop;
                    incidentParms.spawnCenter = spawnCenter2;
                    incidentParms.points *= 20f;
                    incidentParms.points = Math.Max(incidentParms.points, 250f);
                    QueuedIncident queuedIncident = new QueuedIncident(new FiringIncident(TorannMagicDefOf.ArcaneEnemyRaid, null, incidentParms), Find.TickManager.TicksGame + Rand.RangeInclusive(500, 5000));
                    Find.Storyteller.incidentQueue.Add(queuedIncident);
                    System.Random random = new System.Random();
                    int rnd = GenMath.RoundRandom(random.Next(0, 10));
                    if (rnd < 5)
                    {
                        incidentParms.points = Math.Max(incidentParms.points*2, 500f);
                        queuedIncident = new QueuedIncident(new FiringIncident(TorannMagicDefOf.ArcaneEnemyRaid, null, incidentParms), Find.TickManager.TicksGame + Rand.RangeInclusive(2000, 3000));
                        Find.Storyteller.incidentQueue.Add(queuedIncident);
                    }
                    if (rnd < 3)
                    {
                        if (GenCollection.TryRandomElement<Faction>(from f in Find.FactionManager.AllFactions
                                                                    where !f.def.hidden && FactionUtility.HostileTo(f, Faction.OfPlayer)
                                                                    select f, out faction))
                        {
                            incidentParms.faction = faction;
                            incidentParms.points = Math.Max(250f, 500f);
                            queuedIncident = new QueuedIncident(new FiringIncident(TorannMagicDefOf.ArcaneEnemyRaid, null, incidentParms), Find.TickManager.TicksGame + Rand.RangeInclusive(5000, 10000));
                            Find.Storyteller.incidentQueue.Add(queuedIncident);
                        }
                    }
                }
            }
        }
    }
}
