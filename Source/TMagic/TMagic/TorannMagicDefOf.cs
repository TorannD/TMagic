using System;
using RimWorld;
using Verse;
using AbilityUser;


namespace TorannMagic
{
	[DefOf]
	public static class TorannMagicDefOf
	{
        //Magic
        public static NeedDef TM_Mana;
        
        public static HediffDef TM_MagicUserHD;
        
        public static ThingDef BookOfInnerFire;
        public static ThingDef Torn_BookOfInnerFire;
        public static ThingDef BookOfHeartOfFrost;
        public static ThingDef Torn_BookOfHeartOfFrost;
        public static ThingDef BookOfStormBorn;
        public static ThingDef Torn_BookOfStormBorn;
        public static ThingDef BookOfArcanist;
        public static ThingDef Torn_BookOfArcanist;
        public static ThingDef BookOfValiant;
        public static ThingDef Torn_BookOfValiant;
        public static ThingDef BookOfSummoner;
        public static ThingDef Torn_BookOfSummoner;
        public static ThingDef BookOfDruid;
        public static ThingDef Torn_BookOfNature;
        public static ThingDef BookOfNecromancer;
        public static ThingDef Torn_BookOfUndead;
        public static ThingDef BookOfPriest;
        public static ThingDef Torn_BookOfPriest;
        public static ThingDef BookOfQuestion;

        public static HediffDef TM_Uncertainty;        

        public static ThingDef SpellOf_Rain;
        public static ThingDef SpellOf_Blink;
        public static ThingDef SpellOf_Teleport;
        public static ThingDef SpellOf_Heal;
        public static ThingDef SpellOf_Heater;
        public static ThingDef SpellOf_Cooler;
        public static ThingDef SpellOf_PowerNode;
        public static ThingDef SpellOf_DryGround;
        public static ThingDef SpellOf_WetGround;
        public static ThingDef SpellOf_ChargeBattery;
        public static ThingDef SpellOf_SmokeCloud;
        public static ThingDef SpellOf_Extinguish;
        public static ThingDef SpellOf_EMP;
        public static ThingDef SpellOf_Firestorm;
        public static ThingDef SpellOf_Blizzard;
        public static ThingDef SpellOf_SummonMinion;
        public static ThingDef SpellOf_TransferMana;
        public static ThingDef SpellOf_SiphonMana;
        public static ThingDef SpellOf_RegrowLimb;
        public static ThingDef SpellOf_EyeOfTheStorm;
        public static ThingDef SpellOf_ManaShield;
        public static ThingDef SpellOf_FoldReality;
        public static ThingDef SpellOf_Resurrection;

        public static ThingDef SkillOf_Sprint;
        public static ThingDef SkillOf_GearRepair;
        public static ThingDef SkillOf_InnerHealing;
        public static ThingDef SkillOf_StrongBack;
        public static ThingDef SkillOf_ThickSkin;
        public static ThingDef SkillOf_FightersFocus;
        public static ThingDef SkillOf_HeavyBlow;

        public static ThingDef ManaPotion;

        public static GameConditionDef ManaDrain;
        public static GameConditionDef ManaSurge;
        public static HediffDef TM_ManaSickness;
        public static HediffDef TM_ArcaneSickness;
        public static HediffDef TM_ArcaneWeakness;

        //Site Defs
        public static WorldObjectDef ArcaneAdventure;
        public static SiteCoreDef ArcaneStash;
        public static SitePartDef ArcaneStashTreasure;
        public static SitePartDef ArcaneDefenders;
        public static SitePartDef EnemyRaidOnArrival;
        public static SitePartDef ArcaneBanditSquad;
        public static IncidentDef ArcaneEnemyRaid;
        
        public static TraitDef Gifted;
        
		//Fire
		public static TraitDef InnerFire;

        public static TMAbilityDef TM_RayofHope;
        public static TMAbilityDef TM_RayofHope_I;
        public static TMAbilityDef TM_RayofHope_II;
        public static TMAbilityDef TM_RayofHope_III;
        public static TMAbilityDef TM_Firebolt;
        public static TMAbilityDef TM_Fireball;
		public static TMAbilityDef TM_Fireclaw;
		public static TMAbilityDef TM_Firestorm;

        public static ThingDef TM_Firestorm_Small;
        public static ThingDef TM_Firestorm_Tiny;
        public static ThingDef TM_Firestorm_Large;

