using System.Text;
using AbilityUser;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace TorannMagic
{
    public class TMAbilityHediffCostProp : TMAbilityCostProperties
    {
        private HediffDef hediffDef;
        private float baseCost = 0f;
        private float baseCostReductionPerUpgrade = 0f;
        public override TMAbilityCost ForPawn(Pawn pawn)
        {
            TMAbilityHediffCost.Backup backup = () => pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef);
            return new TMAbilityHediffCost(hediffDef, baseCost, backup);
        }
    }

    public class TMAbilityHediffCost : TMAbilityCost
    {
        public delegate Hediff Backup();

        private string hediffLabel;
        private Hediff found;
        private Backup backup;
        private float baseCost = 0f;
        //private float baseCostReductionPerUpgrade = 0f;
        private Hediff hediff
        {
            get {
                if (found == null) // TODO: limit check frequency
                {
                    found = backup();
                }
                return found;
            }
        }

        public TMAbilityHediffCost(HediffDef def, float baseCost, Backup backup)
        {
            this.hediffLabel = def.LabelCap;
            this.baseCost = baseCost;
            this.backup = backup;
        }

        public override float BaseCost
        {
            get => baseCost;
        }
        public override string Description
        {
            get => string.Concat(BaseCost, " ", hediffLabel);
        }
        public override bool CanPayCost(out float actualCost)
        {
            actualCost = baseCost;
            return hediff?.Severity > actualCost;
        }
        public override bool PayCost(out float actualCost)
        {
            if (hediff?.Severity >= baseCost)
            {
                actualCost = baseCost;
                hediff.Severity -= baseCost;
                return true;
            }
            else
            {
                actualCost = baseCost;
                return false;
            }
        }
        public override bool RefundCost(out float actualRefund)
        {
            actualRefund = baseCost;
            if (hediff != null)
            {
                hediff.Severity += baseCost;
                return true;
            }
            return false;
        }
    }
}
