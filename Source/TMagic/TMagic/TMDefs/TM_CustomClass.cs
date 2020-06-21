﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using UnityEngine;

namespace TorannMagic.TMDefs
{
    public class TM_CustomClass
    {
        //Class Defining Trait
        public TraitDef classTrait = null;
        public int traitDegree = 4;
        public string classIconPath = "";
        public Color classIconColor = new Color(1f,1f,1f);
        public string classTexturePath = "";

        //Class Hediff
        public HediffDef classHediff = null;
        public float hediffSeverity = 1f;

        //Class Abilities
        public List<TMAbilityDef> classMageAbilities = new List<TMAbilityDef>();
        public List<TMAbilityDef> classFighterAbilities = new List<TMAbilityDef>();
        public List<ThingDef> learnableSpells = new List<ThingDef>();
        public List<ThingDef> learnableSkills = new List<ThingDef>();
        public List<PowerDef> customPowers = new List<PowerDef>();

        //Class Designations
        public bool isMage = false;
        public int maxMageLevel = 150;
        public bool isFighter = false;
        public int maxFighterLevel = 150;
        public bool isNecromancer = false;
        public bool isUndead = false;
        public bool isAndroid = false;
        public bool isAdvancedClass = false;

        //Class Items
        public ThingDef tornScript = null;
        public ThingDef fullScript = null;

        public List<TMAbilityDef> classAbilities
        {
            get
            {
                List<TMAbilityDef> allAbilities = new List<TMAbilityDef>();
                allAbilities.Clear();
                allAbilities.AddRange(classFighterAbilities);
                allAbilities.AddRange(classMageAbilities);
                return allAbilities;
            }
        }

    }
}