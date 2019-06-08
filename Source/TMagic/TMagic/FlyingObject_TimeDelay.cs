using RimWorld;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using AbilityUser;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    public class FlyingObject_TimeDelay : ThingWithComps
    {
        protected Vector3 origin;
        protected Vector3 destination;
        private Vector3 direction;

        public float speed = 25f;
        public int spinRate = 0;        //spin rate > 0 makes the object rotate every spinRate Ticks
        private int rotation = 0;
        protected int ticksToImpact;
        protected Thing launcher;
        protected Thing assignedTarget;
        protected Thing flyingThing;
        private bool drafted = false;

        public float force = 1f;
        public int duration = 600;

        private bool earlyImpact = false;
        private float impactForce = 0;

        public DamageInfo? impactDamage;

        public bool damageLaunched = true;

        public bool explosion = false;

        public int weaponDmg = 0;

        Pawn pawn;
        CompAbilityUserMagic comp;

        TMPawnSummoned newPawn = new TMPawnSummoned();

        protected int StartingTicksToImpact
        {
            get
            {
                int num = Mathf.RoundToInt((this.origin - this.destination).magnitude / (this.speed / 100f));
                bool flag = num < 1;
                if (flag)
                {
                    num = 1;
                }
                return num;
            }
        }

        protected IntVec3 DestinationCell
        {
            get
            {
                return new IntVec3(this.destination);
            }
        }

        public virtual Vector3 ExactPosition
        {
            get
            {
                //Vector3 b = (this.destination - this.origin) * (1f - (float)this.ticksToImpact / (float)this.StartingTicksToImpact);
                //return this.origin + b + Vector3.up * this.def.Altitude;
                return this.origin + Vector3.up * this.def.Altitude;
            }
        }

        public virtual Quaternion ExactRotation
        {
            get
            {
                return Quaternion.LookRotation(this.origin - this.destination);
            }
        }

        public override Vector3 DrawPos
        {
            get
            {
                return this.ExactPosition;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<Vector3>(ref this.origin, "origin", default(Vector3), false);
            Scribe_Values.Look<Vector3>(ref this.destination, "destination", default(Vector3), false);
            Scribe_Values.Look<int>(ref this.ticksToImpact, "ticksToImpact", 0, false);
            Scribe_Values.Look<bool>(ref this.damageLaunched, "damageLaunched", true, false);
            Scribe_Values.Look<bool>(ref this.explosion, "explosion", false, false);
            Scribe_References.Look<Thing>(ref this.assignedTarget, "assignedTarget", false);
            Scribe_References.Look<Thing>(ref this.launcher, "launcher", false);
            Scribe_Deep.Look<Thing>(ref this.flyingThing, "flyingThing", new object[0]);
            Scribe_Values.Look<bool>(ref this.drafted, "drafted", false, false);
        }

        private void Initialize()
        {
            if (pawn != null)
            {
                MoteMaker.ThrowDustPuff(pawn.Position, pawn.Map, Rand.Range(1.2f, 1.8f));
            }

            this.direction = TM_Calc.GetVector(this.origin.ToIntVec3(), this.destination.ToIntVec3());
            //flyingThing.ThingID += Rand.Range(0, 2147).ToString();
        }

        public void Launch(Thing launcher, LocalTargetInfo targ, Thing flyingThing, DamageInfo? impactDamage)
        {
            this.Launch(launcher, base.Position.ToVector3Shifted(), targ, flyingThing, impactDamage);
        }

        public void Launch(Thing launcher, LocalTargetInfo targ, Thing flyingThing)
        {
            this.Launch(launcher, base.Position.ToVector3Shifted(), targ, flyingThing, null);
        }

        public void Launch(Thing launcher, LocalTargetInfo targ, Thing flyingThing, int _spinRate)
        {
            this.spinRate = _spinRate;
            this.Launch(launcher, base.Position.ToVector3Shifted(), targ, flyingThing, null);
        }

        public void Launch(Thing launcher, Vector3 origin, LocalTargetInfo targ, Thing flyingThing, DamageInfo? newDamageInfo = null)
        {
            
            bool spawned = flyingThing.Spawned;            
            pawn = launcher as Pawn;
            if (pawn != null && pawn.Drafted)
            {
                this.drafted = true;
            }
            if (spawned)
            {               
                flyingThing.DeSpawn();
            }
            this.speed = this.speed * this.force;
            this.launcher = launcher;
            this.origin = origin;
            this.impactDamage = newDamageInfo;
            this.flyingThing = flyingThing;

            bool flag = targ.Thing != null;
            if (flag)
            {
                this.assignedTarget = targ.Thing;
            }
            this.destination = targ.Cell.ToVector3Shifted();
            this.ticksToImpact = this.StartingTicksToImpact;
            this.Initialize();
        }        

        public override void Tick()
        {
            //base.Tick();
            //Vector3 exactPosition = this.ExactPosition;
            //this.ticksToImpact--;
            this.duration--;
            //bool flag = !this.ExactPosition.InBounds(base.Map);
            //if (flag)
            //{
            //    this.ticksToImpact++;
            //    base.Position = this.ExactPosition.ToIntVec3();
            //    this.Destroy(DestroyMode.Vanish);
            //}
            //else if(!this.ExactPosition.ToIntVec3().Walkable(base.Map))
            //{
            //    this.earlyImpact = true;
            //    this.impactForce = (this.DestinationCell - this.ExactPosition.ToIntVec3()).LengthHorizontal + (this.speed * .2f);
            //    this.ImpactSomething();
            //}
            //else
            //{
            base.Position = this.origin.ToIntVec3(); // this.ExactPosition.ToIntVec3();
                //if(Find.TickManager.TicksGame % 3 == 0)
                //{
                //    MoteMaker.ThrowDustPuff(base.Position, base.Map, Rand.Range(0.6f, .8f));
                //}               
                
                bool flag2 = this.duration <= 0;
                if (flag2)
                {
                    //bool flag3 = this.DestinationCell.InBounds(base.Map);
                    //if (flag3)
                    //{
                    //base.Position = this.origin.ToIntVec3();
                    //}
                    this.ImpactSomething();
                }                
            //}
        }

        public override void Draw()
        {
            bool flag = this.flyingThing != null;
            if (flag)
            {
                if (this.spinRate > 0)
                {
                    if(Find.TickManager.TicksGame % this.spinRate ==0)
                    {
                        this.rotation++;
                        if(this.rotation >= 4)
                        {
                            this.rotation = 0;
                        }
                    }
                    if (rotation == 0)
                    {
                        this.flyingThing.Rotation = Rot4.West;
                    }
                    else if (rotation == 1)
                    {
                        this.flyingThing.Rotation = Rot4.North;
                    }
                    else if (rotation == 2)
                    {
                        this.flyingThing.Rotation = Rot4.East;
                    }
                    else
                    {
                        this.flyingThing.Rotation = Rot4.South;
                    }
                }

                bool flag2 = this.flyingThing is Pawn;
                if (flag2)
                {
                    Vector3 arg_2B_0 = this.DrawPos;
                    bool flag4 = !this.DrawPos.ToIntVec3().IsValid;
                    if (flag4)
                    {
                        return;
                    }
                    Pawn pawn = this.flyingThing as Pawn;
                    pawn.Drawer.DrawAt(this.DrawPos);
                    Material bubble = TM_MatPool.TimeBubble;
                    Vector3 vec3 = this.DrawPos;
                    vec3.y++;
                    Vector3 s = new Vector3(2f, 1f, 2f);
                    Matrix4x4 matrix = default(Matrix4x4);
                    matrix.SetTRS(vec3, Quaternion.AngleAxis(0, Vector3.up), s);
                    Graphics.DrawMesh(MeshPool.plane10, matrix, bubble, 0, null);
                    //Graphics.DrawMesh(MeshPool.plane10, vec3, this.ExactRotation, bubble, 0);
                }
                else
                {
                    Graphics.DrawMesh(MeshPool.plane10, this.DrawPos, this.ExactRotation, this.flyingThing.def.DrawMatSingle, 0);
                }
            }
            else
            {
                Graphics.DrawMesh(MeshPool.plane10, this.DrawPos, this.ExactRotation, this.flyingThing.def.DrawMatSingle, 0);
            }
            base.Comps_PostDraw();
        }


        private void ImpactSomething()
        {
            this.Impact(null);            
        }

        protected virtual void Impact(Thing hitThing)
        {
            //try
            //{

                //GenSpawn.Spawn(this.flyingThing, base.Position, base.Map);
                GenPlace.TryPlaceThing(this.flyingThing, base.Position, base.Map, ThingPlaceMode.Near);
                if (this.flyingThing is Pawn)
                {
                    Pawn p = this.flyingThing as Pawn;
                    if (p.IsColonist && this.drafted && p.drafter != null)
                    {
                        p.drafter.Drafted = true;
                    }
                }                
                this.Destroy(DestroyMode.Vanish);
            //}
            //catch
            //{
            //    if (!this.flyingThing.Spawned)
            //    {
            //        GenSpawn.Spawn(this.flyingThing, base.Position, base.Map);
            //    }
            //    this.Destroy(DestroyMode.Vanish);
            //}
        }

    }
}
