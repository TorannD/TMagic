using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;

namespace TorannMagic
{
    public class ItemCollectionGenerator_Internal_Arcane
    {
        private const float ArtifactsChance = 0.03f;
        private const float LuciferiumChance = 0.2f;
        private const float ArcaneScriptChance = 0.1f;
        private const float DrugChance = 0.15f;
        private const float SpellChance = 0.2f;
        private const float SkillChance = 0.2f;
        private const float MasterSpellChance = 0.1f;

        private static readonly IntRange ArtifactsCountRange = new IntRange(1, 2);
        private static readonly IntRange LuciferiumCountRange = new IntRange(4, 8);
        private static readonly IntRange ManaPotionRange = new IntRange(1, 4);
        private static readonly IntRange DrugCountRange = new IntRange(3, 10);
        private static readonly IntRange SpellCountRange = new IntRange(1, 2);
        private static readonly IntRange SkillCountRange = new IntRange(1, 2);

        private float collectiveMarketValue = 0;

        public List<Thing> Generate(ItemCollectionGeneratorParams parms, List<Thing> outThings)
        {
            
            for (int j = 0; j < 10; j++)
            {
                //Torn Scripts
                if (Rand.Chance(0.3f) && (parms.totalMarketValue - collectiveMarketValue) > TorannMagicDefOf.Torn_BookOfArcanist.BaseMarketValue / 2)
                {
                    if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.Torn_BookOfInnerFire, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.Torn_BookOfHeartOfFrost, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.Torn_BookOfStormBorn, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.Torn_BookOfArcanist, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.Torn_BookOfValiant, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.Torn_BookOfSummoner, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.Torn_BookOfNature, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.Torn_BookOfUndead, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.Torn_BookOfPriest, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.Torn_BookOfBard, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }

                }
                //Arcane Scripts
                if (Rand.Chance(0.1f) && (parms.totalMarketValue - collectiveMarketValue) > TorannMagicDefOf.BookOfArcanist.BaseMarketValue / 2)
                {
                    if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfInnerFire, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    else if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfHeartOfFrost, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    else if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfStormBorn, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    else if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfArcanist, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    else if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfValiant, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }

                    if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfSummoner, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    else if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfNecromancer, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    else if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfDruid, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    else if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfPriest, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    else if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfBard, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }

                    if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfGladiator, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    else if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfSniper, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    else if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfBladedancer, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }
                    else if (Rand.Chance(ArcaneScriptChance))
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfRanger, null);
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue;
                    }

                }
                //Mana Potions
                if (Rand.Chance(0.2f) && (parms.totalMarketValue - collectiveMarketValue) > TorannMagicDefOf.ManaPotion.BaseMarketValue * ManaPotionRange.RandomInRange)
                {
                    int randomInRange = ManaPotionRange.RandomInRange;
                    for (int i = 0; i < randomInRange; i++)
                    {
                        Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.ManaPotion, null);
                        thing.stackCount = ManaPotionRange.RandomInRange;
                        outThings.Add(thing);
                        collectiveMarketValue += thing.MarketValue * thing.stackCount;
                    }
                }
                //Artifacts
                if (Rand.Chance(ArtifactsChance) && (parms.totalMarketValue - collectiveMarketValue) > 1000f)
                {
                    int randomInRange = ArtifactsCountRange.RandomInRange;
                    for (int i = 0; i < randomInRange; i++)
                    {
                        ThingDef def = ItemCollectionGenerator_Artifacts.artifacts.RandomElement<ThingDef>();
                        Thing item = ThingMaker.MakeThing(def, null);
                        outThings.Add(item);
                        collectiveMarketValue += item.MarketValue;
                    }
                }
                //Luciferium
                if (Rand.Chance(LuciferiumChance) && (parms.totalMarketValue - collectiveMarketValue) > ThingDefOf.Luciferium.BaseMarketValue * LuciferiumCountRange.RandomInRange)
                {
                    Thing thing = ThingMaker.MakeThing(ThingDefOf.Luciferium, null);
                    thing.stackCount = LuciferiumCountRange.RandomInRange;
                    outThings.Add(thing);
                    collectiveMarketValue += thing.MarketValue * thing.stackCount;
                }
                //Ambrosia
                if (Rand.Chance(DrugChance))
                {
                    int randomInRange = ArtifactsCountRange.RandomInRange;
                    for (int i = 0; i < randomInRange; i++)
                    {
                        //Thing thing = ThingMaker.MakeThing(, null);
                        Thing thing = ThingMaker.MakeThing(ThingDef.Named("Ambrosia"), null);
                        outThings.Add(thing);
                    }
                }
                //Master Spells
                if (Rand.Chance(MasterSpellChance) && (parms.totalMarketValue - collectiveMarketValue) > TorannMagicDefOf.SpellOf_Blizzard.BaseMarketValue)
                {
                    Thing thing;
                    float rnd = Rand.Range(0f, 18f);
                    if (rnd > 16)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_SummonPoppi, null);
                    }
                    else if (rnd > 14 && rnd <= 16)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_BattleHymn, null);
                    }
                    else if (rnd > 12 && rnd <= 14)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_HolyWrath, null);
                    }
                    else if (rnd > 10 && rnd <= 12)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_LichForm, null);
                    }
                    else if (rnd > 8 && rnd <= 10)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Blizzard, null);
                    }
                    else if (rnd > 6 && rnd <= 8)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Firestorm, null);
                    }
                    else if (rnd > 4 && rnd <= 6)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_FoldReality, null);
                    }
                    else if (rnd > 2 && rnd <= 4)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Resurrection, null);
                    }
                    else
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_RegrowLimb, null);
                    }
                    outThings.Add(thing);
                    collectiveMarketValue += thing.MarketValue;
                }
                //Spells
                if (Rand.Chance(SpellChance) && (parms.totalMarketValue - collectiveMarketValue) > 1000f)
                {
                    int randomInRange = SpellCountRange.RandomInRange;
                    Thing thing = new Thing();
                    for (int i = 0; i < randomInRange; i++)
                    {
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Blink, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Teleport, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Heal, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Rain, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Heater, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Cooler, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_DryGround, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_WetGround, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_ChargeBattery, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_SmokeCloud, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_EMP, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Extinguish, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_SummonMinion, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_TransferMana, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_SiphonMana, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_ManaShield, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_PowerNode, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Sunlight, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_SpellMending, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_CauterizeWound, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_FertileLands, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                    }
                }
                //Skills
                if (Rand.Chance(SpellChance) && (parms.totalMarketValue - collectiveMarketValue) > 600f)
                {
                    int randomInRange = SkillCountRange.RandomInRange;
                    Thing thing = new Thing();
                    for (int i = 0; i < randomInRange; i++)
                    {
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SkillOf_Sprint, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SkillOf_GearRepair, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SkillOf_InnerHealing, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SkillOf_HeavyBlow, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SkillOf_StrongBack, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SkillOf_ThickSkin, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }
                        if (Rand.Range(0, 10) > 9)
                        {
                            thing = ThingMaker.MakeThing(TorannMagicDefOf.SkillOf_FightersFocus, null);
                            outThings.Add(thing);
                            collectiveMarketValue += thing.MarketValue;
                        }                        
                    }
                }
            }
            return outThings;
        }
    }
}
