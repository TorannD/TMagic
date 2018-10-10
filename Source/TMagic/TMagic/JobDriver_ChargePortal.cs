using System.Collections.Generic;
using Verse.AI;
using System;
using Verse;
using RimWorld;


namespace TorannMagic
{
    internal class JobDriver_ChargePortal: JobDriver
    {
        private const TargetIndex building = TargetIndex.A;
        Building_TMPortal portalBldg;
        CompAbilityUserMagic comp;

        int age = -1;
        int chargeAge = 0;
        int ticksTillCharge = 30;
        int effectsAge = 0;
        int ticksTillEffects = 12;
        int duration = 1000;
        int xpNum = 0;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return true;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDestroyedOrNull(building);
            Toil reserveTargetA = Toils_Reserve.Reserve(building);
            yield return reserveTargetA;
            comp = pawn.GetComp<CompAbilityUserMagic>();
            portalBldg = TargetA.Thing as Building_TMPortal;

            Toil gotoPortal = new Toil()
            {
                initAction = () =>
                {
                    pawn.pather.StartPath(portalBldg.InteractionCell, PathEndMode.OnCell);
                },
                defaultCompleteMode = ToilCompleteMode.PatherArrival
            };
            yield return gotoPortal;

            Toil chargePortal = new Toil()
            {
                initAction = () =>
                {
                    if (age > duration)
                    {
                        this.EndJobWith(JobCondition.Succeeded);
                    }
                    if (comp.Mana.CurLevel < .01f)
                    {
                        this.EndJobWith(JobCondition.Succeeded);
                    }
                },
                tickAction = () =>
                {
                    if (age > (effectsAge + ticksTillEffects))
                    {
                        TM_MoteMaker.ThrowCastingMote(pawn.DrawPos, pawn.Map, Rand.Range(1.2f, 2f));
                        TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_Shadow, pawn.DrawPos, pawn.Map, Rand.Range(.4f, .6f), Rand.Range(.1f, .2f), .04f, Rand.Range(.1f, .2f), 300, 5f, Rand.Range(-10, 10), 0);
                    }
                    if (age > (chargeAge + ticksTillCharge))
                    {
                        comp.Mana.CurLevel -= .01f;
                        xpNum += 3;
                        portalBldg.ArcaneEnergyCur += .01f;
                        chargeAge = age;
                    }
                    age++;
                    if (age > duration)
                    {
                        AttributeXP(comp);
                        this.EndJobWith(JobCondition.Succeeded);
                    }
                    if (comp.Mana.CurLevel < .01f)
                    {
                        AttributeXP(comp);
                        this.EndJobWith(JobCondition.Succeeded);
                    }
                    if (portalBldg.ArcaneEnergyCur >= 1f)
                    {
                        AttributeXP(comp);
                        this.EndJobWith(JobCondition.Succeeded);
                    }
                },               

            };
            yield return chargePortal;
        }

        private void AttributeXP(CompAbilityUserMagic comp)
        {
            comp.MagicUserXP += xpNum;
            MoteMaker.ThrowText(this.pawn.DrawPos, this.pawn.MapHeld, "XP +" + xpNum, -1f);
        }
    }
}


//Toil chargePortal = new Toil();
//            while (age<duration)
//            {
//                if (portalBldg != null && pawn != null && !pawn.Downed && !pawn.Dead)
//                {
//                    if(age > chargeAge + ticksTillCharge)
//                    {
//                        if (comp.Mana.CurLevel <= 0.01f)
//                        {
//                            age = duration;
//                        }
//                        comp.Mana.CurLevel -= .01f;
//                        portalBldg.ArcaneEnergyCur += .01f;
//                        Log.Message("Portal Energy at " + portalBldg.ArcaneEnergyCur);
//                        chargeAge = age;
//                    }
//                    //chargePortal.initAction = () =>
//                    //{
//                    //    portalBldg = TargetA.Thing as Building_TMPortal;

//                    //    Log.Message("" + pawn.Label + " has " + comp.Mana.CurLevel + " mana");
//                    //};
//                    //chargePortal.AddPreTickAction(() =>
//                    //{
//                    //    if (comp.Mana.CurLevel < 0.01f)
//                    //    {
//                    //        ReadyForNextToil();
//                    //    }
//                    //});
//                    //chargePortal.tickAction = () =>
//                    //{
//                    //    if (comp.Mana.CurLevel >= 0.01f)
//                    //    {
//                    //        comp.Mana.CurLevel -= .01f;
//                    //        portalBldg.ArcaneEnergyCur += .01f;
//                    //        Log.Message("Portal Energy at " + portalBldg.ArcaneEnergyCur);
//                    //    }
//                    //};
//                    //chargePortal.AddFinishAction(() =>
//                    //{
//                    //    Log.Message("Portal charging complete.");
//                    //});
//                }
//                age++;
//                yield return chargePortal;
//    }