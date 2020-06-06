using System.Text;
using AbilityUser;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace TorannMagic
{
	public class TMAbilityDef : AbilityUser.AbilityDef
	{
        //Add new variables here to control skill levels
        public float manaCost = 0f;
        public float staminaCost = 0f;
        public float bloodCost = 0f;
        public float chiCost = 0f;
        public bool consumeEnergy = true;
        public int abilityPoints = 1;
        public float learnChance = 1f;
        public float efficiencyReductionPercent = 0f;
        public float upkeepEnergyCost = 0f;
        public float upkeepRegenCost = 0f;
        public float upkeepEfficiencyPercent = 0f;
        public bool shouldInitialize = true;
        public float weaponDamageFactor = 1f;
        public HediffDef abilityHediff = null;
        public ThingDef learnItem = null;
        public bool canCopy = false;

        public string GetPointDesc()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.GetDescription());
			return stringBuilder.ToString();
		}        
    }
}
