using System;
using System.Linq;
using Verse;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;

namespace TorannMagic.Conditions
{
    public class IncidentWorker_WanderingLich : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (settingsRef.wanderingLichChallenge > 0)
            {
                Map map = (Map)parms.target;
                int duration = Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f);
                TM_Action.ForceFactionDiscoveryAndRelation(TorannMagicDefOf.TM_SkeletalFaction);
                GameCondition_WanderingLich gameCondition_WanderingLich = (GameCondition_WanderingLich)GameConditionMaker.MakeCondition(GameConditionDef.Named("WanderingLich"), duration);
                map.gameConditionManager.RegisterCondition(gameCondition_WanderingLich);
                base.SendStandardLetter(parms, null, "");
                //base.SendStandardLetter(new TargetInfo(gameCondition_WanderingLich.edgeLocation.ToIntVec3, map, false), null, new string[0]);
                List<Faction> lichFaction = Find.FactionManager.AllFactions.ToList();
                bool factionFlag = false;
                for (int i = 0; i < lichFaction.Count; i++)
                {
                    if (lichFaction[i].def == TorannMagicDefOf.TM_SkeletalFaction)
                    {
                        Faction.OfPlayer.TrySetRelationKind(lichFaction[i], FactionRelationKind.Hostile, false, null, null);
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