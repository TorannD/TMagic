using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using AbilityUser;
using Verse;
using Verse.AI;
using UnityEngine;
using Verse.Sound;


namespace TorannMagic
{   
    [StaticConstructorOnStartup]
    public class Building_60mmMortar : Building
    {
        int mortarMaxRange = 70;
        int mortarMinRange = 20;
        int mortarTicksToFire = 60;
        int mortarCount = 3;
        float mortarAccuracy = 2f;
        ThingDef projectileDef = TorannMagicDefOf.FlyingObject_60mmMortar;

        private int verVal = 0;
        private int pwrVal = 0;
        private int effVal = 0;

        protected CompMannable mannableComp;

        private bool MannedByColonist => mannableComp != null && mannableComp.ManningPawn != null && mannableComp.ManningPawn.Faction == Faction.OfPlayer;
        private bool MannedByNonColonist => mannableComp != null && mannableComp.ManningPawn != null && mannableComp.ManningPawn.Faction != Faction.OfPlayer;
        private bool PlayerControlled => (base.Faction == Faction.OfPlayer || MannedByColonist) && !MannedByNonColonist;
        private bool Manned => MannedByColonist || MannedByNonColonist;
        private bool initialized = false;

        CompAbilityUserMight comp;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.verVal, "verVal", 0, false);
            Scribe_Values.Look<int>(ref this.pwrVal, "pwrVal", 0, false);
            Scribe_Values.Look<int>(ref this.effVal, "effVal", 0, false);
            Scribe_Values.Look<int>(ref this.mortarMaxRange, "mortarMaxRange", 70, false);
            Scribe_Values.Look<int>(ref this.mortarTicksToFire, "mortarTicksToFire", 50, false);
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            mannableComp = GetComp<CompMannable>();
        }

        public override void Tick()
        {
            //base.Tick();

            if (Manned)
            {

                if (!initialized)
                {
                    comp = mannableComp.ManningPawn.GetComp<CompAbilityUserMight>();
                    this.verVal = mannableComp.ManningPawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_60mmMortar.FirstOrDefault((MightPowerSkill x) => x.label == "TM_60mmMortar_ver").level;
                    this.pwrVal = mannableComp.ManningPawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_60mmMortar.FirstOrDefault((MightPowerSkill x) => x.label == "TM_60mmMortar_pwr").level;
                    this.effVal = mannableComp.ManningPawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_60mmMortar.FirstOrDefault((MightPowerSkill x) => x.label == "TM_60mmMortar_eff").level;
                    this.mortarTicksToFire = Find.TickManager.TicksGame + (240 - (20*verVal));
                    this.mortarMaxRange += (verVal * 10);
                    if(verVal >= 3)
                    {
                        this.mortarCount++;
                    }
                    this.mortarAccuracy = mortarAccuracy - (.6f * effVal);
                    this.initialized = true;
                }

                if (!mannableComp.ManningPawn.DestroyedOrNull() && !mannableComp.ManningPawn.Dead && !mannableComp.ManningPawn.Downed)
                {
                    if (this.mortarTicksToFire < Find.TickManager.TicksGame && this.mortarCount > 0)
                    {
                        this.mortarTicksToFire = Find.TickManager.TicksGame + (50 - (5 * verVal));
                        Pawn target = TM_Calc.FindNearbyEnemy(this.Position, this.Map, this.Faction, this.mortarMaxRange, this.mortarMinRange);
                        if (target != null && target.Position.IsValid && target.Position.DistanceToEdge(this.Map) > 5)
                        {
                            bool flag = target.Position != default(IntVec3);
                            if (flag)
                            {
                                IntVec3 rndTarget = target.Position;
                                rndTarget.x += Mathf.RoundToInt(Rand.Range(-mortarAccuracy, mortarAccuracy));
                                rndTarget.z += Mathf.RoundToInt(Rand.Range(-mortarAccuracy, mortarAccuracy));
                                Thing launchedThing = new Thing()
                                {
                                    def = projectileDef
                                };
                                int arc = 1;
                                if(target.Position.x >= this.Position.x)
                                {
                                    arc = -1;
                                }
                                FlyingObject_Advanced flyingObject = (FlyingObject_Advanced)GenSpawn.Spawn(this.projectileDef, this.Position, this.Map);
                                flyingObject.AdvancedLaunch(this, null, 0, Rand.Range(60, 70), false, this.DrawPos, rndTarget, launchedThing, Rand.Range(40, 46), true, Rand.Range(14 + pwrVal, 20 + (2*pwrVal)), (2 + (.35f * pwrVal)), DamageDefOf.Bomb, null, arc, true);
                                this.mortarCount--;
                            }                            
                            SoundInfo info = SoundInfo.InMap(new TargetInfo(this.Position, this.Map, false), MaintenanceType.None);
                            info.pitchFactor = 1.6f;
                            info.volumeFactor = .7f;
                            SoundDef.Named("Mortar_LaunchA").PlayOneShot(info);
                        }
                    }
                }

                if(this.mortarCount <= 0)
                {
                    this.mannableComp.ManningPawn.jobs.EndCurrentJob(JobCondition.Succeeded);
                    this.Destroy(DestroyMode.Vanish);
                }
            }
            else
            {                
                this.Destroy(DestroyMode.Vanish);
            }
        }        

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            for (int i = 0; i < 4; i++)
            {
                Vector3 rndPos = this.DrawPos;
                rndPos.x += Rand.Range(-.5f, .5f);
                rndPos.z += Rand.Range(-.5f, .5f);
                TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_SparkFlash"), rndPos, this.Map, Rand.Range(.6f, .8f), .1f, .05f, .05f, 0, 0, 0, Rand.Range(0, 360));
                MoteMaker.ThrowSmoke(rndPos, this.Map, Rand.Range(.8f, 1.2f));
                rndPos = this.DrawPos;
                rndPos.x += Rand.Range(-.5f, .5f);
                rndPos.z += Rand.Range(-.5f, .5f);
                TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_ElectricalSpark"), rndPos, this.Map, Rand.Range(.4f, .7f), .2f, .05f, .1f, 0, 0, 0, Rand.Range(0, 360));
            }
            base.Destroy(mode);
        }
    }
}
