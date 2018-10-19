﻿using RimWorld;
using Verse;

namespace TorannMagic.SihvRMagicScrollScribe
{
    public class CompUseEffect_WriteFullScript : CompUseEffect
    {

        public override void DoEffect(Pawn user)
        {
            ThingDef tempPod = null;
            IntVec3 currentPos = parent.PositionHeld;
            Map map = parent.Map;
            if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.InnerFire))
            {
                tempPod = ThingDef.Named("BookOfInnerFire");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
            {
                tempPod = ThingDef.Named("BookOfHeartOfFrost");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.StormBorn))
            {
                tempPod = ThingDef.Named("BookOfStormBorn");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Arcanist))
            {
                tempPod = ThingDef.Named("BookOfArcanist");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Paladin))
            {
                tempPod = ThingDef.Named("BookOfValiant");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Summoner))
            {
                tempPod = ThingDef.Named("BookOfSummoner");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Druid))
            {
                tempPod = ThingDef.Named("BookOfNature");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || user.story.traits.HasTrait(TorannMagicDefOf.Lich)))
            {
                tempPod = ThingDef.Named("BookOfNecromancer");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Priest))
            {
                tempPod = ThingDef.Named("BookOfPriest");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.TM_Bard))
            {
                tempPod = ThingDef.Named("BookOfBard");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Succubus) || user.story.traits.HasTrait(TorannMagicDefOf.Warlock)))
            {
                tempPod = ThingDef.Named("BookOfDemons");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Geomancer)))
            {
                tempPod = ThingDef.Named("BookOfEarth");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Gifted))
            {
                tempPod = ThingDef.Named("BookOfQuestion");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else
            {
                Messages.Message("NotGiftedPawn".Translate(
                        user.LabelShort
                    ), MessageTypeDefOf.RejectInput);
            }
            if (tempPod != null)
            {
                SihvSpawnThings.SpawnThingDefOfCountAt(tempPod, 1, new TargetInfo(currentPos, map));
            }
        }
    }
}
