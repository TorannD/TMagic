using System;
using Verse.AI;
using Verse;
using RimWorld;
using System.Collections.Generic;

namespace TorannMagic.Enchantment
{
    public class JobDriver_EnchantItem : JobDriver
    {
        Thing thing;
        IntVec3 thingLoc;
        Pawn actor;

        public override bool TryMakePreToilReservations()
        {
            return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
        }

        public static void ErrorCheck(Pawn pawn, Thing haulThing)
        {
            if (!haulThing.Spawned)
            {
                Log.Message(string.Concat(new object[]
                {
                    pawn,
                    " tried to start carry ",
                    haulThing,
                    " which isn't spawned."
                }));
                pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
            }
            if (haulThing.stackCount == 0)
            {
                Log.Message(string.Concat(new object[]
                {
                    pawn,
                    " tried to start carry ",
                    haulThing,
                    " which had stackcount 0."
                }));
                pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true);
            }
            if (pawn.jobs.curJob.count <= 0)
            {
                Log.Error(string.Concat(new object[]
                {
                    "Invalid count: ",
                    pawn.jobs.curJob.count,
                    ", setting to 1. Job was ",
                    pawn.jobs.curJob
                }));
                pawn.jobs.curJob.count = 1;
            }
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDestroyedOrNull(TargetIndex.A);


            Toil gotoThing = new Toil();
            gotoThing.initAction = delegate
            {
                this.pawn.pather.StartPath(this.TargetThingA, PathEndMode.Touch);
            };
            gotoThing.defaultCompleteMode = ToilCompleteMode.PatherArrival;
            gotoThing.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            yield return gotoThing;
            Toil enchanting = new Toil();//actions performed to enchant an item
            enchanting.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            enchanting.FailOnDestroyedOrNull(TargetIndex.A);
            enchanting.initAction = delegate
            {
                actor = enchanting.actor;
                thing = TargetThingA;
                thingLoc = thing.Position;
                if (!(thing.def.IsMeleeWeapon || thing.def.IsRangedWeapon || thing.def.IsApparel))
                {
                    actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
                    Log.Message("Failed to initialize enchanting - invalid item type.");
                }
            };
            enchanting.tickAction = delegate
            {
                if(thing.Position != thingLoc || thing.Destroyed)
                {
                    actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
                    Log.Message("Failed to complete enchanting - item being enchanted not at enchanting location or destroyed");
                }
            };
            enchanting.WithProgressBar(TargetIndex.A, delegate
            {
                if (thing == null)
                {
                    return 1f;
                }
                return 1f - (float)enchanting.actor.jobs.curDriver.ticksLeftThisToil / 600;

            }, false, 0f);
            enchanting.defaultCompleteMode = ToilCompleteMode.Delay;
            enchanting.defaultDuration = 600;
            enchanting.AddFinishAction(delegate
            {
                Log.Message("Completing enchantment");
                CompEnchantedItem enchantment = thing.TryGetComp<CompEnchantedItem>();
                CompEnchant enchantingItem = actor.TryGetComp<CompEnchant>();
                CompAbilityUserMagic pawnComp = actor.TryGetComp<CompAbilityUserMagic>();
                if (enchantment != null && enchantingItem != null && enchanting.actor.jobs.curDriver.ticksLeftThisToil < 1)                    
                {                    
                    EnchantItem(enchantingItem.enchantingContainer[0], enchantment);
                    enchantingItem.enchantingContainer[0].Destroy();
                    pawnComp.Mana.CurLevel -= .5f;
                    
                    //DestroyEnchantingStone(enchantingItem.innerContainer[0]);
                }
                else
                {
                    Log.Message("Detected null enchanting comp.");
                }
            });
            yield return enchanting;
        }

        private void EnchantItem(Thing gemstone, CompEnchantedItem enchantment)
        {
            enchantment.Props.HasEnchantment = true;
            switch (gemstone.def.defName)
            {
                case "TM_EStone_maxMP_minor":                    
                    enchantment.Props.maxMP = .05f;
                    enchantment.Props.maxMPTier = EnchantmentTier.Minor;
                    break;
                case "TM_EStone_maxMP":
                    enchantment.Props.maxMP = .1f;
                    enchantment.Props.maxMPTier = EnchantmentTier.Standard;
                    break;
                case "TM_EStone_maxMP_major":
                    enchantment.Props.maxMP = .15f;
                    enchantment.Props.maxMPTier = EnchantmentTier.Major;
                    break;
                case "TM_EStone_mpRegenRate_minor":
                    enchantment.Props.mpRegenRate = .05f;
                    enchantment.Props.mpRegenRateTier = EnchantmentTier.Minor;
                    break;
                case "TM_EStone_mpRegenRate":
                    enchantment.Props.mpRegenRate = .1f;
                    enchantment.Props.mpRegenRateTier = EnchantmentTier.Standard;
                    break;
                case "TM_EStone_mpRegenRate_major":
                    enchantment.Props.mpRegenRate = .15f;
                    enchantment.Props.mpRegenRateTier = EnchantmentTier.Major;
                    break;
                case "TM_EStone_mpCost_minor":
                    enchantment.Props.mpCost = -.03f;
                    enchantment.Props.mpCostTier = EnchantmentTier.Minor;
                    break;
                case "TM_EStone_mpCost":
                    enchantment.Props.mpCost = -.05f;
                    enchantment.Props.mpCostTier = EnchantmentTier.Standard;
                    break;
                case "TM_EStone_mpCost_major":
                    enchantment.Props.mpCost = -.07f;
                    enchantment.Props.mpCostTier = EnchantmentTier.Major;
                    break;
                case "TM_EStone_coolDown_minor":
                    enchantment.Props.coolDown = -.03f;
                    enchantment.Props.coolDownTier = EnchantmentTier.Minor;
                    break;
                case "TM_EStone_coolDown":
                    enchantment.Props.coolDown = -.05f;
                    enchantment.Props.coolDownTier = EnchantmentTier.Standard;
                    break;
                case "TM_EStone_coolDown_major":
                    enchantment.Props.coolDown = -.07f;
                    enchantment.Props.coolDownTier = EnchantmentTier.Major;
                    break;
                case "TM_EStone_xpGain_minor":
                    enchantment.Props.xpGain = .05f;
                    enchantment.Props.xpGainTier = EnchantmentTier.Minor;
                    break;
                case "TM_EStone_xpGain":
                    enchantment.Props.xpGain = .10f;
                    enchantment.Props.xpGainTier = EnchantmentTier.Standard;
                    break;
                case "TM_EStone_xpGain_major":
                    enchantment.Props.xpGain = .15f;
                    enchantment.Props.xpGainTier = EnchantmentTier.Major;
                    break;
                case "null":
                    Log.Message("null");
                    break;
            }
        }

        private void DestroyEnchantingStone(Thing enchantingStone)
        {

        }
    }
}
