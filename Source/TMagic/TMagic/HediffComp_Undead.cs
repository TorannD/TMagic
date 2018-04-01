using System;
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
        private bool initializing = true;

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
                MoteMaker.ThrowLightningGlow(base.Pawn.TrueCenter(), base.Pawn.Map, 3f);
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
                necroValid = false;
                if (base.Pawn.Map != null)
                {
                    foreach (Pawn current in base.Pawn.Map.mapPawns.PawnsInFaction(base.Pawn.Faction))
                    {
                        if (current.RaceProps.Humanlike)
                        {
                            if (current.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || current.story.traits.HasTrait(TorannMagicDefOf.Lich))
                            {
                                //necromancer alive to sustain undead                                
                                necroValid = true;
                                lichStrike = 0;
                            }
                        }
                    }
                    if(necroValid == false)  //give a buffer that allows flight or other temporary despawning of the necromancers or lichs before destroying undead
                    {
                        if (lichStrike < 3)
                        {
                            lichStrike++;
                            necroValid = true;
                        }
                    }
                }
                else //for caravans
                {
                    foreach (Pawn current in base.Pawn.holdingOwner)
                    {
                        if (current != null)
                        {
                            if (current.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || current.story.traits.HasTrait(TorannMagicDefOf.Lich))
                            {
                                necroValid = true;
                            }
                        }
                    }
                }

                if (!necroValid && base.Pawn.Map != null)
                {
                    TM_MoteMaker.ThrowScreamMote(base.Pawn.Position.ToVector3(), base.Pawn.Map, .8f, 255, 255, 255);
                    base.Pawn.Kill(null, null);
                }
                else if (!necroValid && base.Pawn.Map == null)
                {
                    base.Pawn.Kill(null, null);
                }
                else
                {
                    if (base.Pawn.needs.food != null)
                    {
                        base.Pawn.needs.food.CurLevel = base.Pawn.needs.food.MaxLevel;
                    }
                    if (base.Pawn.needs.rest != null)
                    {
                        base.Pawn.needs.rest.CurLevel = 1.01f;
                    }

                    if (base.Pawn.IsColonist)
                    {
                        base.Pawn.needs.beauty.CurLevel = .5f;
                        base.Pawn.needs.comfort.CurLevel = .5f;
                        base.Pawn.needs.joy.CurLevel = .5f;
                        base.Pawn.needs.mood.CurLevel = .5f;
                        base.Pawn.needs.space.CurLevel = .5f;
                    }
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
                                        bool flag5 = current.CanHealNaturally() && !current.IsOld();
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

                    using (IEnumerator<BodyPartRecord> enumerator = pawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            BodyPartRecord rec = enumerator.Current;
                            IEnumerable<Hediff_Injury> arg_BB_0 = pawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                            Func<Hediff_Injury, bool> arg_BB_1;
                            arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);
                            foreach (Hediff_Injury currentTendable in arg_BB_0.Where(arg_BB_1))
                            {
                                if (currentTendable.TendableNow && !currentTendable.IsOld())
                                {
                                    currentTendable.Tended(1, 1);
                                }
                            }
                        }
                    }

                    using (IEnumerator<Hediff> enumerator = pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Hediff rec = enumerator.Current;
                            if (!rec.IsOld())
                            {
                                if (rec.def.defName == "Cataract" || rec.def.defName == "HearingLoss" || rec.def.defName.Contains("ToxicBuildup"))
                                {
                                    pawn.health.RemoveHediff(rec);
                                }
                                if ((rec.def.defName == "Blindness" || rec.def.defName.Contains("Asthma") || rec.def.defName == "Cirrhosis" || rec.def.defName == "ChemicalDamageModerate"))
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
                            if(rec.def.makesSickThought)
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
