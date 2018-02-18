using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace TorannMagic
{
    public class ItemCollectionGenerator_AncientTempleContents : ItemCollectionGenerator
    {
        private const float ArtifactsChance = 0.9f;

        private const float LuciferiumChance = 0.9f;

        private static readonly IntRange ArtifactsCountRange = new IntRange(1, 3);

        private static readonly IntRange LuciferiumCountRange = new IntRange(5, 20);

        private const float ArcaneScriptChance = 0.9f;

        private static readonly IntRange ArcaneScriptCountRange = new IntRange(1, 3);

        protected override void Generate(ItemCollectionGeneratorParams parms, List<Thing> outThings)
        {
            Messages.Message("TM item collection called", MessageSound.Benefit);
            if (Rand.Chance(0.9f))
            {
                Thing thing = ThingMaker.MakeThing(ThingDefOf.Luciferium, null);
                thing.stackCount = LuciferiumCountRange.RandomInRange;
                outThings.Add(thing);
            }
            if (Rand.Chance(0.9f))
            {
                int randomInRange = ArtifactsCountRange.RandomInRange;
                for (int i = 0; i < randomInRange; i++)
                {
                    ThingDef def = ItemCollectionGenerator_Artifacts.artifacts.RandomElement<ThingDef>();
                    Thing item = ThingMaker.MakeThing(def, null);
                    outThings.Add(item);
                }
            }
            if (Rand.Chance(0.9f))
            {
                Messages.Message("random create called", MessageSound.Benefit);
                int randomInRange = ArcaneScriptCountRange.RandomInRange;
                for (int i = 0; i < randomInRange; i++)
                {
                    Thing thing = ThingMaker.MakeThing(TorannMagicDefOf.BookOfInnerFire, null);
                    outThings.Add(thing);
                    Messages.Message("Book of fire should be created", MessageSound.Benefit);
                }

            }
        }
    }
}
