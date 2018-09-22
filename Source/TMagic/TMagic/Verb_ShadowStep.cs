using RimWorld;
using System;
using Verse;
using AbilityUser;
using UnityEngine;
using System.Linq;

namespace TorannMagic
{
    class Verb_ShadowStep : Verb_UseAbility  
    {

        protected override bool TryCastShot()
        {
            bool result = false;
            CompAbilityUserMagic comp = this.CasterPawn.GetComp<CompAbilityUserMagic>();
            Pawn soulPawn = comp.soulBondPawn;

            if(soulPawn != null && !soulPawn.Dead && !soulPawn.Destroyed)
            {
                Pawn p = this.CasterPawn;
                bool drafted = this.CasterPawn.Drafted;
                Map map = this.CasterPawn.Map;
                IntVec3 casterCell = this.CasterPawn.Position;
                IntVec3 targetCell = soulPawn.Position;
                try
                {
                    p.DeSpawn();
                    GenSpawn.Spawn(p, targetCell, soulPawn.Map);
                    if (drafted)
                    {
                        p.drafter.Drafted = true;
                    }
                    CameraJumper.TryJumpAndSelect(p);
                }
                catch
                {
                    Log.Message("Exception occured when trying to shadow step to soul bound pawn - recovered caster at original position");
                    GenSpawn.Spawn(p, casterCell, map);
                    
                }
                this.Ability.PostAbilityAttempt();
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
