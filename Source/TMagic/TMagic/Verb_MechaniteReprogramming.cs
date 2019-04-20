using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using AbilityUser;
using Verse;
using UnityEngine;


namespace TorannMagic
{
    public class Verb_MechaniteReprogramming : Verb_UseAbility
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
            Pawn caster = base.CasterPawn;
            Pawn pawn = this.currentTarget.Thing as Pawn;

            bool flag = pawn != null;
            if (flag)
            {
                int num = 1;

                if(!pawn.DestroyedOrNull() && pawn.health != null || pawn.health.hediffSet != null && !pawn.Dead) 
                {
                    bool success = false;
                    using (IEnumerator<Hediff> enumerator = pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Hediff rec = enumerator.Current;
                            bool flag2 = num > 0;
                            
                            if ( rec.def.defName == "SensoryMechanites")
                            {
                                pawn.health.RemoveHediff(rec);
                                HealthUtility.AdjustSeverity(pawn, TorannMagicDefOf.TM_ReprogrammedSensoryMechanites_HD, .001f);
                                num--;
                                success = true;
                            }
                            else if(rec.def.defName == "FibrousMechanites")
                            {
                                pawn.health.RemoveHediff(rec);
                                HealthUtility.AdjustSeverity(pawn, TorannMagicDefOf.TM_ReprogrammedFibrousMechanites_HD, .001f);
                                num--;
                                success = true;
                            }                            
                                                        
                        }
                    }
                    if (success == true)
                    {
                        TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3(), pawn.Map, 1.5f);
                        MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, "Mechanite Reprogramming" + ": " + StringsToTranslate.AU_CastSuccess, -1f);
                    }
                    else
                    {
                        Messages.Message("TM_CureDiseaseTypeFail".Translate(), MessageTypeDefOf.NegativeEvent);
                        MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, "Mechanite Reprogramming" + ": " + StringsToTranslate.AU_CastFailure, -1f);
                    }
                }
                else
                {
                    MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, "Mechanite Reprogramming" + ": " + StringsToTranslate.AU_CastFailure, -1f);
                }
                
            }
            return false;
        }
    }
}
