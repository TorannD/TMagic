using RimWorld;
using Verse;

namespace TorannMagic.SihvRMagicScrollScribe
{
    public class CompUseEffect_WriteTornScript : CompUseEffect
    {

        public override void DoEffect(Pawn user)
        {
            ThingDef tempPod = null;
            IntVec3 currentPos = parent.PositionHeld;
            Map map = parent.Map;
            if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.InnerFire))
            {
                tempPod = ThingDef.Named("Torn_BookOfInnerFire");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
            {
                tempPod = ThingDef.Named("Torn_BookOfHeartOfFrost");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.StormBorn))
            {
                tempPod = ThingDef.Named("Torn_BookOfStormBorn");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Arcanist))
            {
                tempPod = ThingDef.Named("Torn_BookOfArcanist");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Paladin))
            {
                tempPod = ThingDef.Named("Torn_BookOfValiant");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Summoner))
            {
                tempPod = ThingDef.Named("Torn_BookOfSummoner");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Druid))
            {
                tempPod = ThingDef.Named("Torn_BookOfNature");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || user.story.traits.HasTrait(TorannMagicDefOf.Lich)))
            {
                tempPod = ThingDef.Named("Torn_BookOfUndead");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Priest))
            {
                tempPod = ThingDef.Named("Torn_BookOfPriest");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.TM_Bard))
            {
                tempPod = ThingDef.Named("Torn_BookOfBard");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Succubus) || user.story.traits.HasTrait(TorannMagicDefOf.Warlock)))
            {
                tempPod = ThingDef.Named("Torn_BookOfDemons");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Geomancer)))
            {
                tempPod = ThingDef.Named("Torn_BookOfEarth");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Technomancer)))
            {
                tempPod = TorannMagicDefOf.Torn_BookOfMagitech;
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.BloodMage)))
            {
                tempPod = TorannMagicDefOf.Torn_BookOfHemomancy;
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Enchanter)))
            {
                tempPod = TorannMagicDefOf.Torn_BookOfEnchanter;
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
