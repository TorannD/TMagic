﻿using System;
using Verse;
using Verse.AI;
using AbilityUser;
using RimWorld;


namespace TorannMagic
{
    public class Verb_Teach : Verb_UseAbility
    {
        bool validTarg;
        //Used specifically for non-unique verbs that ignore LOS (can be used with shield belt)
        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            if (targ.IsValid && targ.CenterVector3.InBounds(base.CasterPawn.Map) && !targ.Cell.Fogged(base.CasterPawn.Map) && targ.Cell.Walkable(base.CasterPawn.Map))
            {
                if ((root - targ.Cell).LengthHorizontal < this.verbProps.range)
                {
                    validTarg = true;
                }
                else
                {
                    //out of range
                    validTarg = false;
                }
            }
            else
            {
                validTarg = false;
            }
            return validTarg;
        }

        protected override bool TryCastShot()
        {
            Map map = base.CasterPawn.Map;
            Pawn mentor = base.CasterPawn;

            if(this.currentTarget.Thing != null && this.currentTarget.Thing is Pawn && this.currentTarget.Thing != mentor)
            {
                Pawn student = this.currentTarget.Thing as Pawn;
                CompAbilityUserMagic mentorCompMagic = mentor.GetComp<CompAbilityUserMagic>();
                CompAbilityUserMight mentorCompMight = mentor.GetComp<CompAbilityUserMight>();
                CompAbilityUserMagic studentCompMagic = student.GetComp<CompAbilityUserMagic>();
                CompAbilityUserMight studentCompMight = student.GetComp<CompAbilityUserMight>();
                if (mentorCompMagic.IsMagicUser && !mentor.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                {
                    if(studentCompMagic.IsMagicUser)
                    {
                        if (mentor.relations.OpinionOf(student) > -20)
                        {
                            Job job = new Job(TorannMagicDefOf.JobDriver_TM_Teach, student);
                            mentor.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                        }
                        else
                        {
                            Messages.Message("TM_CanNotTeachMagicDislike".Translate(
                            mentor.LabelShort,
                            student.LabelShort
                        ), MessageTypeDefOf.RejectInput, false);
                        }
                    }
                    else
                    {
                        Messages.Message("TM_CanNotTeachMagic".Translate(
                            mentor.LabelShort,
                            student.LabelShort
                        ), MessageTypeDefOf.RejectInput, false);
                    }
                }
                else if (mentorCompMight.IsMightUser)
                {
                    if(studentCompMight.IsMightUser)
                    {
                        if(mentor.relations.OpinionOf(student) > -20)
                        {
                            Job job = new Job(TorannMagicDefOf.JobDriver_TM_Teach, student);
                            mentor.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                        }
                        else
                        {
                            Messages.Message("TM_CanNotTeachCombatDislike".Translate(
                                mentor.LabelShort,
                                student.LabelShort
                            ), MessageTypeDefOf.RejectInput, false);
                        }
                    }
                    else
                    {
                        Messages.Message("TM_CanNotTeachCombat".Translate(
                            mentor.LabelShort,
                            student.LabelShort
                        ), MessageTypeDefOf.RejectInput, false);
                    }
                }
                else
                {
                    Log.Message("undetected might or magic user attempting to teach skill");
                }                
            }
            else
            {
                Messages.Message("TM_InvalidTarget".Translate(
                        mentor.LabelShort,
                        this.Ability.Def.label
                    ), MessageTypeDefOf.RejectInput, false);
            }

            this.burstShotsLeft = 0;
            return false;
        }
    }
}
