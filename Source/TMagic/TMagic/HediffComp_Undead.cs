﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using AbilityUser;
using Verse;
using UnityEngine;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    class HediffComp_Undead : HediffComp
    {
        private bool necroValid = true;
        private int lichStrike = 0;
        private bool initialized = false;

        public Pawn linkedPawn = null;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_References.Look<Pawn>(ref linkedPawn, "linkedPawn", false);
        }

        public override string CompLabelInBracketsExtra => linkedPawn != null ? linkedPawn.LabelShort + base.CompLabelInBracketsExtra : base.CompLabelInBracketsExtra;

        public string labelCap
        {
            get
            {
                if (this.linkedPawn != null)
                {
                    return base.Def.LabelCap + "(" + this.linkedPawn.LabelShort + ")";
                }
                return base.Def.LabelCap;
            }
        }

        public string label
        {
            get
            {
                if (this.linkedPawn != null)
                {
                    return base.Def.label + "(" + this.linkedPawn.LabelShort + ")";
                }
                return base.Def.label;
            }
        }

        private void Initialize()
        {
            bool spawned = base.Pawn.Spawned;
            if (spawned)
            {
                //MoteMaker.ThrowLightningGlow(base.Pawn.TrueCenter(), base.Pawn.Map, 3f);
            }            
        }

        private void UpdateHediff()
        {
            if(this.linkedPawn != null)
            {
                CompAbilityUserMagic comp = linkedPawn.TryGetComp<CompAbilityUserMagic>();
                if(comp != null)
                {
                    int ver = TM_Calc.GetMagicSkillLevel(linkedPawn, comp.MagicData.MagicPowerSkill_RaiseUndead, "TM_RaiseUndead", "_ver");
                    if(this.parent.Severity != ver + .5f)
                    {
                        this.parent.Severity = .5f + ver;
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
                if (!initialized)
                {
                    initialized = true;
                    this.Initialize();
                }
            }

            if(Find.TickManager.TicksGame % 16 == 0)
            {
                IEnumerable<Hediff> hdEnum = this.Pawn.health.hediffSet.GetHediffs<Hediff>();
                foreach(Hediff hd in hdEnum)
                {
                    if(hd.def.defName == "SpaceHypoxia")
                    {
                        this.Pawn.health.RemoveHediff(hd);
                        break;
                    }
                }
            }

            if (Find.TickManager.TicksGame % 6000 == 0)
            {
                if (base.Pawn.RaceProps.Animal)
                {
                    if (base.Pawn.training.CanAssignToTrain(TrainableDefOf.Tameness).Accepted)
                    {
                        while (!base.Pawn.training.HasLearned(TrainableDefOf.Tameness))
                        {
                            base.Pawn.training.Train(TrainableDefOf.Tameness, null);
                        }
                    }

                    if (base.Pawn.training.CanAssignToTrain(TrainableDefOf.Obedience).Accepted)
                    {
                        while (!base.Pawn.training.HasLearned(TrainableDefOf.Obedience))
                        {
                            base.Pawn.training.Train(TrainableDefOf.Obedience, null);
                        }
                    }

                    if (base.Pawn.training.CanAssignToTrain(TrainableDefOf.Release).Accepted)
                    {
                        while (!base.Pawn.training.HasLearned(TrainableDefOf.Release))
                        {
                            base.Pawn.training.Train(TrainableDefOf.Release, null);
                        }
                    }

                    if (base.Pawn.training.CanAssignToTrain(TorannMagicDefOf.Haul).Accepted)
                    {
                        while (!base.Pawn.training.HasLearned(TorannMagicDefOf.Haul))
                        {
                            base.Pawn.training.Train(TorannMagicDefOf.Haul, null);
                        }
                    }

                    if (base.Pawn.training.CanAssignToTrain(TorannMagicDefOf.Rescue).Accepted)
                    {
                        while (!base.Pawn.training.HasLearned(TorannMagicDefOf.Rescue))
                        {
                            base.Pawn.training.Train(TorannMagicDefOf.Rescue, null);
                        }
                    }
                }
            }
            bool flag4 = Find.TickManager.TicksGame % 600 == 0 && this.Pawn.def != TorannMagicDefOf.TM_SkeletonR && this.Pawn.def != TorannMagicDefOf.TM_GiantSkeletonR;
            if (flag4)
            {
                UpdateHediff();
                necroValid = false;
                if (base.Pawn != null && !linkedPawn.DestroyedOrNull())
                {
                    necroValid = true;
                    lichStrike = 0; 
                }
                else
                {
                    lichStrike++;
                }

                if (!necroValid && lichStrike > 2)
                {
                    if (base.Pawn.Map != null)
                    {
                        TM_MoteMaker.ThrowScreamMote(base.Pawn.Position.ToVector3(), base.Pawn.Map, .8f, 255, 255, 255);
                        base.Pawn.Kill(null, null);
                    }
                    else
                    {
                        base.Pawn.Kill(null, null);
                    }
                }
                else
                {
                    List<Need> needs = base.Pawn.needs.AllNeeds;
                    for (int i = 0; i < needs.Count; i++)
                    {
                        if (needs[i].def == NeedDefOf.Food || needs[i].def == NeedDefOf.Joy || needs[i].def == NeedDefOf.Rest || needs[i].def.defName == "Mood" || needs[i].def.defName == "Beauty" ||
                            needs[i].def.defName == "Comfort" || needs[i].def.defName == "Outdoors" || needs[i].def.defName == "RoomSize" || needs[i].def.defName == "Bladder" || needs[i].def.defName == "Hygiene")
                        {
                            needs[i].CurLevel = needs[i].MaxLevel;
                        }
                    }
                    //if (base.Pawn.needs.food != null)
                    //{
                    //    base.Pawn.needs.food.CurLevel = base.Pawn.needs.food.MaxLevel;
                    //}
                    //if (base.Pawn.needs.rest != null)
                    //{
                    //    base.Pawn.needs.rest.CurLevel = base.Pawn.needs.rest.MaxLevel;
                    //}

                    //if (base.Pawn.IsColonist)
                    //{
                    //    base.Pawn.needs.beauty.CurLevel = .5f;
                    //    base.Pawn.needs.comfort.CurLevel = .5f;
                    //    base.Pawn.needs.joy.CurLevel = .5f;
                    //    base.Pawn.needs.mood.CurLevel = .5f;
                    //    base.Pawn.needs.space.CurLevel = .5f;
                    //}
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
                                            current.Heal(2.0f);
                                            num--;
                                            num2--;
                                        }
                                        else
                                        {
                                            current.Heal(1.0f);
                                            num--;
                                            num2--;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    using (IEnumerator<Hediff> enumerator = pawn.health.hediffSet.GetHediffsTendable().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Hediff rec = enumerator.Current;
                            if (rec.TendableNow()) // && !currentTendable.IsPermanent()
                            {
                                rec.Tended(1, 1);
                            }
                        }
                    }

                    using (IEnumerator<Hediff> enumerator = pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Hediff rec = enumerator.Current;
                            if (!rec.IsPermanent())
                            {
                                if (rec.def.defName == "Cataract" || rec.def.defName == "HearingLoss" || rec.def.defName.Contains("ToxicBuildup") || rec.def.defName == "Abasia" || rec.def.defName == "BloodRot")
                                {
                                    pawn.health.RemoveHediff(rec);
                                }
                                if ((rec.def.defName == "Blindness" || rec.def.defName.Contains("Asthma") || rec.def.defName == "Cirrhosis" || rec.def.defName == "ChemicalDamageModerate") || rec.def.defName =="Scaria")
                                {
                                    pawn.health.RemoveHediff(rec);
                                }
                                if ((rec.def.defName == "Frail" || rec.def.defName == "BadBack" || rec.def.defName.Contains("Carcinoma") || rec.def.defName == "ChemicalDamageSevere"))
                                {
                                    pawn.health.RemoveHediff(rec);
                                }
                                if ((rec.def.defName.Contains("Alzheimers") || rec.def.defName == "Dementia" || rec.def.defName.Contains("HeartArteryBlockage") || rec.def.defName == "CatatonicBreakdown"))
                                {
                                    pawn.health.RemoveHediff(rec);
                                }
                            }
                            if (rec.def.makesSickThought)
                            {
                                pawn.health.RemoveHediff(rec);
                            }
                        }
                    }                        
                }
            }
            
        }
    }
}
