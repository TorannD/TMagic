using Verse;
using UnityEngine;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    public class Building_TMPowerNode : Building
    {

        private float arcaneEnergyCur = 0;
        private float arcaneEnergyMax = 1;

        private static readonly Material powernodeMat_1 = MaterialPool.MatFrom("Other/energynode_1", false);
        private static readonly Material powernodeMat_2 = MaterialPool.MatFrom("Other/energynode_2", false);
        private static readonly Material powernodeMat_3 = MaterialPool.MatFrom("Other/energynode_3", false);
        private static readonly Material powernodeMat_4 = MaterialPool.MatFrom("Other/energynode_4", false);

        private int matRng = 0;
        private float matMagnitude = 1;

        private bool initialized = false;

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            //LessonAutoActivator.TeachOpportunity(ConceptDef.Named("TM_Portals"), OpportunityType.GoodToKnow);
        }
                
        public override void Tick()
        {
            if(!initialized)
            {
                initialized = true;
            }
            if(Find.TickManager.TicksGame % 8 == 0)
            {
                this.matRng++;
                if(this.matRng >= 4)
                {
                    matRng = 0;
                }
            }
            base.Tick();
        }        

        public override void Draw()
        {
            Vector3 vector = base.DrawPos;
            vector.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
            Vector3 s = new Vector3(matMagnitude, matMagnitude, matMagnitude);
            Matrix4x4 matrix = default(Matrix4x4);
            float angle = 0f;
            matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
            if (matRng == 0)
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, Building_TMPowerNode.powernodeMat_1, 0);
            }
            else if (matRng == 1)
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, Building_TMPowerNode.powernodeMat_2, 0);
            }
            else if (matRng == 2)
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, Building_TMPowerNode.powernodeMat_3, 0);
            }
            else 
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, Building_TMPowerNode.powernodeMat_4, 0);
            }            
        }
    }
}
