using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;
using RimWorld;
using Verse.AI;
using UnityEngine;
using AbilityUser;

namespace TorannMagic
{
    internal class JobDriver_Teach : JobDriver
    {
        private const TargetIndex caster = TargetIndex.B;

        int age = -1;
        int lastEffect = 0;
        int ticksTillEffects = 200;
        public int duration = 1260;
        bool isMageTeaching = false;
        bool isFighterTeaching = false;


        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (pawn.Reserve(TargetA, this.job, 1, 1, null, errorOnFailed))
            {
                return true;
            }
            return false;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            Pawn student = TargetA.Thing as Pawn;
            Toil gotoStudent = new Toil()
            {
                initAction = () =>
                {
                    pawn.pather.StartPath(TargetA, PathEndMode.Touch);
                },
                defaultCompleteMode = ToilCompleteMode.PatherArrival
            };
            yield return gotoStudent;
            Toil doTeaching = new Toil();
            doTeaching.initAction = delegate
            {
                CompAbilityUserMagic compMagic = student.GetComp<CompAbilityUserMagic>();
                CompAbilityUserMight compMight = student.GetComp<CompAbilityUserMight>();
                if(compMagic.IsMagicUser && !student.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                {
                    this.isMageTeaching = true;
                }
                else if(compMight.IsMightUser)
                {
                    this.isFighterTeaching = true;
                }
                else
                {
                    Log.Message("ending teaching job due to undetected class - should never occur unless initializing verb is faulty");
                    this.EndJobWith(JobCondition.Incompletable);
                }
                    
                if (age > duration)
                {
                    this.EndJobWith(JobCondition.Succeeded);
                }
                if (student.DestroyedOrNull() || student.Dead)
                {
                    this.EndJobWith(JobCondition.Incompletable);
                }

                this.pawn.rotationTracker.FaceTarget(TargetA);
                student.rotationTracker.FaceTarget(this.pawn);
                student.ClearAllReservations(false);
                this.pawn.ClearAllReservations(false);

            };
            doTeaching.tickAction = delegate
            {
                if (student.DestroyedOrNull() || student.Dead)
                {
                    this.EndJobWith(JobCondition.Incompletable);
                }
                if (age > (lastEffect + ticksTillEffects))
                {
                    DoTeachingEffects(student);
                    lastEffect = age;
                }
                if (!student.Drafted && student.CurJobDef != JobDefOf.Wait)
                {
                    if (student.jobs.posture == PawnPosture.Standing)
                    {
                        Job job = new Job(JobDefOf.Wait, student);
                        student.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                    }
                }
                age++;
                ticksLeftThisToil = duration - age;
                if (age > duration)
                {
                    this.EndJobWith(JobCondition.Succeeded);
                }
            };
            doTeaching.defaultCompleteMode = ToilCompleteMode.Delay;
            doTeaching.defaultDuration = this.duration;
            doTeaching.WithProgressBar(TargetIndex.B, delegate
            {
                if (this.pawn.DestroyedOrNull() || this.pawn.Dead)
                {
                    return 1f;
                }
                return 1f - (float)doTeaching.actor.jobs.curDriver.ticksLeftThisToil / this.duration;

            }, false, 0f);
            doTeaching.AddFinishAction(delegate
            {
                if (this.isMageTeaching)
                {
                    AssignMagicXP(student);
                }
                if(this.isFighterTeaching)
                {
                    AssignMightXP(student);
                }
                student.jobs.EndCurrentJob(JobCondition.Succeeded, false);
            });
            yield return doTeaching;
        }

