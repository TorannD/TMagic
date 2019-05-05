using RimWorld;
using System;
using RimWorld.Planet;
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
                if(map == null)
                {
                    Hediff bondHediff = null;
                    bondHediff = soulPawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_SoulBondPhysicalHD"), false);
                    if (bondHediff != null)
                    {
                        HediffComp_SoulBondHost compS = bondHediff.TryGetComp<HediffComp_SoulBondHost>();
                        if (compS != null && compS.polyHost != null && !compS.polyHost.DestroyedOrNull() && !compS.polyHost.Dead)
                        {
                            soulPawn = compS.polyHost;
                        }
                    }
                    bondHediff = null;

                    bondHediff = soulPawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_SoulBondMentalHD"), false);
                    if (bondHediff != null)
                    {
                        HediffComp_SoulBondHost compS = bondHediff.TryGetComp<HediffComp_SoulBondHost>();
                        if (compS != null && compS.polyHost != null && !compS.polyHost.DestroyedOrNull() && !compS.polyHost.Dead)
                        {
                            soulPawn = compS.polyHost;
                        }
                    }
                    if (soulPawn.ParentHolder != null && soulPawn.ParentHolder is Caravan)
                    {
                        //Log.Message("caravan detected");
                        //p.DeSpawn();
                        Caravan van = soulPawn.ParentHolder as Caravan;
                        van.RemovePawn(soulPawn);
                        GenPlace.TryPlaceThing(soulPawn, this.CasterPawn.Position, this.CasterPawn.Map, ThingPlaceMode.Near);
                        if(van.PawnsListForReading != null && van.PawnsListForReading.Count <= 0)
                        {
                            CaravanEnterMapUtility.Enter(van, this.CasterPawn.Map, CaravanEnterMode.Center, CaravanDropInventoryMode.DropInstantly, false);
                        }
                        
                        //Messages.Message("" + p.LabelShort + " has shadow stepped to a caravan with " + soulPawn.LabelShort, MessageTypeDefOf.NeutralEvent);
                        goto fin;
                    }
                }
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
            fin:;
            this.burstShotsLeft = 0;
            //this.ability.TicksUntilCasting = (int)base.UseAbilityProps.SecondsToRecharge * 60;
            return result;
        }
    }
}
