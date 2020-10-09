using UnityEngine;
using Verse;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using RimWorld;
using System;

namespace TorannMagic
{
    public static class TM_Data
    {
        public static List<ThingDef> SpellList()
        {
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where (def.defName.Contains("SpellOf_"))
                                               select def;
            return enumerable.ToList();
        }

        public static List<ThingDef> MasterSpellList()
        {
            List<ThingDef> masterSpellList = new List<ThingDef>();
            masterSpellList.Add(TorannMagicDefOf.SpellOf_Firestorm);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_Blizzard);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_EyeOfTheStorm);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_RegrowLimb);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_FoldReality);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_Resurrection);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_HolyWrath);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_LichForm);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_SummonPoppi);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_BattleHymn);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_PsychicShock);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_Scorn);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_Meteor);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_OrbitalStrike);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_BloodMoon);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_Shapeshift);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_Recall);
            masterSpellList.Add(TorannMagicDefOf.SpellOf_SpiritOfLight);
            return masterSpellList;
        }

        public static List<ThingDef> RestrictedAbilities
        {
            get
            {
                List<ThingDef> restricted = new List<ThingDef>();
                restricted.Add(TorannMagicDefOf.SpellOf_BattleHymn);
                restricted.Add(TorannMagicDefOf.SpellOf_BlankMind);
                restricted.Add(TorannMagicDefOf.SpellOf_Blizzard);
                restricted.Add(TorannMagicDefOf.SpellOf_BloodMoon);
                restricted.Add(TorannMagicDefOf.SpellOf_BriarPatch);
                restricted.Add(TorannMagicDefOf.SpellOf_CauterizeWound);
                restricted.Add(TorannMagicDefOf.SpellOf_ChargeBattery);
                restricted.Add(TorannMagicDefOf.SpellOf_DryGround);
                restricted.Add(TorannMagicDefOf.SpellOf_EyeOfTheStorm);
                restricted.Add(TorannMagicDefOf.SpellOf_FertileLands);
                restricted.Add(TorannMagicDefOf.SpellOf_Firestorm);
                restricted.Add(TorannMagicDefOf.SpellOf_FoldReality);
                restricted.Add(TorannMagicDefOf.SpellOf_HolyWrath);
                restricted.Add(TorannMagicDefOf.SpellOf_LichForm);
                restricted.Add(TorannMagicDefOf.SpellOf_MechaniteReprogramming);
                restricted.Add(TorannMagicDefOf.SpellOf_Meteor);
                restricted.Add(TorannMagicDefOf.SpellOf_OrbitalStrike);
                restricted.Add(TorannMagicDefOf.SpellOf_Overdrive);
                restricted.Add(TorannMagicDefOf.SpellOf_PsychicShock);
                restricted.Add(TorannMagicDefOf.SpellOf_Recall);
                restricted.Add(TorannMagicDefOf.SpellOf_RegrowLimb);
                restricted.Add(TorannMagicDefOf.SpellOf_Resurrection);
                restricted.Add(TorannMagicDefOf.SpellOf_Sabotage);
                restricted.Add(TorannMagicDefOf.SpellOf_Scorn);
                restricted.Add(TorannMagicDefOf.SpellOf_Shapeshift);
                restricted.Add(TorannMagicDefOf.SpellOf_SummonPoppi);
                restricted.Add(TorannMagicDefOf.SpellOf_TechnoShield);
                restricted.Add(TorannMagicDefOf.SpellOf_WetGround);
                restricted.Add(TorannMagicDefOf.SpellOf_SpiritOfLight);
                return restricted;
            }
        }

        public static List<ThingDef> StandardSpellList()
        {
            return SpellList().Except(MasterSpellList()).ToList();
        }

        public static List<ThingDef> StandardSkillList()
        {
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where (def.defName.Contains("SkillOf_"))
                                               select def;
            return enumerable.ToList();
        }

        public static List<ThingDef> FighterBookList()
        {
            List<ThingDef> fighterBookList = new List<ThingDef>();
            fighterBookList.Add(TorannMagicDefOf.BookOfGladiator);
            fighterBookList.Add(TorannMagicDefOf.BookOfBladedancer);
            fighterBookList.Add(TorannMagicDefOf.BookOfDeathKnight);
            fighterBookList.Add(TorannMagicDefOf.BookOfFaceless);
            fighterBookList.Add(TorannMagicDefOf.BookOfPsionic);
            fighterBookList.Add(TorannMagicDefOf.BookOfRanger);
            fighterBookList.Add(TorannMagicDefOf.BookOfSniper);
            fighterBookList.Add(TorannMagicDefOf.BookOfMonk);
            fighterBookList.Add(TorannMagicDefOf.BookOfCommander);
            fighterBookList.Add(TorannMagicDefOf.BookOfSuperSoldier);
            return fighterBookList;
        }

        public static List<ThingDef> MageBookList()
        {
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where (def.defName.Contains("BookOf"))
                                               select def;

            enumerable = enumerable.Except(MageTornScriptList());
            return enumerable.Except(FighterBookList()).ToList();
        }

        public static List<ThingDef> AllBooksList()
        {
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where (def.defName.Contains("BookOf"))
                                               select def;

            return enumerable.Except(MageTornScriptList()).ToList();
        }

        public static List<ThingDef> MageTornScriptList()
        {
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where (def.defName.Contains("Torn_BookOf"))
                                               select def;

            return enumerable.ToList();
        }

        public static List<TraitDef> MagicTraits
        {
            get
            {
                List<TraitDef> magicTraits = new List<TraitDef>();
                magicTraits.Clear();
                magicTraits.Add(TorannMagicDefOf.Arcanist);
                magicTraits.Add(TorannMagicDefOf.InnerFire);
                magicTraits.Add(TorannMagicDefOf.HeartOfFrost);
                magicTraits.Add(TorannMagicDefOf.StormBorn);
                magicTraits.Add(TorannMagicDefOf.Druid);
                magicTraits.Add(TorannMagicDefOf.Priest);
                magicTraits.Add(TorannMagicDefOf.Necromancer);
                magicTraits.Add(TorannMagicDefOf.Technomancer);
                magicTraits.Add(TorannMagicDefOf.Geomancer);
                magicTraits.Add(TorannMagicDefOf.Warlock);
                magicTraits.Add(TorannMagicDefOf.Succubus);
                magicTraits.Add(TorannMagicDefOf.ChaosMage);
                magicTraits.Add(TorannMagicDefOf.Paladin);
                magicTraits.Add(TorannMagicDefOf.Summoner);
                magicTraits.Add(TorannMagicDefOf.Lich);
                magicTraits.Add(TorannMagicDefOf.TM_Bard);
                magicTraits.Add(TorannMagicDefOf.Chronomancer);
                magicTraits.Add(TorannMagicDefOf.Enchanter);
                magicTraits.Add(TorannMagicDefOf.BloodMage);
                magicTraits.Add(TorannMagicDefOf.TM_Wanderer);
                magicTraits.Add(TorannMagicDefOf.TM_Gifted);
                return magicTraits;
            }
        }

        public static List<TraitDef> MightTraits
        {
            get
            {
                List<TraitDef> mightTraits = new List<TraitDef>();
                mightTraits.Clear();
                mightTraits.Add(TorannMagicDefOf.Bladedancer);
                mightTraits.Add(TorannMagicDefOf.Gladiator);
                mightTraits.Add(TorannMagicDefOf.Faceless);
                mightTraits.Add(TorannMagicDefOf.TM_Sniper);
                mightTraits.Add(TorannMagicDefOf.Ranger);
                mightTraits.Add(TorannMagicDefOf.TM_Psionic);
                mightTraits.Add(TorannMagicDefOf.TM_Monk);
                mightTraits.Add(TorannMagicDefOf.TM_Commander);
                mightTraits.Add(TorannMagicDefOf.TM_SuperSoldier);
                mightTraits.Add(TorannMagicDefOf.TM_Wayfarer);
                mightTraits.Add(TorannMagicDefOf.PhysicalProdigy);
                return mightTraits;
            }
        }

        public static List<TraitDef> AllClassTraits
        {
            get
            {
                List<TraitDef> allClassTraits = new List<TraitDef>();
                allClassTraits.Clear();
                allClassTraits.AddRange(MightTraits);
                allClassTraits.AddRange(MagicTraits);
                return allClassTraits;
            }
        }

        public static List<ThingDef> MagicFociList()
        {
            List<ThingDef> magicFocis = new List<ThingDef>();
            magicFocis.Clear();
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where (true)
                                               select def;
            List<string> magicFociList = WeaponCategoryList.Named("TM_Category_MagicalFoci").weaponDefNames;
            foreach (ThingDef current in enumerable)
            {
                for (int i = 0; i < magicFociList.Count; i++)
                {
                    if (current.defName == magicFociList[i].ToString() || magicFociList[i].ToString() == "*")
                    {
                        //Log.Message("adding magicFoci def " + current.defName);
                        magicFocis.AddDistinct(current);
                    }
                }
            }
            return magicFocis;
        }

        public static List<ThingDef> BowList()
        {
            List<ThingDef> bows = new List<ThingDef>();
            bows.Clear();
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where (true)
                                               select def;
            List<string> bowList = WeaponCategoryList.Named("TM_Category_Bows").weaponDefNames;
            foreach (ThingDef current in enumerable)
            {
                for (int i = 0; i < bowList.Count; i++)
                {
                    if (current.defName == bowList[i].ToString() || bowList[i].ToString() == "*")
                    {
                        //Log.Message("adding bow def " + current.defName);
                        bows.AddDistinct(current);
                    }
                }
            }
            return bows;
        }

        public static List<ThingDef> PistolList()
        {
            List<ThingDef> pistols = new List<ThingDef>();
            pistols.Clear();
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where (true)
                                               select def;
            List<string> pistolList = WeaponCategoryList.Named("TM_Category_Pistols").weaponDefNames;
            foreach (ThingDef current in enumerable)
            {
                for (int i = 0; i < pistolList.Count; i++)
                {
                    if (current.defName == pistolList[i].ToString() || pistolList[i].ToString() == "*")
                    {
                        //Log.Message("adding pistol def " + current.defName);
                        pistols.AddDistinct(current);
                    }
                }
            }
            return pistols;
        }

        public static List<ThingDef> RifleList()
        {
            List<ThingDef> rifles = new List<ThingDef>();
            rifles.Clear();
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where (true)
                                               select def;
            List<string> rifleList = WeaponCategoryList.Named("TM_Category_Rifles").weaponDefNames;
            foreach (ThingDef current in enumerable)
            {
                for (int i = 0; i < rifleList.Count; i++)
                {
                    if (current.defName == rifleList[i].ToString() || rifleList[i].ToString() == "*")
                    {
                        //Log.Message("adding rifle def " + current.defName);
                        rifles.AddDistinct(current);
                    }
                }
            }
            return rifles;
        }

        public static List<ThingDef> ShotgunList()
        {
            List<ThingDef> shotguns = new List<ThingDef>();
            shotguns.Clear();
            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                               where (true)
                                               select def;
            List<string> shotgunList = WeaponCategoryList.Named("TM_Category_Shotguns").weaponDefNames;
            foreach (ThingDef current in enumerable)
            {
                for (int i = 0; i < shotgunList.Count; i++)
                {
                    if (current.defName == shotgunList[i].ToString() || shotgunList[i].ToString() == "*")
                    {
                        //Log.Message("adding shotgun def " + current.defName);
                        shotguns.AddDistinct(current);
                    }
                }
            }
            return shotguns;
        }

        public static IEnumerable<TM_CustomPowerDef> CustomFighterPowerDefs()
        {
            IEnumerable<TM_CustomPowerDef> enumerable = from def in DefDatabase<TM_CustomPowerDef>.AllDefs
                                                        where (def.customPower != null && def.customPower.forFighter)
                                                        select def;
            return enumerable;
        }

        public static IEnumerable<TM_CustomPowerDef> CustomMagePowerDefs()
        {
            IEnumerable<TM_CustomPowerDef> enumerable = from def in DefDatabase<TM_CustomPowerDef>.AllDefs
                                                        where (def.customPower != null && def.customPower.forMage)
                                                        select def;
            return enumerable;
        }

    }
}
