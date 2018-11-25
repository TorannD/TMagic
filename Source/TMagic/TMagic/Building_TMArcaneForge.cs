using Verse;
using RimWorld;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Verse.Sound;


namespace TorannMagic
{
    [StaticConstructorOnStartup]
    public class Building_TMArcaneForge : Building_WorkTable
    {
        private Thing targetThing = null;
        private LocalTargetInfo infoTarget = null;

        List<RecipeDef> replicatedRecipes = new List<RecipeDef>();

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            replicatedRecipes = new List<RecipeDef>();
            replicatedRecipes.Clear();
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            
            var gizmoList = base.GetGizmos().ToList();
            if (ResearchProjectDef.Named("TM_ForgeReplication").IsFinished)
            {
                bool canScan = true;
                for(int i =0; i < this.BillStack.Count; i++)
                {
                    if(this.BillStack[i].recipe.defName == "ArcaneForge_Replication")
                    {
                        canScan = false;
                    }
                }
                if (canScan)
                {
                    TargetingParameters newParameters = new TargetingParameters();
                    newParameters.canTargetItems = true;
                    newParameters.canTargetBuildings = true;
                    newParameters.canTargetLocations = true;


                    String label = "TM_Replicate".Translate();
                    String desc = "TM_ReplicateDesc".Translate();

                    Command_LocalTargetInfo item = new Command_LocalTargetInfo
                    {
                        defaultLabel = label,
                        defaultDesc = desc,
                        order = 67,
                        icon = ContentFinder<Texture2D>.Get("UI/replicate", true),
                        targetingParams = newParameters
                    };
                    item.action = delegate (LocalTargetInfo thing)
                    {
                        this.infoTarget = thing;
                        IntVec3 localCell = thing.Cell;
                        this.targetThing = thing.Cell.GetFirstItem(this.Map);
                        TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Scan"), localCell.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather), this.Map, 1.2f, .8f, 0f, .5f, -400, 0, 0, Rand.Range(0, 360));
                        TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Scan"), localCell.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather), this.Map, 1.2f, .8f, 0f, .5f, 400, 0, 0, Rand.Range(0, 360));
                        TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Scan"), this.DrawPos, this.Map, 1.2f, .8f, 0f, .5f, 400, 0, 0, Rand.Range(0, 360));
                        TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Scan"), this.DrawPos, this.Map, 1.2f, .8f, 0f, .5f, -400, 0, 0, Rand.Range(0, 360));
                        SoundInfo info = SoundInfo.InMap(new TargetInfo(thing.Cell, this.Map, false), MaintenanceType.None);
                        info.pitchFactor = 1.3f;
                        info.volumeFactor = 1.3f;
                        SoundDefOf.TurretAcquireTarget.PlayOneShot(info);
                        if (targetThing != null && targetThing.def.EverHaulable)
                        {
                            ClearReplication();
                            Replicate();
                        }
                        else
                        {
                            Messages.Message("TM_FoundNoReplicateTarget".Translate(), MessageTypeDefOf.CautionInput);
                        }
                    };
                    gizmoList.Add(item);
                }
                else
                {
                    String label = "TM_ReplicateDisabled".Translate();
                    String desc = "TM_ReplicateDisabledDesc".Translate();

                    Command_Action item2 = new Command_Action
                    {
                        defaultLabel = label,
                        defaultDesc = desc,
                        order = 68,
                        icon = ContentFinder<Texture2D>.Get("UI/replicateDisabled", true),
                        action = delegate
                        {
                            ClearReplication();
                        }
                    };
                    gizmoList.Add(item2);
                }
            }

            return gizmoList;            
        }

        private void ClearReplication()
        {
            for (int i = 0; i < this.BillStack.Count; i++)
            {
                if (this.BillStack[i].recipe.defName == "ArcaneForge_Replication")
                {
                    List<Pawn> mapPawns = this.Map.mapPawns.AllPawnsSpawned;
                    for(int j =0; j < mapPawns.Count; j++)
                    {
                        if(mapPawns[j].IsColonist && mapPawns[j].RaceProps.Humanlike && mapPawns[j].CurJob != null && mapPawns[j].CurJob.bill != null)
                        {
                            if (mapPawns[j].CurJob.bill.recipe.defName == this.BillStack[i].recipe.defName)
                            {
                                mapPawns[j].jobs.EndCurrentJob(Verse.AI.JobCondition.Incompletable, false);
                            }
                        }
                    }
                    
                    this.BillStack.Bills.Remove(this.BillStack[i]);
                }
            }
            
            
        }

