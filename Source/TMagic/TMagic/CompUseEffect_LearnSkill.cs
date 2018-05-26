using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace TorannMagic
{
    public class CompUseEffect_LearnSkill : CompUseEffect
    {
        public override void DoEffect(Pawn user)
        {
            CompAbilityUserMight comp = user.GetComp<CompAbilityUserMight>();
            MightPower mightPower;

            if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Faceless) || user.story.traits.HasTrait(TorannMagicDefOf.Gladiator) || user.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) || user.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper) || parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Ranger))))
            {
                if (parent.def.defName == "SkillOf_Sprint" && comp.skill_Sprint == false && !user.story.traits.HasTrait(TorannMagicDefOf.Gladiator))
                {
                    comp.skill_Sprint = true;
                    comp.AddPawnAbility(TorannMagicDefOf.TM_Sprint);
                    comp.InitializeSkill();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SkillOf_GearRepair" && comp.skill_GearRepair == false)
                {
                    comp.skill_GearRepair = true;
                    comp.AddPawnAbility(TorannMagicDefOf.TM_GearRepair);
                    comp.InitializeSkill();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SkillOf_InnerHealing" && comp.skill_InnerHealing == false)
                {
                    comp.skill_InnerHealing = true;
                    comp.AddPawnAbility(TorannMagicDefOf.TM_InnerHealing);
                    comp.InitializeSkill();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SkillOf_StrongBack" && comp.skill_StrongBack == false)
                {
                    comp.skill_StrongBack = true;
                    comp.AddPawnAbility(TorannMagicDefOf.TM_StrongBack);
                    comp.InitializeSkill();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SkillOf_HeavyBlow" && comp.skill_HeavyBlow == false)
                {
                    comp.skill_HeavyBlow = true;
                    comp.AddPawnAbility(TorannMagicDefOf.TM_HeavyBlow);
                    comp.InitializeSkill();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SkillOf_ThickSkin" && comp.skill_ThickSkin == false)
                {
                    comp.skill_ThickSkin = true;
                    comp.AddPawnAbility(TorannMagicDefOf.TM_ThickSkin);
                    comp.InitializeSkill();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SkillOf_FightersFocus" && comp.skill_FightersFocus == false)
                {
                    comp.skill_FightersFocus = true;
                    comp.AddPawnAbility(TorannMagicDefOf.TM_FightersFocus);
                    comp.InitializeSkill();
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else
                {
                    Messages.Message("CannotLearnSkill".Translate(), MessageTypeDefOf.RejectInput);
                }
            }
            else
            {
                Messages.Message("NotFighterToLearnSkill".Translate(), MessageTypeDefOf.RejectInput);
            }
        }
    }
}