		//Ice
		public static TraitDef HeartOfFrost;

        public static TMAbilityDef TM_Soothe;
        public static TMAbilityDef TM_Soothe_I;
        public static TMAbilityDef TM_Soothe_II;
        public static TMAbilityDef TM_Soothe_III;
        public static TMAbilityDef TM_Icebolt;
		public static TMAbilityDef TM_Snowball;
        public static TMAbilityDef TM_FrostRay;
        public static TMAbilityDef TM_FrostRay_I;
        public static TMAbilityDef TM_FrostRay_II;
        public static TMAbilityDef TM_FrostRay_III;
        public static TMAbilityDef TM_Rainmaker;
        public static TMAbilityDef TM_Blizzard;

        public static ThingDef TM_Blizzard_Small;
        public static ThingDef TM_Blizzard_Tiny;
        public static ThingDef TM_Blizzard_Large;

        //Lightning
        public static TraitDef StormBorn;

        public static TMAbilityDef TM_AMP;
        public static TMAbilityDef TM_AMP_I;
        public static TMAbilityDef TM_AMP_II;
        public static TMAbilityDef TM_AMP_III;
        public static TMAbilityDef TM_LightningBolt;
        public static TMAbilityDef TM_LightningCloud;
		public static TMAbilityDef TM_LightningStorm;
        public static TMAbilityDef TM_EyeOfTheStorm;
        public static ThingDef FlyingObject_EyeOfTheStorm;
        

		//Arcane
		public static TraitDef Arcanist;

        public static TMAbilityDef TM_Shadow;
        public static TMAbilityDef TM_Shadow_I;
        public static TMAbilityDef TM_Shadow_II;
        public static TMAbilityDef TM_Shadow_III;
        public static TMAbilityDef TM_MagicMissile;
        public static TMAbilityDef TM_MagicMissile_I;
        public static TMAbilityDef TM_MagicMissile_II;
        public static TMAbilityDef TM_MagicMissile_III;
        public static TMAbilityDef TM_Blink;
        public static TMAbilityDef TM_Blink_I;
        public static TMAbilityDef TM_Blink_II;
        public static TMAbilityDef TM_Blink_III;
        public static TMAbilityDef TM_Summon;
        public static TMAbilityDef TM_Summon_I;
        public static TMAbilityDef TM_Summon_II;
        public static TMAbilityDef TM_Summon_III;
        public static TMAbilityDef TM_Teleport;
        public static TMAbilityDef TM_FoldReality;

        public static JobDef PortalDestination;
        public static JobDef UsePortal;
        public static JobDef DeactivatePortal;
        public static JobDef ChargePortal;
        public static JobDef PortalStockpile;

        //Holy (Paladin)
        public static TraitDef Paladin;

        public static TMAbilityDef TM_Heal;
        public static TMAbilityDef TM_Shield;
        public static TMAbilityDef TM_Shield_I;
        public static TMAbilityDef TM_Shield_II;
        public static TMAbilityDef TM_Shield_III;
        public static HediffDef TM_HediffShield;
        public static TMAbilityDef TM_ValiantCharge;
        public static HediffDef TM_HediffInvulnerable;
        public static TMAbilityDef TM_Overwhelm;
        public static HediffDef TM_Blind;

        //Summoner
        public static TraitDef Summoner;
        public static ThingDef TM_Earth_ElementalR;
        public static ThingDef TM_LesserEarth_ElementalR;
        public static ThingDef TM_GreaterEarth_ElementalR;
        public static ThingDef TM_Fire_ElementalR;
        public static ThingDef TM_LesserFire_ElementalR;
        public static ThingDef TM_GreaterFire_ElementalR;
        public static ThingDef TM_Water_ElementalR;
        public static ThingDef TM_LesserWater_ElementalR;
        public static ThingDef TM_GreaterWater_ElementalR;
        public static ThingDef TM_Wind_ElementalR;
        public static ThingDef TM_LesserWind_ElementalR;
        public static ThingDef TM_GreaterWind_ElementalR;
        public static ThingDef TM_MinionR;
        public static ThingDef TM_GreaterMinionR;
        public static FactionDef TM_ElementalFaction;
        public static FactionDef TM_SummonedFaction;

        public static TMAbilityDef TM_SummonMinion;
        public static TMAbilityDef TM_DismissMinion;
        public static TMAbilityDef TM_SummonPylon;
        public static TMAbilityDef TM_SummonExplosive;
        public static TMAbilityDef TM_SummonElemental;

