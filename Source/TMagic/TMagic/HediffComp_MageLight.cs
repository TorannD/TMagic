using RimWorld;
using Verse;

namespace TorannMagic
{
    public class HediffComp_MageLight : HediffComp
    {
        private bool initializing = true;
        CompProperties_Glower gProps = new CompProperties_Glower();
        CompGlower glower = new CompGlower();
        IntVec3 oldPos = default(IntVec3);
        CompAbilityUserMagic comp;

        public ColorInt glowColor = new ColorInt(255, 255, 204, 1);

        public string labelCap
        {
            get
            {
                return base.Def.LabelCap;
            }
        }

        public string label
        {
            get
            {
                return base.Def.label;
            }
        }

        private void Initialize()
        {
            bool spawned = base.Pawn.Spawned;
            if (spawned && base.Pawn.Map != null)
            {
                MoteMaker.ThrowLightningGlow(base.Pawn.TrueCenter(), base.Pawn.Map, 3f);
                this.glower = new CompGlower();
                gProps.glowColor = glowColor;
                gProps.glowRadius = 7f;
                glower.parent = this.Pawn;
                glower.Initialize(gProps);
                comp = base.Pawn.GetComp<CompAbilityUserMagic>();
            }
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            bool flag = base.Pawn != null;
            if (flag)
            {
                if (initializing)
                {
                    initializing = false;
                    this.Initialize();
                }
            }

            if (this.glower != null && glower.parent != null && comp != null)
            {
                if (this.Pawn != null && this.Pawn.Map != null)
                {
                    if (oldPos != default(IntVec3))
                    {
                        this.Pawn.Map.mapDrawer.MapMeshDirty(oldPos, MapMeshFlag.Things);
                        this.Pawn.Map.glowGrid.DeRegisterGlower(glower);
                    }
                    if (this.Pawn.Map.skyManager.CurSkyGlow < 0.8f && !comp.mageLightSet)
                    {
                        this.Pawn.Map.mapDrawer.MapMeshDirty(this.Pawn.Position, MapMeshFlag.Things);
                        oldPos = this.Pawn.Position;
                        this.Pawn.Map.glowGrid.RegisterGlower(glower);                        
                    }
                }
            }
            else
            {
                initializing = false;
                this.Initialize();
            }
        }

        public override void CompPostPostRemoved()
        {
            if (oldPos != default(IntVec3))
            {
                this.Pawn.Map.mapDrawer.MapMeshDirty(oldPos, MapMeshFlag.Things);
                this.Pawn.Map.glowGrid.DeRegisterGlower(glower);
            }
            base.CompPostPostRemoved();
        }
    }
}