using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;

namespace TorannMagic
{
    public class ItemCollectionGenerator_Arcane : ItemCollectionGenerator
    {
        private const float ArtifactsChance = 0.5f;
        private const float LuciferiumChance = 0.9f;
        private const float ArcaneScriptChance = 0.2f;
        private const float DrugChance = 0.5f;
        private const float SpellChance = 0.8f;
        private const float MasterSpellChance = 0.6f;

        private static readonly IntRange ArtifactsCountRange = new IntRange(1, 3);
        private static readonly IntRange LuciferiumCountRange = new IntRange(5, 20);
        private static readonly IntRange ManaPotionRange = new IntRange(2, 7);
        private static readonly IntRange DrugCountRange = new IntRange(3, 10);
        private static readonly IntRange SpellCountRange = new IntRange(1, 2);

        private int collectiveMarketValue = 0;

        protected override void Generate(ItemCollectionGeneratorParams parms, List<Thing> outThings)
        {

            if (Rand.Chance(1f))
            {
                if (Rand.Chance(ArcaneScriptChance))
                {
                    Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfInnerFire, null);
                    outThings.Add(thing);
                }
                if (Rand.Chance(ArcaneScriptChance))
                {
                    Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfHeartOfFrost, null);
                    outThings.Add(thing);
                }
                if (Rand.Chance(ArcaneScriptChance))
                {
                    Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfStormBorn, null);
                    outThings.Add(thing);
                }
                if (Rand.Chance(ArcaneScriptChance))
                {
                    Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfArcanist, null);
                    outThings.Add(thing);
                }
                if (Rand.Chance(ArcaneScriptChance))
                {
                    Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfValiant, null);
                    outThings.Add(thing);
                }
                if (Rand.Chance(ArcaneScriptChance))
                {
                    Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfSummoner, null);
                    outThings.Add(thing);
                }
                if (Rand.Chance(ArcaneScriptChance))
                {
                    Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfGladiator, null);
                    outThings.Add(thing);
                }
            }
            if (Rand.Chance(1f))
            {
                int randomInRange = ManaPotionRange.RandomInRange;
                for (int i = 0; i < randomInRange; i++)
                {
                    Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.ManaPotion, null);
                    thing.stackCount = ManaPotionRange.RandomInRange;
                    outThings.Add(thing);
                }
            }
            if (Rand.Chance(ArtifactsChance))
            {
                int randomInRange = ArtifactsCountRange.RandomInRange;
                for (int i = 0; i < randomInRange; i++)
                {
                    ThingDef def = ItemCollectionGenerator_Artifacts.artifacts.RandomElement<ThingDef>();
                    Thing item = ThingMaker.MakeThing(def, null);
                    outThings.Add(item);
                }
            }
            if (Rand.Chance(LuciferiumChance))
            {
                Thing thing = ThingMaker.MakeThing(ThingDefOf.Luciferium, null);
                thing.stackCount = LuciferiumCountRange.RandomInRange;
                outThings.Add(thing);
            }

            if (Rand.Chance(DrugChance))
            {
                int randomInRange = ArtifactsCountRange.RandomInRange;
                for (int i = 0; i < randomInRange; i++)
                {
                    //Thing thing = ThingMaker.MakeThing(, null);
                    Thing thing = ThingMaker.MakeThing(ThingDefOf.PlantAmbrosia, null);
                    outThings.Add(thing);
                }
            }

            if (Rand.Chance(MasterSpellChance))
            {
                Thing thing;
                if (Rand.Range(0, 10) > 5)
                {
                    thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Blizzard, null);
                }
                else
                {
                    thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Firestorm, null);
                }
                outThings.Add(thing);
            }

            if (Rand.Chance(SpellChance))
            {
                int randomInRange = SpellCountRange.RandomInRange;
                Thing thing = new Thing();
                for (int i = 0; i < randomInRange; i++)
                {
                    if (Rand.Range(0, 10) > 9)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Blink, null);
                        outThings.Add(thing);
                    }
                    if (Rand.Range(0, 10) > 9)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Teleport, null);
                        outThings.Add(thing);
                    }
                    if (Rand.Range(0, 10) > 9)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Heal, null);
                        outThings.Add(thing);
                    }
                    if (Rand.Range(0, 10) > 9)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Rain, null);
                        outThings.Add(thing);
                    }
                    if (Rand.Range(0, 10) > 9)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Heater, null);
                        outThings.Add(thing);
                    }
                    if (Rand.Range(0, 10) > 9)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Cooler, null);
                        outThings.Add(thing);
                    }
                    if (Rand.Range(0, 10) > 9)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_DryGround, null);
                        outThings.Add(thing);
                    }
                    if (Rand.Range(0, 10) > 9)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_WetGround, null);
                        outThings.Add(thing);
                    }
                    if (Rand.Range(0, 10) > 9)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_ChargeBattery, null);
                        outThings.Add(thing);
                    }
                    if (Rand.Range(0, 10) > 9)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_SmokeCloud, null);
                        outThings.Add(thing);
                    }
                    if (Rand.Range(0, 10) > 9)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_EMP, null);
                        outThings.Add(thing);
                    }
                    if (Rand.Range(0, 10) > 9)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_Extinguish, null);
                        outThings.Add(thing);
                    }
                    if (Rand.Range(0, 10) > 9)
                    {
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.SpellOf_SummonMinion, null);
                        outThings.Add(thing);
                    }
                }
            }            
        }

    }
}
