using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    class HediffComp_RangerBond : HediffComp
    {
        private bool initializing = true;
        public Pawn bonderPawn = null;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_References.Look<Pawn>(ref this.bonderPawn, "bonderPawn", false);
        }

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
            if (spawned)
            {
                MoteMaker.ThrowHeatGlow(base.Pawn.DrawPos.ToIntVec3(), base.Pawn.Map, 2f);
            }
            List<Pawn> mapPawns = this.Pawn.Map.mapPawns.AllPawnsSpawned;
            for (int i = 0; i < mapPawns.Count(); i++)
            {
                if (!mapPawns[i].DestroyedOrNull() && mapPawns[i].Spawned && !mapPawns[i].Downed && mapPawns[i].RaceProps.Humanlike)
                {
                    CompAbilityUserMight comp = mapPawns[i].GetComp<CompAbilityUserMight>();
                    if (comp.IsMightUser && comp.bondedPet != null)
                    {
                        if (comp.bondedPet == this.Pawn)
                        {
                            this.bonderPawn = comp.Pawn;
                            break;
                        }                        
                    }
                }
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
            bool flag4 = Find.TickManager.TicksGame % 600 == 0;
            if (flag4)
            {
                Pawn pawn = base.Pawn;
                int num = 1;
                int num2 = 1;

                using (IEnumerator<BodyPartRecord> enumerator = pawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        BodyPartRecord rec = enumerator.Current;
                        bool flag2 = num > 0;

                        if (flag2)
                        {
                            IEnumerable<Hediff_Injury> arg_BB_0 = pawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                            Func<Hediff_Injury, bool> arg_BB_1;

                            arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                            foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                            {
                                bool flag3 = num2 > 0;
                                if (flag3)
                                {
                                    bool flag5 = current.CanHealNaturally() && !current.IsPermanent();
                                    if (flag5)
                                    {
                                        current.Heal(1.0f);
                                        num--;
                                        num2--;
                                    }
                                    else
                                    {
                                        current.Heal(.2f);
                                        num--;
                                        num2--;
                                    }
                                }
                            }
                        }
                    }
                }
                if (this.bonderPawn != null && !this.bonderPawn.Destroyed && !this.bonderPawn.Dead)
                {
                    RefreshBond();
                    UpdateBond();                    
                }
                else
                {
                    this.Pawn.health.RemoveHediff(this.Pawn.health.hediffSet.GetFirstHediffOfDef(this.parent.def));
                    //this.Pawn.SetFactionDirect(null);
                }
            }
        }

        private void UpdateBond()
        {
            CompAbilityUserMight comp = this.bonderPawn.GetComp<CompAbilityUserMight>();
            MightPowerSkill ver = comp.MightData.MightPowerSkill_AnimalFriend.FirstOrDefault((MightPowerSkill x) => x.label == "TM_AnimalFriend_ver");
            this.parent.Severity = .5f + ver.level;
        }

        public void RefreshBond()
        {
            if (this.Pawn.training.CanBeTrained(TrainableDefOf.Tameness))
            {
                while (!this.Pawn.training.HasLearned(TrainableDefOf.Tameness))
                {
                    this.Pawn.training.Train(TrainableDefOf.Tameness, this.bonderPawn);
                }
            }

            if (this.Pawn.training.CanBeTrained(TrainableDefOf.Obedience))
            {
                while (!this.Pawn.training.HasLearned(TrainableDefOf.Obedience))
                {
                    this.Pawn.training.Train(TrainableDefOf.Obedience, this.bonderPawn);
                }
            }

            if (this.Pawn.training.CanBeTrained(TrainableDefOf.Release))
            {
                while (!this.Pawn.training.HasLearned(TrainableDefOf.Release))
                {
                    this.Pawn.training.Train(TrainableDefOf.Release, this.bonderPawn);
                }
            }

            if (this.Pawn.training.CanBeTrained(TorannMagicDefOf.Haul))
            {
                while (!this.Pawn.training.HasLearned(TorannMagicDefOf.Haul))
                {
                    this.Pawn.training.Train(TorannMagicDefOf.Haul, this.bonderPawn);
                }
            }

            if (this.Pawn.training.CanBeTrained(TorannMagicDefOf.Rescue))
            {
                while (!this.Pawn.training.HasLearned(TorannMagicDefOf.Rescue))
                {
                    this.Pawn.training.Train(TorannMagicDefOf.Rescue, this.bonderPawn);
                }
            }
        }
    }
}
