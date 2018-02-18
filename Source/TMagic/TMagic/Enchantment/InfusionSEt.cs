using System;

namespace TorannMagic.Enchantment
{
    public class InfusionSet : IEquatable<InfusionSet>
    {
        public EnchantmentDef enchantment;

        public static InfusionSet Empty = new InfusionSet(null);

        public InfusionSet(EnchantmentDef enchantment)
        {
            this.enchantment = enchantment;
        }

        public bool Equals(InfusionSet other)
        {
            return !object.ReferenceEquals(null, other) && (object.ReferenceEquals(this, other) || (object.Equals(this.enchantment, other.enchantment)));
        }
    }
}
