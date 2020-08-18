using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using AbilityUser;
using Verse;
using UnityEngine;
using Verse.Sound;
using Verse.AI;


namespace TorannMagic
{
    public class Verb_Taunt : Verb_UseAbility
    {
        float radius = 15f;
        float tauntChance = .6f;
        int targetsMax = 5;

        protected override bool TryCastShot()
        {
            Pawn caster = base.CasterPawn;
            Pawn pawn = this.currentTarget.Thing as Pawn;

            bool flag = pawn != null && !pawn.Dead;
            if (flag)
            {

                CompAbilityUserMight comp = caster.GetComp<CompAbilityUserMight>();
                int verVal = TM_Calc.GetMightSkillLevel(caster, comp.MightData.MightPowerSkill_Custom, "TM_Taunt", "_ver", true);
                int pwrVal = TM_Calc.GetMightSkillLevel(caster, comp.MightData.MightPowerSkill_Custom, "TM_Taunt", "_pwr", true);
                radius += (2f * verVal);
                tauntChance += (pwrVal * .05f);
                targetsMax += pwrVal;

                SoundInfo info = SoundInfo.InMap(new TargetInfo(caster.Position, caster.Map, false), MaintenanceType.None);
                if(this.CasterPawn.gender == Gender.Female)
                {
                    info.pitchFactor = Rand.Range(1.1f, 1.3f); 
                }
                else
                {
                    info.pitchFactor = Rand.Range(.7f, .9f);
                }
                TorannMagicDefOf.TM_Roar.PlayOneShot(info);
                Effecter RageWave = TorannMagicDefOf.TM_RageWaveED.Spawn();
                RageWave.Trigger(new TargetInfo(caster.Position, caster.Map, false), new TargetInfo(caster.Position, caster.Map, false));
                RageWave.Cleanup();
                SearchAndTaunt();
                
            }

            return true;
        }

        public void SearchAndTaunt()
        {
            List<Pawn> mapPawns = this.caster.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> tauntTargets = new List<Pawn>();
            tauntTargets.Clear();
            if (mapPawns != null && mapPawns.Count > 0)
            {
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    Pawn victim = mapPawns[i];
                    if (!victim.DestroyedOrNull() && !victim.Dead && victim.Map != null && !victim.Downed && victim.mindState != null && !victim.InMentalState && victim.jobs != null)
                    {
                        if (caster.Faction.HostileTo(victim.Faction) && (victim.Position - caster.Position).LengthHorizontal < this.radius)
                        {
                            tauntTargets.Add(victim);
                        }
                    }
                    if(tauntTargets.Count >= targetsMax)
                    {
                        break;
                    }
                }
                for(int i = 0; i < tauntTargets.Count; i++)
                {
                    if (Rand.Chance(tauntChance))
                    {

                        //Log.Message("taunting " + threatPawns[i].LabelShort + " doing job " + threatPawns[i].CurJobDef.defName + " with follow radius of " + threatPawns[i].CurJob.followRadius);
                        if (tauntTargets[i].CurJobDef == JobDefOf.Follow || tauntTargets[i].CurJobDef == JobDefOf.FollowClose)
                        {
                            Job job = new Job(JobDefOf.AttackMelee, this.CasterPawn);
                            tauntTargets[i].jobs.TryTakeOrderedJob(job, JobTag.Misc);
                        }
                        HealthUtility.AdjustSeverity(tauntTargets[i], TorannMagicDefOf.TM_TauntHD, 1);
                        Hediff hd = tauntTargets[i].health?.hediffSet?.GetFirstHediffOfDef(TorannMagicDefOf.TM_TauntHD);
                        HediffComp_Disappears comp_d = hd.TryGetComp<HediffComp_Disappears>();
                        if(comp_d != null)
                        {
                            comp_d.ticksToDisappear = 600;
                        }
                        HediffComp_Taunt comp_t = hd.TryGetComp<HediffComp_Taunt>();
                        if(comp_t != null)
                        {
                            comp_t.tauntTarget = CasterPawn;
                        }
                        MoteMaker.ThrowText(tauntTargets[i].DrawPos, tauntTargets[i].Map, "Taunted!", -1);
                    }
                    else
                    {
                        MoteMaker.ThrowText(tauntTargets[i].DrawPos, tauntTargets[i].Map, "TM_ResistedSpell".Translate(), -1);
                    }
                }
            }            
        }
    }
}