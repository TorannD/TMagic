using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using AbilityUser;
using UnityEngine;
using Verse.AI.Group;

namespace TorannMagic
{
    public class Verb_SummonElemental : Verb_UseAbility
    {
        protected override bool TryCastShot()
        {

            CellRect cellRect = CellRect.CenteredOn(this.currentTarget.Cell, 1);
            Map map = base.CasterPawn.Map;
            cellRect.ClipInsideMap(map);

            IntVec3 centerCell = cellRect.CenterCell;
            bool result = false;
            SpawnThings spawnThing = new SpawnThings();
            spawnThing.def = TorannMagicDefOf.TM_Earth_ElementalR;
            spawnThing.factionDef = TorannMagicDefOf.TM_ElementalFaction;
            spawnThing.spawnCount = 1;
            spawnThing.kindDef = PawnKindDef.Named("TM_Earth_Elemental");
            spawnThing.temporary = false;

            SingleSpawnLoop(spawnThing, centerCell, map);
            
            return result;
        }

        public void SingleSpawnLoop(SpawnThings spawnables, IntVec3 position, Map map)
        {
            Log.Message("single spawn loop start");
            bool flag = spawnables.def != null;
            if (flag)
            {
                Faction faction = Faction.OfPlayer;
                bool flag2 = spawnables.def.race != null;
                if (flag2)
                {
                    bool flag3 = spawnables.kindDef == null;
                    if (flag3)
                    {
                        Log.Error("Missing kinddef");
                    }
                    else
                    {
                        PawnSummoned newPawn = (PawnSummoned)PawnGenerator.GeneratePawn(spawnables.kindDef, faction);
                        newPawn.Spawner = this.CasterPawn;
                        newPawn.Temporary = false;
                        if (newPawn.Faction != Faction.OfPlayerSilentFail)
                        {
                            Log.Message("Failing check for newpawn faction: " + newPawn.Faction);
                            newPawn.SetFaction(this.CasterPawn.Faction, null);
                        }
                        Log.Message("attempting to spawn " + newPawn.def.defName + " of " + newPawn.kindDef.defName + " of faction " + newPawn.Faction);
                        GenSpawn.Spawn(newPawn, position, Find.VisibleMap);
                        if (newPawn.Faction != null && newPawn.Faction != Faction.OfPlayer)
                        {
                            Lord lord = null;
                            if (newPawn.Map.mapPawns.SpawnedPawnsInFaction(faction).Any((Pawn p) => p != newPawn))
                            {
                                Predicate<Thing> validator = (Thing p) => p != newPawn && ((Pawn)p).GetLord() != null;
                                Pawn p2 = (Pawn)GenClosest.ClosestThing_Global(newPawn.Position, newPawn.Map.mapPawns.SpawnedPawnsInFaction(faction), 99999f, validator, null);
                                lord = p2.GetLord();
                            }
                            bool flag4 = lord == null;
                            if (flag4)
                            {
                                LordJob_DefendPoint lordJob = new LordJob_DefendPoint(newPawn.Position);
                                lord = LordMaker.MakeNewLord(faction, lordJob, Find.VisibleMap, null);
                            }
                            lord.AddPawn(newPawn);
                        }
                    }
                }
                else
                {
                    Log.Message("Missing race");
                }
            }
        }
    }
}
