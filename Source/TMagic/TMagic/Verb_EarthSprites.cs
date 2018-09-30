using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using AbilityUser;
using Verse;


namespace TorannMagic
{
    public class Verb_EarthSprites : Verb_UseAbility
    {

        private int verVal;
        private int effVal;

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
            bool flag = false;
            CompAbilityUserMagic comp = this.CasterPawn.GetComp<CompAbilityUserMagic>();
            MagicPowerSkill eff = base.CasterPawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_EarthSprites.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EarthSprites_eff");
            MagicPowerSkill ver = base.CasterPawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_EarthSprites.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EarthSprites_ver");
            effVal = eff.level;
            verVal = ver.level;

            if(!(comp.maxMP < .6f - (.07f * verVal)))
            {
                if(this.currentTarget.IsValid && this.currentTarget.CenterVector3.InBounds(base.CasterPawn.Map))
                {
                    Building isBuilding = null;
                    TerrainDef terrain = null;
                    isBuilding = this.currentTarget.Cell.GetFirstBuilding(this.CasterPawn.Map);
                    terrain = this.currentTarget.Cell.GetTerrain(this.CasterPawn.Map);
                    if (isBuilding != null)
                    {
                        var mineable = isBuilding as Mineable;
                        if (mineable != null)
                        {
                            comp.earthSprites = this.currentTarget.Cell;
                            comp.earthSpriteType = 1;
                            comp.earthSpriteMap = this.CasterPawn.Map;
                            comp.nextEarthSpriteAction = Find.TickManager.TicksGame + 300;
                        }
                        else
                        {
                            Messages.Message("TM_InvalidTarget".Translate(new object[]
                            {
                                this.CasterPawn.LabelShort,
                                "Earth Sprites"
                            }), MessageTypeDefOf.RejectInput);
                        }
                    }
                    else if (terrain != null && (terrain.defName == "MarshyTerrain" || terrain.defName == "Mud" || terrain.defName == "Marsh" || terrain.defName == "WaterShallow" || terrain.defName == "Ice" ||
                        terrain.defName == "Sand" || terrain.defName == "Gravel" || terrain.defName == "Soil" || terrain.defName == "MossyTerrain" || terrain.defName == "SoftSand"))
                    {
                        comp.earthSprites = this.currentTarget.Cell;
                        comp.earthSpriteType = 2;
                        comp.earthSpriteMap = this.CasterPawn.Map;
                        comp.nextEarthSpriteAction = Find.TickManager.TicksGame + 20000;
                    }
                    else
                    {
                        Messages.Message("TM_InvalidTarget".Translate(new object[]
                        {
                            this.CasterPawn.LabelShort,
                            "Earth Sprites"
                        }), MessageTypeDefOf.RejectInput);
                    }
                }
                else
                {
                    Messages.Message("TM_InvalidTarget".Translate(new object[]
                    {
                        this.CasterPawn.LabelShort,
                        "Earth Sprites"
                    }), MessageTypeDefOf.RejectInput);
                }
            }
            else
            {
                Messages.Message("TM_NotEnoughMaxMana".Translate(new object[]
                {
                    this.CasterPawn.LabelShort,
                    "Earth Sprites"
                }), MessageTypeDefOf.RejectInput);
            }

            this.burstShotsLeft = 0;
            return false;
        }
    }
}