        //Druid
        public static TraitDef Druid;

        public static TMAbilityDef TM_Poison;
        public static HediffDef TM_Poison_HD;
        public static HediffDef TM_Poisoned_HD;
        public static TMAbilityDef TM_SootheAnimal;
        public static TMAbilityDef TM_SootheAnimal_I;
        public static TMAbilityDef TM_SootheAnimal_II;
        public static TMAbilityDef TM_SootheAnimal_III;
        public static TMAbilityDef TM_Regenerate;
        public static HediffDef TM_Regeneration;
        public static HediffDef TM_Regeneration_I;
        public static HediffDef TM_Regeneration_II;
        public static HediffDef TM_Regeneration_III;
        public static TMAbilityDef TM_CureDisease;
        public static TMAbilityDef TM_RegrowLimb;  // ultimate 
        public static HediffDef TM_ArmRegrowth;
        public static HediffDef TM_HandRegrowth;
        public static HediffDef TM_FootRegrowth;
        public static HediffDef TM_LegRegrowth;

        //Necromancer
        public static TraitDef Necromancer;
        public static TraitDef Undead;

        public static TMAbilityDef TM_RaiseUndead;
        public static HediffDef TM_UndeadHD;
        public static HediffDef TM_UndeadAnimalHD;
        public static TrainableDef Haul;
        public static TMAbilityDef TM_DeathMark;
        public static TMAbilityDef TM_DeathMark_I;
        public static TMAbilityDef TM_DeathMark_II;
        public static TMAbilityDef TM_DeathMark_III;
        public static HediffDef TM_DeathMarkHD;
        public static TMAbilityDef TM_FogOfTorment;
        public static HediffDef TM_TormentHD;
        public static TMAbilityDef TM_ConsumeCorpse;
        public static TMAbilityDef TM_ConsumeCorpse_I;
        public static TMAbilityDef TM_ConsumeCorpse_II;
        public static TMAbilityDef TM_ConsumeCorpse_III;
        public static TMAbilityDef TM_CorpseExplosion;
        public static TMAbilityDef TM_CorpseExplosion_I;
        public static TMAbilityDef TM_CorpseExplosion_II;
        public static TMAbilityDef TM_CorpseExplosion_III;
        public static TMAbilityDef TM_DismissUndead;

        public static WorkTypeDef Art;
        public static WorkTypeDef Research;

        //Priest
        public static TraitDef Priest;

        public static TMAbilityDef TM_AdvancedHeal;
        public static TMAbilityDef TM_Purify;
        public static TMAbilityDef TM_HealingCircle;
        public static TMAbilityDef TM_HealingCircle_I;
        public static TMAbilityDef TM_HealingCircle_II;
        public static TMAbilityDef TM_HealingCircle_III;
        public static TMAbilityDef TM_BestowMight;
        public static HediffDef BestowMightHD;
        public static HediffDef BestowMightHD_I;
        public static HediffDef BestowMightHD_II;
        public static HediffDef BestowMightHD_III;
        public static TMAbilityDef TM_BestowMight_I;
        public static TMAbilityDef TM_BestowMight_II;
        public static TMAbilityDef TM_BestowMight_III;
        public static TMAbilityDef TM_Resurrection;

        //Might 
        public static NeedDef TM_Stamina;
        public static HediffDef TM_MightUserHD;

        public static ThingDef BookOfGladiator;
        public static ThingDef BookOfSniper;
        public static ThingDef BookOfBladedancer;
        public static ThingDef BookOfRanger;
        public static TraitDef PhysicalProdigy;

        //Might (Gladiator)
        public static TraitDef Gladiator;

        public static TMAbilityDef TM_Sprint;
        public static TMAbilityDef TM_Sprint_I;
        public static TMAbilityDef TM_Sprint_II;
        public static TMAbilityDef TM_Sprint_III;
        public static TMAbilityDef TM_Fortitude;
        public static HediffDef TM_HediffFortitude;
        public static TMAbilityDef TM_Grapple;
        public static TMAbilityDef TM_Grapple_I;
        public static TMAbilityDef TM_Grapple_II;
        public static TMAbilityDef TM_Grapple_III;
        public static HediffDef TM_GrapplingHook;
        public static TMAbilityDef TM_Cleave;
        public static TMAbilityDef TM_Whirlwind;
        public static HediffDef TM_Whirlwind_Knockdown;

