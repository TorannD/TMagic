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
    public class Verb_TimeMark : Verb_UseAbility  
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
                SetRecallHediffs();
                SetRecallNeeds();
                SetRecallPosition();
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

        private void SetRecallHediffs()
        {
            comp.recallHediffList = new List<Hediff>();
            comp.recallHediffList.Clear();
            comp.recallInjuriesList = new List<Hediff_Injury>();
            comp.recallInjuriesList.Clear();
            for (int i = 0; i < this.CasterPawn.health.hediffSet.hediffs.Count; i++)
            {
                if (!this.CasterPawn.health.hediffSet.hediffs[i].IsPermanent() && this.CasterPawn.health.hediffSet.hediffs[i].def != TorannMagicDefOf.TM_MagicUserHD && !this.CasterPawn.health.hediffSet.hediffs[i].def.defName.Contains("TM_HediffEnchantment") && !this.CasterPawn.health.hediffSet.hediffs[i].def.defName.Contains("TM_Artifact"))
                {
                    if (this.CasterPawn.health.hediffSet.hediffs[i] is Hediff_Injury)
                    {                        
                        Hediff_Injury rhd = this.CasterPawn.health.hediffSet.hediffs[i] as Hediff_Injury;
                        Hediff_Injury hediff = new Hediff_Injury();
                        //hediff = TM_Calc.Clone<Hediff>(this.CasterPawn.health.hediffSet.hediffs[i]);
                        hediff.def = rhd.def;
                        hediff.Part = rhd.Part;
                        Traverse.Create(root: hediff).Field(name: "visible").SetValue(rhd.Visible);
                        Traverse.Create(root: hediff).Field(name: "severityInt").SetValue(rhd.Severity);
                        //hediff.Severity = rhd.Severity;                        
                        hediff.ageTicks = rhd.ageTicks;
                        comp.recallInjuriesList.Add(hediff);
                    }
                    else if(this.CasterPawn.health.hediffSet.hediffs[i] is Hediff_MissingPart || this.CasterPawn.health.hediffSet.hediffs[i] is Hediff_AddedPart)
                    {
                        //do nothing
                    }
                    else
                    {
                        Hediff rhd = this.CasterPawn.health.hediffSet.hediffs[i];
                        //Log.Message("sev def is " + rhd.def.defName);
                        Hediff hediff = new Hediff();
                        //hediff = TM_Calc.Clone<Hediff>(this.CasterPawn.health.hediffSet.hediffs[i]);
                        hediff.def = rhd.def;
                        hediff.Part = rhd.Part;
                        Traverse.Create(root: hediff).Field(name: "visible").SetValue(rhd.Visible);
                        Traverse.Create(root: hediff).Field(name: "severityInt").SetValue(rhd.Severity);
                        hediff.Severity = rhd.Severity;
                        hediff.ageTicks = rhd.ageTicks;
                        
                        comp.recallHediffList.Add(hediff);
                    }
                    //Log.Message("adding " + this.CasterPawn.health.hediffSet.hediffs[i].def + " at severity " + this.CasterPawn.health.hediffSet.hediffs[i].Severity);
                }
            }
            //Log.Message("hediffs set");
        }

        private void SetRecallNeeds()
        {
            comp.recallNeedDefnames = new List<string>();
            comp.recallNeedDefnames.Clear();
            comp.recallNeedValues = new List<float>();
            comp.recallNeedValues.Clear();
            //comp.recallNeedValues = new List<Need>();
            //comp.recallNeedValues.Clear();
            for (int i = 0; i < this.CasterPawn.needs.AllNeeds.Count; i++)
            {
                comp.recallNeedDefnames.Add(this.CasterPawn.needs.AllNeeds[i].def.defName);
                comp.recallNeedValues.Add(this.CasterPawn.needs.AllNeeds[i].CurLevel);
                //comp.recallNeedValues.Add(TM_Calc.Clone<Need>(this.CasterPawn.needs.AllNeeds[i]));
            }
            //Log.Message("needs set");
        }

        private void SetRecallPosition()
        {
            comp.recallPosition = this.CasterPawn.Position;
            comp.recallMap = this.CasterPawn.Map;
            //Log.Message("position set");
        }
    }
}
