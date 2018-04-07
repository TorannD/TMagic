using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;
using AbilityUser;
using Verse.AI;
using UnityEngine;
using System.Text;

namespace TorannMagic
{
    [CompilerGenerated]
    [Serializable]
    public class CompAbilityUserMight : CompAbilityUser
    {
        public string LabelKey = "TM_Might";

        public bool mightPowersInitialized = false;
        public bool firstMightTick = false;
        private int age = -1;
        private int fortitudeMitigationDelay = 0;
        private int mightXPRate = 1200;
        private int lastMightXPGain = 0;

        private float G_Sprint_eff = 0.20f;
        private float G_Grapple_eff = 0.10f;
        private float G_Cleave_eff = 0.10f;
        private float G_Whirlwind_eff = 0.10f;
        private float S_Headshot_eff = 0.10f;
        private float S_DisablingShot_eff = 0.10f;
        private float S_AntiArmor_eff = .10f;
        private float B_SeismicSlash_eff = 0.10f;
        private float B_BladeSpin_eff = 0.10f;
        private float B_PhaseStrike_eff = 0.08f;
        private float R_AnimalFriend_eff = 0.15f;
        private float R_ArrowStorm_eff = 0.08f;

        private float global_seff = 0.03f;

        public bool skill_Sprint = false;
        public bool skill_GearRepair = false;
        public bool skill_InnerHealing = false;
        public bool skill_HeavyBlow = false;
        public bool skill_StrongBack = false;
        public bool skill_ThickSkin = false;
        public bool skill_FightersFocus = false;

        public float maxSP = 1;
        public float spRegenRate = 1;
        public float coolDown = 1;
        public float spCost = 1;
        public float xpGain = 1;

        public bool animalBondingDisabled = false;
        private int animalFriendDisabledPeriod;        

        public List<Thing> combatItems = new List<Thing>();

        public Pawn bondedPet = null;

        private MightData mightData = null;
        public MightData MightData
        {
            get
            {
                bool flag = this.mightData == null && this.IsMightUser;
                if (flag)
                {
                    this.mightData = new MightData(this);
                }
                return this.mightData;
            }
        }

        public static List<TMAbilityDef> MightAbilities = null;    
        
