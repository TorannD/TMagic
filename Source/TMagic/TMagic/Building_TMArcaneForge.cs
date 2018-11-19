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
                    this.BillStack.Bills.Remove(this.BillStack[i]);
                }
            }
        }

        public void Replicate()
        {
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

            if(potentialRecipes.Count > 0)
            {
                replicant = potentialRecipes.RandomElement();
                forgeRecipe.ingredients = replicant.ingredients;
                for (int i = 0; i < forgeRecipe.ingredients.Count; i++)
                {
                    if(!forgeRecipe.ingredients[i].IsFixedIngredient)
                    {
                        forgeRecipe.ingredients.Remove(forgeRecipe.ingredients[i]);
                    }
                }

                IngredientCount ic = new IngredientCount();
                if(targetThing.Stuff != null && targetThing.def.MadeFromStuff)
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

            }
            else
            {
                Messages.Message("TM_FoundNoReplicateRecipe".Translate(this.targetThing.def.defName), MessageTypeDefOf.CautionInput);
            }
      
        }
    }
}
