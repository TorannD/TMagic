using RimWorld;
using Verse;
using AbilityUser;
using UnityEngine;
using System.Linq;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    class Verb_DeathMark : Verb_UseAbility  
    {

        private int verVal;
        private int pwrVal;

        bool validTarg;

        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            if (targ.IsValid && targ.CenterVector3.InBounds(base.CasterPawn.Map) && !targ.Cell.Fogged(base.CasterPawn.Map) && targ.Cell.Walkable(base.CasterPawn.Map))
            {
                if ((root - targ.Cell).LengthHorizontal < this.verbProps.range)
                {
                    //out of range
                    validTarg = true;
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
            bool result = false;
            Pawn p = this.CasterPawn;
            CompAbilityUserMagic comp = this.CasterPawn.GetComp<CompAbilityUserMagic>();
            MagicPowerSkill pwr = comp.MagicData.MagicPowerSkill_DeathMark.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_DeathMark_pwr");
            MagicPowerSkill ver = comp.MagicData.MagicPowerSkill_DeathMark.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_DeathMark_ver");
            verVal = ver.level;
            pwrVal = pwr.level;
            if (p.story.traits.HasTrait(TorannMagicDefOf.Faceless))
            {
                MightPowerSkill mpwr = p.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_pwr");
                MightPowerSkill mver = p.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_ver");
                pwrVal = mpwr.level;
                verVal = mver.level;
            }

            if (this.currentTarget != null && base.CasterPawn != null)
            {
                
                Map map = this.CasterPawn.Map;
                this.TargetsAoE.Clear();
                this.UpdateTargets();
                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                for(int i =0; i < this.TargetsAoE.Count; i++)
                {
                    if (this.TargetsAoE[i].Thing is Pawn)
                    {
                        Pawn victim = this.TargetsAoE[i].Thing as Pawn;
                        if(!victim.RaceProps.IsMechanoid)
                        {
                            HealthUtility.AdjustSeverity(victim, HediffDef.Named("TM_DeathMarkCurse"), Rand.Range(1f + pwrVal, 4 + 2 * pwrVal));
                            TM_MoteMaker.ThrowSiphonMote(victim.DrawPos, victim.Map, 1f);
                            if (comp.Pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_LichHD"), false))
                            {
                                comp.PowerModifier += 1;
                            }                            

                            if (Rand.Chance(verVal * .2f))
                            {
                                if (Rand.Chance(verVal * .1f)) //terror
                                {
                                    HealthUtility.AdjustSeverity(victim, HediffDef.Named("TM_Terror"), Rand.Range(3f * verVal, 5f * verVal));
                                    TM_MoteMaker.ThrowDiseaseMote(victim.DrawPos, victim.Map, 1f, .5f, .2f, .4f);
                                    MoteMaker.ThrowText(victim.DrawPos, victim.Map, "Terror", -1);
                                }
                                if (Rand.Chance(verVal * .1f)) //berserk
                                {
                                    if (victim.mindState != null && victim.RaceProps != null && victim.RaceProps.Humanlike)
                                    {
                                        victim.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, "cursed", true, false, null);
                                        MoteMaker.ThrowMicroSparks(victim.DrawPos, victim.Map);
                                        MoteMaker.ThrowText(victim.DrawPos, victim.Map, "Berserk", -1);
                                    }

                                }
                            }
                        }
                    }
                }

                result = true;
            }

            this.burstShotsLeft = 0;
            //this.ability.TicksUntilCasting = (int)base.UseAbilityProps.SecondsToRecharge * 60;
            return result;
        }        
    }
}
