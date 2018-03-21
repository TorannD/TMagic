using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace TorannMagic
{
    public class CompUseEffect_LearnSpell : CompUseEffect
    {
        public override void DoEffect(Pawn user)
        {
            CompAbilityUserMagic comp = user.GetComp<CompAbilityUserMagic>();
            MagicPower magicPower;

            if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Priest) || user.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || user.story.traits.HasTrait(TorannMagicDefOf.Druid) || parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Summoner) || user.story.traits.HasTrait(TorannMagicDefOf.InnerFire) || user.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost) || user.story.traits.HasTrait(TorannMagicDefOf.StormBorn) || user.story.traits.HasTrait(TorannMagicDefOf.Arcanist) || user.story.traits.HasTrait(TorannMagicDefOf.Paladin))))
            {
                if (parent.def.defName == "SpellOf_Rain" && comp.spell_Rain == false)
                {
                    comp.spell_Rain = true;
                    magicPower = comp.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Rainmaker);
                    comp.AddPawnAbility(TorannMagicDefOf.TM_Rainmaker);
                    magicPower.learned = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Blink" && comp.spell_Blink == false)
                {
                    comp.spell_Blink = true;
                    magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Blink);
                    comp.AddPawnAbility(TorannMagicDefOf.TM_Blink);
                    magicPower.learned = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Teleport" && comp.spell_Teleport == false)
                {
                    comp.spell_Teleport = true;
                    magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Teleport);
                    comp.AddPawnAbility(TorannMagicDefOf.TM_Teleport);
                    magicPower.learned = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Heal" && comp.spell_Heal == false)
                {
                    comp.spell_Heal = true;
                    magicPower = comp.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Heal);
                    comp.AddPawnAbility(TorannMagicDefOf.TM_Heal);
                    magicPower.learned = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Heater" && comp.spell_Heater == false)
                {
                    comp.spell_Heater = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Cooler" && comp.spell_Cooler == false)
                {
                    comp.spell_Cooler = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_PowerNode" && comp.spell_PowerNode == false)
                {
                    comp.spell_PowerNode = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Sunlight" && comp.spell_Sunlight == false)
                {
                    comp.spell_Sunlight = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_DryGround" && comp.spell_DryGround == false && user.story.traits.HasTrait(TorannMagicDefOf.InnerFire))
                {
                    comp.spell_DryGround = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Firestorm" && comp.spell_Firestorm == false && user.story.traits.HasTrait(TorannMagicDefOf.InnerFire))
                {
                    comp.spell_Firestorm = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_WetGround" && comp.spell_WetGround == false && user.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
                {
                    comp.spell_WetGround = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Blizzard" && comp.spell_Blizzard == false && user.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
                {
                    comp.spell_Blizzard = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_ChargeBattery" && comp.spell_ChargeBattery == false && user.story.traits.HasTrait(TorannMagicDefOf.StormBorn))
                {
                    comp.spell_ChargeBattery = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_SmokeCloud" && comp.spell_SmokeCloud == false)
                {
                    comp.spell_SmokeCloud = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Extinguish" && comp.spell_Extinguish == false)
                {
                    comp.spell_Extinguish = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_EMP" && comp.spell_EMP == false)
                {
                    comp.spell_EMP = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_SummonMinion" && comp.spell_SummonMinion == false)
                {
                    comp.spell_SummonMinion = true;
                    magicPower = comp.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonMinion);
                    comp.AddPawnAbility(TorannMagicDefOf.TM_SummonMinion);
                    magicPower.learned = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_TransferMana" && comp.spell_TransferMana == false)
                {
                    comp.spell_TransferMana = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_SiphonMana" && comp.spell_SiphonMana == false)
                {
                    comp.spell_SiphonMana = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_RegrowLimb" && comp.spell_RegrowLimb == false && user.story.traits.HasTrait(TorannMagicDefOf.Druid))
                {
                    comp.spell_RegrowLimb = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_EyeOfTheStorm" && comp.spell_EyeOfTheStorm == false && user.story.traits.HasTrait(TorannMagicDefOf.StormBorn))
                {
                    comp.spell_EyeOfTheStorm = true;
                    magicPower = comp.MagicData.MagicPowersSB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EyeOfTheStorm);
                    magicPower.learned = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_ManaShield" && comp.spell_ManaShield == false)
                {
                    comp.spell_ManaShield = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_FoldReality" && comp.spell_FoldReality == false && user.story.traits.HasTrait(TorannMagicDefOf.Arcanist))
                {
                    comp.spell_FoldReality = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Resurrection" && comp.spell_Resurrection == false && user.story.traits.HasTrait(TorannMagicDefOf.Priest))
                {
                    comp.spell_Resurrection = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_HolyWrath" && comp.spell_HolyWrath == false && user.story.traits.HasTrait(TorannMagicDefOf.Paladin))
                {
                    comp.spell_HolyWrath = true;
                    comp.InitializeSpell();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else
                {
                    Messages.Message("CannotLearnSpell".Translate(), MessageTypeDefOf.RejectInput);
                }
            }
            else
            {
                Messages.Message("NotMageToLearnSpell".Translate(), MessageTypeDefOf.RejectInput);
            }
        }
    }
}