        private void DoTeachingEffects(Pawn student)
        {
            int effectNum = Rand.RangeInclusive(0, 4);
            Color randomColor = new Color(Rand.Range(0f, 1f), Rand.Range(0f, 1f), Rand.Range(0f, 1f), Rand.Range(.7f, 1f));
            Vector3 drawPos;
            int rangeMax = Rand.Range(8, 15);
            if(Rand.Chance(.5f))
            {
                drawPos = this.pawn.DrawPos;
            }
            else
            {
                drawPos = student.DrawPos;
            }

            if(effectNum == 0)
            {
                for (int i = 0; i < rangeMax; i++)
                {
                    float direction = Rand.Range(0, 360);
                    ThingDef mote = new ThingDef();
                    mote = ThingDef.Named("Mote_Psi_Grayscale");
                    mote.graphicData.color = randomColor;
                    mote.rotatable = true;
                    mote.mote.rotateTowardsMoveDirection = true;
                    TM_MoteMaker.ThrowGenericMote(mote, drawPos, this.pawn.Map, Rand.Range(.1f, .3f), 0.2f, .02f, .1f, 0, Rand.Range(8, 10), direction, direction);
                    SoundInfo info = SoundInfo.InMap(new TargetInfo(this.pawn.Position, this.pawn.Map, false), MaintenanceType.None);
                    info.pitchFactor = 1.5f;
                    info.volumeFactor = .6f;
                    TorannMagicDefOf.ItemEnchanted.PlayOneShot(info);
                }
            }
            else if(effectNum ==1)
            {
                for (int i = 0; i < rangeMax / 2; i++)
                {
                    float direction = Rand.Range(-20, 20);
                    Vector3 rndPos = drawPos;
                    rndPos.x += Rand.Range(-1f, 1f);
                    rndPos.z += Rand.Range(-1f, 1f);
                    ThingDef mote = ThingDefOf.Mote_MicroSparks;
                    TM_MoteMaker.ThrowGenericMote(mote, rndPos, this.pawn.Map, Rand.Range(.6f, .9f), 0.4f, .1f, .8f, 0, Rand.Range(1, 1.5f), direction, direction);
                    SoundInfo info = SoundInfo.InMap(new TargetInfo(this.pawn.Position, this.pawn.Map, false), MaintenanceType.None);
                    info.pitchFactor = .75f;
                    info.volumeFactor = 1.2f;
                    TorannMagicDefOf.TM_AirWoosh.PlayOneShot(info);
                }
            }
            else if(effectNum == 2)
            {
                for (int i = 0; i < rangeMax; i++)
                {
                    float direction = Rand.Range(10, 20);
                    Vector3 rndPos = drawPos;
                    rndPos.x += Rand.Range(-2f, 2f);
                    rndPos.z += Rand.Range(-2f, 2f);
                    ThingDef mote = ThingDefOf.Mote_Smoke;
                    TM_MoteMaker.ThrowGenericMote(mote, rndPos, this.pawn.Map, Rand.Range(1f, 1.4f), 0.7f, .2f, .9f, Rand.Range(0, 200), Rand.Range(.3f, .6f), direction, direction);
                    SoundInfo info = SoundInfo.InMap(new TargetInfo(this.pawn.Position, this.pawn.Map, false), MaintenanceType.None);
                    info.pitchFactor = 1.4f;
                    info.volumeFactor = .5f;
                    SoundDefOf.Artillery_ShellLoaded.PlayOneShot(info);
                }
            }
            else if(effectNum == 3)
            {
                for (int i = 0; i < rangeMax; i++)
                {
                    float direction = Rand.Range(0,360);
                    Vector3 rndPos = drawPos;
                    rndPos.x += Rand.Range(-.3f, .3f);
                    rndPos.z += Rand.Range(-.3f, .3f);
                    ThingDef mote = ThingDef.Named("Mote_Shadow");
                    TM_MoteMaker.ThrowGenericMote(mote, rndPos, this.pawn.Map, Rand.Range(1f, 1.4f), 0.4f, Rand.Range(.1f, .4f), Rand.Range(.2f, .6f), Rand.Range(-100, 100), Rand.Range(1.2f, 2f), direction, direction);
                    SoundInfo info = SoundInfo.InMap(new TargetInfo(this.pawn.Position, this.pawn.Map, false), MaintenanceType.None);
                    info.pitchFactor = .8f;
                    info.volumeFactor = 1.2f;
                    TorannMagicDefOf.TM_Vibration.PlayOneShot(info);
                }
            }
            else if (effectNum == 4)
            {
                for (int i = 0; i < rangeMax; i++)
                {
                    float direction = Rand.Range(-20, 20);
                    Vector3 rndPos = drawPos;
                    rndPos.x += Rand.Range(-1f, 1f);
                    rndPos.z += Rand.Range(-1f, 1f);
                    ThingDef mote = ThingDef.Named("Mote_Twinkle");
                    TM_MoteMaker.ThrowGenericMote(mote, rndPos, this.pawn.Map, Rand.Range(.2f, .7f), Rand.Range(.2f, .6f), Rand.Range(0f, .8f), Rand.Range(.5f,.8f), Rand.Range(-50, 50), Rand.Range(.5f, 1f), direction, direction);
                    SoundInfo info = SoundInfo.InMap(new TargetInfo(this.pawn.Position, this.pawn.Map, false), MaintenanceType.None);
                    info.pitchFactor = 1.2f;
                    info.volumeFactor = .6f;
                    TorannMagicDefOf.TM_Gong.PlayOneShot(info);
                }
            }
        }

        private void AssignMagicXP(Pawn student)
        {
            CompAbilityUserMagic studentComp = student.GetComp<CompAbilityUserMagic>();
            CompAbilityUserMagic mentorComp = this.pawn.GetComp<CompAbilityUserMagic>();

            int studentOpinion = student.relations.OpinionOf(this.pawn);
            int mentorOpinion = this.pawn.relations.OpinionOf(student);
            int xpBase = Rand.Range(150,220) + studentOpinion + mentorOpinion;
            int xpGain = (int)(xpBase * ((mentorComp.MagicUserLevel - studentComp.MagicUserLevel) / 10));
            MoteMaker.ThrowText(student.DrawPos, student.MapHeld, "XP +" + xpGain, -1f);
            studentComp.MagicUserXP += xpGain;
        }

        private void AssignMightXP(Pawn student)
        {
            CompAbilityUserMight studentComp = student.GetComp<CompAbilityUserMight>();
            CompAbilityUserMight mentorComp = this.pawn.GetComp<CompAbilityUserMight>();

            int studentOpinion = student.relations.OpinionOf(this.pawn);
            int mentorOpinion = this.pawn.relations.OpinionOf(student);
            int xpBase = Rand.Range(150, 220) + studentOpinion + mentorOpinion;
            int xpGain = (int)(xpBase * ((mentorComp.MightUserLevel - studentComp.MightUserLevel) / 10));
            MoteMaker.ThrowText(student.DrawPos, student.MapHeld, "XP +" + xpGain, -1f);
            studentComp.MightUserXP += xpGain;

        }

        public Vector3 GetVector(IntVec3 center, IntVec3 objectPos)
        {
            Vector3 heading = (objectPos - center).ToVector3();
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;
            return direction;
        }
    }
}