        public int LevelUpSkill_global_refresh(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_global_refresh.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }        
        public int LevelUpSkill_global_seff(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_global_seff.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }        
        public int LevelUpSkill_global_strength(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_global_strength.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_global_endurance(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_global_endurance.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_Sprint(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Sprint.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Fortitude(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Fortitude.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Grapple(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Grapple.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Cleave(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Cleave.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Whirlwind(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Whirlwind.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_SniperFocus(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_SniperFocus.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Headshot(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Headshot.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_DisablingShot(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_DisablingShot.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_AntiArmor(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_AntiArmor.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_BladeFocus(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_BladeFocus.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_BladeArt(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_BladeArt.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_SeismicSlash(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_SeismicSlash.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_BladeSpin(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_BladeSpin.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_PhaseStrike(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_PhaseStrike.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_RangerTraining(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_RangerTraining.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_BowTraining(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_BowTraining.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_PoisonTrap(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_PoisonTrap.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_AnimalFriend(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_AnimalFriend.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_ArrowStorm(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_ArrowStorm.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }

        public override void CompTick()
        {
            bool flag = base.AbilityUser != null;
            if (flag)
            {
                bool spawned = base.AbilityUser.Spawned;
                if (spawned)
                {
                    bool isMightUser = this.IsMightUser;
                    if (isMightUser)
                    {
                        bool flag3 = !this.MightData.Initialized;
                        if (flag3)
                        {
                            this.PostInitializeTick();
                        }
                        base.CompTick();
                        this.age++;
                        if(Find.TickManager.TicksGame % 20 == 0)
                        {
                            ResolveSustainedSkills();
                            ResolveClassSkills();
                        }
                        if (Find.TickManager.TicksGame % 3600 == 0)
                        {
                            ResolveClassSkills();
                        }
                        if (this.Stamina.CurLevel >  (.99f * this.Stamina.MaxLevel))
                        {                            
                            if (this.age > (lastMightXPGain + mightXPRate))
                            {
                                MightData.MightUserXP++;
                                lastMightXPGain = this.age;
                            }
                        }
                        bool flag4 = Find.TickManager.TicksGame % 30 == 0;
                        if (flag4)
                        {
                            bool flag5 = this.MightUserXP > this.MightUserXPTillNextLevel;
                            if (flag5)
                            {
                                this.LevelUp(false);
                            }
                        }
                    }
                }
            }
            if (IsInitialized)
            {
                //custom code
            }
        }

        public void PostInitializeTick()
        {
            bool flag = base.AbilityUser != null;
            if (flag)
            {
                bool spawned = base.AbilityUser.Spawned;
                if (spawned)
                {
                    bool flag2 = base.AbilityUser.story != null;
                    if (flag2)
                    {
                        //this.MightData.Initialized = true;
                        this.Initialize();
                        this.ResolveMightTab();
                        this.ResolveMightPowers();
                        this.ResolveStamina();
                    }
                }
            }
        }

        public bool IsMightUser
        {
            get
            {
                bool flag = base.AbilityUser != null;
                if (flag)
                {
                    bool flag3 = base.AbilityUser.story != null;
                    if (flag3)
                    {
                        bool flag4 = base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Gladiator) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Ranger);
                        if (flag4)
                        {                            
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public int MightUserLevel
        {
            get
            {
                return this.MightData.MightUserLevel;
            }
            set
            {
                bool flag = value > this.MightData.MightUserLevel;
                if (flag)
                {
                    this.MightData.MightAbilityPoints++;
                    bool flag2 = this.MightData.MightUserXP < value * 500;
                    if (flag2)
                    {
                        this.MightData.MightUserXP = value * 500;
                    }
                }
                this.MightData.MightUserLevel = value;
            }
        }

        public int MightUserXP
        {
            get
            {
                return this.MightData.MightUserXP;
            }
            set
            {
                this.MightData.MightUserXP = value;
            }
        }

        public float XPLastLevel
        {
            get
            {
                float result = 0f;
                bool flag = this.MightUserLevel > 0;
                if (flag)
                {
                    result = (float)(this.MightUserLevel * 500);
                }
                return result;
            }
        }

        public float XPTillNextLevelPercent
        {
            get
            {
                return ((float)this.MightData.MightUserXP - this.XPLastLevel) / ((float)this.MightUserXPTillNextLevel - this.XPLastLevel);
            }
        }

        public int MightUserXPTillNextLevel
        {
            get
            {
                return (this.MightData.MightUserLevel + 1) * 500; 
            }
        }

        public void LevelUp(bool hideNotification = false)
        {
            this.MightUserLevel++;
            bool flag = !hideNotification;
            if (flag)
            {
                Messages.Message(Translator.Translate("TM_MightLevelUp", new object[]
                {
                    this.parent.Label
                }), MessageTypeDefOf.PositiveEvent);
            }
        }

        public void LevelUpPower(MightPower power)
        {
            foreach (AbilityDef current in power.TMabilityDefs)
            {
                base.RemovePawnAbility(current);
            }
            power.level++;
            base.AddPawnAbility(power.nextLevelAbilityDef, true, -1f);
            this.UpdateAbilities();
        }

        public Need_Stamina Stamina
        {
            get
            {
                return base.AbilityUser.needs.TryGetNeed<Need_Stamina>();
            }
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            bool flag = CompAbilityUserMight.MightAbilities == null;
            if (flag)
            {
                if (this.mightPowersInitialized == false)
                {
                    Pawn abilityUser = base.AbilityUser;
                    bool flag2;
                    MightData.MightUserLevel = 0;
                    MightData.MightAbilityPoints = 0;

                    flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Gladiator);
                    if (flag2)
                    {
                        //Log.Message("Initializing Gladiator Abilities");
                        this.AddPawnAbility(TorannMagicDefOf.TM_Sprint);
                        //this.AddPawnAbility(TorannMagicDefOf.TM_Fortitude);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Grapple);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Cleave);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Whirlwind);
                    }
                    flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper);
                    if (flag2)
                    {
                        //Log.Message("Initializing Sniper Abilities");
                        //this.AddPawnAbility(TorannMagicDefOf.TM_SniperFocus);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Headshot);
                        this.AddPawnAbility(TorannMagicDefOf.TM_DisablingShot);
                        this.AddPawnAbility(TorannMagicDefOf.TM_AntiArmor);
                    }
                    flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Bladedancer);
                    if (flag2)
                    {
                       // Log.Message("Initializing Bladedancer Abilities");
                       // this.AddPawnAbility(TorannMagicDefOf.TM_BladeFocus);
                        //this.AddPawnAbility(TorannMagicDefOf.TM_BladeArt);
                        this.AddPawnAbility(TorannMagicDefOf.TM_SeismicSlash);
                        this.AddPawnAbility(TorannMagicDefOf.TM_BladeSpin);
                        this.AddPawnAbility(TorannMagicDefOf.TM_PhaseStrike);
                    }
                    flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Ranger);
                    if (flag2)
                    {
                        //Log.Message("Initializing Ranger Abilities");
                        //this.AddPawnAbility(TorannMagicDefOf.TM_RangerTraining);
                       // this.AddPawnAbility(TorannMagicDefOf.TM_BowTraining);
                        this.AddPawnAbility(TorannMagicDefOf.TM_PoisonTrap);
                        this.AddPawnAbility(TorannMagicDefOf.TM_AnimalFriend);
                        this.AddPawnAbility(TorannMagicDefOf.TM_ArrowStorm);
                    }
                    this.mightPowersInitialized = true;
                    //base.UpdateAbilities();

                }
                //this.UpdateAbilities();
                //base.UpdateAbilities();
            }
        }

        public void InitializeSkill()  //used for class independant skills
        {
            Pawn abilityUser = base.AbilityUser;

            if (this.skill_Sprint == true && !abilityUser.story.traits.HasTrait(TorannMagicDefOf.Gladiator))
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint);
                this.AddPawnAbility(TorannMagicDefOf.TM_Sprint);
            }
            if (this.skill_GearRepair == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_GearRepair);
                this.AddPawnAbility(TorannMagicDefOf.TM_GearRepair);
            }
            if (this.skill_InnerHealing == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_InnerHealing);
                this.AddPawnAbility(TorannMagicDefOf.TM_InnerHealing);
            }
            if (this.skill_StrongBack == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_StrongBack);
                this.AddPawnAbility(TorannMagicDefOf.TM_StrongBack);
            }
            if (this.skill_HeavyBlow == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_HeavyBlow);
                this.AddPawnAbility(TorannMagicDefOf.TM_HeavyBlow);
            }
            if (this.skill_ThickSkin == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_ThickSkin);
                this.AddPawnAbility(TorannMagicDefOf.TM_ThickSkin);
            }
            if (this.skill_FightersFocus == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_FightersFocus);
                this.AddPawnAbility(TorannMagicDefOf.TM_FightersFocus);
            }

        }

        public void FixPowers()
        {
            Pawn abilityUser = base.AbilityUser;
            if (this.mightPowersInitialized == true)
            {
                bool flag2;
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Gladiator);
                if (flag2)
                {
                    Log.Message("Fixing Gladiator Abilities");
                    foreach (MightPower currentG in this.MightData.MightPowersG)
                    {
                        if (currentG.abilityDef.defName == "TM_Sprint" || currentG.abilityDef.defName == "TM_Sprint_I" || currentG.abilityDef.defName == "TM_Sprint_II" || currentG.abilityDef.defName == "TM_Sprint_III")
                        {
                            if (currentG.level == 0)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_I);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_II);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_III);
                            }
                            else if (currentG.level == 1)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_II);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_III);
                            }
                            else if (currentG.level == 2)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_I);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_III);
                            }
                            else if (currentG.level == 3)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_II);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_I);
                            }
                            else
                            {
                                Log.Message("Ability level not found to fix");
                            }
                        }
                        if (currentG.abilityDef.defName == "TM_Grapple" || currentG.abilityDef.defName == "TM_Grapple_I" || currentG.abilityDef.defName == "TM_Grapple_II" || currentG.abilityDef.defName == "TM_Grapple_III")
                        {
                            if (currentG.level == 0)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_I);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_II);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_III);
                            }
                            else if (currentG.level == 1)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_II);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_III);
                            }
                            else if (currentG.level == 2)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_I);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_III);
                            }
                            else if (currentG.level == 3)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_II);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_I);
                            }
                            else
                            {
                                Log.Message("Ability level not found to fix");
                            }
                        }
                    }            
                }
            }
            //this.UpdateAbilities();
            //base.UpdateAbilities();
        }

        public override bool TryTransformPawn()
        {
            return this.IsMightUser;
        }

        public bool TryAddPawnAbility(TMAbilityDef ability)
        {
            //add check to verify no ability is already added
            bool result = false;
            base.AddPawnAbility(ability, true, -1f);
            result = true;
            return result;
        }        

        public void ClearPowers()
        {
            int tmpLvl = this.MightUserLevel;
            int tmpExp = this.MightUserXP;
            base.IsInitialized = false;
            this.mightData = null;
            this.CompTick();
            this.MightUserLevel = tmpLvl;
            this.MightUserXP = tmpExp;
            this.mightData.MightAbilityPoints = tmpLvl;
        }

        private void ClearPower(MightPower current)
        {
            Log.Message("Removing ability: " + current.abilityDef.defName.ToString());
            base.RemovePawnAbility(current.abilityDef);
            base.UpdateAbilities();
        }

        public void ClearTraits()
        {
            List<Trait> traits = this.AbilityUser.story.traits.allTraits;
            for (int i = 0; i < traits.Count; i++)
            {
                if (traits[i].def.defName == "Gladiator")
                {
                    Log.Message("Removing trait " + traits[i].def.defName);
                    traits.Remove(traits[i]);
                    i--;
                }
                if (traits[i].def.defName == "TM_Sniper")
                {
                    Log.Message("Removing trait " + traits[i].def.defName);
                    traits.Remove(traits[i]);
                    i--;
                }
                if (traits[i].def.defName == "Bladedancer")
                {
                    Log.Message("Removing trait " + traits[i].def.defName);
                    traits.Remove(traits[i]);
                    i--;
                }
                if (traits[i].def.defName == "Ranger")
                {
                    Log.Message("Removing trait " + traits[i].def.defName);
                    traits.Remove(traits[i]);
                    i--;
                }
            }
        }

        private void LoadPowers(Pawn pawn)
        {
            if (pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator))
            {
                foreach (MightPower currentG in this.MightData.MightPowersG)
                {
                    Log.Message("Removing ability: " + currentG.abilityDef.defName.ToString());
                    currentG.level = 0;
                    base.RemovePawnAbility(currentG.abilityDef);
                }
                
            }
        }

        public int MightAttributeEffeciencyLevel(string attributeName)
        {
            int result = 0;

            if (attributeName == "TM_Sprint_eff")
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Sprint.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = mightPowerSkill != null;
                if (flag)
                {
                    result = mightPowerSkill.level;
                }
            }
            if (attributeName == "TM_Fortitude_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_Fortitude.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Grapple_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_Grapple.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Cleave_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_Cleave.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Whirlwind_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_Whirlwind.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Headshot_eff")
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Headshot.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = mightPowerSkill != null;
                if (flag)
                {
                    result = mightPowerSkill.level;
                }
            }
            if (attributeName == "TM_DisablingShot_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_DisablingShot.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_AntiArmor_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_AntiArmor.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_SeismicSlash_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_SeismicSlash.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_BladeSpin_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_BladeSpin.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_PhaseStrike_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_PhaseStrike.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_AnimalFriend_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_AnimalFriend.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_ArrowStorm_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_ArrowStorm.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }

            return result;
        }

        public float ActualStaminaCost(TMAbilityDef mightDef)
        {
            float adjustedStaminaCost = mightDef.staminaCost;
            if (mightDef == TorannMagicDefOf.TM_Sprint || mightDef == TorannMagicDefOf.TM_Sprint_I || mightDef == TorannMagicDefOf.TM_Sprint_II || mightDef == TorannMagicDefOf.TM_Sprint_III)
            {
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.G_Sprint_eff * (float)this.MightAttributeEffeciencyLevel("TM_Sprint_eff"));
            }
            if (mightDef == TorannMagicDefOf.TM_Grapple || mightDef == TorannMagicDefOf.TM_Grapple_I || mightDef == TorannMagicDefOf.TM_Grapple_II || mightDef == TorannMagicDefOf.TM_Grapple_III)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Grapple.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Grapple_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.G_Grapple_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_Cleave)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Cleave.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Cleave_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.G_Cleave_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_Whirlwind)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Whirlwind.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Whirlwind_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.G_Whirlwind_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_Headshot)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Headshot.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Headshot_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.S_Headshot_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_DisablingShot || mightDef == TorannMagicDefOf.TM_DisablingShot_I || mightDef == TorannMagicDefOf.TM_DisablingShot_II || mightDef == TorannMagicDefOf.TM_DisablingShot_III)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_DisablingShot.FirstOrDefault((MightPowerSkill x) => x.label == "TM_DisablingShot_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.S_DisablingShot_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_AntiArmor)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_AntiArmor.FirstOrDefault((MightPowerSkill x) => x.label == "TM_AntiArmor_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.S_AntiArmor_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_SeismicSlash)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_SeismicSlash.FirstOrDefault((MightPowerSkill x) => x.label == "TM_SeismicSlash_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.B_SeismicSlash_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_BladeSpin)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_BladeSpin.FirstOrDefault((MightPowerSkill x) => x.label == "TM_BladeSpin_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.B_BladeSpin_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_PhaseStrike)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_PhaseStrike.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PhaseStrike_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.B_PhaseStrike_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_AnimalFriend)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_AnimalFriend.FirstOrDefault((MightPowerSkill x) => x.label == "TM_AnimalFriend_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.R_AnimalFriend_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_ArrowStorm || mightDef == TorannMagicDefOf.TM_ArrowStorm_I || mightDef == TorannMagicDefOf.TM_ArrowStorm_II || mightDef == TorannMagicDefOf.TM_ArrowStorm_III)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_ArrowStorm.FirstOrDefault((MightPowerSkill x) => x.label == "TM_ArrowStorm_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.R_ArrowStorm_eff * (float)mightPowerSkill.level);
            }

            MightPowerSkill globalSkill = this.MightData.MightPowerSkill_global_seff.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_seff_pwr");
            if (globalSkill != null)
            {
                return (adjustedStaminaCost - (adjustedStaminaCost * (global_seff * globalSkill.level)));
            }
            else
            {
                return adjustedStaminaCost;
            }

        }

        public override List<HediffDef> IgnoredHediffs()
        {
            return new List<HediffDef>
            {
                TorannMagicDefOf.TM_MagicUserHD
            };
        }

        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            Pawn abilityUser = base.AbilityUser;
            absorbed = false;
            bool isGladiator = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Gladiator);
            if (isGladiator)
            {
                List<Hediff> list = new List<Hediff>();
                List<Hediff> arg_32_0 = list;
                IEnumerable<Hediff> arg_32_1;
                if (abilityUser == null)
                {
                    arg_32_1 = null;
                }
                else
                {
                    Pawn_HealthTracker expr_1A = abilityUser.health;
                    if (expr_1A == null)
                    {
                        arg_32_1 = null;
                    }
                    else
                    {
                        HediffSet expr_26 = expr_1A.hediffSet;
                        arg_32_1 = ((expr_26 != null) ? expr_26.hediffs : null);
                    }
                }
                arg_32_0.AddRange(arg_32_1);
                Pawn expr_3E = abilityUser;
                int? arg_84_0;
                if (expr_3E == null)
                {
                    arg_84_0 = null;
                }
                else
                {
                    Pawn_HealthTracker expr_52 = expr_3E.health;
                    if (expr_52 == null)
                    {
                        arg_84_0 = null;
                    }
                    else
                    {
                        HediffSet expr_66 = expr_52.hediffSet;
                        arg_84_0 = ((expr_66 != null) ? new int?(expr_66.hediffs.Count<Hediff>()) : null);
                    }
                }
                bool flag = (arg_84_0 ?? 0) > 0;
                if (flag)
                {
                    foreach (Hediff current in list)
                    {
                        if (current.def.defName == "TM_HediffInvulnerable")
                        {
                            absorbed = true;
                            MoteMaker.MakeStaticMote(AbilityUser.Position, AbilityUser.Map, ThingDefOf.Mote_ExplosionFlash, 10);
                            dinfo.SetAmount(0);
                            return;
                        }
                        if (fortitudeMitigationDelay < this.age )
                        {
                            if (current.def.defName == "TM_HediffFortitude")
                            {
                                MightPowerSkill pwr = this.MightData.MightPowerSkill_Fortitude.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Fortitude_pwr");
                                MightPowerSkill ver = this.MightData.MightPowerSkill_Fortitude.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Fortitude_ver");
                                absorbed = true;
                                int mitigationAmt = 2 + (2 * pwr.level);
                                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                                if (settingsRef.AIHardMode && !abilityUser.IsColonist)
                                {
                                    mitigationAmt = 8;
                                }
                                int actualDmg;
                                int dmgAmt = dinfo.Amount;
                                this.Stamina.GainNeed((.005f * (float)dmgAmt) + (.002f * (float)ver.level));
                                if (dmgAmt < mitigationAmt)
                                {
                                    actualDmg = 0;
                                    return;
                                }
                                else
                                {
                                    actualDmg = dmgAmt - mitigationAmt;
                                }
                                fortitudeMitigationDelay = this.age + 10;
                                dinfo.SetAmount(actualDmg);
                                abilityUser.TakeDamage(dinfo);
                                return;
                            }
                            base.PostPreApplyDamage(dinfo, out absorbed);
                        }
                    }
                }
                list.Clear();
                list = null;
            }
            base.PostPreApplyDamage(dinfo, out absorbed);
        }

        public void ResolveClassSkills()
        {
            if (this.IsMightUser && !this.Pawn.Dead && !this.Pawn.Downed)
            {
                if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer))
                {
                    MightPowerSkill bladefocus_pwr = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_BladeFocus.FirstOrDefault((MightPowerSkill x) => x.label == "TM_BladeFocus_pwr");

                    List<Trait> traits = this.Pawn.story.traits.allTraits;
                    for (int i = 0; i < traits.Count; i++)
                    {
                        if (traits[i].def.defName == "Bladedancer")
                        {
                            if (traits[i].Degree != bladefocus_pwr.level)
                            {
                                traits.Remove(traits[i]);
                                this.Pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Bladedancer"), bladefocus_pwr.level, false));
                                MoteMaker.ThrowHeatGlow(this.Pawn.Position, this.Pawn.Map, 2);
                            }
                        }
                    }
                }

                if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator))
                {
                    if (!this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_HediffFortitude))
                    {
                        HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_HediffFortitude, -5f);
                        HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_HediffFortitude, 1f);
                    }
                }

                if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger))
                {
                    MightPowerSkill rangertraining_pwr = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_RangerTraining.FirstOrDefault((MightPowerSkill x) => x.label == "TM_RangerTraining_pwr");

                    List<Trait> traits = this.Pawn.story.traits.allTraits;
                    for (int i = 0; i < traits.Count; i++)
                    {
                        if (traits[i].def.defName == "Ranger")
                        {

                            if (traits[i].Degree != rangertraining_pwr.level)
                            {
                                traits.Remove(traits[i]);
                                this.Pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Ranger"), rangertraining_pwr.level, false));
                                MoteMaker.ThrowHeatGlow(this.Pawn.Position, this.Pawn.Map, 2);
                            }
                        }
                    }
                }

                if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper))
                {
                    MightPowerSkill sniperfocus_pwr = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_SniperFocus.FirstOrDefault((MightPowerSkill x) => x.label == "TM_SniperFocus_pwr");

                    List<Trait> traits = this.Pawn.story.traits.allTraits;
                    for (int i = 0; i < traits.Count; i++)
                    {
                        if (traits[i].def.defName == "TM_Sniper")
                        {
                            if (traits[i].Degree != sniperfocus_pwr.level)
                            {
                                traits.Remove(traits[i]);
                                this.Pawn.story.traits.GainTrait(new Trait(TraitDef.Named("TM_Sniper"), sniperfocus_pwr.level, false));
                                MoteMaker.ThrowHeatGlow(base.Pawn.Position, this.Pawn.Map, 2);
                            }
                        }
                    }
                }

                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) && !this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_BladeArtHD))
                {
                    MightPowerSkill bladeart_pwr = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_BladeArt.FirstOrDefault((MightPowerSkill x) => x.label == "TM_BladeArt_pwr");

                    //HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BladeArtHD, -5f);
                    HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BladeArtHD, (.5f) + bladeart_pwr.level);
                    if (!this.Pawn.IsColonist && settingsRef.AIHardMode)
                    {
                        HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BladeArtHD, 4);
                    }
                }
                else if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) && !this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_BowTrainingHD))
                {
                    MightPowerSkill bowtraining_pwr = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_BowTraining.FirstOrDefault((MightPowerSkill x) => x.label == "TM_BowTraining_pwr");

                    //HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BowTrainingHD, -5f);
                    HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BowTrainingHD, (.5f) + bowtraining_pwr.level);
                    if (!this.Pawn.IsColonist && settingsRef.AIHardMode)
                    {
                        HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BowTrainingHD, 4);
                    }

                }
                else
                {
                    using (IEnumerator<Hediff> enumerator = this.Pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Hediff rec = enumerator.Current;

                            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) && rec.def == TorannMagicDefOf.TM_BladeArtHD && this.Pawn.IsColonist)
                            {
                                MightPowerSkill bladeart_pwr = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_BladeArt.FirstOrDefault((MightPowerSkill x) => x.label == "TM_BladeArt_pwr");
                                if (rec.Severity < (float)(.5f + bladeart_pwr.level) || rec.Severity > (float)(.6f + bladeart_pwr.level))
                                {
                                    HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BladeArtHD, -5f);
                                    HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BladeArtHD, (.5f) + bladeart_pwr.level);
                                    MoteMaker.ThrowDustPuff(this.Pawn.Position.ToVector3Shifted(), this.Pawn.Map, .6f);
                                    MoteMaker.ThrowHeatGlow(this.Pawn.Position, this.Pawn.Map, 1.6f);
                                }
                            }

                            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) && rec.def == TorannMagicDefOf.TM_BowTrainingHD && this.Pawn.IsColonist)
                            {
                                MightPowerSkill bowtraining_pwr = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_BowTraining.FirstOrDefault((MightPowerSkill x) => x.label == "TM_BowTraining_pwr");
                                if (rec.Severity < (float)(.5f + bowtraining_pwr.level) || rec.Severity > (float)(.6f + bowtraining_pwr.level))
                                {
                                    HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BowTrainingHD, -5f);
                                    HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BowTrainingHD, (.5f) + bowtraining_pwr.level);
                                    MoteMaker.ThrowDustPuff(this.Pawn.Position.ToVector3Shifted(), this.Pawn.Map, .6f);
                                    MoteMaker.ThrowHeatGlow(this.Pawn.Position, this.Pawn.Map, 1.6f);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ResolveSustainedSkills()
        {
            float _maxSP = 0;
            float _spRegeRate = 0;
            float coolDown = 0;
            float _spCost = 0;
            float _xpGain = 0;

            using (IEnumerator<Hediff> enumerator = this.Pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Hediff rec = enumerator.Current;
                    if (rec.def.defName == ("TM_HediffSprint"))
                    {
                        MightPowerSkill eff = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Sprint.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Sprint_eff");
                        _maxSP -= .3f * (1 - (.1f * eff.level));
                        //Catch negative values
                        if (this.maxSP < 0)
                        {
                            this.Pawn.health.RemoveHediff(rec);
                            Log.Message("Removed " + rec.def.LabelCap + ", insufficient stamina to maintain.");
                        }
                    }
                    if (rec.def.defName == "TM_HediffGearRepair")
                    {
                        _maxSP -= .2f;
                        //Catch negative values
                        if (this.maxSP < 0)
                        {
                            this.Pawn.health.RemoveHediff(rec);
                            Log.Message("Removed " + rec.def.LabelCap + ", insufficient stamina to maintain.");
                        }
                    }
                    if (rec.def.defName == "TM_HediffInnerHealing")
                    {
                        _maxSP -= .1f;
                        //Catch negative values
                        if (this.maxSP < 0)
                        {
                            this.Pawn.health.RemoveHediff(rec);
                            Log.Message("Removed " + rec.def.LabelCap + ", insufficient stamina to maintain.");
                        }
                    }
                    if (rec.def.defName == "TM_HediffHeavyBlow")
                    {
                        _maxSP -= .3f;
                        //Catch negative values
                        if (this.maxSP < 0)
                        {
                            this.Pawn.health.RemoveHediff(rec);
                            Log.Message("Removed " + rec.def.LabelCap + ", insufficient stamina to maintain.");
                        }
                    }
                    if (rec.def.defName == "TM_HediffStrongBack")
                    {
                        _maxSP -= .1f;
                        //Catch negative values
                        if (this.maxSP < 0)
                        {
                            this.Pawn.health.RemoveHediff(rec);
                            Log.Message("Removed " + rec.def.LabelCap + ", insufficient stamina to maintain.");
                        }
                    }
                    if (rec.def.defName == "TM_HediffThickSkin")
                    {
                        _maxSP -= .3f;
                        //Catch negative values
                        if (this.maxSP < 0)
                        {
                            this.Pawn.health.RemoveHediff(rec);
                            Log.Message("Removed " + rec.def.LabelCap + ", insufficient stamina to maintain.");
                        }
                    }
                    if (rec.def.defName == "TM_HediffFightersFocus")
                    {
                        _maxSP -= .15f;
                        //Catch negative values
                        if (this.maxSP < 0)
                        {
                            this.Pawn.health.RemoveHediff(rec);
                            Log.Message("Removed " + rec.def.LabelCap + ", insufficient stamina to maintain.");
                        }
                    }
                }
            }
            if (this.bondedPet != null)
            {
                _maxSP -= .30f;
                if (this.bondedPet.Dead || this.bondedPet.Destroyed)
                {
                    this.Pawn.needs.mood.thoughts.memories.TryGainMemory(TorannMagicDefOf.RangerPetDied, null);
                    this.bondedPet = null;
                }
                else if (this.bondedPet.Faction != null && this.bondedPet.Faction != this.Pawn.Faction)
                {
                    //sold? punish evil
                    this.Pawn.needs.mood.thoughts.memories.TryGainMemory(TorannMagicDefOf.RangerSoldBondedPet, null);
                    this.bondedPet = null;
                }
            }
            if(this.Pawn.needs.mood.thoughts.memories.NumMemoriesOfDef(ThoughtDef.Named("RangerSoldBondedPet")) > 0)
            {
                if(this.animalBondingDisabled == false)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_AnimalFriend);
                    this.animalBondingDisabled = true;
                }
            }
            else
            {
                if(this.animalBondingDisabled == true)
                {
                    this.AddPawnAbility(TorannMagicDefOf.TM_AnimalFriend);
                    this.animalBondingDisabled = false;
                }
            }
            MightPowerSkill endurance = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_global_endurance.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_endurance_pwr");
            this.maxSP = 1 + (.02f * endurance.level) + _maxSP;
        }

        public void ResolveStamina()
        {
            bool flag = this.Stamina == null;
            if (flag)
            {
                Hediff firstHediffOfDef = base.AbilityUser.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_MightUserHD, false);
                bool flag2 = firstHediffOfDef != null;
                if (flag2)
                {
                    firstHediffOfDef.Severity = 1f;
                }
                else
                {
                    Hediff hediff = HediffMaker.MakeHediff(TorannMagicDefOf.TM_MightUserHD, base.AbilityUser, null);
                    hediff.Severity = 1f;
                    base.AbilityUser.health.AddHediff(hediff, null, null);
                }
            }
        }
        public void ResolveMightPowers()
        {
            bool flag = this.mightPowersInitialized;
            if (!flag)
            {
                this.mightPowersInitialized = true;
            }
        }
        public void ResolveMightTab()
        {
            InspectTabBase inspectTabsx = base.AbilityUser.GetInspectTabs().FirstOrDefault((InspectTabBase x) => x.labelKey == "TM_TabMight");
            IEnumerable<InspectTabBase> inspectTabs = base.AbilityUser.GetInspectTabs();
            bool flag = inspectTabs != null && inspectTabs.Count<InspectTabBase>() > 0;
            if (flag)
            {         
                if (inspectTabsx == null)
                {
                    try
                    {
                        base.AbilityUser.def.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(typeof(ITab_Pawn_Might)));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Concat(new object[]
                        {
                            "Could not instantiate inspector tab of type ",
                            typeof(ITab_Pawn_Might),
                            ": ",
                            ex
                        }));
                    }
                }
            }
        }

        public override void PostExposeData()
        {
            //base.PostExposeData();
            Scribe_Values.Look<bool>(ref this.mightPowersInitialized, "mightPowersInitialized", false, false);
            Scribe_Collections.Look<Thing>(ref this.combatItems, "combatItems", LookMode.Reference);
            Scribe_References.Look<Pawn>(ref this.bondedPet, "bondedPet", false);
            Scribe_Values.Look<bool>(ref this.skill_GearRepair, "skill_GearRepair", false, false);
            Scribe_Values.Look<bool>(ref this.skill_InnerHealing, "skill_InnerHealing", false, false);
            Scribe_Values.Look<bool>(ref this.skill_HeavyBlow, "skill_HeavyBlow", false, false);
            Scribe_Values.Look<bool>(ref this.skill_Sprint, "skill_Sprint", false, false);
            Scribe_Values.Look<bool>(ref this.skill_StrongBack, "skill_StrongBack", false, false);
            Scribe_Values.Look<bool>(ref this.skill_ThickSkin, "skill_ThickSkin", false, false);
            Scribe_Values.Look<bool>(ref this.skill_FightersFocus, "skill_FightersFocus", false, false);
            Scribe_Deep.Look<MightData>(ref this.mightData, "mightData", new object[]
            {
                this
            });
            bool flag11 = Scribe.mode == LoadSaveMode.PostLoadInit;
            if (flag11)
            {
                Pawn abilityUser = base.AbilityUser;
                bool flag40 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Gladiator);
                if (flag40)
                {
                    bool flag14 = !this.MightData.MightPowersG.NullOrEmpty<MightPower>();
                    if (flag14)
                    {
                        foreach (MightPower current3 in this.MightData.MightPowersG)
                        {
                            bool flag15 = current3.abilityDef != null;
                            if (flag15)
                            {
                                if ((current3.abilityDef == TorannMagicDefOf.TM_Sprint || current3.abilityDef == TorannMagicDefOf.TM_Sprint_I || current3.abilityDef == TorannMagicDefOf.TM_Sprint_II || current3.abilityDef == TorannMagicDefOf.TM_Sprint_III))
                                {
                                    if (current3.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Sprint);
                                    }
                                    else if (current3.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Sprint_I);
                                    }
                                    else if (current3.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Sprint_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Sprint_III);
                                    }
                                }
                                if ((current3.abilityDef == TorannMagicDefOf.TM_Grapple || current3.abilityDef == TorannMagicDefOf.TM_Grapple_I || current3.abilityDef == TorannMagicDefOf.TM_Grapple_II || current3.abilityDef == TorannMagicDefOf.TM_Grapple_III))
                                {
                                    if (current3.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Grapple);
                                    }
                                    else if (current3.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Grapple_I);
                                    }
                                    else if (current3.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Grapple_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Grapple_III);
                                    }
                                }
                            }
                        }
                    }
                }
                if (flag40)
                {
                   // Log.Message("Loading Gladiator Abilities");
                    //this.AddPawnAbility(TorannMagicDefOf.TM_Fortitude);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Cleave);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Whirlwind);
                }
                bool flag41 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper);
                if (flag41)
                {
                    bool flag17 = !this.MightData.MightPowersS.NullOrEmpty<MightPower>();
                    if (flag17)
                    {
                        foreach (MightPower current4 in this.MightData.MightPowersS)
                        {
                            bool flag18 = current4.abilityDef != null;
                            if (flag18)
                            {
                                if ((current4.abilityDef == TorannMagicDefOf.TM_DisablingShot || current4.abilityDef == TorannMagicDefOf.TM_DisablingShot_I || current4.abilityDef == TorannMagicDefOf.TM_DisablingShot_II || current4.abilityDef == TorannMagicDefOf.TM_DisablingShot_III))
                                {
                                    if (current4.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_DisablingShot);
                                    }
                                    else if (current4.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_DisablingShot_I);
                                    }
                                    else if (current4.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_DisablingShot_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_DisablingShot_III);
                                    }
                                }
                            }
                        }
                    }
                }
                if (flag41)
                {
                    //Log.Message("Loading Sniper Abilities");
                    //this.AddPawnAbility(TorannMagicDefOf.TM_SniperFocus);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Headshot);
                    this.AddPawnAbility(TorannMagicDefOf.TM_AntiArmor);
                }
                bool flag42 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Bladedancer);
                if (flag42)
                {
                    bool flag19 = !this.MightData.MightPowersB.NullOrEmpty<MightPower>();
                    if (flag19)
                    {
                        foreach (MightPower current5 in this.MightData.MightPowersB)
                        {
                            bool flag20 = current5.abilityDef != null;
                            if (flag20)
                            {
                                if ((current5.abilityDef == TorannMagicDefOf.TM_PhaseStrike || current5.abilityDef == TorannMagicDefOf.TM_PhaseStrike_I || current5.abilityDef == TorannMagicDefOf.TM_PhaseStrike_II || current5.abilityDef == TorannMagicDefOf.TM_PhaseStrike_III))
                                {
                                    if (current5.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_PhaseStrike);
                                    }
                                    else if (current5.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_PhaseStrike_I);
                                    }
                                    else if (current5.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_PhaseStrike_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_PhaseStrike_III);
                                    }
                                }
                            }
                        }
                    }
                }
                if (flag42)
                {
                    //Log.Message("Loading Bladedancer Abilities");
                    //this.AddPawnAbility(TorannMagicDefOf.TM_BladeFocus);
                    //this.AddPawnAbility(TorannMagicDefOf.TM_BladeArt);
                    this.AddPawnAbility(TorannMagicDefOf.TM_SeismicSlash);
                    this.AddPawnAbility(TorannMagicDefOf.TM_BladeSpin);
                }
                bool flag43 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Ranger);
                if (flag43)
                {
                    bool flag21 = !this.MightData.MightPowersR.NullOrEmpty<MightPower>();
                    if (flag21)
                    {
                        foreach (MightPower current6 in this.MightData.MightPowersR)
                        {
                            bool flag22 = current6.abilityDef != null;
                            if (flag22)
                            {
                                if ((current6.abilityDef == TorannMagicDefOf.TM_ArrowStorm || current6.abilityDef == TorannMagicDefOf.TM_ArrowStorm_I || current6.abilityDef == TorannMagicDefOf.TM_ArrowStorm_II || current6.abilityDef == TorannMagicDefOf.TM_ArrowStorm_III))
                                {
                                    if (current6.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ArrowStorm);
                                    }
                                    else if (current6.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ArrowStorm_I);
                                    }
                                    else if (current6.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ArrowStorm_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ArrowStorm_III);
                                    }
                                }
                            }
                        }
                    }
                }
                if (flag43)
                {
                    //Log.Message("Loading Ranger Abilities");
                    //this.AddPawnAbility(TorannMagicDefOf.TM_RangerTraining);
                    //this.AddPawnAbility(TorannMagicDefOf.TM_BowTraining);
                    this.AddPawnAbility(TorannMagicDefOf.TM_PoisonTrap);
                    this.AddPawnAbility(TorannMagicDefOf.TM_AnimalFriend);
                }

                this.InitializeSkill();
                //base.UpdateAbilities();
            }

        }

    }
}
