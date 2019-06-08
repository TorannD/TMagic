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
            return masterSpellList;
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

    }
}
