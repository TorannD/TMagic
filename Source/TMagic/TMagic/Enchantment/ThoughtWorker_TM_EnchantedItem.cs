using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace TorannMagic.Enchantment
{
    public class ThoughtWorker_TM_EnchantedItem : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn pawn)
        {
            if (!pawn.Spawned || !pawn.RaceProps.Humanlike)
            {
                return false;
            }
            if (!(pawn.equipment != null && pawn.apparel != null))
            {
                return false;
            }
            if(pawn.apparel.WornApparelCount <= 0)
            {
                return false;
            }
            if(pawn.equipment.Primary != null)
            {
                CompEnchantedItem compItemW = pawn.equipment.Primary.TryGetComp<CompEnchantedItem>();
                if (compItemW != null && compItemW.HasEnchantment)
                {
                    if (compItemW.enchantmentThought != null)
                    {                        
                        ThoughtDef td = compItemW.enchantmentThought;
                        return CreateThought(td, pawn);
                    }
                    if(compItemW.MadeFromEnchantedStuff && compItemW.EnchantedStuff.appliedThoughts != null)
                    {
                        ThoughtDef td = compItemW.EnchantedStuff.appliedThoughts;
                        return CreateThought(td, pawn);
                    }
                }
            }
            List<Apparel> apparel = pawn.apparel.WornApparel;
            for(int i = 0; i < apparel.Count; i++)
            {
                CompEnchantedItem compItem = apparel[i].TryGetComp<CompEnchantedItem>();
                if(compItem != null && compItem.HasEnchantment && compItem.enchantmentThought != null)
                {
                    if (compItem.enchantmentThought != null)
                    {
                        ThoughtDef td = compItem.enchantmentThought;
                        return CreateThought(td, pawn);
                    }
                    if (compItem.MadeFromEnchantedStuff && compItem.EnchantedStuff.appliedThoughts != null)
                    {
                        ThoughtDef td = compItem.EnchantedStuff.appliedThoughts;
                        return CreateThought(td, pawn);
                    }
                }
            }
            return false;
        }

        private bool CreateThought(ThoughtDef td, Pawn pawn)
        {
            Thought_Situational ts = new Thought_Situational();
            try
            {
                if (!ThoughtUtility.CanGetThought(pawn, td))
                {
                    return false;
                }
                if (!td.Worker.CurrentState(pawn).ActiveFor(td))
                {
                    return false;
                }
                ts = (Thought_Situational)ThoughtMaker.MakeThought(td);
                ts.pawn = pawn;
                ts.RecalculateState();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Exception while recalculating " + def + " thought state for pawn " + pawn + ": " + ex);
                return false;
            }
        }
    }
}
