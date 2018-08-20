using RimWorld;
using Verse;
using System.Collections.Generic;


namespace TorannMagic
{
	public class CompUseEffect_LearnMight : CompUseEffect
	{

		public override void DoEffect(Pawn user)
		{
            if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.PhysicalProdigy))
            {
                Trait giftedTrait = new Trait();
                if (parent.def.defName == "BookOfGladiator")
                {
                    FixTrait(user, user.story.traits.allTraits);
                    user.story.traits.GainTrait(new Trait(TraitDef.Named("Gladiator"), 4, false));
                    this.parent.Destroy(DestroyMode.Vanish);
                    CompAbilityUserMight comp = user.GetComp<CompAbilityUserMight>();
                    comp.skill_Sprint = true;
                }
                else if (parent.def.defName == "BookOfSniper")
                {
                    FixTrait(user, user.story.traits.allTraits);
                    user.story.traits.GainTrait(new Trait(TraitDef.Named("TM_Sniper"), 0, false));
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "BookOfBladedancer")
                {
                    FixTrait(user, user.story.traits.allTraits);
                    user.story.traits.GainTrait(new Trait(TraitDef.Named("Bladedancer"), 0, false));
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "BookOfRanger")
                {
                    FixTrait(user, user.story.traits.allTraits);
                    user.story.traits.GainTrait(new Trait(TraitDef.Named("Ranger"), 0, false));
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "BookOfFaceless")
                {
                    FixTrait(user, user.story.traits.allTraits);
                    user.story.traits.GainTrait(new Trait(TraitDef.Named("Faceless"), 4, false));
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "BookOfPsionic")
                {
                    if (user.GetStatValue(StatDefOf.PsychicSensitivity, false) > 1)
                    {
                        FixTrait(user, user.story.traits.allTraits);
                        user.story.traits.GainTrait(new Trait(TraitDef.Named("TM_Psionic"), 4, false));
                        this.parent.Destroy(DestroyMode.Vanish);
                    }
                    else if(user.Map.GameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicDrone) || user.Map.GameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicSoothe))
                    {
                        FixTrait(user, user.story.traits.allTraits);
                        user.story.traits.GainTrait(new Trait(TraitDef.Named("TM_Psionic"), 4, false));
                        this.parent.Destroy(DestroyMode.Vanish);
                    }
                    else
                    {
                        Messages.Message("TM_NotPsychicSensitive".Translate(new object[]
                        {
                            user.LabelShort
                        }), MessageTypeDefOf.RejectInput);
                    }
                }
                else
                {
                    Messages.Message("NotCombatBook".Translate(), MessageTypeDefOf.RejectInput);
                }

            }
            else
            {
                Messages.Message("NotPhyAdeptPawn".Translate(), MessageTypeDefOf.RejectInput);
            }

		}

        private void FixTrait(Pawn pawn, List<Trait> traits)
        {
            for (int i = 0; i < traits.Count; i++)
            {
                if (traits[i].def.defName == "PhysicalProdigy")
                {
                    traits.Remove(traits[i]);
                    i--;
                }                
            }
        }
	}
}
