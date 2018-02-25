using System;
using Verse;
using RimWorld;

namespace TorannMagic.Enchantment
{
    public class CompEnchant : ThingComp
    {
        public ThingOwner<Thing> enchantingContainer = new ThingOwner<Thing>();

        private Pawn pawn
        {
            get
            {
                Pawn pawn = this.parent as Pawn;
                bool flag = pawn == null;
                if (flag)
                {
                    Log.Error("pawn is null");
                }
                return pawn;
            }
        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            Pawn pawn = this.parent as Pawn;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Deep.Look<ThingOwner<Thing>>(ref this.enchantingContainer, "enchantingContainer", new object[0]);
        }
    }
}
