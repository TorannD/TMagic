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
    public class FlyingObject_Spinning : ThingWithComps
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

        public float force = 1f;

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
                Vector3 b = (this.destination - this.origin) * (1f - (float)this.ticksToImpact / (float)this.StartingTicksToImpact);
                return this.origin + b + Vector3.up * this.def.Altitude;
            }
        }

        public virtual Quaternion ExactRotation
        {
            get
            {
                return Quaternion.LookRotation(this.destination - this.origin);
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
            base.Tick();
            Vector3 exactPosition = this.ExactPosition;
            this.ticksToImpact--;
            bool flag = !this.ExactPosition.InBounds(base.Map);
            if (flag)
            {
                this.ticksToImpact++;
                base.Position = this.ExactPosition.ToIntVec3();
                this.Destroy(DestroyMode.Vanish);
            }
            else if(!this.ExactPosition.ToIntVec3().Walkable(base.Map))
            {
                this.earlyImpact = true;
                this.impactForce = (this.DestinationCell - this.ExactPosition.ToIntVec3()).LengthHorizontal + (this.speed * .2f);
                this.ImpactSomething();
            }
            else
            {
                base.Position = this.ExactPosition.ToIntVec3();
                if(Find.TickManager.TicksGame % 3 == 0)
                {
                    MoteMaker.ThrowDustPuff(base.Position, base.Map, Rand.Range(0.6f, .8f));
                }               
                
                bool flag2 = this.ticksToImpact <= 0;
                if (flag2)
                {
                    bool flag3 = this.DestinationCell.InBounds(base.Map);
                    if (flag3)
                    {
                        base.Position = this.DestinationCell;
                    }
                    this.ImpactSomething();
                }                
            }
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

        private void DrawEffects(Vector3 pawnVec, Pawn flyingPawn, int magnitude)
        {
            bool flag = !pawn.Dead && !pawn.Downed;
            if (flag)
            {

            }
        }

        private void ImpactSomething()
        {
            bool flag = this.assignedTarget != null;
            if (flag)
            {
                Pawn pawn = this.assignedTarget as Pawn;
                bool flag2 = pawn != null && pawn.GetPosture() != PawnPosture.Standing && (this.origin - this.destination).MagnitudeHorizontalSquared() >= 20.25f && Rand.Value > 0.2f;
                if (flag2)
                {
                    this.Impact(null);
                }
                else
                {
                    this.Impact(this.assignedTarget);
                }
            }
            else
            {
                this.Impact(null);
            }
        }

        protected virtual void Impact(Thing hitThing)
        {
            bool flag = hitThing == null;
            if (flag)
            {
                Pawn pawn;
                bool flag2 = (pawn = (base.Position.GetThingList(base.Map).FirstOrDefault((Thing x) => x == this.assignedTarget) as Pawn)) != null;
                if (flag2)
                {
                    hitThing = pawn;
                }
            }
            bool hasValue = this.impactDamage.HasValue;
            if (hasValue)
            {
                hitThing.TakeDamage(this.impactDamage.Value);
            }
            try
            {
                SoundDefOf.Ambient_AltitudeWind.sustainFadeoutTime.Equals(30.0f);                

                GenSpawn.Spawn(this.flyingThing, base.Position, base.Map);
                Thing p = this.flyingThing;
                if (this.flyingThing is Pawn)
                {
                    if (this.earlyImpact)
                    {
                        damageEntities(p, this.impactForce, DamageDefOf.Blunt);
                        damageEntities(p, 2 * this.impactForce, DamageDefOf.Stun);
                    }
                }
                else if (flyingThing.def.thingCategories != null && (flyingThing.def.thingCategories.Contains(ThingCategoryDefOf.Chunks) || flyingThing.def.thingCategories.Contains(ThingCategoryDef.Named("StoneChunks"))))
                {
                    float radius = 3f;
                    Vector3 center = this.ExactPosition;
                    if (this.earlyImpact)
                    {
                        bool wallFlag90neg = false;
                        IntVec3 wallCheck = (center + (Quaternion.AngleAxis(-90, Vector3.up) * this.direction)).ToIntVec3();
                        MoteMaker.ThrowMicroSparks(wallCheck.ToVector3Shifted(), base.Map);
                        wallFlag90neg = wallCheck.Walkable(base.Map);

                        wallCheck = (center + (Quaternion.AngleAxis(90, Vector3.up) * this.direction)).ToIntVec3();
                        MoteMaker.ThrowMicroSparks(wallCheck.ToVector3Shifted(), base.Map);
                        bool wallFlag90 = wallCheck.Walkable(base.Map);

                        if ((!wallFlag90 && !wallFlag90neg) || (wallFlag90 && wallFlag90neg))
                        {
                            //fragment energy bounces in reverse direction of travel
                            center = center + ((Quaternion.AngleAxis(180, Vector3.up) * this.direction) * 3);
                        }
                        else if (wallFlag90)
                        {
                            center = center + ((Quaternion.AngleAxis(90, Vector3.up) * this.direction) * 3);
                        }
                        else if (wallFlag90neg)
                        {
                            center = center + ((Quaternion.AngleAxis(-90, Vector3.up) * this.direction) * 3);
                        }

                    }

                    int num = GenRadial.NumCellsInRadius(radius);
                    for (int i = 0; i < (num *.75f); i++)
                    {
                        IntVec3 intVec = center.ToIntVec3() + GenRadial.RadialPattern[Rand.Range(1, num)];
                        if (intVec.IsValid && intVec.InBounds(base.Map))
                        {
                            Vector3 moteDirection = TM_Calc.GetVector(this.ExactPosition.ToIntVec3(), intVec);
                            TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Rubble"), this.ExactPosition, base.Map, Rand.Range(.3f, .5f), .2f, .02f, .05f, Rand.Range(-100, 100), 12f, (Quaternion.AngleAxis(90, Vector3.up) * moteDirection).ToAngleFlat(), 0);
                            GenExplosion.DoExplosion(intVec, base.Map, .4f, DamageDefOf.Blunt, pawn, Rand.Range(14, 22), 0, SoundDefOf.Pawn_Melee_Punch_HitBuilding, null, null, null, ThingDefOf.Filth_RubbleRock, .6f, 1, false, null, 0f, 1, 0, false);
                            MoteMaker.ThrowSmoke(intVec.ToVector3Shifted(), base.Map, Rand.Range(.6f, 1f));
                        }
                    }
                    damageEntities(p, 305, DamageDefOf.Blunt);
                }
                this.Destroy(DestroyMode.Vanish);
            }
            catch
            {
                GenSpawn.Spawn(this.flyingThing, base.Position, base.Map);
                Pawn p = this.flyingThing as Pawn;

                this.Destroy(DestroyMode.Vanish);
            }
        }

        public void damageEntities(Thing e, float d, DamageDef type)
        {
            int amt = Mathf.RoundToInt(Rand.Range(.75f, 1.25f) * d);
            DamageInfo dinfo = new DamageInfo(type, amt, 0, (float)-1, this.launcher, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
            bool flag = e != null;
            if (flag)
            {
                e.TakeDamage(dinfo);
            }
        }
    }
}
