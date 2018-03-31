using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using AbilityUser;
using Verse;
using UnityEngine;


namespace TorannMagic
{
    public class Verb_Entertain : Verb_UseAbility
    {
        protected override bool TryCastShot()
        {
            Pawn caster = base.CasterPawn;
            Pawn pawn = this.currentTarget.Thing as Pawn;
            bool flag = pawn != null && !pawn.Dead;
            if (flag)
            {
                if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_EntertainingHD")))
                {
                    using (IEnumerator<Hediff> enumerator = pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Hediff rec = enumerator.Current;
                            if (rec.def.defName.Contains("TM_EntertainingHD"))
                            {
                                pawn.health.RemoveHediff(rec);
                            }
                        }
                    }
                }
                else
                {
                    HealthUtility.AdjustSeverity(pawn, HediffDef.Named("TM_EntertainingHD"), .95f);
                    TM_MoteMaker.ThrowNoteMote(pawn.DrawPos, pawn.Map, .8f);
                }
            }
            return true;
        }
    }
}
