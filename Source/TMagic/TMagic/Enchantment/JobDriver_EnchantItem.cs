using System;
using Verse.AI;
using Verse;
using RimWorld;
using System.Collections.Generic;
using Verse.Sound;

namespace TorannMagic.Enchantment
{
    public class JobDriver_EnchantItem : JobDriver
    {
        Thing thing;
        IntVec3 thingLoc;
        Pawn actor;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
            throw new NotImplementedException();
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
                else if(thing.def.defName.Contains("TM_Artifact"))
                {
                    Messages.Message("TM_CannotEnchantArtifact".Translate(
                        actor.LabelShort,
                        thing.LabelShort
                    ), MessageTypeDefOf.RejectInput);
                    actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);                    
                }
            };
            enchanting.tickAction = delegate
            {
                if(thing.Position != thingLoc || thing.Destroyed)
                {
                    actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
                    Log.Message("Failed to complete enchanting - item being enchanted not at enchanting location or destroyed");
                }
                if (Find.TickManager.TicksGame % 5 == 0)
                {
                    TM_MoteMaker.ThrowEnchantingMote(TargetLocA.ToVector3Shifted(), actor.Map, .6f);
                }
            };
            enchanting.WithProgressBar(TargetIndex.A, delegate
            {
                if (thing == null)
                {
                    return 1f;
                }
                return 1f - (float)enchanting.actor.jobs.curDriver.ticksLeftThisToil / 240;

            }, false, 0f);
            enchanting.defaultCompleteMode = ToilCompleteMode.Delay;
            enchanting.defaultDuration = 240;
            enchanting.AddFinishAction(delegate
            {
                CompEnchantedItem enchantment = thing.TryGetComp<CompEnchantedItem>();            
                CompEnchant enchantingItem = actor.TryGetComp<CompEnchant>();
                CompAbilityUserMagic pawnComp = actor.TryGetComp<CompAbilityUserMagic>();
                if (enchantment != null && enchantingItem != null && enchanting.actor.jobs.curDriver.ticksLeftThisToil < 1)
                {
                    EnchantItem(enchantingItem.enchantingContainer[0], enchantment);
                    enchantingItem.enchantingContainer[0].Destroy();
                    pawnComp.Mana.CurLevel -= .5f;
                    int num = Rand.Range(130, 180);
                    pawnComp.MagicUserXP += num;
                    MoteMaker.ThrowText(actor.DrawPos, actor.Map, "XP +" + num, -1f);
                    MoteMaker.ThrowText(TargetLocA.ToVector3Shifted(), actor.Map, "TM_Enchanted".Translate(), -1);
                    SoundStarter.PlayOneShotOnCamera(TorannMagicDefOf.ItemEnchanted, null);
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
            enchantment.HasEnchantment = true;
            switch (gemstone.def.defName)
            {
                case "TM_EStone_wonder_minor":
                    enchantment.maxMP = .05f;
                    enchantment.maxMPTier = EnchantmentTier.Minor;
                    enchantment.mpRegenRate = .05f;
                    enchantment.mpRegenRateTier = EnchantmentTier.Minor;
                    enchantment.mpCost = -.03f;
                    enchantment.mpCostTier = EnchantmentTier.Minor;
                    enchantment.coolDown = -.03f;
                    enchantment.coolDownTier = EnchantmentTier.Minor;
                    enchantment.xpGain = .05f;
                    enchantment.xpGainTier = EnchantmentTier.Minor;
                    enchantment.arcaneRes = .10f;
                    enchantment.arcaneResTier = EnchantmentTier.Minor;
                    enchantment.arcaneDmg = .04f;
                    enchantment.arcaneDmgTier = EnchantmentTier.Minor;
                    break;
                case "TM_EStone_wonder":
                    enchantment.maxMP = .1f;
                    enchantment.maxMPTier = EnchantmentTier.Standard;
                    enchantment.mpRegenRate = .1f;
                    enchantment.mpRegenRateTier = EnchantmentTier.Standard;
                    enchantment.mpCost = -.05f;
                    enchantment.mpCostTier = EnchantmentTier.Standard;
                    enchantment.coolDown = -.05f;
                    enchantment.coolDownTier = EnchantmentTier.Standard;
                    enchantment.xpGain = .10f;
                    enchantment.xpGainTier = EnchantmentTier.Standard;
                    enchantment.arcaneRes = .20f;
                    enchantment.arcaneResTier = EnchantmentTier.Standard;
                    enchantment.arcaneDmg = .08f;
                    enchantment.arcaneDmgTier = EnchantmentTier.Standard;
                    break;
                case "TM_EStone_wonder_major":
                    enchantment.maxMP = .15f;
                    enchantment.maxMPTier = EnchantmentTier.Major;
                    enchantment.mpRegenRate = .15f;
                    enchantment.mpRegenRateTier = EnchantmentTier.Major;
                    enchantment.mpCost = -.07f;
                    enchantment.mpCostTier = EnchantmentTier.Major;
                    enchantment.coolDown = -.07f;
                    enchantment.coolDownTier = EnchantmentTier.Major;
                    enchantment.xpGain = .15f;
                    enchantment.xpGainTier = EnchantmentTier.Major;
                    enchantment.arcaneRes = .30f;
                    enchantment.arcaneResTier = EnchantmentTier.Major;
                    enchantment.arcaneDmg = .12f;
                    enchantment.arcaneDmgTier = EnchantmentTier.Major;
                    break;
                case "TM_EStone_maxMP_minor":                    
                    enchantment.maxMP = .05f;
                    enchantment.maxMPTier = EnchantmentTier.Minor;
                    break;
                case "TM_EStone_maxMP":
                    enchantment.maxMP = .1f;
                    enchantment.maxMPTier = EnchantmentTier.Standard;
                    break;
                case "TM_EStone_maxMP_major":
                    enchantment.maxMP = .15f;
                    enchantment.maxMPTier = EnchantmentTier.Major;
                    break;
                case "TM_EStone_mpRegenRate_minor":
                    enchantment.mpRegenRate = .05f;
                    enchantment.mpRegenRateTier = EnchantmentTier.Minor;
                    break;
                case "TM_EStone_mpRegenRate":
                    enchantment.mpRegenRate = .1f;
                    enchantment.mpRegenRateTier = EnchantmentTier.Standard;
                    break;
                case "TM_EStone_mpRegenRate_major":
                    enchantment.mpRegenRate = .15f;
                    enchantment.mpRegenRateTier = EnchantmentTier.Major;
                    break;
                case "TM_EStone_mpCost_minor":
                    enchantment.mpCost = -.03f;
                    enchantment.mpCostTier = EnchantmentTier.Minor;
                    break;
                case "TM_EStone_mpCost":
                    enchantment.mpCost = -.05f;
                    enchantment.mpCostTier = EnchantmentTier.Standard;
                    break;
                case "TM_EStone_mpCost_major":
                    enchantment.mpCost = -.07f;
                    enchantment.mpCostTier = EnchantmentTier.Major;
                    break;
                case "TM_EStone_coolDown_minor":
                    enchantment.coolDown = -.03f;
                    enchantment.coolDownTier = EnchantmentTier.Minor;
                    break;
                case "TM_EStone_coolDown":
                    enchantment.coolDown = -.05f;
                    enchantment.coolDownTier = EnchantmentTier.Standard;
                    break;
                case "TM_EStone_coolDown_major":
                    enchantment.coolDown = -.07f;
                    enchantment.coolDownTier = EnchantmentTier.Major;
                    break;
                case "TM_EStone_xpGain_minor":
                    enchantment.xpGain = .05f;
                    enchantment.xpGainTier = EnchantmentTier.Minor;
                    break;
                case "TM_EStone_xpGain":
                    enchantment.xpGain = .10f;
                    enchantment.xpGainTier = EnchantmentTier.Standard;
                    break;
                case "TM_EStone_xpGain_major":
                    enchantment.xpGain = .15f;
                    enchantment.xpGainTier = EnchantmentTier.Major;
                    break;
                case "TM_EStone_arcaneRes_minor":
                    enchantment.arcaneRes = .10f;
                    enchantment.arcaneResTier = EnchantmentTier.Minor;
                    break;
                case "TM_EStone_arcaneRes":
                    enchantment.arcaneRes = .20f;
                    enchantment.arcaneResTier = EnchantmentTier.Standard;
                    break;
                case "TM_EStone_arcaneRes_major":
                    enchantment.arcaneRes = .30f;
                    enchantment.arcaneResTier = EnchantmentTier.Major;
                    break;
                case "TM_EStone_arcaneDmg_minor":
                    enchantment.arcaneDmg = .04f;
                    enchantment.arcaneDmgTier = EnchantmentTier.Minor;
                    break;
                case "TM_EStone_arcaneDmg":
                    enchantment.arcaneDmg = .08f;
                    enchantment.arcaneDmgTier = EnchantmentTier.Standard;
                    break;
                case "TM_EStone_arcaneDmg_major":
                    enchantment.arcaneDmg = .12f;
                    enchantment.arcaneDmgTier = EnchantmentTier.Major;
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
