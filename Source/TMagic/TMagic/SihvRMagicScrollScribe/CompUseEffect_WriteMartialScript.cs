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
                int attempt = 0;
                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                RetryWrite:;
                if (attempt < 20)
                {
                    float rnd = Rand.Range(0, 6);
                    if (rnd < 1)
                    {
                        if (settingsRef.Gladiator)
                        {
                            tempPod = ThingDef.Named("BookOfGladiator");
                        }
                        else
                        {
                            attempt++;
                            goto RetryWrite;
                        }
                    }
                    else if (rnd < 2)
                    {
                        
                        if (settingsRef.Sniper)
                        {
                            tempPod = ThingDef.Named("BookOfSniper");
                        }
                        else
                        {
                            attempt++;
                            goto RetryWrite;
                        }
                    }
                    else if (rnd < 3)
                    {                       
                        if (settingsRef.Bladedancer)
                        {
                            tempPod = ThingDef.Named("BookOfBladedancer");
                        }
                        else
                        {
                            attempt++;
                            goto RetryWrite;
                        }
                    }
                    else if (rnd < 4)
                    {                        
                        if (settingsRef.Ranger)
                        {
                            tempPod = ThingDef.Named("BookOfRanger");
                        }
                        else
                        {
                            attempt++;
                            goto RetryWrite;
                        }
                    }
                    else if (rnd < 5)
                    {                        
                        if (settingsRef.Psionic)
                        {
                            tempPod = ThingDef.Named("BookOfPsionic");
                        }
                        else
                        {
                            attempt++;
                            goto RetryWrite;
                        }
                    }
                    else
                    {                        
                        if (settingsRef.Faceless)
                        {
                            tempPod = ThingDef.Named("BookOfFaceless");
                        }
                        else
                        {
                            attempt++;
                            goto RetryWrite;
                        }
                    }
                    this.parent.Destroy(DestroyMode.Vanish);
                }
                else
                {
                    Messages.Message("Unable to find a valid combat book type to write - ending attempt.", MessageTypeDefOf.RejectInput);
                }
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
