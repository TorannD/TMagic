using RimWorld;
using Verse;
using AbilityUser;
using System.Linq;


namespace TorannMagic
{
	public class Projectile_Blizzard : Projectile_AbilityBase
	{
        private int age = 0;
        private int duration = 1200;
        private int lastStrikeTiny = 0;
        private int lastStrikeSmall = 0;
        private int lastStrikeLarge = 0;
        private int snowCount = 0;
        private int[] ticksTillSnow = new int[400];
        private IntVec3[] snowPos = new IntVec3[400];
        private bool initialized = false;
        CellRect cellRect;
        Pawn pawn;
        MagicPowerSkill pwr;
        MagicPowerSkill ver;

        public override void ExposeData()
        {
            base.ExposeData();
            //Scribe_Values.Look<bool>(ref this.initialized, "initialized", false, false);
            Scribe_Values.Look<int>(ref this.age, "age", 0, false);
            Scribe_Values.Look<int>(ref this.duration, "duration", 720, false);
            Scribe_Values.Look<int>(ref this.lastStrikeTiny, "lastStrikeTiny", 0, false);
            Scribe_Values.Look<int>(ref this.lastStrikeSmall, "lastStrikeSmall", 0, false);
            Scribe_Values.Look<int>(ref this.lastStrikeLarge, "lastStrikeLarge", 0, false);
        }

        public void Initialize(Map map)
        {
            pawn = this.launcher as Pawn;
            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            pwr = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Blizzard.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Blizzard_pwr");
            ver = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Blizzard.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Blizzard_ver");
            cellRect = CellRect.CenteredOn(base.Position, (int)(base.def.projectile.explosionRadius + (.75 *(ver.level + pwr.level))));
            cellRect.ClipInsideMap(map);
            duration = duration + (240 * ver.level);
            initialized = true;
        }

        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            base.Impact(hitThing);
            ThingDef def = this.def;
            IntVec3 impactPos;
            if (!initialized)
            {
                Initialize(map);
            }
            impactPos = cellRect.RandomCell;
            if (this.age > lastStrikeLarge + Rand.Range(300 - (pwr.level * 45), duration/(4 + pwr.level)) && impactPos.Standable(map) && impactPos.InBounds(map))
            {
                this.lastStrikeLarge = this.age;
                SkyfallerMaker.SpawnSkyfaller(TorannMagicDefOf.TM_Blizzard_Large, impactPos, map);
                MoteMaker.ThrowSmoke(impactPos.ToVector3(), map, 5f);
                ticksTillSnow[snowCount] = TorannMagicDefOf.TM_Blizzard_Large.skyfaller.ticksToImpactRange.RandomInRange+4;
                snowPos[snowCount] = impactPos;
                snowCount++;
            }
            impactPos = cellRect.RandomCell;
            if (this.age > lastStrikeTiny + Rand.Range(7-(pwr.level), 22-(2*pwr.level)) && impactPos.Standable(map) && impactPos.InBounds(map))
            {
                this.lastStrikeTiny = this.age;
                SkyfallerMaker.SpawnSkyfaller(TorannMagicDefOf.TM_Blizzard_Tiny, impactPos, map);
                MoteMaker.ThrowSmoke(impactPos.ToVector3(), map, 1f);
                ticksTillSnow[snowCount] = TorannMagicDefOf.TM_Blizzard_Tiny.skyfaller.ticksToImpactRange.RandomInRange +2;
                snowPos[snowCount] = impactPos;
                snowCount++;
            }
            impactPos = cellRect.RandomCell;
            if ( this.age > lastStrikeSmall + Rand.Range(40-(2*pwr.level), 80-(4*pwr.level)) && impactPos.Standable(map) && impactPos.InBounds(map))
            {
                this.lastStrikeSmall = this.age;
                SkyfallerMaker.SpawnSkyfaller(TorannMagicDefOf.TM_Blizzard_Small, impactPos, map);
                MoteMaker.ThrowSmoke(impactPos.ToVector3(), map, 3f);
                ticksTillSnow[snowCount] = TorannMagicDefOf.TM_Blizzard_Small.skyfaller.ticksToImpactRange.RandomInRange+2;
                snowPos[snowCount] = impactPos;
                snowCount++;
            }

            for(int i = 0; i <= snowCount; i++)
            {
                if (ticksTillSnow[i] == 0)
                {
                    AddSnowRadial(snowPos[i], map, 2f, 2f);
                    MoteMaker.ThrowSmoke(snowPos[i].ToVector3(), map, 4f);                
                    ticksTillSnow[i]--;
                }
                else
                {
                    ticksTillSnow[i]--;
                }
            }

        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            bool flag = this.age < duration;
            if (!flag)
            {
                base.Destroy(mode);
            }
        }

        public override void Tick()
        {
            base.Tick();
            this.age++;
        }

        public static void AddSnowRadial(IntVec3 center, Map map, float radius, float depth)
        {
            int num = GenRadial.NumCellsInRadius(radius);
            for (int i = 0; i < num; i++)
            {
                IntVec3 intVec = center + GenRadial.RadialPattern[i];
                if (intVec.InBounds(map))
                {
                    float lengthHorizontal = (center - intVec).LengthHorizontal;
                    float num2 = 1f - lengthHorizontal / radius;
                    map.snowGrid.AddDepth(intVec, num2 * depth);

                }
            }
        }

    }
}
