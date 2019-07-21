using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using AbilityUser;
using Verse;
using UnityEngine;


namespace TorannMagic
{
    public class Verb_Transmutate : Verb_UseAbility
    {

        private int verVal;
        private int pwrVal;

        bool validTarg;
        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            if (targ.IsValid && targ.CenterVector3.InBounds(base.CasterPawn.Map) && !targ.Cell.Fogged(base.CasterPawn.Map))
            {
                if ((root - targ.Cell).LengthHorizontal < this.verbProps.range)
                {
                    validTarg = true;
                }
                else
                {
                    //out of range
                    validTarg = false;
                }
            }
            else
            {
                validTarg = false;
            }
            return validTarg;
        }

        protected override bool TryCastShot()
        {
            CompAbilityUserMagic comp = this.CasterPawn.GetComp<CompAbilityUserMagic>();
            MagicPowerSkill pwr = base.CasterPawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Transmutate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Transmutate_pwr");
            MagicPowerSkill ver = base.CasterPawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Transmutate.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Transmutate_ver");
            pwrVal = pwr.level;
            verVal = ver.level;

            List<Thing> thingList = this.currentTarget.Cell.GetThingList(caster.Map);
            Thing transmutateThing = null;

            bool flagRawResource = false;
            bool flagStuffItem = false;
            bool flagNoStuffItem = false;
            bool flagNutrition = false;
            bool flagCorpse = false;

            for (int i = 0; i < thingList.Count; i++)
            {
                if (thingList[i] != null && !(thingList[i] is Pawn) && !(thingList[i] is Building))
                {
                    //if (thingList[i].def.thingCategories != null && thingList[i].def.thingCategories.Count > 0 && (thingList[i].def.thingCategories.Contains(ThingCategoryDefOf.ResourcesRaw) || thingList[i].def.thingCategories.Contains(ThingCategoryDefOf.StoneBlocks) || thingList[i].def.defName == "RawMagicyte"))                    
                    if (thingList[i].def.MadeFromStuff && verVal >= 3)
                    {
                        //Log.Message("stuff item");
                        flagStuffItem = true;
                        transmutateThing = thingList[i];
                        break;
                    }
                    if(!thingList[i].def.MadeFromStuff && thingList[i].TryGetComp<CompQuality>() != null && verVal >= 3)
                    {
                        //Log.Message("non stuff item");
                        flagNoStuffItem = true;
                        transmutateThing = thingList[i];
                        break;
                    }
                    if ((thingList[i].def.statBases != null && thingList[i].GetStatValue(StatDefOf.Nutrition) > 0) && !(thingList[i] is Corpse) && verVal >= 1)
                    {
                        //Log.Message("food item");
                        flagNutrition = true;
                        transmutateThing = thingList[i];
                        break;
                    }
                    if(thingList[i] is Corpse && verVal >= 2)
                    {
                        //Log.Message("corpse");
                        flagCorpse = true;
                        transmutateThing = thingList[i];
                        break;
                    }
                    if (thingList[i].def != null && !thingList[i].def.IsIngestible && ((thingList[i].def.stuffProps != null && thingList[i].def.stuffProps.categories != null && thingList[i].def.stuffProps.categories.Count > 0) || thingList[i].def.defName == "RawMagicyte"))
                    {
                        //Log.Message("resource");
                        flagRawResource = true;
                        transmutateThing = thingList[i];
                        break;
                    }
                }
            }

            if(transmutateThing != null)
            {
                //Log.Message("Current target thing is " + transmutateThing.LabelShort + " with a stack count of " + transmutateThing.stackCount + " and market value of " + transmutateThing.MarketValue + " base market value of " + transmutateThing.def.BaseMarketValue + " total value of stack " + transmutateThing.def.BaseMarketValue * transmutateThing.stackCount);
                if(flagNoStuffItem)
                {
                    CompQuality compQual = transmutateThing.TryGetComp<CompQuality>();
                    float wornRatio = ((float)transmutateThing.HitPoints / (float)transmutateThing.MaxHitPoints);
                    Thing thing = transmutateThing;

                    if (compQual != null && Rand.Chance((.02f * pwrVal)* comp.arcaneDmg))
                    {
                        thing.TryGetComp<CompQuality>().SetQuality(compQual.Quality + 1, ArtGenerationContext.Colony);
                    }
                    thing.HitPoints = Mathf.RoundToInt((wornRatio * thing.MaxHitPoints) + ((.3f + (.1f * pwrVal)) * thing.MaxHitPoints));
                    if (thing.HitPoints > thing.MaxHitPoints)
                    {
                        thing.HitPoints = thing.MaxHitPoints;
                    }

                    TransmutateEffects(this.currentTarget.Cell);
                    
                }
                else if (flagRawResource)
                {
                    //if (transmutateThing.def.defName != "RawMagicyte")
                    //{
                    //    for (int i = 0; i < transmutateThing.def.stuffProps.categories.Count; i++)
                    //    {
                    //        Log.Message("categories include " + transmutateThing.def.stuffProps.categories[i].defName);
                    //    }
                    //}

                    int transStackCount = 0;
                    if (transmutateThing.stackCount > 250)
                    {
                        transStackCount = 250;
                    }
                    else
                    {
                        transStackCount = transmutateThing.stackCount;
                    }
                    int transStackValue = Mathf.RoundToInt(transStackCount * transmutateThing.def.BaseMarketValue);
                    float newMatCount = 0;
                    IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                                        where (def != transmutateThing.def && ((def.stuffProps != null && def.stuffProps.categories != null && def.stuffProps.categories.Count > 0) || def.defName == "RawMagicyte"))
                                                        select def;

                    foreach (ThingDef current in enumerable)
                    {
                        if (current != null && current.defName != null)
                        {
                            newMatCount = transStackValue / current.BaseMarketValue;
                            //Log.Message("transumtation resource " + current.defName + " base value " + current.BaseMarketValue + " value count converts to " + newMatCount);
                        }
                    }

                    transmutateThing.SplitOff(transStackCount).Destroy(DestroyMode.Vanish);
                    Thing thing = null;
                    ThingDef newThingDef = enumerable.RandomElement();
                    newMatCount = transStackValue / newThingDef.BaseMarketValue;
                    thing = ThingMaker.MakeThing(newThingDef);
                    thing.stackCount = Mathf.RoundToInt((.7f + (.05f * pwrVal)) * newMatCount);

                    if (thing != null)
                    {
                        GenPlace.TryPlaceThing(thing, this.currentTarget.Cell, this.caster.Map, ThingPlaceMode.Near, null);
                        TransmutateEffects(this.currentTarget.Cell);
                    }
                }
                else if (flagStuffItem)
                {
                    //Log.Message("" + transmutateThing.LabelShort + " is made from " + transmutateThing.Stuff.label);
                    float transValue = transmutateThing.MarketValue;
                    IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                                       where (def.stuffProps != null && def.stuffProps.categories != null && def.stuffProps.categories.Contains(transmutateThing.Stuff.stuffProps.categories.RandomElement()))
                                                       select def;

                    //foreach (ThingDef current in enumerable)
                    //{
                    //    if (current != null && current.defName != null)
                    //    {
                    //        Log.Message("transumtation resource " + current.defName + " base value " + current.BaseMarketValue);
                    //    }
                    //}

                    CompQuality compQual = transmutateThing.TryGetComp<CompQuality>();
                    float wornRatio = ((float)transmutateThing.HitPoints / (float)transmutateThing.MaxHitPoints);
                    Thing thing = new Thing();
                    ThingDef newThingDef = enumerable.RandomElement();
                    thing = ThingMaker.MakeThing(transmutateThing.def, newThingDef);
                   
                    if (compQual != null)
                    {
                        thing.TryGetComp<CompQuality>().SetQuality(compQual.Quality, ArtGenerationContext.Colony);
                    }
                    thing.HitPoints = Mathf.RoundToInt((wornRatio * thing.MaxHitPoints) - ((.2f - (.1f * pwrVal)) * thing.MaxHitPoints));
                    if(thing.HitPoints > thing.MaxHitPoints)
                    {
                        thing.HitPoints = thing.MaxHitPoints;
                    }
                    transmutateThing.Destroy(DestroyMode.Vanish);
                    if (thing != null)
                    {
                        GenPlace.TryPlaceThing(thing, this.currentTarget.Cell, this.caster.Map, ThingPlaceMode.Near, null);                        
                        if (thing.HitPoints <= 0)
                        {
                            thing.Destroy(DestroyMode.Vanish);
                            Messages.Message("TM_TransmutationLostCohesion".Translate(thing.def.label), MessageTypeDefOf.NeutralEvent);
                        }
                        else
                        {
                            thing.SetForbidden(true, false);
                        }
                        TransmutateEffects(this.currentTarget.Cell);
                    }
                }
                else if (flagNutrition)
                {                    
                    int transStackCount = 0;
                    if (transmutateThing.stackCount > 500)
                    {
                        transStackCount = 500;
                    }
                    else
                    {
                        transStackCount = transmutateThing.stackCount;
                    }
                    float transNutritionTotal = transmutateThing.GetStatValue(StatDefOf.Nutrition) * transStackCount;
                    float newMatCount = 0;
                    ThingDef newThingDef = null;
                    //Log.Message("" + transmutateThing.LabelShort + " has a nutrition value of " + transmutateThing.GetStatValue(StatDefOf.Nutrition) + " and stack count of " + transmutateThing.stackCount + " for a total nutrition value of " + transNutritionTotal);
                    IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                                       where (def.defName == "Pemmican" || def.defName == "MealNutrientPaste")
                                                       select def;

                    newThingDef = enumerable.RandomElement();
                    if(newThingDef != null)
                    {
                        transmutateThing.SplitOff(transStackCount).Destroy(DestroyMode.Vanish);
                        Thing thing = null;
                        newMatCount = transNutritionTotal / newThingDef.GetStatValueAbstract(StatDefOf.Nutrition);
                        thing = ThingMaker.MakeThing(newThingDef);
                        thing.stackCount = Mathf.RoundToInt((.7f + (.05f * pwrVal)) * newMatCount);
                        if(thing.stackCount < 1)
                        {
                            thing.stackCount = 1;
                        }

                        if (thing != null)
                        {
                            GenPlace.TryPlaceThing(thing, this.currentTarget.Cell, this.caster.Map, ThingPlaceMode.Near, null);
                            TransmutateEffects(this.currentTarget.Cell);
                        }
                    }
                    else
                    {
                        Log.Message("No known edible foods to transmutate to - pemmican and nutrient paste removed?");
                    }
                }
                else if(flagCorpse)
                {
                    Corpse transCorpse = transmutateThing as Corpse;
                    ThingDef newThingDef = null;
                    float corpseNutritionValue = 0;
                    if (transCorpse != null)
                    {
                        List<Thing> butcherProducts = transCorpse.ButcherProducts(this.CasterPawn, 1f).ToList();
                        for (int j = 0; j < butcherProducts.Count; j++)
                        {
                            if (butcherProducts[j].GetStatValue(StatDefOf.Nutrition) > 0)
                            {
                                corpseNutritionValue = (butcherProducts[j].GetStatValue(StatDefOf.Nutrition) * butcherProducts[j].stackCount);
                                //Log.Message("corpse has a meat nutrition amount of " + (butcherProducts[j].GetStatValue(StatDefOf.Nutrition) * butcherProducts[j].stackCount));
                            }
                        }

                        IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                                           where (def.defName == "MealNutrientPaste")
                                                           select def;

                        newThingDef = enumerable.RandomElement();
                        if (newThingDef != null)
                        {
                            transCorpse.Destroy(DestroyMode.Vanish);
                            Thing thing = null;
                            int newMatCount = Mathf.RoundToInt(corpseNutritionValue / newThingDef.GetStatValueAbstract(StatDefOf.Nutrition));
                            thing = ThingMaker.MakeThing(newThingDef);
                            thing.stackCount = Mathf.RoundToInt((.7f + (.05f * pwrVal)) * newMatCount);

                            if (thing != null)
                            {
                                GenPlace.TryPlaceThing(thing, this.currentTarget.Cell, this.caster.Map, ThingPlaceMode.Near, null);
                                TransmutateEffects(this.currentTarget.Cell);
                            }
                        }
                        else
                        {
                            Log.Message("No known edible foods to transmutate to - nutrient paste removed?");
                        }
                    }
                }
                else
                {
                    Messages.Message("TM_UnableToTransmutate".Translate(
                        this.CasterPawn.LabelShort,
                        this.currentTarget.Thing.LabelShort
                    ), MessageTypeDefOf.RejectInput);
                }                
            }
            else
            {
                Messages.Message("TM_NoThingToTransmutate".Translate(
                    this.CasterPawn.LabelShort
                ), MessageTypeDefOf.RejectInput);
            }

            this.burstShotsLeft = 0;
            return false;
        }

        public void TransmutateEffects(IntVec3 position)
        {
            Vector3 rndPos = position.ToVector3Shifted();
            MoteMaker.ThrowHeatGlow(position, this.CasterPawn.Map, 1f);
            for(int i =0; i < 6; i++)
            {
                rndPos.x += Rand.Range(-.5f, .5f);
                rndPos.z += Rand.Range(-.5f, .5f);
                rndPos.y += Rand.Range(.3f, 1.3f);
                MoteMaker.ThrowSmoke(rndPos, this.CasterPawn.Map, Rand.Range(.7f, 1.1f));
                MoteMaker.ThrowLightningGlow(position.ToVector3Shifted(), this.CasterPawn.Map, 1.4f);
            }
        }
    }
}