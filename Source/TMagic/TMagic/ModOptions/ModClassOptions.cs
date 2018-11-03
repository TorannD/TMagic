using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using UnityEngine;
using System.Text;
using System.Reflection;
using Harmony;

namespace TorannMagic.ModOptions
{
    internal class ModClassOptions : Mod
    {
        public ModClassOptions(ModContentPack mcp) : base(mcp)
        {
            LongEventHandler.ExecuteWhenFinished(new Action(ModClassOptions.RestrictClasses));
        }

        private static void RestrictClasses()
        {
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();

            IEnumerable<ThingDef> enumerable = (from def in DefDatabase<ThingDef>.AllDefs
                                                select def);
            List<ThingDef> removedThings = new List<ThingDef>();

            foreach (ThingDef current in enumerable)
            {
                if (!settingsRef.Sniper)
                {
                    if(current.defName == "BookOfSniper")
                    {
                        removedThings.Add(current);                     
                    }
                }
                if (!settingsRef.Ranger)
                {
                    if (current.defName == "BookOfRanger" || current.defName == "TM_PoisonTrap")
                    {
                        removedThings.Add(current);
                    }
                }
                if (!settingsRef.Gladiator)
                {
                    if (current.defName == "BookOfGladiator")
                    {
                        removedThings.Add(current);
                    }
                }
                if (!settingsRef.Bladedancer)
                {
                    if (current.defName == "BookOfBladedancer")
                    {
                        removedThings.Add(current);
                    }
                }
                if (!settingsRef.Faceless)
                {
                    if (current.defName == "BookOfFaceless")
                    {
                        removedThings.Add(current);
                    }
                }
                if (!settingsRef.Psionic)
                {
                    if (current.defName == "BookOfPsionic")
                    {
                        removedThings.Add(current);
                    }
                }

                if (!settingsRef.Arcanist)
                {
                    if (current.defName == "Torn_BookOfArcanist" || current.defName == "BookOfArcanist" || current.defName == "SpellOf_FoldReality")
                    {
                        removedThings.Add(current);
                    }
                }
                if (!settingsRef.FireMage)
                {
                    if (current.defName == "Torn_BookOfInnerFire" || current.defName == "BookOfInnerFire" || current.defName == "SpellOf_Firestorm" || current.defName == "SpellOf_DryGround")
                    {
                        removedThings.Add(current);
                    }
                }
                if (!settingsRef.IceMage)
                {
                    if (current.defName == "Torn_BookOfHeartOfFrost" || current.defName == "BookOfHeartOfFrost" || current.defName == "SpellOf_Blizzard" || current.defName == "SpellOf_WetGround")
                    {
                        removedThings.Add(current);
                    }
                }
                if (!settingsRef.LitMage)
                {
                    if (current.defName == "Torn_BookOfStormBorn" || current.defName == "BookOfStormBorn" || current.defName == "SpellOf_EyeOfTheStorm" || current.defName == "SpellOf_ChargeBattery")
                    {
                        removedThings.Add(current);
                    }
                }
                if (!settingsRef.Druid)
                {
                    if (current.defName == "Torn_BookOfNature" || current.defName == "BookOfNature" || current.defName == "SpellOf_RegrowLimb" || current.defName == "SeedofRegrowth" || current.defName == "SpellOf_FertileLands")
                    {
                        removedThings.Add(current);
                    }
                }
                if (!settingsRef.Summoner)
                {
                    if (current.defName == "Torn_BookOfSummoner" || current.defName == "BookOfSummoner" || current.defName == "SpellOf_SummonPoppi" ||
                        current.defName == "TM_ManaMine" || current.defName == "TM_ManaMine_I" || current.defName == "TM_ManaMine_II" || current.defName == "TM_ManaMine_III" ||
                        current.defName == "DefensePylon" || current.defName == "DefensePylon_I" || current.defName == "DefensePylon_II" || current.defName == "DefensePylon_III" || current.defName == "Bullet_DefensePylon" ||
                        current.defName == "Launcher_DefensePylon" || current.defName == "Launcher_DefensePylon_I" || current.defName == "Launcher_DefensePylon_II" || current.defName == "Launcher_DefensePylon_III" ||
                        current.defName == "TM_Poppi")
                    {
                        removedThings.Add(current);
                    }
                }
                if (!settingsRef.Paladin)
                {
                    if (current.defName == "Torn_BookOfValiant" || current.defName == "BookOfValiant" || current.defName == "SpellOf_HolyWrath")
                    {
                        removedThings.Add(current);
                    }
                }
                if (!settingsRef.Priest)
                {
                    if (current.defName == "Torn_BookOfPriest" || current.defName == "BookOfPriest" || current.defName == "SpellOf_Resurrection")
                    {
                        removedThings.Add(current);
                    }
                }
                if (!settingsRef.Bard)
                {
                    if (current.defName == "Torn_BookOfBard" || current.defName == "BookOfBard" || current.defName == "SpellOf_BattleHymn")
                    {
                        removedThings.Add(current);
                    }
                }
                if (!settingsRef.Necromancer)
                {
                    if (current.defName == "Torn_BookOfArcanist" || current.defName == "BookOfArcanist" || current.defName == "SpellOf_FoldReality")
                    {
                        removedThings.Add(current);
                    }
                }
                if (!settingsRef.Geomancer)
                {
                    if (current.defName == "Torn_BookOfEarth" || current.defName == "BookOfEarth" || current.defName == "SpellOf_Meteor" ||
                        current.defName == "TM_Lesser_SentinelR" || current.defName == "TM_SentinelR" || current.defName == "TM_Greater_SentinelR")
                    {
                        removedThings.Add(current);
                    }
                }
                if (!settingsRef.Demonkin)
                {
                    if (current.defName == "Torn_BookOfDemons" || current.defName == "BookOfDemons" || current.defName == "SpellOf_Scorn" || current.defName == "SpellOf_PsychicShock")
                    {
                        removedThings.Add(current);
                    }
                }
                if (!settingsRef.Demonkin)
                {
                    if (current.defName == "Torn_BookOfMagitech" || current.defName == "BookOfMagitech" || current.defName == "SpellOf_TechnoShield" || current.defName == "SpellOf_Sabotage" || current.defName == "SpellOf_Overdrive" || current.defName == "SpellOf_OrbitalStrike")
                    {
                        removedThings.Add(current);
                    }
                }

            }

            for (int i = 0; i < removedThings.Count(); i++)
            {
                //Log.Message("removing " + removedThings[i].defName + " from def database");
                DefDatabase<ThingDef>.AllDefsListForReading.Remove(removedThings[i]);
            }

            IEnumerable<RecipeDef> RecipeEnumerable = (from def in DefDatabase<RecipeDef>.AllDefs
                                                select def);
            List<RecipeDef> removedRecipes = new List<RecipeDef>();

            foreach (RecipeDef current in RecipeEnumerable)
            {
                if (!settingsRef.Arcanist)
                {
                    if (current.defName == "SpellOf_FoldReality")
                    {
                        removedRecipes.Add(current);
                    }
                }
                if (!settingsRef.FireMage)
                {
                    if (current.defName == "Make_Make_SpellOf_Firestorm" || current.defName == "Make_Make_SpellOf_DryGround")
                    {
                        removedRecipes.Add(current);
                    }
                }
                if (!settingsRef.IceMage)
                {
                    if (current.defName == "Make_SpellOf_Blizzard" || current.defName == "Make_SpellOf_WetGround")
                    {
                        removedRecipes.Add(current);
                    }
                }
                if (!settingsRef.LitMage)
                {
                    if (current.defName == "Make_SpellOf_EyeOfTheStorm" || current.defName == "Make_SpellOf_ChargeBattery")
                    {
                        removedRecipes.Add(current);
                    }
                }
                if (!settingsRef.Druid)
                {
                    if (current.defName == "Make_SpellOf_RegrowLimb" || current.defName == "Make_SpellOf_FertileLands")
                    {
                        removedRecipes.Add(current);
                    }
                }
                if (!settingsRef.Summoner)
                {
                    if (current.defName == "Make_SpellOf_SummonPoppi")
                    {
                        removedRecipes.Add(current);
                    }
                }
                if (!settingsRef.Paladin)
                {
                    if (current.defName == "Make_SpellOf_HolyWrath")
                    {
                        removedRecipes.Add(current);
                    }
                }
                if (!settingsRef.Priest)
                {
                    if (current.defName == "Make_SpellOf_Resurrection")
                    {
                        removedRecipes.Add(current);
                    }
                }
                if (!settingsRef.Bard)
                {
                    if (current.defName == "Make_SpellOf_BattleHymn")
                    {
                        removedRecipes.Add(current);
                    }
                }
                if (!settingsRef.Necromancer)
                {
                    if (current.defName == "Make_SpellOf_FoldReality")
                    {
                        removedRecipes.Add(current);
                    }
                }
                if (!settingsRef.Geomancer)
                {
                    if (current.defName == "Make_SpellOf_Meteor")
                    {
                        removedRecipes.Add(current);
                    }
                }
                if (!settingsRef.Demonkin)
                {
                    if (current.defName == "Make_SpellOf_Scorn" || current.defName == "Make_SpellOf_PsychicShock")
                    {
                        removedRecipes.Add(current);
                    }
                }
                if (!settingsRef.Technomancer)
                {
                    if (current.defName == "Make_SpellOf_TechnoShield" || current.defName == "Make_SpellOf_Sabotage" || current.defName == "Make_SpellOf_Overdrive" || current.defName == "Make_SpellOf_OrbitalStrike")
                    {
                        removedRecipes.Add(current);
                    }
                }
            }

            for (int i = 0; i < removedRecipes.Count(); i++)
            {
                //Log.Message("removing " + removedRecipes[i].defName + " from def database");
                DefDatabase<RecipeDef>.AllDefsListForReading.Remove(removedRecipes[i]);
            }
            
        }

        private static void RemoveStuffFromDatabase(Type databaseType, IEnumerable<Def> defs)
        {
            IEnumerable<Def> enumerable = (defs as Def[]) ?? defs.ToArray();
            if (enumerable.Any())
            {
                Traverse traverse = Traverse.Create(databaseType).Method("Remove", enumerable.First());
                foreach (Def item in enumerable)
                {
                    //Log.Message("- " + item.label);
                    traverse.GetValue(item);
                }
            }
        }
    }
}
