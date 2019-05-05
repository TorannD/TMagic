using System;
using System.Linq;
using Verse;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;


namespace TorannMagic.Conditions
{
    public class IncidentWorker_ElementalAssault : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (settingsRef.riftChallenge > 0)
            {
                Map map = (Map)parms.target;
                int duration = Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f);
                GameCondition_ElementalAssault gameCondition_ElementalAssault = (GameCondition_ElementalAssault)GameConditionMaker.MakeCondition(GameConditionDef.Named("ElementalAssault"), duration, 0);
                map.gameConditionManager.RegisterCondition(gameCondition_ElementalAssault);
                base.SendStandardLetter(new TargetInfo(gameCondition_ElementalAssault.centerLocation.ToIntVec3, map, false), null, new string[0]);
                List<Faction> elementalFaction = Find.FactionManager.AllFactions.ToList();
                bool factionFlag = false;
                for (int i = 0; i < elementalFaction.Count; i++)
                {
                    if (elementalFaction[i].def.defName == "TM_ElementalFaction")
                    {
                        Faction.OfPlayer.TrySetRelationKind(elementalFaction[i], FactionRelationKind.Hostile, false, null, null);
                        factionFlag = true;
                    }
                }
                if(!factionFlag)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false; 
            }
        }
    }
}



