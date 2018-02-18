using System;
using UnityEngine;

namespace TorannMagic.Enchantment
{
    public static class MathUtility
    {
        public static InfusionTier Max(InfusionTier a, InfusionTier b)
        {
            if (a == InfusionTier.Undefined)
            {
                return b;
            }
            if (b == InfusionTier.Undefined)
            {
                return a;
            }
            return (a >= b) ? a : b;
        }

        public static float ToAbs(this float f)
        {
            return Mathf.Abs(f);
        }

        public static bool FloatEqual(this float a, float b)
        {
            return (a - b).ToAbs() < 1E-05f;
        }

        public static bool FloatNotEqual(this float a, float b)
        {
            return !a.FloatEqual(b);
        }
    }
}
