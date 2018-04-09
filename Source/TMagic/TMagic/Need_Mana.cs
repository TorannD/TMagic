using RimWorld;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace TorannMagic
{
    public class Need_Mana : Need  
    {
        public const float BaseGainPerTickRate = 150f;

        public const float BaseFallPerTick = 1E-05f;

        public const float ThreshVeryLow = 0.1f;

        public const float ThreshLow = 0.3f;

        public const float ThreshSatisfied = 0.5f;

        public const float ThreshHigh = 0.7f;

        public const float ThreshVeryHigh = 0.9f;

        public float lastNeed;

        public int ticksUntilBaseSet = 500;

        private int lastGainTick;

        public override float CurLevel
        {            
            get => base.CurLevel;
            set => base.CurLevel = Mathf.Clamp(value, 0f, this.pawn.GetComp<CompAbilityUserMagic>().maxMP);            
        }

        public override float MaxLevel => this.pawn.GetComp<CompAbilityUserMagic>().maxMP;

        public ManaPoolCategory CurCategory
        {
            get
            {
                bool flag = this.CurLevel < 0.1f;
                ManaPoolCategory result;
                if (flag)
                {
                    result = ManaPoolCategory.Drained;
                }
                else
                {
                    bool flag2 = this.CurLevel < 0.3f;
                    if (flag2)
                    {
                        result = ManaPoolCategory.Weakened;
                    }
                    else
                    {
                        bool flag3 = this.CurLevel < 0.5f;
                        if (flag3)
                        {
                            result = ManaPoolCategory.Steady;
                        }
                        else
                        {
                            bool flag4 = this.CurLevel < 0.7f;
                            if (flag4)
                            {
                                result = ManaPoolCategory.Flowing;
                            }
                            else
                            {
                                result = ManaPoolCategory.Surging;
                            }
                        }
                    }
                }
                return result;
            }
        }

        public override int GUIChangeArrow
        {
            get
            {
                return this.GainingNeed ? 1 : -1;
            }
        }

        public override float CurInstantLevel
        {
            get
            {
                return this.CurLevel;
            }
        }

        private bool GainingNeed
        {
            get
            {
                return Find.TickManager.TicksGame < this.lastGainTick + 10;
            }
        }

        static Need_Mana()
        {
        }

        public Need_Mana(Pawn pawn) : base(pawn)
		    {
            this.lastGainTick = -999;
            this.threshPercents = new List<float>();
            this.threshPercents.Add((0.25f / this.MaxLevel));
            this.threshPercents.Add((0.5f / this.MaxLevel));
            this.threshPercents.Add((0.75f / this.MaxLevel));         
        }

        private void AdjustThresh()
        {
            this.threshPercents.Clear();
            this.threshPercents.Add((0.25f / this.MaxLevel));
            this.threshPercents.Add((0.5f / this.MaxLevel));
            this.threshPercents.Add((0.75f / this.MaxLevel));
            if (this.MaxLevel > 1)
            {
                this.threshPercents.Add((1f / this.MaxLevel));
            }
            if (this.MaxLevel > 1.25f)
            {
                this.threshPercents.Add((1.25f / this.MaxLevel));
            }
            if (this.MaxLevel > 1.5f)
            {
                this.threshPercents.Add((1.5f / this.MaxLevel));
            }
            if (this.MaxLevel > 1.75f)
            {
                this.threshPercents.Add((1.75f / this.MaxLevel));
            }            
        }

        public override void SetInitialLevel()
        {
            this.CurLevel = 0.7f;
        }

        public void GainNeed(float amount)
        {
            if (base.pawn.Map != null && !base.pawn.Dead)
            {
                Pawn pawn = base.pawn;
                CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
                if (comp.IsMagicUser)
                {
                    if (!pawn.Faction.IsPlayer)
                    {
                        amount *= (0.025f);
                        amount = Mathf.Min(amount, this.MaxLevel - this.CurLevel);
                        this.curLevelInt += amount;
                    }
                    else
                    {
                        ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                        
                        MagicPowerSkill manaRegen = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_global_regen.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_regen_pwr");
                        amount *= (((0.0012f * comp.mpRegenRate)) * settingsRef.needMultiplier);
                        amount = Mathf.Min(amount, this.MaxLevel - this.CurLevel);
                        float necroReduction = 0;
                        int necroCount = 0;
                        int undeadCount = 0;
                        float averageNecroMana=0;

                        if(comp.summonedMinions.Count >0)
                        {
                            MagicPowerSkill summonerEff = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_SummonMinion.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_SummonMinion_eff");
                            amount -= (0.0012f * (comp.summonedMinions.Count * (.2f - (.01f * summonerEff.level))));
                        }

                        if (pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Lich))
                        {
                            foreach (Pawn current in this.pawn.Map.mapPawns.PawnsInFaction(this.pawn.Faction))
                            {
                                if (current.RaceProps.Humanlike)
                                {
                                    if (current.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || current.story.traits.HasTrait(TorannMagicDefOf.Lich))
                                    {
                                        
                                        CompAbilityUserMagic tempComp = current.GetComp<CompAbilityUserMagic>();
                                        if(tempComp.Mana.CurLevel < 0.01f && current != pawn)
                                        {
                                            necroCount--;
                                        }
                                        necroCount++;
                                        averageNecroMana += tempComp.Mana.CurLevel;
                                    }
                                    if (current.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadHD))
                                    {
                                        undeadCount += 2;
                                    }
                                }
                                if (current.RaceProps.Animal)
                                {
                                    if (current.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadAnimalHD))
                                    {
                                        undeadCount++;
                                    }
                                }
                            }
                            averageNecroMana = averageNecroMana / necroCount;
                            if (averageNecroMana < 0.01f)
                            {
                                foreach (Pawn current in this.pawn.Map.mapPawns.PawnsInFaction(this.pawn.Faction))
                                {
                                    if (this.CurLevel < 0.01f && undeadCount > 0 && (current.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadHD) || current.health.hediffSet.HasHediff(TorannMagicDefOf.TM_UndeadAnimalHD)))
                                    {
                                        //consume corpse
                                        Messages.Message("TM_UndeadCollapsed".Translate(new object[]
                                        {
                                            pawn.LabelShort,
                                            current.LabelShort
                                        }), MessageTypeDefOf.NegativeEvent);
                                        if (!current.RaceProps.Animal)
                                        {
                                            current.inventory.DropAllNearPawn(current.Position, false, true);
                                            current.equipment.DropAllEquipment(current.Position, false);
                                            current.apparel.DropAll(current.Position, false);
                                        }
                                        TM_MoteMaker.ThrowBloodSquirt(current.Position.ToVector3Shifted(), current.Map, 2.5f);
                                        current.Destroy();
                                        undeadCount--;
                                        this.curLevelInt = .12f + (.025f * manaRegen.level);
                                    }
                                }
                            }
                            MagicPowerSkill eff = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_RaiseUndead.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_RaiseUndead_eff");
                            necroReduction = ((0.0012f * (.15f - (.15f * (.1f * eff.level))) / necroCount) * undeadCount);
                            //Log.Message("" + pawn.LabelShort + " is 1 of " + necroCount + " contributing necros and had necro reduction of " + necroReduction);

                        }

                        if (pawn.Map.GameConditionManager.ConditionIsActive(TorannMagicDefOf.ManaDrain))
                        {
                            this.curLevelInt = this.curLevelInt - amount - necroReduction;                            
                            if (this.CurLevel < .01)
                            {
                                float pain = pawn.health.hediffSet.PainTotal;
                                float con = pawn.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness);
                                float sev = (.015f * (1 + (3 * pain) + (1 - con)));
                                HealthUtility.AdjustSeverity(pawn, TorannMagicDefOf.TM_ManaSickness, sev);
                            }
                            if (this.CurLevel < 0)
                            {
                                this.CurLevel = 0;
                            }
                        }
                        else if (pawn.Map.GameConditionManager.ConditionIsActive(TorannMagicDefOf.ManaSurge) && !pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_ArcaneSickness))
                        {
                            if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_ArcaneWeakness))
                            {
                                this.curLevelInt += amount - necroReduction;
                            }
                            else
                            {
                                this.curLevelInt += (amount * 2.25f) - necroReduction;
                            }
                        }
                        else if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_ArcaneSickness))
                        {
                            //no mana gain
                            this.curLevelInt -= necroReduction;
                        }
                        else if (pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_ArcaneWeakness))
                        {
                            //reduced mana gain if weakened
                            this.curLevelInt += (amount * 0.25f) - necroReduction;
                        }
                        else
                        {
                            this.curLevelInt += amount - necroReduction;
                        }

                        if ((lastNeed - this.curLevelInt) > .25f && (lastNeed - this.curLevelInt) < .45f)
                        {
                            if ((lastNeed - this.curLevelInt) >= .45f && (lastNeed - this.curLevelInt) < .79f)
                            {
                                if ((lastNeed - this.curLevelInt) >= .79f && (lastNeed - this.curLevelInt) < 2)
                                {
                                    //0.0 to 0.21 max
                                    float sev = 8.5f + ((lastNeed - this.curLevelInt) - .79f) * 40;
                                    HealthUtility.AdjustSeverity(pawn, TorannMagicDefOf.TM_ArcaneWeakness, sev);
                                }
                                else
                                {
                                    //0.0 to 0.34 max
                                    float sev = 1.4f + ((lastNeed - this.curLevelInt) - .45f) * 25;
                                    HealthUtility.AdjustSeverity(pawn, TorannMagicDefOf.TM_ArcaneWeakness, sev);
                                }
                            }
                            else
                            {
                                //0.0 to 0.2 max
                                float sev = ((lastNeed - this.curLevelInt) - .25f) * 10;
                                HealthUtility.AdjustSeverity(pawn, TorannMagicDefOf.TM_ArcaneWeakness, sev);
                            }
                        }
                        comp.Mana.curLevelInt = Mathf.Clamp(comp.Mana.curLevelInt, 0f, this.MaxLevel);
                        lastNeed = this.curLevelInt;
                        this.lastGainTick = Find.TickManager.TicksGame;
                    }
                }
                else
                {
                    this.curLevelInt = 0;
                }
            }
            else
            {                
                Pawn pawn = base.pawn;
                CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
                if (comp != null)
                {
                    if (comp.IsMagicUser)
                    {
                        ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                        MagicPowerSkill manaRegen = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_global_regen.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_global_regen_pwr");
                        amount *= ((0.0012f + 0.00006f * manaRegen.level) * settingsRef.needMultiplier);
                        amount = Mathf.Min(amount, this.MaxLevel - this.CurLevel);
                        comp.Mana.curLevelInt = Mathf.Clamp(comp.Mana.curLevelInt += amount, 0f, this.MaxLevel);
                    }
                }
            }
            AdjustThresh();
        }        

        public void UseMagicPower(float amount)
        {
            this.curLevelInt = Mathf.Clamp(this.curLevelInt - amount, 0f, this.pawn.GetComp<CompAbilityUserMagic>().maxMP);
        }

        public override void NeedInterval()
        {
            this.GainNeed(1f);
        }

        public override string GetTipString()
        {
            //return base.GetTipString();
            return string.Concat(new string[]
            {
                this.LabelCap,
                ": ",
                (this.CurLevel / 1f).ToStringPercent(),
                "\n",
                this.def.description
            });
        }

        public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
        {
            bool flag = rect.height > 70f;
            if (flag)
            {
                float num = (rect.height - 70f) / 2f;
                rect.height = 70f;
                rect.y += num;
            }
            bool flag2 = Mouse.IsOver(rect);
            if (flag2)
            {
                Widgets.DrawHighlight(rect);
            }
            TooltipHandler.TipRegion(rect, new TipSignal(() => this.GetTipString(), rect.GetHashCode()));
            float num2 = 14f;
            float num3 = num2 + 15f;
            bool flag3 = rect.height < 50f;
            if (flag3)
            {
                num2 *= Mathf.InverseLerp(0f, 50f, rect.height);
            }            
            Text.Font = ((rect.height <= 55f) ? GameFont.Tiny : GameFont.Small);
            Text.Anchor = TextAnchor.LowerLeft;
            Rect rect2 = new Rect(rect.x + num3 + rect.width * 0.1f, rect.y, rect.width - num3 - rect.width * 0.1f, rect.height / 2f);
            Widgets.Label(rect2, base.LabelCap);
            GUI.color = Color.magenta;
            Text.Anchor = TextAnchor.UpperLeft;
            Rect rect3 = new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
            rect3 = new Rect(rect3.x + num3, rect3.y, rect3.width - num3 * 2f, rect3.height - num2);
            Widgets.FillableBar(rect3, base.CurLevelPercentage);
            bool flag4 = this.threshPercents != null;
            if (flag4)
            {
                for (int i = 0; i < this.threshPercents.Count; i++)
                {
                    this.DrawBarThreshold(rect3, this.threshPercents[i]);
                }
            }
            float curInstantLevelPercentage = Mathf.Clamp(this.CurLevel / this.MaxLevel, 0f, 1f); // base.CurInstantLevelPercentage;
            bool flag5 = curInstantLevelPercentage >= 0f;
            if (flag5)
            {
                base.DrawBarInstantMarkerAt(rect3, curInstantLevelPercentage);
            }
            bool flag6 = !this.def.tutorHighlightTag.NullOrEmpty();
            if (flag6)
            {
                UIHighlighter.HighlightOpportunity(rect, this.def.tutorHighlightTag);
            }
            Text.Font = GameFont.Small;
        }

        private void DrawBarThreshold(Rect barRect, float threshPct)
        {
            float num = (float)((barRect.width <= 60f) ? 1 : 2);
            Rect position = new Rect(barRect.x + barRect.width * threshPct - (num - 1f), barRect.y + barRect.height / 2f, num, barRect.height / 2f);
            bool flag = threshPct < base.CurLevelPercentage;
            Texture2D image;
            if (flag)
            {
                image = BaseContent.BlackTex;
                GUI.color = new Color(1f, 1f, 1f, 0.9f);
            }
            else
            {
                image = BaseContent.GreyTex;
                GUI.color = new Color(1f, 1f, 1f, 0.5f);
            }
            GUI.DrawTexture(position, image);
            GUI.color = Color.white;
        }
    }
}
