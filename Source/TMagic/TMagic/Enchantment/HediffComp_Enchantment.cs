using Verse;
using RimWorld;
using System.Linq;
using System.Text;

namespace TorannMagic.Enchantment
{
    class HediffComp_Enchantment : HediffComp
    {
        private bool initializing = true;
        private bool removeNow = false;

        private string enchantment ="";

        CompAbilityUserMagic compMagic;
        CompAbilityUserMight compMight;

        public string labelCap
        {
            get
            {
                return base.Def.LabelCap;
            }
        }

        public string label
        {
            get
            {
                return base.Def.label;
            }
        }

        public bool IsMagicUser
        {
            get
            {
                if(compMagic != null && compMagic.IsMagicUser)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsMightUser
        {
            get
            {
                if (compMight != null && compMight.IsMightUser)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsDualClass
        {
            get
            {
                if (IsMagicUser && IsMightUser)
                {
                    return true;
                }
                return false;
            }
        }

        public override string CompLabelInBracketsExtra => this.enchantment;

        private void Initialize()
        {
            bool spawned = base.Pawn.Spawned;
            if (spawned)
            {
                //MoteMaker.ThrowLightningGlow(base.Pawn.TrueCenter(), base.Pawn.Map, 3f);
            }
        }

        public override bool CompShouldRemove => base.CompShouldRemove || this.removeNow;

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            bool flag = base.Pawn != null;
            if (flag)
            {
                if (initializing)
                {
                    initializing = false;
                    this.Initialize();
                }
            }
            if(Find.TickManager.TicksGame % 120 == 0)
            {
                compMagic = this.Pawn.GetComp<CompAbilityUserMagic>();
                compMight = this.Pawn.GetComp<CompAbilityUserMight>();
                DetermineEnchantments();
            }
            if(Find.TickManager.TicksGame % 480 == 0 && this.enchantment == "unknown")
            {
                this.removeNow = true;
            }
        }

        private void DetermineEnchantments()
        {
            if (this.parent.def.defName == "TM_HediffEnchantment_maxEnergy")
            {
                if (IsDualClass)
                {
                    DisplayEnchantments(compMagic.maxMP, compMight.maxSP);
                }
                else if (IsMagicUser)
                {
                    DisplayEnchantments(compMagic.maxMP, 0f);
                }
                else if (IsMightUser)
                {
                    DisplayEnchantments(0f, compMight.maxSP);
                }
                else
                {
                    this.removeNow = true;
                }
            }
            else if (this.parent.def.defName == "TM_HediffEnchantment_coolDown")
            {
                if (IsDualClass)
                {
                    DisplayEnchantments(compMagic.coolDown, compMight.coolDown);
                }
                else if (IsMagicUser)
                {
                    DisplayEnchantments(compMagic.coolDown, 0f);
                }
                else if (IsMightUser)
                {
                    DisplayEnchantments(0f, compMight.coolDown);
                }
                else
                {
                    this.removeNow = true;
                }
            }
            else if (this.parent.def.defName == "TM_HediffEnchantment_energyCost")
            {
                if (IsDualClass)
                {
                    DisplayEnchantments(compMagic.mpCost, compMight.spCost);
                }
                else if (IsMagicUser)
                {
                    DisplayEnchantments(compMagic.mpCost, 0f);
                }
                else if (IsMightUser)
                {
                    DisplayEnchantments(0f, compMight.spCost);
                }
                else
                {
                    this.removeNow = true;
                }
            }
            else if (this.parent.def.defName == "TM_HediffEnchantment_energyRegen")
            {
                if (IsDualClass)
                {
                    DisplayEnchantments(compMagic.mpRegenRate, compMight.spRegenRate);
                }
                else if (IsMagicUser)
                {
                    DisplayEnchantments(compMagic.mpRegenRate, 0f);
                }
                else if (IsMightUser)
                {
                    DisplayEnchantments(0f, compMight.spRegenRate);
                }
                else
                {
                    this.removeNow = true;
                }
            }
            else if (this.parent.def.defName == "TM_HediffEnchantment_xpGain")
            {
                if (IsDualClass)
                {
                    DisplayEnchantments(compMagic.xpGain, compMight.xpGain);
                }
                else if (IsMagicUser)
                {
                    DisplayEnchantments(compMagic.xpGain, 0f);
                }
                else if (IsMightUser)
                {
                    DisplayEnchantments(0f, compMight.xpGain);
                }
                else
                {
                    this.removeNow = true;
                }
            }
            else if (this.parent.def.defName == "TM_HediffEnchantment_dmgResistance")
            {
                if (IsDualClass)
                {
                    DisplayEnchantments(compMagic.arcaneRes, compMight.arcaneRes);
                }
                else if (IsMagicUser)
                {
                    DisplayEnchantments(compMagic.arcaneRes, 0f);
                }
                else if (IsMightUser)
                {
                    DisplayEnchantments(0f, compMight.arcaneRes);
                }
                else
                {
                    this.removeNow = true;
                }
            }
            else if (this.parent.def.defName == "TM_HediffEnchantment_dmgBonus")
            {
                if (IsDualClass)
                {
                    DisplayEnchantments(compMagic.arcaneDmg, compMight.mightPwr);
                }
                else if (IsMagicUser)
                {
                    DisplayEnchantments(compMagic.arcaneDmg, 0f);
                }
                else if (IsMightUser)
                {
                    DisplayEnchantments(0f, compMight.mightPwr);
                }
                else
                {
                    this.removeNow = true;
                }
            }
            else if (this.parent.def.defName == "TM_HediffEnchantment_arcalleumCooldown")
            {
                if (IsDualClass)
                {
                    DisplayEnchantments(compMagic.arcalleumCooldown, compMight.arcalleumCooldown);
                }
                else if (IsMagicUser)
                {
                    DisplayEnchantments(compMagic.arcalleumCooldown, 0f);
                }
                else if (IsMightUser)
                {
                    DisplayEnchantments(0f, compMight.arcalleumCooldown);
                }
                else
                {
                    this.removeNow = true;
                }
            }
            else if (this.parent.def.defName == "TM_HediffEnchantment_arcaneSpectre")
            {
                this.enchantment = "TM_ArcaneSpectre".Translate();
            }
            else if (this.parent.def.defName == "TM_HediffEnchantment_phantomShift")
            {
                this.enchantment = "TM_PhantomShift".Translate();
            }
            else
            {
                Log.Message("enchantment unknkown");
                this.enchantment = "unknown";
            }           

        }

        private void DisplayEnchantments(float magVal = 0f, float mitVal = 0f)
        {
            string txtMagic = "";
            string txtMight = "";

            if (magVal != 0)
            {
                txtMagic = (magVal * 100).ToString("0.##") + "%";
            }
            if (mitVal != 0)
            {
                txtMight = (mitVal* 100).ToString("0.##") + "%";                 
            }

            if (txtMagic != "" && txtMight != "")
            {
                if (magVal != mitVal)
                { 
                    this.enchantment = txtMagic + " | " + txtMight;
                }
                else
                {
                    this.enchantment = txtMagic;
                }
            }
            else
            {
                this.enchantment = txtMagic + txtMight;
            }
            
            if(this.enchantment == "")
            {
                this.removeNow = true;
            }
        }
    }
}
