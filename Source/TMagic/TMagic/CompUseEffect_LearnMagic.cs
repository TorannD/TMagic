using RimWorld;
using Verse;
using System.Collections.Generic;
using UnityEngine;


namespace TorannMagic
{
	public class CompUseEffect_LearnMagic : CompUseEffect
	{

		public override void DoEffect(Pawn user)
		{
            if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Gifted))
            {
                Trait giftedTrait = new Trait();
                if (parent.def.defName == "BookOfInnerFire" || parent.def.defName == "Torn_BookOfInnerFire")
                {
                    FixTrait(user, user.story.traits.allTraits);
                    user.story.traits.GainTrait(new Trait(TraitDef.Named("InnerFire"), 4, false));
                    if(parent.def.defName == "BookOfInnerFire")
                    {
                        HealthUtility.AdjustSeverity(user, TorannMagicDefOf.TM_Uncertainty, 0.2f);
                    }
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "BookOfHeartOfFrost" || parent.def.defName == "Torn_BookOfHeartOfFrost")
                {
                    FixTrait(user, user.story.traits.allTraits);
                    user.story.traits.GainTrait(new Trait(TraitDef.Named("HeartOfFrost"), 4, false));
                    if (parent.def.defName == "BookOfHeartOfFrost")
                    {
                        HealthUtility.AdjustSeverity(user, TorannMagicDefOf.TM_Uncertainty, 0.2f);
                    }
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "BookOfStormBorn" || parent.def.defName == "Torn_BookOfStormBorn")
                {
                    FixTrait(user, user.story.traits.allTraits);
                    user.story.traits.GainTrait(new Trait(TraitDef.Named("StormBorn"), 4, false));
                    if (parent.def.defName == "BookOfStormBorn")
                    {
                        HealthUtility.AdjustSeverity(user, TorannMagicDefOf.TM_Uncertainty, 0.2f);
                    }
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "BookOfArcanist" || parent.def.defName == "Torn_BookOfArcanist")
                {
                    FixTrait(user, user.story.traits.allTraits);
                    user.story.traits.GainTrait(new Trait(TraitDef.Named("Arcanist"), 4, false));
                    if (parent.def.defName == "BookOfArcanist")
                    {
                        HealthUtility.AdjustSeverity(user, TorannMagicDefOf.TM_Uncertainty, 0.2f);
                    }
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "BookOfValiant" || parent.def.defName == "Torn_BookOfValiant")
                {
                    FixTrait(user, user.story.traits.allTraits);
                    user.story.traits.GainTrait(new Trait(TraitDef.Named("Paladin"), 4, false));
                    if (parent.def.defName == "BookOfValiant")
                    {
                        HealthUtility.AdjustSeverity(user, TorannMagicDefOf.TM_Uncertainty, 0.2f);
                    }
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "BookOfSummoner" || parent.def.defName == "Torn_BookOfSummoner")
                {
                    FixTrait(user, user.story.traits.allTraits);
                    user.story.traits.GainTrait(new Trait(TraitDef.Named("Summoner"), 4, false));
                    if (parent.def.defName == "BookOfSummoner")
                    {
                        HealthUtility.AdjustSeverity(user, TorannMagicDefOf.TM_Uncertainty, 0.2f);
                    }
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "BookOfDruid" || parent.def.defName == "Torn_BookOfNature")
                {
                    FixTrait(user, user.story.traits.allTraits);
                    user.story.traits.GainTrait(new Trait(TraitDef.Named("Druid"), 4, false));
                    if (parent.def.defName == "BookOfDruid")
                    {
                        HealthUtility.AdjustSeverity(user, TorannMagicDefOf.TM_Uncertainty, 0.2f);
                    }
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "BookOfNecromancer" || parent.def.defName == "Torn_BookOfUndead")
                {
                    FixTrait(user, user.story.traits.allTraits);
                    user.story.traits.GainTrait(new Trait(TraitDef.Named("Necromancer"), 4, false));
                    if (parent.def.defName == "BookOfNecromancer")
                    {
                        HealthUtility.AdjustSeverity(user, TorannMagicDefOf.TM_Uncertainty, 0.2f);
                    }
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "BookOfPriest" || parent.def.defName == "Torn_BookOfPriest")
                {
                    FixTrait(user, user.story.traits.allTraits);
                    user.story.traits.GainTrait(new Trait(TraitDef.Named("Priest"), 4, false));
                    if (parent.def.defName == "BookOfPriest")
                    {
                        HealthUtility.AdjustSeverity(user, TorannMagicDefOf.TM_Uncertainty, 0.2f);
                    }
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "BookOfQuestion")
                {
                    FixTrait(user, user.story.traits.allTraits);
                    int rnd = Mathf.RoundToInt(Rand.Range(0, 8));
                    switch (rnd)
                    {
                        case 1:
                            user.story.traits.GainTrait(new Trait(TraitDef.Named("Necromancer"), 4, false));
                            break;
                        case 2:
                            user.story.traits.GainTrait(new Trait(TraitDef.Named("Druid"), 4, false));
                            break;
                        case 3:
                            user.story.traits.GainTrait(new Trait(TraitDef.Named("Summoner"), 4, false));
                            break;
                        case 4:
                            user.story.traits.GainTrait(new Trait(TraitDef.Named("InnerFire"), 4, false));
                            break;
                        case 5:
                            user.story.traits.GainTrait(new Trait(TraitDef.Named("HeartOfFrost"), 4, false));
                            break;
                        case 6:
                            user.story.traits.GainTrait(new Trait(TraitDef.Named("StormBorn"), 4, false));
                            break;
                        case 7:
                            user.story.traits.GainTrait(new Trait(TraitDef.Named("Arcanist"), 4, false));
                            break;
                        case 8:
                            user.story.traits.GainTrait(new Trait(TraitDef.Named("Priest"), 4, false));
                            break;
                    }
                    //HealthUtility.AdjustSeverity(user, TorannMagicDefOf.TM_Uncertainty, 0.2f);
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else
                {
                    Messages.Message("NotArcaneBook".Translate(), MessageTypeDefOf.RejectInput);
                }

            }
            else
            {
                Messages.Message("NotGiftedPawn".Translate(new object[]
                    {
                        user.LabelShort
                    }), MessageTypeDefOf.RejectInput);
            }

		}

        private void FixTrait(Pawn pawn, List<Trait> traits)
        {
            for (int i = 0; i < traits.Count; i++)
            {
                if (traits[i].def.defName == "Gifted")
                {
                    traits.Remove(traits[i]);
                    i--;
                }
                
            }
        }
	}
}
