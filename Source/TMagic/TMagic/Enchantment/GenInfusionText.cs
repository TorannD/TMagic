using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace TorannMagic.Enchantment
{
    public static class GenInfusionText
    {
        private struct InfusedLabelRequest
        {
            public Thing Thing;

            public BuildableDef BuildableDef;

            public ThingDef StuffDef;

            public int HitPoints;

            public int MaxHitPoints;

            public override int GetHashCode()
            {
                int num = 7437233;
                if (this.Thing != null)
                {
                    num ^= this.Thing.GetHashCode() * 712433;
                }
                int num2 = num ^ this.BuildableDef.GetHashCode() * 345111;
                if (this.StuffDef != null)
                {
                    num2 ^= this.StuffDef.GetHashCode() * 666613;
                }
                ThingDef thingDef = this.BuildableDef as ThingDef;
                if (thingDef == null)
                {
                    return num2;
                }
                InfusionSet infusionSet;
                if (this.Thing != null && this.Thing.TryGetInfusions(out infusionSet))
                {
                    if (infusionSet.enchantment != null)
                    {
                        num2 ^= infusionSet.enchantment.GetHashCode();
                    }
                }
                if (thingDef.useHitPoints)
                {
                    num2 = (num2 ^ this.HitPoints * 743273 ^ this.MaxHitPoints * 7437);
                }
                return num2;
            }
        }

        private static Dictionary<int, string> infusedLabelDict = new Dictionary<int, string>();

        private const int LabelDictionaryMaxCount = 1000;

        public static string GetInfusedLabel(this Thing thing, bool isStuffed = true, bool isDetailed = true)
        {
            GenInfusionText.InfusedLabelRequest infusedLabelRequest = new GenInfusionText.InfusedLabelRequest
            {
                BuildableDef = thing.def,
                Thing = thing
            };
            if (isStuffed)
            {
                infusedLabelRequest.StuffDef = thing.Stuff;
            }
            if (isDetailed)
            {
                infusedLabelRequest.MaxHitPoints = thing.MaxHitPoints;
                infusedLabelRequest.HitPoints = thing.HitPoints;
            }
            int hashCode = infusedLabelRequest.GetHashCode();
            string text;
            if (GenInfusionText.infusedLabelDict.TryGetValue(hashCode, out text))
            {
                return text;
            }
            if (GenInfusionText.infusedLabelDict.Count > 1000)
            {
                GenInfusionText.infusedLabelDict.Clear();
            }
            text = GenInfusionText.NewInfusedThingLabel(thing, isStuffed, isDetailed);
            GenInfusionText.infusedLabelDict.Add(hashCode, text);
            return text;
        }

        private static string NewInfusedThingLabel(Thing thing, bool isStuffed, bool isDetailed)
        {
            string text;
            if (isStuffed && thing.Stuff != null)
            {
                text = Translator.Translate(ResourceBank.StringInfusionLabel, new object[]
                {
                    thing.Stuff.LabelAsStuff,
                    thing.def.label
                });
            }
            else
            {
                text = thing.def.label;
            }
            StringBuilder stringBuilder = new StringBuilder();
            InfusionSet infusionSet;
            if (!thing.TryGetInfusions(out infusionSet))
            {
                return text;
            }
            EnchantmentDef enchantment = infusionSet.enchantment;
            if (enchantment != null)
            {
                stringBuilder.Append(text + " (" + Translator.Translate(ResourceBank.StringInfusionOf, new object[]
            {
                text,
                GenText.CapitalizeFirst(enchantment.label)
            }) + ")");
            }
            return stringBuilder.ToString();
        }

        public static string GetInfusionDesc(this Thing thing)
        {
            InfusionSet infusionSet;
            if (!thing.TryGetInfusions(out infusionSet))
            {
                return null;
            }
            EnchantmentDef enchantment = infusionSet.enchantment;
            StringBuilder stringBuilder = new StringBuilder(null);
            if (enchantment != null)
            {
                stringBuilder.Append(Translator.Translate(ResourceBank.StringInfusionDescFrom, new object[]
                {
                    enchantment.LabelCap
                })).Append(" (").Append(enchantment.tier.Translate()).AppendLine(")");
                foreach (KeyValuePair<StatDef, StatMod> current in enchantment.stats)
                {
                    if (current.Value.offset.FloatNotEqual(0f))
                    {
                        stringBuilder.Append("     " + ((current.Value.offset <= 0f) ? "-" : "+"));
                        if (current.Key == StatDefOf.ComfyTemperatureMax || current.Key == StatDefOf.ComfyTemperatureMin)
                        {
                            stringBuilder.Append(GenText.ToStringTemperatureOffset(current.Value.offset.ToAbs(), "F1"));
                        }
                        else
                        {
                            StatPart_InfusionModifier statPart_InfusionModifier = current.Key.parts.Find((StatPart s) => s is StatPart_InfusionModifier) as StatPart_InfusionModifier;
                            if (statPart_InfusionModifier != null)
                            {
                                stringBuilder.Append(statPart_InfusionModifier.parentStat.ValueToString(current.Value.offset.ToAbs(), (ToStringNumberSense)1));
                            }
                        }
                        stringBuilder.AppendLine(" " + current.Key.LabelCap);
                    }
                    if (current.Value.multiplier.FloatNotEqual(1f))
                    {
                        stringBuilder.Append("     " + GenText.ToStringPercent(current.Value.multiplier.ToAbs()));
                        stringBuilder.AppendLine(" " + current.Key.LabelCap);
                    }
                }
                stringBuilder.AppendLine();
            }
            if (enchantment == null)
            {
                return stringBuilder.ToString();
            }
            stringBuilder.Append(Translator.Translate(ResourceBank.StringInfusionDescFrom, new object[]
            {
                enchantment.LabelCap
            })).Append(" (").Append(enchantment.tier.Translate()).AppendLine(")");
            foreach (KeyValuePair<StatDef, StatMod> current2 in enchantment.stats)
            {
                if (current2.Value.offset.FloatNotEqual(0f))
                {
                    stringBuilder.Append("     " + ((current2.Value.offset <= 0f) ? "-" : "+"));
                    if (current2.Key == StatDefOf.ComfyTemperatureMax || current2.Key == StatDefOf.ComfyTemperatureMin)
                    {
                        stringBuilder.Append(GenText.ToStringTemperatureOffset(current2.Value.offset.ToAbs(), "F1"));
                    }
                    else
                    {
                        StatPart_InfusionModifier statPart_InfusionModifier2 = current2.Key.parts.Find((StatPart s) => s is StatPart_InfusionModifier) as StatPart_InfusionModifier;
                        if (statPart_InfusionModifier2 != null)
                        {
                            stringBuilder.Append(statPart_InfusionModifier2.parentStat.ValueToString(current2.Value.offset.ToAbs(), (ToStringNumberSense)1));
                        }
                    }
                    stringBuilder.AppendLine(" " + current2.Key.LabelCap);
                }
                if (current2.Value.multiplier.FloatNotEqual(1f))
                {
                    stringBuilder.Append("     " + GenText.ToStringPercent(current2.Value.multiplier.ToAbs()));
                    stringBuilder.AppendLine(" " + current2.Key.LabelCap);
                }
            }
            return stringBuilder.ToString();
        }
    }
}
