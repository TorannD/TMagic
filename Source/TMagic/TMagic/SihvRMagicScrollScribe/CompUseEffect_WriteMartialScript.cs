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
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Faceless))
            {
                tempPod = ThingDef.Named("BookOfFaceless");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic))
            {
                tempPod = ThingDef.Named("BookOfPsionic");
                this.parent.Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.PhysicalProdigy))
            {
                float rnd = Rand.Range(0, 6);
                if(rnd < 1)
                {
                    tempPod = ThingDef.Named("BookOfGladiator");
                }
                else if (rnd < 2)
                {
                    tempPod = ThingDef.Named("BookOfSniper");
                }
                else if (rnd < 3)
                {
                    tempPod = ThingDef.Named("BookOfBladedancer");
                }
                else if (rnd < 4)
                {
                    tempPod = ThingDef.Named("BookOfRanger");
                }
                else if (rnd < 5)
                {
                    tempPod = ThingDef.Named("BookOfPsionic");
                }
                else
                {
                    tempPod = ThingDef.Named("BookOfFaceless");
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
