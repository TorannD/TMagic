using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace TorannMagic
{
    public class CompUseEffect_GemOfInsight : CompUseEffect
    {
        public override void DoEffect(Pawn user)
        {
            CompAbilityUserMight compMight = user.GetComp<CompAbilityUserMight>();
            CompAbilityUserMagic compMagic = user.GetComp<CompAbilityUserMagic>();

            if(!(compMagic.IsMagicUser || compMight.IsMightUser || user.story.traits.HasTrait(TorannMagicDefOf.Gifted) || user.story.traits.HasTrait(TorannMagicDefOf.PhysicalProdigy)))
            {
                
                if (parent.def != null && parent.def.defName == "GemstoneOfInsight_Magic")
                {
                    if (user.story.traits.allTraits.Count > 7)
                    {
                        int rnd = Rand.RangeInclusive(0, 6);
                        RemoveTrait(rnd, user.story.traits.allTraits);
                    }
                    user.story.traits.GainTrait(new Trait(TraitDef.Named("Gifted"), 2, false));
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else if(parent.def != null && parent.def.defName == "GemstoneOfInsight_Might")
                {
                    if (user.story.traits.allTraits.Count > 7)
                    {
                        int rnd = Rand.RangeInclusive(0, 6);
                        RemoveTrait(rnd, user.story.traits.allTraits);
                    }
                    user.story.traits.GainTrait(new Trait(TraitDef.Named("PhysicalProdigy"), 2, false));
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else
                {
                    Log.Message("TM_ItemUseFailed".Translate(new object[]
                    {
                        "Unrecognized Gemstone of Insight"
                    }));
                }                
            }
            else
            {
                Messages.Message("TM_CannotUseGemOfInsight".Translate(new object[]
                    {
                    user.LabelShort
                    }), MessageTypeDefOf.RejectInput);
            }
        }

        private void RemoveTrait(int index, List<Trait> traits)
        {
            traits.Remove(traits[index]);            
        }
    }
}
