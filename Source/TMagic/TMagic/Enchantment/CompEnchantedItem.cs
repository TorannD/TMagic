using System;
using Verse;
using RimWorld;
using AbilityUser;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TorannMagic.Enchantment
{
    public class CompEnchantedItem : ThingComp
    {
        public List<AbilityDef> MagicAbilities = new List<AbilityDef>();

        public List<Trait> SoulOrbTraits = new List<Trait>();

        public CompAbilityUserMagic CompAbilityUserMagicTarget = null;

        public CompProperties_EnchantedItem Props
        {
            get
            {
                return (CompProperties_EnchantedItem)this.props;
            }
        }

        public void GetOverlayGraphic()
        {
        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            Pawn pawn = this.parent as Pawn;
            if (!initialized)
            {
                this.hasEnchantment = this.Props.hasEnchantment;

                this.arcaneDmg = this.Props.arcaneDmg;
                this.arcaneDmgTier = this.Props.arcaneDmgTier;
                this.arcaneRes = this.Props.arcaneRes;
                this.arcaneResTier = this.Props.arcaneResTier;

                this.maxMP = this.Props.maxMP;
                this.maxMPTier = this.Props.maxMPTier;
                this.mpRegenRate = this.Props.mpRegenRate;
                this.mpRegenRateTier = this.Props.mpRegenRateTier;
                this.coolDown = this.Props.coolDown;
                this.coolDownTier = this.Props.coolDownTier;
                this.mpCost = this.Props.mpCost;
                this.mpCostTier = this.Props.mpCostTier;
                this.xpGain = this.Props.xpGain;
                this.xpGainTier = this.Props.xpGainTier;

                this.healthRegenRate = this.Props.healthRegenRate;

                this.arcaneSpectre = this.Props.arcaneSpectre;
                this.phantomShift = this.Props.phantomShift;
                this.arcalleumCooldown = this.Props.arcalleumCooldown;

                this.skillTier = this.Props.skillTier;

                this.hediff = this.Props.hediff;
                this.hediffSeverity = this.Props.hediffSeverity;

                if (this.parent.def.tickerType == TickerType.Rare)
                {
                    Find.TickManager.RegisterAllTickabilityFor(this.parent);
                }

                if(this.Props.hasAbility && !abilitiesInitialized)
                {
                    InitializeAbilities(this.parent as Apparel);
                }

                this.initialized = true;
            }
        }

        private void InitializeAbilities(Apparel abilityApparel)
        {
            if (abilityApparel != null && abilityApparel.Wearer != null)
            {
                AbilityUserMod.Notify_ApparelRemoved_PostFix(abilityApparel.Wearer.apparel, abilityApparel);
                AbilityUserMod.Notify_ApparelAdded_PostFix(abilityApparel.Wearer.apparel, abilityApparel);
                this.abilitiesInitialized = true;
            }
        }

        public override void CompTickRare()
        {
            if (this.hediff != null)
            {
                Apparel artifact = this.parent as Apparel;
                if (artifact != null)
                {
                    if (artifact.Wearer != null)
                    {
                        //Log.Message("" + artifact.LabelShort + " has holding owner " + artifact.Wearer.LabelShort);
                        if(artifact.Wearer.health.hediffSet.GetFirstHediffOfDef(hediff, false) != null)
                        {

                        }
                        else
                        {                            
                            HealthUtility.AdjustSeverity(artifact.Wearer, hediff, hediffSeverity);
                            artifact.Wearer.health.hediffSet.GetFirstHediffOfDef(hediff, false).TryGetComp<HediffComp_EnchantedItem>().enchantedItem = artifact;
                            //HediffComp_EnchantedItem comp = diff.TryGetComp<HediffComp_EnchantedItem>();

                        }
                    }

                }
            }
            if (this.Props.hasAbility && !this.abilitiesInitialized)
            {
                Apparel artifact = this.parent as Apparel;
                if (artifact != null)
                {
                    if (artifact.Wearer != null)
                    {
                        //Log.Message("" + artifact.LabelShort + " has holding owner " + artifact.Wearer.LabelShort);
                        this.InitializeAbilities(artifact);                        
                    }

                    this.MagicAbilities = artifact.GetComp<CompAbilityItem>().Props.Abilities;
                    //this.MagicAbilities = new List<AbilityDef>();
                    //this.MagicAbilities.Clear();
                    // abilities;
                }
            }
            base.CompTickRare();
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            bool flag = this.parent.def.tickerType == TickerType.Never;
            if (flag)
            {
                //this.parent.def.tickerType = TickerType.Rare;
                //Find.TickManager.RegisterAllTickabilityFor(this.parent);
            }
            base.PostSpawnSetup(respawningAfterLoad);
            
        }
        
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<float>(ref this.maxMP, "maxMP", 0, false);
            Scribe_Values.Look<float>(ref this.mpRegenRate, "mpRegenRateP", 0, false);
            Scribe_Values.Look<float>(ref this.coolDown, "coolDown", 0, false);
            Scribe_Values.Look<float>(ref this.mpCost, "mpCost", 0, false);
            Scribe_Values.Look<float>(ref this.xpGain, "xpGain", 0, false);
            Scribe_Values.Look<float>(ref this.arcaneRes, "arcaneRes", 0, false);
            Scribe_Values.Look<float>(ref this.arcaneDmg, "arcaneDmg", 0, false);
            Scribe_Values.Look<float>(ref this.necroticEnergy, "necroticEnergy", 0f, false);
            Scribe_Values.Look<bool>(ref this.arcaneSpectre, "arcaneSpectre", false, false);
            Scribe_Values.Look<bool>(ref this.phantomShift, "phantomShift", false, false);
            Scribe_Values.Look<float>(ref this.arcalleumCooldown, "arcalleumCooldown", 0f, false);
            Scribe_Values.Look<EnchantmentTier>(ref this.maxMPTier, "maxMPTier", (EnchantmentTier)0, false);
            Scribe_Values.Look<EnchantmentTier>(ref this.mpRegenRateTier, "mpRegenRateTier", (EnchantmentTier)0, false);
            Scribe_Values.Look<EnchantmentTier>(ref this.coolDownTier, "coolDownTier", (EnchantmentTier)0, false);
            Scribe_Values.Look<EnchantmentTier>(ref this.mpCostTier, "mpCostTier", (EnchantmentTier)0, false);
            Scribe_Values.Look<EnchantmentTier>(ref this.xpGainTier, "xpGainTier", (EnchantmentTier)0, false);
            Scribe_Values.Look<EnchantmentTier>(ref this.arcaneResTier, "arcaneResTier", (EnchantmentTier)0, false);
            Scribe_Values.Look<EnchantmentTier>(ref this.arcaneDmgTier, "arcaneDmgTier", (EnchantmentTier)0, false);
            Scribe_Values.Look<bool>(ref this.hasEnchantment, "hasEnchantment", false, false);
            Scribe_Values.Look<bool>(ref this.initialized, "initialized", false, false);
            Scribe_Collections.Look<Trait>(ref this.SoulOrbTraits, "SoulOrbTraits", LookMode.Deep);
            //this.Props.ExposeData();
        }

        public override string GetDescriptionPart()
        {
            string text = string.Empty;
            bool flag = this.Props.MagicAbilities.Count == 1;
            if (flag)
            {
                text += "Item Ability:";
            }
            else
            {
                bool flag2 = this.Props.MagicAbilities.Count > 1;
                if (flag2)
                {
                    text += "Item Abilities:";
                }
            }
            foreach (TMAbilityDef current in this.Props.MagicAbilities)
            {
                text += "\n\n";
                text = text + current.label.CapitalizeFirst() + " - ";
                text += current.GetDescription();
            }
            bool flag3 = this.SoulOrbTraits != null && this.SoulOrbTraits.Count > 0;
            if (flag3)
            {
                text += "Absorbed Traits:";
                foreach (Trait current in this.SoulOrbTraits)
                {
                    text += "\n";
                    text = text + current.LabelCap;
                }

            }
            bool flag4 = this.necroticEnergy != 0;
            if(flag4)
            {
                text += "Necrotic Energy: " + this.NecroticEnergy.ToString("N1");
            }
            return text;
        }

        private bool initialized = false;
        private bool abilitiesInitialized = false;
        private bool hasEnchantment = false;

        public EnchantmentTier maxMPTier;
        public EnchantmentTier mpRegenRateTier;
        public EnchantmentTier coolDownTier;
        public EnchantmentTier mpCostTier;
        public EnchantmentTier xpGainTier;
        public EnchantmentTier arcaneResTier;
        public EnchantmentTier arcaneDmgTier;

        //Magic Stats (%)
        public float maxMP = 0;
        public float mpRegenRate = 0;
        public float coolDown = 0;
        public float mpCost = 0;
        public float xpGain = 0;

        public float arcaneRes = 0;
        public float arcaneDmg = 0;

        //Might Stats (%)

        //Common Stats (%)        

        public float healthRegenRate = 0;
        private float necroticEnergy = 0f;

        //Special Abilities
        public EnchantmentTier skillTier = EnchantmentTier.Skill;
        public bool arcaneSpectre = false;
        public bool phantomShift = false;

        public float arcalleumCooldown = 0f;

        //Hediffs
        public HediffDef hediff = null;
        public float hediffSeverity = 0f;

        //Abilities

        public float NecroticEnergy
        {
            get
            {
                return Mathf.Clamp(this.necroticEnergy, 0f, 100f);
            }
            set
            {
                this.necroticEnergy = Mathf.Clamp(value, 0f, 100f);
            }
        }

        private float StuffMultiplier
        {
            get
            {
                if(this.parent.Stuff != null && this.parent.Stuff.defName == "TM_Manaweave")
                {
                    return 120f;
                }
                else
                {
                    return 100f;
                }
            }
        }

        public string MaxMPLabel
        {
            get
            {
                return "TM_MaxMPLabel".Translate(
                    this.maxMP * StuffMultiplier
                );
            }
        }

        public string MPRegenRateLabel
        {
            get
            {
                return "TM_MPRegenRateLabel".Translate(
                    this.mpRegenRate * StuffMultiplier
                );
            }
        }

        public string CoolDownLabel
        {
            get
            {
                return "TM_CoolDownLabel".Translate(
                    this.coolDown * StuffMultiplier
                );
            }
        }

        public string MPCostLabel
        {
            get
            {
                return "TM_MPCostLabel".Translate(
                    this.mpCost * StuffMultiplier
                );
            }
        }

        public string XPGainLabel
        {
            get
            {
                return "TM_XPGainLabel".Translate(
                    this.xpGain * StuffMultiplier
                );
            }
        }

        public string ArcaneResLabel
        {
            get
            {
                return "TM_ArcaneResLabel".Translate(
                    this.arcaneRes * StuffMultiplier
                );
            }
        }

        public string ArcaneDmgLabel
        {
            get
            {
                return "TM_ArcaneDmgLabel".Translate(
                    this.arcaneDmg * StuffMultiplier
                );
            }
        }

        public string ArcaneSpectreLabel
        {
            get
            {
                return "TM_ArcaneSpectre".Translate();
            }
        }

        public string PhantomShiftLabel
        {
            get
            {
                return "TM_PhantomShift".Translate();
            }
        }

        public string ArcalleumCooldownLabel
        {
            get
            {
                return "TM_ArcalleumCooldown".Translate(
                    this.arcalleumCooldown);
            }
        }

        public string HediffLabel
        {
            get
            {
                return this.hediff.LabelCap;
            }
        }

        public bool HasMagic
        {
            get
            {
                return MagicAbilities.Count > 0;
            }
        }

        public EnchantmentTier SetTier(float mod)
        {
            if (mod < 0)
            {
                return EnchantmentTier.Negative;
            }
            if (mod <= .05f)
            {
                return EnchantmentTier.Minor;
            }
            else if (mod <= .1f)
            {
                return EnchantmentTier.Standard;
            }
            else if (mod <= .15f)
            {
                return EnchantmentTier.Major;
            }
            else if (mod > .15f)
            {
                return EnchantmentTier.Crafted;
            }
            else
            {
                return EnchantmentTier.Undefined;
            }
        }

        public bool HasEnchantment
        {
            get
            {
                return hasEnchantment;
            }
            set
            {
                hasEnchantment = value;
            }
        }        
    }
}
