using System;
using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TorannMagic
{
    public class CompLeaper : ThingComp
    {
        private bool initialized = true;
        public float explosionRadius = 2f;
        private int nextLeap = 0;

        private Pawn pawn
        {
            get
            {
                Pawn pawn = this.parent as Pawn;
                bool flag = pawn == null;
                if (flag)
                {
                    Log.Error("pawn is null");
                }
                return pawn;
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            if(Find.TickManager.TicksGame % 10 == 0)
            {
                if(this.pawn.Downed)
                {
                    GenExplosion.DoExplosion(this.pawn.Position, this.pawn.Map, Rand.Range(this.explosionRadius * .5f, this.explosionRadius * 1.5f), DamageDefOf.Burn, this.pawn, Rand.Range(6, 10), null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
                    this.pawn.Kill(null, null);
                }
            }
            if(Find.TickManager.TicksGame % nextLeap == 0 && !pawn.Downed && !pawn.Dead)
            {
                this.nextLeap = Mathf.RoundToInt(Rand.Range(Props.ticksBetweenLeapChance * .75f, 1.25f * Props.ticksBetweenLeapChance));
                LocalTargetInfo lti = this.pawn.jobs.curJob.targetA.Thing;
                if (lti != null)
                {
                    Thing target = lti.Thing;
                    if (target is Pawn && target.Spawned)
                    {
                        float targetRange = (target.Position - this.pawn.Position).LengthHorizontal;
                        if (targetRange <= this.Props.leapRangeMax && targetRange > this.Props.leapRangeMin)
                        {
                            if (Rand.Chance(this.Props.GetLeapChance))
                            {
                                if (CanHitTargetFrom(this.pawn.Position, target))
                                {
                                    LeapAttack(target);
                                }
                            }
                            else
                            {
                                if (this.Props.textMotes)
                                {
                                    if (Rand.Chance(.5f))
                                    {
                                        MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, "grrr", -1);
                                    }
                                    else
                                    {
                                        MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.Map, "hsss", -1);
                                    }
                                }
                            }
                        }
                        else if (this.Props.bouncingLeaper)
                        {
                            Faction targetFaction = null;
                            if (target != null && target.Faction != null)
                            {
                                targetFaction = target.Faction;
                            }                            
                            IntVec3 curCell;
                            
                            IEnumerable<IntVec3> targets = GenRadial.RadialCellsAround(this.pawn.Position, this.Props.leapRangeMax, false);
                            for (int i = 0; i < targets.Count(); i++)
                            {
                                Pawn bounceTarget = null;

                                curCell = targets.ToArray<IntVec3>()[i];
                                if (curCell.InBounds(this.pawn.Map) && curCell.IsValid)
                                {
                                    bounceTarget = curCell.GetFirstPawn(this.pawn.Map);
                                    if (bounceTarget != null && bounceTarget != target && !bounceTarget.Downed && !bounceTarget.Dead && bounceTarget.RaceProps != null)
                                    {
                                        if(bounceTarget.Faction != null && bounceTarget.Faction == targetFaction)
                                        {
                                            if (Rand.Chance(1 - this.Props.leapChance))
                                            {
                                                i = targets.Count();
                                            }
                                            else
                                            {
                                                bounceTarget = null;
                                            }
                                        }
                                        else
                                        {
                                            bounceTarget = null;
                                        }                                        
                                    }
                                    else
                                    {
                                        bounceTarget = null;
                                    }
                                }                                

                                if (bounceTarget != null)
                                {
                                    
                                    if (CanHitTargetFrom(this.pawn.Position, target))
                                    {
                                        this.pawn.jobs.StopAll();
                                        this.pawn.TryStartAttack(bounceTarget);
                                        LeapAttack(bounceTarget);
                                    }                                    
                                }
                                targets.GetEnumerator().MoveNext();
                            }
                        }
                    }
                }
            }
        }

        public void LeapAttack(LocalTargetInfo target)
        {                
            bool flag = target.Cell != default(IntVec3);
            if (flag)
            {
                if (this.pawn != null && this.pawn.Position.IsValid && this.pawn.Spawned && this.pawn.Map != null)
                {
                    FlyingObject_Leap flyingObject = (FlyingObject_Leap)GenSpawn.Spawn(ThingDef.Named("FlyingObject_Leap"), this.pawn.Position, this.pawn.Map);
                    flyingObject.Launch(this.pawn, target.Cell, this.pawn);
                }
            }            
        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            this.initialized = true;
            Pawn pawn = this.parent as Pawn;
            this.nextLeap = Mathf.RoundToInt(Rand.Range(Props.ticksBetweenLeapChance * .75f, 1.25f * Props.ticksBetweenLeapChance));
            this.explosionRadius = this.Props.explodingLeaperRadius * Rand.Range(.8f, 1.25f);
        }

        public CompProperties_Leaper Props
        {
            get
            {
                return (CompProperties_Leaper)this.props;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<bool>(ref this.initialized, "initialized", true, false);
        }

        private bool CanHitTargetFrom(IntVec3 pawn, LocalTargetInfo target)
        {
            bool result = false;
            if (target.IsValid && target.CenterVector3.InBounds(this.pawn.Map) && !target.Cell.Fogged(this.pawn.Map) && target.Cell.Walkable(this.pawn.Map))
            {
                ShootLine shootLine;
                result = this.TryFindShootLineFromTo(pawn, target, out shootLine);                
            }
            else
            {
                result = false;
            }
            
            return result;
        }

        public bool TryFindShootLineFromTo(IntVec3 root, LocalTargetInfo targ, out ShootLine resultingLine)
        {
            if (targ.HasThing && targ.Thing.Map != this.pawn.Map)
            {
                resultingLine = default(ShootLine);
                return false;
            }
            resultingLine = new ShootLine(root, targ.Cell);
            if (!GenSight.LineOfSightToEdges(root, targ.Cell, this.pawn.Map, true, null))
            {
                return false;
            }
            return true;
        }
    }
}
