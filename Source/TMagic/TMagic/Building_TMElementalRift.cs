using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using System.Diagnostics;
using UnityEngine;
using RimWorld;
using AbilityUser;



namespace TorannMagic
{
    [StaticConstructorOnStartup]
    public class Building_TMElementalRift : Building
    {

        private float arcaneEnergyCur = 0;
        private float arcaneEnergyMax = 1;

        private static readonly Material portalMat_1 = MaterialPool.MatFrom("Motes/rift_swirl1", false);
        private static readonly Material portalMat_2 = MaterialPool.MatFrom("Motes/rift_swirl2", false);
        private static readonly Material portalMat_3 = MaterialPool.MatFrom("Motes/rift_swirl3", false);

        private int matRng = 0;
        private float matMagnitude = 0;

        private bool initialized = false;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<float>(ref this.arcaneEnergyCur, "arcaneEnergyCur", 0f, false);
            Scribe_Values.Look<float>(ref this.arcaneEnergyMax, "arcaneEnergyMax", 0f, false);
        }
        
        public float ArcaneEnergyCur
        {
            get
            {
                return arcaneEnergyCur;
            }
            set
            {
                arcaneEnergyCur = value;
            }
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            //LessonAutoActivator.TeachOpportunity(ConceptDef.Named("TM_Portals"), OpportunityType.GoodToKnow);
        }
                
        public override void Tick()
        {
            if(!initialized)
            {
                SpawnCycle();
                initialized = true;
            }
            if(Find.TickManager.TicksGame % 8 == 0)
            {
                this.matRng = Rand.RangeInclusive(0, 2);
                this.matMagnitude = 4 * this.arcaneEnergyMax;
            }
            if (Find.TickManager.TicksGame % 2000 == 0)
            {
                SpawnCycle();                
            }
        }

