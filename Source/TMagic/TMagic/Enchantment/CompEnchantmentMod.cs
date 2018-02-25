using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TorannMagic.Enchantment
{
    internal class CompEnchantmentMod : Mod
    {
        public CompEnchantmentMod(ModContentPack mcp) : base(mcp)
        {
            LongEventHandler.ExecuteWhenFinished(new Action(CompEnchantmentMod.AddComp));
        }

        private static void AddComp()
        {
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where (def.IsMeleeWeapon || def.IsRangedWeapon || def.IsApparel) && def.HasComp(typeof(CompQuality)) && !def.HasComp(typeof(CompEnchantedItem))
                                               select def;
            Type typeFromHandle = typeof(ITab_Enchantment);
            InspectTabBase sharedInstance = InspectTabManager.GetSharedInstance(typeFromHandle);            
            foreach (ThingDef current in enumerable)
            {
                CompProperties_EnchantedItem item = new CompProperties_EnchantedItem
                {
                    compClass = typeof(CompEnchantedItem)
                };
                current.comps.Add(item);

                if (current.inspectorTabs == null || current.inspectorTabs.Count == 0)
                {
                    current.inspectorTabs = new List<Type>();
                    current.inspectorTabsResolved = new List<InspectTabBase>();
                }
                current.inspectorTabs.Add(typeFromHandle);
                current.inspectorTabsResolved.Add(sharedInstance);
            }
        }

    }
}
