using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using AbilityUser;
using Verse;


namespace TorannMagic
{
    public class Verb_SootheAnimal : Verb_UseAbility
    {

        bool validTarg;
        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            if (targ.IsValid && targ.CenterVector3.InBounds(base.CasterPawn.Map) && !targ.Cell.Fogged(base.CasterPawn.Map))
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
            bool flag = false;
            this.TargetsAoE.Clear();
            this.UpdateTargets();
            int shotsPerBurst = this.ShotsPerBurst;
            MagicPowerSkill pwr = base.CasterPawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_SootheAnimal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SootheAnimal_pwr");
            MagicPowerSkill ver = base.CasterPawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_SootheAnimal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SootheAnimal_ver");
            bool flag2 = this.UseAbilityProps.AbilityTargetCategory != AbilityTargetCategory.TargetAoE && this.TargetsAoE.Count > 1;
            if (flag2)
            {
                this.TargetsAoE.RemoveRange(0, this.TargetsAoE.Count - 1);
            }
            for (int i = 0; i < this.TargetsAoE.Count; i++)
            {
                if (this.TargetsAoE[i].Thing.Faction != this.CasterPawn.Faction)
                {
                    Pawn newPawn = this.TargetsAoE[i].Thing as Pawn;
                    bool flag1 = newPawn.mindState.mentalStateHandler.CurStateDef != MentalStateDefOf.ManhunterPermanent;
                    if (flag1)
                    {
                        if(newPawn.kindDef.RaceProps.Animal)
                        {
                            newPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, true, false, null);
                            float sev = Rand.Range(pwr.level, 2 * pwr.level);
                            HealthUtility.AdjustSeverity(newPawn, TorannMagicDefOf.TM_Manipulation, sev);
                            sev = Rand.Range(pwr.level, 2 * pwr.level);
                            HealthUtility.AdjustSeverity(newPawn, TorannMagicDefOf.TM_Movement, sev);
                            sev = Rand.Range(pwr.level, 2 * pwr.level);
                            HealthUtility.AdjustSeverity(newPawn, TorannMagicDefOf.TM_Breathing, sev);
                            sev = Rand.Range(pwr.level, 2 * pwr.level);
                            HealthUtility.AdjustSeverity(newPawn, TorannMagicDefOf.TM_Sight, sev);
                            MoteMaker.ThrowMicroSparks(newPawn.Position.ToVector3().normalized, newPawn.Map);
                            if (pwr.level > 0)
                            {                                
                                TM_MoteMaker.ThrowManaPuff(newPawn.Position.ToVector3(), newPawn.Map, 1f);                                
                            }
                        }
                    }
                    if(!flag1)
                    {
                        if (newPawn.kindDef.RaceProps.Animal)
                        {
                            newPawn.mindState.mentalStateHandler.Reset();
                            MoteMaker.ThrowMicroSparks(newPawn.Position.ToVector3().normalized, newPawn.Map);
                            float sev = Rand.Range(pwr.level, 2 * pwr.level);
                            HealthUtility.AdjustSeverity(newPawn, TorannMagicDefOf.TM_AntiManipulation, sev);
                            sev = Rand.Range(pwr.level, 2 * pwr.level);
                            HealthUtility.AdjustSeverity(newPawn, TorannMagicDefOf.TM_AntiMovement, sev);
                            sev = Rand.Range(pwr.level, 2 * pwr.level);
                            HealthUtility.AdjustSeverity(newPawn, TorannMagicDefOf.TM_AntiBreathing, sev);
                            sev = Rand.Range(pwr.level, 2 * pwr.level);
                            HealthUtility.AdjustSeverity(newPawn, TorannMagicDefOf.TM_AntiSight, sev);
                            if (pwr.level > 0)
                            {
                                TM_MoteMaker.ThrowSiphonMote(newPawn.Position.ToVector3(), newPawn.Map, 1f);
                            }                    
                        }
                    }
                }
            }
            this.PostCastShot(flag, out flag);
            return flag;
        }
    }
}
