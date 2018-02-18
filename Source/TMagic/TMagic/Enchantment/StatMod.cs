using System;

namespace TorannMagic.Enchantment
{
    public class StatMod
    {
        public float offset;

        public float multiplier = 1f;

        public override string ToString()
        {
            return string.Format("[StatMod offset={0}, multiplier={1}]", this.offset, this.multiplier);
        }
    }
}
