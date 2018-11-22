using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace TorannMagic.Enchantment
{
	public class CompEnchant : ThingComp, IThingHolder
	{
		public ThingOwner enchantingContainer;

		public CompEnchant()
		{
			enchantingContainer = new ThingOwner<Thing>(this);
		}

		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			enchantingContainer.TryDropAll(parent.Position, map, ThingPlaceMode.Near, null, null);
		}

		public ThingOwner GetDirectlyHeldThings()
		{
			return enchantingContainer;
		}

		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look(ref enchantingContainer, "enchantingContainer", new object[]
			{
				this
			});
		}
	}
}
