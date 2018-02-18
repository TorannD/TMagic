using System;
using UnityEngine;

namespace TorannMagic.Enchantment
{
    public static class GenInfusionColor
    {
        public static readonly Color Uncommon = new Color(0.12f, 1f, 0f);

        public static readonly Color Rare = new Color(0f, 0.44f, 1f);

        public static readonly Color Epic = new Color(0.64f, 0.21f, 0.93f);

        public static readonly Color Legendary = new Color(1f, 0.5f, 0f);

        public static readonly Color Artifact = new Color(0.92f, 0.84f, 0.56f);

        public static Color InfusionColor(this InfusionTier it)
        {
            switch (it)
            {
                case InfusionTier.Common:
                    return Color.white;
                case InfusionTier.Uncommon:
                    return GenInfusionColor.Uncommon;
                case InfusionTier.Rare:
                    return GenInfusionColor.Rare;
                case InfusionTier.Epic:
                    return GenInfusionColor.Epic;
                case InfusionTier.Legendary:
                    return GenInfusionColor.Legendary;
                case InfusionTier.Artifact:
                    return GenInfusionColor.Artifact;
                default:
                    return Color.white;
            }
        }
    }
}