        //Precision (Sniper)
        public static TraitDef TM_Sniper;

        public static TMAbilityDef TM_SniperFocus;
        public static TMAbilityDef TM_Headshot;
        public static TMAbilityDef TM_DisablingShot;
        public static TMAbilityDef TM_DisablingShot_I;
        public static TMAbilityDef TM_DisablingShot_II;
        public static TMAbilityDef TM_DisablingShot_III;
        public static HediffDef TM_DisablingShot_HD;
        public static TMAbilityDef TM_AntiArmor;

        //Bladedancer
        public static TraitDef Bladedancer;

        public static TMAbilityDef TM_BladeFocus;
        public static TMAbilityDef TM_BladeArt;
        public static HediffDef TM_BladeArtHD;
        public static TMAbilityDef TM_SeismicSlash;
        public static TMAbilityDef TM_BladeSpin;
        public static TMAbilityDef TM_PhaseStrike;
        public static TMAbilityDef TM_PhaseStrike_I;
        public static TMAbilityDef TM_PhaseStrike_II;
        public static TMAbilityDef TM_PhaseStrike_III;
        public static HediffDef TM_HamstringHD;

        //Ranger
        public static TraitDef Ranger;

        public static TMAbilityDef TM_RangerTraining;
        public static TMAbilityDef TM_BowTraining;
        public static HediffDef TM_BowTrainingHD;
        public static TMAbilityDef TM_PoisonTrap;
        public static TMAbilityDef TM_AnimalFriend;
        public static HediffDef TM_RangerBondHD;
        public static TMAbilityDef TM_ArrowStorm;
        public static TMAbilityDef TM_ArrowStorm_I;
        public static TMAbilityDef TM_ArrowStorm_II;
        public static TMAbilityDef TM_ArrowStorm_III;

        public static JobDef PlacePoisonTrap;
        public static TrainableDef Rescue;

        //Standalone
        public static TMAbilityDef TM_Heater;
        public static TMAbilityDef TM_Cooler;
        public static TMAbilityDef TM_PowerNode;
        public static TMAbilityDef TM_DryGround;
        public static TMAbilityDef TM_WetGround;
        public static TMAbilityDef TM_ChargeBattery;
        public static TMAbilityDef TM_SmokeCloud;
        public static TMAbilityDef TM_Extinguish;
        public static TMAbilityDef TM_EMP;
        public static TMAbilityDef TM_TransferMana;
        public static TMAbilityDef TM_SiphonMana;
        public static TMAbilityDef TM_ManaShield;
        public static HediffDef TM_ManaShieldHD;

        public static HediffDef TM_Sight;
        public static HediffDef TM_Breathing;
        public static HediffDef TM_Manipulation;
        public static HediffDef TM_Movement;

        public static HediffDef TM_AntiSight;
        public static HediffDef TM_AntiBreathing;
        public static HediffDef TM_AntiManipulation;
        public static HediffDef TM_AntiMovement;

        public static TMAbilityDef TM_GearRepair;
        public static TMAbilityDef TM_InnerHealing;
        public static TMAbilityDef TM_HeavyBlow;
        public static TMAbilityDef TM_StrongBack;
        public static TMAbilityDef TM_ThickSkin;
        public static TMAbilityDef TM_FightersFocus;

        //Graphics

        public static ThingDef Mote_ManaPuff;
        public static ThingDef Mote_Enchanting;
        public static ThingDef Mote_Siphon;
        public static ThingDef Mote_Poison;
        public static ThingDef Mote_Regen;
        public static ThingDef Mote_CrossStrike;
        public static ThingDef Mote_BloodSquirt;
        public static ThingDef Mote_MultiStrike;
        public static ThingDef Mote_ScreamMote;
        public static ThingDef Fog_Torment;
        public static ThingDef Mote_PowerBeamBlue;
        public static ThingDef Mote_PowerBeamGold;
        public static ThingDef Mote_Bombardment;
        public static ThingDef Fog_Poison;

        //Jobs
        public static JobDef TMCastAbilityVerb;
        public static JobDef TMCastAbilitySelf;
        public static JobDef JobDriver_RemoveEnchantingGem;
        public static JobDef JobDriver_AddEnchantingGem;
        public static JobDef JobDriver_EnchantItem;

        //Magicyte
        public static ThingDef RawMagicyte;

        //Sounds
        public static SoundDef ItemEnchanted;

    }
}
