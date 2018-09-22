using RimWorld;
using System;
using Verse;
using AbilityUser;
using UnityEngine;
using System.Linq;

namespace TorannMagic
{
    class Verb_ShadowCall : Verb_UseAbility  
    {
        bool arg_41_0;
        bool arg_42_0;

        protected override bool TryCastShot()
        {
            bool result = false;
            CompAbilityUserMagic comp = this.CasterPawn.GetComp<CompAbilityUserMagic>();
            Pawn soulPawn = comp.soulBondPawn;

            if(soulPawn != null && !soulPawn.Dead && !soulPawn.Destroyed)
            {
                bool drafted = soulPawn.Drafted;
                Map map = soulPawn.Map;
                IntVec3 casterCell = this.CasterPawn.Position;
                IntVec3 targetCell = soulPawn.Position;
                try
                {                    
                    soulPawn.DeSpawn();
                    GenSpawn.Spawn(soulPawn, casterCell, this.CasterPawn.Map);
                    if (drafted)
                    {
                        soulPawn.drafter.Drafted = true;
                    }
                }
                catch
                {
                    Log.Message("Exception occured when trying to summon soul bound pawn - recovered pawn at original position");
                    GenSpawn.Spawn(soulPawn, targetCell, map);
                    
                }
                //this.Ability.PostAbilityAttempt();
                result = true;
            }
            else
            {
                Log.Warning("No soul bond found to shadow call.");
            }
            this.burstShotsLeft = 0;
            //this.ability.TicksUntilCasting = (int)base.UseAbilityProps.SecondsToRecharge * 60;
            return result;
        }
    }
}
