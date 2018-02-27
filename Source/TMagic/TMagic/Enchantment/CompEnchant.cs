using System;
using Verse;
using RimWorld;

namespace TorannMagic.Enchantment
{
    public class CompEnchant : ThingComp
    {
        public ThingOwner<Thing> enchantingContainer = new ThingOwner<Thing>();

        private bool initialize = true;

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
            if(initialize)
            {
                this.enchantingContainer = new ThingOwner<Thing>();
                this.initialize = false;
            }
            
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Deep.Look<ThingOwner<Thing>>(ref this.enchantingContainer, "enchantingContainer", new object[0]);
            Scribe_Values.Look<bool>(ref this.initialize, "initialize", true, false);
        }
    }
}
