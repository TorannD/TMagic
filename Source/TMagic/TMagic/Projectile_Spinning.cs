using System.Linq;
using Verse;
using AbilityUser;
using UnityEngine;
using RimWorld;
using System.Collections.Generic;
using System;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    class Projectile_Spinning : Projectile_AbilityBase
    {
        private int rotationOffset = 0;

        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            Pawn pawn = this.launcher as Pawn;
            base.Impact(hitThing);
            ThingDef def = this.def;
            try
            {
                if (pawn != null)
                {
                    Pawn victim = hitThing as Pawn;
                    CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();

                    if (victim != null && comp != null && Rand.Chance(.8f))
                    {
                        TM_Action.DamageEntities(victim, null, this.def.projectile.GetDamageAmount(1, null) * comp.mightPwr, DamageDefOf.Cut, pawn);
                        TM_MoteMaker.ThrowBloodSquirt(victim.DrawPos, victim.Map, .8f);
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                //
            }
        }

        public override void Draw()
        {
            this.rotationOffset += Rand.Range(20, 36);
            if(this.rotationOffset > 360)
            {
                this.rotationOffset = 0;
            }
            Mesh mesh = MeshPool.GridPlane(this.def.graphicData.drawSize);
            Graphics.DrawMesh(mesh, DrawPos, (Quaternion.AngleAxis(rotationOffset, Vector3.up) * ExactRotation), def.DrawMatSingle, 0);
            Comps_PostDraw();
        }

    }    
}


