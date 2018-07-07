using System;
using Verse;
using RimWorld;
using UnityEngine;


namespace TorannMagic.Conditions
{
    public class IncidentWorker_ElementalAssault : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            int duration = Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f);
            GameCondition_ElementalAssault gameCondition_ElementalAssault = (GameCondition_ElementalAssault)GameConditionMaker.MakeCondition(GameConditionDef.Named("ElementalAssault"), duration, 0);
            map.gameConditionManager.RegisterCondition(gameCondition_ElementalAssault);
            base.SendStandardLetter(new TargetInfo(gameCondition_ElementalAssault.centerLocation.ToIntVec3, map, false), null, new string[0]);
            return true;
        }
    }
}



