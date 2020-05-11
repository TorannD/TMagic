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
            LongEventHandler.ExecuteWhenFinished(new Action(CompEnchantmentMod.AddUniversalBodyparts));
            LongEventHandler.ExecuteWhenFinished(new Action(CompEnchantmentMod.InitializeFactionSettings));
            LongEventHandler.ExecuteWhenFinished(new Action(CompEnchantmentMod.InitializeCustomClassActions));
        }

        private static void AddComp()
        {
            //unrelated, single time load mod check
            //foreach (ModContentPack p in LoadedModManager.RunningMods)
            //{
            //    Log.Message(p.Name + "");
            //}

            //&& def.HasComp(typeof(CompQuality))
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where (def.IsMeleeWeapon || def.IsRangedWeapon || def.IsApparel) && !def.HasComp(typeof(CompEnchantedItem))
                                               select def;
            Type typeFromHandle = typeof(ITab_Enchantment);
            InspectTabBase sharedInstance = InspectTabManager.GetSharedInstance(typeFromHandle);            
            foreach (ThingDef current in enumerable)
            {
                //if (current.defName != "TM_ThrumboAxe" && current.defName != "TM_FireWand" && current.defName != "TM_IceWand" && current.defName != "TM_LightningWand" &&
                //    current.defName != "TM_BlazingPowerStaff" && current.defName != "TM_DefenderStaff")
                if(!current.defName.Contains("TM_"))
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

        private static void AddUniversalBodyparts()
        {
            IEnumerable<BodyPartDef> universalBodyParts = from def in DefDatabase<BodyPartDef>.AllDefs
                                                          where (def.destroyableByDamage)
                                                          select def;
            foreach (BodyPartDef current1 in universalBodyParts)
            {
                TorannMagicDefOf.UniversalRegrowth.appliedOnFixedBodyParts.AddDistinct(current1);
            }

            IEnumerable<ThingDef> universalPawnTypes = from def in DefDatabase<ThingDef>.AllDefs
                                                       where (def.category == ThingCategory.Pawn && !def.defName.Contains("TM_") && def.race.IsFlesh)
                                                       select def;
            foreach (ThingDef current2 in universalPawnTypes)
            {
                TorannMagicDefOf.UniversalRegrowth.recipeUsers.AddDistinct(current2);
            }
        }

        private static void InitializeFactionSettings()
        {
            ModOptions.FactionDictionary.InitializeFactionSettings();
        }

        private static void InitializeCustomClassActions()
        {
            //Conflicting trait levelset
            List<TraitDef> customTraits = new List<TraitDef>();
            customTraits.Clear();
            for(int i = 0; i < TM_ClassUtility.CustomClasses().Count; i++)
            {
                TMDefs.TM_CustomClass customClass = TM_ClassUtility.CustomClasses()[i];
                customTraits.AddDistinct(customClass.classTrait);
                customClass.classTrait.conflictingTraits.AddRange(TM_Data.AllClassTraits);
            }

            IEnumerable<TraitDef> enumerable = from def in DefDatabase<TraitDef>.AllDefs
                                               where (TM_Data.AllClassTraits.Contains(def))
                                               select def;

            foreach(TraitDef current in enumerable)
            {
                current.conflictingTraits.AddRange(customTraits);
            }

            for(int i = 0; i < customTraits.Count; i++)
            {
                for(int j = 0; j < customTraits.Count; j++)
                {
                    if(customTraits[i] != customTraits[j])
                    {
                        customTraits[i].conflictingTraits.Add(customTraits[j]);
                    }
                }
            }
        }

    }
}
