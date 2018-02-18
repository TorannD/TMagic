using RimWorld;
using System;
using System.Linq;
using Verse;

namespace TorannMagic.Enchantment
{
    public static class GenInfusion
    {
        public static float GetInfusionChance(Thing thing, QualityCategory qc)
        {
            ChanceDef chanceDef = (from chance in DefDatabase<ChanceDef>.AllDefs.Reverse<ChanceDef>()
                                   where chance.Allows(thing)
                                   select chance).FirstOrDefault<ChanceDef>();
            if (chanceDef == null)
            {
                return 0f;
            }
            return chanceDef.Chance(qc);
        }

        public static InfusionTier GetTier(QualityCategory qc, float multiplier)
        {
            float value = Rand.Value;
            if ((double)value < 0.02 * (double)GenInfusion.QualityMultiplier(qc) * (double)multiplier)
            {
                return InfusionTier.Artifact;
            }
            if ((double)value < 0.045 * (double)GenInfusion.QualityMultiplier(qc) * (double)multiplier)
            {
                return InfusionTier.Legendary;
            }
            if ((double)value < 0.09 * (double)GenInfusion.QualityMultiplier(qc) * (double)multiplier)
            {
                return InfusionTier.Epic;
            }
            if ((double)value < 0.18 * (double)GenInfusion.QualityMultiplier(qc) * (double)multiplier)
            {
                return InfusionTier.Rare;
            }
            if ((double)value < 0.5 * (double)GenInfusion.QualityMultiplier(qc) * (double)multiplier)
            {
                return InfusionTier.Uncommon;
            }
            return InfusionTier.Common;
        }

        private static float QualityMultiplier(QualityCategory qc)
        {
            return (float)qc / 3f;
        }

        public static bool TryGetInfusions(this Thing thing, out InfusionSet targInf)
        {
            CompInfusion compInfusion = ThingCompUtility.TryGetComp<CompInfusion>(thing);
            if (compInfusion == null)
            {
                targInf = InfusionSet.Empty;
                return false;
            }
            targInf = compInfusion.Infusions;
            return compInfusion.Infused;
        }
    }
}
