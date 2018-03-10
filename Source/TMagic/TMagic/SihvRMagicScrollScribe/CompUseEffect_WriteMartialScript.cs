using RimWorld;
using Verse;
using UnityEngine;

namespace TorannMagic.SihvRMagicScrollScribe
{
    public class CompUseEffect_WriteMartialScript : CompUseEffect
    {

        public override void DoEffect(Pawn user)
        {
            ThingDef tempPod = null;
            IntVec3 currentPos = parent.PositionHeld;
            Map map = parent.Map;
            if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Gladiator))
            {
                tempPod = ThingDef.Named("BookOfGladiator");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper))
            {
                tempPod = ThingDef.Named("BookOfSniper");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Bladedancer))
            {
                tempPod = ThingDef.Named("BookOfBladedancer");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Ranger))
            {
                tempPod = ThingDef.Named("BookOfRanger");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.PhysicalProdigy))
            {
                int rnd = Mathf.RoundToInt(Rand.Range(0, 4));
                switch (rnd)
                {
                    case 1:
                        {
                            tempPod = ThingDef.Named("BookOfGladiator");
                            break;
                        }
                    case 2:
                        {
                            tempPod = ThingDef.Named("BookOfSniper");
                            break;
                        }
                    case 3:
                        {
                            tempPod = ThingDef.Named("BookOfBladedancer");
                            break;
                        }
                    case 4:
                        {
                            tempPod = ThingDef.Named("BookOfRanger");
                            break;
                        }
                }
                this.parent.Destroy(DestroyMode.Vanish);
                }
            else
            {
                Messages.Message("NotPhyAdeptPawn".Translate(), MessageTypeDefOf.RejectInput);
            }
            if (tempPod != null)
            {
                SihvSpawnThings.SpawnThingDefOfCountAt(tempPod, 1, new TargetInfo(currentPos, map));
            }
        }
    }
}
