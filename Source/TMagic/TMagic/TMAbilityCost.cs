using System.Text;
using AbilityUser;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace TorannMagic
{
    public abstract class TMAbilityCostProperties
    {
        public abstract TMAbilityCost ForPawn(Pawn pawn);
    }
    public abstract class TMAbilityCost
    {
        public abstract float BaseCost { get; }
        public abstract string Description { get; }
        public abstract bool CanPayCost(out float actualCost);
        public abstract bool PayCost(out float actualPaid);
        public abstract bool RefundCost(out float actualRefund);
    }
    public class TMAbilityBadCost : TMAbilityCost
    {
        public override float BaseCost => -1;
        public override string Description
        {
            get => "impossible cost";
        }
        public override bool CanPayCost(out float actualCost)
        {
            actualCost = BaseCost;
            return false;
        }
        public override bool PayCost(out float actualPaid)
        {
            return CanPayCost(out actualPaid);
        }
        public override bool RefundCost(out float actualRefund)
        {
            return CanPayCost(out actualRefund);
        }
    }
}
