using Verse;
using RimWorld;
using AbilityUser;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace TorannMagic
{
    public class Projectile_FertileLands : Projectile_AbilityBase
    {
        bool initialized = false;
        Pawn caster;

        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            base.Impact(hitThing);
            ThingDef def = this.def;
            this.caster = this.launcher as Pawn;

            if(!this.initialized)
            {
                this.initialized = true;
            }

            CompAbilityUserMagic comp = this.caster.GetComp<CompAbilityUserMagic>();            
            IEnumerable<IntVec3> targetCells = GenRadial.RadialCellsAround(base.Position, 6, true);            
            for (int i = 0; i < targetCells.Count(); i++)
            {
                comp.fertileLands.Add(targetCells.ToArray<IntVec3>()[i]);
            }
            TM_MoteMaker.ThrowTwinkle(base.Position.ToVector3Shifted(), map, 1f);
            ModOptions.Constants.SetGrowthCells(comp.fertileLands);
            comp.fertileLandsCopied = true;
            comp.RemovePawnAbility(TorannMagicDefOf.TM_FertileLands);
            comp.AddPawnAbility(TorannMagicDefOf.TM_DismissFertileLands);

        }       
    }
}


