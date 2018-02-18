using RimWorld;
using System;
using System.Linq;
using Verse;
using AbilityUser;
using Verse.AI.Group;

namespace TorannMagic
{
    public class Projectile_SummonMinion : Projectile_Ability
    {

        private int age = -1;

        private bool initialized = false;
        private bool destroyed = false;

        private int duration = 72000;
        private int durationMultiplier = 36000;

        private int verVal;
        private int pwrVal;

        PawnSummoned[] newPawn = new PawnSummoned[3];
        private int sumNum = 0;
        CompAbilityUserMagic comp;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.initialized, "initialized", false, false);
            Scribe_Values.Look<int>(ref this.age, "age", -1, false);
            Scribe_Values.Look<int>(ref this.duration, "duration", 72000, false);
            Scribe_Values.Look<bool>(ref this.destroyed, "destroyed", false, false);
            Scribe_Values.Look<int>(ref this.sumNum, "sumNum", 0, false);
            for (int i = 0; i < sumNum; i++)
            {
                Scribe_References.Look<PawnSummoned>(ref this.newPawn[i], "newPawn" + i, false);
            }
        }

        public override void Tick()
        {
            base.Tick();
            this.age++;

            for (int i=0; i < sumNum; i++)
            {
                try
                {
                    if (newPawn[i].Downed && !newPawn[i].Destroyed && newPawn[i] != null)
                    {
                        Messages.Message("MinionFled".Translate(), MessageTypeDefOf.NeutralEvent);
                        MoteMaker.ThrowSmoke(newPawn[i].Position.ToVector3(), base.Map, 1);
                        MoteMaker.ThrowHeatGlow(newPawn[i].Position, base.Map, 1);
                        comp.summonedMinions.Remove(newPawn[i]);
                        newPawn[i].Destroy();
                    }
                }
                catch
                {
                    Log.Message("TM_ExceptionTick".Translate(new object[]
                    {
                        this.def.defName
                    }));
                    this.age = this.duration;
                }
        }
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            bool flag = this.age < duration;
            if (!flag)
            {
                try
                { 
                    for (int i = 0; i < sumNum; i++)
                    {
                        if (newPawn[i] != null && newPawn[i].Spawned && !newPawn[i].Dead)
                        {                    
                            if (!newPawn[i].Destroyed)
                            {                            
                                MoteMaker.ThrowSmoke(newPawn[i].Position.ToVector3(), base.Map, 3);
                                comp.summonedMinions.Remove(newPawn[i]);
                                newPawn[i].Destroy();
                            }
                        }
                    }
                }
                catch
                {
                    Log.Message("TM_ExceptionClose".Translate(new object[]
                    {
                            this.def.defName
                    }));
                    base.Destroy(mode);
                }
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
                Pawn pawn = this.launcher as Pawn;
                comp = pawn.GetComp<CompAbilityUserMagic>();
                MagicPowerSkill pwr = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_SummonMinion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonMinion_pwr");
                MagicPowerSkill ver = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_SummonMinion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonMinion_ver");
                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                pwrVal = pwr.level;
                verVal = ver.level;
                if (settingsRef.AIHardMode && !pawn.IsColonistPlayerControlled)
                {
                    pwrVal = 3;
                    verVal = 3;
                }
                CellRect cellRect = CellRect.CenteredOn(this.Position, 1);
                cellRect.ClipInsideMap(map);

                IntVec3 centerCell = cellRect.CenterCell;
                System.Random random = new System.Random();
                random = new System.Random();

                duration += (verVal * durationMultiplier);
                
                spawnThing.factionDef = TorannMagicDefOf.TM_SummonedFaction;
                spawnThing.spawnCount = 1;
                spawnThing.temporary = false;

                if (pwrVal >= 2)
                {
                    for (int i = 0; i < pwrVal - 1; i++)
                    {
                        spawnThing.def = TorannMagicDefOf.TM_GreaterMinionR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_GreaterMinion");
                        try
                        {
                            SingleSpawnLoop(spawnThing, centerCell, map);
                        }
                        catch
                        {
                            this.age = this.duration;
                            comp.Mana.CurLevel += comp.ActualManaCost(TorannMagicDefOf.TM_SummonMinion);
                            Log.Message("TM_Exception".Translate(new object[]
                                {
                                pawn.LabelShort,
                                this.def.defName
                                }));
                        }
                        
                    }
                    MoteMaker.ThrowSmoke(centerCell.ToVector3(), map, 2 + pwrVal);
                    MoteMaker.ThrowMicroSparks(centerCell.ToVector3(), map);
                    MoteMaker.ThrowHeatGlow(centerCell, map, 2 + pwrVal);
                }
                else
                {
                    for (int i = 0; i < pwrVal + 1; i++)
                    {
                        spawnThing.def = TorannMagicDefOf.TM_MinionR;
                        spawnThing.kindDef = PawnKindDef.Named("TM_Minion");
                        try
                        {
                            SingleSpawnLoop(spawnThing, centerCell, map);                            
                        }
                        catch
                        {
                            this.age = this.duration;
                            comp.Mana.CurLevel += comp.ActualManaCost(TorannMagicDefOf.TM_SummonMinion);
                            Log.Message("TM_Exception".Translate(new object[]
                                {
                                pawn.LabelShort,
                                this.def.defName
                                }));
                        }
                    }
                    MoteMaker.ThrowSmoke(centerCell.ToVector3(), map, 1 + pwrVal);
                    MoteMaker.ThrowMicroSparks(centerCell.ToVector3(), map);
                    MoteMaker.ThrowHeatGlow(centerCell, map, 1 + pwrVal);
                }

                SoundDefOf.AmbientAltitudeWind.sustainFadeoutTime.Equals(30.0f);

                this.initialized = true;
            }
        }

        public void SingleSpawnLoop(SpawnThings spawnables, IntVec3 position, Map map)
        {
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
                        newPawn[sumNum] = (PawnSummoned)PawnGenerator.GeneratePawn(spawnables.kindDef, faction);
                        newPawn[sumNum].Spawner = this.Caster;
                        newPawn[sumNum].Temporary = false;

                        if (newPawn[sumNum].Faction != Faction.OfPlayerSilentFail)
                        {
                            newPawn[sumNum].SetFaction(this.Caster.Faction, null);
                        }
                        newPawn[sumNum].playerSettings.master = this.Caster;
                        GenSpawn.Spawn(newPawn[sumNum], position, Find.VisibleMap);
                        if(comp.summonedMinions.Count >= 4)
                        {
                            Thing dismissMinion = comp.summonedMinions[0];
                            MoteMaker.ThrowSmoke(dismissMinion.Position.ToVector3(), base.Map, 1);
                            MoteMaker.ThrowHeatGlow(dismissMinion.Position, base.Map, 1);
                            comp.summonedMinions.Remove(dismissMinion);
                            if (!dismissMinion.Destroyed)
                            {
                                dismissMinion.Destroy();
                                Messages.Message("TM_SummonedCreatureLimitExceeded".Translate(new object[]
                                {
                                    this.launcher.LabelShort
                                }), MessageTypeDefOf.NeutralEvent);
                            }
                            
                        }
                        comp.summonedMinions.Add(newPawn[sumNum]);
                        if (newPawn[sumNum].Faction != null && newPawn[sumNum].Faction != Faction.OfPlayer)
                        {
                            Lord lord = null;
                            if (newPawn[sumNum].Map.mapPawns.SpawnedPawnsInFaction(faction).Any((Pawn p) => p != newPawn[sumNum]))
                            {
                                Predicate<Thing> validator = (Thing p) => p != newPawn[sumNum] && ((Pawn)p).GetLord() != null;
                                Pawn p2 = (Pawn)GenClosest.ClosestThing_Global(newPawn[sumNum].Position, newPawn[sumNum].Map.mapPawns.SpawnedPawnsInFaction(faction), 99999f, validator, null);
                                lord = p2.GetLord();
                            }
                            bool flag4 = lord == null;
                            if (flag4)
                            {
                                LordJob_DefendPoint lordJob = new LordJob_DefendPoint(newPawn[sumNum].Position);
                                lord = LordMaker.MakeNewLord(faction, lordJob, Find.VisibleMap, null);
                            }
                            lord.AddPawn(newPawn[sumNum]);
                        }
                        sumNum++;
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
