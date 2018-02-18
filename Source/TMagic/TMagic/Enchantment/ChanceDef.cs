using RimWorld;
using System;
using Verse;

namespace TorannMagic.Enchantment
{
    public class ChanceDef : Def
    {
        public TechLevelRange techLevel = new TechLevelRange();

        public ThingFilter match = new ThingFilter();

        public QualityChances chances = new QualityChances();

        public bool Allows(Thing thing)
        {
            TechLevel techLevel = thing.def.techLevel;
            return techLevel >= this.techLevel.min && techLevel <= this.techLevel.max && this.match.Allows(thing);
        }

        public float Chance(QualityCategory qc)
        {
            switch (qc)
            {
                case (QualityCategory)0:
                    return this.chances.awful;
                case (QualityCategory)1:
                    return this.chances.shoddy;
                case (QualityCategory)2:
                    return this.chances.poor;
                case (QualityCategory)3:
                    return this.chances.normal;
                case (QualityCategory)4:
                    return this.chances.good;
                case (QualityCategory)5:
                    return this.chances.superior;
                case (QualityCategory)6:
                    return this.chances.excellent;
                case (QualityCategory)7:
                    return this.chances.masterwork;
                case (QualityCategory)8:
                    return this.chances.legendary;
                default:
                    return 0f;
            }
        }

        public override void ResolveReferences()
        {
            base.ResolveReferences();
            this.match.ResolveReferences();
        }
    }
}
