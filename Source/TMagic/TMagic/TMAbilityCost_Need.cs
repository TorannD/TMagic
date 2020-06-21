using System.Text;
using AbilityUser;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace TorannMagic
{
    public class TMAbilityNeedCostProp : TMAbilityCostProperties
    {
        private NeedDef needDef;
        private float baseCost = 0f;
        private float baseCostReductionPerUpgrade = 0f;
        public override TMAbilityCost ForPawn(Pawn pawn)
        {
            Need need = pawn.needs.TryGetNeed(needDef);
            return need != null ? new TMAbilityNeedCost(need, baseCost) : null;
        }
    }

    public class TMAbilityNeedCost : TMAbilityCost
    {
        private Need need;
        private float baseCost = 0f;
        //private float baseCostReductionPerUpgrade = 0f;

        public TMAbilityNeedCost(Need need, float baseCost)
        {
            this.need = need;
            this.baseCost = baseCost / 100f;
            Log.Message("Init cost " + baseCost + need.LabelCap);
        }

        public override float BaseCost
        {
            get => baseCost;
        }
        public override string Description
        {
            get => string.Concat(BaseCost * 100f, " ", need.LabelCap);
        }
        public override bool CanPayCost(out float actualCost)
        {
            actualCost = baseCost;
            return need.CurLevel > actualCost;
        }
        public override bool PayCost(out float actualCost)
        {
            if (need?.CurLevel < baseCost)
            {
                actualCost = baseCost;
                return false;
            }
            else
            {
                actualCost = baseCost;
                need.CurLevel -= baseCost;
                return true;
            }
        }
        public override bool RefundCost(out float actualRefund)
        {
            actualRefund = baseCost;
            if (need != null)
            {
                need.CurLevel += baseCost;
            }
            return true;
        }
    }
}
