using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;
using AbilityUser;
using UnityEngine;
using Verse.AI.Group;

namespace TorannMagic
{
    public class Verb_Stoneskin : Verb_UseAbility  
    {
        
        int pwrVal;
        int verVal;
        CompAbilityUserMagic comp;

        bool validTarg;
        //Used for non-unique abilities that can be used with shieldbelt
        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            if (targ.Thing != null && targ.Thing == this.caster)
            {
                return this.verbProps.targetParams.canTargetSelf;
            }
            if (targ.IsValid && targ.CenterVector3.InBounds(base.CasterPawn.Map) && !targ.Cell.Fogged(base.CasterPawn.Map) && targ.Cell.Walkable(base.CasterPawn.Map))
            {
                if ((root - targ.Cell).LengthHorizontal < this.verbProps.range)
                {
                    ShootLine shootLine;
                    validTarg = this.TryFindShootLineFromTo(root, targ, out shootLine);
                }
                else
                {
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
            Pawn caster = base.CasterPawn;
            Pawn pawn = this.currentTarget.Thing as Pawn;

            comp = caster.GetComp<CompAbilityUserMagic>();
            MagicPowerSkill pwr = comp.MagicData.MagicPowerSkill_Stoneskin.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Stoneskin_pwr");
            MagicPowerSkill ver = comp.MagicData.MagicPowerSkill_Stoneskin.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Stoneskin_ver");
            pwrVal = pwr.level;
            verVal = ver.level;
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (caster.story.traits.HasTrait(TorannMagicDefOf.Faceless))
            {
                pwrVal = caster.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_pwr").level;
                verVal = caster.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_ver").level;
            }
            if (settingsRef.AIHardMode && !caster.IsColonist)
            {
                pwrVal = 3;
                verVal = 3;
            }

            if (pawn != null)
            {
                if(pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_StoneskinHD"), false))
                {
                    RemoveHediffs(pawn);
                    comp.stoneskinPawns.Remove(pawn);
                    SoundInfo info = SoundInfo.InMap(new TargetInfo(pawn.Position, pawn.Map, false), MaintenanceType.None);
                    info.pitchFactor = .7f;
                    SoundDefOf.EnergyShield_Broken.PlayOneShot(info);
                    MoteMaker.ThrowLightningGlow(pawn.DrawPos, pawn.Map, 1.5f);
                }
                else
                {
                    if (comp.stoneskinPawns.Count() < verVal + 2)
                    {
                        ApplyHediffs(pawn);
                        comp.stoneskinPawns.Add(pawn);
                        SoundInfo info = SoundInfo.InMap(new TargetInfo(pawn.Position, pawn.Map, false), MaintenanceType.None);
                        info.pitchFactor = .7f;
                        SoundDefOf.EnergyShield_Reset.PlayOneShot(info);
                        MoteMaker.ThrowLightningGlow(pawn.DrawPos, pawn.Map, 1.5f);
                        Effecter stoneskinEffecter = TorannMagicDefOf.TM_Stoneskin_Effecter.Spawn();
                        stoneskinEffecter.def.offsetTowardsTarget = FloatRange.Zero;
                        stoneskinEffecter.Trigger(new TargetInfo(pawn.Position, pawn.Map, false), new TargetInfo(pawn.Position, pawn.Map, false));
                        stoneskinEffecter.Cleanup();
                    }
                    else
                    {
                        string stoneskinPawns = "";
                        int count = comp.stoneskinPawns.Count();
                        for(int i = 0; i < count; i++)
                        {
                            if (i + 1 == count) //last name
                            {
                                stoneskinPawns += comp.stoneskinPawns[i].LabelShort;
                            }
                            else
                            {
                                stoneskinPawns += comp.stoneskinPawns[i].LabelShort + " & ";
                            }
                        }
                        Messages.Message("TM_TooManyStoneskins".Translate(new object[]
                            {
                                caster.LabelShort,
                                verVal + 2,
                                stoneskinPawns
                            }), MessageTypeDefOf.RejectInput);
                    }
                }
            }
            
            return true;
        }

        private void ApplyHediffs(Pawn target)
        {
            if (pwrVal == 3)
            {
                HealthUtility.AdjustSeverity(target, HediffDef.Named("TM_StoneskinHD"), 10);
            }
            else if (pwrVal == 2)
            {
                HealthUtility.AdjustSeverity(target, HediffDef.Named("TM_StoneskinHD"), 9);
            }
            else if(pwrVal == 1)
            {
                HealthUtility.AdjustSeverity(target, HediffDef.Named("TM_StoneskinHD"), 8);
            }
            else
            {
                HealthUtility.AdjustSeverity(target, HediffDef.Named("TM_StoneskinHD"), 6);                
            }            
        }

        private void RemoveHediffs(Pawn target)
        {
            Hediff hediff = new Hediff();
            hediff = target.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_StoneskinHD"));
            target.health.RemoveHediff(hediff);
        }
    }
}
