using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace TorannMagic
{
    public class DamageWorker_TM : DamageWorker
    {

        public override float Apply(DamageInfo dinfo, Thing victim)
        {
            Log.Message("damage worker called");
            Pawn pawn = victim as Pawn;
            Hediff shield = new Hediff();
            shield = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_HediffShield);
            Log.Message("calling damageworker_tm with shield as " + shield.Label);
            if (shield != null)
            {
                Log.Message("adjust damage, reduce energy");
            }
            return base.Apply(dinfo, victim);
        }

    }
}
