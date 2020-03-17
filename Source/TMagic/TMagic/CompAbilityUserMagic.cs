using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using AbilityUser;
using Verse;
using Verse.AI;
using Verse.Sound;
using AbilityUserAI;

namespace TorannMagic
{
    [CompilerGenerated]
    [Serializable]
    [StaticConstructorOnStartup]
    public class CompAbilityUserMagic : CompAbilityUser
    {
        public string LabelKey = "TM_Magic";

        public bool firstTick = false;
        public bool magicPowersInitialized = false;
        public bool magicPowersInitializedForColonist = true;
        private bool colonistPowerCheck = true;
        private int resMitigationDelay = 0;
        private int damageMitigationDelay = 0;
        public int magicXPRate = 1000;
        public int lastXPGain = 0;
        private int age = -1;
        private bool doOnce = true;
        private int autocastTick = 0;
        private int nextAICastAttemptTick = 0;

        private float IF_RayofHope_eff = 0.08f;
        private float IF_Firebolt_eff = 0.10f;
        private float IF_Fireclaw_eff = 0.10f;
        private float IF_Fireball_eff = 0.08f;
        private float IF_Firestorm_eff = 0.05f;
        private float HoF_Soothe_eff = 0.08f;
        private float HoF_Icebolt_eff = 0.08f;
        private float HoF_FrostRay_eff = 0.08f;
        private float HoF_Snowball_eff = 0.08f;
        private float HoF_Blizzard_eff = 0.05f;
        private float HoF_Rainmaker_eff = 0.15f;
        private float SB_AMP_eff = 0.08f;
        private float SB_LightningBolt_eff = 0.08f;
        private float SB_LightningCloud_eff = 0.06f;
        private float SB_LightningStorm_eff = 0.06f;
        private float SB_EyeOfTheStorm_eff = 0.05f;
        private float A_Shadow_eff = 0.08f;
        private float A_MagicMissile_eff = 0.08f;
        private float A_Blink_eff = 0.10f;
        private float A_Summon_eff = 0.10f;
        private float A_Teleport_eff = 0.10f;
        private float A_FoldReality_eff = 0.06f;
        private float P_Heal_eff = 0.07f;
        private float P_Shield_eff = 0.08f;
        private float P_ValiantCharge_eff = 0.08f;
        private float P_Overwhelm_eff = 0.10f;
        private float P_HolyWrath_eff = 0.05f;
        private float S_SummonElemental_eff = 0.06f;
        private float S_SummonExplosive_eff = 0.08f;
        private float S_SummonMinion_eff = 0.10f;
        private float S_SummonPylon_eff = 0.08f;
        private float S_SummonPoppi_eff = 0.05f;
        private float D_Poison_eff = 0.10f;
        private float D_SootheAnimal_eff = 0.08f;
        private float D_Regenerate_eff = 0.07f;
        private float D_CureDisease_eff = 0.10f;
        private float D_RegrowLimb_eff = 0.06f;
        private float N_RaiseUndead_eff = 0.05f;
        private float N_DeathMark_eff = 0.08f;
        private float N_FogOfTorment_eff = 0.08f;
        private float N_ConsumeCorpse_eff = 0.0f;
        private float N_CorpseExplosion_eff = 0.08f;
        private float N_DeathBolt_eff = 0.06f;
        private float PR_AdvancedHeal_eff = 0.08f;
        private float PR_Purify_eff = 0.07f;
        private float PR_HealingCircle_eff = 0.07f;
        private float PR_BestowMight_eff = 0.08f;
        private float PR_Resurrection_eff = 0.05f;
        private float B_Lullaby_eff = 0.08f;
        private float B_BattleHymn_eff = 0.06f;
        private float SoulBond_eff = 0.10f;
        private float ShadowBolt_eff = .08f;
        private float Dominate_eff = 0.06f;
        private float WD_Repulsion_eff = .08f;
        private float WD_PsychicShock_eff = .06f;
        private float SD_Attraction_eff = .08f;
        private float SD_Scorn_eff = .06f;
        private float G_Encase_eff = .08f;
        private float G_EarthSprites_eff = 0.06f;
        private float G_EarthernHammer_eff = 0.06f;
        private float G_Meteor_eff = 0.05f;
        private float T_TechnoTurret_eff = 0.02f;
        private float T_TechnoShield_eff = 0.06f;
        private float T_Overdrive_eff = 0.08f;
        private float T_Sabotage_eff = 0.06f;
        private float T_OrbitalStrike_eff = 0.05f;
        private float BM_BloodGift_eff = 0.05f;
        private float BM_IgniteBlood_eff = .06f;
        private float BM_BloodForBlood_eff = .06f;
        private float BM_BloodShield_eff = .06f;
        private float BM_Rend_eff = .08f;
        private float BM_BloodMoon_eff = .05f;
        private float E_EnchantedBody_eff = .15f;
        private float E_Transmutate_eff = .12f;
        private float E_EnchantWeapon_eff = .1f;
        private float E_EnchanterStone_eff = .10f;
        private float E_Polymorph_eff = .06f;
        private float E_Shapeshift_eff = .05f;
        private float C_Prediction_eff = .15f;
        private float C_AlterFate_eff = .1f;
        private float C_AccelerateTime_eff = .08f;
        private float C_ReverseTime_eff = .08f;
        private float C_ChronostaticField_eff = .06f;
        private float C_Recall_eff = .1f;

        private float W_eff = .01f;
        private float global_eff = 0.025f;

        public bool spell_Rain = false;
        public bool spell_Blink = false;
        public bool spell_Teleport = false;
        public bool spell_Heal = false;
        public bool spell_Heater = false;
        public bool spell_Cooler = false;
        public bool spell_DryGround = false;
        public bool spell_WetGround = false;
        public bool spell_ChargeBattery = false;
        public bool spell_SmokeCloud = false;
        public bool spell_Extinguish = false;
        public bool spell_EMP = false;
        public bool spell_Firestorm = false;
        public bool spell_Blizzard = false;
        public bool spell_SummonMinion = false;
        public bool spell_TransferMana = false;
        public bool spell_SiphonMana = false;
        public bool spell_RegrowLimb = false;
        public bool spell_EyeOfTheStorm = false;
        public bool spell_ManaShield = false;
        public bool spell_FoldReality = false;
        public bool spell_Resurrection = false;
        public bool spell_PowerNode = false;
        public bool spell_Sunlight = false;
        public bool spell_HolyWrath = false;
        public bool spell_LichForm = false;
        public bool spell_Flight = false;
        public bool spell_SummonPoppi = false;
        public bool spell_BattleHymn = false;
        public bool spell_CauterizeWound = false;
        public bool spell_FertileLands = false;
        public bool spell_SpellMending = false;
        public bool spell_ShadowStep = false;
        public bool spell_ShadowCall = false;
        public bool spell_Scorn = false;
        public bool spell_PsychicShock = false;
        public bool spell_SummonDemon = false;
        public bool spell_Meteor = false;
        public bool spell_Teach = false;
        public bool spell_OrbitalStrike = false;
        public bool spell_BloodMoon = false;
        public bool spell_EnchantedAura = false;        
        public bool spell_Shapeshift = false;
        public bool spell_ShapeshiftDW = false;
        public bool spell_Blur = false;
        public bool spell_BlankMind = false;
        public bool spell_DirtDevil = false;
        public bool spell_MechaniteReprogramming = false;
        public bool spell_ArcaneBolt = false;
        public bool spell_LightningTrap = false;
        public bool spell_Invisibility = false;
        public bool spell_BriarPatch = false;
        public bool spell_Recall = false;
        public bool spell_MageLight = false;

        private bool item_StaffOfDefender = false;

        public float maxMP = 1;
        public float mpRegenRate = 1;
        public float coolDown = 1;
        public float mpCost = 1;
        public float xpGain = 1;
        public float arcaneDmg = 1;
        public float arcaneRes = 1;
        public float arcalleumCooldown = 0f;

        public List<TM_ChaosPowers> chaosPowers = new List<TM_ChaosPowers>();
        public TMAbilityDef mimicAbility = null;
        public List<Thing> summonedMinions = new List<Thing>();
        public List<Thing> summonedSentinels = new List<Thing>();
        public List<Pawn> stoneskinPawns = new List<Pawn>();
        public IntVec3 earthSprites = default(IntVec3);
        public bool earthSpritesInArea = false;
        public Map earthSpriteMap = null;
        public int nextEarthSpriteAction = 0;
        public int nextEarthSpriteMote = 0;
        public int earthSpriteType = 0;
        private bool dismissEarthSpriteSpell = false;
        public List<Thing> summonedLights = new List<Thing>();
        public List<Thing> summonedHeaters = new List<Thing>();
        public List<Thing> summonedCoolers = new List<Thing>();
        public List<Thing> summonedPowerNodes = new List<Thing>();
        public Pawn soulBondPawn = null;
        private bool dismissMinionSpell = false;
        private bool dismissUndeadSpell = false;
        private bool dismissSunlightSpell = false;
        private bool dispelStoneskin = false;
        private bool dismissCoolerSpell = false;
        private bool dismissHeaterSpell = false;
        private bool dismissPowerNodeSpell = false;
        private bool dispelEnchantWeapon = false;
        private bool dismissEnchanterStones = false;
        private bool dismissLightningTrap = false;
        private bool shatterSentinel = false;
        public List<IntVec3> fertileLands = new List<IntVec3>();
        public Thing mageLightThing = null;
        public bool mageLightActive = false;
        public bool mageLightSet = false;
        public bool useTechnoBitToggle = true;
        public bool useTechnoBitRepairToggle = true;
        public Vector3 bitPosition = Vector3.zero;        
        private bool bitFloatingDown = true;
        private float bitOffset = .45f;
        public int technoWeaponDefNum = -1;
        public Thing technoWeaponThing = null;
        public ThingDef technoWeaponThingDef = null;
        public bool useElementalShotToggle = true;
        public Building overdriveBuilding = null;
        public int overdriveDuration = 0;
        public float overdrivePowerOutput = 0;
        public int overdriveFrequency = 100;
        public Building sabotageBuilding = null;
        public bool ArcaneForging = false;
        public List<Pawn> weaponEnchants = new List<Pawn>();
        public Thing enchanterStone = null;
        public List<Thing> enchanterStones = new List<Thing>();
        public List<Thing> lightningTraps = new List<Thing>();
        public IncidentDef predictionIncidentDef = null;
        public int predictionTick = 0;
        //Recall fields
        //position, hediffs, needs, mana, manual recall bool, recall duration
        public IntVec3 recallPosition = default(IntVec3);
        public Map recallMap = null;
        public List<string> recallNeedDefnames = null;
        public List<float> recallNeedValues = null;
        public List<Hediff> recallHediffList = null;
        public List<Hediff_Injury> recallInjuriesList = null;
        public bool recallSet = false;
        public int recallExpiration = 0;
        public bool recallSpell = false;        

        private Effecter powerEffecter = null;
        private int powerModifier = 0;
        private int maxPower = 10;
        public int nextEntertainTick = -1;
        public int nextSuccubusLovinTick = -1;

