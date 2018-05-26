using RimWorld;
using System;
using System.Linq;
using Verse;
using AbilityUser;
using UnityEngine;
using Verse.AI.Group;

namespace TorannMagic
{
    public class Projectile_SummonElemental : Projectile_Ability
    {

        private int age = -1;
        private bool initialized = false;
        private bool destroyed = false;
        private int duration = 1800;
        TMPawnSummoned newPawn = new TMPawnSummoned();
        Pawn pawn;
        private int verVal;
        private int pwrVal;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.initialized, "initialized", false, false);
            Scribe_Values.Look<int>(ref this.age, "age", -1, false);
            Scribe_Values.Look<int>(ref this.duration, "duration", 3600, false);
            Scribe_Values.Look<bool>(ref this.destroyed, "destroyed", false, false);
        }        

        public override void Tick()
        {
            base.Tick();
            this.age++;
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            bool flag = this.age < duration;
            if (!flag)
            {
                base.Destroy(mode);
            }
        }        

        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            base.Impact(hitThing);

            if (!initialized)
            {
                SpawnThings spawnThing = new SpawnThings();
                pawn = this.launcher as Pawn;
                MagicPowerSkill pwr = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_SummonElemental.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonElemental_pwr");
                MagicPowerSkill ver = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_SummonElemental.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonElemental_ver");
                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                pwrVal = pwr.level;
                verVal = ver.level;
                if (pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                {
                    MightPowerSkill mpwr = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_pwr");
                    MightPowerSkill mver = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_ver");
                    pwrVal = mpwr.level;
                    verVal = mver.level;
                }
                if (settingsRef.AIHardMode && !pawn.IsColonist)
                {
                    pwrVal = 3;
                    verVal = 3;
                }
                CellRect cellRect = CellRect.CenteredOn(this.Position, 1);
                cellRect.ClipInsideMap(map);

                IntVec3 centerCell = cellRect.CenterCell;
                System.Random random = new System.Random();
                random = new System.Random();
 
                duration += (verVal * 900);

                int rnd = GenMath.RoundRandom(random.Next(0, 8));
                if (rnd < 2)
                {
                    spawnThing.factionDef = TorannMagicDefOf.TM_ElementalFaction;
                    spawnThing.spawnCount = 1;
                    spawnThing.temporary = false;

                    if (pwrVal == 3)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            spawnThing.def = TorannMagicDefOf.TM_LesserEarth_ElementalR;
                            spawnThing.kindDef = PawnKindDef.Named("TM_LesserEarth_Elemental");
                            SingleSpawnLoop(spawnThing, centerCell, map);
                        }
                        spawnThing.def = TorannMagicDefOf.TM_GreaterEarth_ElementalR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_GreaterEarth_Elemental");
                    }
                    else if (pwrVal == 2)
                    {
                        spawnThing.def = TorannMagicDefOf.TM_GreaterEarth_ElementalR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_GreaterEarth_Elemental");
                    }
                    else if (pwrVal == 1)
                    {
                        spawnThing.def = TorannMagicDefOf.TM_Earth_ElementalR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_Earth_Elemental");
                    }
                    else
                    {
                        spawnThing.def = TorannMagicDefOf.TM_LesserEarth_ElementalR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_LesserEarth_Elemental");
                    }
                    MoteMaker.ThrowSmoke(centerCell.ToVector3(), map, pwrVal);
                    MoteMaker.ThrowMicroSparks(centerCell.ToVector3(), map);
                    SoundDefOf.AmbientAltitudeWind.sustainFadeoutTime.Equals(30.0f);
                }
                else if (rnd >= 2 && rnd < 4)
                {
                    spawnThing.factionDef = TorannMagicDefOf.TM_ElementalFaction;
                    spawnThing.spawnCount = 1;
                    spawnThing.temporary = false;

                    if (pwrVal == 3)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            spawnThing.def = TorannMagicDefOf.TM_LesserFire_ElementalR;
                            spawnThing.kindDef = PawnKindDef.Named("TM_LesserFire_Elemental");
                            SingleSpawnLoop(spawnThing, centerCell, map);
                        }
                        spawnThing.def = TorannMagicDefOf.TM_GreaterFire_ElementalR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_GreaterFire_Elemental");
                    }
                    else if (pwrVal == 2)
                    {
                        spawnThing.def = TorannMagicDefOf.TM_GreaterFire_ElementalR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_GreaterFire_Elemental");
                    }
                    else if (pwrVal == 1)
                    {
                        spawnThing.def = TorannMagicDefOf.TM_Fire_ElementalR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_Fire_Elemental");
                    }
                    else
                    {
                        spawnThing.def = TorannMagicDefOf.TM_LesserFire_ElementalR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_LesserFire_Elemental");
                    }
                    MoteMaker.ThrowSmoke(centerCell.ToVector3(), map, pwrVal);
                    MoteMaker.ThrowMicroSparks(centerCell.ToVector3(), map);
                    MoteMaker.ThrowFireGlow(centerCell, map, pwrVal);
                    MoteMaker.ThrowHeatGlow(centerCell, map, pwrVal);
                }
                else if (rnd >= 4 && rnd < 6)
                {
                    spawnThing.factionDef = TorannMagicDefOf.TM_ElementalFaction;
                    spawnThing.spawnCount = 1;
                    spawnThing.temporary = false;

                    if (pwrVal == 3)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            spawnThing.def = TorannMagicDefOf.TM_LesserWater_ElementalR;
                            spawnThing.kindDef = PawnKindDef.Named("TM_LesserWater_Elemental");
                            SingleSpawnLoop(spawnThing, centerCell, map);
                        }
                        spawnThing.def = TorannMagicDefOf.TM_GreaterWater_ElementalR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_GreaterWater_Elemental");
                    }
                    else if (pwrVal == 2)
                    {
                        spawnThing.def = TorannMagicDefOf.TM_GreaterWater_ElementalR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_GreaterWater_Elemental");
                    }
                    else if (pwrVal == 1)
                    {
                        spawnThing.def = TorannMagicDefOf.TM_Water_ElementalR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_Water_Elemental");
                    }
                    else
                    {
                        spawnThing.def = TorannMagicDefOf.TM_LesserWater_ElementalR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_LesserWater_Elemental");
                    }
                    MoteMaker.ThrowSmoke(centerCell.ToVector3(), map, pwrVal);
                    SoundDefOf.AmbientAltitudeWind.sustainFadeoutTime.Equals(30.0f);
                    MoteMaker.ThrowTornadoDustPuff(centerCell.ToVector3(), map, pwrVal, Color.blue);
                    MoteMaker.ThrowTornadoDustPuff(centerCell.ToVector3(), map, pwrVal, Color.blue);
                    MoteMaker.ThrowTornadoDustPuff(centerCell.ToVector3(), map, pwrVal, Color.blue);
                }
                else
                {
                    spawnThing.factionDef = TorannMagicDefOf.TM_ElementalFaction;
                    spawnThing.spawnCount = 1;
                    spawnThing.temporary = false;

                    if (pwrVal == 3)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            spawnThing.def = TorannMagicDefOf.TM_LesserWind_ElementalR;
                            spawnThing.kindDef = PawnKindDef.Named("TM_LesserWind_Elemental");
                            SingleSpawnLoop(spawnThing, centerCell, map);
                        }
                        spawnThing.def = TorannMagicDefOf.TM_GreaterWind_ElementalR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_GreaterWind_Elemental");
                    }
                    else if (pwrVal == 2)
                    {
                        spawnThing.def = TorannMagicDefOf.TM_GreaterWind_ElementalR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_GreaterWind_Elemental");
                    }
                    else if (pwrVal == 1)
                    {
                        spawnThing.def = TorannMagicDefOf.TM_Wind_ElementalR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_Wind_Elemental");
                    }
                    else
                    {
                        spawnThing.def = TorannMagicDefOf.TM_LesserWind_ElementalR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_LesserWind_Elemental");
                    }
                    MoteMaker.ThrowSmoke(centerCell.ToVector3(), map, 1 + pwrVal * 2);
                    SoundDefOf.AmbientAltitudeWind.sustainFadeoutTime.Equals(30.0f);
                    MoteMaker.ThrowTornadoDustPuff(centerCell.ToVector3(), map, pwrVal, Color.white);
                }

                SingleSpawnLoop(spawnThing, centerCell, map);

                this.age = this.duration;                
                this.initialized = true;
            }
        }

        public void SingleSpawnLoop(SpawnThings spawnables, IntVec3 position, Map map)
        {
            bool flag = spawnables.def != null;
            if (flag)
            {
                Faction faction = pawn.Faction;
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
                        newPawn.Spawner = this.Caster;
                        newPawn.Temporary = true;
                        newPawn.TicksToDestroy = this.duration;
                        //if (newPawn.Faction != Faction.OfPlayerSilentFail)
                        //{
                        //    newPawn.SetFaction(this.Caster.Faction, null);
                        //}
                        try
                        {
                            GenSpawn.Spawn(newPawn, position, map);
                        }
                        catch
                        {
                            pawn.GetComp<CompAbilityUserMagic>().Mana.CurLevel += pawn.GetComp<CompAbilityUserMagic>().ActualManaCost(TorannMagicDefOf.TM_SummonElemental);
                            Log.Message("TM_Exception".Translate(new object[]
                                {
                                pawn.LabelShort,
                                this.def.defName
                                }));
                            this.Destroy(DestroyMode.Vanish);
                        }
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
                                lord = LordMaker.MakeNewLord(faction, lordJob, map, null);
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