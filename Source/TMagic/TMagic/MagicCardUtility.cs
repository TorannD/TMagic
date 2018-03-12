﻿using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace TorannMagic
{
    class MagicCardUtility //original code by Jecrell
    {
        public static Vector2 magicCardSize = new Vector2(700f, 556f);

        public static float ButtonSize = 40f;

        public static float MagicButtonSize = 46f;

        public static float MagicButtonPointSize = 24f;

        public static float HeaderSize = 24f;

        public static float TextSize = 22f;

        public static float Padding = 3f;

        public static float SpacingOffset = 22f;

        public static float SectionOffset = 8f;

        public static float ColumnSize = 245f;

        public static float SkillsColumnHeight = 113f;

        public static float SkillsColumnDivider = 114f;

        public static float SkillsTextWidth = 138f;

        public static float SkillsBoxSize = 18f;

        public static float PowersColumnHeight = 195f;

        public static float PowersColumnWidth = 123f;

        public static bool adjustedForLanguage = false;

        public static void DrawMagicCard(Rect rect, Pawn pawn)
        {
            GUI.BeginGroup(rect);
            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            bool flag = comp != null;
            if (flag)
            {
                bool flag2 = true; //comp.MagicUserLevel > 0;
                if (flag2)
                {
                    float x = Text.CalcSize("TM_Header".Translate()).x;
                    Rect rect2 = new Rect(rect.width / 2f - (x / 2), rect.y, rect.width, MagicCardUtility.HeaderSize);
                    Text.Font = GameFont.Small;
                    Widgets.Label(rect2, "TM_Header".Translate().CapitalizeFirst());
                    Text.Font = GameFont.Small;
                    Widgets.DrawLineHorizontal(rect.x - 10f, rect2.yMax, rect.width - 15f);
                    Rect rect9 = new Rect(rect.x, rect2.yMax + MagicCardUtility.Padding, rect2.width, MagicCardUtility.SkillsColumnHeight);
                    Rect inRect = new Rect(rect9.x, rect9.y + MagicCardUtility.Padding, MagicCardUtility.SkillsColumnDivider, MagicCardUtility.SkillsColumnHeight);
                    Rect inRect2 = new Rect(rect9.x + MagicCardUtility.SkillsColumnDivider, rect9.y + MagicCardUtility.Padding, rect9.width - MagicCardUtility.SkillsColumnDivider, MagicCardUtility.SkillsColumnHeight);
                    MagicCardUtility.InfoPane(inRect, pawn.GetComp<CompAbilityUserMagic>(), pawn);
                    float x5 = Text.CalcSize("TM_Spells".Translate()).x;
                    Rect rect10 = new Rect(rect.width / 2f - x5 / 2f, rect9.yMax - 60f, rect.width, MagicCardUtility.HeaderSize);
                    Text.Font = GameFont.Small;
                    Widgets.Label(rect10, "TM_Spells".Translate().CapitalizeFirst());
                    Text.Font = GameFont.Small;
                    Widgets.DrawLineHorizontal(rect.x - 10f, rect10.yMax, rect.width - 15f);
                    Rect rect11 = new Rect(rect.x, rect10.yMax + MagicCardUtility.SectionOffset, rect10.width, MagicCardUtility.PowersColumnHeight);
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.InnerFire))
                    {
                        Rect inRect3 = new Rect(rect.x, rect11.y, MagicCardUtility.PowersColumnWidth, MagicCardUtility.PowersColumnHeight);
                        if(pawn.GetComp<CompAbilityUserMagic>().spell_Firestorm == true)
                        {
                            MagicCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMagic>(), pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowersIF, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_RayofHope, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Firebolt, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Fireclaw, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Fireball, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Firestorm, null, TexButton.TMTex_SkillPointUsed);
                        }
                        else
                        {
                            MagicCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMagic>(), pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowersIF, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_RayofHope, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Firebolt, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Fireclaw, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Fireball, null, null, TexButton.TMTex_SkillPointUsed);
                        }
                        
                    }
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
                    {
                        Rect inRect3 = new Rect(rect.x, rect11.y, MagicCardUtility.PowersColumnWidth, MagicCardUtility.PowersColumnHeight);
                        if (pawn.GetComp<CompAbilityUserMagic>().spell_Blizzard == true)
                        {
                            MagicCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMagic>(), pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowersHoF, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Soothe, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Rainmaker, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Icebolt, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_FrostRay, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Snowball, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Blizzard, TexButton.TMTex_SkillPointUsed);
                        }
                        else
                        {
                            MagicCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMagic>(), pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowersHoF, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Soothe, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Rainmaker, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Icebolt, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_FrostRay, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Snowball, null, TexButton.TMTex_SkillPointUsed);
                        }
                    }
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.StormBorn))
                    {
                        Rect inRect3 = new Rect(rect.x, rect11.y, MagicCardUtility.PowersColumnWidth, MagicCardUtility.PowersColumnHeight);
                        if (pawn.GetComp<CompAbilityUserMagic>().spell_EyeOfTheStorm == true)
                        {
                            MagicCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMagic>(), pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowersSB, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_AMP, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_LightningBolt, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_LightningCloud, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_LightningStorm, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_EyeOfTheStorm, null, TexButton.TMTex_SkillPointUsed);
                        }
                        else
                        {
                            MagicCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMagic>(), pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowersSB, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_AMP, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_LightningBolt, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_LightningCloud, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_LightningStorm, null, null, TexButton.TMTex_SkillPointUsed);
                        }
                        
                    }
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.Arcanist))
                    {
                        Rect inRect3 = new Rect(rect.x, rect11.y, MagicCardUtility.PowersColumnWidth, MagicCardUtility.PowersColumnHeight);
                        if (pawn.GetComp<CompAbilityUserMagic>().spell_FoldReality == true)
                        {
                            MagicCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMagic>(), pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowersA, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Shadow, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_MagicMissile, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Blink, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Summon, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Teleport, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_FoldReality, TexButton.TMTex_SkillPointUsed);
                        }
                        else
                        {
                            MagicCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMagic>(), pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowersA, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Shadow, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_MagicMissile, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Blink, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Summon, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Teleport, null, TexButton.TMTex_SkillPointUsed);
                        }
                    }
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.Paladin))
                    {
                        Rect inRect3 = new Rect(rect.x, rect11.y, MagicCardUtility.PowersColumnWidth, MagicCardUtility.PowersColumnHeight);
                        MagicCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMagic>(), pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowersP, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Heal, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Shield, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_ValiantCharge, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Overwhelm, null, null, TexButton.TMTex_SkillPointUsed);
                    }
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.Summoner))
                    {
                        Rect inRect3 = new Rect(rect.x, rect11.y, MagicCardUtility.PowersColumnWidth, MagicCardUtility.PowersColumnHeight);
                        MagicCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMagic>(), pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowersS, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_SummonMinion, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_SummonPylon, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_SummonExplosive, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_SummonElemental, null, null, TexButton.TMTex_SkillPointUsed);
                    }
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.Druid))
                    {
                        Rect inRect3 = new Rect(rect.x, rect11.y, MagicCardUtility.PowersColumnWidth, MagicCardUtility.PowersColumnHeight);
                        if (pawn.GetComp<CompAbilityUserMagic>().spell_RegrowLimb == true)
                        {
                            MagicCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMagic>(), pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowersD, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Poison, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_SootheAnimal, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Regenerate, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_CureDisease, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_RegrowLimb, null, TexButton.TMTex_SkillPointUsed);
                        }
                        else
                        {
                            MagicCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMagic>(), pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowersD, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Poison, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_SootheAnimal, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Regenerate, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_CureDisease, null, null, TexButton.TMTex_SkillPointUsed);
                        }
                    }
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer))
                    {
                        Rect inRect3 = new Rect(rect.x, rect11.y, MagicCardUtility.PowersColumnWidth, MagicCardUtility.PowersColumnHeight);
                        MagicCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMagic>(), pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowersN, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_RaiseUndead, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_DeathMark, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_FogOfTorment, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_ConsumeCorpse, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_CorpseExplosion, null, TexButton.TMTex_SkillPointUsed);
                    }
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.Priest))
                    {
                        Rect inRect3 = new Rect(rect.x, rect11.y, MagicCardUtility.PowersColumnWidth, MagicCardUtility.PowersColumnHeight);
                        if (pawn.GetComp<CompAbilityUserMagic>().spell_Resurrection == true)
                        {
                            MagicCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMagic>(), pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowersPR, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_AdvancedHeal, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Purify, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_HealingCircle, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_BestowMight, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Resurrection, null, TexButton.TMTex_SkillPointUsed);
                        }
                        else
                        {
                            MagicCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMagic>(), pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowersPR, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_AdvancedHeal, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_Purify, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_HealingCircle, pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_BestowMight, null, null, TexButton.TMTex_SkillPointUsed);
                        }
                    }

                }
            }
            GUI.EndGroup();
        }

        public static void InfoPane(Rect inRect, CompAbilityUserMagic compMagic, Pawn pawn)
        {
            Rect rect = new Rect(inRect.x, inRect.y, inRect.width * 0.7f, MagicCardUtility.TextSize);
            Text.Font = GameFont.Tiny;
            Widgets.Label(rect, "TM_Level".Translate().CapitalizeFirst() + ": " + compMagic.MagicData.MagicUserLevel.ToString());
            Text.Font = GameFont.Tiny;
            bool godMode = DebugSettings.godMode;
            if (godMode)
            {
                Rect rect2 = new Rect(rect.xMax, inRect.y, inRect.width * 0.3f, MagicCardUtility.TextSize);
                bool flag = Widgets.ButtonText(rect2, "+", true, false, true);
                if (flag)
                {
                    compMagic.LevelUp(true);
                }
                Rect rect22 = new Rect(rect.xMax + 60f, inRect.y, 50f, MagicCardUtility.TextSize * 2);
                bool flag22 = Widgets.ButtonText(rect22, "Clear Powers", true, false, true);
                if (flag22)
                {
                    compMagic.ClearPowers();
                }
                Rect rect23 = new Rect(rect.xMax + 115f, inRect.y, 50f, MagicCardUtility.TextSize * 2);
                bool flag23 = Widgets.ButtonText(rect23, "Clear Trait", true, false, true);
                if (flag23)
                {
                    compMagic.ClearTraits();
                }
            }
            Rect rect4 = new Rect(inRect.x, rect.yMax, inRect.width, MagicCardUtility.TextSize);
            Text.Font = GameFont.Tiny;
            Widgets.Label(rect4, "TM_PointsAvail".Translate() + ": " + compMagic.MagicData.MagicAbilityPoints);
            Text.Font = GameFont.Tiny;
            Rect rect5 = new Rect(rect4.x, rect4.yMax + 3f, inRect.width + 100f, MagicCardUtility.HeaderSize * 0.6f);
            MagicCardUtility.DrawLevelBar(rect5, compMagic, pawn, inRect);
        }

        public static void DrawLevelBar(Rect rect, CompAbilityUserMagic compMagic, Pawn pawn, Rect rectG)
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
            TooltipHandler.TipRegion(rect, new TipSignal(() => MagicCardUtility.MagicXPTipString(compMagic), rect.GetHashCode()));
            float num2 = 14f;
            bool flag3 = rect.height < 50f;
            if (flag3)
            {
                num2 *= Mathf.InverseLerp(0f, 50f, rect.height);
            }
            Text.Anchor = TextAnchor.UpperLeft;
            Rect rect2 = new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height);
            rect2 = new Rect(rect2.x, rect2.y, rect2.width, rect2.height - num2);
            Widgets.FillableBar(rect2, compMagic.XPTillNextLevelPercent, (Texture2D)AccessTools.Field(typeof(Widgets), "BarFullTexHor").GetValue(null), BaseContent.GreyTex, false);
            Rect rect3 = new Rect(rect2.x + 272f + MagicCardUtility.MagicButtonPointSize, rectG.y, 136f, MagicCardUtility.TextSize);
            Rect rect31 = new Rect(rect2.x + 272f, rectG.y, MagicCardUtility.MagicButtonPointSize, MagicCardUtility.TextSize);
            Rect rect4 = new Rect(rect3.x + rect3.width + (MagicCardUtility.MagicButtonPointSize * 2), rectG.y, 136f, MagicCardUtility.TextSize);
            Rect rect41 = new Rect(rect3.x + rect3.width + MagicCardUtility.MagicButtonPointSize, rectG.y, MagicCardUtility.MagicButtonPointSize, MagicCardUtility.TextSize);
            Rect rect5 = new Rect(rect2.x + 272f + MagicCardUtility.MagicButtonPointSize, rectG.yMin + 24f, 136f, MagicCardUtility.TextSize);
            Rect rect51 = new Rect(rect2.x + 272f, rectG.yMin + 24f, MagicCardUtility.MagicButtonPointSize, MagicCardUtility.TextSize);

            List<MagicPowerSkill> skill1 = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_global_regen;
            List<MagicPowerSkill> skill2 = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_global_eff;
            List<MagicPowerSkill> skill3 = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_global_spirit;

            using (List<MagicPowerSkill>.Enumerator enumerator1 = skill1.GetEnumerator())
            {
                while (enumerator1.MoveNext())
                {
                    MagicPowerSkill skill = enumerator1.Current;
                    TooltipHandler.TipRegion(rect3, new TipSignal(() => enumerator1.Current.desc.Translate(), rect3.GetHashCode()));
                    bool flag11 = skill.level >= skill.levelMax || compMagic.MagicData.MagicAbilityPoints == 0;
                    if (flag11)
                    {
                        Widgets.Label(rect3, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                    }
                    else
                    {
                        bool flag12 = Widgets.ButtonText(rect31, "+", true, false, true) && compMagic.AbilityUser.Faction == Faction.OfPlayer;
                        Widgets.Label(rect3, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                        if (flag12)
                        {
                            if (skill.label == "TM_global_regen_pwr")
                            {
                                compMagic.LevelUpSkill_global_regen(skill.label);
                                skill.level++;
                                compMagic.MagicData.MagicAbilityPoints -= 1;
                            }
                        }
                    }
                }
            }
            using (List<MagicPowerSkill>.Enumerator enumerator2 = skill2.GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    MagicPowerSkill skill = enumerator2.Current;
                    TooltipHandler.TipRegion(rect4, new TipSignal(() => enumerator2.Current.desc.Translate(), rect4.GetHashCode()));
                    bool flag11 = skill.level >= skill.levelMax || compMagic.MagicData.MagicAbilityPoints == 0;
                    if (flag11)
                    {
                        Widgets.Label(rect4, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                    }
                    else
                    {
                        bool flag12 = Widgets.ButtonText(rect41, "+", true, false, true) && compMagic.AbilityUser.Faction == Faction.OfPlayer;
                        Widgets.Label(rect4, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                        if (flag12)
                        {
                            if (skill.label == "TM_global_eff_pwr")
                            {
                                compMagic.LevelUpSkill_global_eff(skill.label);
                                skill.level++;
                                compMagic.MagicData.MagicAbilityPoints -= 1;
                            }
                        }
                    }
                }
            }
            using (List<MagicPowerSkill>.Enumerator enumerator3 = skill3.GetEnumerator())
            {
                while (enumerator3.MoveNext())
                {
                    MagicPowerSkill skill = enumerator3.Current;
                    TooltipHandler.TipRegion(rect5, new TipSignal(() => enumerator3.Current.desc.Translate(), rect5.GetHashCode()));
                    bool flag11 = skill.level >= skill.levelMax || compMagic.MagicData.MagicAbilityPoints == 0;
                    if (flag11)
                    {
                        Widgets.Label(rect5, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                    }
                    else
                    {
                        bool flag12 = Widgets.ButtonText(rect51, "+", true, false, true) && compMagic.AbilityUser.Faction == Faction.OfPlayer;
                        Widgets.Label(rect5, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                        if (flag12)
                        {
                            if (skill.label == "TM_global_spirit_pwr")
                            {
                                compMagic.LevelUpSkill_global_spirit(skill.label);
                                skill.level++;
                                compMagic.MagicData.MagicAbilityPoints -= 1;
                            }
                        }
                    }
                }
            }
        }

        public static string MagicXPTipString(CompAbilityUserMagic compMagic)
        {
            string result;

            result = string.Concat(new string[]
            {
                compMagic.MagicUserXP.ToString(),
                " / ",
                compMagic.MagicUserXPTillNextLevel.ToString(),
                "\n",
                "TM_MagicXPDesc".Translate()
            });

            return result;
        }

        public static void PowersGUIHandler(Rect inRect, CompAbilityUserMagic compMagic, List<MagicPower> MagicPowers, List<MagicPowerSkill> MagicPowerSkill1, List<MagicPowerSkill> MagicPowerSkill2, List<MagicPowerSkill> MagicPowerSkill3, List<MagicPowerSkill> MagicPowerSkill4, List<MagicPowerSkill> MagicPowerSkill5, List<MagicPowerSkill> MagicPowerSkill6, Texture2D pointTexture)
        {
            float num = inRect.y;
            int itnum = 1;
            bool flag999;
            using (List<MagicPower>.Enumerator enumerator = MagicPowers.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    MagicPower power = enumerator.Current;
                    Text.Font = GameFont.Small;
                    Rect rect = new Rect(MagicCardUtility.magicCardSize.x / 2f - MagicCardUtility.MagicButtonSize, num, MagicCardUtility.MagicButtonSize, MagicCardUtility.MagicButtonSize);
                    if (itnum > 1)
                    {
                        Widgets.DrawLineHorizontal(0f + 20f, rect.y - 2f, 700f - 40f);
                    }
                    if (power.level < 3 && (power.abilityDef == TorannMagicDefOf.TM_RayofHope || power.abilityDef == TorannMagicDefOf.TM_RayofHope_I || power.abilityDef == TorannMagicDefOf.TM_RayofHope_II || 
                        power.abilityDef == TorannMagicDefOf.TM_Soothe || power.abilityDef == TorannMagicDefOf.TM_Soothe_I || power.abilityDef == TorannMagicDefOf.TM_Soothe_II || 
                        power.abilityDef == TorannMagicDefOf.TM_Shadow || power.abilityDef == TorannMagicDefOf.TM_Shadow_I || power.abilityDef == TorannMagicDefOf.TM_Shadow_II || 
                        power.abilityDef == TorannMagicDefOf.TM_AMP || power.abilityDef == TorannMagicDefOf.TM_AMP_I || power.abilityDef == TorannMagicDefOf.TM_AMP_II || 
                        power.abilityDef == TorannMagicDefOf.TM_Shield || power.abilityDef == TorannMagicDefOf.TM_Shield_I || power.abilityDef == TorannMagicDefOf.TM_Shield_II || 
                        power.abilityDef == TorannMagicDefOf.TM_Blink || power.abilityDef == TorannMagicDefOf.TM_Blink_I || power.abilityDef == TorannMagicDefOf.TM_Blink_II || 
                        power.abilityDef == TorannMagicDefOf.TM_Summon || power.abilityDef == TorannMagicDefOf.TM_Summon_I || power.abilityDef == TorannMagicDefOf.TM_Summon_II || 
                        power.abilityDef == TorannMagicDefOf.TM_MagicMissile || power.abilityDef == TorannMagicDefOf.TM_MagicMissile_I || power.abilityDef == TorannMagicDefOf.TM_MagicMissile_II || 
                        power.abilityDef == TorannMagicDefOf.TM_FrostRay || power.abilityDef == TorannMagicDefOf.TM_FrostRay_I || power.abilityDef == TorannMagicDefOf.TM_FrostRay_II ||
                        power.abilityDef == TorannMagicDefOf.TM_SootheAnimal || power.abilityDef == TorannMagicDefOf.TM_SootheAnimal_I || power.abilityDef == TorannMagicDefOf.TM_SootheAnimal_II ||
                        power.abilityDef == TorannMagicDefOf.TM_DeathMark || power.abilityDef == TorannMagicDefOf.TM_DeathMark_I || power.abilityDef == TorannMagicDefOf.TM_DeathMark_II ||
                        power.abilityDef == TorannMagicDefOf.TM_ConsumeCorpse || power.abilityDef == TorannMagicDefOf.TM_ConsumeCorpse_I || power.abilityDef == TorannMagicDefOf.TM_ConsumeCorpse_II ||
                        power.abilityDef == TorannMagicDefOf.TM_CorpseExplosion || power.abilityDef == TorannMagicDefOf.TM_CorpseExplosion_I || power.abilityDef == TorannMagicDefOf.TM_CorpseExplosion_II ||
                        power.abilityDef == TorannMagicDefOf.TM_HealingCircle || power.abilityDef == TorannMagicDefOf.TM_HealingCircle_I || power.abilityDef == TorannMagicDefOf.TM_HealingCircle_II ||
                        power.abilityDef == TorannMagicDefOf.TM_BestowMight || power.abilityDef == TorannMagicDefOf.TM_BestowMight_I || power.abilityDef == TorannMagicDefOf.TM_BestowMight_II))
                    {
                        TooltipHandler.TipRegion(rect, () => string.Concat(new string[]
                        {
                            power.abilityDef.label,
                            "\n\nCurrent Level:\n",
                            power.abilityDescDef.description, 
                            "\n\nNext Level:\n",
                            power.nextLevelAbilityDescDef.description,  
                            "\n\n",
                            "TM_CheckPointsForMoreInfo".Translate()
                       }), 398462);
                    }
                    else
                    {
                        TooltipHandler.TipRegion(rect, () => string.Concat(new string[]
							{
                            power.abilityDef.label,
                            "\n\n",
                            power.abilityDescDef.description,
                            "\n\n",
                            "TM_CheckPointsForMoreInfo".Translate()
                            }), 398462);
                    }

                    float x2 = Text.CalcSize("TM_Effeciency".Translate()).x;
                    float x3 = Text.CalcSize("TM_Versatility".Translate()).x;
                    Rect rect3 = new Rect(0f + MagicCardUtility.SpacingOffset, rect.y + 2f, MagicCardUtility.magicCardSize.x, MagicCardUtility.ButtonSize * 1.15f);

                    Rect rect5 = new Rect(rect3.x + rect3.width / 2f - x2, rect3.y, (rect3.width - 20f) / 3f, rect3.height);
                    Rect rect6 = new Rect(rect3.width - x3 * 2f, rect3.y, rect3.width / 3f, rect3.height);

                    float x4 = Text.CalcSize(" # / # ").x;
                    //bool flag9 = power.abilityDef.label == "Ray of Hope" || power.abilityDef.label == "Soothing Breeze" || power.abilityDef.label == "Frost Ray" || power.abilityDef.label == "AMP" || power.abilityDef.label == "Shadow" || power.abilityDef.label == "Magic Missile" || power.abilityDef.label == "Blink" || power.abilityDef.label == "Summon" || power.abilityDef.label == "Shield"; //add all other buffs or xml based upgrades
                    
                    if (power.abilityDef.defName == "TM_RayofHope" || power.abilityDef.defName ==  "TM_RayofHope_I" || power.abilityDef.defName ==  "TM_RayofHope_II" || power.abilityDef.defName == "TM_RayofHope_III" || 
                        power.abilityDef.defName == "TM_Soothe" || power.abilityDef.defName == "TM_Soothe_I" || power.abilityDef.defName == "TM_Soothe_II" || power.abilityDef.defName == "TM_Soothe_III" ||
                        power.abilityDef.defName == "TM_FrostRay" || power.abilityDef.defName == "TM_FrostRay_I" || power.abilityDef.defName == "TM_FrostRay_II" || power.abilityDef.defName == "TM_FrostRay_III" ||
                        power.abilityDef.defName == "TM_AMP" || power.abilityDef.defName == "TM_AMP_I" || power.abilityDef.defName == "TM_AMP_II" || power.abilityDef.defName == "TM_AMP_III" ||
                        power.abilityDef.defName == "TM_Shadow" || power.abilityDef.defName == "TM_Shadow_I" || power.abilityDef.defName == "TM_Shadow_II" || power.abilityDef.defName == "TM_Shadow_III" ||
                        power.abilityDef.defName == "TM_Blink" || power.abilityDef.defName == "TM_Blink_I" || power.abilityDef.defName == "TM_Blink_II" || power.abilityDef.defName == "TM_Blink_III" ||
                        power.abilityDef.defName == "TM_Summon" || power.abilityDef.defName == "TM_Summon_I" || power.abilityDef.defName == "TM_Summon_II" || power.abilityDef.defName == "TM_Summon_III" ||
                        power.abilityDef.defName == "TM_MagicMissile" || power.abilityDef.defName == "TM_MagicMissile_I" || power.abilityDef.defName == "TM_MagicMissile_II" || power.abilityDef.defName == "TM_MagicMissile_III" ||
                        power.abilityDef.defName == "TM_Shield" || power.abilityDef.defName == "TM_Shield_I" || power.abilityDef.defName == "TM_Shield_II" || power.abilityDef.defName == "TM_Shield_III" ||
                        power.abilityDef.defName == "TM_SootheAnimal" || power.abilityDef.defName == "TM_SootheAnimal_I" || power.abilityDef.defName == "TM_SootheAnimal_II" || power.abilityDef.defName == "TM_SootheAnimal_III" ||
                        power.abilityDef.defName == "TM_DeathMark" || power.abilityDef.defName == "TM_DeathMark_I" || power.abilityDef.defName == "TM_DeathMark_II" || power.abilityDef.defName == "TM_DeathMark_III" ||
                        power.abilityDef.defName == "TM_ConsumeCorpse" || power.abilityDef.defName == "TM_ConsumeCorpse_I" || power.abilityDef.defName == "TM_ConsumeCorpse_II" || power.abilityDef.defName == "TM_ConsumeCorpse_III" ||
                        power.abilityDef.defName == "TM_CorpseExplosion" || power.abilityDef.defName == "TM_CorpseExplosion_I" || power.abilityDef.defName == "TM_CorpseExplosion_II" || power.abilityDef.defName == "TM_CorpseExplosion_III" ||
                        power.abilityDef.defName == "TM_HealingCircle" || power.abilityDef.defName == "TM_HealingCircle_I" || power.abilityDef.defName == "TM_HealingCircle_II" || power.abilityDef.defName == "TM_HealingCircle_III" ||
                        power.abilityDef.defName == "TM_BestowMight" || power.abilityDef.defName == "TM_BestowMight_I" || power.abilityDef.defName == "TM_BestowMight_II" || power.abilityDef.defName == "TM_BestowMight_III")
                    {
                        flag999 = true;
                    }
                    else
                    {
                        flag999 = false;
                    }
                    if (enumerator.Current.learned != true)
                    {
                        Widgets.DrawTextureFitted(rect, power.Icon, 1f);
                        Rect rectLearn = new Rect(rect.xMin - 44f, rect.yMin, 40f, MagicCardUtility.MagicButtonPointSize);
                        if (compMagic.MagicData.MagicAbilityPoints >= enumerator.Current.learnCost)
                        {
                            Text.Font = GameFont.Tiny;                            
                            bool flagLearn = Widgets.ButtonText(rectLearn, "TM_Learn".Translate(), true, false, true) && compMagic.AbilityUser.Faction == Faction.OfPlayer;
                            if (flagLearn)
                            {
                                enumerator.Current.learned = true;
                                compMagic.AddPawnAbility(enumerator.Current.abilityDef);
                                compMagic.MagicData.MagicAbilityPoints -= enumerator.Current.learnCost;
                            }
                        }
                        else
                        {
                            Rect rectToLearn = new Rect(rect.xMin - 98f, rect.yMin, 100f, MagicButtonPointSize);
                            Text.Font = GameFont.Tiny;
                            bool flagLearn = Widgets.ButtonText(rectToLearn, "" + enumerator.Current.learnCost + " points to " + "TM_Learn".Translate(), false, false, false) && compMagic.AbilityUser.Faction == Faction.OfPlayer;
                        }
                    }
                    else
                    {
                        bool flag10 = enumerator.Current.level >= 3 || compMagic.MagicData.MagicAbilityPoints == 0;
                        if (flag10)
                        {
                            if (flag999)
                            {
                                Widgets.DrawTextureFitted(rect, power.Icon, 1f);
                                Rect rect19 = new Rect(rect.xMax, rect.yMin, x4, MagicCardUtility.TextSize);
                                Widgets.Label(rect19, " " + enumerator.Current.level + " / 3");
                            }
                            else
                            {
                                Widgets.DrawTextureFitted(rect, power.Icon, 1f);
                            }

                        }
                        else
                        {
                            if (flag999)
                            {
                                Rect rect10 = new Rect(rect.xMax, rect.yMin, x4, MagicCardUtility.TextSize);
                                bool flag1 = Widgets.ButtonImage(rect, power.Icon) && compMagic.AbilityUser.Faction == Faction.OfPlayer;
                                Widgets.Label(rect10, " " + power.level + " / 3");
                                if (flag1)
                                {
                                    compMagic.LevelUpPower(power);
                                    compMagic.MagicData.MagicAbilityPoints -= 1;
                                    compMagic.FixPowers();
                                }
                            }
                            else
                            {
                                Widgets.DrawTextureFitted(rect, power.Icon, 1f);
                            }
                        }
                    }

                    Text.Font = GameFont.Tiny;
                    float num2 = rect3.x;
                    if (itnum == 1 && MagicPowerSkill1 != null)
                    {
                        using (List<MagicPowerSkill>.Enumerator enumerator1 = MagicPowerSkill1.GetEnumerator())
                        {
                            while (enumerator1.MoveNext())
                            {
                                var drawMagicCardResult = DrawMagicCardForPower(rect3, num2, compMagic, enumerator.Current, enumerator1.Current);
								num2 = drawMagicCardResult.xPos;
                            }
                        }
                        itnum++;
                    }
                    else if (itnum == 2 && MagicPowerSkill2 != null)
                    {
                        using (List<MagicPowerSkill>.Enumerator enumerator2 = MagicPowerSkill2.GetEnumerator())
                        {
                            while (enumerator2.MoveNext())
                            {
                                var drawMagicCardResult = DrawMagicCardForPower(rect3, num2, compMagic, enumerator.Current, enumerator2.Current);
								num2 = drawMagicCardResult.xPos;
                            }
                        }
                        itnum++;
                    }
                    else if (itnum == 3 && MagicPowerSkill3 != null)
                    {
                        using (List<MagicPowerSkill>.Enumerator enumerator3 = MagicPowerSkill3.GetEnumerator())
                        {
                            while (enumerator3.MoveNext())
                            {
                                var drawMagicCardResult = DrawMagicCardForPower(rect3, num2, compMagic, enumerator.Current, enumerator3.Current);
								num2 = drawMagicCardResult.xPos;
                            }
                        }
                        itnum++;
                    }
                    else if (itnum == 4 && MagicPowerSkill4 != null)
                    {
                        using (List<MagicPowerSkill>.Enumerator enumerator4 = MagicPowerSkill4.GetEnumerator())
                        {
                            while (enumerator4.MoveNext())
                            {
                                var drawMagicCardResult = DrawMagicCardForPower(rect3, num2, compMagic, enumerator.Current, enumerator4.Current);
								num2 = drawMagicCardResult.xPos;
                            }
                        }
                        itnum++;
                    }
                    else if (itnum == 5 && MagicPowerSkill5 != null)
                    {
                        using (List<MagicPowerSkill>.Enumerator enumerator5 = MagicPowerSkill5.GetEnumerator())
                        {
                            while (enumerator5.MoveNext())
                            {
                                var drawMagicCardResult = DrawMagicCardForPower(rect3, num2, compMagic, enumerator.Current, enumerator5.Current);
								num2 = drawMagicCardResult.xPos;
                            }
                        }
                        itnum++;
                    }
                    else if (itnum == 6 && MagicPowerSkill6 != null)
                    {
                        using (List<MagicPowerSkill>.Enumerator enumerator6 = MagicPowerSkill6.GetEnumerator())
                        {
                            while (enumerator6.MoveNext())
                            {
								var drawMagicCardResult = DrawMagicCardForPower(rect3, num2, compMagic, enumerator.Current, enumerator6.Current);
								num2 = drawMagicCardResult.xPos;
							}
                        }
                        itnum++;
                    }
                    else
                    {
                        //Log.Message("No skill iteration found.");
                    }
                    num += MagicCardUtility.MagicButtonSize + MagicCardUtility.TextSize + 4f;//MagicCardUtility.SpacingOffset; //was 4f                    
                }  
            }
        }

		public class DrawMagicCardForPowerReturn
		{
			public float xPos;
			public CompAbilityUserMagic caum;
			public MagicPowerSkill power;
		}

		public static DrawMagicCardForPowerReturn DrawMagicCardForPower(Rect parentRect, float xPos, CompAbilityUserMagic compMagic, MagicPower classPower, MagicPowerSkill powerToPutOnCard)
		{
			// List<MagicPower>.Enumerator -> Enumerator for the magic power skill list provided by the wizard class.
			//		MagicPower power = enumerator.Current;
			// (List<MagicPowerSkill>.Enumerator enumerator1 = MagicPowerSkill1.GetEnumerator(); -> Enumerator for the current magic power skill being iterated (as it may consist of several levels). Comes from the individual magic power parameters.
			var result = new DrawMagicCardForPowerReturn();
			result.xPos = xPos;
			result.caum = compMagic;
			result.power = powerToPutOnCard;

			Rect rect4 = new Rect(result.xPos + MagicCardUtility.MagicButtonPointSize, parentRect.yMax, MagicCardUtility.magicCardSize.x / 3f, parentRect.height);
			Rect rect41 = new Rect(result.xPos, rect4.y, MagicCardUtility.MagicButtonPointSize, MagicCardUtility.MagicButtonPointSize);
			Rect rect42 = new Rect(rect41.x, rect4.y, rect4.width - MagicCardUtility.MagicButtonPointSize, rect4.height / 2);

			TooltipHandler.TipRegion(rect42, new TipSignal(() => result.power.desc.Translate(), rect4.GetHashCode()));
			bool flag11 = result.power.level >= result.power.levelMax || result.caum.MagicData.MagicAbilityPoints == 0 || !classPower.learned;
			if (flag11)
			{
				Widgets.Label(rect4, result.power.label.Translate() + ": " + result.power.level + " / " + result.power.levelMax);
			}
			else
			{
				bool flag12 = Widgets.ButtonText(rect41, "+", true, false, true) && result.caum.AbilityUser.Faction == Faction.OfPlayer;
				Widgets.Label(rect4, result.power.label.Translate() + ": " + result.power.level + " / " + result.power.levelMax);
				if (flag12)
				{
					bool flag17 = result.caum.AbilityUser.story != null && result.caum.AbilityUser.story.WorkTagIsDisabled(WorkTags.Violent) && classPower.abilityDef.MainVerb.isViolent;
					if (flag17)
					{
						Messages.Message("IsIncapableOfViolenceLower".Translate(new object[]
						{
							result.caum.parent.LabelShort
						}), MessageTypeDefOf.RejectInput);
						return result;
					}
					var levelupResult = DoSkillLevelUp(powerToLevel: classPower, compMagic: result.caum, skill: result.power);
					result.caum = levelupResult.caum;
					result.power = levelupResult.mps;
				}
			}
			result.xPos += (MagicCardUtility.magicCardSize.x / 3) - MagicCardUtility.SpacingOffset;
			return result;
		}

		public class DoSKillLevelUpReturn
		{
			public CompAbilityUserMagic caum;
			public MagicPowerSkill mps;
		}
		
		public static DoSKillLevelUpReturn DoSkillLevelUp(MagicPower powerToLevel, CompAbilityUserMagic compMagic, MagicPowerSkill skill)
		{
			var powerToLevelName = powerToLevel.abilityDef.defName;
			if (powerToLevelName == "TM_RayofHope" || powerToLevelName == "TM_RayofHope_I" || powerToLevelName == "TM_RayofHope_II" || powerToLevelName == "TM_RayofHope_III")
			{
				compMagic.LevelUpSkill_RayofHope(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Firebolt")
			{
				compMagic.LevelUpSkill_Firebolt(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Fireclaw")
			{
				compMagic.LevelUpSkill_Fireclaw(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Fireball")
			{
				compMagic.LevelUpSkill_Fireball(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Soothe" || powerToLevelName == "TM_Soothe_I" || powerToLevelName == "TM_Soothe_II" || powerToLevelName == "TM_Soothe_III")
			{
				compMagic.LevelUpSkill_Soothe(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Icebolt")
			{
				compMagic.LevelUpSkill_Icebolt(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_FrostRay" || powerToLevelName == "TM_FrostRay_I" || powerToLevelName == "TM_FrostRay_II" || powerToLevelName == "TM_FrostRay_III")
			{
				compMagic.LevelUpSkill_FrostRay(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Snowball")
			{
				compMagic.LevelUpSkill_Snowball(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Rainmaker")
			{
				compMagic.LevelUpSkill_Rainmaker(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_AMP" || powerToLevelName == "TM_AMP_I" || powerToLevelName == "TM_AMP_II" || powerToLevelName == "TM_AMP_III")
			{
				compMagic.LevelUpSkill_AMP(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_LightningBolt")
			{
				compMagic.LevelUpSkill_LightningBolt(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_LightningCloud")
			{
				compMagic.LevelUpSkill_LightningCloud(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_LightningStorm")
			{
				compMagic.LevelUpSkill_LightningStorm(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Shadow" || powerToLevelName == "TM_Shadow_I" || powerToLevelName == "TM_Shadow_II" || powerToLevelName == "TM_Shadow_III")
			{
				compMagic.LevelUpSkill_Shadow(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_MagicMissile" || powerToLevelName == "TM_MagicMissile_I" || powerToLevelName == "TM_MagicMissile_II" || powerToLevelName == "TM_MagicMissile_III")
			{
				compMagic.LevelUpSkill_MagicMissile(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Blink" || powerToLevelName == "TM_Blink_I" || powerToLevelName == "TM_Blink_II" || powerToLevelName == "TM_Blink_III")
			{
				compMagic.LevelUpSkill_Blink(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Summon" || powerToLevelName == "TM_Summon_I" || powerToLevelName == "TM_Summon_II" || powerToLevelName == "TM_Summon_III")
			{
				compMagic.LevelUpSkill_Summon(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Teleport")
			{
				compMagic.LevelUpSkill_Teleport(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_FoldReality")
			{
				compMagic.LevelUpSkill_FoldReality(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Heal")
			{
				compMagic.LevelUpSkill_Heal(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Shield" || powerToLevelName == "TM_Shield_I" || powerToLevelName == "TM_Shield_II" || powerToLevelName == "TM_Shield_III")
			{
				compMagic.LevelUpSkill_Shield(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_ValiantCharge")
			{
				compMagic.LevelUpSkill_ValiantCharge(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Overwhelm")
			{
				compMagic.LevelUpSkill_Overwhelm(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Firestorm")
			{
				compMagic.LevelUpSkill_Firestorm(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Blizzard")
			{
				compMagic.LevelUpSkill_Blizzard(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_SummonMinion")
			{
				compMagic.LevelUpSkill_SummonMinion(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_SummonPylon")
			{
				compMagic.LevelUpSkill_SummonPylon(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_SummonExplosive")
			{
				compMagic.LevelUpSkill_SummonExplosive(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_SummonElemental")
			{
				compMagic.LevelUpSkill_SummonElemental(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Poison")
			{
				compMagic.LevelUpSkill_Poison(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_SootheAnimal" || powerToLevelName == "TM_SootheAnimal_I" || powerToLevelName == "TM_SootheAnimal_II" || powerToLevelName == "TM_SootheAnimal_III")
			{
				compMagic.LevelUpSkill_SootheAnimal(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Regenerate")
			{
				compMagic.LevelUpSkill_Regenerate(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_CureDisease")
			{
				compMagic.LevelUpSkill_CureDisease(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_RegrowLimb")
			{
				compMagic.LevelUpSkill_RegrowLimb(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_EyeOfTheStorm")
			{
				compMagic.LevelUpSkill_EyeOfTheStorm(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_RaiseUndead")
			{
				compMagic.LevelUpSkill_RaiseUndead(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_DeathMark" || powerToLevelName == "TM_DeathMark_I" || powerToLevelName == "TM_DeathMark_II" || powerToLevelName == "TM_DeathMark_III")
			{
				compMagic.LevelUpSkill_DeathMark(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_FogOfTorment")
			{
				compMagic.LevelUpSkill_FogOfTorment(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_ConsumeCorpse" || powerToLevelName == "TM_ConsumeCorpse_I" || powerToLevelName == "TM_ConsumeCorpse_II" || powerToLevelName == "TM_ConsumeCorpse_III")
			{
				compMagic.LevelUpSkill_ConsumeCorpse(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_CorpseExplosion" || powerToLevelName == "TM_CorpseExplosion_I" || powerToLevelName == "TM_CorpseExplosion_II" || powerToLevelName == "TM_CorpseExplosion_III")
			{
				compMagic.LevelUpSkill_CorpseExplosion(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_AdvancedHeal")
			{
				compMagic.LevelUpSkill_AdvancedHeal(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Purify")
			{
				compMagic.LevelUpSkill_Purify(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_HealingCircle" || powerToLevelName == "TM_HealingCircle_I" || powerToLevelName == "TM_HealingCircle_II" || powerToLevelName == "TM_HealingCircle_III")
			{
				compMagic.LevelUpSkill_HealingCircle(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_BestowMight" || powerToLevelName == "TM_BestowMight_I" || powerToLevelName == "TM_BestowMight_II" || powerToLevelName == "TM_BestowMight_III")
			{
				compMagic.LevelUpSkill_BestowMight(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}
			if (powerToLevelName == "TM_Resurrection")
			{
				compMagic.LevelUpSkill_Resurrection(skill.label);
				skill.level++;
				compMagic.MagicData.MagicAbilityPoints -= 1;
			}

			return new DoSKillLevelUpReturn { caum = compMagic, mps = skill };
		}
	}
}