        public bool HasTechnoBit
        {
            get
            {
                return this.IsMagicUser && this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TechnoBit).learned;
            }
        }
        public bool HasTechnoTurret
        {
            get
            {
                return this.IsMagicUser && this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TechnoTurret).learned;
            }
        }

        public bool HasTechnoWeapon
        {
            get
            {
                return this.IsMagicUser && this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TechnoWeapon).learned;
            }
        }

        public int PowerModifier
        {
            get
            {
                return powerModifier;
            }
            set
            {
                TM_MoteMaker.ThrowSiphonMote(this.Pawn.DrawPos, this.Pawn.Map, 1f);
                powerModifier = Mathf.Clamp(value, 0, maxPower);
            }
        }

        private MagicData magicData = null;
        public MagicData MagicData
        {
            get
            {
                bool flag = this.magicData == null && this.IsMagicUser;
                if (flag)
                {
                    this.magicData = new MagicData(this);
                }
                return this.magicData;
            }
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
            bool flag = this.powerEffecter != null;
            if (flag)
            {
                this.powerEffecter.Cleanup();
            }
        }

        public override void PostDraw()
        {
            if (IsMagicUser)
            {
                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                if (settingsRef.AIFriendlyMarking && base.AbilityUser.IsColonist && this.IsMagicUser)
                {
                    if (!this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                    {
                        DrawMageMark();
                    }
                }
                if (settingsRef.AIMarking && !base.AbilityUser.IsColonist && this.IsMagicUser)
                {
                    if (!this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                    {
                        DrawMageMark();
                    }
                }

                if ((this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer) || this.Pawn.story.traits.HasTrait(TorannMagicDefOf.ChaosMage)) && this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower mp) => mp.abilityDef == TorannMagicDefOf.TM_TechnoBit).learned == true)
                {
                    DrawTechnoBit();
                }

                if(this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DemonScornHD) || this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DemonScornHD_I) || this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DemonScornHD_II) || this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DemonScornHD_III))
                {
                    DrawScornWings();
                }

                if(this.mageLightActive)
                {
                    DrawMageLight();
                }

                Enchantment.CompEnchant compEnchant = this.Pawn.GetComp<Enchantment.CompEnchant>();
                try
                {
                    if (this.IsMagicUser && compEnchant != null && compEnchant.enchantingContainer.Count > 0)
                    {
                        DrawEnchantMark();
                    }
                }
                catch
                {
                    Enchantment.CompProperties_Enchant newEnchantComp = new Enchantment.CompProperties_Enchant();
                    this.Pawn.def.comps.Add(newEnchantComp);
                }
            }
            base.PostDraw();
        }

        public static List<TMAbilityDef> MagicAbilities = null;  
        
        public int LevelUpSkill_global_regen(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_global_regen.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }        
        public int LevelUpSkill_global_eff(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_global_eff.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_global_spirit(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_global_spirit.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_RayofHope(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_RayofHope.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }        
        public int LevelUpSkill_Firebolt(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Firebolt.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }        
        public int LevelUpSkill_Fireball(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Fireball.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }        
        public int LevelUpSkill_Fireclaw(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Fireclaw.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }        
        public int LevelUpSkill_Firestorm(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Firestorm.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
                
        public int LevelUpSkill_Soothe(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Soothe.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Icebolt(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Icebolt.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_FrostRay(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_FrostRay.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Snowball(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Snowball.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Rainmaker(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Rainmaker.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Blizzard(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Blizzard.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_AMP(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_AMP.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_LightningBolt(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_LightningBolt.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_LightningCloud(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_LightningCloud.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_LightningStorm(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_LightningStorm.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_EyeOfTheStorm(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EyeOfTheStorm.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_Shadow(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Shadow.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_MagicMissile(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_MagicMissile.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Blink(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Blink.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Summon(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Summon.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Teleport(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Teleport.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_FoldReality(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_FoldReality.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_Heal(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Heal.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Shield(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Shield.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_ValiantCharge(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ValiantCharge.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Overwhelm(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Overwhelm.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_HolyWrath(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_HolyWrath.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_SummonMinion(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SummonMinion.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_SummonPylon(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SummonPylon.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_SummonExplosive(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SummonExplosive.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_SummonElemental(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SummonElemental.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_SummonPoppi(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SummonPoppi.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_Poison(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Poison.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_SootheAnimal(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SootheAnimal.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Regenerate(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Regenerate.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_CureDisease(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_CureDisease.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_RegrowLimb(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_RegrowLimb.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_RaiseUndead(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_RaiseUndead.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_DeathMark(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_DeathMark.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_FogOfTorment(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_FogOfTorment.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_ConsumeCorpse(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ConsumeCorpse.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_CorpseExplosion(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_CorpseExplosion.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_DeathBolt(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_DeathBolt.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_AdvancedHeal(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_AdvancedHeal.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Purify(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Purify.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_HealingCircle(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_HealingCircle.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_BestowMight(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BestowMight.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Resurrection(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Resurrection.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_BardTraining(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BardTraining.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Entertain(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Entertain.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Inspire(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Inspire.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Lullaby(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Lullaby.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_BattleHymn(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BattleHymn.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_SoulBond(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SoulBond.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_ShadowBolt(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ShadowBolt.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Dominate(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Dominate.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Attraction(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Attraction.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Repulsion(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Repulsion.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Scorn(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Scorn.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_PsychicShock(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_PsychicShock.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_Stoneskin(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Stoneskin.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Encase(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Encase.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_EarthSprites(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EarthSprites.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_EarthernHammer(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EarthernHammer.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Meteor(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Meteor.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Sentinel(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Sentinel.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_TechnoBit(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_TechnoBit.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_TechnoTurret(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_TechnoTurret.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_TechnoWeapon(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_TechnoWeapon.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_TechnoShield(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_TechnoShield.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Sabotage(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Sabotage.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Overdrive(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Overdrive.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_OrbitalStrike(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_OrbitalStrike.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_BloodGift(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BloodGift.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_IgniteBlood(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_IgniteBlood.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_BloodForBlood(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BloodForBlood.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_BloodShield(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BloodShield.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Rend(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Rend.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_BloodMoon(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BloodMoon.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_EnchantedBody(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EnchantedBody.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Transmutate(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Transmutate.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_EnchanterStone(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EnchanterStone.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_EnchantWeapon(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EnchantWeapon.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Polymorph(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Polymorph.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Shapeshift(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Shapeshift.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Prediction(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Prediction.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_AlterFate(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_AlterFate.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_AccelerateTime(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_AccelerateTime.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_ReverseTime(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ReverseTime.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_ChronostaticField(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ChronostaticField.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Recall(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Recall.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_ChaosTradition(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ChaosTradition.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_WandererCraft(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_WandererCraft.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Cantrips(string skillName)
        {
            int result = 0;
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Cantrips.FirstOrDefault((MagicPowerSkill x) => x.label == skillName);
            bool flag = magicPowerSkill != null;
            if (flag)
            {
                result = magicPowerSkill.level;
            }
            return result;
        }

        private void SingleEvent()
        {
            this.doOnce = false;
        }

        private void DoOncePerLoad()
        {
            if(this.spell_FertileLands == true)
            {
                if(this.fertileLands.Count > 0)
                {
                    List<IntVec3> cellList = ModOptions.Constants.GetGrowthCells();
                    if(cellList.Count != 0)
                    {
                        for (int i = 0; i < fertileLands.Count; i++)
                        {
                            ModOptions.Constants.RemoveGrowthCell(fertileLands[i]);
                        }                       
                    }
                    ModOptions.Constants.SetGrowthCells(fertileLands);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_FertileLands);
                    this.AddPawnAbility(TorannMagicDefOf.TM_DismissFertileLands);
                }
            }
        }

        public override void CompTick()
        {
            bool flag = base.AbilityUser != null;
            if (flag)
            {                
                bool spawned = base.AbilityUser.Spawned;
                if (spawned)
                {
                    bool isMagicUser = this.IsMagicUser && !this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless) && !this.Pawn.IsWildMan();
                    if (isMagicUser)
                    {
                        bool flag3 = !this.firstTick;
                        if (flag3)
                        {
                            this.PostInitializeTick();
                        }
                        if(this.doOnce)
                        {
                            SingleEvent();
                        }
                        base.CompTick();
                        this.age++;
                        if (this.Mana != null)
                        {
                            if (Find.TickManager.TicksGame % 4 == 0 && this.Pawn.CurJob != null && this.Pawn.CurJobDef == JobDefOf.DoBill && this.Pawn.CurJob.targetA != null && this.Pawn.CurJob.targetA.Thing != null)
                            {
                                DoArcaneForging();
                            }
                            if (this.Mana.CurLevel >= (.99f * this.Mana.MaxLevel))
                            {
                                if (this.age > (lastXPGain + magicXPRate))
                                {
                                    MagicData.MagicUserXP++;
                                    lastXPGain = this.age;
                                }
                            }
                            bool flag4 = Find.TickManager.TicksGame % 30 == 0;
                            if (flag4)
                            {
                                bool flag5 = this.MagicUserXP > this.MagicUserXPTillNextLevel;
                                if (flag5)
                                {
                                    this.LevelUp(false);
                                }
                            }
                            if (Find.TickManager.TicksGame % 60 == 0)
                            {
                                if (this.Pawn.IsColonist && !this.magicPowersInitializedForColonist)
                                {
                                    ResolveFactionChange();
                                }
                                else if (!this.Pawn.IsColonist)
                                {
                                    this.magicPowersInitializedForColonist = false;
                                }

                                if (this.Pawn.IsColonist)
                                {
                                    ResolveEnchantments();
                                    for (int i = 0; i < this.summonedMinions.Count; i++)
                                    {
                                        Pawn evaluateMinion = this.summonedMinions[i] as Pawn;
                                        if (evaluateMinion == null || evaluateMinion.Dead || evaluateMinion.Destroyed)
                                        {
                                            this.summonedMinions.Remove(this.summonedMinions[i]);
                                        }
                                    }
                                    ResolveMinions();
                                    ResolveSustainers();
                                    if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Lich))
                                    {
                                        ResolveUndead();
                                    }
                                    ResolveEffecter();
                                    ResolveClassSkills();
                                    ResolveChronomancerTimeMark();
                                }
                                if (this.Mana.CurLevel < 0)
                                {
                                    this.Mana.CurLevel = 0;
                                }
                                else if (this.Mana.CurLevel > (this.Mana.MaxLevel + .01f))
                                {
                                    this.Mana.CurLevel -= .01f;
                                }
                                else if(this.Mana.CurLevel > (this.Mana.MaxLevel))
                                {
                                    this.Mana.CurLevel = this.Mana.MaxLevel;
                                }

                            }
                            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                            if (this.autocastTick < Find.TickManager.TicksGame)  //180 default
                            {
                                if (this.Pawn.IsColonist && !this.Pawn.Dead && !this.Pawn.Downed && this.Pawn.Map != null && this.Pawn.story != null && this.Pawn.story.traits != null && this.MagicData != null && this.AbilityData != null)
                                {
                                    ResolveAutoCast();
                                }
                                this.autocastTick = Find.TickManager.TicksGame + (int)Rand.Range(.8f * settingsRef.autocastEvaluationFrequency, 1.2f * settingsRef.autocastEvaluationFrequency);
                            }
                            if (!this.Pawn.IsColonist && settingsRef.AICasting && settingsRef.AIAggressiveCasting && Find.TickManager.TicksGame > this.nextAICastAttemptTick) //Aggressive AI Casting
                            {
                                this.nextAICastAttemptTick = Find.TickManager.TicksGame + Rand.Range(200, 300);
                                IEnumerable<AbilityUserAIProfileDef> enumerable = this.Pawn.EligibleAIProfiles();
                                if (enumerable != null && enumerable.Count() > 0)
                                {
                                    foreach (AbilityUserAIProfileDef item in enumerable)
                                    {
                                        if (item != null)
                                        {
                                            AbilityAIDef useThisAbility = null;
                                            if (item.decisionTree != null)
                                            {
                                                useThisAbility = item.decisionTree.RecursivelyGetAbility(this.Pawn);
                                            }
                                            if (useThisAbility != null)
                                            {
                                                ThingComp val = this.Pawn.AllComps.First((ThingComp comp) => ((object)comp).GetType() == item.compAbilityUserClass);
                                                CompAbilityUser compAbilityUser = val as CompAbilityUser;
                                                if (compAbilityUser != null)
                                                {
                                                    PawnAbility pawnAbility = compAbilityUser.AbilityData.AllPowers.First((PawnAbility ability) => ability.Def == useThisAbility.ability);
                                                    string reason = "";
                                                    if (pawnAbility.CanCastPowerCheck(AbilityContext.AI, out reason))
                                                    {
                                                        LocalTargetInfo target = useThisAbility.Worker.TargetAbilityFor(useThisAbility, this.Pawn);
                                                        if (target.IsValid)
                                                        {
                                                            pawnAbility.UseAbility(AbilityContext.Player, target);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (Find.TickManager.TicksGame % this.overdriveFrequency == 0)
                        {
                            if(this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer))
                            {
                                ResolveTechnomancerOverdrive();
                            }
                        }
                        if(Find.TickManager.TicksGame % 600 == 0)
                        {
                            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Warlock))
                            {
                                ResolveWarlockEmpathy();
                            }
                        }
                        if(Find.TickManager.TicksGame % 2000 == 0)
                        {
                            if(this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus))
                            {
                                ResolveSuccubusLovin();
                            }
                        }
                    }
                }
                else
                {
                    if (Find.TickManager.TicksGame % 600 == 0)
                    {
                        if (this.Pawn.Map == null)
                        {
                            if (this.IsMagicUser)
                            {
                                int num;
                                if (AbilityData?.AllPowers != null)
                                {
                                    AbilityData obj = AbilityData;
                                    num = ((obj != null && obj.AllPowers.Count > 0) ? 1 : 0);
                                }
                                else
                                {
                                    num = 0;
                                }
                                if (num != 0)
                                {
                                    foreach (PawnAbility allPower in AbilityData.AllPowers)
                                    {
                                        allPower.CooldownTicksLeft -= 600;
                                        if(allPower.CooldownTicksLeft <= 0)
                                        {
                                            allPower.CooldownTicksLeft = 0;
                                        }
                                    }
                                }
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
                        this.firstTick = true;
                        this.Initialize();
                        this.ResolveMagicTab();
                        this.ResolveMagicPowers();
                        this.ResolveMana();
                        this.DoOncePerLoad();
                    }
                }
            }
        }

        public bool IsMagicUser
        {
            get
            {
                bool flag = base.AbilityUser != null;
                bool result;
                if (flag)
                {
                    bool flag3 = base.AbilityUser.story != null;
                    if (flag3)
                    {
                        bool flag4 = base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Enchanter) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.BloodMage) || 
                            base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Technomancer) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Geomancer) || 
                            base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Warlock) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Succubus) || 
                            base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Faceless) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.InnerFire) || 
                            base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.StormBorn) || 
                            base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Arcanist) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Paladin) || 
                            base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Summoner) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Druid) || 
                            (base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Lich)) || 
                            base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Priest) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.TM_Bard) ||
                            base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Chronomancer) || TM_Calc.IsWanderer(base.AbilityUser) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.ChaosMage);
                        if (flag4)
                        {
                            result = true;
                            return result;
                        }
                    }
                }
                result = false;
                return result;
            }
        }   
        
        private void DrawTechnoBit()
        {
            float num = Mathf.Lerp(1.2f, 1.55f, 1f);
            if(this.bitFloatingDown)
            {
                if(this.bitOffset < .38f)
                {
                    this.bitFloatingDown = false;
                }
                this.bitOffset -= .001f;                    
            }
            else
            {
                if (this.bitOffset > .57f)
                {
                    this.bitFloatingDown = true;
                }
                this.bitOffset += .001f;
            }

            this.bitPosition = this.AbilityUser.Drawer.DrawPos;
            this.bitPosition.x -= .5f + Rand.Range(-.01f, .01f);
            this.bitPosition.z += this.bitOffset;
            this.bitPosition.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
            float angle = 0f;
            Vector3 s = new Vector3(.35f, 1f, .35f);
            Matrix4x4 matrix = default(Matrix4x4);
            matrix.SetTRS(this.bitPosition, Quaternion.AngleAxis(angle, Vector3.up), s);
            Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.bitMat, 0);
        }

        private void DrawMageLight()
        {
            if (!mageLightSet)
            {
                float num = Mathf.Lerp(1.2f, 1.55f, 1f);
                Vector3 lightPos = Vector3.zero;

                lightPos = this.AbilityUser.Drawer.DrawPos;
                lightPos.x -= .5f;
                lightPos.z += .6f;

                lightPos.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
                float angle = Rand.Range(0, 360);
                Vector3 s = new Vector3(.27f, .5f, .27f);
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(lightPos, Quaternion.AngleAxis(angle, Vector3.up), s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.mageLightMat, 0);
            }

        }

        public void DrawMageMark()
        {
            float num = Mathf.Lerp(1.2f, 1.55f, 1f);
            Vector3 vector = this.AbilityUser.Drawer.DrawPos;
            vector.x = vector.x + .45f;
            vector.z = vector.z + .45f;
            vector.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
            float angle = 0f;
            Vector3 s = new Vector3(.28f, 1f, .28f);
            Matrix4x4 matrix = default(Matrix4x4);
            matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
            if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.InnerFire))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.fireMarkMat, 0);
            }
            else if(this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.iceMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.StormBorn))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.lightningMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Arcanist))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.arcanistMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Paladin))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.paladinMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Summoner))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.summonerMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Druid))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.druidMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Lich))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.necroMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Priest))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.priestMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.TM_Bard))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.bardMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Succubus) || this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Warlock))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.demonkinMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Geomancer))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.earthMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Technomancer))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.technoMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.BloodMage))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.bloodmageMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Enchanter))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.enchanterMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Chronomancer))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.chronomancerMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.ChaosMage))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.chaosMarkMat, 0);
            }
            else if (TM_Calc.IsWanderer(this.AbilityUser))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.wandererMarkMat, 0);
            }
            else 
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.mageMarkMat, 0);
            }
            
        }

        public void DrawEnchantMark()
        {
            float num = Mathf.Lerp(1.2f, 1.55f, 1f);
            Vector3 vector = this.AbilityUser.Drawer.DrawPos;
            vector.x = vector.x + .45f;
            vector.z = vector.z + .45f;
            vector.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
            float angle = 0f;
            Vector3 s = new Vector3(.5f, 1f, .5f);
            Matrix4x4 matrix = default(Matrix4x4);
            matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
            Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.enchantMark, 0);           

        }

        public void DrawScornWings()
        {
            bool flag = !this.Pawn.Dead && !this.Pawn.Downed;
            if (flag)
            {
                float num = Mathf.Lerp(1.2f, 1.55f, 1f);
                Vector3 vector = this.Pawn.Drawer.DrawPos;
                vector.y = Altitudes.AltitudeFor(AltitudeLayer.Pawn);
                if (this.Pawn.Rotation == Rot4.North)
                {
                    vector.y = Altitudes.AltitudeFor(AltitudeLayer.PawnState);
                }
                float angle = (float)Rand.Range(0, 360);
                Vector3 s = new Vector3(3f, 3f, 3f);
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(vector, Quaternion.AngleAxis(0f, Vector3.up), s);
                if (this.Pawn.Rotation == Rot4.South || this.Pawn.Rotation == Rot4.North)
                {                   
                    Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.scornWingsNS, 0);
                }
                if (this.Pawn.Rotation == Rot4.East)
                {
                    Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.scornWingsE, 0);
                }
                if (this.Pawn.Rotation == Rot4.West)
                {
                    Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.scornWingsW, 0);
                }
            }
        }

        public int MagicUserLevel
        {
            get
            {
                return this.MagicData.MagicUserLevel;
            }
            set
            {
                bool flag = value > this.MagicData.MagicUserLevel;
                if (flag)
                {
                    this.MagicData.MagicAbilityPoints++;
                    bool flag2 = this.MagicData.MagicUserXP < value * 500;
                    if (flag2)
                    {
                        this.MagicData.MagicUserXP = value * 500;
                    }
                }
                this.MagicData.MagicUserLevel = value;
            }
        }

        public int MagicUserXP
        {
            get
            {
                return this.MagicData.MagicUserXP;
            }
            set
            {
                this.MagicData.MagicUserXP = value;
            }
        }

        public float XPLastLevel
        {
            get
            {
                float result = 0f;
                bool flag = this.MagicUserLevel > 0;
                if (flag)
                {
                    result = (float)(this.MagicUserLevel * 500); 
                }
                return result;
            }
        }

        public float XPTillNextLevelPercent
        {
            get
            {
                return ((float)this.MagicData.MagicUserXP - this.XPLastLevel) / ((float)this.MagicUserXPTillNextLevel - this.XPLastLevel);
            }
        }

        public int MagicUserXPTillNextLevel
        {
            get
            {
                return (this.MagicData.MagicUserLevel +1) * 500; //was this.magicUserLevel * 100
            }
        }

        public void LevelUp(bool hideNotification = false)
        {
            if (!(this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless) || this.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Wayfarer)))
            {
                if (this.MagicUserLevel < 150)
                {
                    this.MagicUserLevel++;
                    bool flag = !hideNotification;
                    if (flag)
                    {
                        ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                        if (Pawn.IsColonist && settingsRef.showLevelUpMessage)
                        {
                            Messages.Message(TranslatorFormattedStringExtensions.Translate("TM_MagicLevelUp",
                        this.parent.Label
                            ), this.Pawn, MessageTypeDefOf.PositiveEvent);
                        }
                    }                    
                }
                else
                {
                    this.MagicUserXP = (int)this.XPLastLevel;
                }
            }
        }

        public void LevelUpPower(MagicPower power)
        {
            foreach (AbilityUser.AbilityDef current in power.TMabilityDefs)
            {
                base.RemovePawnAbility(current);
            }

            power.level++;
            base.AddPawnAbility(power.nextLevelAbilityDef, true, -1f);
            this.UpdateAbilities();
        }

        public Need_Mana Mana
        {
            get
            {
                if (!this.AbilityUser.DestroyedOrNull() && !this.AbilityUser.Dead)
                {
                    return base.AbilityUser.needs.TryGetNeed<Need_Mana>();
                }
                return null;
            }
        }

        public void ResolveFactionChange()
        {
            if (!this.colonistPowerCheck)
            {
                RemovePowers();
                this.spell_BattleHymn = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_BattleHymn);
                this.spell_Blizzard = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_Blizzard);
                this.spell_BloodMoon = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_BloodMoon);
                this.spell_EyeOfTheStorm = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_EyeOfTheStorm);
                this.spell_Firestorm = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_Firestorm);
                this.spell_FoldReality = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_FoldReality);
                this.spell_HolyWrath = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_HolyWrath);
                this.spell_LichForm = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_BattleHymn);
                this.spell_Meteor = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_Meteor);
                this.spell_OrbitalStrike = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_OrbitalStrike);
                this.spell_PsychicShock = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_PsychicShock);
                this.spell_RegrowLimb = false;
                this.spell_Resurrection = false;
                this.spell_Scorn = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_Scorn);
                this.spell_Shapeshift = false;
                this.spell_SummonPoppi = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_SummonPoppi);
                this.RemovePawnAbility(TorannMagicDefOf.TM_Recall);
                this.spell_Recall = false;
                AssignAbilities();
            }
            this.magicPowersInitializedForColonist = true;
            this.colonistPowerCheck = true;
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            bool flag = CompAbilityUserMagic.MagicAbilities == null;
            if (flag)
            {
                if (this.magicPowersInitialized == false)
                {                    
                    MagicData.MagicUserLevel = 0;
                    MagicData.MagicAbilityPoints = 0;
                    AssignAbilities();
                    if(!this.Pawn.IsColonist)
                    {
                        InitializeSpell();
                        this.colonistPowerCheck = false;
                    }
                }
                this.magicPowersInitialized = true;
                base.UpdateAbilities();                                
            }
        }

        private void AssignAbilities()
        {
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            float hardModeMasterChance = .35f;
            float masterChance = .05f;
            Pawn abilityUser = base.AbilityUser;
            bool flag2;
            if (abilityUser != null && abilityUser.story != null && abilityUser.story.traits != null)
            {
                flag2 = TM_Calc.IsWanderer(abilityUser);
                if (flag2)
                {
                    //Log.Message("Initializing Wanderer Abilities");
                    if (!abilityUser.IsColonist)
                    {
                        this.spell_ArcaneBolt = true;
                        this.AddPawnAbility(TorannMagicDefOf.TM_ArcaneBolt);
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.InnerFire);
                if (flag2)
                {
                    //Log.Message("Initializing Inner Fire Abilities");
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_RayofHope);
                            this.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_RayofHope).learned = true;
                        }
                        else
                        {
                            MagicPower mpIF = this.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_RayofHope);
                            mpIF.learned = false;
                        }
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Firebolt);
                            this.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Firebolt).learned = true;
                        }
                        else
                        {
                            MagicPower mpIF = this.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Firebolt);
                            mpIF.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Fireclaw);
                            this.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Fireclaw).learned = true;
                        }
                        else
                        {
                            MagicPower mpIF = this.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Fireclaw);
                            mpIF.learned = false;
                        }
                        if (Rand.Chance(.2f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Fireball);
                            this.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Fireball).learned = true;
                        }
                        else
                        {
                            MagicPower mpIF = this.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Fireball);
                            mpIF.learned = false;
                        }
                    }
                    else
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_RayofHope);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Firebolt);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Fireclaw);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Fireball);

                        if (!abilityUser.IsColonist)
                        {
                            if ((settingsRef.AIHardMode && Rand.Chance(hardModeMasterChance)) || Rand.Chance(masterChance))
                            {
                                this.spell_Firestorm = true;
                            }
                        }
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost);
                if (flag2)
                {
                    //Log.Message("Initializing Heart of Frost Abilities");
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Soothe);
                            this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Soothe).learned = true;
                        }
                        else
                        {
                            MagicPower mpHoF = this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Soothe);
                            mpHoF.learned = false;
                        }
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Icebolt);
                            this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Icebolt).learned = true;
                        }
                        else
                        {
                            MagicPower mpHoF = this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Icebolt);
                            mpHoF.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Snowball);
                            this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Snowball).learned = true;
                        }
                        else
                        {
                            MagicPower mpHoF = this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Snowball);
                            mpHoF.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_FrostRay);
                            this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_FrostRay).learned = true;
                        }
                        else
                        {
                            MagicPower mpHoF = this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_FrostRay);
                            mpHoF.learned = false;
                        }
                        if (Rand.Chance(.7f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Rainmaker);
                            this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Rainmaker).learned = true;
                            this.spell_Rain = true;
                        }
                        else
                        {
                            MagicPower mpHoF = this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Rainmaker);
                            mpHoF.learned = false;
                        }
                    }
                    else
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Soothe);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Icebolt);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Snowball);
                        this.AddPawnAbility(TorannMagicDefOf.TM_FrostRay);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Rainmaker);
                        this.spell_Rain = true;

                        if (!abilityUser.IsColonist)
                        {
                            if ((settingsRef.AIHardMode && Rand.Chance(hardModeMasterChance)) || Rand.Chance(masterChance))
                            {
                                this.spell_Blizzard = true;
                            }
                        }
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.StormBorn);
                if (flag2)
                {
                    //Log.Message("Initializing Storm Born Abilities");
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_AMP);
                            this.MagicData.MagicPowersSB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_AMP).learned = true;
                        }
                        else
                        {
                            MagicPower mpSB = this.MagicData.MagicPowersSB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_AMP);
                            mpSB.learned = false;
                        }
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_LightningBolt);
                            this.MagicData.MagicPowersSB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_LightningBolt).learned = true;
                        }
                        else
                        {
                            MagicPower mpSB = this.MagicData.MagicPowersSB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_LightningBolt);
                            mpSB.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_LightningCloud);
                            this.MagicData.MagicPowersSB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_LightningCloud).learned = true;
                        }
                        else
                        {
                            MagicPower mpSB = this.MagicData.MagicPowersSB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_LightningCloud);
                            mpSB.learned = false;
                        }
                        if (Rand.Chance(.2f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_LightningStorm);
                            this.MagicData.MagicPowersSB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_LightningStorm).learned = true;
                        }
                        else
                        {
                            MagicPower mpSB = this.MagicData.MagicPowersSB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_LightningStorm);
                            mpSB.learned = false;
                        }
                    }
                    else
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_AMP);
                        this.AddPawnAbility(TorannMagicDefOf.TM_LightningBolt);
                        this.AddPawnAbility(TorannMagicDefOf.TM_LightningCloud);
                        this.AddPawnAbility(TorannMagicDefOf.TM_LightningStorm);

                        if (!abilityUser.IsColonist)
                        {
                            if ((settingsRef.AIHardMode && Rand.Chance(hardModeMasterChance)) || Rand.Chance(masterChance))
                            {
                                this.spell_EyeOfTheStorm = true;
                            }
                        }
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Arcanist);
                if (flag2)
                {
                    //Log.Message("Initializing Arcane Abilities");
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Shadow);
                            this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shadow).learned = true;
                        }
                        else
                        {
                            this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shadow).learned = false;
                        }
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_MagicMissile);
                            this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_MagicMissile).learned = true;
                        }
                        else
                        {
                            this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_MagicMissile).learned = false;
                        }
                        if (Rand.Chance(.7f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Blink);
                            this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Blink).learned = true;
                            this.spell_Blink = true;
                        }
                        else
                        {
                            this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Blink).learned = false;
                        }
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Summon);
                            this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Summon).learned = true;
                        }
                        else
                        {
                            this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Summon).learned = false;
                        }
                        if (Rand.Chance(.2f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Teleport);
                            this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Teleport).learned = true;
                            this.spell_Teleport = true;
                        }
                        else
                        {
                            this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Teleport).learned = false;
                        }
                    }
                    else
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Shadow);
                        this.AddPawnAbility(TorannMagicDefOf.TM_MagicMissile);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Blink);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Summon);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Teleport);  //Pending Redesign (graphics?)
                        this.spell_Blink = true;
                        this.spell_Teleport = true;

                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Paladin);
                if (flag2)
                {
                    //Log.Message("Initializing Paladin Abilities");
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Heal);
                            this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Heal).learned = true;
                            this.spell_Heal = true;
                        }
                        else
                        {
                            MagicPower mpP = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Heal);
                            mpP.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Shield);
                            this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield).learned = true;
                        }
                        else
                        {
                            MagicPower mpP = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield);
                            mpP.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_ValiantCharge);
                            this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ValiantCharge).learned = true;
                        }
                        else
                        {
                            MagicPower mpP = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ValiantCharge);
                            mpP.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Overwhelm);
                            this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Overwhelm).learned = true;
                        }
                        else
                        {
                            MagicPower mpP = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Overwhelm);
                            mpP.learned = false;
                        }
                    }
                    else
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Heal);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Shield);
                        this.AddPawnAbility(TorannMagicDefOf.TM_ValiantCharge);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Overwhelm);
                        this.spell_Heal = true;

                        if (!abilityUser.IsColonist)
                        {
                            if ((settingsRef.AIHardMode && Rand.Chance(hardModeMasterChance)) || Rand.Chance(masterChance))
                            {
                                this.spell_HolyWrath = true;
                            }
                        }
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Summoner);
                if (flag2)
                {
                    //Log.Message("Initializing Summoner Abilities");
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_SummonMinion);
                            this.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonMinion).learned = true;
                            this.spell_SummonMinion = true;
                        }
                        else
                        {
                            MagicPower mpS = this.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonMinion);
                            mpS.learned = false;
                        }
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_SummonPylon);
                            this.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonPylon).learned = true;
                        }
                        else
                        {
                            MagicPower mpS = this.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonPylon);
                            mpS.learned = false;
                        }
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_SummonExplosive);
                            this.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonExplosive).learned = true;
                        }
                        else
                        {
                            MagicPower mpS = this.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonExplosive);
                            mpS.learned = false;
                        }
                        if (Rand.Chance(.2f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_SummonElemental);
                            this.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonElemental).learned = true;
                        }
                        else
                        {
                            MagicPower mpS = this.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonElemental);
                            mpS.learned = false;
                        }
                    }
                    else
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_SummonMinion);
                        this.AddPawnAbility(TorannMagicDefOf.TM_SummonPylon);
                        this.AddPawnAbility(TorannMagicDefOf.TM_SummonExplosive);
                        this.AddPawnAbility(TorannMagicDefOf.TM_SummonElemental);
                        this.spell_SummonMinion = true;

                        if (!abilityUser.IsColonist)
                        {
                            if ((settingsRef.AIHardMode && Rand.Chance(hardModeMasterChance)) || Rand.Chance(masterChance))
                            {
                                this.spell_SummonPoppi = true;
                            }
                        }
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Druid);
                if (flag2)
                {
                    // Log.Message("Initializing Druid Abilities");
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Poison);
                            this.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Poison).learned = true;
                        }
                        else
                        {
                            MagicPower mpD = this.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Poison);
                            mpD.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_SootheAnimal);
                            this.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SootheAnimal).learned = true;
                        }
                        else
                        {
                            MagicPower mpD = this.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SootheAnimal);
                            mpD.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Regenerate);
                            this.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Regenerate).learned = true;
                        }
                        else
                        {
                            MagicPower mpD = this.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Regenerate);
                            mpD.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_CureDisease);
                            this.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_CureDisease).learned = true;
                        }
                        else
                        {
                            MagicPower mpD = this.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_CureDisease);
                            mpD.learned = false;
                        }
                    }
                    else
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Poison);
                        this.AddPawnAbility(TorannMagicDefOf.TM_SootheAnimal);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Regenerate);
                        this.AddPawnAbility(TorannMagicDefOf.TM_CureDisease);
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || abilityUser.story.traits.HasTrait(TorannMagicDefOf.Lich);
                if (flag2)
                {
                    //Log.Message("Initializing Necromancer Abilities");
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_RaiseUndead);
                            this.MagicData.MagicPowersN.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_RaiseUndead).learned = true;
                        }
                        else
                        {
                            MagicPower mpN = this.MagicData.MagicPowersN.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_RaiseUndead);
                            mpN.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_DeathMark);
                            this.MagicData.MagicPowersN.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_DeathMark).learned = true;
                        }
                        else
                        {
                            MagicPower mpN = this.MagicData.MagicPowersN.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_DeathMark);
                            mpN.learned = false;
                        }
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_FogOfTorment);
                            this.MagicData.MagicPowersN.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_FogOfTorment).learned = true;
                        }
                        else
                        {
                            MagicPower mpN = this.MagicData.MagicPowersN.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_FogOfTorment);
                            mpN.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_ConsumeCorpse);
                            this.MagicData.MagicPowersN.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ConsumeCorpse).learned = true;
                        }
                        else
                        {
                            MagicPower mpN = this.MagicData.MagicPowersN.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ConsumeCorpse);
                            mpN.learned = false;
                        }
                        if (Rand.Chance(.2f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_CorpseExplosion);
                            this.MagicData.MagicPowersN.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_CorpseExplosion).learned = true;
                        }
                        else
                        {
                            MagicPower mpN = this.MagicData.MagicPowersN.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_CorpseExplosion);
                            mpN.learned = false;
                        }
                    }
                    else
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_RaiseUndead);
                        this.AddPawnAbility(TorannMagicDefOf.TM_DeathMark);
                        this.AddPawnAbility(TorannMagicDefOf.TM_FogOfTorment);
                        this.AddPawnAbility(TorannMagicDefOf.TM_ConsumeCorpse);
                        this.AddPawnAbility(TorannMagicDefOf.TM_CorpseExplosion);

                        if (!abilityUser.IsColonist)
                        {
                            if ((settingsRef.AIHardMode && Rand.Chance(hardModeMasterChance)) || Rand.Chance(masterChance))
                            {
                                this.AddPawnAbility(TorannMagicDefOf.TM_DeathBolt);
                            }
                        }
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Priest);
                if (flag2)
                {
                    //Log.Message("Initializing Priest Abilities");
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_AdvancedHeal);
                            this.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_AdvancedHeal).learned = true;
                        }
                        else
                        {
                            MagicPower mpPR = this.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_AdvancedHeal);
                            mpPR.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Purify);
                            this.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Purify).learned = true;
                        }
                        else
                        {
                            MagicPower mpPR = this.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Purify);
                            mpPR.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_HealingCircle);
                            this.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_HealingCircle).learned = true;
                        }
                        else
                        {
                            MagicPower mpPR = this.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_HealingCircle);
                            mpPR.learned = false;
                        }
                        if (Rand.Chance(.4f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_BestowMight);
                            this.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_BestowMight).learned = true;
                        }
                        else
                        {
                            MagicPower mpPR = this.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_BestowMight);
                            mpPR.learned = false;
                        }
                    }
                    else
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_AdvancedHeal);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Purify);
                        this.AddPawnAbility(TorannMagicDefOf.TM_HealingCircle);
                        this.AddPawnAbility(TorannMagicDefOf.TM_BestowMight);
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.TM_Bard);
                if (flag2)
                {
                    //Log.Message("Initializing Priest Abilities");
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        //if (Rand.Chance(.5f))
                        //{
                        //    this.AddPawnAbility(TorannMagicDefOf.TM_BardTraining);
                        //}
                        if (true)
                        {
                            MagicPower mpB = this.MagicData.MagicPowersB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_BardTraining);
                            mpB.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Entertain);
                            this.MagicData.MagicPowersB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Entertain).learned = true;
                        }
                        else
                        {
                            MagicPower mpB = this.MagicData.MagicPowersB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Entertain);
                            mpB.learned = false;
                        }
                        //if (Rand.Chance(.3f))
                        //{
                        //    this.AddPawnAbility(TorannMagicDefOf.TM_Inspire);
                        //}
                        //else
                        //{
                        //    MagicPower mpB = this.MagicData.MagicPowersB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Inspire);
                        //    mpB.learned = false;
                        //}
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Lullaby);
                            this.MagicData.MagicPowersB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Lullaby).learned = true;
                        }
                        else
                        {
                            MagicPower mpB = this.MagicData.MagicPowersB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Lullaby);
                            mpB.learned = false;
                        }
                    }
                    else
                    {
                        //this.AddPawnAbility(TorannMagicDefOf.TM_BardTraining);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Entertain);
                        //this.AddPawnAbility(TorannMagicDefOf.TM_Inspire);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Lullaby);

                        if (!abilityUser.IsColonist)
                        {
                            if ((settingsRef.AIHardMode && Rand.Chance(hardModeMasterChance)) || Rand.Chance(masterChance))
                            {
                                this.spell_BattleHymn = true;
                            }
                        }
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Succubus);
                if (flag2)
                {
                    //Log.Message("Initializing Succubus Abilities");
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        if (Rand.Chance(.7f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_SoulBond);
                            this.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SoulBond).learned = true;
                        }
                        else
                        {
                            MagicPower mpSD = this.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SoulBond);
                            mpSD.learned = false;
                        }

                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_ShadowBolt);
                            this.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt).learned = true;
                        }
                        else
                        {
                            MagicPower mpSD = this.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt);
                            mpSD.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Dominate);
                            this.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Dominate).learned = true;
                        }
                        else
                        {
                            MagicPower mpSD = this.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Dominate);
                            mpSD.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Attraction);
                            this.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Attraction).learned = true;
                        }
                        else
                        {
                            MagicPower mpSD = this.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Attraction);
                            mpSD.learned = false;
                        }
                    }
                    else
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_SoulBond);
                        this.AddPawnAbility(TorannMagicDefOf.TM_ShadowBolt);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Dominate);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Attraction);

                        if (!abilityUser.IsColonist)
                        {
                            if ((settingsRef.AIHardMode && Rand.Chance(hardModeMasterChance)) || Rand.Chance(masterChance))
                            {
                                this.spell_Scorn = true;
                            }
                        }
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Warlock);
                if (flag2)
                {
                    //Log.Message("Initializing Succubus Abilities");
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        if (Rand.Chance(.7f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_SoulBond);
                            this.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SoulBond).learned = true;
                        }
                        else
                        {
                            MagicPower mpWD = this.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SoulBond);
                            mpWD.learned = false;
                        }
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_ShadowBolt);
                            this.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt).learned = true;
                        }
                        else
                        {
                            MagicPower mpWD = this.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt);
                            mpWD.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Dominate);
                            this.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Dominate).learned = true;
                        }
                        else
                        {
                            MagicPower mpWD = this.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Dominate);
                            mpWD.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Repulsion);
                            this.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Repulsion).learned = true;
                        }
                        else
                        {
                            MagicPower mpWD = this.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Repulsion);
                            mpWD.learned = false;
                        }
                    }
                    else
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_SoulBond);
                        this.AddPawnAbility(TorannMagicDefOf.TM_ShadowBolt);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Dominate);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Repulsion);
                        if (!abilityUser.IsColonist)
                        {
                            if ((settingsRef.AIHardMode && Rand.Chance(hardModeMasterChance)) || Rand.Chance(masterChance))
                            {
                                this.spell_PsychicShock = true;
                            }
                        }
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Geomancer);
                if (flag2)
                {
                    //Log.Message("Initializing Heart of Frost Abilities");
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        if (Rand.Chance(.4f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Stoneskin);
                            this.MagicData.MagicPowersG.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Stoneskin).learned = true;
                        }
                        else
                        {
                            MagicPower mpG = this.MagicData.MagicPowersG.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Stoneskin);
                            mpG.learned = false;
                        }
                        if (Rand.Chance(.6f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Encase);
                            this.MagicData.MagicPowersG.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Encase).learned = true;
                        }
                        else
                        {
                            MagicPower mpG = this.MagicData.MagicPowersG.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Encase);
                            mpG.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_EarthSprites);
                            this.MagicData.MagicPowersG.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EarthSprites).learned = true;
                        }
                        else
                        {
                            MagicPower mpG = this.MagicData.MagicPowersG.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EarthSprites);
                            mpG.learned = false;
                        }
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_EarthernHammer);
                            this.MagicData.MagicPowersG.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EarthernHammer).learned = true;
                        }
                        else
                        {
                            MagicPower mpG = this.MagicData.MagicPowersG.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EarthernHammer);
                            mpG.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Sentinel);
                            this.MagicData.MagicPowersG.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Sentinel).learned = true;
                        }
                        else
                        {
                            MagicPower mpG = this.MagicData.MagicPowersG.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Sentinel);
                            mpG.learned = false;
                        }
                    }
                    else
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Stoneskin);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Encase);
                        this.AddPawnAbility(TorannMagicDefOf.TM_EarthSprites);
                        this.AddPawnAbility(TorannMagicDefOf.TM_EarthernHammer);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Sentinel);

                        if (!abilityUser.IsColonist)
                        {
                            if ((settingsRef.AIHardMode && Rand.Chance(hardModeMasterChance)) || Rand.Chance(masterChance))
                            {
                                this.AddPawnAbility(TorannMagicDefOf.TM_Meteor);
                                this.spell_Meteor = true;
                            }
                        }
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Technomancer);
                if (flag2)
                {
                    //Log.Message("Initializing Technomancer Abilities");

                    this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TechnoBit).learned = false;
                    this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TechnoTurret).learned = false;
                    this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TechnoWeapon).learned = false;
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        if (Rand.Chance(.4f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_TechnoShield);
                            this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TechnoShield).learned = true;
                        }
                        else
                        {
                            this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TechnoShield).learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Sabotage);
                            this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Sabotage).learned = true;
                        }
                        else
                        {
                            this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Sabotage).learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Overdrive);
                            this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Overdrive).learned = true;
                        }
                        else
                        {
                            this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Overdrive).learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.spell_OrbitalStrike = true;
                            this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_OrbitalStrike).learned = true;
                            this.InitializeSpell();
                        }
                    }
                    else
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_TechnoShield);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Sabotage);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Overdrive);
                        if (!abilityUser.IsColonist)
                        {
                            if ((settingsRef.AIHardMode && Rand.Chance(hardModeMasterChance)) || Rand.Chance(masterChance))
                            {
                                this.spell_OrbitalStrike = true;
                            }
                        }
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.BloodMage);
                if (flag2)
                {
                    //Log.Message("Initializing Heart of Frost Abilities");
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        if (Rand.Chance(1f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_BloodGift);
                            this.MagicData.MagicPowersBM.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_BloodGift).learned = true;
                        }
                        if (Rand.Chance(.4f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_IgniteBlood);
                            this.MagicData.MagicPowersBM.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_IgniteBlood).learned = true;
                        }
                        else
                        {
                            MagicPower mpBM = this.MagicData.MagicPowersBM.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_IgniteBlood);
                            mpBM.learned = false;
                        }
                        if (Rand.Chance(.4f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_BloodForBlood);
                            this.MagicData.MagicPowersBM.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_BloodForBlood).learned = true;
                        }
                        else
                        {
                            MagicPower mpBM = this.MagicData.MagicPowersBM.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_BloodForBlood);
                            mpBM.learned = false;
                        }
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_BloodShield);
                            this.MagicData.MagicPowersBM.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_BloodShield).learned = true;
                        }
                        else
                        {
                            MagicPower mpBM = this.MagicData.MagicPowersBM.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_BloodShield);
                            mpBM.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Rend);
                            this.MagicData.MagicPowersBM.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Rend).learned = true;
                        }
                        else
                        {
                            MagicPower mpBM = this.MagicData.MagicPowersBM.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Rend);
                            mpBM.learned = false;
                        }
                    }
                    else
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_BloodGift);
                        this.AddPawnAbility(TorannMagicDefOf.TM_IgniteBlood);
                        this.AddPawnAbility(TorannMagicDefOf.TM_BloodForBlood);
                        this.AddPawnAbility(TorannMagicDefOf.TM_BloodShield);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Rend);
                        if (!abilityUser.IsColonist)
                        {
                            if ((settingsRef.AIHardMode && Rand.Chance(hardModeMasterChance)) || Rand.Chance(masterChance))
                            {
                                this.AddPawnAbility(TorannMagicDefOf.TM_BloodMoon);
                                this.spell_BloodMoon = true;
                            }
                        }
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Enchanter);
                if (flag2)
                {
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        if (Rand.Chance(.5f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_EnchantedBody);
                            this.AddPawnAbility(TorannMagicDefOf.TM_EnchantedAura);
                            this.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchantedAura).learned = true;
                            this.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchantedBody).learned = true;
                            this.spell_EnchantedAura = true;
                        }
                        else
                        {
                            MagicPower mpE = this.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchantedBody);
                            mpE.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Transmutate);
                            this.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Transmutate).learned = true;
                        }
                        else
                        {
                            MagicPower mpE = this.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Transmutate);
                            mpE.learned = false;
                        }
                        if (Rand.Chance(.4f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_EnchanterStone);
                            this.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchanterStone).learned = true;
                        }
                        else
                        {
                            MagicPower mpE = this.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchanterStone);
                            mpE.learned = false;
                        }
                        if (Rand.Chance(.4f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_EnchantWeapon);
                            this.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchantWeapon).learned = true;
                        }
                        else
                        {
                            MagicPower mpE = this.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchantWeapon);
                            mpE.learned = false;
                        }
                        if (Rand.Chance(.4f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Polymorph);
                            this.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Polymorph).learned = true;
                        }
                        else
                        {
                            MagicPower mpE = this.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Polymorph);
                            mpE.learned = false;
                        }
                    }
                    else
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_EnchantedBody);
                        this.AddPawnAbility(TorannMagicDefOf.TM_EnchantedAura);
                        this.spell_EnchantedAura = true;
                        this.AddPawnAbility(TorannMagicDefOf.TM_Transmutate);
                        this.AddPawnAbility(TorannMagicDefOf.TM_EnchanterStone);
                        this.AddPawnAbility(TorannMagicDefOf.TM_EnchantWeapon);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Polymorph);
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Chronomancer);
                if (flag2)
                {
                    //Log.Message("Initializing Chronomancer Abilities");
                    if (abilityUser.IsColonist && !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false))
                    {
                        if (Rand.Chance(.4f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_Prediction);
                            this.MagicData.MagicPowersC.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Prediction).learned = true;
                        }
                        else
                        {
                            MagicPower mpC = this.MagicData.MagicPowersC.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Prediction);
                            mpC.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_AlterFate);
                            this.MagicData.MagicPowersC.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_AlterFate).learned = true;
                        }
                        else
                        {
                            MagicPower mpC = this.MagicData.MagicPowersC.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_AlterFate);
                            mpC.learned = false;
                        }
                        if (Rand.Chance(.4f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_AccelerateTime);
                            this.MagicData.MagicPowersC.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_AccelerateTime).learned = true;
                        }
                        else
                        {
                            MagicPower mpC = this.MagicData.MagicPowersC.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_AccelerateTime);
                            mpC.learned = false;
                        }
                        if (Rand.Chance(.4f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_ReverseTime);
                            this.MagicData.MagicPowersC.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ReverseTime).learned = true;
                        }
                        else
                        {
                            MagicPower mpC = this.MagicData.MagicPowersC.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ReverseTime);
                            mpC.learned = false;
                        }
                        if (Rand.Chance(.3f))
                        {
                            this.AddPawnAbility(TorannMagicDefOf.TM_ChronostaticField);
                            this.MagicData.MagicPowersC.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ChronostaticField).learned = true;
                        }
                        else
                        {
                            MagicPower mpC = this.MagicData.MagicPowersC.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ChronostaticField);
                            mpC.learned = false;
                        }
                    }
                    else
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Prediction);
                        this.AddPawnAbility(TorannMagicDefOf.TM_AlterFate);
                        this.AddPawnAbility(TorannMagicDefOf.TM_AccelerateTime);
                        this.AddPawnAbility(TorannMagicDefOf.TM_ReverseTime);
                        this.AddPawnAbility(TorannMagicDefOf.TM_ChronostaticField);

                        if (!abilityUser.IsColonist)
                        {
                            if ((settingsRef.AIHardMode && Rand.Chance(hardModeMasterChance)) || Rand.Chance(masterChance))
                            {
                                this.AddPawnAbility(TorannMagicDefOf.TM_Recall);
                                this.spell_Recall = true;
                            }
                        }
                    }
                }
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.ChaosMage);
                if (flag2)
                {
                    foreach (MagicPower current in this.MagicData.AllMagicPowers)
                    {
                        if (current.abilityDef != TorannMagicDefOf.TM_ChaosTradition)
                        {
                            current.learned = false;
                        }
                    }
                    this.AddPawnAbility(TorannMagicDefOf.TM_ChaosTradition);

                    if (abilityUser.IsColonist)
                    {
                        TM_Calc.AssignChaosMagicPowers(this);
                    }
                    else
                    {
                        TM_Calc.AssignChaosMagicPowers(this, true);
                    }
                }
            }
        }

        public void InitializeSpell()
        {
            Pawn abilityUser = base.AbilityUser;
            if (this.IsMagicUser)
            {
                if (this.spell_Rain == true && !abilityUser.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Rainmaker);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Rainmaker);

                }
                if (this.spell_Blink == true && !abilityUser.story.traits.HasTrait(TorannMagicDefOf.Arcanist))
                {
                    if (!abilityUser.story.traits.HasTrait(TorannMagicDefOf.ChaosMage))
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_Blink);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Blink);
                    }
                    else
                    {
                        bool hasAbility = false;
                        for (int i = 0; i < this.chaosPowers.Count; i++)
                        {
                            if (this.chaosPowers[i].Ability == TorannMagicDefOf.TM_Blink || this.chaosPowers[i].Ability == TorannMagicDefOf.TM_Blink_I || this.chaosPowers[i].Ability == TorannMagicDefOf.TM_Blink_II || this.chaosPowers[i].Ability == TorannMagicDefOf.TM_Blink_III)
                            {
                                hasAbility = true;
                            }
                        }
                        if(!hasAbility)
                        {
                            this.RemovePawnAbility(TorannMagicDefOf.TM_Blink);
                            this.AddPawnAbility(TorannMagicDefOf.TM_Blink);
                        }
                    }
                }
                if (this.spell_Teleport == true && !abilityUser.story.traits.HasTrait(TorannMagicDefOf.Arcanist))
                {
                    if (!(abilityUser.story.traits.HasTrait(TorannMagicDefOf.ChaosMage) && this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Teleport).learned))
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_Teleport);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Teleport);
                    }
                }
                if (this.spell_Heal == true && !abilityUser.story.traits.HasTrait(TorannMagicDefOf.Paladin))
                {
                    if (!abilityUser.story.traits.HasTrait(TorannMagicDefOf.ChaosMage))
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_Heal);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Heal);
                    }
                    else
                    {
                        bool hasAbility = false;
                        for (int i = 0; i < this.chaosPowers.Count; i++)
                        {
                            if (this.chaosPowers[i].Ability == TorannMagicDefOf.TM_Heal)
                            {
                                hasAbility = true;
                            }
                        }
                        if (!hasAbility)
                        {
                            this.RemovePawnAbility(TorannMagicDefOf.TM_Heal);
                            this.AddPawnAbility(TorannMagicDefOf.TM_Heal);
                        }
                    }
                }
                if (this.spell_Heater == true)
                {
                    //if (this.summonedHeaters == null || (this.summonedHeaters != null && this.summonedHeaters.Count <= 0))
                    //{
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Heater);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Heater);
                    //}
                }
                if (this.spell_Cooler == true)
                {
                    //if(this.summonedCoolers == null || (this.summonedCoolers != null && this.summonedCoolers.Count <= 0))
                    //{
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Cooler);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Cooler);
                    //}
                }
                if (this.spell_PowerNode == true)
                {
                    //if (this.summonedPowerNodes == null || (this.summonedPowerNodes != null && this.summonedPowerNodes.Count <= 0))
                    //{
                    this.RemovePawnAbility(TorannMagicDefOf.TM_PowerNode);
                    this.AddPawnAbility(TorannMagicDefOf.TM_PowerNode);
                    //}
                }
                if (this.spell_Sunlight == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Sunlight);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Sunlight);

                }
                if (this.spell_DryGround == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_DryGround);
                    this.AddPawnAbility(TorannMagicDefOf.TM_DryGround);
                }
                if (this.spell_WetGround == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_WetGround);
                    this.AddPawnAbility(TorannMagicDefOf.TM_WetGround);
                }
                if (this.spell_ChargeBattery == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ChargeBattery);
                    this.AddPawnAbility(TorannMagicDefOf.TM_ChargeBattery);
                }
                if (this.spell_SmokeCloud == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_SmokeCloud);
                    this.AddPawnAbility(TorannMagicDefOf.TM_SmokeCloud);
                }
                if (this.spell_Extinguish == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Extinguish);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Extinguish);
                }
                if (this.spell_EMP == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_EMP);
                    this.AddPawnAbility(TorannMagicDefOf.TM_EMP);
                }
                if (this.spell_Blizzard == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Blizzard);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Blizzard);
                }
                if (this.spell_Firestorm == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Firestorm);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Firestorm);
                }
                if (this.spell_SummonMinion == true && !abilityUser.story.traits.HasTrait(TorannMagicDefOf.Summoner))
                {
                    if (!abilityUser.story.traits.HasTrait(TorannMagicDefOf.ChaosMage))
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_SummonMinion);
                        this.AddPawnAbility(TorannMagicDefOf.TM_SummonMinion);
                    }
                    else
                    {
                        bool hasAbility = false;
                        for (int i = 0; i < this.chaosPowers.Count; i++)
                        {
                            if (this.chaosPowers[i].Ability == TorannMagicDefOf.TM_SummonMinion)
                            {
                                hasAbility = true;
                            }
                        }
                        if (!hasAbility)
                        {
                            this.RemovePawnAbility(TorannMagicDefOf.TM_SummonMinion);
                            this.AddPawnAbility(TorannMagicDefOf.TM_SummonMinion);
                        }
                    }
                }
                if (this.spell_TransferMana == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_TransferMana);
                    this.AddPawnAbility(TorannMagicDefOf.TM_TransferMana);
                }
                if (this.spell_SiphonMana == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_SiphonMana);
                    this.AddPawnAbility(TorannMagicDefOf.TM_SiphonMana);
                }
                if (this.spell_RegrowLimb == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_RegrowLimb);
                    this.AddPawnAbility(TorannMagicDefOf.TM_RegrowLimb);
                }
                if (this.spell_EyeOfTheStorm == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_EyeOfTheStorm);
                    this.AddPawnAbility(TorannMagicDefOf.TM_EyeOfTheStorm);
                }
                if (this.spell_ManaShield == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ManaShield);
                    this.AddPawnAbility(TorannMagicDefOf.TM_ManaShield);
                }
                if (this.spell_Blur == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Blur);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Blur);
                }
                if (this.spell_FoldReality == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_FoldReality);
                    this.AddPawnAbility(TorannMagicDefOf.TM_FoldReality);
                }
                if (this.spell_Resurrection == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Resurrection);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Resurrection);
                }
                if (this.spell_HolyWrath == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_HolyWrath);
                    this.AddPawnAbility(TorannMagicDefOf.TM_HolyWrath);
                }
                if (this.spell_LichForm == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_LichForm);
                    this.AddPawnAbility(TorannMagicDefOf.TM_LichForm);
                }
                if (this.spell_Flight == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Flight);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Flight);
                }
                if (this.spell_SummonPoppi == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_SummonPoppi);
                    this.AddPawnAbility(TorannMagicDefOf.TM_SummonPoppi);
                }
                if (this.spell_BattleHymn == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_BattleHymn);
                    this.AddPawnAbility(TorannMagicDefOf.TM_BattleHymn);
                }
                if (this.spell_CauterizeWound == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_CauterizeWound);
                    this.AddPawnAbility(TorannMagicDefOf.TM_CauterizeWound);
                }
                if (this.spell_SpellMending == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_SpellMending);
                    this.AddPawnAbility(TorannMagicDefOf.TM_SpellMending);
                }
                if (this.spell_FertileLands == true)
                {
                    //if (this.fertileLands == null || (this.fertileLands != null && this.fertileLands.Count <= 0))
                    //{
                    this.RemovePawnAbility(TorannMagicDefOf.TM_FertileLands);
                    this.AddPawnAbility(TorannMagicDefOf.TM_FertileLands);
                    //}
                }
                if (this.spell_PsychicShock == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_PsychicShock);
                    this.AddPawnAbility(TorannMagicDefOf.TM_PsychicShock);
                }
                if (this.spell_Scorn == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Scorn);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Scorn);
                }
                if (this.spell_BlankMind == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_BlankMind);
                    this.AddPawnAbility(TorannMagicDefOf.TM_BlankMind);
                }
                if (this.spell_ShadowStep == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ShadowStep);
                    this.AddPawnAbility(TorannMagicDefOf.TM_ShadowStep);
                }
                if (this.spell_ShadowCall == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ShadowCall);
                    this.AddPawnAbility(TorannMagicDefOf.TM_ShadowCall);
                }
                if (this.spell_Teach == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_TeachMagic);
                    this.AddPawnAbility(TorannMagicDefOf.TM_TeachMagic);
                }
                if (this.spell_Meteor == true)
                {
                    MagicPower meteorPower = this.MagicData.MagicPowersG.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Meteor);
                    if (meteorPower == null)
                    {
                        meteorPower = this.MagicData.MagicPowersG.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Meteor_I);
                        if (meteorPower == null)
                        {
                            meteorPower = this.MagicData.MagicPowersG.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Meteor_II);
                            if (meteorPower == null)
                            {
                                meteorPower = this.MagicData.MagicPowersG.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Meteor_III);
                            }
                        }
                    }
                    if (meteorPower.level == 3)
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_Meteor_III);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Meteor_III);
                    }
                    else if (meteorPower.level == 2)
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_Meteor_II);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Meteor_II);
                    }
                    else if (meteorPower.level == 1)
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_Meteor_I);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Meteor_I);
                    }
                    else
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_Meteor);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Meteor);
                    }
                }
                if (this.spell_OrbitalStrike == true)
                {
                    MagicPower OrbitalStrikePower = this.MagicData.magicPowerT.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_OrbitalStrike);
                    if (OrbitalStrikePower == null)
                    {
                        OrbitalStrikePower = this.MagicData.magicPowerT.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_OrbitalStrike_I);
                        if (OrbitalStrikePower == null)
                        {
                            OrbitalStrikePower = this.MagicData.magicPowerT.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_OrbitalStrike_II);
                            if (OrbitalStrikePower == null)
                            {
                                OrbitalStrikePower = this.MagicData.magicPowerT.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_OrbitalStrike_III);
                            }
                        }
                    }
                    if (OrbitalStrikePower.level == 3)
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_OrbitalStrike_III);
                        this.AddPawnAbility(TorannMagicDefOf.TM_OrbitalStrike_III);
                    }
                    else if (OrbitalStrikePower.level == 2)
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_OrbitalStrike_II);
                        this.AddPawnAbility(TorannMagicDefOf.TM_OrbitalStrike_II);
                    }
                    else if (OrbitalStrikePower.level == 1)
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_OrbitalStrike_I);
                        this.AddPawnAbility(TorannMagicDefOf.TM_OrbitalStrike_I);
                    }
                    else
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_OrbitalStrike);
                        this.AddPawnAbility(TorannMagicDefOf.TM_OrbitalStrike);
                    }
                }
                if (this.spell_BloodMoon == true)
                {
                    MagicPower BloodMoonPower = this.MagicData.MagicPowersBM.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_BloodMoon);
                    if (BloodMoonPower == null)
                    {
                        BloodMoonPower = this.MagicData.MagicPowersBM.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_BloodMoon_I);
                        if (BloodMoonPower == null)
                        {
                            BloodMoonPower = this.MagicData.MagicPowersBM.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_BloodMoon_II);
                            if (BloodMoonPower == null)
                            {
                                BloodMoonPower = this.MagicData.MagicPowersBM.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_BloodMoon_III);
                            }
                        }
                    }
                    if (BloodMoonPower.level == 3)
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_BloodMoon_III);
                        this.AddPawnAbility(TorannMagicDefOf.TM_BloodMoon_III);
                    }
                    else if (BloodMoonPower.level == 2)
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_BloodMoon_II);
                        this.AddPawnAbility(TorannMagicDefOf.TM_BloodMoon_II);
                    }
                    else if (BloodMoonPower.level == 1)
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_BloodMoon_I);
                        this.AddPawnAbility(TorannMagicDefOf.TM_BloodMoon_I);
                    }
                    else
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_BloodMoon);
                        this.AddPawnAbility(TorannMagicDefOf.TM_BloodMoon);
                    }
                }
                if (this.spell_EnchantedAura == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_EnchantedAura);
                    this.AddPawnAbility(TorannMagicDefOf.TM_EnchantedAura);
                }
                if (this.spell_Shapeshift == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Shapeshift);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Shapeshift);
                }
                if (this.spell_ShapeshiftDW == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ShapeshiftDW);
                    this.AddPawnAbility(TorannMagicDefOf.TM_ShapeshiftDW);
                }
                if (this.spell_DirtDevil == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_DirtDevil);
                    this.AddPawnAbility(TorannMagicDefOf.TM_DirtDevil);
                }
                if (this.spell_MechaniteReprogramming == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_MechaniteReprogramming);
                    this.AddPawnAbility(TorannMagicDefOf.TM_MechaniteReprogramming);
                }
                if (this.spell_ArcaneBolt == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ArcaneBolt);
                    this.AddPawnAbility(TorannMagicDefOf.TM_ArcaneBolt);
                }
                if (this.spell_LightningTrap == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_LightningTrap);
                    this.AddPawnAbility(TorannMagicDefOf.TM_LightningTrap);
                }
                if (this.spell_Invisibility == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Invisibility);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Invisibility);
                }
                if (this.spell_BriarPatch == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_BriarPatch);
                    this.AddPawnAbility(TorannMagicDefOf.TM_BriarPatch);
                }
                if (this.spell_Recall == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_TimeMark);
                    this.AddPawnAbility(TorannMagicDefOf.TM_TimeMark);
                    if (this.recallSpell)
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_Recall);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Recall);
                    }
                }
                if(this.spell_MageLight == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_MageLight);
                    this.AddPawnAbility(TorannMagicDefOf.TM_MageLight);
                }
                //this.UpdateAbilities();
            }            
        }

        public void RemovePowers(bool clearStandalone = true)
        {
            Pawn abilityUser = base.AbilityUser;
            if (this.magicPowersInitialized == true)
            {
                bool flag2 = true;
                //flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.InnerFire);
                if (TM_Calc.IsWanderer(this.AbilityUser))
                {
                    this.spell_ArcaneBolt = false;
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ArcaneBolt);
                }
                if (abilityUser.story.traits.HasTrait(TorannMagicDefOf.ChaosMage))
                {
                    foreach (MagicPower current in this.MagicData.AllMagicPowersForChaosMage)
                    {
                        if (current.abilityDef != TorannMagicDefOf.TM_ChaosTradition)
                        {
                            current.learned = false;
                            this.RemovePawnAbility(current.abilityDef);
                        }                           
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_EnchantedAura);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_NanoStimulant);
                    this.spell_EnchantedAura = false;                    
                    this.spell_ShadowCall = false;
                    this.spell_ShadowStep = false;
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ShadowCall);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ShadowStep);

                }
                if (flag2)
                {
                    //Log.Message("Fixing Inner Fire Abilities");
                    foreach (MagicPower currentIF in this.MagicData.MagicPowersIF)
                    {
                        if (currentIF.abilityDef != TorannMagicDefOf.TM_Firestorm)
                        {
                            currentIF.learned = false;
                        }
                        this.RemovePawnAbility(currentIF.abilityDef);
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_RayofHope_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_RayofHope_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_RayofHope_III);

                }
                //flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost);
                if (flag2)
                {
                    //Log.Message("Fixing Heart of Frost Abilities");
                    foreach (MagicPower currentHoF in this.MagicData.MagicPowersHoF)
                    {
                        if (currentHoF.abilityDef != TorannMagicDefOf.TM_Blizzard)
                        {
                            currentHoF.learned = false;
                        }
                        this.RemovePawnAbility(currentHoF.abilityDef);                        
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Soothe_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Soothe_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Soothe_III);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_FrostRay_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_FrostRay_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_FrostRay_III);
                }
                //flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.StormBorn);
                if (flag2)
                {
                    //Log.Message("Fixing Storm Born Abilities");                   
                    foreach (MagicPower currentSB in this.MagicData.MagicPowersSB)
                    {
                        if (currentSB.abilityDef != TorannMagicDefOf.TM_EyeOfTheStorm)
                        {
                            currentSB.learned = false;
                        }
                        this.RemovePawnAbility(currentSB.abilityDef);                        
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_AMP_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_AMP_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_AMP_III);
                }
                //flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Arcanist);
                if (flag2)
                {
                    //Log.Message("Fixing Arcane Abilities");
                    foreach (MagicPower currentA in this.MagicData.MagicPowersA)
                    {
                        if (currentA.abilityDef != TorannMagicDefOf.TM_FoldReality)
                        {
                            currentA.learned = false;
                        }
                        this.RemovePawnAbility(currentA.abilityDef);                        
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Shadow_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Shadow_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Shadow_III);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_MagicMissile_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_MagicMissile_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_MagicMissile_III);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Blink_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Blink_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Blink_III);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Summon_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Summon_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Summon_III);

                }
                //flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Paladin);
                if (flag2)
                {
                    //Log.Message("Fixing Paladin Abilities");
                    foreach (MagicPower currentP in this.MagicData.MagicPowersP)
                    {
                        if (currentP.abilityDef != TorannMagicDefOf.TM_HolyWrath)
                        {
                            currentP.learned = false;
                        }
                        this.RemovePawnAbility(currentP.abilityDef);                        
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Shield_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Shield_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Shield_III);
                }
                //flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Summoner);
                if (flag2)
                {
                    foreach (MagicPower currentS in this.MagicData.MagicPowersS)
                    {
                        if (currentS.abilityDef != TorannMagicDefOf.TM_SummonPoppi)
                        {
                            currentS.learned = false;
                        }
                        this.RemovePawnAbility(currentS.abilityDef);
                    }
                }
                //flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Druid);
                if (flag2)
                {
                    foreach (MagicPower currentD in this.MagicData.MagicPowersD)
                    {
                        if (currentD.abilityDef != TorannMagicDefOf.TM_RegrowLimb)
                        {
                            currentD.learned = false;
                        }
                        this.RemovePawnAbility(currentD.abilityDef);
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_SootheAnimal_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_SootheAnimal_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_SootheAnimal_III);
                }
                //flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || abilityUser.story.traits.HasTrait(TorannMagicDefOf.Lich);
                if (flag2)
                {
                    foreach (MagicPower currentN in this.MagicData.MagicPowersN)
                    {
                        if (currentN.abilityDef != TorannMagicDefOf.TM_LichForm)
                        {
                            currentN.learned = false;
                        }
                        this.RemovePawnAbility(currentN.abilityDef);
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_DeathMark_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_DeathMark_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_DeathMark_III);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ConsumeCorpse_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ConsumeCorpse_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ConsumeCorpse_III);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_CorpseExplosion_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_CorpseExplosion_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_CorpseExplosion_III);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_DeathBolt_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_DeathBolt_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_DeathBolt_III);
                }
                //flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Priest);
                if (flag2)
                {
                    foreach (MagicPower currentPR in this.MagicData.MagicPowersPR)
                    {
                        if (currentPR.abilityDef != TorannMagicDefOf.TM_Resurrection)
                        {
                            currentPR.learned = false;
                        }
                        this.RemovePawnAbility(currentPR.abilityDef);
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_HealingCircle_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_HealingCircle_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_HealingCircle_III);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_BestowMight_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_BestowMight_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_BestowMight_III);
                }
                //flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.TM_Bard);
                if (flag2)
                {
                    foreach (MagicPower currentB in this.MagicData.MagicPowersB)
                    {
                        if (currentB.abilityDef != TorannMagicDefOf.TM_BattleHymn)
                        {
                            currentB.learned = false;
                        }
                        this.RemovePawnAbility(currentB.abilityDef);
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Lullaby_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Lullaby_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Lullaby_III);
                }
                //flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Succubus);
                if (flag2)
                {
                    foreach (MagicPower currentSD in this.MagicData.MagicPowersSD)
                    {
                        if (currentSD.abilityDef != TorannMagicDefOf.TM_Scorn)
                        {
                            currentSD.learned = false;
                        }
                        this.RemovePawnAbility(currentSD.abilityDef);
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ShadowBolt_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ShadowBolt_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ShadowBolt_III);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Attraction_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Attraction_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Attraction_III);
                }
                //flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Warlock);
                if (flag2)
                {
                    foreach (MagicPower currentWD in this.MagicData.MagicPowersWD)
                    {
                        if (currentWD.abilityDef != TorannMagicDefOf.TM_PsychicShock)
                        {
                            currentWD.learned = false;
                        }
                        this.RemovePawnAbility(currentWD.abilityDef);
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Repulsion_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Repulsion_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Repulsion_III);
                }
               // flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Geomancer);
                if (flag2)
                {
                    foreach (MagicPower currentG in this.MagicData.MagicPowersG)
                    {
                        if (currentG.abilityDef == TorannMagicDefOf.TM_Meteor || currentG.abilityDef == TorannMagicDefOf.TM_Meteor_I || currentG.abilityDef == TorannMagicDefOf.TM_Meteor_II || currentG.abilityDef == TorannMagicDefOf.TM_Meteor_III)
                        {
                            currentG.learned = true;
                        }
                        else
                        {
                            currentG.learned = false;
                        }
                        this.RemovePawnAbility(currentG.abilityDef);
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Encase_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Encase_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Encase_III);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Meteor_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Meteor_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Meteor_III);
                }
                //flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Technomancer);
                if (flag2)
                {
                    foreach (MagicPower currentT in this.MagicData.MagicPowersT)
                    {
                        if (currentT.abilityDef == TorannMagicDefOf.TM_OrbitalStrike || currentT.abilityDef == TorannMagicDefOf.TM_OrbitalStrike_I || currentT.abilityDef == TorannMagicDefOf.TM_OrbitalStrike_II || currentT.abilityDef == TorannMagicDefOf.TM_OrbitalStrike_III)
                        {
                            currentT.learned = true;
                        }
                        else
                        {
                            currentT.learned = false;
                        }
                        this.RemovePawnAbility(currentT.abilityDef);
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_OrbitalStrike_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_OrbitalStrike_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_OrbitalStrike_III);
                }
                //flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.BloodMage);
                if (flag2)
                {
                    foreach (MagicPower currentBM in this.MagicData.MagicPowersBM)
                    {
                        if (currentBM.abilityDef == TorannMagicDefOf.TM_BloodMoon || currentBM.abilityDef == TorannMagicDefOf.TM_BloodMoon_I || currentBM.abilityDef == TorannMagicDefOf.TM_BloodMoon_II || currentBM.abilityDef == TorannMagicDefOf.TM_BloodMoon_III)
                        {
                            currentBM.learned = true;
                        }
                        else
                        {
                            currentBM.learned = false;
                        }
                        this.RemovePawnAbility(currentBM.abilityDef);
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Rend_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Rend_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Rend_III);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_BloodMoon_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_BloodMoon_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_BloodMoon_III);
                }
                //flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Enchanter);
                if (flag2)
                {

                    foreach (MagicPower currentE in this.MagicData.MagicPowersE)
                    {
                        if (currentE.abilityDef != TorannMagicDefOf.TM_Shapeshift)
                        {
                            currentE.learned = false;
                        }
                        this.RemovePawnAbility(currentE.abilityDef);                       
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Polymorph_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Polymorph_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_Polymorph_III);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_EnchantedAura);
                }
                // flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Chronomancer);
                if (flag2)
                {
                    foreach (MagicPower currentC in this.MagicData.MagicPowersC)
                    {
                        if (currentC.abilityDef != TorannMagicDefOf.TM_Recall)
                        {
                            currentC.learned = false;
                        }
                        this.RemovePawnAbility(currentC.abilityDef);
                    }
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ChronostaticField_I);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ChronostaticField_II);
                    this.RemovePawnAbility(TorannMagicDefOf.TM_ChronostaticField_III);
                }
                if (clearStandalone)
                {
                    foreach (MagicPower currentStandalone in this.MagicData.MagicPowersStandalone)
                    {
                        this.RemovePawnAbility(currentStandalone.abilityDef);
                    }
                }                
            }
        }

        public override bool TryTransformPawn()
        {
            return this.IsMagicUser;
        }        

        public bool TryAddPawnAbility(TMAbilityDef ability)
        {
            //add check to verify no ability is already added
            bool result = false;
            base.AddPawnAbility(ability, true, -1f);
            result = true;
            return result;
        }

        private void ClearPower(MagicPower current)
        {
            Log.Message("Removing ability: " + current.abilityDef.defName.ToString());
            base.RemovePawnAbility(current.abilityDef);
            base.UpdateAbilities();
        }

        public void ResetSkills()
        {
            List<bool> powerLearned = new List<bool>();
            int skillpoints = 0;
            skillpoints += this.MagicData.MagicPowerSkill_global_regen.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_regen_pwr").level;
            skillpoints += this.MagicData.MagicPowerSkill_global_eff.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_eff_pwr").level;
            skillpoints += this.MagicData.MagicPowerSkill_global_spirit.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_spirit_pwr").level;
            if (TM_Calc.IsWanderer(this.Pawn))
            {
                skillpoints += this.MagicData.MagicPowerSkill_WandererCraft.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_WandererCraft_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_WandererCraft.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_WandererCraft_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_WandererCraft.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_WandererCraft_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Cantrips.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Cantrips_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Cantrips.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Cantrips_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Cantrips.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Cantrips_ver").level;
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.InnerFire))
            {
                skillpoints += this.MagicData.MagicPowerSkill_RayofHope.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_RayofHope_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Firebolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Firebolt_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Firebolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Firebolt_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Fireball.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Fireball_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Fireball.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Fireball_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Fireball.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Fireball_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Fireclaw.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Fireclaw_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Fireclaw.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Fireclaw_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Fireclaw.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Fireclaw_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Firestorm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Firestorm_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Firestorm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Firestorm_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Firestorm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Firestorm_ver").level;

                for (int i = 0; i < this.MagicData.MagicPowersIF.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersIF[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersIF[i].learned);
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
            {
                skillpoints += this.MagicData.MagicPowerSkill_Soothe.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Soothe_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Icebolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Icebolt_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Icebolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Icebolt_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Icebolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Icebolt_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Snowball.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Snowball_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Snowball.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Snowball_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Snowball.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Snowball_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Rainmaker.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Rainmaker_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_FrostRay.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_FrostRay_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Blizzard.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Blizzard_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Blizzard.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Blizzard_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Blizzard.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Blizzard_ver").level;
                for (int i = 0; i < this.MagicData.MagicPowersHoF.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersHoF[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersHoF[i].learned);
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.StormBorn))
            {
                skillpoints += this.MagicData.MagicPowerSkill_AMP.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_AMP_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_LightningBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_LightningBolt_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_LightningBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_LightningBolt_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_LightningBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_LightningBolt_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_LightningCloud.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_LightningCloud_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_LightningCloud.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_LightningCloud_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_LightningCloud.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_LightningCloud_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_LightningStorm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_LightningStorm_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_LightningStorm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_LightningStorm_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_LightningStorm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_LightningStorm_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_EyeOfTheStorm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EyeOfTheStorm_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_EyeOfTheStorm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EyeOfTheStorm_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_EyeOfTheStorm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EyeOfTheStorm_ver").level;
                for (int i = 0; i < this.MagicData.MagicPowersSB.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersSB[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersSB[i].learned);
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Arcanist))
            {
                skillpoints += this.MagicData.MagicPowerSkill_Shadow.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Shadow_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_MagicMissile.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_MagicMissile_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Blink.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Blink_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Summon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Summon_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Teleport.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Teleport_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Teleport.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Teleport_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Teleport.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Teleport_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_FoldReality.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_FoldReality_eff").level;
                for (int i = 0; i < this.MagicData.MagicPowersA.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersA[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersA[i].learned);
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Druid))
            {
                skillpoints += this.MagicData.MagicPowerSkill_SootheAnimal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SootheAnimal_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_SootheAnimal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SootheAnimal_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Regenerate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Regenerate_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Regenerate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Regenerate_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Regenerate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Regenerate_ver").level;                
                skillpoints += this.MagicData.MagicPowerSkill_Poison.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Poison_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Poison.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Poison_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Poison.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Poison_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_CureDisease.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_CureDisease_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_CureDisease.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_CureDisease_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_CureDisease.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_CureDisease_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_RegrowLimb.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_RegrowLimb_eff").level;
                for (int i = 0; i < this.MagicData.MagicPowersD.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersD[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersD[i].learned);
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Paladin))
            {
                skillpoints += this.MagicData.MagicPowerSkill_Heal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Heal_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Heal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Heal_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Heal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Heal_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Shield.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Shield_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_ValiantCharge.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ValiantCharge_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_ValiantCharge.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ValiantCharge_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_ValiantCharge.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ValiantCharge_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Overwhelm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Overwhelm_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Overwhelm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Overwhelm_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Overwhelm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Overwhelm_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_HolyWrath.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_HolyWrath_pwr").level * 2;
                skillpoints += this.MagicData.MagicPowerSkill_HolyWrath.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_HolyWrath_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_HolyWrath.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_HolyWrath_ver").level * 2;
                for (int i = 0; i < this.MagicData.MagicPowersP.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersP[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersP[i].learned);
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Lich))
            {
                skillpoints += this.MagicData.MagicPowerSkill_RaiseUndead.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_RaiseUndead_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_RaiseUndead.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_RaiseUndead_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_RaiseUndead.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_RaiseUndead_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_DeathMark.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_DeathMark_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_DeathMark.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_DeathMark_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_DeathMark.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_DeathMark_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_FogOfTorment.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_FogOfTorment_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_FogOfTorment.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_FogOfTorment_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_FogOfTorment.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_FogOfTorment_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_ConsumeCorpse.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ConsumeCorpse_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_ConsumeCorpse.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ConsumeCorpse_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_CorpseExplosion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_CorpseExplosion_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_CorpseExplosion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_CorpseExplosion_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_CorpseExplosion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_CorpseExplosion_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_DeathBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_DeathBolt_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_DeathBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_DeathBolt_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_DeathBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_DeathBolt_ver").level;

                for (int i = 0; i < this.MagicData.MagicPowersN.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersN[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersN[i].learned);
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Summoner))
            {
                skillpoints += this.MagicData.MagicPowerSkill_SummonMinion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonMinion_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_SummonMinion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonMinion_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_SummonMinion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonMinion_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_SummonPylon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonPylon_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_SummonPylon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonPylon_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_SummonPylon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonPylon_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_SummonExplosive.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonExplosive_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_SummonExplosive.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonExplosive_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_SummonExplosive.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonExplosive_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_SummonElemental.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonElemental_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_SummonElemental.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonElemental_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_SummonElemental.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonElemental_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_SummonPoppi.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonPoppi_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_SummonPoppi.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonPoppi_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_SummonPoppi.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonPoppi_ver").level;
                for (int i = 0; i < this.MagicData.MagicPowersS.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersS[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersS[i].learned);
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Priest))
            {
                skillpoints += this.MagicData.MagicPowerSkill_Resurrection.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Resurrection_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Resurrection.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Resurrection_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_AdvancedHeal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_AdvancedHeal_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_AdvancedHeal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_AdvancedHeal_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_AdvancedHeal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_AdvancedHeal_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Purify.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Purify_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Purify.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Purify_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Purify.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Purify_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_HealingCircle.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_HealingCircle_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_HealingCircle.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_HealingCircle_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_HealingCircle.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_HealingCircle_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_BestowMight.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BestowMight_eff").level;
                for (int i = 0; i < this.MagicData.MagicPowersPR.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersPR[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersPR[i].learned);
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Bard))
            {
                skillpoints += this.MagicData.MagicPowerSkill_BardTraining.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BardTraining_pwr").level * 2;
                skillpoints += this.MagicData.MagicPowerSkill_Entertain.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Entertain_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Entertain.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Entertain_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Inspire.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Inspire_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Inspire.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Inspire_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Lullaby.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Lullaby_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Lullaby.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Lullaby_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Lullaby.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Lullaby_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_BattleHymn.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BattleHymn_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_BattleHymn.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BattleHymn_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_BattleHymn.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BattleHymn_eff").level;
                for (int i = 0; i < this.MagicData.MagicPowersB.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersB[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersB[i].learned);
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus))
            {
                skillpoints += this.MagicData.MagicPowerSkill_SoulBond.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SoulBond_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_SoulBond.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SoulBond_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_SoulBond.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SoulBond_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_ShadowBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ShadowBolt_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_ShadowBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ShadowBolt_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_ShadowBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ShadowBolt_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Dominate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Dominate_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Dominate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Dominate_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Dominate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Dominate_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Attraction.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Attraction_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Attraction.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Attraction_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Attraction.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Attraction_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Scorn.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Scorn_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Scorn.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Scorn_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Scorn.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Scorn_eff").level;
                for (int i = 0; i < this.MagicData.MagicPowersSD.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersSD[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersSD[i].learned);
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Warlock))
            {
                skillpoints += this.MagicData.MagicPowerSkill_SoulBond.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SoulBond_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_SoulBond.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SoulBond_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_SoulBond.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SoulBond_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_ShadowBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ShadowBolt_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_ShadowBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ShadowBolt_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_ShadowBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ShadowBolt_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Dominate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Dominate_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Dominate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Dominate_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Dominate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Dominate_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Repulsion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Repulsion_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Repulsion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Repulsion_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Repulsion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Repulsion_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_PsychicShock.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_PsychicShock_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_PsychicShock.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_PsychicShock_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_PsychicShock.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_PsychicShock_eff").level;
                for (int i = 0; i < this.MagicData.MagicPowersWD.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersWD[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersWD[i].learned);
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Geomancer))
            {
                skillpoints += this.MagicData.MagicPowerSkill_Stoneskin.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Stoneskin_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Stoneskin.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Stoneskin_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Stoneskin.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Stoneskin_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Encase.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Encase_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Encase.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Encase_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Encase.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Encase_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_EarthSprites.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EarthSprites_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_EarthSprites.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EarthSprites_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_EarthSprites.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EarthSprites_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_EarthernHammer.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EarthernHammer_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_EarthernHammer.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EarthernHammer_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_EarthernHammer.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EarthernHammer_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Sentinel.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Sentinel_pwr").level * 2;
                skillpoints += this.MagicData.MagicPowerSkill_Sentinel.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Sentinel_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Sentinel.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Sentinel_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Meteor.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Meteor_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Meteor.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Meteor_eff").level;
                for (int i = 0; i < this.MagicData.MagicPowersG.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersG[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersG[i].learned);
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer))
            {
                skillpoints += this.MagicData.MagicPowerSkill_TechnoBit.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoBit_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_TechnoBit.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoBit_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_TechnoBit.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoBit_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_TechnoTurret.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoTurret_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_TechnoTurret.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoTurret_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_TechnoTurret.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoTurret_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_TechnoWeapon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoWeapon_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_TechnoWeapon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoWeapon_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_TechnoWeapon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoWeapon_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_TechnoShield.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoShield_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_TechnoShield.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoShield_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_TechnoShield.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoShield_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Sabotage.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Sabotage_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Sabotage.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Sabotage_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Sabotage.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Sabotage_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Overdrive.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Overdrive_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Overdrive.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Overdrive_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Overdrive.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Overdrive_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_OrbitalStrike.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_OrbitalStrike_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_OrbitalStrike.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_OrbitalStrike_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_OrbitalStrike.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_OrbitalStrike_eff").level;
                for (int i = 0; i < this.MagicData.MagicPowersT.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersT[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersT[i].learned);
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.BloodMage))
            {
                skillpoints += this.MagicData.MagicPowerSkill_BloodGift.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodGift_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_BloodGift.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodGift_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_BloodGift.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodGift_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_IgniteBlood.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_IgniteBlood_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_IgniteBlood.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_IgniteBlood_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_IgniteBlood.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_IgniteBlood_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_BloodForBlood.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodForBlood_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_BloodForBlood.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodForBlood_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_BloodForBlood.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodForBlood_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_BloodShield.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodShield_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_BloodShield.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodShield_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_BloodShield.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodShield_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Rend.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Rend_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Rend.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Rend_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Rend.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Rend_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_BloodMoon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodMoon_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_BloodMoon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodMoon_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_BloodMoon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodMoon_eff").level;
                for (int i = 0; i < this.MagicData.MagicPowersBM.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersBM[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersBM[i].learned);
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Enchanter))
            {
                skillpoints += this.MagicData.MagicPowerSkill_EnchantedBody.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EnchantedBody_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_EnchantedBody.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EnchantedBody_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_EnchantedBody.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EnchantedBody_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Transmutate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Transmutate_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Transmutate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Transmutate_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Transmutate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Transmutate_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_EnchanterStone.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EnchanterStone_ver").level * 2;
                skillpoints += this.MagicData.MagicPowerSkill_EnchanterStone.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EnchanterStone_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_EnchantWeapon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EnchantWeapon_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_EnchantWeapon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EnchantWeapon_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Polymorph.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Polymorph_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Polymorph.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Polymorph_ver").level * 2;
                skillpoints += this.MagicData.MagicPowerSkill_Polymorph.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Polymorph_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Shapeshift.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Shapeshift_pwr").level * 2;
                skillpoints += this.MagicData.MagicPowerSkill_Shapeshift.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Shapeshift_ver").level * 2;
                skillpoints += this.MagicData.MagicPowerSkill_Shapeshift.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Shapeshift_eff").level * 2;
                for (int i = 0; i < this.MagicData.MagicPowersE.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersE[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersE[i].learned);
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Chronomancer))
            {
                skillpoints += this.MagicData.MagicPowerSkill_Prediction.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Prediction_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Prediction.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Prediction_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Prediction.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Prediction_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_AlterFate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_AlterFate_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_AlterFate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_AlterFate_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_AccelerateTime.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_AccelerateTime_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_AccelerateTime.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_AccelerateTime_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_AccelerateTime.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_AccelerateTime_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_ReverseTime.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ReverseTime_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_ReverseTime.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ReverseTime_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_ReverseTime.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ReverseTime_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_ChronostaticField.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ChronostaticField_pwr").level * 2;
                skillpoints += this.MagicData.MagicPowerSkill_ChronostaticField.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ChronostaticField_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_ChronostaticField.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ChronostaticField_eff").level;
                skillpoints += this.MagicData.MagicPowerSkill_Recall.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Recall_pwr").level;
                skillpoints += this.MagicData.MagicPowerSkill_Recall.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Recall_ver").level;
                skillpoints += this.MagicData.MagicPowerSkill_Recall.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Recall_eff").level;
                for (int i = 0; i < this.MagicData.MagicPowersC.Count; i++)
                {
                    skillpoints += this.MagicData.MagicPowersC[i].level;
                    powerLearned.Add(this.MagicData.MagicPowersC[i].learned);
                }
            }

            int tmpLvl = this.MagicUserLevel;
            int tmpExp = this.MagicUserXP;
            int magicPts = this.magicData.MagicAbilityPoints;
            base.IsInitialized = false;
            this.magicData = null;
            this.CompTick();
            this.MagicUserLevel = tmpLvl;
            this.MagicUserXP = tmpExp;
            this.magicData.MagicAbilityPoints = magicPts + skillpoints;

            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.InnerFire))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersIF[i].learned = powerLearned[i];
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersHoF[i].learned = powerLearned[i];
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.StormBorn))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersSB[i].learned = powerLearned[i];
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Arcanist))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersA[i].learned = powerLearned[i];
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Summoner))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersS[i].learned = powerLearned[i];
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Druid))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersD[i].learned = powerLearned[i];
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Paladin))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersP[i].learned = powerLearned[i];
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersN[i].learned = powerLearned[i];
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Priest))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersPR[i].learned = powerLearned[i];
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Bard))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersB[i].learned = powerLearned[i];
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersSD[i].learned = powerLearned[i];
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Warlock))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersWD[i].learned = powerLearned[i];
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Geomancer))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersG[i].learned = powerLearned[i];
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersT[i].learned = powerLearned[i];
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.BloodMage))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersBM[i].learned = powerLearned[i];
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Enchanter))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersE[i].learned = powerLearned[i];
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Chronomancer))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersC[i].learned = powerLearned[i];
                }
            }
            if(TM_Calc.IsWanderer(this.Pawn))
            {
                for (int i = 0; i < powerLearned.Count; i++)
                {
                    this.MagicData.MagicPowersW[i].learned = powerLearned[i];
                }
            }
            if(this.Pawn.story.traits.HasTrait(TorannMagicDefOf.ChaosMage))
            {
                this.magicData.MagicAbilityPoints = this.MagicUserLevel;
            }
        }

        private void LoadPowers()
        {
            foreach (MagicPower currentA in this.MagicData.MagicPowersA)
            {
                //Log.Message("Removing ability: " + currentA.abilityDef.defName.ToString());
                currentA.level = 0;
                base.RemovePawnAbility(currentA.abilityDef);
            }
            foreach (MagicPower currentHoF in this.MagicData.MagicPowersHoF)
            {
                //Log.Message("Removing ability: " + currentHoF.abilityDef.defName.ToString());
                currentHoF.level = 0;
                base.RemovePawnAbility(currentHoF.abilityDef);
            }
            foreach (MagicPower currentSB in this.MagicData.MagicPowersSB)
            {
                //Log.Message("Removing ability: " + currentSB.abilityDef.defName.ToString());
                currentSB.level = 0;
                base.RemovePawnAbility(currentSB.abilityDef);
            }
            foreach (MagicPower currentIF in this.MagicData.MagicPowersIF)
            {
                //Log.Message("Removing ability: " + currentIF.abilityDef.defName.ToString());
                currentIF.level = 0;
                base.RemovePawnAbility(currentIF.abilityDef);
            }
            foreach (MagicPower currentP in this.MagicData.MagicPowersP)
            {
                //Log.Message("Removing ability: " + currentP.abilityDef.defName.ToString());
                currentP.level = 0;
                base.RemovePawnAbility(currentP.abilityDef);
            }
            foreach (MagicPower currentS in this.MagicData.MagicPowersS)
            {
                //Log.Message("Removing ability: " + currentP.abilityDef.defName.ToString());
                currentS.level = 0;
                base.RemovePawnAbility(currentS.abilityDef);
            }
            foreach (MagicPower currentD in this.MagicData.MagicPowersD)
            {
                //Log.Message("Removing ability: " + currentP.abilityDef.defName.ToString());
                currentD.level = 0;
                base.RemovePawnAbility(currentD.abilityDef);
            }
            foreach (MagicPower currentN in this.MagicData.MagicPowersN)
            {
                //Log.Message("Removing ability: " + currentP.abilityDef.defName.ToString());
                currentN.level = 0;
                base.RemovePawnAbility(currentN.abilityDef);
            }
        }

        public void RemoveTraits()
        {
            List<Trait> traits = this.AbilityUser.story.traits.allTraits;
            for (int i = 0; i < traits.Count; i++)
            {
                if (traits[i].def == TorannMagicDefOf.InnerFire || traits[i].def == TorannMagicDefOf.HeartOfFrost || traits[i].def == TorannMagicDefOf.StormBorn  || traits[i].def == TorannMagicDefOf.Arcanist || traits[i].def == TorannMagicDefOf.Paladin ||
                    traits[i].def == TorannMagicDefOf.Druid || traits[i].def == TorannMagicDefOf.Priest || traits[i].def == TorannMagicDefOf.Necromancer || traits[i].def == TorannMagicDefOf.Warlock || traits[i].def == TorannMagicDefOf.Succubus ||
                    traits[i].def == TorannMagicDefOf.TM_Bard || traits[i].def == TorannMagicDefOf.Geomancer || traits[i].def == TorannMagicDefOf.Technomancer || traits[i].def == TorannMagicDefOf.BloodMage || traits[i].def == TorannMagicDefOf.Enchanter ||
                    traits[i].def == TorannMagicDefOf.Chronomancer || traits[i].def == TorannMagicDefOf.ChaosMage || traits[i].def == TorannMagicDefOf.TM_Wanderer)
                {
                    Log.Message("Removing trait " + traits[i].Label);
                    traits.Remove(traits[i]);
                    i--;
                }
            }            
        }

        public void RemoveTMagicHediffs()
        {
            List<Hediff> allHediffs = this.Pawn.health.hediffSet.GetHediffs<Hediff>().ToList();
            for (int i = 0; i < allHediffs.Count(); i++)
            {
                Hediff hediff = allHediffs[i];
                if(hediff.def.defName.Contains("TM_"))
                {
                    this.Pawn.health.RemoveHediff(hediff);
                }

            }
        }

        public void RemoveAbilityUser()
        {            
            this.RemovePowers();
            this.RemoveTMagicHediffs();
            this.RemoveTraits();
            this.magicData = null;
            base.IsInitialized = false;            
        }

        public int MagicAttributeEffeciencyLevel(string attributeName)
        {
            int result = 0;

            if (attributeName == "TM_RayofHope_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_RayofHope.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Firebolt_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Firebolt.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Fireclaw_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Fireclaw.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Fireball_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Fireball.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Firestorm_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Firestorm.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }

            if (attributeName == "TM_Soothe_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Soothe.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Icebolt_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Icebolt.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_FrostRay_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_FrostRay.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Snowball_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Snowball.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Rainmaker_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Rainmaker.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Blizzard_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Blizzard.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }

            if (attributeName == "TM_AMP_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_AMP.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_LightningBolt_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_LightningBolt.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_LightningCloud_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_LightningCloud.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_LightningStorm_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_LightningStorm.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }

            if (attributeName == "TM_Shadow_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Shadow.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_MagicMissile_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_MagicMissile.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Blink_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Blink.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Summon_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Summon.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Teleport_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Teleport.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_FoldReality_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_FoldReality.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Heal_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Heal.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Shield_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Shield.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_ValiantCharge_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ValiantCharge.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Overwhelm_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Overwhelm.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_HolyWrath_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_HolyWrath.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_SummonMinion_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SummonMinion.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_SummonPylon_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SummonPylon.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_SummonExplosive_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SummonExplosive.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_SummonElemental_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SummonElemental.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_SummonPoppi_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SummonPoppi.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Poison_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Poison.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_SootheAnimal_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SootheAnimal.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Regenerate_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Regenerate.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_CureDisease_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_CureDisease.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_RegrowLimb_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_RegrowLimb.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_EyeOfTheStorm_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EyeOfTheStorm.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_RaiseUndead_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_RaiseUndead.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_DeathMark_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_DeathMark.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_FogOfTorment_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_FogOfTorment.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_ConsumeCorpse_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ConsumeCorpse.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_CorpseExplosion_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_CorpseExplosion.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_DeathBolt_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_DeathBolt.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_AdvancedHeal_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_AdvancedHeal.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Purify_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Purify.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_HealingCircle_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_HealingCircle.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_BestowMight_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BestowMight.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Resurrection_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Resurrection.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Lullaby_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Lullaby.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_BattleHymn_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BattleHymn.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_SoulBond_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SoulBond.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_ShadowBolt_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ShadowBolt.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Dominate_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Dominate.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Attraction_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Attraction.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Repulsion_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Repulsion.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Scorn_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Scorn.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_PsychicShock_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_PsychicShock.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Stoneskin_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Stoneskin.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Encase_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Encase.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_EarthSprites_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EarthSprites.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_EarthernHammer_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EarthernHammer.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Sentinel_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Sentinel.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Meteor_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Meteor.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_TechnoTurret_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_TechnoTurret.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_TechnoShield_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_TechnoShield.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Sabotage_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Sabotage.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Overdrive_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Overdrive.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_OrbitalStrike_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_OrbitalStrike.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_BloodGift_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BloodGift.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_IgniteBlood_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_IgniteBlood.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_BloodForBlood_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BloodForBlood.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_BloodShield_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BloodShield.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Rend_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Rend.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_BloodMoon_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BloodMoon.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_EnchantedBody_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EnchantedBody.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Transmutate_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Transmutate.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_EnchanterStone_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EnchanterStone.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_EnchantWeapon_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EnchantWeapon.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Polymorph_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Polymorph.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Shapeshift_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Shapeshift.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Prediction_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Prediction.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_AlterFate_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_AlterFate.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_AccelerateTime_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_AccelerateTime.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_ReverseTime_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ReverseTime.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_ChronostaticField_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ChronostaticField.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Recall_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Recall.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_WandererCraft_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_WandererCraft.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Cantrips_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Cantrips.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_ChaosTradition_eff")
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ChaosTradition.FirstOrDefault((MagicPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }

            return result;
        }

        public float ActualManaCost(TMAbilityDef magicDef)
        {
            float adjustedManaCost = magicDef.manaCost;
            if (magicDef == TorannMagicDefOf.TM_RayofHope || magicDef == TorannMagicDefOf.TM_RayofHope_I || magicDef == TorannMagicDefOf.TM_RayofHope_II || magicDef == TorannMagicDefOf.TM_RayofHope_III)
            {
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.IF_RayofHope_eff * (float)this.MagicAttributeEffeciencyLevel("TM_RayofHope_eff"));
            }
            if (magicDef == TorannMagicDefOf.TM_Firebolt)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Firebolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Firebolt_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.IF_Firebolt_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Fireclaw)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Fireclaw.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Fireclaw_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.IF_Fireclaw_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Fireball)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Fireball.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Fireball_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.IF_Fireball_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Firestorm)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Firestorm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Firestorm_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.IF_Firestorm_eff * (float)magicPowerSkill.level);
            }

            if (magicDef == TorannMagicDefOf.TM_Soothe || magicDef == TorannMagicDefOf.TM_Soothe_I || magicDef == TorannMagicDefOf.TM_Soothe_II || magicDef == TorannMagicDefOf.TM_Soothe_III)
            {
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.HoF_Soothe_eff * (float)this.MagicAttributeEffeciencyLevel("TM_Soothe_eff"));
            }
            if (magicDef == TorannMagicDefOf.TM_Icebolt)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Icebolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Icebolt_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.HoF_Icebolt_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_FrostRay || magicDef == TorannMagicDefOf.TM_FrostRay_I || magicDef == TorannMagicDefOf.TM_FrostRay_II || magicDef == TorannMagicDefOf.TM_FrostRay_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_FrostRay.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_FrostRay_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.HoF_FrostRay_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Snowball)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Snowball.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Snowball_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.HoF_Snowball_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Rainmaker)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Rainmaker.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Rainmaker_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.HoF_Rainmaker_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Blizzard)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Blizzard.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Blizzard_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.HoF_Blizzard_eff * (float)magicPowerSkill.level);
            }

            if (magicDef == TorannMagicDefOf.TM_AMP || magicDef == TorannMagicDefOf.TM_AMP_I || magicDef == TorannMagicDefOf.TM_AMP_II || magicDef == TorannMagicDefOf.TM_AMP_III)
            {
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.SB_AMP_eff * (float)this.MagicAttributeEffeciencyLevel("TM_AMP_eff"));
            }
            if (magicDef == TorannMagicDefOf.TM_LightningBolt)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_LightningBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_LightningBolt_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.SB_LightningBolt_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_LightningCloud)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_LightningCloud.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_LightningCloud_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.SB_LightningCloud_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_LightningStorm)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_LightningStorm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_LightningStorm_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.SB_LightningStorm_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_EyeOfTheStorm)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EyeOfTheStorm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EyeOfTheStorm_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.SB_EyeOfTheStorm_eff * (float)magicPowerSkill.level);
            }

            if (magicDef == TorannMagicDefOf.TM_Shadow || magicDef == TorannMagicDefOf.TM_Shadow_I || magicDef == TorannMagicDefOf.TM_Shadow_II || magicDef == TorannMagicDefOf.TM_Shadow_III)
            {
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.A_Shadow_eff * (float)this.MagicAttributeEffeciencyLevel("TM_Shadow_eff"));
            }
            if (magicDef == TorannMagicDefOf.TM_MagicMissile || magicDef == TorannMagicDefOf.TM_MagicMissile_I || magicDef == TorannMagicDefOf.TM_MagicMissile_II || magicDef == TorannMagicDefOf.TM_MagicMissile_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_MagicMissile.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_MagicMissile_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.A_MagicMissile_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Blink || magicDef == TorannMagicDefOf.TM_Blink_I || magicDef == TorannMagicDefOf.TM_Blink_II || magicDef == TorannMagicDefOf.TM_Blink_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Blink.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Blink_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.A_Blink_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Summon || magicDef == TorannMagicDefOf.TM_Summon_I || magicDef == TorannMagicDefOf.TM_Summon_II || magicDef == TorannMagicDefOf.TM_Summon_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Summon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Summon_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.A_Summon_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Teleport)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Teleport.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Teleport_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.A_Teleport_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_FoldReality)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_FoldReality.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_FoldReality_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.A_FoldReality_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Heal)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Heal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Heal_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.P_Heal_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Shield || magicDef == TorannMagicDefOf.TM_Shield_I || magicDef == TorannMagicDefOf.TM_Shield_II || magicDef == TorannMagicDefOf.TM_Shield_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Shield.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Shield_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.P_Shield_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_ValiantCharge)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ValiantCharge.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ValiantCharge_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.P_ValiantCharge_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Overwhelm)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Overwhelm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Overwhelm_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.P_Overwhelm_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_HolyWrath)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_HolyWrath.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_HolyWrath_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.P_HolyWrath_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_SummonMinion)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SummonMinion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonMinion_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.S_SummonMinion_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_SummonPylon )
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SummonPylon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonPylon_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.S_SummonPylon_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_SummonExplosive)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SummonExplosive.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonExplosive_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.S_SummonExplosive_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_SummonElemental)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SummonElemental.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonElemental_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.S_SummonElemental_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_SummonPoppi)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SummonPoppi.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonPoppi_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.S_SummonPoppi_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Poison)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Poison.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Poison_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.D_Poison_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_SootheAnimal || magicDef == TorannMagicDefOf.TM_SootheAnimal_I || magicDef == TorannMagicDefOf.TM_SootheAnimal_II || magicDef == TorannMagicDefOf.TM_SootheAnimal_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SootheAnimal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SootheAnimal_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.D_SootheAnimal_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Regenerate)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Regenerate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Regenerate_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.D_Regenerate_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_CureDisease)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_CureDisease.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_CureDisease_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.D_CureDisease_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_RegrowLimb)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_RegrowLimb.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_RegrowLimb_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.D_RegrowLimb_eff * (float)magicPowerSkill.level);
            }            
            if (magicDef == TorannMagicDefOf.TM_RaiseUndead)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_RaiseUndead.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_RaiseUndead_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.N_RaiseUndead_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_DeathMark || magicDef == TorannMagicDefOf.TM_DeathMark_I || magicDef == TorannMagicDefOf.TM_DeathMark_II || magicDef == TorannMagicDefOf.TM_DeathMark_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_DeathMark.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_DeathMark_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.N_DeathMark_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_FogOfTorment)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_FogOfTorment.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_FogOfTorment_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.N_FogOfTorment_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_ConsumeCorpse || magicDef == TorannMagicDefOf.TM_ConsumeCorpse_I || magicDef == TorannMagicDefOf.TM_ConsumeCorpse_II || magicDef == TorannMagicDefOf.TM_ConsumeCorpse_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ConsumeCorpse.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ConsumeCorpse_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.N_ConsumeCorpse_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_CorpseExplosion || magicDef == TorannMagicDefOf.TM_CorpseExplosion_I || magicDef == TorannMagicDefOf.TM_CorpseExplosion_II || magicDef == TorannMagicDefOf.TM_CorpseExplosion_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_CorpseExplosion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_CorpseExplosion_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.N_CorpseExplosion_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_DeathBolt || magicDef == TorannMagicDefOf.TM_DeathBolt_I || magicDef == TorannMagicDefOf.TM_DeathBolt_II || magicDef == TorannMagicDefOf.TM_DeathBolt_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_DeathBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_DeathBolt_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.N_DeathBolt_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_AdvancedHeal)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_AdvancedHeal.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_AdvancedHeal_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.PR_AdvancedHeal_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Purify)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Purify.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Purify_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.PR_Purify_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_HealingCircle || magicDef == TorannMagicDefOf.TM_HealingCircle_I || magicDef == TorannMagicDefOf.TM_HealingCircle_II || magicDef == TorannMagicDefOf.TM_HealingCircle_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_HealingCircle.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_HealingCircle_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.PR_HealingCircle_eff * (float)magicPowerSkill.level);
            }            
            if (magicDef == TorannMagicDefOf.TM_BestowMight || magicDef == TorannMagicDefOf.TM_BestowMight_I || magicDef == TorannMagicDefOf.TM_BestowMight_II || magicDef == TorannMagicDefOf.TM_BestowMight_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BestowMight.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BestowMight_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.PR_BestowMight_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Resurrection)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Resurrection.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Resurrection_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.PR_Resurrection_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Lullaby || magicDef == TorannMagicDefOf.TM_Lullaby_I || magicDef == TorannMagicDefOf.TM_Lullaby_II || magicDef == TorannMagicDefOf.TM_Lullaby_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Lullaby.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Lullaby_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.B_Lullaby_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_BattleHymn)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BattleHymn.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BattleHymn_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.B_BattleHymn_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_SoulBond)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SoulBond.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SoulBond_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.SoulBond_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_ShadowStep)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SoulBond.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SoulBond_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.SoulBond_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_ShadowCall)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_SoulBond.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SoulBond_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.SoulBond_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_ShadowBolt || magicDef == TorannMagicDefOf.TM_ShadowBolt_I || magicDef == TorannMagicDefOf.TM_ShadowBolt_II || magicDef == TorannMagicDefOf.TM_ShadowBolt_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ShadowBolt.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ShadowBolt_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.ShadowBolt_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Dominate)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Dominate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Dominate_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.Dominate_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Attraction || magicDef == TorannMagicDefOf.TM_Attraction_I || magicDef == TorannMagicDefOf.TM_Attraction_II || magicDef == TorannMagicDefOf.TM_Attraction_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Attraction.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Attraction_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.SD_Attraction_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Scorn)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Scorn.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Scorn_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.SD_Scorn_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Repulsion || magicDef == TorannMagicDefOf.TM_Repulsion_I || magicDef == TorannMagicDefOf.TM_Repulsion_II || magicDef == TorannMagicDefOf.TM_Repulsion_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Repulsion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Repulsion_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.WD_Repulsion_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_PsychicShock)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_PsychicShock.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_PsychicShock_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.WD_PsychicShock_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Encase || magicDef == TorannMagicDefOf.TM_Encase_I || magicDef == TorannMagicDefOf.TM_Encase_II || magicDef == TorannMagicDefOf.TM_Encase_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Encase.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Encase_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.G_Encase_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_EarthSprites)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EarthSprites.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EarthSprites_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.G_EarthSprites_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_EarthernHammer)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EarthernHammer.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EarthernHammer_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.G_EarthernHammer_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Meteor || magicDef == TorannMagicDefOf.TM_Meteor_I || magicDef == TorannMagicDefOf.TM_Meteor_II || magicDef == TorannMagicDefOf.TM_Meteor_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Meteor.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Meteor_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.G_Meteor_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_TechnoTurret)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_TechnoTurret.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoTurret_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.T_TechnoTurret_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_TechnoShield)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_TechnoShield.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoShield_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.T_TechnoShield_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Sabotage)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Sabotage.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Sabotage_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.T_Sabotage_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Overdrive)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Overdrive.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Overdrive_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.T_Overdrive_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_OrbitalStrike || magicDef == TorannMagicDefOf.TM_OrbitalStrike_I || magicDef == TorannMagicDefOf.TM_OrbitalStrike_II || magicDef == TorannMagicDefOf.TM_OrbitalStrike_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_OrbitalStrike.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_OrbitalStrike_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.T_OrbitalStrike_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_BloodGift)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BloodGift.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodGift_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.BM_BloodGift_eff * (float)magicPowerSkill.level);
            }
            //if (magicDef == TorannMagicDefOf.TM_BloodForBlood)
            //{
            //    MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BloodForBlood.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodForBlood_eff");
            //    adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.BM_BloodForBlood_eff * (float)magicPowerSkill.level);
            //}
            //if (magicDef == TorannMagicDefOf.TM_BloodShield)
            //{
            //    MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BloodShield.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodShield_eff");
            //    adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.BM_BloodShield_eff * (float)magicPowerSkill.level);
            //}
            //if (magicDef == TorannMagicDefOf.TM_Rend || magicDef == TorannMagicDefOf.TM_Rend_I || magicDef == TorannMagicDefOf.TM_Rend_II || magicDef == TorannMagicDefOf.TM_Rend_III)
            //{
            //    MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Rend.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Rend_eff");
            //    adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.BM_Rend_eff * (float)magicPowerSkill.level);
            //}
            //if (magicDef == TorannMagicDefOf.TM_BloodMoon || magicDef == TorannMagicDefOf.TM_BloodMoon_I || magicDef == TorannMagicDefOf.TM_BloodMoon_II || magicDef == TorannMagicDefOf.TM_BloodMoon_III)
            //{
            //    MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_BloodMoon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodMoon_eff");
            //    adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.BM_BloodMoon_eff * (float)magicPowerSkill.level);
            //}
            if (magicDef == TorannMagicDefOf.TM_EnchantedBody || magicDef == TorannMagicDefOf.TM_EnchantedAura)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EnchantedBody.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EnchantedBody_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.E_EnchantedBody_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Transmutate)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Transmutate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Transmutate_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.E_Transmutate_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_EnchantWeapon)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EnchantWeapon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EnchantWeapon_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.E_EnchantWeapon_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_EnchanterStone)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EnchanterStone.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EnchanterStone_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.E_EnchanterStone_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Polymorph || magicDef == TorannMagicDefOf.TM_Polymorph_I || magicDef == TorannMagicDefOf.TM_Polymorph_II || magicDef == TorannMagicDefOf.TM_Polymorph_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Polymorph.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Polymorph_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.E_Polymorph_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_Shapeshift || magicDef == TorannMagicDefOf.TM_ShapeshiftDW)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_Shapeshift.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Shapeshift_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.E_Shapeshift_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_AlterFate)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_AlterFate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_AlterFate_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.C_AlterFate_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_AccelerateTime)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_AccelerateTime.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_AccelerateTime_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.C_AccelerateTime_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_ReverseTime)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ReverseTime.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ReverseTime_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.C_ReverseTime_eff * (float)magicPowerSkill.level);
            }
            if (magicDef == TorannMagicDefOf.TM_ChronostaticField || magicDef == TorannMagicDefOf.TM_ChronostaticField_I || magicDef == TorannMagicDefOf.TM_ChronostaticField_II || magicDef == TorannMagicDefOf.TM_ChronostaticField_III)
            {
                MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_ChronostaticField.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ChronostaticField_eff");
                adjustedManaCost = magicDef.manaCost - magicDef.manaCost * (this.C_ChronostaticField_eff * (float)magicPowerSkill.level);
            }
            if (this.Pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_SyrriumSenseHD"), false))
            {
                adjustedManaCost = adjustedManaCost * .9f;
            }
            if (this.mpCost != 1f)
            {
                adjustedManaCost = adjustedManaCost * this.mpCost;
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
            {
                adjustedManaCost = 0;
            }
            if(this.Pawn.story.traits.HasTrait(TorannMagicDefOf.ChaosMage))
            {
                adjustedManaCost = adjustedManaCost * 1.2f;
            }
            return Mathf.Max(adjustedManaCost, (.5f * magicDef.manaCost));           
        }

        public override List<HediffDef> IgnoredHediffs()
        {
            return new List<HediffDef>
            {
                TorannMagicDefOf.TM_MightUserHD
            };
        }

        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            Pawn abilityUser = base.AbilityUser;

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
                    if (current.def == TorannMagicDefOf.TM_HediffInvulnerable) 
                    {
                        absorbed = true;
                        MoteMaker.MakeStaticMote(AbilityUser.Position, AbilityUser.Map, ThingDefOf.Mote_ExplosionFlash, 10);
                        dinfo.SetAmount(0);
                        return;
                    }
                    if (current.def == TorannMagicDefOf.TM_LichHD && this.damageMitigationDelay < this.age)
                    {
                        absorbed = true;
                        int mitigationAmt = 4;
                        int actualDmg;
                        int dmgAmt = Mathf.RoundToInt(dinfo.Amount);
                        if (dmgAmt < mitigationAmt)
                        {
                            MoteMaker.ThrowText(this.Pawn.DrawPos, this.Pawn.Map, "TM_DamageAbsorbedAll".Translate(), -1);
                            actualDmg = 0;
                            return;
                        }
                        else
                        {
                            MoteMaker.ThrowText(this.Pawn.DrawPos, this.Pawn.Map, "TM_DamageAbsorbed".Translate(
                                dmgAmt,
                                mitigationAmt
                            ), -1);
                            actualDmg = dmgAmt - mitigationAmt;
                        }
                        this.damageMitigationDelay = this.age + 6;
                        dinfo.SetAmount(actualDmg);
                        abilityUser.TakeDamage(dinfo);
                        return;
                    }
                    if (current.def == TorannMagicDefOf.TM_HediffEnchantment_phantomShift && Rand.Chance(.2f))
                    {
                        absorbed = true;
                        MoteMaker.MakeStaticMote(AbilityUser.Position, AbilityUser.Map, ThingDefOf.Mote_ExplosionFlash, 8);
                        MoteMaker.ThrowSmoke(abilityUser.Position.ToVector3Shifted(), abilityUser.Map, 1.2f);
                        dinfo.SetAmount(0);
                        return;
                    }
                    if (arcaneRes !=0 && resMitigationDelay < this.age)
                    {
                        if (current.def == TorannMagicDefOf.TM_HediffEnchantment_arcaneRes)
                        {
                            if (dinfo.Def.defName.Contains("TM_") || dinfo.Def.defName == "FrostRay" || dinfo.Def.defName == "Snowball" || dinfo.Def.defName == "Iceshard" || dinfo.Def.defName == "Firebolt")
                            {
                                absorbed = true;
                                int actualDmg = Mathf.RoundToInt(dinfo.Amount - (dinfo.Amount * arcaneRes));
                                resMitigationDelay = this.age + 10;
                                dinfo.SetAmount(actualDmg);
                                abilityUser.TakeDamage(dinfo);
                                return;
                            }
                        }
                    }
                    if (current.def == TorannMagicDefOf.TM_HediffShield)
                    {
                        float sev = current.Severity;
                        absorbed = true;
                        int actualDmg = 0;
                        float dmgAmt = (float)dinfo.Amount;
                        float dmgToSev = 0.004f;
                        ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                        if (!abilityUser.IsColonist)
                        {
                            if (settingsRef.AIHardMode)
                            {
                                dmgToSev = 0.0025f;
                            }
                            else
                            {
                                dmgToSev = 0.003f;
                            }
                        }
                        sev = sev - (dmgAmt * dmgToSev);
                        if (sev < 0)
                        {
                            actualDmg = (int)Mathf.RoundToInt(Mathf.Abs(sev / dmgToSev));
                            BreakShield(abilityUser);
                        }
                        TM_Action.DisplayShieldHit(abilityUser, dinfo);
                        current.Severity = sev;
                        dinfo.SetAmount(actualDmg);
                
                        return;
                    }
                    if (current.def == TorannMagicDefOf.TM_DemonScornHD || current.def == TorannMagicDefOf.TM_DemonScornHD_I || current.def == TorannMagicDefOf.TM_DemonScornHD_II || current.def == TorannMagicDefOf.TM_DemonScornHD_III)
                    {
                        float sev = current.Severity;
                        absorbed = true;
                        int actualDmg = 0;
                        float dmgAmt = (float)dinfo.Amount;
                        float dmgToSev = 1f;
                        ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                        if (!abilityUser.IsColonist)
                        {
                            if (settingsRef.AIHardMode)
                            {
                                dmgToSev = 0.8f;
                            }
                            else
                            {
                                dmgToSev = 1f;
                            }
                        }
                        sev = sev - (dmgAmt * dmgToSev);
                        if (sev < 0)
                        {
                            actualDmg = (int)Mathf.RoundToInt(Mathf.Abs(sev / dmgToSev));
                            BreakShield(abilityUser);
                        }
                        TM_Action.DisplayShieldHit(abilityUser, dinfo);
                        current.Severity = sev;
                        dinfo.SetAmount(actualDmg);

                        return;
                    }
                    if (current.def == TorannMagicDefOf.TM_ManaShieldHD && this.damageMitigationDelay < this.age)
                    {
                        float sev = this.Mana.CurLevel;
                        absorbed = true;
                        int actualDmg = 0;
                        float dmgAmt = (float)dinfo.Amount;
                        float dmgToSev = 0.02f;
                        float maxDmg = 11f;
                        if (this.MagicData.MagicPowerSkill_Cantrips.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Cantrips_ver").level >= 3)
                        {
                            dmgToSev = 0.015f;
                            maxDmg = 14f;
                            if (this.MagicData.MagicPowerSkill_Cantrips.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Cantrips_ver").level >= 7)
                            {
                                dmgToSev = 0.012f;
                                maxDmg = 17f;
                            }
                        }
                        if (dmgAmt >= maxDmg)
                        {
                            actualDmg = Mathf.RoundToInt(dmgAmt - maxDmg);
                            sev = sev - (maxDmg * dmgToSev);                            
                        }
                        else
                        {
                            sev = sev - (dmgAmt * dmgToSev);
                        }
                        this.Mana.CurLevel = sev;
                        if (sev < 0)
                        {
                            actualDmg = (int)Mathf.RoundToInt(Mathf.Abs(sev / dmgToSev));
                            BreakShield(abilityUser);
                            current.Severity = sev;
                            abilityUser.health.RemoveHediff(current);
                        }
                        TM_Action.DisplayShieldHit(abilityUser, dinfo);
                        this.damageMitigationDelay = this.age + 2;
                        dinfo.SetAmount(actualDmg);
                        abilityUser.TakeDamage(dinfo);
                        return;
                    }

                }
                
                list.Clear();
                list = null;
            }
            base.PostPreApplyDamage(dinfo, out absorbed);
        }

        private void BreakShield(Pawn pawn)
        {
            SoundDefOf.EnergyShield_Broken.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
            MoteMaker.MakeStaticMote(pawn.TrueCenter(), pawn.Map, ThingDefOf.Mote_ExplosionFlash, 12f);
            for (int i = 0; i < 6; i++)
            {
                Vector3 loc = pawn.TrueCenter() + Vector3Utility.HorizontalVectorFromAngle((float)Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f);
                MoteMaker.ThrowDustPuff(loc, pawn.Map, Rand.Range(0.8f, 1.2f));
            }
        }

        public void DoArcaneForging()
        {
            if (this.Pawn.CurJob.targetA.Thing.def.defName == "TableArcaneForge")
            {
                this.ArcaneForging = true;
                Job job = this.Pawn.CurJob;
                Thing forge = this.Pawn.CurJob.targetA.Thing;
                if (this.Pawn.Position == forge.InteractionCell && this.Pawn.jobs.curDriver.CurToilIndex >= 10)
                {
                    if (Find.TickManager.TicksGame % 20 == 0)
                    {
                        if (this.Mana.CurLevel >= .1f)
                        {
                            this.Mana.CurLevel -= .025f;
                            this.MagicUserXP += 4;
                            MoteMaker.ThrowSmoke(forge.DrawPos, forge.Map, Rand.Range(.8f, 1.2f));
                        }
                        else
                        {
                            this.Pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true);
                        }
                    }

                    ThingDef mote = null;
                    int rnd = Rand.RangeInclusive(0, 3);
                    switch (rnd)
                    {
                        case 0:
                            mote = ThingDef.Named("Mote_ArcaneFabricationA");
                            break;
                        case 1:
                            mote = ThingDef.Named("Mote_ArcaneFabricationB");
                            break;
                        case 2:
                            mote = ThingDef.Named("Mote_ArcaneFabricationC");
                            break;
                        case 3:
                            mote = ThingDef.Named("Mote_ArcaneFabricationD");
                            break;
                    }
                    Vector3 drawPos = forge.DrawPos;
                    drawPos.x += Rand.Range(-.05f, .05f);
                    drawPos.z += Rand.Range(-.05f, .05f);
                    TM_MoteMaker.ThrowGenericMote(mote, drawPos, forge.Map, Rand.Range(.25f, .4f), .02f, 0f, .01f, Rand.Range(-200, 200), 0, 0, forge.Rotation.AsAngle);
                }
            }
            else
            {
                this.ArcaneForging = false;
            }
        }

        public void ResolveAutoCast()
        {
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            bool flagCM = this.Pawn.story.traits.HasTrait(TorannMagicDefOf.ChaosMage);
            //bool flagCP = this.Pawn.story.traits.HasTrait(TorannMagicDefOf.ChaosMage) || this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless);
            //CompAbilityUserMight compMight = null;
            //if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
            //{
            //    compMight = this.Pawn.TryGetComp<CompAbilityUserMight>();
            //}
            if (settingsRef.autocastEnabled && this.Pawn.jobs != null && this.Pawn.CurJob != null && this.Pawn.CurJob.def != TorannMagicDefOf.TMCastAbilityVerb && this.Pawn.CurJob.def != JobDefOf.Ingest && this.Pawn.GetPosture() == PawnPosture.Standing)
            {
                //Log.Message("pawn " + this.Pawn.LabelShort + " current job is " + this.Pawn.CurJob.def.defName);
                //non-combat (undrafted) spells
                bool castSuccess = false;
                if (this.Pawn.drafter != null && !this.Pawn.Drafted && this.Mana != null && this.Mana.CurLevelPercentage >= settingsRef.autocastMinThreshold)
                {
                    if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Summoner) || flagCM)
                    {
                        MagicPower magicPower = this.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonMinion);
                        if (magicPower != null && magicPower.autocast && magicPower.learned && !this.Pawn.CurJob.playerForced && this.summonedMinions.Count() < 4)
                        {
                            PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_SummonMinion);
                            AutoCast.CastOnSelf.Evaluate(this, TorannMagicDefOf.TM_SummonMinion, ability, magicPower, out castSuccess);
                            if (castSuccess) goto AutoCastExit;
                        }
                    }
                    if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Chronomancer) && this.spell_Recall && !this.recallSet)
                    {
                        MagicPower magicPower = this.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TimeMark);
                        if (magicPower != null && magicPower.autocast && magicPower.learned && !this.Pawn.CurJob.playerForced)
                        {
                            PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_TimeMark);
                            AutoCast.CastOnSelf.Evaluate(this, TorannMagicDefOf.TM_TimeMark, ability, magicPower, out castSuccess);
                            if (castSuccess) goto AutoCastExit;
                        }
                    }
                    if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Arcanist) || flagCM)
                    {
                        PawnAbility ability = null;
                        foreach (MagicPower current in this.MagicData.MagicPowersA)
                        {
                            if (current != null && current.abilityDef != null)
                            {                                
                                if ((current.abilityDef == TorannMagicDefOf.TM_Summon || current.abilityDef == TorannMagicDefOf.TM_Summon_I || current.abilityDef == TorannMagicDefOf.TM_Summon_II || current.abilityDef == TorannMagicDefOf.TM_Summon_III) && !this.Pawn.CurJob.playerForced)
                                {
                                    //Log.Message("evaluating " + current.abilityDef.defName);
                                    if (current.level == 0)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Summon);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Summon);
                                            float minDistance = ActualManaCost(TorannMagicDefOf.TM_Summon) * 150;
                                            AutoCast.Summon.Evaluate(this, TorannMagicDefOf.TM_Summon, ability, magicPower, minDistance, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else if (current.level == 1)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Summon);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Summon_I);
                                            float minDistance = ActualManaCost(TorannMagicDefOf.TM_Summon_I) * 150;
                                            AutoCast.Summon.Evaluate(this, TorannMagicDefOf.TM_Summon_I, ability, magicPower, minDistance, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }

                                    }
                                    else if (current.level == 2)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Summon_I);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Summon_II);
                                            float minDistance = ActualManaCost(TorannMagicDefOf.TM_Summon_II) * 150;
                                            AutoCast.Summon.Evaluate(this, TorannMagicDefOf.TM_Summon_II, ability, magicPower, minDistance, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Summon_II);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Summon_III);
                                            float minDistance = ActualManaCost(TorannMagicDefOf.TM_Summon_III) * 150;
                                            AutoCast.Summon.Evaluate(this, TorannMagicDefOf.TM_Summon_III, ability, magicPower, minDistance, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                }                                
                                if ((current.abilityDef == TorannMagicDefOf.TM_Blink || current.abilityDef == TorannMagicDefOf.TM_Blink_I || current.abilityDef == TorannMagicDefOf.TM_Blink_II || current.abilityDef == TorannMagicDefOf.TM_Blink_III))
                                {
                                    if (current.level == 0)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Blink);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Blink);
                                            float minDistance = ActualManaCost(TorannMagicDefOf.TM_Blink) * 240;
                                            AutoCast.Blink.Evaluate(this, TorannMagicDefOf.TM_Blink, ability, magicPower, minDistance, out castSuccess);
                                            if (castSuccess)
                                            {
                                                goto AutoCastExit;
                                            }
                                        }
                                        if(flagCM && magicPower != null && this.spell_Blink && !magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Blink);
                                            float minDistance = ActualManaCost(TorannMagicDefOf.TM_Blink) * 200;
                                            AutoCast.Blink.Evaluate(this, TorannMagicDefOf.TM_Blink, ability, magicPower, minDistance, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else if (current.level == 1)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Blink);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Blink_I);
                                            float minDistance = ActualManaCost(TorannMagicDefOf.TM_Blink_I) * 240;
                                            AutoCast.Blink.Evaluate(this, TorannMagicDefOf.TM_Blink_I, ability, magicPower, minDistance, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }

                                    }
                                    else if (current.level == 2)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Blink_I);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Blink_II);
                                            float minDistance = ActualManaCost(TorannMagicDefOf.TM_Blink_II) * 240;
                                            AutoCast.Blink.Evaluate(this, TorannMagicDefOf.TM_Blink_II, ability, magicPower, minDistance, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Blink_II);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Blink_III);
                                            float minDistance = ActualManaCost(TorannMagicDefOf.TM_Blink_III) * 240;
                                            AutoCast.Blink.Evaluate(this, TorannMagicDefOf.TM_Blink_III, ability, magicPower, minDistance, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    
                                }
                            }
                        }
                    }
                    if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Druid) || flagCM)
                    {
                        PawnAbility ability = null;
                        foreach (MagicPower current in this.MagicData.MagicPowersD)
                        {
                            if (current != null && current.abilityDef != null && current.learned && !this.Pawn.CurJob.playerForced)
                            {
                                if (current.abilityDef == TorannMagicDefOf.TM_Regenerate)
                                {
                                    MagicPower magicPower = this.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Regenerate);
                                    if (magicPower != null && magicPower.learned && magicPower.autocast)
                                    {
                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Regenerate);
                                        MagicPowerSkill pwr = this.MagicData.MagicPowerSkill_Regenerate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Regenerate_pwr");
                                        if (pwr.level == 0)
                                        {
                                            AutoCast.HediffHealSpell.EvaluateMinSeverity(this, TorannMagicDefOf.TM_Regenerate, ability, magicPower, HediffDef.Named("TM_Regeneration"), 10f, out castSuccess);
                                        }
                                        else if(pwr.level == 1)
                                        {
                                            AutoCast.HediffHealSpell.EvaluateMinSeverity(this, TorannMagicDefOf.TM_Regenerate, ability, magicPower, HediffDef.Named("TM_Regeneration_I"), 12f, out castSuccess);
                                        }
                                        else if(pwr.level ==2)
                                        {
                                            AutoCast.HediffHealSpell.EvaluateMinSeverity(this, TorannMagicDefOf.TM_Regenerate, ability, magicPower, HediffDef.Named("TM_Regeneration_II"), 14f, out castSuccess);
                                        }
                                        else
                                        {
                                            AutoCast.HediffHealSpell.EvaluateMinSeverity(this, TorannMagicDefOf.TM_Regenerate, ability, magicPower, HediffDef.Named("TM_Regeneration_III"), 16f, out castSuccess);
                                        }
                                        if (castSuccess) goto AutoCastExit;
                                    }
                                }
                                if (current.abilityDef == TorannMagicDefOf.TM_CureDisease)
                                {
                                    MagicPower magicPower = this.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_CureDisease);
                                    if (magicPower != null && magicPower.learned && magicPower.autocast)
                                    {
                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_CureDisease);
                                        MagicPowerSkill ver = this.MagicData.MagicPowerSkill_CureDisease.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_CureDisease_ver");

                                        List<string> afflictionList = new List<string>();
                                        afflictionList.Clear();
                                        afflictionList.Add("Infection");
                                        afflictionList.Add("WoundInfection");
                                        afflictionList.Add("Flu");
                                        if (ver.level >= 1)
                                        {
                                            afflictionList.Add("GutWorms");
                                            afflictionList.Add("Malaria");
                                            afflictionList.Add("FoodPoisoning");
                                        }
                                        if (ver.level >= 2)
                                        {
                                            afflictionList.Add("SleepingSickness");
                                            afflictionList.Add("MuscleParasites");
                                        }
                                        if(ver.level >= 3)
                                        {
                                            afflictionList.Add("Plague");
                                            afflictionList.Add("Animal_Plague");
                                        }
                                        AutoCast.CureSpell.Evaluate(this, TorannMagicDefOf.TM_CureDisease, ability, magicPower, afflictionList, out castSuccess);
                                        if (castSuccess) goto AutoCastExit;
                                    }
                                }
                            }
                        }
                    }                    
                    if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Paladin) || flagCM)
                    {
                        PawnAbility ability = null;
                        foreach (MagicPower current in this.MagicData.MagicPowersP)
                        {
                            if (current != null && current.abilityDef != null && current.learned && !this.Pawn.CurJob.playerForced)
                            {
                                if (current.abilityDef == TorannMagicDefOf.TM_Heal)
                                {
                                    MagicPower magicPower = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Heal);
                                    if (magicPower != null && magicPower.learned && magicPower.autocast)
                                    {
                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Heal);
                                        AutoCast.HealSpell.Evaluate(this, TorannMagicDefOf.TM_Heal, ability, magicPower, out castSuccess);
                                        if (castSuccess) goto AutoCastExit;
                                    }
                                }
                                if ((current.abilityDef == TorannMagicDefOf.TM_Shield || current.abilityDef == TorannMagicDefOf.TM_Shield_I || current.abilityDef == TorannMagicDefOf.TM_Shield_II || current.abilityDef == TorannMagicDefOf.TM_Shield_III))
                                {
                                    if (current.level == 0)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Shield);
                                            AutoCast.Shield.Evaluate(this, TorannMagicDefOf.TM_Shield, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else if (current.level == 1)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Shield_I);
                                            AutoCast.Shield.Evaluate(this, TorannMagicDefOf.TM_Shield_I, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }

                                    }
                                    else if (current.level == 2)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield_I);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Shield_II);
                                            AutoCast.Shield.Evaluate(this, TorannMagicDefOf.TM_Shield_II, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield_II);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Shield_III);
                                            AutoCast.Shield.Evaluate(this, TorannMagicDefOf.TM_Shield_III, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Priest) || flagCM)
                    {
                        PawnAbility ability = null;
                        foreach (MagicPower current in this.MagicData.MagicPowersPR)
                        {
                            if (current != null && current.abilityDef != null && current.learned && !this.Pawn.CurJob.playerForced)
                            {
                                if (current.abilityDef == TorannMagicDefOf.TM_AdvancedHeal)
                                {
                                    MagicPower magicPower = this.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_AdvancedHeal);
                                    if (magicPower != null && magicPower.learned && magicPower.autocast)
                                    {
                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_AdvancedHeal);
                                        AutoCast.HealSpell.EvaluateMinSeverity(this, TorannMagicDefOf.TM_AdvancedHeal, ability, magicPower, 1f, out castSuccess);
                                        if (castSuccess) goto AutoCastExit;
                                    }
                                }
                                if (current.abilityDef == TorannMagicDefOf.TM_Purify)
                                {
                                    MagicPower magicPower = this.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Purify);
                                    if (magicPower != null && magicPower.learned && magicPower.autocast)
                                    {
                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Purify);
                                        MagicPowerSkill pwr = this.MagicData.MagicPowerSkill_Purify.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Purify_pwr");
                                        AutoCast.HealPermanentSpell.Evaluate(this, TorannMagicDefOf.TM_Purify, ability, magicPower, out castSuccess);
                                        if (castSuccess) goto AutoCastExit;
                                        List<string> afflictionList = new List<string>();
                                        afflictionList.Clear();
                                        afflictionList.Add("Cataract");
                                        afflictionList.Add("HearingLoss");
                                        afflictionList.Add("ToxicBuildup");
                                        if (pwr.level >= 1)
                                        {
                                            afflictionList.Add("Blindness");
                                            afflictionList.Add("Asthma");
                                            afflictionList.Add("Cirrhosis");
                                            afflictionList.Add("ChemicalDamageModerate");
                                        }
                                        if (pwr.level >= 2)
                                        {
                                            afflictionList.Add("Frail");
                                            afflictionList.Add("BadBack");
                                            afflictionList.Add("Carcinoma");
                                            afflictionList.Add("ChemicalDamageSevere");
                                        }
                                        if (pwr.level >= 3)
                                        {
                                            afflictionList.Add("Alzheimers");
                                            afflictionList.Add("Dementia");
                                            afflictionList.Add("HeartArteryBlockage");
                                            afflictionList.Add("PsychicShock");
                                            afflictionList.Add("CatatonicBreakdown");
                                        }
                                        AutoCast.CureSpell.Evaluate(this, TorannMagicDefOf.TM_Purify, ability, magicPower, afflictionList, out castSuccess);
                                        if (castSuccess) goto AutoCastExit;
                                        List<string> addictionList = new List<string>();
                                        addictionList.Clear();
                                        addictionList.Add("Alcohol");
                                        addictionList.Add("Smokeleaf");
                                        if (pwr.level >= 1)
                                        {
                                            addictionList.Add("GoJuice");
                                            addictionList.Add("WakeUp");
                                        }
                                        if (pwr.level >= 2)
                                        {
                                            addictionList.Add("Psychite");
                                        }
                                        if (pwr.level >= 3)
                                        {
                                            IEnumerable<ChemicalDef> enumerable = from def in DefDatabase<ChemicalDef>.AllDefs
                                                                                where (true)
                                                                                select def;
                                            foreach (ChemicalDef addiction in enumerable)
                                            {
                                                if (addiction.defName != "ROMV_VitaeAddiction" && addiction != TorannMagicDefOf.Luciferium)
                                                {
                                                    addictionList.AddDistinct(addiction.defName);
                                                }
                                            }                                            
                                        }
                                        AutoCast.CureAddictionSpell.Evaluate(this, TorannMagicDefOf.TM_Purify, ability, magicPower, addictionList, out castSuccess);
                                        if (castSuccess) goto AutoCastExit;
                                    }
                                }
                            }
                        }
                    }
                    if (this.spell_MechaniteReprogramming && this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer) || flagCM)
                    {
                        MagicPower magicPower = this.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_MechaniteReprogramming);
                        if (magicPower != null && magicPower.autocast && !this.Pawn.CurJob.playerForced)
                        {
                            PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_MechaniteReprogramming);
                            List<string> afflictionList = new List<string>();
                            afflictionList.Clear();
                            afflictionList.Add("SensoryMechanites");
                            afflictionList.Add("FibrousMechanites");
                            AutoCast.CureSpell.Evaluate(this, TorannMagicDefOf.TM_MechaniteReprogramming, ability, magicPower, afflictionList, out castSuccess);
                            if (castSuccess) goto AutoCastExit;
                        }
                    }
                    if (this.spell_Heal && !this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Paladin))
                    {
                        MagicPower magicPower = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Heal);
                        if (magicPower.autocast && !this.Pawn.CurJob.playerForced)
                        {
                            PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Heal);
                            AutoCast.HealSpell.Evaluate(this, TorannMagicDefOf.TM_Heal, ability, magicPower, out castSuccess);
                            if (castSuccess) goto AutoCastExit;
                        }
                    }                    
                    if (this.spell_TransferMana && !this.Pawn.CurJob.playerForced)
                    {
                        MagicPower magicPower = this.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TransferMana);
                        if (magicPower.autocast)
                        {
                            PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_TransferMana);
                            AutoCast.TransferManaSpell.Evaluate(this, TorannMagicDefOf.TM_TransferMana, ability, magicPower, false, false, out castSuccess);
                            if (castSuccess) goto AutoCastExit;
                        }
                    }
                    if (this.spell_SiphonMana && !this.Pawn.CurJob.playerForced)
                    {
                        MagicPower magicPower = this.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SiphonMana);
                        if (magicPower.autocast)
                        {
                            PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_SiphonMana);
                            AutoCast.TransferManaSpell.Evaluate(this, TorannMagicDefOf.TM_SiphonMana, ability, magicPower, false, true, out castSuccess);
                            if (castSuccess) goto AutoCastExit;
                        }
                    }
                    if (this.spell_CauterizeWound && !this.Pawn.CurJob.playerForced)
                    {
                        MagicPower magicPower = this.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_CauterizeWound);
                        if (magicPower.autocast)
                        {
                            PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_CauterizeWound);
                            AutoCast.HealSpell.EvaluateMinSeverity(this, TorannMagicDefOf.TM_CauterizeWound, ability, magicPower, 40f, out castSuccess);
                            if (castSuccess) goto AutoCastExit;
                        }
                    }
                    if (this.spell_SpellMending && !this.Pawn.CurJob.playerForced)
                    {
                        MagicPower magicPower = this.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SpellMending);
                        if (magicPower.autocast)
                        {
                            PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_SpellMending);
                            AutoCast.SpellMending.Evaluate(this, TorannMagicDefOf.TM_SpellMending, ability, magicPower, HediffDef.Named("SpellMendingHD"), out castSuccess);
                            if (castSuccess) goto AutoCastExit;
                        }
                    }
                    if (this.spell_Teach && !this.Pawn.CurJob.playerForced)
                    {
                        MagicPower magicPower = this.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TeachMagic);
                        if (magicPower.autocast)
                        {
                            if (this.Pawn.CurJobDef.joyKind != null || this.Pawn.CurJobDef == JobDefOf.Wait_Wander || Pawn.CurJobDef == JobDefOf.GotoWander)
                            {
                                PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_TeachMagic);
                                AutoCast.Teach.Evaluate(this, TorannMagicDefOf.TM_TeachMagic, ability, magicPower, out castSuccess);
                                if (castSuccess) goto AutoCastExit;
                            }
                        }
                    }
                    if (this.spell_SummonMinion && !this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Summoner))
                    {
                        MagicPower magicPower = this.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonMinion);
                        if (magicPower.autocast && !this.Pawn.CurJob.playerForced && this.summonedMinions.Count() < 4)
                        {
                            PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_SummonMinion);
                            AutoCast.CastOnSelf.Evaluate(this, TorannMagicDefOf.TM_SummonMinion, ability, magicPower, out castSuccess);
                            if (castSuccess) goto AutoCastExit;
                        }
                    }
                    if (this.spell_DirtDevil)
                    {
                        MagicPower magicPower = this.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_DirtDevil);
                        if (magicPower != null && magicPower.autocast && !this.Pawn.CurJob.playerForced && this.Pawn.GetRoom() != null)
                        {
                            float roomCleanliness = this.Pawn.GetRoom().GetStat(RoomStatDefOf.Cleanliness);

                            if (roomCleanliness < -2f)
                            {
                                PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_DirtDevil);
                                AutoCast.CastOnSelf.Evaluate(this, TorannMagicDefOf.TM_DirtDevil, ability, magicPower, out castSuccess);
                                if (castSuccess) goto AutoCastExit;
                            }
                        }
                    }
                    if (this.spell_Blink && !this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Arcanist) && !flagCM)
                    {
                        MagicPower magicPower = this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Blink);
                        if (magicPower.autocast)
                        {
                            PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Blink);
                            float minDistance = ActualManaCost(TorannMagicDefOf.TM_Blink) * 200;
                            AutoCast.Blink.Evaluate(this, TorannMagicDefOf.TM_Blink, ability, magicPower, minDistance, out castSuccess);
                            if (castSuccess) goto AutoCastExit;
                        }
                    }
                }

                //combat (drafted) spells
                if (this.Pawn.drafter != null && this.Pawn.Drafted && this.Pawn.CurJob.def != JobDefOf.Goto && this.Mana != null && this.Mana.CurLevelPercentage >= settingsRef.autocastMinThreshold)
                {
                    if ((this.Pawn.story.traits.HasTrait(TorannMagicDefOf.InnerFire) || flagCM ) && this.Pawn.story.DisabledWorkTagsBackstoryAndTraits != WorkTags.Violent)
                    {
                        PawnAbility ability = null;
                        foreach (MagicPower current in this.MagicData.MagicPowersIF)
                        {
                            if (current != null && current.abilityDef != null && current.learned)
                            {
                                if (current.abilityDef == TorannMagicDefOf.TM_Firebolt)
                                {
                                    MagicPower magicPower = this.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Firebolt);
                                    if (magicPower != null && magicPower.learned && magicPower.autocast)
                                    {
                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Firebolt);
                                        AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_Firebolt, ability, magicPower, out castSuccess);
                                        if (castSuccess) goto AutoCastExit;
                                    }
                                }
                            }
                        }
                    }
                    if ((this.Pawn.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost) || flagCM) && this.Pawn.story.DisabledWorkTagsBackstoryAndTraits != WorkTags.Violent)
                    {
                        PawnAbility ability = null;
                        foreach (MagicPower current in this.MagicData.MagicPowersHoF)
                        {
                            if (current != null && current.abilityDef != null && current.learned)
                            {
                                if (current.abilityDef == TorannMagicDefOf.TM_Icebolt)
                                {
                                    MagicPower magicPower = this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Icebolt);
                                    if (magicPower != null && magicPower.learned && magicPower.autocast)
                                    {
                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Icebolt);
                                        AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_Icebolt, ability, magicPower, out castSuccess);
                                        if (castSuccess) goto AutoCastExit;
                                    }
                                }
                                else if ((current.abilityDef == TorannMagicDefOf.TM_FrostRay || current.abilityDef == TorannMagicDefOf.TM_FrostRay_I || current.abilityDef == TorannMagicDefOf.TM_FrostRay_II || current.abilityDef == TorannMagicDefOf.TM_FrostRay_III))
                                {
                                    if (current.level == 0)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_FrostRay);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_FrostRay);
                                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_FrostRay, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else if (current.level == 1)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_FrostRay);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_FrostRay_I);
                                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_FrostRay_I, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }

                                    }
                                    else if (current.level == 2)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_FrostRay_I);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_FrostRay_II);
                                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_FrostRay_II, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_FrostRay_II);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_FrostRay_III);
                                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_FrostRay_III, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if((this.Pawn.story.traits.HasTrait(TorannMagicDefOf.StormBorn) || flagCM) && this.Pawn.story.DisabledWorkTagsBackstoryAndTraits != WorkTags.Violent)
                    {
                        PawnAbility ability = null;
                        foreach (MagicPower current in this.MagicData.MagicPowersSB)
                        {
                            if (current != null && current.abilityDef != null && current.learned)
                            {
                                if (current.abilityDef == TorannMagicDefOf.TM_LightningBolt)
                                {
                                    MagicPower magicPower = this.MagicData.MagicPowersSB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_LightningBolt);
                                    if (magicPower != null && magicPower.learned && magicPower.autocast)
                                    {
                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_LightningBolt);
                                        AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_LightningBolt, ability, magicPower, out castSuccess);
                                        if (castSuccess) goto AutoCastExit;
                                    }
                                }
                            }
                        }
                    }
                    if ((this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Arcanist) || flagCM) && this.Pawn.story.DisabledWorkTagsBackstoryAndTraits != WorkTags.Violent)
                    {
                        PawnAbility ability = null;
                        foreach (MagicPower current in this.MagicData.MagicPowersA)
                        {
                            if (current != null && current.abilityDef != null && current.learned)
                            {
                                if ((current.abilityDef == TorannMagicDefOf.TM_MagicMissile || current.abilityDef == TorannMagicDefOf.TM_MagicMissile_I || current.abilityDef == TorannMagicDefOf.TM_MagicMissile_II || current.abilityDef == TorannMagicDefOf.TM_MagicMissile_III))
                                {
                                    if (current.level == 0)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_MagicMissile);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_MagicMissile);
                                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_MagicMissile, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else if (current.level == 1)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_MagicMissile);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_MagicMissile_I);
                                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_MagicMissile_I, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }

                                    }
                                    else if (current.level == 2)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_MagicMissile_I);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_MagicMissile_II);
                                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_MagicMissile_II, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_MagicMissile_II);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_MagicMissile_III);
                                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_MagicMissile_III, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if ((this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Druid) || flagCM))
                    {
                        PawnAbility ability = null;
                        foreach (MagicPower current in this.MagicData.MagicPowersD)
                        {
                            if (current != null && current.abilityDef != null && current.learned)
                            {
                                if (current.abilityDef == TorannMagicDefOf.TM_Poison && this.Pawn.story.DisabledWorkTagsBackstoryAndTraits != WorkTags.Violent)
                                {
                                    MagicPower magicPower = this.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Poison);
                                    if (magicPower != null && magicPower.learned && magicPower.autocast)
                                    {
                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Poison);
                                        AutoCast.HediffSpell.EvaluateMinRange(this, TorannMagicDefOf.TM_Poison, ability, magicPower, HediffDef.Named("TM_Poisoned_HD"), 10, out castSuccess);
                                        if (castSuccess) goto AutoCastExit;
                                    }
                                }
                                if (current.abilityDef == TorannMagicDefOf.TM_Regenerate)
                                {
                                    MagicPower magicPower = this.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Regenerate);
                                    if (magicPower != null && magicPower.learned && magicPower.autocast)
                                    {
                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Regenerate);
                                        MagicPowerSkill pwr = this.MagicData.MagicPowerSkill_Regenerate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Regenerate_pwr");
                                        if (pwr.level == 0)
                                        {
                                            AutoCast.HediffHealSpell.EvaluateMinSeverity(this, TorannMagicDefOf.TM_Regenerate, ability, magicPower, HediffDef.Named("TM_Regeneration"), 10f, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                        else if (pwr.level == 1)
                                        {
                                            AutoCast.HediffHealSpell.EvaluateMinSeverity(this, TorannMagicDefOf.TM_Regenerate, ability, magicPower, HediffDef.Named("TM_Regeneration_I"), 12f, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                        else if (pwr.level == 2)
                                        {
                                            AutoCast.HediffHealSpell.EvaluateMinSeverity(this, TorannMagicDefOf.TM_Regenerate, ability, magicPower, HediffDef.Named("TM_Regeneration_II"), 14f, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                        else
                                        {
                                            AutoCast.HediffHealSpell.EvaluateMinSeverity(this, TorannMagicDefOf.TM_Regenerate, ability, magicPower, HediffDef.Named("TM_Regeneration_III"), 16f, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if ((this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus) || flagCM) && this.Pawn.story.DisabledWorkTagsBackstoryAndTraits != WorkTags.Violent)
                    {
                        PawnAbility ability = null;
                        foreach (MagicPower current in this.MagicData.MagicPowersSD)
                        {
                            if (current != null && current.abilityDef != null && current.learned)
                            {
                                if ((current.abilityDef == TorannMagicDefOf.TM_ShadowBolt || current.abilityDef == TorannMagicDefOf.TM_ShadowBolt_I || current.abilityDef == TorannMagicDefOf.TM_ShadowBolt_II || current.abilityDef == TorannMagicDefOf.TM_ShadowBolt_III))
                                {
                                    if (current.level == 0)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ShadowBolt);
                                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_ShadowBolt, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else if (current.level == 1)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ShadowBolt_I);
                                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_ShadowBolt_I, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }

                                    }
                                    else if (current.level == 2)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt_I);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ShadowBolt_II);
                                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_ShadowBolt_II, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt_II);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ShadowBolt_III);
                                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_ShadowBolt_III, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if ((this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Warlock) || flagCM) && this.Pawn.story.DisabledWorkTagsBackstoryAndTraits != WorkTags.Violent)
                    {
                        PawnAbility ability = null;
                        foreach (MagicPower current in this.MagicData.MagicPowersWD)
                        {
                            if (current != null && current.abilityDef != null && current.learned)
                            {
                                if ((current.abilityDef == TorannMagicDefOf.TM_ShadowBolt || current.abilityDef == TorannMagicDefOf.TM_ShadowBolt_I || current.abilityDef == TorannMagicDefOf.TM_ShadowBolt_II || current.abilityDef == TorannMagicDefOf.TM_ShadowBolt_III))
                                {
                                    if (current.level == 0)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ShadowBolt);
                                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_ShadowBolt, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else if (current.level == 1)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ShadowBolt_I);
                                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_ShadowBolt_I, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }

                                    }
                                    else if (current.level == 2)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt_I);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ShadowBolt_II);
                                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_ShadowBolt_II, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ShadowBolt_II);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ShadowBolt_III);
                                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_ShadowBolt_III, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                }
                            }
                        }
                    }                    
                    if ((this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Paladin) || flagCM))
                    {
                        PawnAbility ability = null;
                        foreach (MagicPower current in this.MagicData.MagicPowersP)
                        {
                            if (current != null && current.abilityDef != null && current.learned)
                            {
                                if (current.abilityDef == TorannMagicDefOf.TM_Heal)
                                {
                                    MagicPower magicPower = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Heal);
                                    if (magicPower != null && magicPower.learned && magicPower.autocast)
                                    {
                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Heal);
                                        AutoCast.HealSpell.Evaluate(this, TorannMagicDefOf.TM_Heal, ability, magicPower, out castSuccess);
                                        if (castSuccess) goto AutoCastExit;
                                    }
                                }
                                if ((current.abilityDef == TorannMagicDefOf.TM_Shield || current.abilityDef == TorannMagicDefOf.TM_Shield_I || current.abilityDef == TorannMagicDefOf.TM_Shield_II || current.abilityDef == TorannMagicDefOf.TM_Shield_III))
                                {
                                    if (current.level == 0)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Shield);
                                            AutoCast.Shield.Evaluate(this, TorannMagicDefOf.TM_Shield, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else if (current.level == 1)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Shield_I);
                                            AutoCast.Shield.Evaluate(this, TorannMagicDefOf.TM_Shield_I, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }

                                    }
                                    else if (current.level == 2)
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield_I);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Shield_II);
                                            AutoCast.Shield.Evaluate(this, TorannMagicDefOf.TM_Shield_II, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else
                                    {
                                        MagicPower magicPower = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shield_II);
                                        if (magicPower != null && magicPower.learned && magicPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Shield_III);
                                            AutoCast.Shield.Evaluate(this, TorannMagicDefOf.TM_Shield_III, ability, magicPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Priest) || flagCM)
                    {
                        PawnAbility ability = null;
                        foreach (MagicPower current in this.MagicData.MagicPowersPR)
                        {
                            if (current != null && current.abilityDef != null && current.learned)
                            {
                                if (current.abilityDef == TorannMagicDefOf.TM_AdvancedHeal)
                                {
                                    MagicPower magicPower = this.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_AdvancedHeal);
                                    if (magicPower != null && magicPower.learned && magicPower.autocast)
                                    {
                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_AdvancedHeal);
                                        AutoCast.HealSpell.EvaluateMinSeverity(this, TorannMagicDefOf.TM_AdvancedHeal, ability, magicPower, 1f, out castSuccess);
                                        if (castSuccess) goto AutoCastExit;
                                    }
                                }
                            }
                        }
                    }
                    if (this.spell_Heal && !this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Paladin))
                    {
                        MagicPower magicPower = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Heal);
                        if (magicPower.autocast)
                        {
                            PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Heal);
                            AutoCast.HealSpell.Evaluate(this, TorannMagicDefOf.TM_Heal, ability, magicPower, out castSuccess);
                            if (castSuccess) goto AutoCastExit;
                        }
                    }
                    if (this.spell_SiphonMana)
                    {
                        MagicPower magicPower = this.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SiphonMana);
                        if (magicPower.autocast)
                        {
                            PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_SiphonMana);
                            AutoCast.TransferManaSpell.Evaluate(this, TorannMagicDefOf.TM_SiphonMana, ability, magicPower, true, true, out castSuccess);
                            if (castSuccess) goto AutoCastExit;
                        }
                    }
                    if (this.spell_CauterizeWound)
                    {
                        MagicPower magicPower = this.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_CauterizeWound);
                        if (magicPower.autocast)
                        {
                            PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_CauterizeWound);
                            AutoCast.HealSpell.EvaluateMinSeverity(this, TorannMagicDefOf.TM_CauterizeWound, ability, magicPower, 40f, out castSuccess);
                            if (castSuccess) goto AutoCastExit;
                        }
                    }
                    if (this.spell_ArcaneBolt && this.Pawn.story.DisabledWorkTagsBackstoryAndTraits != WorkTags.Violent)
                    {
                        MagicPower magicPower = this.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ArcaneBolt);
                        if (magicPower != null && magicPower.autocast)
                        {
                            PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ArcaneBolt);
                            AutoCast.DamageSpell.Evaluate(this, TorannMagicDefOf.TM_ArcaneBolt, ability, magicPower, out castSuccess);
                            if (castSuccess) goto AutoCastExit;
                        }
                    }
                }
                AutoCastExit:;
            }
        }

        private void ResolveEarthSpriteAction()
        {
            MagicPowerSkill magicPowerSkill = this.MagicData.MagicPowerSkill_EarthSprites.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EarthSprites_pwr");
            //Log.Message("resolving sprites");
            if(this.earthSpriteMap == null)
                {
                    this.earthSpriteMap = this.Pawn.Map;
                }
            if (this.earthSpriteType == 1) //mining stone
            {
                //Log.Message("stone");
                Building mineTarget = this.earthSprites.GetFirstBuilding(this.earthSpriteMap);                
                this.nextEarthSpriteAction = Find.TickManager.TicksGame + Mathf.RoundToInt((300 * (1 - (.1f * magicPowerSkill.level))) / this.arcaneDmg);
                TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_SparkFlash"), this.earthSprites.ToVector3Shifted(), this.earthSpriteMap, Rand.Range(2f, 5f), .05f, 0f, .1f, 0, 0f, 0f, 0f);
                var mineable = mineTarget as Mineable;
                int num = 80;
                if(mineable != null && mineTarget.HitPoints > num)
                {
                    var dinfo = new DamageInfo(DamageDefOf.Mining, num, 0, -1f, this.Pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
                    mineTarget.TakeDamage(dinfo);
                    ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                    if (Rand.Chance(settingsRef.magicyteChance *2))
                    {
                        Thing thing = null;
                        thing = ThingMaker.MakeThing(TorannMagicDefOf.RawMagicyte);
                        thing.stackCount = Rand.Range(8, 16);
                        if (thing != null)
                        {
                            GenPlace.TryPlaceThing(thing, this.earthSprites, this.earthSpriteMap, ThingPlaceMode.Near, null);
                        }
                    }
                }
                else if(mineable != null && mineTarget.HitPoints <= num)
                {
                    mineable.DestroyMined(this.Pawn);
                }

                if (mineable.DestroyedOrNull())
                {
                    IntVec3 oldEarthSpriteLoc = this.earthSprites;
                    Building newMineSpot = null;
                    if (this.earthSpritesInArea)
                    {
                        //Log.Message("moving in area");
                        List<IntVec3> spriteAreaCells = GenRadial.RadialCellsAround(oldEarthSpriteLoc, 6f, false).ToList();
                        spriteAreaCells.Shuffle();
                        for (int i = 0; i < spriteAreaCells.Count; i++)
                        {
                            IntVec3 intVec = spriteAreaCells[i];
                            newMineSpot = intVec.GetFirstBuilding(this.earthSpriteMap);
                            if (newMineSpot != null && !intVec.Fogged(earthSpriteMap) && TM_Calc.GetSpriteArea() != null && TM_Calc.GetSpriteArea().ActiveCells.Contains(intVec))
                            {
                                mineable = newMineSpot as Mineable;
                                if (mineable != null)
                                {
                                    this.earthSprites = intVec;
                                    //Log.Message("assigning");
                                    break;
                                }
                                newMineSpot = null;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            IntVec3 intVec = earthSprites + GenAdj.AdjacentCells.RandomElement();
                            newMineSpot = intVec.GetFirstBuilding(this.earthSpriteMap);
                            if (newMineSpot != null)
                            {
                                mineable = newMineSpot as Mineable;
                                if (mineable != null)
                                {
                                    this.earthSprites = intVec;
                                    i = 20;
                                }
                                newMineSpot = null;
                            }
                        }
                    }

                    if(oldEarthSpriteLoc == this.earthSprites)
                    {
                        this.earthSpriteType = 0;
                        this.earthSprites = IntVec3.Invalid;                        
                        this.earthSpritesInArea = false;
                    }
                }
            }
            else if(this.earthSpriteType == 2) //transforming soil
            {
                //Log.Message("earth");
                this.nextEarthSpriteAction = Find.TickManager.TicksGame + Mathf.RoundToInt((24000 * (1 - (.1f * magicPowerSkill.level)))/this.arcaneDmg); 
                for (int m = 0; m < 4; m++)
                {
                    TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_ThickDust"), this.earthSprites.ToVector3Shifted(), this.earthSpriteMap, Rand.Range(.3f, .5f), Rand.Range(.2f, .3f), .05f, Rand.Range(.4f, .6f), Rand.Range(-20, 20), Rand.Range(.5f, 1f), Rand.Range(0, 360), Rand.Range(0, 360));
                }
                Map map = this.earthSpriteMap;
                IntVec3 curCell = this.earthSprites;
                TerrainDef terrain = curCell.GetTerrain(map);
                if (Rand.Chance(.8f))
                {
                    Thing thing = null;
                    thing = ThingMaker.MakeThing(TorannMagicDefOf.RawMagicyte);
                    thing.stackCount = Rand.Range(10, 20);
                    if (thing != null)
                    {
                        GenPlace.TryPlaceThing(thing, this.earthSprites, this.earthSpriteMap, ThingPlaceMode.Near, null);
                    }
                }
                if (curCell.InBounds(map) && curCell.IsValid && terrain != null)
                {
                    if (terrain.defName == "MarshyTerrain" || terrain.defName == "Mud" || terrain.defName == "Marsh")
                    {
                        map.terrainGrid.SetTerrain(curCell, terrain.driesTo);
                    }
                    else if (terrain.defName == "WaterShallow")
                    {
                        map.terrainGrid.SetTerrain(curCell, TerrainDef.Named("Marsh"));
                    }
                    else if (terrain.defName == "Ice")
                    {
                        map.terrainGrid.SetTerrain(curCell, TerrainDef.Named("Mud"));
                    }
                    else if(terrain.defName == "Soil")
                    {
                        map.terrainGrid.SetTerrain(curCell, TerrainDef.Named("SoilRich"));
                    }
                    else if (terrain.defName == "Sand" || terrain.defName == "Gravel" || terrain.defName == "MossyTerrain")
                    {
                        map.terrainGrid.SetTerrain(curCell, TerrainDef.Named("Soil"));
                    }
                    else if (terrain.defName == "SoftSand")
                    {
                        map.terrainGrid.SetTerrain(curCell, TerrainDef.Named("Sand"));
                    }
                    else
                    {
                        Log.Message("unable to resolve terraindef - resetting earth sprite parameters");
                        this.earthSprites = IntVec3.Invalid;
                        this.earthSpriteMap = null;
                        this.earthSpriteType = 0;
                        this.earthSpritesInArea = false;
                    }

                    terrain = curCell.GetTerrain(map);
                    if (terrain.defName == "SoilRich")
                    {
                        //look for new spot to transform
                        IntVec3 oldEarthSpriteLoc = this.earthSprites;
                        if (this.earthSpritesInArea)
                        {
                            //Log.Message("moving in area");
                            List<IntVec3> spriteAreaCells = GenRadial.RadialCellsAround(oldEarthSpriteLoc, 6f, false).ToList();
                            spriteAreaCells.Shuffle();
                            for (int i = 0; i < spriteAreaCells.Count; i++)
                            {
                                IntVec3 intVec = spriteAreaCells[i];
                                terrain = intVec.GetTerrain(map);
                                if (terrain.defName == "MarshyTerrain" || terrain.defName == "Mud" || terrain.defName == "Marsh" || terrain.defName == "WaterShallow" || terrain.defName == "Ice" ||
                            terrain.defName == "Sand" || terrain.defName == "Gravel" || terrain.defName == "Soil" || terrain.defName == "MossyTerrain" || terrain.defName == "SoftSand")
                                {
                                    Building terrainHasBuilding = null;
                                    terrainHasBuilding = intVec.GetFirstBuilding(this.earthSpriteMap);
                                    if (TM_Calc.GetSpriteArea() != null && TM_Calc.GetSpriteArea().ActiveCells.Contains(intVec)) //dont transform terrain underneath buildings
                                    {
                                        //Log.Message("assigning");
                                        this.earthSprites = intVec;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                IntVec3 intVec = earthSprites + GenAdj.AdjacentCells.RandomElement();
                                terrain = intVec.GetTerrain(map);
                                if (terrain.defName == "MarshyTerrain" || terrain.defName == "Mud" || terrain.defName == "Marsh" || terrain.defName == "WaterShallow" || terrain.defName == "Ice" ||
                            terrain.defName == "Sand" || terrain.defName == "Gravel" || terrain.defName == "Soil" || terrain.defName == "MossyTerrain" || terrain.defName == "SoftSand")
                                {
                                    Building terrainHasBuilding = null;
                                    terrainHasBuilding = intVec.GetFirstBuilding(this.earthSpriteMap);
                                    if (terrainHasBuilding == null) //dont transform terrain underneath buildings
                                    {
                                        this.earthSprites = intVec;
                                        i = 20;
                                    }
                                }
                            }
                        }

                        if(oldEarthSpriteLoc == earthSprites)
                        {
                            this.earthSpriteType = 0;
                            this.earthSpriteMap = null;
                            this.earthSprites = IntVec3.Invalid;
                            this.earthSpritesInArea = false;
                            //Log.Message("ending");
                        }
                    }
                }
            }
        }

        public void ResolveEffecter()
        {
            bool spawned = this.Pawn.Spawned;
            if (spawned)
            {
                if (this.powerEffecter != null && this.PowerModifier == 0)
                {
                    this.powerEffecter.Cleanup();
                    this.powerEffecter = null;
                }
                bool flag4 = this.powerEffecter == null && this.PowerModifier > 0;
                if (flag4)
                {
                    EffecterDef progressBar = EffecterDefOf.ProgressBar;
                    this.powerEffecter = progressBar.Spawn();
                }
                if(this.powerEffecter != null && this.PowerModifier > 0)
                {
                    this.powerEffecter.EffectTick(this.Pawn, TargetInfo.Invalid);                    
                    MoteProgressBar mote = ((SubEffecter_ProgressBar)this.powerEffecter.children[0]).mote;
                    bool flag5 = mote != null;
                    if (flag5)
                    {
                        float value = (float)(this.powerModifier) / (float)(this.maxPower);
                        mote.progress = Mathf.Clamp01(value);
                        mote.offsetZ = +0.85f;
                    }
                }
            }
        }

        public void ResolveUndead()
        {
            int undeadCount = 0;
            foreach (Pawn current in this.Pawn.Map.mapPawns.PawnsInFaction(this.Pawn.Faction))
            {
                if (current.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadHD) || current.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadAnimalHD))
                {
                    undeadCount++;
                }
            }
            if(undeadCount > 0 && this.dismissUndeadSpell == false)
            {
                this.AddPawnAbility(TorannMagicDefOf.TM_DismissUndead);
                this.dismissUndeadSpell = true;
            }
            if(undeadCount <= 0 && this.dismissUndeadSpell == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_DismissUndead);
                this.dismissUndeadSpell = false;
            }            
        }

        public void ResolveSuccubusLovin()
        {
            if(this.Pawn.CurrentBed() != null && !this.Pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_VitalityBoostHD"), false))
            {
                Pawn pawnInMyBed = TM_Calc.FindNearbyOtherPawn(this.Pawn, 1);
                if(pawnInMyBed != null)
                {
                    if (pawnInMyBed.CurrentBed() != null && pawnInMyBed.RaceProps.Humanlike)
                    {
                        Job job = new Job(JobDefOf.Lovin, pawnInMyBed, this.Pawn.CurrentBed());
                        this.Pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                        HealthUtility.AdjustSeverity(pawnInMyBed, HediffDef.Named("TM_VitalityDrainHD"), 8);
                        HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_VitalityBoostHD"), 6);
                    }
                }
            }
        }

        public void ResolveWarlockEmpathy()
        {
            //strange bug observed where other pawns will get the old offset of the previous pawn's offset unless other pawn has no empathy existing
            //in other words, empathy base mood effect seems to carry over from last otherpawn instead of using current otherpawn values
            if (Rand.Chance(this.Pawn.GetStatValue(StatDefOf.PsychicSensitivity, false) - 1))
            {
                Pawn otherPawn = TM_Calc.FindNearbyOtherPawn(this.Pawn, 5);
                if (otherPawn != null && otherPawn.RaceProps.Humanlike && otherPawn.IsColonist)
                {
                    if (Rand.Chance(otherPawn.GetStatValue(StatDefOf.PsychicSensitivity, false) - .3f))
                    {
                        ThoughtHandler pawnThoughtHandler = new ThoughtHandler(this.Pawn);
                        List<Thought> pawnThoughts = new List<Thought>();
                        pawnThoughtHandler.GetAllMoodThoughts(pawnThoughts);
                        List<Thought> otherThoughts = new List<Thought>();
                        otherPawn.needs.mood.thoughts.GetAllMoodThoughts(otherThoughts);                        
                        List<Thought_Memory> memoryThoughts = new List<Thought_Memory>();
                        memoryThoughts.Clear();
                        float oldMemoryOffset = 0;
                        if (Rand.Chance(.3f)) //empathy absorbed by warlock
                        {
                            ThoughtDef empathyThought = ThoughtDef.Named("WarlockEmpathy");
                            memoryThoughts = this.Pawn.needs.mood.thoughts.memories.Memories;
                            for (int i = 0; i < memoryThoughts.Count; i++)
                            {
                                if (memoryThoughts[i].def.defName == "WarlockEmpathy")
                                {
                                    oldMemoryOffset = memoryThoughts[i].MoodOffset();
                                    if (oldMemoryOffset > 30)
                                    {
                                        oldMemoryOffset = 30;
                                    }
                                    else if(oldMemoryOffset < -30)
                                    {
                                        oldMemoryOffset = -30;
                                    }
                                    this.Pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(memoryThoughts[i].def);
                                }
                            }
                            Thought transferThought = otherThoughts.RandomElement();
                            float newOffset = Mathf.RoundToInt(transferThought.CurStage.baseMoodEffect / 2);
                            empathyThought.stages.FirstOrDefault().baseMoodEffect = newOffset + oldMemoryOffset;
                            
                            this.Pawn.needs.mood.thoughts.memories.TryGainMemory(empathyThought, null);
                            Vector3 drawPosOffset = this.Pawn.DrawPos;
                            drawPosOffset.z += .3f;
                            TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_ArcaneCircle"), drawPosOffset, this.Pawn.Map, newOffset / 20, .2f, .1f, .1f, Rand.Range(100, 200), 0, 0, Rand.Range(0, 360));
                        }
                        else //empathy bleeding to other pawn
                        {
                            ThoughtDef empathyThought = ThoughtDef.Named("PsychicEmpathy");
                            memoryThoughts = otherPawn.needs.mood.thoughts.memories.Memories;
                            for (int i = 0; i < memoryThoughts.Count; i++)
                            {
                                if (memoryThoughts[i].def.defName == "PsychicEmpathy")
                                {
                                    oldMemoryOffset = memoryThoughts[i].CurStage.baseMoodEffect;
                                    if(oldMemoryOffset > 30)
                                    {
                                        oldMemoryOffset = 30;
                                    }
                                    else if (oldMemoryOffset < -30)
                                    {
                                        oldMemoryOffset = -30;
                                    }
                                    otherPawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(memoryThoughts[i].def);
                                }
                            }
                            Thought transferThought = pawnThoughts.RandomElement();
                            float newOffset = Mathf.RoundToInt(transferThought.CurStage.baseMoodEffect / 2);
                            empathyThought.stages.FirstOrDefault().baseMoodEffect = newOffset + oldMemoryOffset;

                            otherPawn.needs.mood.thoughts.memories.TryGainMemory(empathyThought, null);
                            Vector3 drawPosOffset = otherPawn.DrawPos;
                            drawPosOffset.z += .3f;
                            TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_ArcaneCircle"), drawPosOffset, otherPawn.Map, newOffset / 20, .2f, .1f, .1f, Rand.Range(100, 200), 0, 0, Rand.Range(0, 360));
                        }
                    }
                }
            }
        }

        public void ResolveTechnomancerOverdrive()
        {
            if (this.overdriveBuilding != null)
            {
                List<Pawn> odPawns = ModOptions.Constants.GetOverdrivePawnList();
                
                if (!odPawns.Contains(this.Pawn))
                {
                    odPawns.Add(this.Pawn);
                    ModOptions.Constants.SetOverdrivePawnList(odPawns);
                }
                Vector3 rndPos = this.overdriveBuilding.DrawPos;
                rndPos.x += Rand.Range(-.4f, .4f);
                rndPos.z += Rand.Range(-.4f, .4f);
                TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_SparkFlash"), rndPos, this.overdriveBuilding.Map, Rand.Range(.6f, .8f), .1f, .05f, .05f, 0, 0, 0, Rand.Range(0, 360));
                MoteMaker.ThrowSmoke(rndPos, this.overdriveBuilding.Map, Rand.Range(.8f, 1.2f));
                rndPos = this.overdriveBuilding.DrawPos;
                rndPos.x += Rand.Range(-.4f, .4f);
                rndPos.z += Rand.Range(-.4f, .4f);
                TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_ElectricalSpark"), rndPos, this.overdriveBuilding.Map, Rand.Range(.4f, .7f), .2f, .05f, .1f, 0, 0, 0, Rand.Range(0, 360));
                SoundInfo info = SoundInfo.InMap(new TargetInfo(this.overdriveBuilding.Position, this.overdriveBuilding.Map, false), MaintenanceType.None);
                info.pitchFactor = .4f;
                info.volumeFactor = .3f;
                SoundDefOf.TurretAcquireTarget.PlayOneShot(info);
                MagicPowerSkill damageControl = this.MagicData.MagicPowerSkill_Overdrive.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Overdrive_ver");
                if (Rand.Chance(.6f - (.06f * damageControl.level)))
                {                    
                    TM_Action.DamageEntities(this.overdriveBuilding, null, Rand.Range(3f, (7f - (1f * damageControl.level))), DamageDefOf.Burn, this.overdriveBuilding);
                }
                this.overdriveFrequency = 100 + (10 * damageControl.level);
                if(Rand.Chance(.4f))
                {
                    this.overdriveFrequency /= 2;
                }
                this.overdriveDuration--;
                if(this.overdriveDuration <= 0)
                {
                    if (odPawns != null && odPawns.Contains(this.Pawn))
                    { 
                        odPawns.Remove(this.Pawn);
                        ModOptions.Constants.SetOverdrivePawnList(odPawns);
                    }
                    this.overdrivePowerOutput = 0;
                    this.overdriveBuilding = null;
                }
            }
        }

        public void ResolveChronomancerTimeMark()
        {
            if(this.recallExpiration <= Find.TickManager.TicksGame)
            {
                this.recallSet = false;
            }
            if(this.recallSet && !this.recallSpell)
            {
                this.AddPawnAbility(TorannMagicDefOf.TM_Recall);
                this.recallSpell = true;
            }
            if(this.recallSpell && (!this.recallSet || this.recallPosition == default(IntVec3)))
            {
                this.recallSpell = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_Recall);
            }
        }

        public void ResolveSustainers()
        {
            if(this.stoneskinPawns.Count() > 0)
            {
                if(!this.dispelStoneskin)
                {
                    this.dispelStoneskin = true;
                    this.AddPawnAbility(TorannMagicDefOf.TM_DispelStoneskin);
                }
                for(int i = 0; i < this.stoneskinPawns.Count(); i++)
                {
                    if(this.stoneskinPawns[i].DestroyedOrNull() || this.stoneskinPawns[i].Dead)
                    {
                        this.stoneskinPawns.Remove(this.stoneskinPawns[i]);
                    }
                    else
                    {
                        if (!this.stoneskinPawns[i].health.hediffSet.HasHediff(HediffDef.Named("TM_StoneskinHD"), false))
                        {
                            this.stoneskinPawns.Remove(this.stoneskinPawns[i]);
                        }
                    }
                }
            }
            else if(this.dispelStoneskin)
            {
                this.dispelStoneskin = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_DispelStoneskin);
            }

            if (this.summonedLights.Count > 0 && dismissSunlightSpell == false)
            {
                this.AddPawnAbility(TorannMagicDefOf.TM_DismissSunlight);
                dismissSunlightSpell = true;
            }

            if (this.summonedLights.Count <= 0 && dismissSunlightSpell == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_DismissSunlight);
                dismissSunlightSpell = false;
            }

            if (this.summonedPowerNodes.Count > 0 && this.dismissPowerNodeSpell == false)
            {
                this.AddPawnAbility(TorannMagicDefOf.TM_DismissPowerNode);
                dismissPowerNodeSpell = true;
            }

            if (this.summonedPowerNodes.Count <= 0 && dismissPowerNodeSpell == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_DismissPowerNode);
                dismissPowerNodeSpell = false;
            }

            if (this.summonedCoolers.Count > 0 && dismissCoolerSpell == false)
            {
                this.AddPawnAbility(TorannMagicDefOf.TM_DismissCooler);
                dismissCoolerSpell = true;
            }

            if (this.summonedCoolers.Count <= 0 && dismissCoolerSpell == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_DismissCooler);
                dismissCoolerSpell = false;
            }

            if (this.summonedHeaters.Count > 0 && dismissHeaterSpell == false)
            {
                this.AddPawnAbility(TorannMagicDefOf.TM_DismissHeater);
                dismissHeaterSpell = true;
            }

            if (this.summonedHeaters.Count <= 0 && dismissHeaterSpell == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_DismissHeater);
                dismissHeaterSpell = false;
            }

            if (this.enchanterStones.Count > 0 && this.dismissEnchanterStones == false)
            {
                this.AddPawnAbility(TorannMagicDefOf.TM_DismissEnchanterStones);
                dismissEnchanterStones = true;
            }
            if (this.enchanterStones.Count <= 0 && this.dismissEnchanterStones == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_DismissEnchanterStones);
                dismissEnchanterStones= false;
            }

            if (this.lightningTraps.Count > 0 && this.dismissLightningTrap == false)
            {
                this.AddPawnAbility(TorannMagicDefOf.TM_DismissLightningTrap);
                dismissLightningTrap = true;
            }
            if (this.lightningTraps.Count <= 0 && this.dismissLightningTrap == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_DismissLightningTrap);
                dismissLightningTrap = false;
            }

            if (this.summonedSentinels.Count > 0 && this.shatterSentinel == false)
            {
                this.AddPawnAbility(TorannMagicDefOf.TM_ShatterSentinel);
                shatterSentinel = true;
            }
            if (this.summonedSentinels.Count <= 0 && this.shatterSentinel == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_ShatterSentinel);
                shatterSentinel = false;
            }

            if (this.soulBondPawn.DestroyedOrNull() && (this.spell_ShadowStep == true || this.spell_ShadowCall == true))
            {
                this.soulBondPawn = null;
                this.spell_ShadowCall = false;
                this.spell_ShadowStep = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_ShadowCall);
                this.RemovePawnAbility(TorannMagicDefOf.TM_ShadowStep);
            }
            if(this.soulBondPawn != null)
            {
                if(this.spell_ShadowStep == false)
                {
                    this.spell_ShadowStep = true;
                    this.InitializeSpell();
                }
                if(this.spell_ShadowCall == false)
                {
                    this.spell_ShadowCall = true;
                    this.InitializeSpell();
                }
            }

            if(this.weaponEnchants != null && this.weaponEnchants.Count > 0)
            {
                for(int i =0; i < this.weaponEnchants.Count; i++)
                {
                    Pawn ewPawn = weaponEnchants[i];
                    if(ewPawn.DestroyedOrNull() || ewPawn.Dead)
                    {
                        this.weaponEnchants.Remove(ewPawn);
                    }
                }

                if(this.dispelEnchantWeapon == false)
                {
                    this.dispelEnchantWeapon = true;
                    this.AddPawnAbility(TorannMagicDefOf.TM_DispelEnchantWeapon);
                }
            }
            else if(this.dispelEnchantWeapon == true)
            {
                this.dispelEnchantWeapon = false;
                this.RemovePawnAbility(TorannMagicDefOf.TM_DispelEnchantWeapon);
            }

            if(this.mageLightActive)
            {
                if (this.Pawn.Map == null && this.mageLightSet)
                {
                    this.mageLightActive = false;
                    this.mageLightThing = null;
                    this.mageLightSet = false;                    
                }
                Hediff hediff = this.Pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_MageLightHD);
                if(hediff == null && !mageLightSet)
                {
                    HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_MageLightHD, .5f);
                }
                if(mageLightSet && this.mageLightThing == null)
                {
                    this.mageLightActive = false;
                }
            }
            else
            {
                Hediff hediff = this.Pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_MageLightHD);
                if(hediff != null)
                {
                    this.Pawn.health.RemoveHediff(hediff);
                }
                if(!this.mageLightThing.DestroyedOrNull())
                {
                    this.mageLightThing.Destroy(DestroyMode.Vanish);
                    this.mageLightThing = null;
                }
                this.mageLightSet = false;
            }
        }

        public void ResolveMinions()
        {
            if(this.summonedMinions.Count > 0 && dismissMinionSpell == false)
            {
                this.AddPawnAbility(TorannMagicDefOf.TM_DismissMinion);
                dismissMinionSpell = true;
            }

            if(this.summonedMinions.Count <= 0 && dismissMinionSpell == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_DismissMinion);
                dismissMinionSpell = false;                    
            }

            if (this.summonedMinions.Count > 0)
            {
                for (int i = 0; i < this.summonedMinions.Count(); i++)
                {
                    Pawn minion = this.summonedMinions[i] as Pawn;
                    if (minion != null)
                    {
                        if (minion.DestroyedOrNull() || minion.Dead)
                        {
                            this.summonedMinions.Remove(this.summonedMinions[i]);
                            i--;
                        }
                    }
                    else
                    {
                        this.summonedMinions.Remove(this.summonedMinions[i]);
                        i--;
                    }
                }
            }

            if (this.earthSpriteType != 0 && dismissEarthSpriteSpell == false)
            {
                this.AddPawnAbility(TorannMagicDefOf.TM_DismissEarthSprites);
                dismissEarthSpriteSpell = true;
            }

            if (this.earthSpriteType == 0 && dismissEarthSpriteSpell == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_DismissEarthSprites);
                dismissEarthSpriteSpell = false;
            }
        }

        public void ResolveMana()
        {
            bool flag = this.Mana == null;
            if (flag)
            {
                Hediff firstHediffOfDef = base.AbilityUser.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_MagicUserHD, false);
                bool flag2 = firstHediffOfDef != null;
                if (flag2)
                {
                    firstHediffOfDef.Severity = 1f;
                }
                else
                {
                    Hediff hediff = HediffMaker.MakeHediff(TorannMagicDefOf.TM_MagicUserHD, base.AbilityUser, null);
                    hediff.Severity = 1f;
                    base.AbilityUser.health.AddHediff(hediff, null, null);
                }
            }
        }
        public void ResolveMagicPowers()
        {
            bool flag = this.magicPowersInitialized;
            if (!flag)
            {
                this.magicPowersInitialized = true;
            }
        }
        public void ResolveMagicTab()
        {
            if (!this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
            {
                InspectTabBase inspectTabsx = base.AbilityUser.GetInspectTabs().FirstOrDefault((InspectTabBase x) => x.labelKey == "TM_TabMagic");
                IEnumerable<InspectTabBase> inspectTabs = base.AbilityUser.GetInspectTabs();
                bool flag = inspectTabs != null && inspectTabs.Count<InspectTabBase>() > 0;
                if (flag)
                {
                    if (inspectTabsx == null)
                    {
                        try
                        {
                            base.AbilityUser.def.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(typeof(ITab_Pawn_Magic)));
                        }
                        catch (Exception ex)
                        {
                            Log.Error(string.Concat(new object[]
                            {
                            "Could not instantiate inspector tab of type ",
                            typeof(ITab_Pawn_Magic),
                            ": ",
                            ex
                            }));
                        }
                    }
                }
            }
        }        

        public void ResolveClassSkills()
        {
            bool flagCM = this.Pawn.story.traits.HasTrait(TorannMagicDefOf.ChaosMage);

            if(this.Pawn.story.traits.HasTrait(TorannMagicDefOf.BloodMage))
            {
                if (!this.Pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_BloodHD")))
                {
                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_BloodHD"), .1f);
                    for (int i = 0; i < 4; i++)
                    {
                        TM_MoteMaker.ThrowBloodSquirt(this.Pawn.DrawPos, this.Pawn.Map, Rand.Range(.5f, .8f));
                    }
                }
            }

            if(this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Chronomancer) || flagCM)
            {
                if(this.predictionIncidentDef != null && (this.predictionTick + 2500) < Find.TickManager.TicksGame)
                {
                    this.predictionIncidentDef = null;
                }
            }

            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Enchanter) || flagCM)
            {
                if (this.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchantedBody).learned && this.spell_EnchantedAura == false)
                {
                    this.spell_EnchantedAura = true;
                    this.InitializeSpell();
                }

                if(this.MagicData.MagicPowerSkill_Shapeshift.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Shapeshift_ver").level >= 3 && this.spell_ShapeshiftDW != true)
                {
                    this.spell_ShapeshiftDW = true;
                    this.InitializeSpell();
                }
            }

            if(this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer) || flagCM)
            {
                if (this.HasTechnoBit)
                {
                    if (!this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_TechnoBitHD))
                    {
                        HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_TechnoBitHD, .5f);
                        Vector3 bitDrawPos = this.Pawn.DrawPos;
                        bitDrawPos.x -= .5f;
                        bitDrawPos.z += .45f;
                        for (int i = 0; i < 4; i++)
                        {
                            MoteMaker.ThrowSmoke(bitDrawPos, this.Pawn.Map, Rand.Range(.6f, .8f));
                        }
                    }
                }
                if(this.HasTechnoWeapon && this.Pawn.equipment.Primary != null)
                {
                    if(this.Pawn.equipment.Primary.def.defName.Contains("TM_TechnoWeapon_Base") && this.Pawn.equipment.Primary.def.Verbs.FirstOrDefault().range < 2)
                    {
                        TM_Action.DoAction_TechnoWeaponCopy(this.Pawn, this.technoWeaponThing, this.technoWeaponThingDef);                                               
                    }

                    if(!this.Pawn.equipment.Primary.def.defName.Contains("TM_TechnoWeapon_Base") && (this.technoWeaponThing != null || this.technoWeaponThingDef != null))
                    {
                        this.technoWeaponThing = null;
                        this.technoWeaponThingDef = null;
                    }
                }
            }

            if(this.MagicUserLevel >= 20 && this.spell_Teach == false)
            {
                this.AddPawnAbility(TorannMagicDefOf.TM_TeachMagic);
                this.spell_Teach = true;
            }

            if((this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Geomancer) || flagCM) && this.earthSpriteType != 0 && this.earthSprites.IsValid)
            {
                if (this.nextEarthSpriteAction < Find.TickManager.TicksGame)
                {
                    ResolveEarthSpriteAction();
                }

                if (this.nextEarthSpriteMote < Find.TickManager.TicksGame)
                {
                    this.nextEarthSpriteMote += Rand.Range(10, 16);
                    Vector3 shiftLoc = this.earthSprites.ToVector3Shifted();
                    shiftLoc.x += Rand.Range(-.3f, .3f);
                    shiftLoc.z += Rand.Range(-.3f, .3f);
                    TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_Twinkle, shiftLoc, this.Pawn.Map, Rand.Range(.4f, 1f), .05f, Rand.Range(.2f, .5f), Rand.Range(.2f, .5f), Rand.Range(-100, 100), Rand.Range(0f, .3f), Rand.Range(0, 360), 0);
                }
            }

            if(this.summonedSentinels.Count > 0)
            {
                for(int i = 0; i < this.summonedSentinels.Count(); i++)
                {
                    if(summonedSentinels[i].DestroyedOrNull())
                    {
                        this.summonedSentinels.Remove(this.summonedSentinels[i]);
                    }
                }
            }

            if (this.lightningTraps.Count > 0)
            {
                for (int i = 0; i < this.lightningTraps.Count(); i++)
                {
                    if (lightningTraps[i].DestroyedOrNull())
                    {
                        this.lightningTraps.Remove(this.lightningTraps[i]);
                    }
                }
            }

            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Lich))
            {
                if (!this.Pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_LichHD")))
                {
                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_LichHD"), .5f);
                }
                if(this.spell_Flight != true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_DeathBolt);
                    this.AddPawnAbility(TorannMagicDefOf.TM_DeathBolt);
                    this.spell_Flight = true;
                    this.InitializeSpell();
                }
            }

            if (this.IsMagicUser && !this.Pawn.Dead && !this.Pawn.Downed)
            {
                if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Bard))
                {
                    MagicPowerSkill bardtraining_pwr = this.Pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_BardTraining.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BardTraining_pwr");

                    List<Trait> traits = this.Pawn.story.traits.allTraits;
                    for (int i = 0; i < traits.Count; i++)
                    {
                        if (traits[i].def.defName == "TM_Bard")
                        {
                            if (traits[i].Degree != bardtraining_pwr.level)
                            {
                                traits.Remove(traits[i]);
                                this.Pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Bard, bardtraining_pwr.level, false));
                                MoteMaker.ThrowHeatGlow(this.Pawn.Position, this.Pawn.Map, 2);
                            }
                        }
                    }
                }

                if(this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus) || this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Warlock))
                {
                    if(this.soulBondPawn != null)
                    {
                        if(!this.soulBondPawn.Spawned)
                        {
                            this.RemovePawnAbility(TorannMagicDefOf.TM_SummonDemon);
                            this.spell_SummonDemon = false;
                        }
                        else if(this.soulBondPawn.health.hediffSet.HasHediff(HediffDef.Named("TM_DemonicPriceHD"), false))
                        {
                            if(this.spell_SummonDemon == true)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_SummonDemon);
                                this.spell_SummonDemon = false;
                            }
                        }
                        else if (this.soulBondPawn.health.hediffSet.HasHediff(HediffDef.Named("TM_SoulBondMentalHD")) && this.soulBondPawn.health.hediffSet.HasHediff(HediffDef.Named("TM_SoulBondPhysicalHD")))
                        {
                            if(this.spell_SummonDemon == false)
                            {
                                this.AddPawnAbility(TorannMagicDefOf.TM_SummonDemon);
                                this.spell_SummonDemon = true;
                            }
                        }
                        else
                        {
                            if (this.spell_SummonDemon == true)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_SummonDemon);
                                this.spell_SummonDemon = false;
                            }
                        }
                    }
                    else if(this.spell_SummonDemon == true)
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_SummonDemon);
                        this.spell_SummonDemon = false;
                    }
                }
            }

            if(this.IsMagicUser && !this.Pawn.Dead & !this.Pawn.Downed && this.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Bard))
            {
                if(!this.Pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_InspirationalHD")) && this.Pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowersB[2].learned)
                {
                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_InspirationalHD"), 0.95f);
                }
            }
        }

        public void ResolveEnchantments()
        {
            float _maxMPUpkeep = 0;
            float _mpRegenRateUpkeep = 0;
            float _maxMP = 0;
            float _mpRegenRate = 0;
            float _coolDown = 0;
            float _xpGain = 0;
            float _mpCost = 0;
            float _arcaneRes = 0;
            float _arcaneDmg = 0;
            bool _arcaneSpectre = false;
            bool _phantomShift = false;
            float _arcalleumCooldown = 0f;
            List<Apparel> apparel = this.Pawn.apparel.WornApparel;
            if (apparel != null)
            {
                for (int i = 0; i < this.Pawn.apparel.WornApparelCount; i++)
                {
                    Enchantment.CompEnchantedItem item = apparel[i].GetComp<Enchantment.CompEnchantedItem>();
                    if (item != null)
                    {
                        if (item.HasEnchantment)
                        {
                            if (apparel[i].Stuff != null && apparel[i].Stuff.defName == "TM_Manaweave")
                            {
                                _maxMP += item.maxMP * 1.2f;
                                _mpRegenRate += item.mpRegenRate * 1.2f;
                                _coolDown += item.coolDown * 1.2f;
                                _xpGain += item.xpGain * 1.2f;
                                _mpCost += item.mpCost * 1.2f;
                                _arcaneRes += item.arcaneRes * 1.2f;
                                _arcaneDmg += item.arcaneDmg * 1.2f;
                            }                            
                            else
                            {
                                _maxMP += item.maxMP;
                                _mpRegenRate += item.mpRegenRate;
                                _coolDown += item.coolDown;
                                _xpGain += item.xpGain;
                                _mpCost += item.mpCost;
                                _arcaneRes += item.arcaneRes;
                                _arcaneDmg += item.arcaneDmg;
                            }
                            if (item.arcaneSpectre == true)
                            {
                                _arcaneSpectre = true;
                            }
                            if (item.phantomShift == true)
                            {
                                _phantomShift = true;
                            }
                        }
                        if (apparel[i].Stuff != null && apparel[i].Stuff.defName == "TM_Arcalleum")
                        {
                            _arcaneRes += .05f;
                            _arcalleumCooldown += (apparel[i].def.BaseMass * .01f);
                        }

                    }
                }
            }
            if (this.Pawn.equipment != null && this.Pawn.equipment.Primary != null)
            {
                Enchantment.CompEnchantedItem item = this.Pawn.equipment.Primary.GetComp<Enchantment.CompEnchantedItem>();
                if (item != null)
                {
                    if (item.HasEnchantment)
                    {
                        _maxMP += item.maxMP;
                        _mpRegenRate += item.mpRegenRate;
                        _coolDown += item.coolDown;
                        _xpGain += item.xpGain;
                        _mpCost += item.mpCost;
                        _arcaneRes += item.arcaneRes;
                        _arcaneDmg += item.arcaneDmg;
                    }
                    if (Pawn.equipment.Primary.Stuff != null && Pawn.equipment.Primary.Stuff.defName == "TM_Arcalleum")
                    {
                        _arcaneDmg += .1f;
                        _arcalleumCooldown += (this.Pawn.equipment.Primary.def.BaseMass * .01f);
                    }
                }
                if(this.Pawn.equipment.Primary.def.defName == "TM_DefenderStaff")
                {
                    if(this.item_StaffOfDefender == false)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_ArcaneBarrier);
                        this.item_StaffOfDefender = true;
                    }
                }
                else
                {
                    if(this.item_StaffOfDefender == true)
                    {
                        this.RemovePawnAbility(TorannMagicDefOf.TM_ArcaneBarrier);
                        this.item_StaffOfDefender = false;
                    }
                }
            }
            CleanupSummonedStructures();            

            if (this.summonedLights.Count > 0)
            {                
                _maxMPUpkeep -= (this.summonedLights.Count * .4f);
                _mpRegenRateUpkeep -= (this.summonedLights.Count * .4f);
            }
            if (this.summonedHeaters.Count > 0)
            {                
                _maxMPUpkeep -= (this.summonedHeaters.Count * .25f);
            }
            if (this.summonedCoolers.Count > 0)
            {                
                _maxMPUpkeep -= (this.summonedCoolers.Count * .25f);
            }
            if (this.summonedPowerNodes.Count > 0)
            {                
                _maxMPUpkeep -= (this.summonedPowerNodes.Count * .25f);
                _mpRegenRateUpkeep -= (this.summonedPowerNodes.Count * .25f);
            }
            if(this.weaponEnchants.Count > 0)
            {
                _maxMPUpkeep -= (this.weaponEnchants.Count * ActualManaCost(TorannMagicDefOf.TM_EnchantWeapon));
            }
            if(this.enchanterStones != null && this.enchanterStones.Count > 0)
            {
                for(int i = 0; i < this.enchanterStones.Count; i++)
                {
                    if(this.enchanterStones[i].DestroyedOrNull())
                    {
                        this.enchanterStones.Remove(this.enchanterStones[i]);
                    }
                }
                _maxMPUpkeep -= (.20f - (.02f * this.MagicData.MagicPowerSkill_EnchanterStone.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EnchanterStone_eff").level)) * this.enchanterStones.Count;
            }
            try
            {
                if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Druid) && this.fertileLands.Count > 0)
                {
                    _mpRegenRateUpkeep += -.4f;
                }
            }
            catch
            {
                
            }            
            if(this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Lich))
            {
                if(this.spell_LichForm == true)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_LichForm);
                    this.spell_LichForm = false;
                }
                _maxMP += .5f;
                _mpRegenRate += .5f;
            }
            if(this.Pawn.Inspired && this.Pawn.Inspiration.def == TorannMagicDefOf.ID_ManaRegen)
            {
                _mpRegenRate += 1f;
            }
            if(this.Pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_EntertainingHD"), false))
            {
                _maxMPUpkeep += -.3f;
            }
            if (this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PredictionHD, false))
            {
                _mpRegenRateUpkeep += -.5f * (1 - (.10f * this.MagicData.MagicPowerSkill_Prediction.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Prediction_eff").level));
            }
            if(this.recallSet)
            {
                _maxMPUpkeep += -.4f * (1 - (.08f * this.MagicData.MagicPowerSkill_Recall.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Recall_eff").level));
                _mpRegenRateUpkeep += -.3f * (1 - (.08f * this.MagicData.MagicPowerSkill_Recall.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Recall_eff").level));
            }
            if (this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Shadow_AuraHD, false))
            {
                _maxMPUpkeep += -.4f * (1 - (.08f * this.MagicData.MagicPowerSkill_Shadow.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Shadow_eff").level));
                _mpRegenRateUpkeep += -.3f * (1 - (.08f * this.MagicData.MagicPowerSkill_Shadow.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Shadow_eff").level));
            }
            if (this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_RayOfHope_AuraHD, false))
            {
                _maxMPUpkeep += -.4f * (1 - (.08f * this.MagicData.MagicPowerSkill_RayofHope.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_RayofHope_eff").level));
                _mpRegenRateUpkeep += -.3f * (1 - (.08f * this.MagicData.MagicPowerSkill_RayofHope.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_RayofHope_eff").level));
            }
            if (this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_SoothingBreeze_AuraHD, false))
            {
                _maxMPUpkeep += -.4f * (1 - (.08f * this.MagicData.MagicPowerSkill_Soothe.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Soothe_eff").level));
                _mpRegenRateUpkeep += -.3f * (1 - (.08f * this.MagicData.MagicPowerSkill_Soothe.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Soothe_eff").level));
            }
            if (this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_EnchantedAuraHD) || this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_EnchantedBodyHD))
            {                
                _maxMPUpkeep += -.3f + (.045f * this.MagicData.MagicPowerSkill_EnchantedBody.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EnchantedBody_eff").level);
                _mpRegenRateUpkeep += -.5f + (.045f * this.MagicData.MagicPowerSkill_EnchantedBody.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EnchantedBody_ver").level);
            }
            if(this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Geomancer) || this.Pawn.story.traits.HasTrait(TorannMagicDefOf.ChaosMage))
            {
                MagicPowerSkill familiarSkin = this.MagicData.MagicPowerSkill_Stoneskin.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Stoneskin_eff");
                MagicPowerSkill sharedSkin = this.MagicData.MagicPowerSkill_Stoneskin.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Stoneskin_ver");
                _maxMPUpkeep += -((.15f - (.02f * familiarSkin.level)) * this.stoneskinPawns.Count());
                //if(this.earthSpriteType != 0) //this is handled within mana gain function
                //{
                //    _mpRegenRate += -(.6f - (.07f * sharedSkin.level));
                //}

                MagicPowerSkill heartofstone = this.MagicData.MagicPowerSkill_Sentinel.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Sentinel_eff");

                if(heartofstone.level == 3)
                {
                    _maxMPUpkeep += -(.15f * this.summonedSentinels.Count);
                }
                else
                {
                    _maxMPUpkeep += -((.2f - (.02f * heartofstone.level)) * this.summonedSentinels.Count);
                }
            }
            if(this.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Wanderer) || this.Pawn.story.traits.HasTrait(TorannMagicDefOf.ChaosMage))
            {
                _arcaneDmg += (.01f * this.MagicData.MagicPowerSkill_WandererCraft.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_WandererCraft_pwr").level);
                _arcaneRes += (.02f * this.MagicData.MagicPowerSkill_WandererCraft.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_WandererCraft_pwr").level);
                _mpCost -= (.01f * this.MagicData.MagicPowerSkill_WandererCraft.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_WandererCraft_eff").level);
                _xpGain += (.02f * this.MagicData.MagicPowerSkill_WandererCraft.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_WandererCraft_eff").level);
                _coolDown -= (.01f * this.MagicData.MagicPowerSkill_WandererCraft.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_WandererCraft_ver").level);
                _mpRegenRate += (.01f * this.MagicData.MagicPowerSkill_WandererCraft.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_WandererCraft_ver").level);
                _maxMP += (.02f * this.MagicData.MagicPowerSkill_WandererCraft.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_WandererCraft_ver").level);
            }
            if(this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_MageLightHD))
            {
                _maxMPUpkeep += -.1f;
                _mpRegenRateUpkeep += -.05f;
            }
            if (this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_SS_SerumHD))
            {
                Hediff def = this.Pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_SS_SerumHD, false);
                _mpRegenRate -= (float)(.15f * def.CurStageIndex);
                _maxMP -= .25f;
                _arcaneRes += (float)(.15f * def.CurStageIndex);
                _arcaneDmg -= (float)(.1f * def.CurStageIndex);
            }
            if (this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_BlurHD))
            {
                _maxMPUpkeep += -.2f;
            }
            MagicPowerSkill spirit = this.MagicData.MagicPowerSkill_global_spirit.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_spirit_pwr");
            MagicPowerSkill clarity = this.MagicData.MagicPowerSkill_global_regen.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_regen_pwr");
            MagicPowerSkill focus = this.MagicData.MagicPowerSkill_global_eff.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_eff_pwr");
            _maxMP += (spirit.level * .04f);
            _mpRegenRate += (clarity.level * .05f);
            _mpCost += (focus.level * -.025f);
            _arcaneRes += ((1 - this.Pawn.GetStatValue(StatDefOf.PsychicSensitivity, false)) / 2);
            _arcaneDmg += ((this.Pawn.GetStatValue(StatDefOf.PsychicSensitivity, false) - 1) / 4);
            if(this.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_ArcaneConduitTD))
            {
                _mpRegenRate += .4f;
                _maxMP -= .2f;
            }
            if(this.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_ManaWellTD))
            {
                _mpRegenRate -= .2f;
                _maxMP += .4f;
            }
            float val = 1f;
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Wanderer) || this.Pawn.story.traits.HasTrait(TorannMagicDefOf.ChaosMage))
            {
               val = (1f - (.03f * this.MagicData.MagicPowerSkill_Cantrips.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Cantrips_eff").level));                
            }
            _maxMP = _maxMP + (_maxMPUpkeep * val);
            _mpRegenRate = _mpRegenRate + (_mpRegenRateUpkeep * val);

            this.maxMP = 1f + _maxMP;
            this.mpRegenRate = 1f +  _mpRegenRate;
            this.coolDown = Mathf.Clamp(1f + _coolDown, 0.25f, 10f);
            this.xpGain = 1f + _xpGain;
            this.mpCost = 1f + _mpCost;
            this.arcaneRes = 1 + _arcaneRes;
            this.arcaneDmg = 1 + _arcaneDmg;
            this.arcalleumCooldown = Mathf.Clamp(0f + _arcalleumCooldown, 0f, .5f);
            if (this.IsMagicUser && !TM_Calc.IsCrossClass(this.Pawn, true))
            {
                if (_maxMP != 0)
                {
                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_maxMP"), .5f);
                }
                if (_mpRegenRate != 0)
                {
                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_mpRegenRate"), .5f);
                }
                if (_coolDown != 0)
                {
                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_coolDown"), .5f);
                }
                if (_xpGain != 0)
                {
                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_xpGain"), .5f);
                }
                if (_mpCost != 0)
                {
                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_mpCost"), .5f);
                }
                if (_arcaneRes != 0)
                {
                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_arcaneRes"), .5f);
                }
                if (_arcaneDmg != 0)
                {
                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_arcaneDmg"), .5f);
                }
                if (_arcaneSpectre == true)
                {
                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_arcaneSpectre"), .5f);
                }
                if (_phantomShift == true)
                {
                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_phantomShift"), .5f);
                }
                if(_arcalleumCooldown != 0f)
                {
                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_arcalleumCooldown"), .5f);
                }

                using (IEnumerator<Hediff> enumerator = this.Pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Hediff rec = enumerator.Current;
                        if (rec.def.defName == "TM_HediffEnchantment_maxMP" && this.maxMP == 1)
                        {
                            Pawn.health.RemoveHediff(rec);
                        }
                        if (rec.def.defName == "TM_HediffEnchantment_coolDown" && this.coolDown == 1)
                        {
                            Pawn.health.RemoveHediff(rec);
                        }
                        if (rec.def.defName == "TM_HediffEnchantment_mpCost" && this.mpCost == 1)
                        {
                            Pawn.health.RemoveHediff(rec);
                        }
                        if (rec.def.defName == "TM_HediffEnchantment_mpRegenRate" && this.mpRegenRate == 1)
                        {
                            Pawn.health.RemoveHediff(rec);
                        }
                        if (rec.def.defName == "TM_HediffEnchantment_xpGain" && this.xpGain == 1)
                        {
                            Pawn.health.RemoveHediff(rec);
                        }
                        if (rec.def.defName == "TM_HediffEnchantment_arcaneRes" && this.arcaneRes == 1)
                        {
                            Pawn.health.RemoveHediff(rec);
                        }
                        if (rec.def.defName == "TM_HediffEnchantment_arcaneDmg" && this.arcaneDmg == 1)
                        {
                            Pawn.health.RemoveHediff(rec);
                        }
                        if (rec.def.defName == "TM_HediffEnchantment_arcaneSpectre" && _arcaneSpectre == false)
                        {
                            Pawn.health.RemoveHediff(rec);
                        }
                        if (rec.def.defName == "TM_HediffEnchantment_phantomShift" && _phantomShift == false)
                        {
                            Pawn.health.RemoveHediff(rec);
                        }
                        if(rec.def.defName == "TM_HediffEnchantment_phantomShift" && _arcalleumCooldown == 0f)
                        {
                            Pawn.health.RemoveHediff(rec);
                        }
                        if (rec.def.defName == "TM_HediffEnchantment_arcalleumCooldown" && _arcalleumCooldown == 0f)
                        {
                            Pawn.health.RemoveHediff(rec);
                        }
                    }
                }
            }
        }

        private void CleanupSummonedStructures()
        {
            for (int i = 0; i < this.summonedLights.Count; i++)
            {
                if (this.summonedLights[i].DestroyedOrNull())
                {
                    this.summonedLights.Remove(this.summonedLights[i]);
                    i--;
                }
            }
            for (int i = 0; i < this.summonedHeaters.Count; i++)
            {
                if (this.summonedHeaters[i].DestroyedOrNull())
                {
                    this.summonedHeaters.Remove(this.summonedHeaters[i]);
                    i--;
                }
            }
            for (int i = 0; i < this.summonedCoolers.Count; i++)
            {
                if (this.summonedCoolers[i].DestroyedOrNull())
                {
                    this.summonedCoolers.Remove(this.summonedCoolers[i]);
                    i--;
                }
            }
            for (int i = 0; i < this.summonedPowerNodes.Count; i++)
            {
                if (this.summonedPowerNodes[i].DestroyedOrNull())
                {
                    this.summonedPowerNodes.Remove(this.summonedPowerNodes[i]);
                    i--;
                }
            }
            for (int i = 0; i < this.lightningTraps.Count; i++)
            {
                if (this.lightningTraps[i].DestroyedOrNull())
                {
                    this.lightningTraps.Remove(this.lightningTraps[i]);
                    i--;
                }
            }
        }

        public override void PostExposeData()
        {
            //base.PostExposeData();            
            Scribe_Values.Look<bool>(ref this.magicPowersInitialized, "magicPowersInitialized", false, false);
            Scribe_Values.Look<bool>(ref this.magicPowersInitializedForColonist, "magicPowersInitializedForColonist", true, false);
            Scribe_Values.Look<bool>(ref this.colonistPowerCheck, "colonistPowerCheck", true, false);
            Scribe_Values.Look<bool>(ref this.spell_Rain, "spell_Rain", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Blink, "spell_Blink", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Teleport, "spell_Teleport", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Heal, "spell_Heal", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Heater, "spell_Heater", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Cooler, "spell_Cooler", false, false);
            Scribe_Values.Look<bool>(ref this.spell_PowerNode, "spell_PowerNode", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Sunlight, "spell_Sunlight", false, false);
            Scribe_Values.Look<bool>(ref this.spell_DryGround, "spell_DryGround", false, false);
            Scribe_Values.Look<bool>(ref this.spell_WetGround, "spell_WetGround", false, false);
            Scribe_Values.Look<bool>(ref this.spell_ChargeBattery, "spell_ChargeBattery", false, false);
            Scribe_Values.Look<bool>(ref this.spell_SmokeCloud, "spell_SmokeCloud", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Extinguish, "spell_Extinguish", false, false);
            Scribe_Values.Look<bool>(ref this.spell_EMP, "spell_EMP", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Blizzard, "spell_Blizzard", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Firestorm, "spell_Firestorm", false, false);
            Scribe_Values.Look<bool>(ref this.spell_EyeOfTheStorm, "spell_EyeOfTheStorm", false, false);
            Scribe_Values.Look<bool>(ref this.spell_SummonMinion, "spell_SummonMinion", false, false);
            Scribe_Values.Look<bool>(ref this.spell_TransferMana, "spell_TransferMana", false, false);
            Scribe_Values.Look<bool>(ref this.spell_SiphonMana, "spell_SiphonMana", false, false);
            Scribe_Values.Look<bool>(ref this.spell_RegrowLimb, "spell_RegrowLimb", false, false);
            Scribe_Values.Look<bool>(ref this.spell_ManaShield, "spell_ManaShield", false, false);
            Scribe_Values.Look<bool>(ref this.spell_FoldReality, "spell_FoldReality", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Resurrection, "spell_Resurrection", false, false);
            Scribe_Values.Look<bool>(ref this.spell_HolyWrath, "spell_HolyWrath", false, false);
            Scribe_Values.Look<bool>(ref this.spell_LichForm, "spell_LichForm", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Flight, "spell_Flight", false, false);
            Scribe_Values.Look<bool>(ref this.spell_SummonPoppi, "spell_SummonPoppi", false, false);
            Scribe_Values.Look<bool>(ref this.spell_BattleHymn, "spell_BattleHymn", false, false);
            Scribe_Values.Look<bool>(ref this.spell_FertileLands, "spell_FertileLands", false, false);
            Scribe_Values.Look<bool>(ref this.spell_CauterizeWound, "spell_CauterizeWound", false, false);
            Scribe_Values.Look<bool>(ref this.spell_SpellMending, "spell_SpellMending", false, false);
            Scribe_Values.Look<bool>(ref this.spell_PsychicShock, "spell_PsychicShock", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Scorn, "spell_Scorn", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Meteor, "spell_Meteor", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Teach, "spell_Teach", false, false);
            Scribe_Values.Look<bool>(ref this.spell_OrbitalStrike, "spell_OrbitalStrike", false, false);
            Scribe_Values.Look<bool>(ref this.spell_BloodMoon, "spell_BloodMoon", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Shapeshift, "spell_Shapeshift", false, false);
            Scribe_Values.Look<bool>(ref this.spell_ShapeshiftDW, "spell_ShapeshiftDW", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Blur, "spell_Blur", false, false);
            Scribe_Values.Look<bool>(ref this.spell_BlankMind, "spell_BlankMind", false, false);
            Scribe_Values.Look<bool>(ref this.spell_DirtDevil, "spell_DirtDevil", false, false);
            Scribe_Values.Look<bool>(ref this.spell_ArcaneBolt, "spell_ArcaneBolt", false, false);
            Scribe_Values.Look<bool>(ref this.spell_LightningTrap, "spell_LightningTrap", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Invisibility, "spell_Invisibility", false, false);
            Scribe_Values.Look<bool>(ref this.spell_BriarPatch, "spell_BriarPatch", false, false);
            Scribe_Values.Look<bool>(ref this.spell_MechaniteReprogramming, "spell_MechaniteReprogramming", false, false);
            Scribe_Values.Look<bool>(ref this.spell_Recall, "spell_Recall", false, false);
            Scribe_Values.Look<bool>(ref this.spell_MageLight, "spell_MageLight", false, false);
            Scribe_Values.Look<bool>(ref this.useTechnoBitToggle, "useTechnoBitToggle", true, false);
            Scribe_Values.Look<bool>(ref this.useTechnoBitRepairToggle, "useTechnoBitRepairToggle", true, false);
            Scribe_Values.Look<bool>(ref this.useElementalShotToggle, "useElementalShotToggle", true, false);
            Scribe_Values.Look<int>(ref this.powerModifier, "powerModifier", 0, false);
            Scribe_Values.Look<int>(ref this.technoWeaponDefNum, "technoWeaponDefNum");
            Scribe_Values.Look<bool>(ref this.doOnce, "doOnce", true, false);
            Scribe_Values.Look<int>(ref this.predictionTick, "predictionTick", 0, false);
            Scribe_References.Look<Thing>(ref this.mageLightThing, "mageLightThing", false);
            Scribe_Values.Look<bool>(ref this.mageLightActive, "mageLightActive", false, false);
            Scribe_Values.Look<bool>(ref this.mageLightSet, "mageLightSet", false, false);
            Scribe_Defs.Look<IncidentDef>(ref this.predictionIncidentDef, "predictionIncidentDef");
            Scribe_References.Look<Pawn>(ref this.soulBondPawn, "soulBondPawn", false);
            Scribe_References.Look<Thing>(ref this.technoWeaponThing, "technoWeaponThing", false);
            Scribe_Defs.Look<ThingDef>(ref this.technoWeaponThingDef, "technoWeaponThingDef");
            Scribe_References.Look<Thing>(ref this.enchanterStone, "enchanterStone", false);
            Scribe_Collections.Look<Thing>(ref this.enchanterStones, "enchanterStones", LookMode.Reference);
            Scribe_Collections.Look<Thing>(ref this.summonedMinions, "summonedMinions", LookMode.Reference);
            Scribe_Collections.Look<Thing>(ref this.summonedLights, "summonedLights", LookMode.Reference);
            Scribe_Collections.Look<Thing>(ref this.summonedPowerNodes, "summonedPowerNodes", LookMode.Reference);
            Scribe_Collections.Look<Thing>(ref this.summonedCoolers, "summonedCoolers", LookMode.Reference);
            Scribe_Collections.Look<Thing>(ref this.summonedHeaters, "summonedHeaters", LookMode.Reference);
            Scribe_Collections.Look<Thing>(ref this.summonedSentinels, "summonedSentinels", LookMode.Reference);
            Scribe_Collections.Look<Pawn>(ref this.stoneskinPawns, "stoneskinPawns", LookMode.Reference);
            Scribe_Collections.Look<Pawn>(ref this.weaponEnchants, "weaponEnchants", LookMode.Reference);
            Scribe_Collections.Look<Thing>(ref this.lightningTraps, "lightningTraps", LookMode.Reference);
            Scribe_Values.Look<IntVec3>(ref this.earthSprites, "earthSprites", default(IntVec3), false);
            Scribe_Values.Look<int>(ref this.earthSpriteType, "earthSpriteType", 0, false);
            Scribe_References.Look<Map>(ref this.earthSpriteMap, "earthSpriteMap", false);
            Scribe_Values.Look<bool>(ref this.earthSpritesInArea, "earthSpritesInArea", false, false);
            Scribe_Values.Look<int>(ref this.nextEarthSpriteAction, "nextEarthSpriteAction", 0, false);
            Scribe_Collections.Look<IntVec3>(ref this.fertileLands, "fertileLands", LookMode.Value);
            Scribe_Values.Look<float>(ref this.maxMP, "maxMP", 1f, false);
            //Scribe_Collections.Look<TM_ChaosPowers>(ref this.chaosPowers, "chaosPowers", LookMode.Deep, new object[0]);
            //Recall variables 
            Scribe_Values.Look<bool>(ref this.recallSet, "recallSet", false, false);
            Scribe_Values.Look<bool>(ref this.recallSpell, "recallSpell", false, false);
            Scribe_Values.Look<int>(ref this.recallExpiration, "recallExpiration", 0, false);
            Scribe_Values.Look<IntVec3>(ref this.recallPosition, "recallPosition", default(IntVec3), false);
            Scribe_References.Look<Map>(ref this.recallMap, "recallMap", false);
            Scribe_Collections.Look<string>(ref this.recallNeedDefnames, "recallNeedDefnames", LookMode.Value);
            Scribe_Collections.Look<float>(ref this.recallNeedValues, "recallNeedValues", LookMode.Value);
            Scribe_Collections.Look<Hediff>(ref this.recallHediffList, "recallHediffList", LookMode.Deep);
            Scribe_Collections.Look<Hediff_Injury>(ref this.recallInjuriesList, "recallInjuriesList", LookMode.Deep);
            //
            Scribe_Deep.Look<MagicData>(ref this.magicData, "magicData", new object[]
            {
                this
            });
            bool flag11 = Scribe.mode == LoadSaveMode.PostLoadInit;
            if (flag11)
            {
                Pawn abilityUser = base.AbilityUser;
                bool flagCM = abilityUser.story.traits.HasTrait(TorannMagicDefOf.ChaosMage);
                bool flag40 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.InnerFire) || flagCM;
                if (flag40)
                {
                    bool flag14 = !this.MagicData.MagicPowersIF.NullOrEmpty<MagicPower>();
                    if (flag14)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current3 in this.MagicData.MagicPowersIF)
                        {
                            bool flag15 = current3.abilityDef != null;
                            if (flag15)
                            {
                                if (current3.learned == true && (current3.abilityDef == TorannMagicDefOf.TM_RayofHope || current3.abilityDef == TorannMagicDefOf.TM_RayofHope_I || current3.abilityDef == TorannMagicDefOf.TM_RayofHope_II || current3.abilityDef == TorannMagicDefOf.TM_RayofHope_III))
                                {
                                    if (current3.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_RayofHope);
                                    }
                                    else if (current3.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_RayofHope_I);
                                    }
                                    else if (current3.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_RayofHope_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_RayofHope_III);
                                    }
                                }                             
                            }
                        }
                    }
                }
                bool flag41 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost) || flagCM;
                if (flag41)
                {
                    bool flag17 = !this.MagicData.MagicPowersHoF.NullOrEmpty<MagicPower>();
                    if (flag17)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current4 in this.MagicData.MagicPowersHoF)
                        {
                            bool flag18 = current4.abilityDef != null;
                            if (flag18)
                            {
                                if (current4.learned == true && (current4.abilityDef == TorannMagicDefOf.TM_Soothe || current4.abilityDef == TorannMagicDefOf.TM_Soothe_I || current4.abilityDef == TorannMagicDefOf.TM_Soothe_II || current4.abilityDef == TorannMagicDefOf.TM_Soothe_III))
                                {
                                    if (current4.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Soothe);
                                    }
                                    else if (current4.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Soothe_I);
                                    }
                                    else if (current4.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Soothe_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Soothe_III);
                                    }
                                }
                                if (current4.learned == true && (current4.abilityDef == TorannMagicDefOf.TM_FrostRay || current4.abilityDef == TorannMagicDefOf.TM_FrostRay_I || current4.abilityDef == TorannMagicDefOf.TM_FrostRay_II || current4.abilityDef == TorannMagicDefOf.TM_FrostRay_III))
                                {
                                    if (current4.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_FrostRay);
                                    }
                                    else if (current4.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_FrostRay_I);
                                    }
                                    else if (current4.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_FrostRay_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_FrostRay_III);
                                    }
                                }
                            }
                        }
                    }
                }
                bool flag42 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.StormBorn) || flagCM;
                if (flag42)
                {
                    bool flag20 = !this.MagicData.MagicPowersSB.NullOrEmpty<MagicPower>();
                    if (flag20)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current5 in this.MagicData.MagicPowersSB)
                        {
                            bool flag21 = current5.abilityDef != null;
                            if (current5.learned == true && (current5.abilityDef == TorannMagicDefOf.TM_AMP || current5.abilityDef == TorannMagicDefOf.TM_AMP_I || current5.abilityDef == TorannMagicDefOf.TM_AMP_II || current5.abilityDef == TorannMagicDefOf.TM_AMP_III))
                            {
                                if (current5.level == 0)
                                {
                                    base.AddPawnAbility(TorannMagicDefOf.TM_AMP);
                                }
                                else if (current5.level == 1)
                                {
                                    base.AddPawnAbility(TorannMagicDefOf.TM_AMP_I);
                                }
                                else if (current5.level == 2)
                                {
                                    base.AddPawnAbility(TorannMagicDefOf.TM_AMP_II);
                                }
                                else
                                {
                                    base.AddPawnAbility(TorannMagicDefOf.TM_AMP_III);
                                }
                            }
                        }
                    }
                }
                bool flag43 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Arcanist) || flagCM;
                if (flag43)
                {
                    bool flag23 = !this.MagicData.MagicPowersA.NullOrEmpty<MagicPower>();
                    if (flag23)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current6 in this.MagicData.MagicPowersA)
                        {
                            bool flag24 = current6.abilityDef != null;
                            if (flag24)
                            {
                                if (current6.learned == true && (current6.abilityDef == TorannMagicDefOf.TM_Shadow || current6.abilityDef == TorannMagicDefOf.TM_Shadow_I || current6.abilityDef == TorannMagicDefOf.TM_Shadow_II || current6.abilityDef == TorannMagicDefOf.TM_Shadow_III))
                                {
                                    if (current6.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Shadow);
                                    }
                                    else if (current6.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Shadow_I);
                                    }
                                    else if (current6.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Shadow_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Shadow_III);
                                    }
                                }
                                if (current6.learned == true && (current6.abilityDef == TorannMagicDefOf.TM_MagicMissile || current6.abilityDef == TorannMagicDefOf.TM_MagicMissile_I || current6.abilityDef == TorannMagicDefOf.TM_MagicMissile_II || current6.abilityDef == TorannMagicDefOf.TM_MagicMissile_III))
                                {
                                    if (current6.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_MagicMissile);
                                    }
                                    else if (current6.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_MagicMissile_I);
                                    }
                                    else if (current6.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_MagicMissile_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_MagicMissile_III);
                                    }
                                }
                                if (current6.learned == true && (current6.abilityDef == TorannMagicDefOf.TM_Blink || current6.abilityDef == TorannMagicDefOf.TM_Blink_I || current6.abilityDef == TorannMagicDefOf.TM_Blink_II || current6.abilityDef == TorannMagicDefOf.TM_Blink_III))
                                {
                                    if (current6.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Blink);
                                    }
                                    else if (current6.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Blink_I);
                                    }
                                    else if (current6.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Blink_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Blink_III);
                                    }
                                }
                                if (current6.learned == true && (current6.abilityDef == TorannMagicDefOf.TM_Summon || current6.abilityDef == TorannMagicDefOf.TM_Summon_I || current6.abilityDef == TorannMagicDefOf.TM_Summon_II || current6.abilityDef == TorannMagicDefOf.TM_Summon_III))
                                {
                                    if (current6.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Summon);
                                    }
                                    else if (current6.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Summon_I);
                                    }
                                    else if (current6.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Summon_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Summon_III);
                                    }
                                }
                            }
                        }
                    }
                }
                bool flag44 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Paladin) || flagCM;
                if (flag44)
                {
                    bool flag26 = !this.MagicData.MagicPowersP.NullOrEmpty<MagicPower>();
                    if (flag26)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current7 in this.MagicData.MagicPowersP)
                        {
                            bool flag27 = current7.abilityDef != null;
                            if (flag27)
                            {
                                if (current7.learned == true && (current7.abilityDef == TorannMagicDefOf.TM_Shield || current7.abilityDef == TorannMagicDefOf.TM_Shield_I || current7.abilityDef == TorannMagicDefOf.TM_Shield_II || current7.abilityDef == TorannMagicDefOf.TM_Shield_III))
                                {
                                    if (current7.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Shield);
                                    }
                                    else if (current7.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Shield_I);
                                    }
                                    else if (current7.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Shield_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Shield_III);
                                    }
                                }
                            }
                        }
                    }
                }
                bool flag45 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Summoner) || flagCM;
                if (flag45)
                {
                    bool flag28 = !this.MagicData.MagicPowersS.NullOrEmpty<MagicPower>();
                    if (flag28)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current8 in this.MagicData.MagicPowersS)
                        {
                            bool flag29 = current8.abilityDef != null;
                            if (flag29)
                            {
                                //if ((current7.abilityDef == TorannMagicDefOf.TM_Shield || current7.abilityDef == TorannMagicDefOf.TM_Shield_I || current7.abilityDef == TorannMagicDefOf.TM_Shield_II || current7.abilityDef == TorannMagicDefOf.TM_Shield_III))
                                //{
                                //    if (current7.level == 0)
                                //    {
                                //        base.AddPawnAbility(TorannMagicDefOf.TM_Shield);
                                //    }
                                //    else if (current7.level == 1)
                                //    {
                                //        base.AddPawnAbility(TorannMagicDefOf.TM_Shield_I);
                                //    }
                                //    else if (current7.level == 2)
                                //    {
                                //        base.AddPawnAbility(TorannMagicDefOf.TM_Shield_II);
                                //    }
                                //    else
                                //    {
                                //        base.AddPawnAbility(TorannMagicDefOf.TM_Shield_III);
                                //    }
                                //}
                            }
                        }
                    }
                }
                bool flag46 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Druid) || flagCM;
                if (flag46)
                {
                    bool flag30 = !this.MagicData.MagicPowersD.NullOrEmpty<MagicPower>();
                    if (flag30)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current9 in this.MagicData.MagicPowersD)
                        {
                            bool flag31 = current9.abilityDef != null;
                            if (flag31)
                            {
                                if (current9.learned == true && (current9.abilityDef == TorannMagicDefOf.TM_SootheAnimal || current9.abilityDef == TorannMagicDefOf.TM_SootheAnimal_I || current9.abilityDef == TorannMagicDefOf.TM_SootheAnimal_II || current9.abilityDef == TorannMagicDefOf.TM_SootheAnimal_III))
                                {
                                    if (current9.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_SootheAnimal);
                                    }
                                    else if (current9.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_SootheAnimal_I);
                                    }
                                    else if (current9.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_SootheAnimal_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_SootheAnimal_III);
                                    }
                                }
                            }
                        }
                    }
                }
                bool flag47 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || abilityUser.story.traits.HasTrait(TorannMagicDefOf.Lich) || flagCM;
                if (flag47)
                {
                    bool flag32 = !this.MagicData.MagicPowersN.NullOrEmpty<MagicPower>();
                    if (flag32)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current10 in this.MagicData.MagicPowersN)
                        {
                            bool flag33 = current10.abilityDef != null;
                            if (flag33)
                            {
                                if (current10.learned == true && (current10.abilityDef == TorannMagicDefOf.TM_DeathMark || current10.abilityDef == TorannMagicDefOf.TM_DeathMark_I || current10.abilityDef == TorannMagicDefOf.TM_DeathMark_II || current10.abilityDef == TorannMagicDefOf.TM_DeathMark_III))
                                {
                                    if (current10.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_DeathMark);
                                    }
                                    else if (current10.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_DeathMark_I);
                                    }
                                    else if (current10.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_DeathMark_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_DeathMark_III);
                                    }
                                }
                                if (current10.learned == true && (current10.abilityDef == TorannMagicDefOf.TM_ConsumeCorpse || current10.abilityDef == TorannMagicDefOf.TM_ConsumeCorpse_I || current10.abilityDef == TorannMagicDefOf.TM_ConsumeCorpse_II || current10.abilityDef == TorannMagicDefOf.TM_ConsumeCorpse_III))
                                {
                                    if (current10.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ConsumeCorpse);
                                    }
                                    else if (current10.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ConsumeCorpse_I);
                                    }
                                    else if (current10.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ConsumeCorpse_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ConsumeCorpse_III);
                                    }
                                }
                                if (current10.learned == true && (current10.abilityDef == TorannMagicDefOf.TM_CorpseExplosion || current10.abilityDef == TorannMagicDefOf.TM_CorpseExplosion_I || current10.abilityDef == TorannMagicDefOf.TM_CorpseExplosion_II || current10.abilityDef == TorannMagicDefOf.TM_CorpseExplosion_III))
                                {
                                    if (current10.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_CorpseExplosion);
                                    }
                                    else if (current10.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_CorpseExplosion_I);
                                    }
                                    else if (current10.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_CorpseExplosion_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_CorpseExplosion_III);
                                    }
                                }
                                if (abilityUser.story.traits.HasTrait(TorannMagicDefOf.Lich) && (current10.learned == true && (current10.abilityDef == TorannMagicDefOf.TM_DeathBolt || current10.abilityDef == TorannMagicDefOf.TM_DeathBolt_I || current10.abilityDef == TorannMagicDefOf.TM_DeathBolt_II || current10.abilityDef == TorannMagicDefOf.TM_DeathBolt_III)))
                                {
                                    if (current10.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_DeathBolt);
                                    }
                                    else if (current10.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_DeathBolt_I);
                                    }
                                    else if (current10.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_DeathBolt_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_DeathBolt_III);
                                    }
                                }
                            }
                        }
                    }
                }
                bool flag48 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Priest) || flagCM;
                if (flag48)
                {
                    bool flag34 = !this.MagicData.MagicPowersPR.NullOrEmpty<MagicPower>();
                    if (flag34)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current11 in this.MagicData.MagicPowersPR)
                        {
                            bool flag33 = current11.abilityDef != null;
                            if (flag33)
                            {
                                if (current11.learned == true && (current11.abilityDef == TorannMagicDefOf.TM_HealingCircle || current11.abilityDef == TorannMagicDefOf.TM_HealingCircle_I || current11.abilityDef == TorannMagicDefOf.TM_HealingCircle_II || current11.abilityDef == TorannMagicDefOf.TM_HealingCircle_III))
                                {
                                    if (current11.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_HealingCircle);
                                    }
                                    else if (current11.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_HealingCircle_I);
                                    }
                                    else if (current11.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_HealingCircle_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_HealingCircle_III);
                                    }
                                }
                                if (current11.learned == true && (current11.abilityDef == TorannMagicDefOf.TM_BestowMight || current11.abilityDef == TorannMagicDefOf.TM_BestowMight_I || current11.abilityDef == TorannMagicDefOf.TM_BestowMight_II || current11.abilityDef == TorannMagicDefOf.TM_BestowMight_III))
                                {
                                    if (current11.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_BestowMight);
                                    }
                                    else if (current11.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_BestowMight_I);
                                    }
                                    else if (current11.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_BestowMight_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_BestowMight_III);
                                    }
                                }                                
                            }
                        }
                    }
                }
                bool flag49 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.TM_Bard) || flagCM;
                if (flag49)
                {
                    bool flag35 = !this.MagicData.MagicPowersB.NullOrEmpty<MagicPower>();
                    if (flag35)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current12 in this.MagicData.MagicPowersB)
                        {
                            bool flag36 = current12.abilityDef != null;
                            if (flag36)
                            {
                                if (current12.learned == true && (current12.abilityDef == TorannMagicDefOf.TM_Lullaby || current12.abilityDef == TorannMagicDefOf.TM_Lullaby_I || current12.abilityDef == TorannMagicDefOf.TM_Lullaby_II || current12.abilityDef == TorannMagicDefOf.TM_Lullaby_III))
                                {
                                    if (current12.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Lullaby);
                                    }
                                    else if (current12.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Lullaby_I);
                                    }
                                    else if (current12.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Lullaby_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Lullaby_III);
                                    }
                                }
                            }
                        }
                    }
                }
                bool flag50 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Succubus) || flagCM;
                if (flag50)
                {
                    bool flag37 = !this.MagicData.MagicPowersSD.NullOrEmpty<MagicPower>();
                    if (flag37)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current13 in this.MagicData.MagicPowersSD)
                        {
                            bool flag38 = current13.abilityDef != null;
                            if (flag38)
                            {
                                if (current13.learned == true && (current13.abilityDef == TorannMagicDefOf.TM_ShadowBolt || current13.abilityDef == TorannMagicDefOf.TM_ShadowBolt_I || current13.abilityDef == TorannMagicDefOf.TM_ShadowBolt_II || current13.abilityDef == TorannMagicDefOf.TM_ShadowBolt_III))
                                {
                                    if (current13.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ShadowBolt);
                                    }
                                    else if (current13.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ShadowBolt_I);
                                    }
                                    else if (current13.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ShadowBolt_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ShadowBolt_III);
                                    }
                                }
                                if (current13.learned == true && (current13.abilityDef == TorannMagicDefOf.TM_Attraction || current13.abilityDef == TorannMagicDefOf.TM_Attraction_I || current13.abilityDef == TorannMagicDefOf.TM_Attraction_II || current13.abilityDef == TorannMagicDefOf.TM_Attraction_III))
                                {
                                    if (current13.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Attraction);
                                    }
                                    else if (current13.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Attraction_I);
                                    }
                                    else if (current13.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Attraction_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Attraction_III);
                                    }
                                }
                            }
                        }
                    }
                }
                bool flag51 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Warlock) || flagCM;
                if (flag51)
                {
                    bool flagWD1 = !this.MagicData.MagicPowersWD.NullOrEmpty<MagicPower>();
                    if (flagWD1)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current14 in this.MagicData.MagicPowersWD)
                        {
                            bool flagWD2 = current14.abilityDef != null;
                            if (flagWD2)
                            {                                
                                if (current14.learned == true && (current14.abilityDef == TorannMagicDefOf.TM_ShadowBolt || current14.abilityDef == TorannMagicDefOf.TM_ShadowBolt_I || current14.abilityDef == TorannMagicDefOf.TM_ShadowBolt_II || current14.abilityDef == TorannMagicDefOf.TM_ShadowBolt_III))
                                {
                                    if (current14.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ShadowBolt);
                                    }
                                    else if (current14.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ShadowBolt_I);
                                    }
                                    else if (current14.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ShadowBolt_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ShadowBolt_III);
                                    }
                                }
                                if (current14.learned == true && (current14.abilityDef == TorannMagicDefOf.TM_Repulsion || current14.abilityDef == TorannMagicDefOf.TM_Repulsion_I || current14.abilityDef == TorannMagicDefOf.TM_Repulsion_II || current14.abilityDef == TorannMagicDefOf.TM_Repulsion_III))
                                {
                                    if (current14.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Repulsion);
                                    }
                                    else if (current14.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Repulsion_I);
                                    }
                                    else if (current14.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Repulsion_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Repulsion_III);
                                    }
                                }
                            }
                        }
                    }
                }
                bool flag52 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Geomancer) || flagCM;
                if (flag52)
                {
                    bool flagG = !this.MagicData.MagicPowersG.NullOrEmpty<MagicPower>();
                    if (flagG)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current15 in this.MagicData.MagicPowersG)
                        {
                            bool flagWD2 = current15.abilityDef != null;
                            if (flagWD2)
                            {
                                if (current15.learned == true && (current15.abilityDef == TorannMagicDefOf.TM_Encase || current15.abilityDef == TorannMagicDefOf.TM_Encase_I || current15.abilityDef == TorannMagicDefOf.TM_Encase_II || current15.abilityDef == TorannMagicDefOf.TM_Encase_III))
                                {
                                    if (current15.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Encase);
                                    }
                                    else if (current15.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Encase_I);
                                    }
                                    else if (current15.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Encase_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Encase_III);
                                    }
                                }                                
                            }
                        }
                    }
                }
                bool flag53 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Technomancer) || flagCM;
                if (flag53)
                {
                    bool flagT = !this.MagicData.MagicPowersT.NullOrEmpty<MagicPower>();
                    if (flagT)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current16 in this.MagicData.MagicPowersT)
                        {
                            bool flagT2 = current16.abilityDef != null;
                            if (flagT2)
                            {

                            }
                        }
                    }
                }
                bool flag54 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.BloodMage);
                if (flag54)
                {
                    bool flagBM = !this.MagicData.MagicPowersBM.NullOrEmpty<MagicPower>();
                    if (flagBM)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current16 in this.MagicData.MagicPowersBM)
                        {
                            bool flagBM2 = current16.abilityDef != null;
                            if (flagBM2)
                            {
                                if (current16.learned == true && (current16.abilityDef == TorannMagicDefOf.TM_Rend || current16.abilityDef == TorannMagicDefOf.TM_Rend_I || current16.abilityDef == TorannMagicDefOf.TM_Rend_II || current16.abilityDef == TorannMagicDefOf.TM_Rend_III))
                                {
                                    if (current16.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Rend);
                                    }
                                    else if (current16.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Rend_I);
                                    }
                                    else if (current16.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Rend_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Rend_III);
                                    }
                                }
                            }
                        }
                    }
                }
                bool flag55 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Enchanter) || flagCM;
                if (flag55)
                {
                    bool flagE = !this.MagicData.MagicPowersE.NullOrEmpty<MagicPower>();
                    if (flagE)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current17 in this.MagicData.MagicPowersE)
                        {
                            bool flagE2 = current17.abilityDef != null;
                            if (flagE2)
                            {
                                if (current17.learned == true && (current17.abilityDef == TorannMagicDefOf.TM_Polymorph || current17.abilityDef == TorannMagicDefOf.TM_Polymorph_I || current17.abilityDef == TorannMagicDefOf.TM_Polymorph_II || current17.abilityDef == TorannMagicDefOf.TM_Polymorph_III))
                                {
                                    if (current17.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Polymorph);
                                    }
                                    else if (current17.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Polymorph_I);
                                    }
                                    else if (current17.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Polymorph_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Polymorph_III);
                                    }
                                }
                            }
                        }
                    }
                }
                bool flag56 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Chronomancer) || flagCM;
                if (flag56)
                {
                    bool flagC = !this.MagicData.MagicPowersC.NullOrEmpty<MagicPower>();
                    if (flagC)
                    {
                        //this.LoadPowers();
                        foreach (MagicPower current18 in this.MagicData.MagicPowersC)
                        {
                            bool flagC2 = current18.abilityDef != null;
                            if (flagC2)
                            {
                                if (current18.learned == true && (current18.abilityDef == TorannMagicDefOf.TM_ChronostaticField || current18.abilityDef == TorannMagicDefOf.TM_ChronostaticField_I || current18.abilityDef == TorannMagicDefOf.TM_ChronostaticField_II || current18.abilityDef == TorannMagicDefOf.TM_ChronostaticField_III))
                                {
                                    if (current18.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ChronostaticField);
                                    }
                                    else if (current18.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ChronostaticField_I);
                                    }
                                    else if (current18.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ChronostaticField_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ChronostaticField_III);
                                    }
                                }
                            }
                        }
                    }
                }
                if (flag40)
                {
                    //Log.Message("Loading Inner Fire Abilities");
                    MagicPower mpIF = this.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Firebolt);
                    if(mpIF.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Firebolt);
                    }
                    mpIF = this.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Fireclaw);
                    if (mpIF.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Fireclaw);
                    }
                    mpIF = this.MagicData.MagicPowersIF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Fireball);
                    if (mpIF.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Fireball);
                    }
                }
                if (flag41)
                {
                    //Log.Message("Loading Heart of Frost Abilities");
                    MagicPower mpHoF = this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Icebolt);
                    if (mpHoF.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Icebolt);
                    }
                    mpHoF = this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Snowball);
                    if (mpHoF.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Snowball);
                    }
                    mpHoF = this.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Rainmaker);
                    if (mpHoF.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Rainmaker);
                    }

                }
                if (flag42)
                {
                    //Log.Message("Loading Storm Born Abilities");
                    MagicPower mpSB = this.MagicData.MagicPowersSB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_LightningBolt);
                    if (mpSB.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_LightningBolt);
                    }
                    mpSB = this.MagicData.MagicPowersSB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_LightningCloud);
                    if (mpSB.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_LightningCloud);
                    }
                    mpSB = this.MagicData.MagicPowersSB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_LightningStorm);
                    if (mpSB.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_LightningStorm);
                    }
                }
                if (flag43)
                {
                    //Log.Message("Loading Arcane Abilities");
                    MagicPower mpA = this.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Teleport);
                    if (mpA.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Teleport);
                    }
                }
                if (flag44)
                {
                    //Log.Message("Loading Paladin Abilities");
                    MagicPower mpP = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Heal);
                    if (mpP.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Heal);
                    }
                    mpP = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ValiantCharge);
                    if (mpP.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_ValiantCharge);
                    }
                    mpP = this.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Overwhelm);
                    if (mpP.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Overwhelm);
                    }
                }
                if (flag45)
                {
                    //Log.Message("Loading Summoner Abilities");
                    MagicPower mpS = this.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonMinion);
                    if (mpS.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_SummonMinion);
                    }
                    mpS = this.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonPylon);
                    if (mpS.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_SummonPylon);
                    }
                    mpS = this.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonExplosive);
                    if (mpS.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_SummonExplosive);
                    }
                    mpS = this.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonElemental);
                    if (mpS.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_SummonElemental);
                    }
                }
                if (flag46)
                {
                    //Log.Message("Loading Druid Abilities");
                    MagicPower mpD = this.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Poison);
                    if (mpD.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Poison);
                    }
                    mpD = this.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Regenerate);
                    if (mpD.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Regenerate);
                    }
                    mpD = this.MagicData.MagicPowersD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_CureDisease);
                    if (mpD.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_CureDisease);
                    }
                }
                if (flag47)
                {
                    //Log.Message("Loading Necromancer Abilities");
                    MagicPower mpN = this.MagicData.MagicPowersN.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_RaiseUndead);
                    if (mpN.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_RaiseUndead);
                    }
                    mpN = this.MagicData.MagicPowersN.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_FogOfTorment);
                    if (mpN.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_FogOfTorment);
                    }
                }
                if (flag48)
                {
                    //Log.Message("Loading Priest Abilities");
                    MagicPower mpPR = this.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_AdvancedHeal);
                    if (mpPR.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_AdvancedHeal);
                    }
                    mpPR = this.MagicData.MagicPowersPR.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Purify);
                    if (mpPR.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Purify);
                    }
                }
                if (flag49)
                {
                    //Log.Message("Loading Bard Abilities");
                    MagicPower mpB = this.MagicData.MagicPowersB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_BardTraining);
                    //if (mpB.learned == true)
                    //{
                    //    this.AddPawnAbility(TorannMagicDefOf.TM_BardTraining);
                    //}
                    mpB = this.MagicData.MagicPowersB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Entertain);
                    if (mpB.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Entertain);
                    }
                    //mpB = this.MagicData.MagicPowersB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Inspire);
                    //if (mpB.learned == true)
                    //{
                    //    this.AddPawnAbility(TorannMagicDefOf.TM_Inspire);
                    //}
                }
                if (flag50)
                {
                    //Log.Message("Loading Succubus Abilities");
                    MagicPower mpSD = this.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Dominate);
                    if (mpSD.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Dominate);
                    }
                    mpSD = this.MagicData.MagicPowersSD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SoulBond);
                    if (mpSD.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_SoulBond);
                    }
                }
                if (flag51)
                {
                    //Log.Message("Loading Warlock Abilities");
                    MagicPower mpWD = this.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Dominate);
                    if (mpWD.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Dominate);
                    }
                    mpWD = this.MagicData.MagicPowersWD.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SoulBond);
                    if(mpWD.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_SoulBond);
                    }
                }
                if (flag52)
                {
                    //Log.Message("Loading Geomancer Abilities");
                    MagicPower mpG = this.MagicData.MagicPowersG.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Stoneskin);
                    if (mpG.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Stoneskin);
                    }
                    mpG = this.MagicData.MagicPowersG.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EarthSprites);
                    if (mpG.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_EarthSprites);
                    }
                    mpG = this.MagicData.MagicPowersG.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EarthernHammer);
                    if (mpG.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_EarthernHammer);
                    }
                    mpG = this.MagicData.MagicPowersG.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Sentinel);
                    if (mpG.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Sentinel);
                    }
                }
                if (flag53)
                {
                    //Log.Message("Loading Geomancer Abilities");
                    MagicPower mpT = this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TechnoTurret);
                    if (mpT.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_TechnoTurret);
                    }
                    mpT = this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TechnoWeapon);
                    if (mpT.learned == true)
                    {
                        //nano weapon applies only when equipping a new weapon
                        this.AddPawnAbility(TorannMagicDefOf.TM_TechnoWeapon);
                        this.AddPawnAbility(TorannMagicDefOf.TM_NanoStimulant);
                    }
                    mpT = this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TechnoShield);
                    if (mpT.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_TechnoShield);
                    }
                    mpT = this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Sabotage);
                    if (mpT.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Sabotage);
                    }
                    mpT = this.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Overdrive);
                    if (mpT.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Overdrive);
                    }
                }
                if (flag54)
                {
                    //Log.Message("Loading BloodMage Abilities");
                    MagicPower mpBM = this.MagicData.MagicPowersBM.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_BloodGift);
                    if (mpBM.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_BloodGift);
                    }
                    mpBM = this.MagicData.MagicPowersBM.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_IgniteBlood);
                    if (mpBM.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_IgniteBlood);
                    }
                    mpBM = this.MagicData.MagicPowersBM.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_BloodForBlood);
                    if (mpBM.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_BloodForBlood);
                    }
                    mpBM = this.MagicData.MagicPowersBM.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_BloodShield);
                    if (mpBM.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_BloodShield);
                    }
                }
                if (flag55)
                {
                    //Log.Message("Loading Enchanter Abilities");
                    MagicPower mpE = this.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchantedBody);
                    if (mpE.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_EnchantedBody);
                        this.spell_EnchantedAura = true;
                    }
                    mpE = this.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Transmutate);
                    if (mpE.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Transmutate);
                    }
                    mpE = this.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchanterStone);
                    if (mpE.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_EnchanterStone);
                    }
                    mpE = this.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchantWeapon);
                    if (mpE.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_EnchantWeapon);
                    }
                }
                if (flag56)
                {
                    //Log.Message("Loading Chronomancer Abilities");
                    MagicPower mpC = this.MagicData.MagicPowersC.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Prediction);
                    if (mpC.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_Prediction);
                    }
                    mpC = this.MagicData.MagicPowersC.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_AlterFate);
                    if (mpC.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_AlterFate);
                    }
                    mpC = this.MagicData.MagicPowersC.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_AccelerateTime);
                    if (mpC.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_AccelerateTime);
                    }
                    mpC = this.MagicData.MagicPowersC.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ReverseTime);
                    if (mpC.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_ReverseTime);
                    }
                }
                if (flagCM)
                {
                    //Log.Message("Loading Chaos Mage Abilities");
                    MagicPower mpCM = this.MagicData.MagicPowersCM.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_ChaosTradition);
                    if (mpCM.learned == true)
                    {
                        this.AddPawnAbility(TorannMagicDefOf.TM_ChaosTradition);
                        this.chaosPowers = new List<TM_ChaosPowers>();
                        this.chaosPowers.Clear();
                        List<MagicPower> learnedList = new List<MagicPower>();
                        learnedList.Clear();
                        for(int i = 0; i < this.MagicData.AllMagicPowersForChaosMage.Count; i++)
                        {
                            MagicPower mp = this.MagicData.AllMagicPowersForChaosMage[i];
                            if(mp.learned)
                            {
                                learnedList.Add(mp);
                            }
                        }
                        int count = learnedList.Count;
                        for (int i = 0; i < 5; i++)
                        {
                            if (i < count)
                            {
                                this.chaosPowers.Add(new TM_ChaosPowers((TMAbilityDef)learnedList[i].abilityDef, TM_Calc.GetAssociatedMagicPowerSkill(this, learnedList[i])));
                            }
                            else
                            {
                                this.chaosPowers.Add(new TM_ChaosPowers((TMAbilityDef)TM_Calc.GetRandomMagicPower(this).abilityDef, null));
                            }
                        }
                    }
                }
                this.InitializeSpell();
                //base.UpdateAbilities();
            }
        }
    }
}
