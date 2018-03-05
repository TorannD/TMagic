using Verse;
using UnityEngine;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    public class Building_TMCooler : Building
    {

        private float arcaneEnergyCur = 0;
        private float arcaneEnergyMax = 1;

        private static readonly Material coolerMat_1 = MaterialPool.MatFrom("Other/cooler", false);
        private static readonly Material coolerMat_2 = MaterialPool.MatFrom("Other/coolerB", false);
        private static readonly Material coolerMat_3 = MaterialPool.MatFrom("Other/coolerC", false);

        private int matRng = 0;
        private float matMagnitude = 1;

        private bool initialized = false;

        //public override void SpawnSetup(Map map, bool respawningAfterLoad)
        //{
        //    base.SpawnSetup(map, respawningAfterLoad);
        //    //LessonAutoActivator.TeachOpportunity(ConceptDef.Named("TM_Portals"), OpportunityType.GoodToKnow);
        //}
                
        public override void Tick()
        {
            if(!initialized)
            {
                initialized = true;
            }
            if(Find.TickManager.TicksGame % 8 == 0)
            {
                this.matRng++;
                if(this.matRng >= 3)
                {
                    matRng = 0;
                }
            }
            base.Tick();

        }        

        public override void Draw()
        {
            base.Draw();

            Vector3 vector = base.DrawPos;
            vector.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
            Vector3 s = new Vector3(matMagnitude, matMagnitude, matMagnitude);
            Matrix4x4 matrix = default(Matrix4x4);
            float angle = 0f;
            matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
            if (matRng == 0)
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, Building_TMCooler.coolerMat_1, 0);
            }
            else if (matRng == 1)
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, Building_TMCooler.coolerMat_2, 0);
            }
            else 
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, Building_TMCooler.coolerMat_3, 0);
            }            
        }
    }
}
