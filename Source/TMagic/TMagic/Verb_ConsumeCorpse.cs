using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using AbilityUser;
using Verse;
using UnityEngine;

namespace TorannMagic
{
    class Verb_ConsumeCorpse : Verb_UseAbility
    {

        bool validTarg;
        //Used specifically for non-unique verbs that ignore LOS (can be used with shield belt)
        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            if (targ.IsValid && targ.CenterVector3.InBounds(base.CasterPawn.Map) && !targ.Cell.Fogged(base.CasterPawn.Map) && targ.Cell.Walkable(base.CasterPawn.Map))
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

            Pawn caster = this.CasterPawn;
            CompAbilityUserMagic comp = caster.GetComp<CompAbilityUserMagic>();
            MagicPowerSkill ver = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_ConsumeCorpse.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ConsumeCorpse_ver");
            MagicPowerSkill manaRegen = caster.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_global_regen.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_regen_pwr");

            Thing undeadThing = this.currentTarget.Thing;
            if (undeadThing is Pawn)
            {
                Pawn undead = (Pawn)undeadThing;

                bool flag = undead != null && !undead.Dead;
                if (flag)
                {
                    if (undead.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadHD))
                    {
                        comp.Mana.CurLevel += (Rand.Range(.20f, .25f) + (manaRegen.level * .006f) + (ver.level * .01f));
                        ConsumeHumanoid(undead);
                        if (ver.level > 0)
                        {
                            HealCaster(caster, 2 + ver.level, 2, 5f + ver.level);
                        }
                        undead.Destroy();
                    }
                    else if (undead.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadAnimalHD))
                    {
                        comp.Mana.CurLevel += (Rand.Range(.16f, .20f) + (manaRegen.level * .006f) + (ver.level * .01f));
                        ConsumeAnimalKind(undead);
                        if (ver.level > 0)
                        {
                            HealCaster(caster, 2, 2, 2 + ver.level);
                        }
                        undead.Destroy();
                    }
                    else
                    {
                        Messages.Message("TM_CannotUseOnLiving".Translate(), MessageTypeDefOf.RejectInput);
                    }
                }
            }

            
            IntVec3 target = this.currentTarget.Cell;
            Thing corpseThing = null;
            Corpse corpse = null;
            List<Thing> thingList;
            thingList = target.GetThingList(caster.Map);
            int i=0;
            while(i < thingList.Count)
            {
                corpseThing = thingList[i];
                if (corpseThing != null)
                {
                    bool validator = corpseThing is Corpse;
                    if (validator)
                    {
                        corpse = corpseThing as Corpse;
                        Pawn undeadPawn = corpse.InnerPawn;
                        if (undeadPawn.RaceProps.IsFlesh)
                        {
                            if (undeadPawn.RaceProps.Humanlike && !undeadPawn.RaceProps.Animal)
                            {
                                if (!corpse.IsNotFresh())
                                {
                                    comp.Mana.CurLevel += (Rand.Range(.11f, .15f) + (manaRegen.level * .003f) + (ver.level * .006f));
                                    caster.needs.rest.CurLevel += .3f;
                                    caster.needs.mood.CurLevel += .3f;
                                    ConsumeHumanoid(corpse);
                                    if (ver.level > 0)
                                    {
                                        HealCaster(caster, 1 + ver.level, 1 + ver.level, 2f + ver.level);
                                    }
                                }
                                else
                                {
                                    comp.Mana.CurLevel += (Rand.Range(.07f, .11f) + (manaRegen.level * .003f) + (ver.level * .006f));
                                    ConsumeHumanoid(corpse);
                                }
                                corpse.Destroy();
                            }
                            else if (undeadPawn.RaceProps.Animal)
                            {
                                if (!corpse.IsNotFresh())
                                {
                                    comp.Mana.CurLevel += (Rand.Range(.07f, .11f) + (manaRegen.level * .002f) + (ver.level * .005f));
                                    caster.needs.food.CurLevel += .4f;
                                    ConsumeAnimalKind(corpse);
                                    if (ver.level > 0)
                                    {
                                        HealCaster(caster, 1, 1, 2f + ver.level);
                                    }
                                }
                                else
                                {
                                    comp.Mana.CurLevel += (Rand.Range(.05f, .09f) + (manaRegen.level * .002f) + (ver.level * .005f));
                                    ConsumeAnimalKind(corpse);
                                }
                                corpse.Destroy();
                            }
                            else
                            {
                                Messages.Message("TM_CannontConsumeCorpseType".Translate(), MessageTypeDefOf.RejectInput);
                            }
                        }
                        else
                        {
                            Messages.Message("TM_InvalidCorpseType".Translate(), MessageTypeDefOf.RejectInput);
                        }
                    }
                }
                i++;
            }        
            
            return false;
        }

        public void ConsumeHumanoid(Pawn undead)
        {            
            TM_MoteMaker.ThrowSiphonMote(undead.Position.ToVector3Shifted(), undead.Map, 1.2f);
            TM_MoteMaker.ThrowBloodSquirt(undead.Position.ToVector3Shifted(), undead.Map, 1f);
            TM_MoteMaker.ThrowBloodSquirt(undead.Position.ToVector3Shifted(), undead.Map, 1.2f);
            TM_MoteMaker.ThrowManaPuff(caster.Position.ToVector3Shifted(), caster.Map, 1f);
            undead.inventory.DropAllNearPawn(undead.Position, false, true);
            undead.equipment.DropAllEquipment(undead.Position, false);
            undead.apparel.DropAll(undead.Position, false);           
        }

        public void ConsumeAnimalKind(Pawn undead)
        {
            TM_MoteMaker.ThrowSiphonMote(undead.Position.ToVector3Shifted(), undead.Map, .8f);
            TM_MoteMaker.ThrowBloodSquirt(undead.Position.ToVector3Shifted(), undead.Map, 1.5f);
            TM_MoteMaker.ThrowManaPuff(caster.Position.ToVector3Shifted(), caster.Map, .8f);
        }

        public void ConsumeHumanoid(Corpse corpse)
        {
            TM_MoteMaker.ThrowSiphonMote(corpse.Position.ToVector3Shifted(), corpse.Map, 1.2f);
            TM_MoteMaker.ThrowBloodSquirt(corpse.Position.ToVector3Shifted(), corpse.Map, 1f);
            TM_MoteMaker.ThrowBloodSquirt(corpse.Position.ToVector3Shifted(), corpse.Map, 1.2f);
            TM_MoteMaker.ThrowManaPuff(caster.Position.ToVector3Shifted(), caster.Map, 1f);
            Pawn corpsePawn = corpse.InnerPawn;
            corpsePawn.inventory.DropAllNearPawn(corpse.Position, false, true);
            corpsePawn.equipment.DropAllEquipment(corpse.Position, false);
            corpsePawn.apparel.DropAll(corpse.Position, false);
        }

        public void ConsumeAnimalKind(Corpse corpse)
        {
            TM_MoteMaker.ThrowSiphonMote(corpse.Position.ToVector3Shifted(), corpse.Map, .8f);
            TM_MoteMaker.ThrowBloodSquirt(corpse.Position.ToVector3Shifted(), corpse.Map, 1.5f);
            TM_MoteMaker.ThrowManaPuff(caster.Position.ToVector3Shifted(), caster.Map, .8f);
        }

        public void HealCaster(Pawn caster, int num, int num2, float healAmt)
        {
            if (num > 0)
            {
                using (IEnumerator<BodyPartRecord> enumerator = caster.health.hediffSet.GetInjuredParts().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        BodyPartRecord rec = enumerator.Current;
                        bool flag2 = num > 0;

                        if (flag2)
                        {
                            IEnumerable<Hediff_Injury> arg_BB_0 = caster.health.hediffSet.GetHediffs<Hediff_Injury>();
                            Func<Hediff_Injury, bool> arg_BB_1;

                            arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                            foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                            {
                                bool flag3 = num2 > 0;
                                if (flag3)
                                {
                                    bool flag5 = current.CanHealNaturally() && !current.IsOld();
                                    if (flag5)
                                    {
                                        current.Heal(healAmt);
                                        num--;
                                        num2--;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
