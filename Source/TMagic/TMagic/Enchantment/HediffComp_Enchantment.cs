using Verse;
using RimWorld;
using System.Linq;

namespace TorannMagic.Enchantment
{
    class HediffComp_Enchantment : HediffComp
    {
        private bool initializing = true;

        private string enchantment ="";

        CompAbilityUserMagic comp;

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
                MoteMaker.ThrowLightningGlow(base.Pawn.TrueCenter(), base.Pawn.Map, 3f);
            }
        }

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
                CalculateMaxMP();
            }
        }

        private void CalculateMaxMP()
        {
            MagicPowerSkill spirit = this.Pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_global_spirit.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_spirit_pwr");
            if (this.parent.def.defName == "TM_HediffEnchantment_maxMP")
            {
                this.enchantment = ((comp.maxMP - (spirit.level *.02f)) * 100).ToString() + "%";
            }
            else if (this.parent.def.defName == "TM_HediffEnchantment_coolDown")
            {
                this.enchantment = (comp.coolDown * 100).ToString() + "%";
            }
            else if (this.parent.def.defName == "TM_HediffEnchantment_mpCost")
            {
                this.enchantment = (comp.mpCost * 100).ToString() + "%";
            }
            else if (this.parent.def.defName == "TM_HediffEnchantment_mpRegenRate")
            {
                this.enchantment = (comp.mpRegenRate * 100).ToString() + "%";
            }
            else if (this.parent.def.defName == "TM_HediffEnchantment_xpGain")
            {
                this.enchantment = (comp.xpGain * 100).ToString() + "%";
            }
            else
            {
                this.enchantment = "unknown";
            }

        }
    }
}
