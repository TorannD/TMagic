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

        MagicPowerSkill pwr;
        MagicPowerSkill ver;

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

            CompAbilityUserMagic comp = this.CasterPawn.GetComp<CompAbilityUserMagic>();
            pwr = comp.MagicData.MagicPowerSkill_DeathMark.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_DeathMark_pwr");
            ver = comp.MagicData.MagicPowerSkill_DeathMark.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_DeathMark_ver");
                    

            if (this.currentTarget != null && base.CasterPawn != null)
            {
                Pawn p = this.CasterPawn;
                Map map = this.CasterPawn.Map;
                this.TargetsAoE.Clear();
                this.UpdateTargets();
                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                for(int i =0; i < this.TargetsAoE.Count; i++)
                {
                    if (this.TargetsAoE[i].Thing is Pawn)
                    {
                        Pawn victim = this.TargetsAoE[i].Thing as Pawn;
                        if (!p.IsColonistPlayerControlled && settingsRef.AIHardMode)
                        {
                            if (Rand.Chance(.4f + .1f * (pwr.level)))
                            {
                                HealthUtility.AdjustSeverity(victim, TorannMagicDefOf.TM_DeathMarkHD, 6 - (ver.level * 2));
                                TM_MoteMaker.ThrowPoisonMote(victim.Position.ToVector3(), victim.Map, 1.5f);
                                TM_MoteMaker.ThrowPoisonMote(victim.Position.ToVector3(), victim.Map, 1.5f);
                            }
                            else
                            {
                                MoteMaker.ThrowText(victim.Position.ToVector3Shifted(), victim.Map, "TM_ResistedSpell".Translate(), -1);
                            }
                        }
                        else
                        {
                            if (Rand.Chance(.2f + .1f * (pwr.level)))
                            {
                                HealthUtility.AdjustSeverity(victim, TorannMagicDefOf.TM_DeathMarkHD, 15 - (ver.level * 2));
                                TM_MoteMaker.ThrowPoisonMote(victim.Position.ToVector3(), victim.Map, 1.5f);
                                TM_MoteMaker.ThrowPoisonMote(victim.Position.ToVector3(), victim.Map, 1.5f);
                            }
                            else
                            {
                                MoteMaker.ThrowText(victim.Position.ToVector3Shifted(), victim.Map, "TM_ResistedSpell".Translate(), -1);
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

        public void DrawStrike(IntVec3 center, Vector3 strikePos, Map map)
        {
            TM_MoteMaker.ThrowCrossStrike(strikePos, map, 1f);
            TM_MoteMaker.ThrowBloodSquirt(strikePos, map, 1.5f);
        }
    }
}
