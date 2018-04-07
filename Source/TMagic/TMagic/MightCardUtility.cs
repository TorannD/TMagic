using Harmony;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace TorannMagic
{
    class MightCardUtility //original code by Jecrell
    {

        public static Vector2 mightCardSize = new Vector2(700f, 556f);

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

        public static void DrawMightCard(Rect rect, Pawn pawn)
        {
            GUI.BeginGroup(rect);
            CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();
            bool flag = comp != null;
            if (flag)
            {
                bool flag2 = true;
                if (flag2)
                {
                    float x = Text.CalcSize("TM_HeaderMight".Translate()).x;
                    Rect rect2 = new Rect(rect.width / 2f - (x/2) , rect.y, rect.width, MightCardUtility.HeaderSize); //+ MightCardUtility.SpacingOffset
                    Text.Font = GameFont.Small;
                    Widgets.Label(rect2, "TM_HeaderMight".Translate().CapitalizeFirst());
                    Text.Font = GameFont.Small;
                    Widgets.DrawLineHorizontal(rect.x - 10f, rect2.yMax, rect.width - 15f);
                    Rect rect9 = new Rect(rect.x, rect2.yMax + MightCardUtility.Padding, rect2.width, MightCardUtility.SkillsColumnHeight);
                    Rect inRect = new Rect(rect9.x, rect9.y + MightCardUtility.Padding, MightCardUtility.SkillsColumnDivider, MightCardUtility.SkillsColumnHeight);
                    Rect inRect2 = new Rect(rect9.x + MightCardUtility.SkillsColumnDivider, rect9.y + MightCardUtility.Padding, rect9.width - MightCardUtility.SkillsColumnDivider, MightCardUtility.SkillsColumnHeight);
                    MightCardUtility.InfoPane(inRect, pawn.GetComp<CompAbilityUserMight>(), pawn);
                    float x5 = Text.CalcSize("TM_Skills".Translate()).x;
                    Rect rect10 = new Rect(rect.width / 2f - x5 / 2f, rect9.yMax - 60f, rect.width, MightCardUtility.HeaderSize);
                    Text.Font = GameFont.Small;
                    Widgets.Label(rect10, "TM_Skills".Translate().CapitalizeFirst());
                    Text.Font = GameFont.Small;
                    Widgets.DrawLineHorizontal(rect.x - 10f, rect10.yMax, rect.width - 15f);
                    Rect rect11 = new Rect(rect.x, rect10.yMax + MightCardUtility.SectionOffset, rect10.width, MightCardUtility.PowersColumnHeight);
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator))
                    {
                        Rect inRect3 = new Rect(rect.x, rect11.y, MightCardUtility.PowersColumnWidth, MightCardUtility.PowersColumnHeight);
                        MightCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMight>(), pawn.GetComp<CompAbilityUserMight>().MightData.MightPowersG, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Sprint, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Fortitude, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Grapple, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Cleave, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Whirlwind, TexButton.TMTex_SkillPointUsed);
                    }
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper))
                    {
                        
                        Rect inRect3 = new Rect(rect.x, rect11.y, MightCardUtility.PowersColumnWidth, MightCardUtility.PowersColumnHeight);
                        MightCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMight>(), pawn.GetComp<CompAbilityUserMight>().MightData.MightPowersS, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_SniperFocus, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Headshot, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_DisablingShot, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_AntiArmor, null, TexButton.TMTex_SkillPointUsed);
                    }
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer))
                    {

                        Rect inRect3 = new Rect(rect.x, rect11.y, MightCardUtility.PowersColumnWidth, MightCardUtility.PowersColumnHeight);
                        MightCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMight>(), pawn.GetComp<CompAbilityUserMight>().MightData.MightPowersB, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_BladeFocus, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_BladeArt, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_SeismicSlash, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_BladeSpin, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_PhaseStrike, TexButton.TMTex_SkillPointUsed);
                    }
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger))
                    {

                        Rect inRect3 = new Rect(rect.x, rect11.y, MightCardUtility.PowersColumnWidth, MightCardUtility.PowersColumnHeight);
                        MightCardUtility.PowersGUIHandler(inRect3, pawn.GetComp<CompAbilityUserMight>(), pawn.GetComp<CompAbilityUserMight>().MightData.MightPowersR, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_RangerTraining, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_BowTraining, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_PoisonTrap, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_AnimalFriend, pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_ArrowStorm, TexButton.TMTex_SkillPointUsed);
                    }
                }
            }
            GUI.EndGroup();
        }

        public static void InfoPane(Rect inRect, CompAbilityUserMight compMight, Pawn pawn)
        {
            Rect rect = new Rect(inRect.x, inRect.y, inRect.width * 0.7f, MightCardUtility.TextSize);
            Text.Font = GameFont.Tiny;
            Widgets.Label(rect, "TM_Level".Translate().CapitalizeFirst() + ": " + compMight.MightUserLevel.ToString());
            Text.Font = GameFont.Tiny;
            bool godMode = DebugSettings.godMode;
            if (godMode)
            {
                Rect rect2 = new Rect(rect.xMax, inRect.y, inRect.width * 0.3f, MightCardUtility.TextSize);
                bool flag = Widgets.ButtonText(rect2, "+", true, false, true);
                if (flag)
                {
                    compMight.LevelUp(true);
                }
                Rect rect22 = new Rect(rect.xMax + 60f, inRect.y, 50f, MightCardUtility.TextSize * 2);
                bool flag22 = Widgets.ButtonText(rect22, "Clear Powers", true, false, true);
                if (flag22)
                {
                    compMight.ClearPowers();
                }
                Rect rect23 = new Rect(rect.xMax + 115f, inRect.y, 50f, MightCardUtility.TextSize * 2);
                bool flag23 = Widgets.ButtonText(rect23, "Clear Trait", true, false, true);
                if (flag23)
                {
                    compMight.ClearTraits();
                }
            }
            Rect rect4 = new Rect(inRect.x, rect.yMax, inRect.width, MightCardUtility.TextSize);
            Text.Font = GameFont.Tiny;
            Widgets.Label(rect4, "TM_PointsAvail".Translate() + ": " + compMight.MightData.MightAbilityPoints);
            Text.Font = GameFont.Tiny;
            Rect rect5 = new Rect(rect4.x, rect4.yMax + 3f, inRect.width + 100f, MightCardUtility.HeaderSize * 0.6f);
            MightCardUtility.DrawLevelBar(rect5, compMight, pawn, inRect);
        }

        public static void DrawLevelBar(Rect rect, CompAbilityUserMight compMight, Pawn pawn, Rect rectG)
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
            TooltipHandler.TipRegion(rect, new TipSignal(() => MightCardUtility.MightXPTipString(compMight), rect.GetHashCode()));
            float num2 = 14f;
            bool flag3 = rect.height < 50f;
            if (flag3)
            {
                num2 *= Mathf.InverseLerp(0f, 50f, rect.height);
            }
            Text.Anchor = TextAnchor.UpperLeft;
            Rect rect2 = new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height);
            rect2 = new Rect(rect2.x, rect2.y, rect2.width, rect2.height - num2);
            Widgets.FillableBar(rect2, compMight.XPTillNextLevelPercent, (Texture2D)AccessTools.Field(typeof(Widgets), "BarFullTexHor").GetValue(null), BaseContent.GreyTex, false);
            //Rect rect3 = new Rect(rect2.x + (rectG.x/4)+ MightCardUtility.MagicButtonPointSize, rect2.yMin + 24f, 136f, MightCardUtility.TextSize);
            // Rect rect31 = new Rect(rect2.x + (rectG.x / 4), rect2.yMin + 24f, MightCardUtility.MagicButtonPointSize, MightCardUtility.TextSize);
            Rect rect3 = new Rect(rect2.x + 272f + MightCardUtility.MagicButtonPointSize, rectG.y, 136f, MightCardUtility.TextSize);
            Rect rect31 = new Rect(rect2.x + 272f, rectG.y, MightCardUtility.MagicButtonPointSize, MightCardUtility.TextSize);
            Rect rect4 = new Rect(rect3.x + rect3.width + (MightCardUtility.MagicButtonPointSize * 2), rectG.y, 136f, MightCardUtility.TextSize); //rect2.yMin + 24f
            Rect rect41 = new Rect(rect3.x + rect3.width + MightCardUtility.MagicButtonPointSize, rectG.y, MightCardUtility.MagicButtonPointSize, MightCardUtility.TextSize);
            Rect rect5 = new Rect(rect2.x + 272f + MightCardUtility.MagicButtonPointSize, rectG.yMin + 24f, 136f, MightCardUtility.TextSize);
            Rect rect51 = new Rect(rect2.x + 272f, rectG.yMin + 24f, MightCardUtility.MagicButtonPointSize, MightCardUtility.TextSize);
            Rect rect6 = new Rect(rect5.x + rect5.width + (MightCardUtility.MagicButtonPointSize * 2), rectG.y + 24f, 136f, MightCardUtility.TextSize); //rect2.yMin + 24f
            Rect rect61 = new Rect(rect5.x + rect5.width + MightCardUtility.MagicButtonPointSize, rectG.y + 24f, MightCardUtility.MagicButtonPointSize, MightCardUtility.TextSize);


            List<MightPowerSkill> skill1 = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_global_refresh;
            List<MightPowerSkill> skill2 = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_global_seff;
            List<MightPowerSkill> skill3 = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_global_strength;
            List<MightPowerSkill> skill4 = pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_global_endurance;

            using (List<MightPowerSkill>.Enumerator enumerator1 = skill1.GetEnumerator())
            {
                while (enumerator1.MoveNext())
                {
                    MightPowerSkill skill = enumerator1.Current;
                    TooltipHandler.TipRegion(rect3, new TipSignal(() => enumerator1.Current.desc.Translate(), rect3.GetHashCode()));
                    bool flag11 = skill.level >= skill.levelMax || compMight.MightData.MightAbilityPoints == 0;
                    if (flag11)
                    {
                        Widgets.Label(rect3, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                    }
                    else
                    {
                        bool flag12 = Widgets.ButtonText(rect31, "+", true, false, true) && compMight.AbilityUser.Faction == Faction.OfPlayer;
                        Widgets.Label(rect3, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                        if (flag12)
                        {
                            if (skill.label == "TM_global_refresh_pwr")
                            {
                                compMight.LevelUpSkill_global_refresh(skill.label);
                                skill.level++;
                                compMight.MightData.MightAbilityPoints -= 1;
                            }
                        }
                    }
                }
            }
            using (List<MightPowerSkill>.Enumerator enumerator2 = skill2.GetEnumerator())
            {
                while (enumerator2.MoveNext())
                {
                    MightPowerSkill skill = enumerator2.Current;
                    TooltipHandler.TipRegion(rect4, new TipSignal(() => enumerator2.Current.desc.Translate(), rect4.GetHashCode()));
                    bool flag11 = skill.level >= skill.levelMax || compMight.MightData.MightAbilityPoints == 0;
                    if (flag11)
                    {
                        Widgets.Label(rect4, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                    }
                    else
                    {
                        bool flag12 = Widgets.ButtonText(rect41, "+", true, false, true) && compMight.AbilityUser.Faction == Faction.OfPlayer;
                        Widgets.Label(rect4, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                        if (flag12)
                        {
                            if (skill.label == "TM_global_seff_pwr")
                            {
                                compMight.LevelUpSkill_global_seff(skill.label);
                                skill.level++;
                                compMight.MightData.MightAbilityPoints -= 1;
                            }
                        }
                    }
                }
            }
            using (List<MightPowerSkill>.Enumerator enumerator3 = skill3.GetEnumerator())
            {
                while (enumerator3.MoveNext())
                {
                    MightPowerSkill skill = enumerator3.Current;
                    TooltipHandler.TipRegion(rect5, new TipSignal(() => enumerator3.Current.desc.Translate(), rect5.GetHashCode()));
                    bool flag11 = skill.level >= skill.levelMax || compMight.MightData.MightAbilityPoints == 0;
                    if (flag11)
                    {
                        Widgets.Label(rect5, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                    }
                    else
                    {
                        bool flag12 = Widgets.ButtonText(rect51, "+", true, false, true) && compMight.AbilityUser.Faction == Faction.OfPlayer;
                        Widgets.Label(rect5, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                        if (flag12)
                        {
                            if (skill.label == "TM_global_strength_pwr")
                            {
                                compMight.LevelUpSkill_global_strength(skill.label);
                                skill.level++;
                                compMight.MightData.MightAbilityPoints -= 1;
                            }
                        }
                    }
                }
            }
            using (List<MightPowerSkill>.Enumerator enumerator4 = skill4.GetEnumerator())
            {
                while (enumerator4.MoveNext())
                {
                    MightPowerSkill skill = enumerator4.Current;
                    TooltipHandler.TipRegion(rect6, new TipSignal(() => enumerator4.Current.desc.Translate(), rect6.GetHashCode()));
                    bool flag11 = skill.level >= skill.levelMax || compMight.MightData.MightAbilityPoints == 0;
                    if (flag11)
                    {
                        Widgets.Label(rect6, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                    }
                    else
                    {
                        bool flag12 = Widgets.ButtonText(rect61, "+", true, false, true) && compMight.AbilityUser.Faction == Faction.OfPlayer;
                        Widgets.Label(rect6, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                        if (flag12)
                        {
                            if (skill.label == "TM_global_endurance_pwr")
                            {
                                compMight.LevelUpSkill_global_endurance(skill.label);
                                skill.level++;
                                compMight.MightData.MightAbilityPoints -= 1;
                            }
                        }
                    }
                }
            }
        }


        public static string MightXPTipString(CompAbilityUserMight compMight)
        {
            string result;

            result = string.Concat(new string[]
            {
                compMight.MightUserXP.ToString(),
                " / ",
                compMight.MightUserXPTillNextLevel.ToString(),
                "\n",
                "TM_MightXPDesc".Translate()
            });

            return result;
        }

        public static void PowersGUIHandler(Rect inRect, CompAbilityUserMight compMight, List<MightPower> MightPowers, List<MightPowerSkill> MightPowerSkill1, List<MightPowerSkill> MightPowerSkill2, List<MightPowerSkill> MightPowerSkill3, List<MightPowerSkill> MightPowerSkill4, List<MightPowerSkill> MightPowerSkill5, Texture2D pointTexture)
        {
            float num = inRect.y;
            int itnum = 1;
            bool flag999;
            using (List<MightPower>.Enumerator enumerator = MightPowers.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    MightPower power = enumerator.Current;
                    Text.Font = GameFont.Small;
                    Rect rect = new Rect(MightCardUtility.mightCardSize.x / 2f - MightCardUtility.MagicButtonSize, num, MightCardUtility.MagicButtonSize, MightCardUtility.MagicButtonSize);
                    if (itnum > 1)
                    {
                        Widgets.DrawLineHorizontal(0f + 20f, rect.y - 2f, 700f - 40f);
                    }
                    //power.abilityDef == TorannMagicDefOf.TM_Sprint || power.abilityDef == TorannMagicDefOf.TM_Sprint_I || power.abilityDef == TorannMagicDefOf.TM_Sprint_II ||
                    if (power.level < 3 && (power.abilityDef == TorannMagicDefOf.TM_Grapple || power.abilityDef == TorannMagicDefOf.TM_Grapple_I || power.abilityDef == TorannMagicDefOf.TM_Grapple_II || 
                        power.abilityDef == TorannMagicDefOf.TM_DisablingShot || power.abilityDef == TorannMagicDefOf.TM_DisablingShot_I || power.abilityDef == TorannMagicDefOf.TM_DisablingShot_II ||
                        power.abilityDef == TorannMagicDefOf.TM_PhaseStrike || power.abilityDef == TorannMagicDefOf.TM_PhaseStrike_I || power.abilityDef == TorannMagicDefOf.TM_PhaseStrike_II ||
                        power.abilityDef == TorannMagicDefOf.TM_ArrowStorm || power.abilityDef == TorannMagicDefOf.TM_ArrowStorm_I || power.abilityDef == TorannMagicDefOf.TM_ArrowStorm_II))
                    {
                        TooltipHandler.TipRegion(rect, () => string.Concat(new string[]
                        {
                            power.abilityDef.label,
                            "\n\nCurrent Level:\n",
                            power.abilityDescDef.description,
                            "\n\nNext Level:\n",
                            power.nextLevelAbilityDescDef.description,
                            "\n\n",
                            MightAbility.PostAbilityDesc((TMAbilityDef)power.abilityDef, compMight),
                            "\n",
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
                            MightAbility.PostAbilityDesc((TMAbilityDef)power.abilityDef, compMight),
                            "\n",
                            "TM_CheckPointsForMoreInfo".Translate()
                            }), 398462);
                    }

                    float x2 = Text.CalcSize("TM_Effeciency".Translate()).x;
                    float x3 = Text.CalcSize("TM_Versatility".Translate()).x;
                    Rect rect3 = new Rect(0f + MightCardUtility.SpacingOffset, rect.y + 2f, MightCardUtility.mightCardSize.x, MightCardUtility.ButtonSize * 1.15f);

                    Rect rect5 = new Rect(rect3.x + rect3.width / 2f - x2, rect3.y, (rect3.width - 20f) / 3f, rect3.height);
                    Rect rect6 = new Rect(rect3.width - x3 * 2f, rect3.y, rect3.width / 3f, rect3.height);

                    float x4 = Text.CalcSize(" # / # ").x;
                    //bool flag9 = power.abilityDef.label == "Sprint" || power.abilityDef.label == "Grapple"; //add all other buffs or xml based upgrades
                    //power.abilityDef.defName == "TM_Sprint" || power.abilityDef.defName ==  "TM_Sprint_I" || power.abilityDef.defName == "TM_Sprint_II" || power.abilityDef.defName == "TM_Sprint_III" ||
                    if (power.abilityDef.defName == "TM_Grapple" || power.abilityDef.defName == "TM_Grapple_I" || power.abilityDef.defName == "TM_Grapple_II" || power.abilityDef.defName == "TM_Grapple_III" ||
                        power.abilityDef.defName == "TM_DisablingShot" || power.abilityDef.defName == "TM_DisablingShot_I" || power.abilityDef.defName == "TM_DisablingShot_II" || power.abilityDef.defName == "TM_DisablingShot_III" ||
                        power.abilityDef.defName == "TM_PhaseStrike" || power.abilityDef.defName == "TM_PhaseStrike_I" || power.abilityDef.defName == "TM_PhaseStrike_II" || power.abilityDef.defName == "TM_PhaseStrike_III" ||
                        power.abilityDef.defName == "TM_ArrowStorm" || power.abilityDef.defName == "TM_ArrowStorm_I" || power.abilityDef.defName == "TM_ArrowStorm_II" || power.abilityDef.defName == "TM_ArrowStorm_III")
                    {
                        flag999 = true;
                    }
                    else
                    {
                        flag999 = false;
                    }
                    bool flag10 = enumerator.Current.level >= 3 || compMight.MightData.MightAbilityPoints == 0; 
                    if (flag10)
                    {                        
                        if (flag999)
                        {
                            Widgets.DrawTextureFitted(rect, power.Icon, 1f);
                            Rect rect19 = new Rect(rect.xMax, rect.yMin, x4, MightCardUtility.TextSize);
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
                            Rect rect10 = new Rect(rect.xMax, rect.yMin, x4, MightCardUtility.TextSize);
                            bool flag1 = Widgets.ButtonImage(rect, power.Icon) && compMight.AbilityUser.Faction == Faction.OfPlayer;
                            Widgets.Label(rect10, " " + power.level + " / 3");
                            if (flag1)
                            {
                                compMight.LevelUpPower(power);
                                compMight.MightData.MightAbilityPoints -= 1;
                                compMight.FixPowers();
                            }
                        }
                        else
                        {
                            Widgets.DrawTextureFitted(rect, power.Icon, 1f);
                        }
                    }
                    Text.Font = GameFont.Tiny;
                    float num2 = rect3.x;
                    if (itnum == 1 && MightPowerSkill1 != null)
                    {
                        using (List<MightPowerSkill>.Enumerator enumerator1 = MightPowerSkill1.GetEnumerator())
                        {                            
                            while (enumerator1.MoveNext())
                            {
                                Rect rect4 = new Rect(num2 + MightCardUtility.MagicButtonPointSize, rect3.yMax, MightCardUtility.mightCardSize.x / 3f, rect3.height); 
                                Rect rect41 = new Rect(num2, rect4.y, MightCardUtility.MagicButtonPointSize, MightCardUtility.MagicButtonPointSize);
                                Rect rect42 = new Rect(rect41.x, rect4.y, rect4.width - MagicCardUtility.MagicButtonPointSize, rect4.height/2);
                                MightPowerSkill skill = enumerator1.Current;
                                TooltipHandler.TipRegion(rect42, new TipSignal(() => skill.desc.Translate(), rect4.GetHashCode()));
                                bool flag11 = (skill.level >= skill.levelMax || compMight.MightData.MightAbilityPoints == 0) || ((enumerator.Current.abilityDef.defName == "TM_BladeFocus" || enumerator.Current.abilityDef.defName == "TM_SniperFocus" || enumerator.Current.abilityDef.defName == "TM_BladeArt" || enumerator.Current.abilityDef.defName == "TM_RangerTraining" || enumerator.Current.abilityDef.defName == "TM_BowTraining") && compMight.MightData.MightAbilityPoints < 2);
                                if (flag11)
                                {
                                    Widgets.Label(rect4, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                                }
                                else
                                {
                                    bool flag12 = Widgets.ButtonText(rect41, "+", true, false, true) && compMight.AbilityUser.Faction == Faction.OfPlayer;
                                    Widgets.Label(rect4, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                                    if (flag12)
                                    {
                                        bool flag17 = compMight.AbilityUser.story != null && compMight.AbilityUser.story.WorkTagIsDisabled(WorkTags.Violent) && power.abilityDef.MainVerb.isViolent;
                                        if (flag17)
                                        {
                                            Messages.Message("IsIncapableOfViolenceLower".Translate(new object[]
                                            {
                                            compMight.parent.LabelShort
                                            }), MessageTypeDefOf.RejectInput);
                                            break;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Sprint" || enumerator.Current.abilityDef.defName == "TM_Sprint_I" || enumerator.Current.abilityDef.defName == "TM_Sprint_II" || enumerator.Current.abilityDef.defName == "TM_Sprint_III")
                                        {
                                            compMight.LevelUpSkill_Sprint(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Fortitude")
                                        {
                                            compMight.LevelUpSkill_Fortitude(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Grapple" || enumerator.Current.abilityDef.defName == "TM_Grapple_I" || enumerator.Current.abilityDef.defName == "TM_Grapple_II" || enumerator.Current.abilityDef.defName == "TM_Grapple_III")
                                        {
                                            compMight.LevelUpSkill_Grapple(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Cleave")
                                        {
                                            compMight.LevelUpSkill_Cleave(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Whirlwind")
                                        {
                                            compMight.LevelUpSkill_Whirlwind(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_SniperFocus" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_SniperFocus(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Headshot")
                                        {
                                            compMight.LevelUpSkill_Headshot(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_DisablingShot" || enumerator.Current.abilityDef.defName == "TM_DisablingShot_I" || enumerator.Current.abilityDef.defName == "TM_DisablingShot_II")
                                        {
                                            compMight.LevelUpSkill_DisablingShot(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_AntiArmor")
                                        {
                                            compMight.LevelUpSkill_AntiArmor(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BladeFocus" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_BladeFocus(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BladeArt" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_BladeArt(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_SeismicSlash")
                                        {
                                            compMight.LevelUpSkill_SeismicSlash(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BladeSpin")
                                        {
                                            compMight.LevelUpSkill_BladeSpin(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_PhaseStrike" || enumerator.Current.abilityDef.defName == "TM_PhaseStrike_I" || enumerator.Current.abilityDef.defName == "TM_PhaseStrike_II" || enumerator.Current.abilityDef.defName == "TM_PhaseStrike_III")
                                        {
                                            compMight.LevelUpSkill_PhaseStrike(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_RangerTraining" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_RangerTraining(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BowTraining" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_BowTraining(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_PoisonTrap")
                                        {
                                            compMight.LevelUpSkill_PoisonTrap(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_AnimalFriend")
                                        {
                                            compMight.LevelUpSkill_AnimalFriend(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_ArrowStorm" || enumerator.Current.abilityDef.defName == "TM_ArrowStorm_I" || enumerator.Current.abilityDef.defName == "TM_ArrowStorm_II" || enumerator.Current.abilityDef.defName == "TM_ArrowStorm_III")
                                        {
                                            compMight.LevelUpSkill_ArrowStorm(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                    }
                                }
                                num2 += (MightCardUtility.mightCardSize.x / 3) - MightCardUtility.SpacingOffset;
                            }
                        }
                        itnum++;
                    }
                    else if (itnum == 2 && MightPowerSkill2 != null)
                    {
                        using (List<MightPowerSkill>.Enumerator enumerator2 = MightPowerSkill2.GetEnumerator())
                        {
                            while (enumerator2.MoveNext())
                            {
                                Rect rect4 = new Rect(num2 + MightCardUtility.MagicButtonPointSize, rect3.yMax, MightCardUtility.mightCardSize.x / 3f, rect3.height);
                                Rect rect41 = new Rect(num2, rect4.y, MightCardUtility.MagicButtonPointSize, MightCardUtility.MagicButtonPointSize);
                                Rect rect42 = new Rect(rect41.x, rect4.y, rect4.width - MagicCardUtility.MagicButtonPointSize, rect4.height/2);
                                MightPowerSkill skill = enumerator2.Current;
                                TooltipHandler.TipRegion(rect42, new TipSignal(() => skill.desc.Translate(), rect4.GetHashCode()));
                                bool flag11 = (skill.level >= skill.levelMax || compMight.MightData.MightAbilityPoints == 0) || ((enumerator.Current.abilityDef.defName == "TM_BladeFocus" || enumerator.Current.abilityDef.defName == "TM_SniperFocus" || enumerator.Current.abilityDef.defName == "TM_BladeArt" || enumerator.Current.abilityDef.defName == "TM_RangerTraining" || enumerator.Current.abilityDef.defName == "TM_BowTraining") && compMight.MightData.MightAbilityPoints < 2);
                                if (flag11)
                                {
                                    Widgets.Label(rect4, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                                }
                                else
                                {
                                    bool flag12 = Widgets.ButtonText(rect41, "+", true, false, true) && compMight.AbilityUser.Faction == Faction.OfPlayer;
                                    Widgets.Label(rect4, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                                    if (flag12)
                                    {
                                        bool flag17 = compMight.AbilityUser.story != null && compMight.AbilityUser.story.WorkTagIsDisabled(WorkTags.Violent) && power.abilityDef.MainVerb.isViolent;
                                        if (flag17)
                                        {
                                            Messages.Message("IsIncapableOfViolenceLower".Translate(new object[]
                                            {
                                            compMight.parent.LabelShort
                                            }), MessageTypeDefOf.RejectInput);
                                            break;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Sprint" || enumerator.Current.abilityDef.defName == "TM_Sprint_I" || enumerator.Current.abilityDef.defName == "TM_Sprint_II" || enumerator.Current.abilityDef.defName == "TM_Sprint_III")
                                        {
                                            compMight.LevelUpSkill_Sprint(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Fortitude")
                                        {
                                            compMight.LevelUpSkill_Fortitude(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Grapple" || enumerator.Current.abilityDef.defName == "TM_Grapple_I" || enumerator.Current.abilityDef.defName == "TM_Grapple_II" || enumerator.Current.abilityDef.defName == "TM_Grapple_III")
                                        {
                                            compMight.LevelUpSkill_Grapple(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Cleave")
                                        {
                                            compMight.LevelUpSkill_Cleave(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Whirlwind")
                                        {
                                            compMight.LevelUpSkill_Whirlwind(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_SniperFocus" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_SniperFocus(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Headshot")
                                        {
                                            compMight.LevelUpSkill_Headshot(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_DisablingShot" || enumerator.Current.abilityDef.defName == "TM_DisablingShot_I" || enumerator.Current.abilityDef.defName == "TM_DisablingShot_II")
                                        {
                                            compMight.LevelUpSkill_DisablingShot(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_AntiArmor")
                                        {
                                            compMight.LevelUpSkill_AntiArmor(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BladeFocus" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_BladeFocus(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BladeArt" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_BladeArt(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_SeismicSlash")
                                        {
                                            compMight.LevelUpSkill_SeismicSlash(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BladeSpin")
                                        {
                                            compMight.LevelUpSkill_BladeSpin(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_PhaseStrike" || enumerator.Current.abilityDef.defName == "TM_PhaseStrike_I" || enumerator.Current.abilityDef.defName == "TM_PhaseStrike_II" || enumerator.Current.abilityDef.defName == "TM_PhaseStrike_III")
                                        {
                                            compMight.LevelUpSkill_PhaseStrike(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_RangerTraining" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_RangerTraining(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BowTraining" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_BowTraining(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_PoisonTrap")
                                        {
                                            compMight.LevelUpSkill_PoisonTrap(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_AnimalFriend")
                                        {
                                            compMight.LevelUpSkill_AnimalFriend(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_ArrowStorm" || enumerator.Current.abilityDef.defName == "TM_ArrowStorm_I" || enumerator.Current.abilityDef.defName == "TM_ArrowStorm_II" || enumerator.Current.abilityDef.defName == "TM_ArrowStorm_III")
                                        {
                                            compMight.LevelUpSkill_ArrowStorm(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                    }
                                }
                                num2 += (MightCardUtility.mightCardSize.x / 3) - MightCardUtility.SpacingOffset;
                            }
                        }
                        itnum++;
                    }
                    else if (itnum == 3 && MightPowerSkill3 != null)
                    {
                        using (List<MightPowerSkill>.Enumerator enumerator3 = MightPowerSkill3.GetEnumerator())
                        {
                            while (enumerator3.MoveNext())
                            {
                                Rect rect4 = new Rect(num2 + MightCardUtility.MagicButtonPointSize, rect3.yMax, MightCardUtility.mightCardSize.x / 3f, rect3.height);
                                Rect rect41 = new Rect(num2, rect4.y, MightCardUtility.MagicButtonPointSize, MightCardUtility.MagicButtonPointSize);
                                Rect rect42 = new Rect(rect41.x, rect4.y, rect4.width - MagicCardUtility.MagicButtonPointSize, rect4.height/2);
                                MightPowerSkill skill = enumerator3.Current;
                                TooltipHandler.TipRegion(rect42, new TipSignal(() => skill.desc.Translate(), rect4.GetHashCode()));
                                bool flag11 = (skill.level >= skill.levelMax || compMight.MightData.MightAbilityPoints == 0) || ((enumerator.Current.abilityDef.defName == "TM_BladeFocus" || enumerator.Current.abilityDef.defName == "TM_SniperFocus" || enumerator.Current.abilityDef.defName == "TM_BladeArt" || enumerator.Current.abilityDef.defName == "TM_RangerTraining" || enumerator.Current.abilityDef.defName == "TM_BowTraining") && compMight.MightData.MightAbilityPoints < 2);
                                if (flag11)
                                {
                                    Widgets.Label(rect4, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                                }
                                else
                                {
                                    bool flag12 = Widgets.ButtonText(rect41, "+", true, false, true) && compMight.AbilityUser.Faction == Faction.OfPlayer;
                                    Widgets.Label(rect4, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                                    if (flag12)
                                    {
                                        bool flag17 = compMight.AbilityUser.story != null && compMight.AbilityUser.story.WorkTagIsDisabled(WorkTags.Violent) && power.abilityDef.MainVerb.isViolent;
                                        if (flag17)
                                        {
                                            Messages.Message("IsIncapableOfViolenceLower".Translate(new object[]
                                            {
                                            compMight.parent.LabelShort
                                            }), MessageTypeDefOf.RejectInput);
                                            break;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Sprint" || enumerator.Current.abilityDef.defName == "TM_Sprint_I" || enumerator.Current.abilityDef.defName == "TM_Sprint_II" || enumerator.Current.abilityDef.defName == "TM_Sprint_III")
                                        {
                                            compMight.LevelUpSkill_Sprint(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Fortitude")
                                        {
                                            compMight.LevelUpSkill_Fortitude(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Grapple" || enumerator.Current.abilityDef.defName == "TM_Grapple_I" || enumerator.Current.abilityDef.defName == "TM_Grapple_II" || enumerator.Current.abilityDef.defName == "TM_Grapple_III")
                                        {
                                            compMight.LevelUpSkill_Grapple(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Cleave")
                                        {
                                            compMight.LevelUpSkill_Cleave(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Whirlwind")
                                        {
                                            compMight.LevelUpSkill_Whirlwind(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_SniperFocus" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_SniperFocus(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Headshot")
                                        {
                                            compMight.LevelUpSkill_Headshot(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_DisablingShot" || enumerator.Current.abilityDef.defName == "TM_DisablingShot_I" || enumerator.Current.abilityDef.defName == "TM_DisablingShot_II")
                                        {
                                            compMight.LevelUpSkill_DisablingShot(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_AntiArmor")
                                        {
                                            compMight.LevelUpSkill_AntiArmor(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BladeFocus" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_BladeFocus(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BladeArt" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_BladeArt(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_SeismicSlash")
                                        {
                                            compMight.LevelUpSkill_SeismicSlash(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BladeSpin")
                                        {
                                            compMight.LevelUpSkill_BladeSpin(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_PhaseStrike" || enumerator.Current.abilityDef.defName == "TM_PhaseStrike_I" || enumerator.Current.abilityDef.defName == "TM_PhaseStrike_II" || enumerator.Current.abilityDef.defName == "TM_PhaseStrike_III")
                                        {
                                            compMight.LevelUpSkill_PhaseStrike(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_RangerTraining" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_RangerTraining(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BowTraining" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_BowTraining(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_PoisonTrap")
                                        {
                                            compMight.LevelUpSkill_PoisonTrap(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_AnimalFriend")
                                        {
                                            compMight.LevelUpSkill_AnimalFriend(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_ArrowStorm" || enumerator.Current.abilityDef.defName == "TM_ArrowStorm_I" || enumerator.Current.abilityDef.defName == "TM_ArrowStorm_II" || enumerator.Current.abilityDef.defName == "TM_ArrowStorm_III")
                                        {
                                            compMight.LevelUpSkill_ArrowStorm(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                    }
                                }
                                num2 += (MightCardUtility.mightCardSize.x / 3) - MightCardUtility.SpacingOffset;
                            }
                        }
                        itnum++;
                    }
                    else if (itnum == 4 && MightPowerSkill4 != null)
                    {
                        using (List<MightPowerSkill>.Enumerator enumerator4 = MightPowerSkill4.GetEnumerator())
                        {
                            while (enumerator4.MoveNext())
                            {
                                Rect rect4 = new Rect(num2 + MightCardUtility.MagicButtonPointSize, rect3.yMax, MightCardUtility.mightCardSize.x / 3f, rect3.height);
                                Rect rect41 = new Rect(num2, rect4.y, MightCardUtility.MagicButtonPointSize, MightCardUtility.MagicButtonPointSize);
                                Rect rect42 = new Rect(rect41.x, rect4.y, rect4.width - MagicCardUtility.MagicButtonPointSize, rect4.height/2);
                                MightPowerSkill skill = enumerator4.Current;
                                TooltipHandler.TipRegion(rect42, new TipSignal(() => skill.desc.Translate(), rect4.GetHashCode()));
                                bool flag11 = (skill.level >= skill.levelMax || compMight.MightData.MightAbilityPoints == 0) || ((enumerator.Current.abilityDef.defName == "TM_BladeFocus" || enumerator.Current.abilityDef.defName == "TM_SniperFocus" || enumerator.Current.abilityDef.defName == "TM_BladeArt" || enumerator.Current.abilityDef.defName == "TM_RangerTraining" || enumerator.Current.abilityDef.defName == "TM_BowTraining") && compMight.MightData.MightAbilityPoints < 2);
                                if (flag11)
                                {
                                    Widgets.Label(rect4, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                                }
                                else
                                {
                                    bool flag12 = Widgets.ButtonText(rect41, "+", true, false, true) && compMight.AbilityUser.Faction == Faction.OfPlayer;
                                    Widgets.Label(rect4, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                                    if (flag12)
                                    {
                                        bool flag17 = compMight.AbilityUser.story != null && compMight.AbilityUser.story.WorkTagIsDisabled(WorkTags.Violent) && power.abilityDef.MainVerb.isViolent;
                                        if (flag17)
                                        {
                                            Messages.Message("IsIncapableOfViolenceLower".Translate(new object[]
                                            {
                                            compMight.parent.LabelShort
                                            }), MessageTypeDefOf.RejectInput);
                                            break;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Sprint" || enumerator.Current.abilityDef.defName == "TM_Sprint_I" || enumerator.Current.abilityDef.defName == "TM_Sprint_II" || enumerator.Current.abilityDef.defName == "TM_Sprint_III")
                                        {
                                            compMight.LevelUpSkill_Sprint(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Fortitude")
                                        {
                                            compMight.LevelUpSkill_Fortitude(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Grapple" || enumerator.Current.abilityDef.defName == "TM_Grapple_I" || enumerator.Current.abilityDef.defName == "TM_Grapple_II" || enumerator.Current.abilityDef.defName == "TM_Grapple_III")
                                        {
                                            compMight.LevelUpSkill_Grapple(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Cleave")
                                        {
                                            compMight.LevelUpSkill_Cleave(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Whirlwind")
                                        {
                                            compMight.LevelUpSkill_Whirlwind(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_SniperFocus" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_SniperFocus(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Headshot")
                                        {
                                            compMight.LevelUpSkill_Headshot(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_DisablingShot" || enumerator.Current.abilityDef.defName == "TM_DisablingShot_I" || enumerator.Current.abilityDef.defName == "TM_DisablingShot_II" )
                                        {
                                            compMight.LevelUpSkill_DisablingShot(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_AntiArmor")
                                        {
                                            compMight.LevelUpSkill_AntiArmor(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BladeFocus" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_BladeFocus(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BladeArt" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_BladeArt(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_SeismicSlash")
                                        {
                                            compMight.LevelUpSkill_SeismicSlash(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BladeSpin")
                                        {
                                            compMight.LevelUpSkill_BladeSpin(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_PhaseStrike" || enumerator.Current.abilityDef.defName == "TM_PhaseStrike_I" || enumerator.Current.abilityDef.defName == "TM_PhaseStrike_II" || enumerator.Current.abilityDef.defName == "TM_PhaseStrike_III")
                                        {
                                            compMight.LevelUpSkill_PhaseStrike(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_RangerTraining" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_RangerTraining(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BowTraining" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_BowTraining(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_PoisonTrap")
                                        {
                                            compMight.LevelUpSkill_PoisonTrap(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_AnimalFriend")
                                        {
                                            compMight.LevelUpSkill_AnimalFriend(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_ArrowStorm" || enumerator.Current.abilityDef.defName == "TM_ArrowStorm_I" || enumerator.Current.abilityDef.defName == "TM_ArrowStorm_II" || enumerator.Current.abilityDef.defName == "TM_ArrowStorm_III")
                                        {
                                            compMight.LevelUpSkill_ArrowStorm(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                    }
                                }
                                num2 += (MightCardUtility.mightCardSize.x / 3) - MightCardUtility.SpacingOffset;
                            }
                        }
                        itnum++;
                    }
                    else if (itnum == 5 && MightPowerSkill5 != null)
                    {
                        using (List<MightPowerSkill>.Enumerator enumerator5 = MightPowerSkill5.GetEnumerator())
                        {
                            while (enumerator5.MoveNext())
                            {
                                Rect rect4 = new Rect(num2 + MightCardUtility.MagicButtonPointSize, rect3.yMax, MightCardUtility.mightCardSize.x / 3f, rect3.height); //was rect3.width / 5f
                                Rect rect41 = new Rect(num2, rect4.y, MightCardUtility.MagicButtonPointSize, MightCardUtility.MagicButtonPointSize);
                                Rect rect42 = new Rect(rect41.x, rect4.y, rect4.width - MagicCardUtility.MagicButtonPointSize, rect4.height/2);
                                MightPowerSkill skill = enumerator5.Current;
                                TooltipHandler.TipRegion(rect42, new TipSignal(() => skill.desc.Translate(), rect4.GetHashCode()));
                                bool flag11 = (skill.level >= skill.levelMax || compMight.MightData.MightAbilityPoints == 0) || ((enumerator.Current.abilityDef.defName == "TM_BladeFocus" || enumerator.Current.abilityDef.defName == "TM_SniperFocus" || enumerator.Current.abilityDef.defName == "TM_BladeArt" || enumerator.Current.abilityDef.defName == "TM_RangerTraining" || enumerator.Current.abilityDef.defName == "TM_BowTraining") && compMight.MightData.MightAbilityPoints < 2);
                                if (flag11)
                                {
                                    Widgets.Label(rect4, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                                }
                                else
                                {
                                    bool flag12 = Widgets.ButtonText(rect41, "+", true, false, true) && compMight.AbilityUser.Faction == Faction.OfPlayer;
                                    Widgets.Label(rect4, skill.label.Translate() + ": " + skill.level + " / " + skill.levelMax);
                                    if (flag12)
                                    {
                                        bool flag17 = compMight.AbilityUser.story != null && compMight.AbilityUser.story.WorkTagIsDisabled(WorkTags.Violent) && power.abilityDef.MainVerb.isViolent;
                                        if (flag17)
                                        {
                                            Messages.Message("IsIncapableOfViolenceLower".Translate(new object[]
                                            {
                                            compMight.parent.LabelShort
                                            }), MessageTypeDefOf.RejectInput);
                                            break;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Sprint" || enumerator.Current.abilityDef.defName == "TM_Sprint_I" || enumerator.Current.abilityDef.defName == "TM_Sprint_II" || enumerator.Current.abilityDef.defName == "TM_Sprint_III")
                                        {
                                            compMight.LevelUpSkill_Sprint(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Fortitude")
                                        {
                                            compMight.LevelUpSkill_Fortitude(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Grapple" || enumerator.Current.abilityDef.defName == "TM_Grapple_I" || enumerator.Current.abilityDef.defName == "TM_Grapple_II" || enumerator.Current.abilityDef.defName == "TM_Grapple_III")
                                        {
                                            compMight.LevelUpSkill_Grapple(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Cleave")
                                        {
                                            compMight.LevelUpSkill_Cleave(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Whirlwind")
                                        {
                                            compMight.LevelUpSkill_Whirlwind(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_SniperFocus" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_SniperFocus(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_Headshot")
                                        {
                                            compMight.LevelUpSkill_Headshot(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_DisablingShot" || enumerator.Current.abilityDef.defName == "TM_DisablingShot_I" || enumerator.Current.abilityDef.defName == "TM_DisablingShot_II" )
                                        {
                                            compMight.LevelUpSkill_DisablingShot(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_AntiArmor")
                                        {
                                            compMight.LevelUpSkill_AntiArmor(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BladeFocus" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_BladeFocus(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BladeArt" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_BladeArt(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_SeismicSlash")
                                        {
                                            compMight.LevelUpSkill_SeismicSlash(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BladeSpin")
                                        {
                                            compMight.LevelUpSkill_BladeSpin(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_PhaseStrike" || enumerator.Current.abilityDef.defName == "TM_PhaseStrike_I" || enumerator.Current.abilityDef.defName == "TM_PhaseStrike_II" || enumerator.Current.abilityDef.defName == "TM_PhaseStrike_III")
                                        {
                                            compMight.LevelUpSkill_PhaseStrike(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_RangerTraining" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_RangerTraining(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_BowTraining" && compMight.MightData.MightAbilityPoints >= 2)
                                        {
                                            compMight.LevelUpSkill_BowTraining(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 2;
                                            compMight.ResolveClassSkills();
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_PoisonTrap")
                                        {
                                            compMight.LevelUpSkill_PoisonTrap(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_AnimalFriend")
                                        {
                                            compMight.LevelUpSkill_AnimalFriend(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                        if (enumerator.Current.abilityDef.defName == "TM_ArrowStorm" || enumerator.Current.abilityDef.defName == "TM_ArrowStorm_I" || enumerator.Current.abilityDef.defName == "TM_ArrowStorm_II" || enumerator.Current.abilityDef.defName == "TM_ArrowStorm_III")
                                        {
                                            compMight.LevelUpSkill_ArrowStorm(skill.label);
                                            skill.level++;
                                            compMight.MightData.MightAbilityPoints -= 1;
                                        }
                                    }
                                }
                                num2 += (MightCardUtility.mightCardSize.x / 3) - MightCardUtility.SpacingOffset;
                            }
                        }
                        itnum++;
                    }
                    else
                    {
                        //Log.Message("No skill iteration found.");
                    }
                    num += MightCardUtility.MagicButtonSize + MightCardUtility.TextSize + 4f; //was 4f
                }  
            }
        }
    }
}
