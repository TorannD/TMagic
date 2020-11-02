using RimWorld;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    public class FlyingObject_SpiritWolves : Projectile
    {
        private float angle;
        Vector3 direction = default(Vector3);
        Vector3 directionP = default(Vector3);
        IEnumerable<IntVec3> lastRadial;

        protected float speed = 20f;
        protected Thing assignedTarget;
        protected Thing flyingThing;
        Pawn pawn;

        public DamageInfo? impactDamage;
        public bool damageLaunched = true;
        public bool explosion = false;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<Vector3>(ref this.direction, "direction", default(Vector3), false);
            Scribe_Values.Look<float>(ref this.angle, "angle", 0, false);
            Scribe_Values.Look<float>(ref this.speed, "speed", 20, false);
            Scribe_Values.Look<bool>(ref this.damageLaunched, "damageLaunched", true, false);
            Scribe_Values.Look<bool>(ref this.explosion, "explosion", false, false);
            Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
            Scribe_References.Look<Thing>(ref this.assignedTarget, "assignedTarget", false);
            Scribe_References.Look<Thing>(ref this.flyingThing, "flyingThing", false);
        }

        protected new int StartingTicksToImpact
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

        protected new IntVec3 DestinationCell
        {
            get
            {
                return new IntVec3(this.destination);
            }
        }

        //public virtual Vector3 ExactPosition
        //{
        //    get
        //    {
        //        Vector3 b = (this.destination - this.origin) * (1f - (float)this.ticksToImpact / (float)this.StartingTicksToImpact);
        //        return this.origin + b + Vector3.up * this.def.Altitude;
        //    }
        //}

        //public virtual Quaternion ExactRotation
        //{
        //    get
        //    {
        //        return Quaternion.LookRotation(this.destination - this.origin);
        //    }
        //}

        public override Vector3 DrawPos
        {
            get
            {
                return this.ExactPosition;
            }
        }

        private void Initialize()
        {
            if (pawn != null)
            {
                MoteMaker.MakeStaticMote(pawn.TrueCenter(), pawn.Map, ThingDefOf.Mote_ExplosionFlash, 12f);
                SoundDefOf.Ambient_AltitudeWind.sustainFadeoutTime.Equals(30.0f);
                MoteMaker.ThrowDustPuff(pawn.Position, pawn.Map, Rand.Range(1.2f, 1.8f));
                GetVector();
                this.angle = (Quaternion.AngleAxis(90, Vector3.up) * this.direction).ToAngleFlat();
            }
        }

        public void GetVector()
        {
            Vector3 heading = (this.destination - this.ExactPosition);
            float distance = heading.magnitude;
            this.direction = heading / distance;
            this.directionP = Quaternion.AngleAxis(90, Vector3.up) * this.direction;
        }

        public void Launch(Thing launcher, LocalTargetInfo targ, Thing flyingThing, DamageInfo? impactDamage)
        {
            this.Launch(launcher, base.Position.ToVector3Shifted(), targ, flyingThing, impactDamage);
        }

        public void Launch(Thing launcher, LocalTargetInfo targ, Thing flyingThing)
        {
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
            this.launcher = launcher;
            this.origin = origin;
            this.impactDamage = newDamageInfo;
            this.speed = this.def.projectile.speed;
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
            if (this.ticksToImpact >= 0)
            {
                IEnumerable<IntVec3> effectRadial = GenRadial.RadialCellsAround(this.ExactPosition.ToIntVec3(), 2, true);
                DrawEffects(effectRadial);
                DoEffects(effectRadial);
                this.lastRadial = effectRadial;
            }
            this.ticksToImpact--;
            base.Position = this.ExactPosition.ToIntVec3();
            bool flag = !this.ExactPosition.InBounds(base.Map);
            if (flag)
            {
                this.ticksToImpact++;
                this.Destroy(DestroyMode.Vanish);
            }
            else
            {
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

        public void DrawEffects(IEnumerable<IntVec3> effectRadial)
        {
            if (lastRadial != null)
            {
                IntVec3 curCell = effectRadial.Except(this.lastRadial).RandomElement();

                bool flag2 = Find.TickManager.TicksGame % 3 == 0;
                float fadeIn = .2f;
                float fadeOut = .25f;
                float solidTime = .05f;
                if (this.direction.ToAngleFlat() >= -135 && this.direction.ToAngleFlat() < -45)
                {                    
                    TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_SpiritWolf_North, curCell.ToVector3(), base.Map, .8f, solidTime, fadeIn, fadeOut, 0, Rand.Range(5, 8), this.angle + Rand.Range(-20, 20), 0);
                    if (flag2)
                    {
                        IEnumerable<IntVec3> effectRadialSmall = GenRadial.RadialCellsAround(this.ExactPosition.ToIntVec3(), 1, true);
                        curCell = effectRadialSmall.RandomElement();
                        TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_SpiritWolf_North, curCell.ToVector3(), base.Map, .8f, solidTime, fadeIn, fadeOut, 0, Rand.Range(10, 15), this.angle + Rand.Range(-20, 20), 0);
                    }
                }
                else if (this.direction.ToAngleFlat() >= 45 && this.direction.ToAngleFlat() < 135)
                {
                   TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_SpiritWolf_South, curCell.ToVector3(), base.Map, .8f, solidTime, fadeIn, fadeOut, 0, Rand.Range(5, 8), this.angle + Rand.Range(-20, 20), 0);
                    if (flag2)
                    {
                        IEnumerable<IntVec3> effectRadialSmall = GenRadial.RadialCellsAround(this.ExactPosition.ToIntVec3(), 1, true);
                        curCell = effectRadialSmall.RandomElement();                        
                        TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_SpiritWolf_South, curCell.ToVector3(), base.Map, .8f, solidTime, fadeIn, fadeOut, 0, Rand.Range(10, 15), this.angle + Rand.Range(-20, 20), 0);
                    }
                }
                else if (this.direction.ToAngleFlat() >= -45 && this.direction.ToAngleFlat() < 45)
                {                    
                    TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_SpiritWolf_East, curCell.ToVector3(), base.Map, .8f, solidTime, fadeIn, fadeOut, 0, Rand.Range(5, 8), this.angle + Rand.Range(-20, 20), 0);
                    if (flag2)
                    {
                        IEnumerable<IntVec3> effectRadialSmall = GenRadial.RadialCellsAround(this.ExactPosition.ToIntVec3(), 1, true);
                        curCell = effectRadialSmall.RandomElement();
                        TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_SpiritWolf_East, curCell.ToVector3(), base.Map, .8f, solidTime, fadeIn, fadeOut, 0, Rand.Range(10, 15), this.angle + Rand.Range(-20, 20), 0);                        
                    }
                }
                else
                {                    
                    TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_SpiritWolf_West, curCell.ToVector3(), base.Map, .8f, solidTime, fadeIn, fadeOut, 0, Rand.Range(5, 8), this.angle + Rand.Range(-20, 20), 0);
                    if (flag2)
                    {
                        IEnumerable<IntVec3> effectRadialSmall = GenRadial.RadialCellsAround(this.ExactPosition.ToIntVec3(), 1, true);
                        curCell = effectRadialSmall.RandomElement();
                        TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_SpiritWolf_West, curCell.ToVector3(), base.Map, .8f, solidTime, fadeIn, fadeOut, 0, Rand.Range(10, 15), this.angle + Rand.Range(-20, 20), 0);
                    }
                }

                curCell = lastRadial.RandomElement();
                if (curCell.InBounds(base.Map) && curCell.IsValid)
                {
                    ThingDef moteSmoke = ThingDef.Named("Mote_Smoke");
                    if (Rand.Chance(.5f))
                    {                        
                        TM_MoteMaker.ThrowGenericMote(moteSmoke, curCell.ToVector3(), base.Map, Rand.Range(1.2f, 1.5f), moteSmoke.mote.solidTime, moteSmoke.mote.fadeInTime, moteSmoke.mote.fadeOutTime, Rand.Range(-2, 2), Rand.Range(.3f, .4f), this.direction.ToAngleFlat(), Rand.Range(0, 360));
                    }
                    else
                    {                        
                        TM_MoteMaker.ThrowGenericMote(moteSmoke, curCell.ToVector3(), base.Map, Rand.Range(1.2f, 1.5f), moteSmoke.mote.solidTime, moteSmoke.mote.fadeInTime, moteSmoke.mote.fadeOutTime, Rand.Range(-2, 2), Rand.Range(.3f, .4f), 180+this.direction.ToAngleFlat(), Rand.Range(0, 360));
                    }
                }
            }
        }

        public void DoEffects(IEnumerable<IntVec3> effectRadial1)
        {
            IEnumerable<IntVec3> effectRadial2 = GenRadial.RadialCellsAround(this.ExactPosition.ToIntVec3(), 1, true);
            IEnumerable<IntVec3> effectRadial = effectRadial1.Except(effectRadial2);
            IntVec3 curCell;
            List<Thing> hitList = new List<Thing>();
            for (int i = 0; i < effectRadial.Count(); i++)
            {
                curCell = effectRadial.ToArray<IntVec3>()[i];
                if (curCell.InBounds(base.Map) && curCell.IsValid)
                {
                    hitList = curCell.GetThingList(base.Map);
                    for (int j = 0; j < hitList.Count; j++)
                    {
                        if (hitList[j] is Pawn && hitList[j].Faction != pawn.Faction)
                        {
                            DamageEntities(hitList[j]);
                        }
                    }
                }
            }
        }

        public void DamageEntities(Thing e)
        {
            int amt = Mathf.RoundToInt(Rand.Range(this.def.projectile.GetDamageAmount(1, null) * .75f, this.def.projectile.GetDamageAmount(1, null) * 1.25f));
            DamageInfo dinfo = new DamageInfo(DamageDefOf.Stun, amt, 0, (float)-1, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
            DamageInfo dinfo2 = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_Spirit, Mathf.RoundToInt(amt * .1f), 0, (float)-1, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
            bool flag = e != null;
            if (flag)
            {
                e.TakeDamage(dinfo);
                e.TakeDamage(dinfo2);
            }
        }

        public override void Draw()
        {
            bool flag = this.flyingThing != null;
            if (flag)
            {
                Graphics.DrawMesh(MeshPool.plane10, this.DrawPos, this.ExactRotation, this.flyingThing.def.DrawMatSingle, 0);
                base.Comps_PostDraw();
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

        protected override void Impact(Thing hitThing)
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
            this.Destroy(DestroyMode.Vanish);
        }
    }
}
