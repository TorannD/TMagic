using Verse;
using AbilityUser;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using RimWorld;

namespace TorannMagic
{
    class Projectile_Scorn : Projectile_AbilityBase
    {
        int age = -1;
        int duration = 20;
        int verVal = 0;
        int pwrVal = 0;
        float arcaneDmg = 1;
        int strikeDelay = 4;
        int strikeNum = 1;
        float radius = 5;
        bool initialized = false;
        float angle = 0;
        List<IntVec3> cellList;
        Pawn pawn;
        IEnumerable<IntVec3> targets;
        Skyfaller skyfaller2;
        Skyfaller skyfaller;
        Map map;

        bool launchedFlag = false;
        bool pivotFlag = false;
        bool landedFlag = false;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.initialized, "initialized", true, false);
            Scribe_Values.Look<int>(ref this.age, "age", -1, false);
            Scribe_Values.Look<int>(ref this.duration, "duration", 1800, false);
            Scribe_Values.Look<int>(ref this.strikeDelay, "strikeDelay", 0, false);
            Scribe_Values.Look<int>(ref this.strikeNum, "strikeNum", 0, false);
            Scribe_Values.Look<int>(ref this.verVal, "verVal", 0, false);
            Scribe_Values.Look<int>(ref this.pwrVal, "pwrVal", 0, false);
            Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
            Scribe_Collections.Look<IntVec3>(ref this.cellList, "cellList", LookMode.Value);
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            bool flag = this.age < duration;
            if (!flag)
            {
                base.Destroy(mode);
            }
        }

        public override void Tick()
        {
            base.Tick();
            //this.age++;
        }

        protected override void Impact(Thing hitThing)
        {            
            base.Impact(hitThing);
           
            ThingDef def = this.def;
            Pawn victim = null;            

            if (!this.initialized)
            {
                this.pawn = this.launcher as Pawn;
                this.map = this.pawn.Map;                
                CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
                MagicPowerSkill pwr = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Repulsion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Repulsion_pwr");
                MagicPowerSkill ver = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Repulsion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Repulsion_ver");
                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                pwrVal = pwr.level;
                verVal = ver.level;
                this.arcaneDmg = comp.arcaneDmg;
                if (settingsRef.AIHardMode && !pawn.IsColonist)
                {
                    pwrVal = 3;
                    verVal = 3;
                }
                this.radius = this.def.projectile.explosionRadius + verVal;
                //this.duration = Mathf.RoundToInt(this.radius * this.strikeDelay);
                this.initialized = true;
            }

            if (!launchedFlag)
            {                
                skyfaller = SkyfallerMaker.SpawnSkyfaller(ThingDef.Named("TM_ScornLeaving"), pawn.Position, this.map);
                if(base.Position.x < pawn.Position.x)
                {
                    this.angle = Rand.Range(20, 40);
                }
                else
                {
                    this.angle = Rand.Range(-40, -20);
                }
                skyfaller.angle = this.angle;
                launchedFlag = true;
                pawn.DeSpawn();
            }
            if(skyfaller.DestroyedOrNull() && !pivotFlag)
            {
                skyfaller2 = SkyfallerMaker.SpawnSkyfaller(ThingDef.Named("TM_ScornIncoming"), base.Position, this.map);
                skyfaller2.angle = this.angle;
                pivotFlag = true;

            }
            if (skyfaller2.DestroyedOrNull() && pivotFlag && launchedFlag && !landedFlag)
            {
                landedFlag = true;
                GenSpawn.Spawn(pawn, base.Position, this.map);
                pawn.drafter.Drafted = true;
            }
            if(landedFlag)
            { 
                if (Find.TickManager.TicksGame % strikeDelay == 0)
                {
                    IEnumerable<IntVec3> targets;
                    if (strikeNum == 1)
                    {
                        targets = GenRadial.RadialCellsAround(base.Position, this.strikeNum, false);
                    }
                    else
                    {
                        IEnumerable<IntVec3> oldTargets = GenRadial.RadialCellsAround(base.Position, this.strikeNum - 1, false);
                        targets = GenRadial.RadialCellsAround(base.Position, this.strikeNum, false).Except(oldTargets);
                    }
                    for (int j = 0; j < targets.Count(); j++)
                    {
                        IntVec3 curCell = targets.ToArray<IntVec3>()[j];
                        if (curCell.IsValid && curCell.InBounds(this.map))
                        {
                            GenExplosion.DoExplosion(curCell, this.Map, .4f, TMDamageDefOf.DamageDefOf.TM_Shadow, this.pawn, (int)((this.def.projectile.damageAmountBase * (1 + .15*pwrVal)) * this.arcaneDmg * Rand.Range(.75f, 1.25f)), TorannMagicDefOf.TM_SoftExplosion, def, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
                        }
                    }
                    this.strikeNum++;
                    if (this.strikeNum > this.radius)
                    {
                        this.age = this.duration;
                        this.Destroy(DestroyMode.Vanish);
                    }
                }               
            }
        }

        public void LaunchFlyingObect(IntVec3 targetCell, Pawn pawn, int force)
        {
            bool flag = targetCell != null && targetCell != default(IntVec3);
            if (flag)
            {
                if (pawn != null && pawn.Position.IsValid && pawn.Spawned && pawn.Map != null && !pawn.Downed && !pawn.Dead)
                {
                    FlyingObject_Spinning flyingObject = (FlyingObject_Spinning)GenSpawn.Spawn(ThingDef.Named("FlyingObject_Spinning"), pawn.Position, pawn.Map);
                    flyingObject.speed = 25 + force;
                    flyingObject.Launch(pawn, targetCell, pawn);
                }
            }
        }

        public Vector3 GetVector(IntVec3 center, IntVec3 objectPos)
        {
            Vector3 heading = (objectPos - center).ToVector3();
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;
            return direction;
        }

        public void damageEntities(Pawn e, float d, DamageDef type)
        {
            int amt = Mathf.RoundToInt(Rand.Range(.5f, 1.5f) * d);
            DamageInfo dinfo = new DamageInfo(type, amt, (float)-1, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
            bool flag = e != null;
            if (flag)
            {
                e.TakeDamage(dinfo);
            }
        }
    }    
}