        public void Replicate()
        {
            CheckForUnfinishedThing();

            RecipeDef forgeRecipe = TorannMagicDefOf.ArcaneForge_Replication;
            RecipeDef replicant = null;
            List<RecipeDef> potentialRecipes = new List<RecipeDef>();
            potentialRecipes.Clear();

            IEnumerable<RecipeDef> enumerable = from def in DefDatabase<RecipeDef>.AllDefs
                                                where (def.defName.Contains(this.targetThing.def.defName))
                                                select def;

            foreach (RecipeDef current in enumerable)
            {
                potentialRecipes.Add(current);
            }

            RecipeDef gemstoneRecipe = null;
            gemstoneRecipe = CheckForGemstone();

            if(gemstoneRecipe != null)
            {
                potentialRecipes.Add(gemstoneRecipe);
            }

            if(potentialRecipes.Count > 0)
            {
                replicant = potentialRecipes.RandomElement();

                IngredientCount ic = new IngredientCount();
                if (replicant.ingredients != null)
                {
                    for (int i = 0; i < replicant.ingredients.Count; i++)
                    {
                        if (!replicant.ingredients[i].IsFixedIngredient && !targetThing.def.MadeFromStuff)
                        {
                            Messages.Message("TM_ReplicatedUnrecognizedIngredient".Translate(this.targetThing.def.LabelCap), MessageTypeDefOf.RejectInput);
                            goto EndReplicate;
                        }
                    }

                    forgeRecipe.ingredients.Clear();

                    if (replicant.ingredients.Count > 0)
                    {
                        for (int i = 0; i < replicant.ingredients.Count; i++)
                        {
                            if (replicant.ingredients[i].filter.ToString() != "ingredients")
                            {
                                forgeRecipe.ingredients.Add(replicant.ingredients[i]);
                            }
                        }
                    }
                }


                if (targetThing.Stuff != null && targetThing.def.MadeFromStuff)
                {

                    ic.filter.SetAllow(targetThing.Stuff, true);
                    ic.SetBaseCount(targetThing.def.costStuffCount);
                    forgeRecipe.ingredients.Add(ic);
                }

                forgeRecipe.workAmount = replicant.workAmount;
                forgeRecipe.description = replicant.description;
                forgeRecipe.label = "Replicate " + replicant.label;
                forgeRecipe.unfinishedThingDef = replicant.unfinishedThingDef;
                forgeRecipe.products = replicant.products;

                EndReplicate:;

                if(forgeRecipe.ingredients.Count == 0)
                {
                    forgeRecipe.ingredients.Clear();

                    replicant = TorannMagicDefOf.ArcaneForge_Replication_Restore;
                    if (replicant.ingredients.Count > 0)
                    {
                        for (int i = 0; i < replicant.ingredients.Count; i++)
                        {
                            if (replicant.ingredients[i].filter.ToString() != "ingredients")
                            {
                                forgeRecipe.ingredients.Add(replicant.ingredients[i]);
                            }
                        }

                        forgeRecipe.workAmount = replicant.workAmount;
                        forgeRecipe.description = replicant.description;
                        forgeRecipe.label = replicant.label;
                        forgeRecipe.unfinishedThingDef = replicant.unfinishedThingDef;
                        forgeRecipe.products = replicant.products;
                    }
                }
            }
            else
            {
                Messages.Message("TM_FoundNoReplicateRecipe".Translate(this.targetThing.def.defName), MessageTypeDefOf.CautionInput);
            }
      
        }

        private void CheckForUnfinishedThing()
        {
            Thing unfinishedThing = this.Position.GetFirstItem(this.Map);
            if(unfinishedThing != null && unfinishedThing.def.isUnfinishedThing)
            {
                unfinishedThing.Destroy(DestroyMode.Cancel);
            }
        }

        private RecipeDef CheckForGemstone()
        {
            RecipeDef returnedRecipe = null;
            String gemString = "Cut";
            String gemType = "";
            String gemQual = "";
            if (this.targetThing.def.defName.Contains("maxMP"))
            {
                gemType = "MPGem";
            }
            if (this.targetThing.def.defName.Contains("mpRegenRate"))
            {
                gemType = "MPRegenRateGem";
            }
            if (this.targetThing.def.defName.Contains("mpCost"))
            {
                gemType = "MPCostGem";
            }
            if (this.targetThing.def.defName.Contains("coolDown"))
            {
                gemType = "CoolDownGem";
            }
            if (this.targetThing.def.defName.Contains("xpGain"))
            {
                gemType = "XPGainGem";
            }
            if (this.targetThing.def.defName.Contains("arcaneRes"))
            {
                gemType = "ArcaneResGem";
            }
            if (this.targetThing.def.defName.Contains("arcaneDmg"))
            {
                gemType = "ArcaneDmgGem";
            }
            if(this.targetThing.def.defName.Contains("_major"))
            {
                gemQual = "Major";
            }
            if (this.targetThing.def.defName.Contains("_minor"))
            {
                gemQual = "Minor";
            }

            gemString += gemQual;
            gemString += gemType;


            IEnumerable<RecipeDef> enumerable = from def in DefDatabase<RecipeDef>.AllDefs
                                                where (def.defName == gemString)
                                                select def;

            foreach (RecipeDef current in enumerable)
            {
                returnedRecipe = current;
            }

            return returnedRecipe;
        }
    }
}
