using System;
using Verse;

namespace TorannMagic.Enchantment
{
    public static class ResourceBank
    {
        public static readonly string StringInfused = Translator.Translate("Enchanted");

        public static readonly string StringInfusionMessage = "EnchantmentMessage";

        public static readonly string StringInfusionOf = "EnchantmentOf";

        public static readonly string StringInfusionLabel = "EnchantmentLabel";

        public static readonly string StringInfusionDescBonus = Translator.Translate("EnchantmentDescBonus");

        public static readonly string StringInfusionDescFrom = "EnchantmentDescFrom";

        public static readonly string StringQuality = Translator.Translate("EnchantmentQuality");

        public static string Translate(this InfusionTier it)
        {
            return Translator.Translate("Enchantment" + it);
        }
    }
}
