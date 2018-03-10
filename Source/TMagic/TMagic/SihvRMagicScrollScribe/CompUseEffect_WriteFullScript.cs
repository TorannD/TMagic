using RimWorld;
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
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Necromancer))
            {
                tempPod = ThingDef.Named("BookOfUndead");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Priest))
            {
                tempPod = ThingDef.Named("BookOfPriest");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Gifted))
            {
                tempPod = ThingDef.Named("BookOfQuestion");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else
            {
                Messages.Message("NotGiftedPawn".Translate(), MessageTypeDefOf.RejectInput);
            }
            if (tempPod != null)
            {
                SihvSpawnThings.SpawnThingDefOfCountAt(tempPod, 1, new TargetInfo(currentPos, map));
            }
        }
    }
}
