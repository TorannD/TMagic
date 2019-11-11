using System.Text;
using AbilityUser;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace TorannMagic
{
	public class TMAbilityDef : AbilityDef
	{
        //Add new variables here to control skill levels
        public float manaCost = 0f;
        public float staminaCost = 0f;
        public float bloodCost = 0f;
        public float chiCost = 0f;
        public int abilityPoints = 1;

        public string GetPointDesc()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.GetDescription());
			return stringBuilder.ToString();
		}        
    }
}
