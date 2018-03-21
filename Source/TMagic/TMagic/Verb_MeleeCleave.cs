using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using UnityEngine;


namespace TorannMagic
{


    [StaticConstructorOnStartup]
    public class Verb_MeleeCleave: Verb_MeleeAttack
    {
        private static readonly Color cleaveColor = new Color(160f, 160f, 160f);
        private static readonly Material cleavingMat = MaterialPool.MatFrom("Spells/cleave_straight", ShaderDatabase.Transparent, Verb_MeleeCleave.cleaveColor);

        protected override DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target)
        {
            for (int i = 0; i < 8; i++)
            {
                IntVec3 intVec = target.Cell + GenAdj.AdjacentCells[i];
                Pawn cleaveVictim = new Pawn();
                cleaveVictim = intVec.GetFirstPawn(target.Thing.Map);
                if (cleaveVictim != null && cleaveVictim.Faction != caster.Faction)
                {
                    DamageInfo dinfo = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_Cleave, (int)(this.tool.power * .6f), (float)-1, this.CasterPawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
                    cleaveVictim.TakeDamage(dinfo);
                    MoteMaker.ThrowMicroSparks(cleaveVictim.Position.ToVector3(), target.Thing.Map);
                    TM_MoteMaker.ThrowCrossStrike(cleaveVictim.Position.ToVector3Shifted(), cleaveVictim.Map, 1f);
                    TM_MoteMaker.ThrowBloodSquirt(cleaveVictim.Position.ToVector3Shifted(), cleaveVictim.Map, 1f);
                    DrawCleaving(cleaveVictim, base.CasterPawn, 10);
                }
            }
            TM_MoteMaker.ThrowCrossStrike(target.Thing.Position.ToVector3Shifted(), target.Thing.Map, 1f);
            TM_MoteMaker.ThrowBloodSquirt(target.Thing.Position.ToVector3Shifted(), target.Thing.Map, 1f);
            return base.ApplyMeleeDamageToTarget(target);  
        }

        private void DrawCleaving(Pawn cleavedPawn, Pawn caster, int magnitude)
        {
            bool flag = !caster.Dead && !caster.Downed;
            if (flag)
            {
                Vector3 vector = cleavedPawn.Position.ToVector3();
                vector.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
                float hyp = Mathf.Sqrt((Mathf.Pow(caster.Position.x - cleavedPawn.Position.x, 2)) + (Mathf.Pow(caster.Position.z - cleavedPawn.Position.z, 2)));
                float angleRad = Mathf.Asin(Mathf.Abs(caster.Position.x - cleavedPawn.Position.x) / hyp);
                float angle = Mathf.Rad2Deg * angleRad;
                //float angle = (float)Rand.Range(0, 360);
                Vector3 s = new Vector3(3f, 3f, 5f);
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, Verb_MeleeCleave.cleavingMat, 0);
            }
        }
    }
}
