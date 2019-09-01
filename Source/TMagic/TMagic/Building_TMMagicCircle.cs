using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using System.Diagnostics;
using UnityEngine;
using RimWorld;


namespace TorannMagic
{
    [StaticConstructorOnStartup]
    public class Building_TMMagicCircle : Building_WorkTable
    {

        private static readonly Vector2 BarSize = new Vector2(1.2f, 0.2f);
        private static readonly Material EnergyBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.0f, 0.0f, 1f), false);
        private static readonly Material EnergyBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.4f, 0.4f, 0.4f), false);

        private bool isActive = false;
        private int matRng = 0;
        private float matMagnitude = 0;

        private List<Pawn> activeMageList = new List<Pawn>();

        public override void ExposeData()
        {
            base.ExposeData();
        }

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            //LessonAutoActivator.TeachOpportunity(ConceptDef.Named("TM_Portals"), OpportunityType.GoodToKnow);
        }

        //[DebuggerHidden]
        //public override IEnumerable<Gizmo> GetGizmos()
        //{
        //    foreach (Gizmo g in base.GetGizmos())
        //    {
        //        yield return g;
        //    }
        //    if (DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>() != null)
        //    {
        //        yield return new Command_Action
        //        {
        //            action = new Action(this.MakeMatchingStockpile),
        //            hotKey = KeyBindingDefOf.Misc3,
        //            defaultDesc = "TM_CommandMakePortalStockpileDesc".Translate(),
        //            icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true),
        //            defaultLabel = "TM_CommandMakePortalStockpileLabel".Translate()
        //        };
        //    }
        //}

        //public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
        //{
        //    List<FloatMenuOption> list = new List<FloatMenuOption>();
           
        //    return list;
        //}

        public override void Tick()
        {
            if (Find.TickManager.TicksGame % 10 == 0)
            {
                
            }
        }

        public override void Draw()
        {
            base.Draw();
            if (this.IsActive)
            {
                Vector3 vector = base.DrawPos;
                vector.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
                Vector3 s = new Vector3(matMagnitude, matMagnitude, matMagnitude);
                Matrix4x4 matrix = default(Matrix4x4);
                float angle = 0f;
                matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
                //if (matRng == 0)
                //{
                //    Graphics.DrawMesh(MeshPool.plane10, matrix, Building_TMPortal.portalMat_1, 0);
                //}
                //else if (matRng == 1)
                //{
                //    Graphics.DrawMesh(MeshPool.plane10, matrix, Building_TMPortal.portalMat_2, 0);
                //}
                //else if (matRng == 2)
                //{
                //    Graphics.DrawMesh(MeshPool.plane10, matrix, Building_TMPortal.portalMat_3, 0);
                //}
                //else if (matRng == 3)
                //{
                //    Graphics.DrawMesh(MeshPool.plane10, matrix, Building_TMPortal.portalMat_4, 0);
                //}
                //else
                //{
                //    Graphics.DrawMesh(MeshPool.plane10, matrix, Building_TMPortal.portalMat_5, 0);
                //}
            }
        }
    }
}
