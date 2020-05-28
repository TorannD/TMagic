using Verse;
using RimWorld;
using System.Linq;

namespace TorannMagic.Enchantment
{
    class HediffComp_Enchantment : HediffComp
    {
        private bool initializing = true;
        private bool removeNow = false;

        private string enchantment ="";

        CompAbilityUserMagic comp;
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
                comp = this.Pawn.GetComp<CompAbilityUserMagic>();
                compMight = this.Pawn.GetComp<CompAbilityUserMight>();
                DisplayEnchantments();
            }
            if(Find.TickManager.TicksGame % 480 == 0 && this.enchantment == "unknown")
            {
                this.removeNow = true;
            }
        }

        private void DisplayEnchantments()
        {
            //MagicPowerSkill spirit = this.Pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_global_spirit.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_spirit_pwr");
            //MagicPowerSkill clarity = this.Pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_global_regen.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_regen_pwr");
            //MagicPowerSkill focus = this.Pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_global_eff.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_eff_pwr");
            bool flag = comp.IsMagicUser && !TM_Calc.IsCrossClass(comp.Pawn, true); // !this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless);
            if (comp != null && comp.IsMagicUser)
            {
                if (this.parent.def.defName == "TM_HediffEnchantment_maxMP")
                {
                    this.enchantment = (comp.maxMP * 100).ToString("0.##") + "%";
                }
                else if (this.parent.def.defName == "TM_HediffEnchantment_coolDown")
                {
                    this.enchantment = (comp.coolDown * 100).ToString("0.##") + "%";
                }
                else if (this.parent.def.defName == "TM_HediffEnchantment_mpCost")
                {
                    this.enchantment = (comp.mpCost * 100).ToString("0.##") + "%";
                }
                else if (this.parent.def.defName == "TM_HediffEnchantment_mpRegenRate")
                {
                    this.enchantment = (comp.mpRegenRate * 100).ToString("0.##") + "%";
                }
                else if (this.parent.def.defName == "TM_HediffEnchantment_arcaneDmg")
                {
                    this.enchantment = (comp.arcaneDmg * 100).ToString("0.##") + "%";
                }
            }

            if(compMight != null && compMight.IsMightUser)
            {
                if (this.parent.def.defName == "TM_HediffEnchantment_maxSP")
                {
                    this.enchantment = (compMight.maxSP * 100).ToString("0.##") + "%";
                }
                else if (this.parent.def.defName == "TM_HediffEnchantment_spCoolDown")
                {
                    this.enchantment = (compMight.coolDown * 100).ToString("0.##") + "%";
                }
                else if (this.parent.def.defName == "TM_HediffEnchantment_spCost")
                {
                    this.enchantment = (compMight.spCost * 100).ToString("0.##") + "%";
                }
                else if (this.parent.def.defName == "TM_HediffEnchantment_spRegenRate")
                {
                    this.enchantment = (compMight.spRegenRate * 100).ToString("0.##") + "%";
                }
                else if (this.parent.def.defName == "TM_HediffEnchantment_combatDmg")
                {
                    this.enchantment = (compMight.mightPwr * 100).ToString("0.##") + "%";
                }
            }

            if (this.parent.def.defName == "TM_HediffEnchantment_xpGain")
            {
                if (flag)
                {
                    this.enchantment = (comp.xpGain * 100).ToString("0.##") + "%";
                }
                else
                {
                    this.enchantment = (compMight.xpGain * 100).ToString("0.##") + "%";
                }
            }
            else if (this.parent.def.defName == "TM_HediffEnchantment_arcaneRes")
            {
                if (flag)
                {
                    this.enchantment = (comp.arcaneRes * 100).ToString("0.##") + "%";
                }
                else
                {
                    this.enchantment = (compMight.arcaneRes * 100).ToString("0.##") + "%";
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
            else if (this.parent.def.defName == "TM_HediffEnchantment_arcalleumCooldown")
            {
                if (flag)
                {
                    this.enchantment = (comp.arcalleumCooldown * 100).ToString("0.##") + "%";
                }
                else
                {
                    this.enchantment = (compMight.arcalleumCooldown * 100).ToString("0.##") + "%";
                }
            }

            if(enchantment == "")
            {
                this.enchantment = "unknown";
            }

        }
    }
}
