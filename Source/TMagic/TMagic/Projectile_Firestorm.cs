using RimWorld;
using Verse;
using AbilityUser;
using System.Linq;


namespace TorannMagic
{
	public class Projectile_Firestorm : Projectile_AbilityBase
	{
        private int age = 0;
        private int duration = 420;
        private int lastStrikeTiny = 0;
        private int lastStrikeSmall = 0;
        private int lastStrikeLarge = 0;
        private int[] ticksTillHeavy = new int[200];
        private IntVec3[] shrapnelPos = new IntVec3[200];
        private int heavyCount = 0;
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
            Scribe_Values.Look<int>(ref this.duration, "duration", 420, false);
            Scribe_Values.Look<int>(ref this.lastStrikeTiny, "lastStrikeTiny", 0, false);
            Scribe_Values.Look<int>(ref this.lastStrikeSmall, "lastStrikeSmall", 0, false);
            Scribe_Values.Look<int>(ref this.lastStrikeLarge, "lastStrikeLarge", 0, false);
            Scribe_Values.Look<int>(ref this.heavyCount, "heavyCount", 0, false);

        }

        public void Initialize(Map map)
        {
            
            pawn = this.launcher as Pawn;
            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            pwr = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Firestorm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Firestorm_pwr");
            ver = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Firestorm.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_Firestorm_ver");
            duration = duration + (60 * ver.level);
            cellRect = CellRect.CenteredOn(base.Position, (int)(base.def.projectile.explosionRadius + .5*(pwr.level + ver.level)));
            cellRect.ClipInsideMap(map);
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
            if (this.age > lastStrikeLarge + Rand.Range((200/(1+pwr.level))+20, (duration/(1+pwr.level))+40) && impactPos.Standable(map) && impactPos.InBounds(map))
            {
                this.lastStrikeLarge = this.age;
                SkyfallerMaker.SpawnSkyfaller(TorannMagicDefOf.TM_Firestorm_Large, impactPos, map);
                CellRect cellRectSec = CellRect.CenteredOn(impactPos, (int)(TorannMagicDefOf.TM_Firestorm_Large.skyfaller.explosionRadius + 2));
                for (int j = 0; j < (int)Rand.Range(1 + ver.level, 5 + ver.level); j++)
                {
                    this.shrapnelPos[heavyCount] = cellRectSec.RandomCell;
                    this.ticksTillHeavy[heavyCount] = TorannMagicDefOf.TM_Firestorm_Large.skyfaller.ticksToImpactRange.RandomInRange + 8;
                    heavyCount++;
                }                
            }
            impactPos = cellRect.RandomCell;
            if (this.age > lastStrikeTiny + Rand.Range(7-pwr.level, 20-pwr.level) && impactPos.Standable(map) && impactPos.InBounds(map))
            {
                this.lastStrikeTiny = this.age;
                SkyfallerMaker.SpawnSkyfaller(TorannMagicDefOf.TM_Firestorm_Tiny, impactPos, map);
            }
            impactPos = cellRect.RandomCell;
            if ( this.age > lastStrikeSmall + Rand.Range(18-(2*pwr.level), 42-(2*pwr.level)) && impactPos.Standable(map) && impactPos.InBounds(map))
            {
                this.lastStrikeSmall = this.age;
                SkyfallerMaker.SpawnSkyfaller(TorannMagicDefOf.TM_Firestorm_Small, impactPos, map);
            }

            for (int i = 0; i <= heavyCount; i++)
            {
                if (ticksTillHeavy[i] == 0)
                {
                    GenExplosion.DoExplosion(shrapnelPos[heavyCount], map, .4f, TMDamageDefOf.DamageDefOf.TM_Firestorm_Small, this.launcher, Rand.Range(5, this.def.projectile.GetDamageAmount(1,null)), 0, SoundDefOf.BulletImpact_Ground, def, this.equipmentDef, null, null, 0f, 1, false, null, 0f, 1, 0.2f, false);
                    ticksTillHeavy[i]--;
                }
                else
                {
                    ticksTillHeavy[i]--;
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

    }
}
