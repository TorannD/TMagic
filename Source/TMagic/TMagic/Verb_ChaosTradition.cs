using RimWorld;
using System;
using Verse;
using AbilityUser;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Harmony;

namespace TorannMagic
{
    public class Verb_ChaosTradition : Verb_UseAbility  
    {

        private int pwrVal = 0;
        CompAbilityUserMagic comp;
        Map map;

        protected override bool TryCastShot()
        {
            bool result = false;
            map = this.CasterPawn.Map;
            comp = this.CasterPawn.GetComp<CompAbilityUserMagic>();
            MagicPowerSkill pwr = comp.MagicData.MagicPowerSkill_Recall.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Recall_pwr");
            pwrVal = pwr.level;

            if (this.CasterPawn != null && !this.CasterPawn.Downed && comp != null)
            {
                comp.recallSet = true;
                comp.recallExpiration = Mathf.RoundToInt(Find.TickManager.TicksGame + (20 * 2500 * (1 + (.2f * pwrVal))));
                TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_AlterFate, CasterPawn.DrawPos, this.CasterPawn.Map, 1f, .2f, 0, 1f, Rand.Range(-500, 500), 0, 0, Rand.Range(0, 360));
                MoteMaker.ThrowHeatGlow(this.CasterPawn.Position, this.CasterPawn.Map, 1.4f);
            }
            else
            {
                Log.Warning("failed to TryCastShot");
            }

            this.burstShotsLeft = 0;
            return result;
        }
        
    }
}
