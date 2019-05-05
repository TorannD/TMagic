using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using UnityEngine;
using AbilityUser;


namespace TorannMagic
{
    [StaticConstructorOnStartup]
    public class Verb_PommelStrike : Verb_UseAbility
    {


        protected override bool TryCastShot()
        {

            BodyPartRecord hitPart = null;
            DamageInfo dinfo = new DamageInfo(DamageDefOf.Stun, (int)(10), 0, (float)-1, this.CasterPawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
            if (this.currentTarget != null && this.currentTarget.Thing != null)
            {
                Pawn targetPawn = this.currentTarget.Thing as Pawn;
                if(targetPawn != null)
                {
                    if (Rand.Chance(targetPawn.health.hediffSet.PainTotal * 2f))
                    {
                        //Log.Message("target pawn in " + targetPawn.health.hediffSet.PainTotal + " pain");
                        hitPart = targetPawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.Spine).RandomElement();
                        if (hitPart != null && this.CasterPawn.equipment != null && this.CasterPawn.equipment.Primary != null)
                        {
                            dinfo = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_DisablingBlow, 6, 12, (float)-1, this.CasterPawn, hitPart, this.CasterPawn.equipment.Primary.def, DamageInfo.SourceCategory.ThingOrUnknown, targetPawn);
                        }
                        else if (hitPart != null)
                        {
                            dinfo = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_DisablingBlow, 4, 12, (float)-1, this.CasterPawn, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown, targetPawn);
                        }
                        else
                        {
                            dinfo = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_DisablingBlow, 4, 2, (float)-1, this.CasterPawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, targetPawn);
                        }
                        Vector3 strikeStartVec = this.CasterPawn.DrawPos;
                        strikeStartVec.z += .7f;
                        Vector3 angle = TM_Calc.GetVector(strikeStartVec, targetPawn.DrawPos);
                        TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_Strike, strikeStartVec, this.CasterPawn.Map, .5f, .1f, .05f, .1f, 0, 10f, (Quaternion.AngleAxis(90, Vector3.up) * angle).ToAngleFlat(), (Quaternion.AngleAxis(90, Vector3.up) * angle).ToAngleFlat());
                    }
                    targetPawn.TakeDamage(dinfo);
                }
            }

            //for (int i = 0; i < 8; i++)
            //{
            //    IntVec3 intVec = target.Cell + GenAdj.AdjacentCells[i];
            //    Pawn cleaveVictim = new Pawn();
            //    cleaveVictim = intVec.GetFirstPawn(target.Thing.Map);
            //    if (cleaveVictim != null && cleaveVictim.Faction != caster.Faction)
            //    {
            //        cleaveVictim.TakeDamage(dinfo);
            //        MoteMaker.ThrowMicroSparks(cleaveVictim.Position.ToVector3(), target.Thing.Map);
            //        TM_MoteMaker.ThrowCrossStrike(cleaveVictim.Position.ToVector3Shifted(), cleaveVictim.Map, 1f);
            //        TM_MoteMaker.ThrowBloodSquirt(cleaveVictim.Position.ToVector3Shifted(), cleaveVictim.Map, 1f);
            //    }
            //}
            //TM_MoteMaker.ThrowCrossStrike(target.Thing.Position.ToVector3Shifted(), target.Thing.Map, 1f);
            //TM_MoteMaker.ThrowBloodSquirt(target.Thing.Position.ToVector3Shifted(), target.Thing.Map, 1f);
            return true;

        }
    }
}
