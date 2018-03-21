using System.Collections.Generic;
using Verse.AI;
using RimWorld;
using Verse;
using AbilityUser;
using System.Linq;


namespace TorannMagic
{
    internal class JobDriver_PlacePoisonTrap: JobDriver
    {

        public override bool TryMakePreToilReservations()
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {

            Toil gotoSpot = new Toil()
            {
                initAction = () =>
                {
                    pawn.pather.StartPath(TargetLocA, PathEndMode.Touch);
                },
                defaultCompleteMode = ToilCompleteMode.PatherArrival
            };
            yield return gotoSpot;

            Toil placeTrap = new Toil()
            {
                initAction = () =>
                {
                    SpawnThings tempPod = new SpawnThings();
                    tempPod.def = ThingDef.Named("TM_PoisonTrap");
                    try
                    {
                        CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();
                        MightPowerSkill ver = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_PoisonTrap.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PoisonTrap_ver");
                        for(int i = 0; i < comp.combatItems.Count; i++)
                        {
                            if(comp.combatItems[i].Destroyed)
                            {
                                comp.combatItems.Remove(comp.combatItems[i]);
                                i--;
                            }                            
                        }
                        if (comp.combatItems.Count > ver.level+1)
                        {
                            Messages.Message("TM_TooManyTraps".Translate(new object[]
                            {
                                pawn.LabelShort,
                                ver.level + 2
                            }), MessageTypeDefOf.NeutralEvent);
                            Thing tempThing = comp.combatItems[0];
                            comp.combatItems.Remove(tempThing);
                            if (tempThing != null && !tempThing.Destroyed)
                            {
                                tempThing.Destroy();
                            }
                        }
                        this.SingleSpawnLoop(tempPod, pawn, TargetLocA, pawn.Map);                                              
                    }
                    catch
                    {
                        Log.Message("Attempted to place a poison trap but threw an unknown exception - recovering and ending attempt");
                        return;
                    }
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
            yield return placeTrap;         
        }

        public void SingleSpawnLoop(SpawnThings spawnables, Pawn pawn, IntVec3 position, Map map)
        {
            bool flag = spawnables.def != null;
            if (flag)
            {
                Faction faction = pawn.Faction;
                bool flag2 = spawnables.def.race != null;
                if (flag2)
                {
                    Log.Error("Trying to spawn a pawn instead of a building.");
                }
                else
                {
                    ThingDef def = spawnables.def;
                    ThingDef stuff = null;
                    bool madeFromStuff = def.MadeFromStuff;
                    if (madeFromStuff)
                    {
                        stuff = ThingDefOf.WoodLog;
                    }
                    Thing thing = ThingMaker.MakeThing(def, stuff);
                    thing.SetFaction(faction, null);
                    CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();
                    GenSpawn.Spawn(thing, position, map, Rot4.North, false);
                    comp.combatItems.Add(thing);
                }
            }
        }
    }
}