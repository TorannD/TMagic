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
            //&& def.HasComp(typeof(CompQuality))
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where (def.IsMeleeWeapon || def.IsRangedWeapon || def.IsApparel) && !def.HasComp(typeof(CompEnchantedItem))
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
            
            //IEnumerable<ThingDef> enumerable1 = from def in DefDatabase<ThingDef>.AllDefs
            //                                   where (def.race != null && def.race.Humanlike && !def.HasComp(typeof(CompEnchant))) 
            //                                    select def;
            //foreach (ThingDef current1 in enumerable1)
            //{
            //    CompProperties_Enchant enchanting = new CompProperties_Enchant
            //    {
            //        compClass = typeof(CompEnchant)
            //    };
            //    current1.comps.Add(enchanting);
            //}            
        }

    }
}