        public void SpawnCycle()
        {
            System.Random random = new System.Random();
            random = new System.Random();
            int rnd = GenMath.RoundRandom(random.Next(0, 8));
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            float geChance = 0.007f;
            if (settingsRef.riftChallenge > 1 )
            {
                 geChance *= settingsRef.riftChallenge;
            }  
            else
            {
                geChance = 0;
            }
            float eChance = 0.035f * settingsRef.riftChallenge;
            float leChance = 0.12f * settingsRef.riftChallenge;

            IntVec3 curCell;
            IEnumerable<IntVec3> targets = GenRadial.RadialCellsAround(this.Position, 2, true);
            for (int j = 0; j < targets.Count(); j++)
            {
                curCell = targets.ToArray<IntVec3>()[j];
                if (curCell.InBounds(this.Map) && curCell.IsValid && curCell.Walkable(this.Map))
                {
                    SpawnThings rogueElemental = new SpawnThings();
                    if (rnd < 2)
                    {
                        if (Rand.Chance(geChance))
                        {
                            MoteMaker.ThrowSmoke(curCell.ToVector3(), this.Map, 1f);
                            MoteMaker.ThrowMicroSparks(curCell.ToVector3(), this.Map);
                            SoundDefOf.AmbientAltitudeWind.sustainFadeoutTime.Equals(30.0f);
                            rogueElemental.def = TorannMagicDefOf.TM_GreaterEarth_ElementalR;
                            rogueElemental.kindDef = PawnKindDef.Named("TM_GreaterEarth_Elemental");
                        }
                        else if (Rand.Chance(eChance))
                        {
                            MoteMaker.ThrowSmoke(curCell.ToVector3(), this.Map, 1f);
                            MoteMaker.ThrowMicroSparks(curCell.ToVector3(), this.Map);
                            SoundDefOf.AmbientAltitudeWind.sustainFadeoutTime.Equals(30.0f);
                            rogueElemental.def = TorannMagicDefOf.TM_Earth_ElementalR;
                            rogueElemental.kindDef = PawnKindDef.Named("TM_Earth_Elemental");
                        }
                        else if (Rand.Chance(leChance))
                        {
                            MoteMaker.ThrowSmoke(curCell.ToVector3(), this.Map, 1f);
                            MoteMaker.ThrowMicroSparks(curCell.ToVector3(), this.Map);
                            SoundDefOf.AmbientAltitudeWind.sustainFadeoutTime.Equals(30.0f);
                            rogueElemental.def = TorannMagicDefOf.TM_LesserEarth_ElementalR;
                            rogueElemental.kindDef = PawnKindDef.Named("TM_LesserEarth_Elemental");
                        }
                        else
                        {
                            rogueElemental = null;
                        }
                    }
                    else if (rnd >= 2 && rnd < 4)
                    {

                        if (Rand.Chance(geChance))
                        {
                            MoteMaker.ThrowSmoke(curCell.ToVector3(), this.Map, 1);
                            MoteMaker.ThrowMicroSparks(curCell.ToVector3(), this.Map);
                            MoteMaker.ThrowFireGlow(curCell, this.Map, 1);
                            MoteMaker.ThrowHeatGlow(curCell, this.Map, 1);
                            rogueElemental.def = TorannMagicDefOf.TM_GreaterFire_ElementalR;
                            rogueElemental.kindDef = PawnKindDef.Named("TM_GreaterFire_Elemental");
                        }
                        else if (Rand.Chance(eChance))
                        {
                            MoteMaker.ThrowSmoke(curCell.ToVector3(), this.Map, 1);
                            MoteMaker.ThrowMicroSparks(curCell.ToVector3(), this.Map);
                            MoteMaker.ThrowFireGlow(curCell, this.Map, 1);
                            MoteMaker.ThrowHeatGlow(curCell, this.Map, 1);
                            rogueElemental.def = TorannMagicDefOf.TM_Fire_ElementalR;
                            rogueElemental.kindDef = PawnKindDef.Named("TM_Fire_Elemental");
                        }
                        else if (Rand.Chance(leChance))
                        {
                            MoteMaker.ThrowSmoke(curCell.ToVector3(), this.Map, 1);
                            MoteMaker.ThrowMicroSparks(curCell.ToVector3(), this.Map);
                            MoteMaker.ThrowFireGlow(curCell, this.Map, 1);
                            MoteMaker.ThrowHeatGlow(curCell, this.Map, 1);
                            rogueElemental.def = TorannMagicDefOf.TM_LesserFire_ElementalR;
                            rogueElemental.kindDef = PawnKindDef.Named("TM_LesserFire_Elemental");
                        }
                        else
                        {
                            rogueElemental = null;
                        }

                    }
                    else if (rnd >= 4 && rnd < 6)
                    {

                        if (Rand.Chance(geChance))
                        {
                            MoteMaker.ThrowSmoke(curCell.ToVector3(), this.Map, 1);
                            SoundDefOf.AmbientAltitudeWind.sustainFadeoutTime.Equals(30.0f);
                            MoteMaker.ThrowTornadoDustPuff(curCell.ToVector3(), this.Map, 1, Color.blue);
                            MoteMaker.ThrowTornadoDustPuff(curCell.ToVector3(), this.Map, 1, Color.blue);
                            MoteMaker.ThrowTornadoDustPuff(curCell.ToVector3(), this.Map, 1, Color.blue);
                            rogueElemental.def = TorannMagicDefOf.TM_GreaterWater_ElementalR;
                            rogueElemental.kindDef = PawnKindDef.Named("TM_GreaterWater_Elemental");
                        }
                        else if (Rand.Chance(eChance))
                        {
                            MoteMaker.ThrowSmoke(curCell.ToVector3(), this.Map, 1);
                            SoundDefOf.AmbientAltitudeWind.sustainFadeoutTime.Equals(30.0f);
                            MoteMaker.ThrowTornadoDustPuff(curCell.ToVector3(), this.Map, 1, Color.blue);
                            MoteMaker.ThrowTornadoDustPuff(curCell.ToVector3(), this.Map, 1, Color.blue);
                            MoteMaker.ThrowTornadoDustPuff(curCell.ToVector3(), this.Map, 1, Color.blue);
                            rogueElemental.def = TorannMagicDefOf.TM_Water_ElementalR;
                            rogueElemental.kindDef = PawnKindDef.Named("TM_Water_Elemental");
                        }
                        else if (Rand.Chance(leChance))
                        {
                            MoteMaker.ThrowSmoke(curCell.ToVector3(), this.Map, 1);
                            SoundDefOf.AmbientAltitudeWind.sustainFadeoutTime.Equals(30.0f);
                            MoteMaker.ThrowTornadoDustPuff(curCell.ToVector3(), this.Map, 1, Color.blue);
                            MoteMaker.ThrowTornadoDustPuff(curCell.ToVector3(), this.Map, 1, Color.blue);
                            MoteMaker.ThrowTornadoDustPuff(curCell.ToVector3(), this.Map, 1, Color.blue);
                            rogueElemental.def = TorannMagicDefOf.TM_LesserWater_ElementalR;
                            rogueElemental.kindDef = PawnKindDef.Named("TM_LesserWater_Elemental");
                        }
                        else
                        {
                            rogueElemental = null;
                        }

                    }
                    else
                    {

                        if (Rand.Chance(geChance))
                        {
                            rogueElemental.def = TorannMagicDefOf.TM_GreaterWind_ElementalR;
                            rogueElemental.kindDef = PawnKindDef.Named("TM_GreaterWind_Elemental");
                            MoteMaker.ThrowSmoke(curCell.ToVector3(), this.Map, 1 + 1 * 2);
                            SoundDefOf.AmbientAltitudeWind.sustainFadeoutTime.Equals(30.0f);
                            MoteMaker.ThrowTornadoDustPuff(curCell.ToVector3(), this.Map, 1, Color.white);
                        }
                        else if (Rand.Chance(eChance))
                        {
                            rogueElemental.def = TorannMagicDefOf.TM_Wind_ElementalR;
                            rogueElemental.kindDef = PawnKindDef.Named("TM_Wind_Elemental");
                            MoteMaker.ThrowSmoke(curCell.ToVector3(), this.Map, 1 + 1 * 2);
                            SoundDefOf.AmbientAltitudeWind.sustainFadeoutTime.Equals(30.0f);
                            MoteMaker.ThrowTornadoDustPuff(curCell.ToVector3(), this.Map, 1, Color.white);
                        }
                        else if (Rand.Chance(leChance))
                        {
                            rogueElemental.def = TorannMagicDefOf.TM_LesserWind_ElementalR;
                            rogueElemental.kindDef = PawnKindDef.Named("TM_LesserWind_Elemental");
                            MoteMaker.ThrowSmoke(curCell.ToVector3(), this.Map, 1 + 1 * 2);
                            SoundDefOf.AmbientAltitudeWind.sustainFadeoutTime.Equals(30.0f);
                            MoteMaker.ThrowTornadoDustPuff(curCell.ToVector3(), this.Map, 1, Color.white);
                        }
                        else
                        {
                            rogueElemental = null;
                        }

                    }
                    if (rogueElemental != null)
                    {
                        SingleSpawnLoop(rogueElemental, curCell, this.Map);
                    }
                }
            }
        }


