using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace TorannMagic.Enchantment
{
    public class EnchantmentDef : Def
    {
        public string labelShort = "#NN";

        public Dictionary<StatDef, StatMod> stats = new Dictionary<StatDef, StatMod>();

        public Dictionary<MagicData, StatMod> magicStats = new Dictionary<MagicData, StatMod>();

        public InfusionType type;

        public InfusionTier tier;

        public InfusionAllowance allowance = new InfusionAllowance();

        public bool TryGetStatValue(StatDef stat, out StatMod mod)
        {
            return this.stats.TryGetValue(stat, out mod);
        }

        public bool MatchItemType(ThingDef def)
        {
            return (def.IsMeleeWeapon && this.allowance.melee) || (def.IsRangedWeapon && this.allowance.ranged) || (def.IsApparel && this.allowance.apparel);
        }

        public override void ResolveReferences()
        {
            base.ResolveReferences();
            Predicate<StatPart> predicate = (StatPart part) => part.GetType() == typeof(StatPart_InfusionModifier);
            foreach (StatDef current in this.stats.Keys)
            {
                if (current.parts == null)
                {
                    current.parts = new List<StatPart>(1);
                }
                else if (GenCollection.Any<StatPart>(current.parts, predicate))
                {
                    continue;
                }
                current.parts.Add(new StatPart_InfusionModifier(current));
            }
        }

        public static EnchantmentDef Named(string defName)
        {
            return (defName == null) ? null : DefDatabase<EnchantmentDef>.GetNamed(defName, true);
        }
    }
}
