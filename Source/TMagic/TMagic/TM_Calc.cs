using UnityEngine;
using Verse;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using RimWorld;
using System;

namespace TorannMagic
{
    public static class TM_Calc
    {
        public static Vector3 GetVector(IntVec3 from, IntVec3 to)
        {
            Vector3 heading = (to - from).ToVector3();
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;
            return direction;
        }

        public static Pawn FindNearbyOtherPawn(Pawn pawn, int radius)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed)
                {
                    if (targetPawn != pawn && !targetPawn.HostileTo(pawn.Faction) && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                    {
                        pawnList.Add(targetPawn);
                        targetPawn = null;
                    }
                    else
                    {
                        targetPawn = null;
                    }
                }
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static Pawn FindNearbyPawn(Pawn pawn, int radius)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed)
                {
                    if (!targetPawn.HostileTo(pawn.Faction) && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                    {
                        pawnList.Add(targetPawn);
                        targetPawn = null;
                    }
                    else
                    {
                        targetPawn = null;
                    }
                }
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static Pawn FindNearbyMage(Pawn pawn, int radius, bool inCombat)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed)
                {
                    if(inCombat)
                    {
                        if (targetPawn.HostileTo(pawn.Faction) && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                        {
                            CompAbilityUserMagic targetComp = targetPawn.GetComp<CompAbilityUserMagic>();
                            if (targetComp != null && targetComp.IsMagicUser)
                            {
                                pawnList.Add(targetPawn);
                            }
                        }
                    }                    
                    else
                    {
                        if (!targetPawn.HostileTo(pawn.Faction) && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                        {
                            CompAbilityUserMagic targetComp = targetPawn.GetComp<CompAbilityUserMagic>();
                            if (targetComp != null && targetComp.IsMagicUser)
                            {
                                pawnList.Add(targetPawn);                                
                            }
                        }
                    }                    
                }
                targetPawn = null;
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static Pawn FindNearbyFighter(Pawn pawn, int radius, bool inCombat)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed)
                {
                    if (inCombat)
                    {
                        if (targetPawn.HostileTo(pawn.Faction) && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                        {
                            CompAbilityUserMight targetComp = targetPawn.GetComp<CompAbilityUserMight>();
                            if (targetComp != null && targetComp.IsMightUser)
                            {
                                pawnList.Add(targetPawn);
                            }
                        }
                    }
                    else
                    {
                        if (!targetPawn.HostileTo(pawn.Faction) && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                        {
                            CompAbilityUserMight targetComp = targetPawn.GetComp<CompAbilityUserMight>();
                            if (targetComp != null && targetComp.IsMightUser)
                            {
                                pawnList.Add(targetPawn);
                            }
                        }
                    }
                }
                targetPawn = null;
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static Pawn FindNearbyInjuredPawn(Pawn pawn, int radius, float minSeverity)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed)
                {
                    if (targetPawn.IsColonist && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                    {
                        float injurySeverity = 0;
                        using (IEnumerator<BodyPartRecord> enumerator = targetPawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                BodyPartRecord rec = enumerator.Current;
                                IEnumerable<Hediff_Injury> arg_BB_0 = targetPawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                                Func<Hediff_Injury, bool> arg_BB_1;
                                arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                                foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                                {
                                    bool flag5 = current.CanHealNaturally() && !current.IsPermanent();
                                    if (flag5)
                                    {
                                        injurySeverity += current.Severity;
                                    }                                        
                                }                                
                            }
                        }
                        if (minSeverity != 0)
                        {
                            if (injurySeverity >= minSeverity)
                            {
                                pawnList.Add(targetPawn);
                            }
                        }
                        else
                        {
                            if (injurySeverity != 0)
                            {
                                pawnList.Add(targetPawn);
                            }
                        }
                        targetPawn = null;
                    }
                    else
                    {
                        targetPawn = null;
                    }
                }
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static Pawn FindNearbyAfflictedPawn(Pawn pawn, int radius, List<string> validAfflictionDefnames)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed)
                {
                    if (targetPawn.IsColonist && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                    {
                        using (IEnumerator<Hediff> enumerator = targetPawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                Hediff rec = enumerator.Current;
                                if (rec.def.makesSickThought)
                                {
                                    for(int j =0; j < validAfflictionDefnames.Count; j++)
                                    {
                                        if (rec.def.defName == validAfflictionDefnames[j])
                                        {
                                            pawnList.Add(targetPawn);
                                        }
                                    }                                    
                                }
                            }
                        }
                        targetPawn = null;
                    }
                    else
                    {
                        targetPawn = null;
                    }
                }
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static Pawn FindNearbyEnemy(Pawn pawn, int radius)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed)
                {
                    if (targetPawn != pawn && targetPawn.HostileTo(pawn.Faction) && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                    {
                        pawnList.Add(targetPawn);
                        targetPawn = null;
                    }
                    else
                    {
                        targetPawn = null;
                    }
                }                
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static List<Pawn> FindPawnsNearTarget(Pawn pawn, int radius, IntVec3 targetCell, bool hostile)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed)
                {
                    if (targetPawn != pawn && (targetCell - targetPawn.Position).LengthHorizontal <= radius)
                    {
                        if (hostile && targetPawn.HostileTo(pawn.Faction))
                        {
                            pawnList.Add(targetPawn);
                        }
                        else if(!hostile && !targetPawn.HostileTo(pawn.Faction))
                        {
                            pawnList.Add(targetPawn);
                        }
                    }
                    targetPawn = null;                    
                }
            }
            if (pawnList.Count > 0)
            {
                return pawnList;
            }
            else
            {
                return null;
            }
        }
    }
}
