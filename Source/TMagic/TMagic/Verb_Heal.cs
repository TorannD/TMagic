using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using AbilityUser;
using Verse;
using UnityEngine;

namespace TorannMagic
{
    class Verb_Heal : Verb_UseAbility
    {

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
            // power affects enumerator
            DamageWorker.DamageResult result = DamageWorker.DamageResult.MakeNew();
            Pawn caster = this.CasterPawn;
            CompAbilityUserMagic comp = caster.GetComp<CompAbilityUserMagic>();
            MagicPowerSkill pwr = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Heal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Heal_pwr");
            MagicPowerSkill ver = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Heal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Heal_ver");
            if(caster.story.traits.HasTrait(TorannMagicDefOf.Priest))
            {
                pwr = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_AdvancedHeal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_AdvancedHeal_pwr");
                ver = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_AdvancedHeal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_AdvancedHeal_ver");
            }
            
            Pawn pawn = (Pawn)this.currentTarget;
            bool flag = pawn != null && !pawn.Dead;
            if (flag)
            {
                int num = 3 + ver.level;
                using (IEnumerator<BodyPartRecord> enumerator = pawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        BodyPartRecord rec = enumerator.Current;
                        bool flag2 = num > 0;
                        
                        if (flag2)
                        {
                            int num2 = 1 + ver.level;
                            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                            if (!this.CasterPawn.IsColonist && settingsRef.AIHardMode)
                            {
                                num2 = 5;
                            }
                            IEnumerable<Hediff_Injury> arg_BB_0 = pawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                            Func<Hediff_Injury, bool> arg_BB_1;

                            arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                            foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                            {
                                bool flag4 = num2 > 0;
                                if (flag4)
                                {
                                    bool flag5 = current.CanHealNaturally() && !current.IsOld();
                                    if (flag5)
                                    {
                                        //current.Heal((float)((int)current.Severity + 1));
                                        if (!this.CasterPawn.IsColonist)
                                        {
                                            current.Heal(20.0f + (float)pwr.level * 3f); // power affects how much to heal
                                        }
                                        else
                                        {
                                            current.Heal((5.0f + (float)pwr.level * 3f)*comp.arcaneDmg); // power affects how much to heal
                                        }
                                        TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3Shifted(), pawn.Map, .6f);
                                        TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3Shifted(), pawn.Map, .4f);
                                        num--;
                                        num2--;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }
    }
}