        public void SingleSpawnLoop(SpawnThings spawnables, IntVec3 position, Map map)
        {
            bool flag = spawnables.def != null;
            if (flag)
            {
                Faction faction = Find.FactionManager.FirstFactionOfDef(FactionDef.Named("TM_ElementalFaction"));
                TMPawnSummoned newPawn = new TMPawnSummoned();
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
                        newPawn = (TMPawnSummoned)PawnGenerator.GeneratePawn(spawnables.kindDef, faction);
                        //newPawn.Spawner = this.Caster;
                        newPawn.Temporary = false;
                        if (newPawn.Faction == null || !newPawn.Faction.HostileTo(Faction.OfPlayer))
                        {
                            Log.Message("elemental faction was null or not hostile");
                            newPawn.SetFaction(Faction.OfMechanoids, null);
                        }
                        GenSpawn.Spawn(newPawn, position, this.Map);
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
                                LordJob_AssaultColony lordJob = new LordJob_AssaultColony(newPawn.Faction, false, false, false, true, false);
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

        public override void Draw()
        {
            base.Draw();

            Vector3 vector = base.DrawPos;
            vector.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
            Vector3 s = new Vector3(matMagnitude, matMagnitude, matMagnitude);
            Matrix4x4 matrix = default(Matrix4x4);
            float angle = 0f;
            matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
            if (matRng == 0)
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, Building_TMElementalRift.portalMat_1, 0);
            }
            else if (matRng == 1)
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, Building_TMElementalRift.portalMat_2, 0);
            }
            else 
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, Building_TMElementalRift.portalMat_3, 0);
            }            
        }
    }
}
