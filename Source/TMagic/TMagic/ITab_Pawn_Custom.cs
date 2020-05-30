using HarmonyLib;
using RimWorld;
using System;
using UnityEngine;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    public class ITab_Pawn_Custom : ITab  //code by Jecrell
    {
        private static readonly Texture2D manaTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.0f, 0.5f, 1f));
        private Pawn PawnToShowInfoAbout
        {
            get
            {
                Pawn pawn = base.SelPawn ?? (base.SelThing as Corpse)?.InnerPawn;
                bool flag3 = pawn == null;
                if (pawn == null)
                {
                    Log.Error("Character tab found no selected pawn to display.");
                }
                return pawn;
            }
        }

        public override bool IsVisible
        {
            get
            {
                bool flag = base.SelPawn.story != null && base.SelPawn.IsColonist;
                if (flag)
                {
                    CompAbilityUserCustom compMagic = base.SelPawn.TryGetComp<CompAbilityUserCustom>();
                    return compMagic?.IsInitialized ?? false;
                }
                return false;
            }
        }

        public ITab_Pawn_Custom()
        {
            this.size = MagicCardUtility.MagicCardSize + new Vector2(17f, 17f) * 2f;
            this.labelKey = "TM_TabCustom";
        }

        protected override void FillTab()
        {
            Rect rect = new Rect(17f, 17f, MagicCardUtility.MagicCardSize.x, MagicCardUtility.MagicCardSize.y);
            DrawMagicCard(rect, this.PawnToShowInfoAbout);
        }

        public static void DrawMagicCard(Rect rect, Pawn pawn)
        {

            CompAbilityUserCustom comp = pawn.GetComp<CompAbilityUserCustom>();
            int sizeY = 0;
            if (comp.customClass != null)
            {
                sizeY = (comp.Data.GetUniquePowersWithSkillsCount() * 80);
                if (sizeY > 500)
                {
                    sizeY -= 500;
                }
                else
                {
                    sizeY = 0;
                }
            }

            Rect sRect = new Rect(rect.x, rect.y, rect.width - 36f, rect.height + 56f + sizeY);
            //scrollPosition = GUI.BeginScrollView(rect, scrollPosition, sRect, false, true);
            GUI.BeginScrollView(rect, Vector2.zero, sRect, false, true);

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
                    InfoPane(inRect, comp, pawn);
                    float x5 = Text.CalcSize("TM_Spells".Translate()).x;
                    Rect rect10 = new Rect(rect.width / 2f - x5 / 2f, rect9.yMax - 60f, rect.width, MagicCardUtility.HeaderSize);
                    Text.Font = GameFont.Small;
                    Widgets.Label(rect10, "TM_Spells".Translate().CapitalizeFirst());
                    Text.Font = GameFont.Small;
                    Widgets.DrawLineHorizontal(rect.x - 10f, rect10.yMax, rect.width - 15f);
                    Rect rect11 = new Rect(rect.x, rect10.yMax + MagicCardUtility.SectionOffset, rect10.width, MagicCardUtility.PowersColumnHeight);
                    if (comp.customClass != null)
                    {
                        Rect inRect3 = new Rect(rect.x, rect11.y, MagicCardUtility.PowersColumnWidth, MagicCardUtility.PowersColumnHeight);
                        CustomPowersHandler(inRect3, comp, TexButton.TMTex_SkillPointUsed);
                    }
                }
            }
            GUI.EndScrollView();
            //Widgets.EndScrollView();
            //GUI.EndGroup();
        }

        public static void InfoPane(Rect inRect, CompAbilityUserCustom compMagic, Pawn pawn)
        {
            Rect rect = new Rect(inRect.x, inRect.y, inRect.width * 0.7f, MagicCardUtility.TextSize);
            Text.Font = GameFont.Tiny;
            Widgets.Label(rect, "TM_Level".Translate().CapitalizeFirst() + ": " + compMagic.Data.UserLevel.ToString());
            Text.Font = GameFont.Tiny;
            bool godMode = DebugSettings.godMode;
            if (godMode)
            {
                Rect rect2 = new Rect(rect.xMax, inRect.y, inRect.width * 0.3f, MagicCardUtility.TextSize);
                bool flag = Widgets.ButtonText(rect2, "+", true, false, true);
                if (flag)
                {
                    //compMagic.LevelUp(true);
                    compMagic.Data.UserXP += 400; // testing xp bar and levelling
                }
                if (false)
                {
                    Rect rect22 = new Rect(rect.xMax + 60f, inRect.y, 50f, MagicCardUtility.TextSize * 2);
                    bool flag22 = Widgets.ButtonText(rect22, "Reset Skills", true, false, true);
                    if (flag22)
                    {
                        compMagic.ResetSkills();
                    }
                    Rect rect23 = new Rect(rect.xMax + 115f, inRect.y, 50f, MagicCardUtility.TextSize * 2);
                    bool flag23 = Widgets.ButtonText(rect23, "Remove Powers", true, false, true);
                    if (flag23)
                    {
                        compMagic.RemoveAbilityUser();
                    }
                }
            }
            Rect rect4 = new Rect(inRect.x, rect.yMax, inRect.width, MagicCardUtility.TextSize);
            Text.Font = GameFont.Tiny;
            Widgets.Label(rect4, "TM_PointsAvail".Translate() + ": " + compMagic.Data.AbilityPoints);
            Text.Font = GameFont.Tiny;
            if (!godMode)
            {
                //TODO: Display resources info
            }
            Rect rect5 = new Rect(rect4.x, rect4.yMax + 3f, inRect.width + 100f, MagicCardUtility.HeaderSize * 0.6f);
            DrawLevelBar(rect5, compMagic, pawn, inRect);
        }

        public static void DrawLevelBar(Rect rect, CompAbilityUserCustom compMagic, Pawn pawn, Rect rectG)
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
            TooltipHandler.TipRegion(rect, new TipSignal(() => MagicXPTipString(compMagic.Data), rect.GetHashCode()));
            float num2 = 14f;
            bool flag3 = rect.height < 50f;
            if (flag3)
            {
                num2 *= Mathf.InverseLerp(0f, 50f, rect.height);
            }
            Text.Anchor = TextAnchor.UpperLeft;
            Rect rect2 = new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height);
            rect2 = new Rect(rect2.x, rect2.y, rect2.width, rect2.height - num2);
            float xpPercent = (compMagic.Data.UserXP - compMagic.Data.XPForLevel(compMagic.Data.UserLevel))
                / (float)(compMagic.Data.XPForLevel(compMagic.Data.UserLevel + 1) - compMagic.Data.XPForLevel(compMagic.Data.UserLevel));
            Widgets.FillableBar(rect2, xpPercent, manaTex, BaseContent.GreyTex, false);
            Rect rect3 = new Rect(rect2.x + 272f + MagicCardUtility.MagicButtonPointSize, rectG.y, 136f, MagicCardUtility.TextSize);
            Rect rect31 = new Rect(rect2.x + 272f, rectG.y, MagicCardUtility.MagicButtonPointSize, MagicCardUtility.TextSize);
            Rect rect4 = new Rect(rect3.x + rect3.width + (MagicCardUtility.MagicButtonPointSize * 2), rectG.y, 136f, MagicCardUtility.TextSize);
            Rect rect41 = new Rect(rect3.x + rect3.width + MagicCardUtility.MagicButtonPointSize, rectG.y, MagicCardUtility.MagicButtonPointSize, MagicCardUtility.TextSize);
            Rect rect5 = new Rect(rect2.x + 272f + MagicCardUtility.MagicButtonPointSize, rectG.yMin + 24f, 136f, MagicCardUtility.TextSize);
            Rect rect51 = new Rect(rect2.x + 272f, rectG.yMin + 24f, MagicCardUtility.MagicButtonPointSize, MagicCardUtility.TextSize);

            //TODO: draw global skills? One could also add a fake power and put the global abilities there.
        }

        public static string MagicXPTipString(CustomData compMagic)
        {
            return string.Concat(
                compMagic.UserXP.ToString(),
                " / ",
                compMagic.XPForLevel(compMagic.UserLevel + 1).ToString(),
                "\n",
                "TM_MagicXPDesc".Translate()
            );
        }

        public static void CustomPowersHandler(Rect inRect, CompAbilityUserCustom compMagic, Texture2D pointTexture)
        {
            float yOffset = inRect.y;
            int itnum = 1;
            using (List<BasePower>.Enumerator enumerator = compMagic.Data.Powers.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    BasePower power = enumerator.Current;
                    TMAbilityDef ability = (TMAbilityDef)power.AbilityDef;
                    if (ability == TorannMagicDefOf.TM_SoulBond || ability == TorannMagicDefOf.TM_ShadowBolt || ability == TorannMagicDefOf.TM_Dominate)
                    {
                        continue;
                    }

                    Text.Font = GameFont.Small;
                    Rect abilityButton = new Rect(MagicCardUtility.MagicCardSize.x / 2f - MagicCardUtility.MagicButtonSize, yOffset, MagicCardUtility.MagicButtonSize, MagicCardUtility.MagicButtonSize);
                    if (itnum > 1)
                    {
                        Widgets.DrawLineHorizontal(0f + 20f, yOffset - 2f, MagicCardUtility.MagicCardSize.x - 40f);
                    }
                    if (power.level < power.maxLevel)
                    {

                        TooltipHandler.TipRegion(abilityButton, () => string.Concat(new string[]
                        {
                        power.AbilityDef.label,
                        "\n\nCurrent Level:\n",
                        power.AbilityDef.description,
                        "\n\nNext Level:\n",
                        power.NextLevelAbilityDef.description,
                        "\n\n",
                        "TM_CheckPointsForMoreInfo".Translate()
                        }), 398462);

                    }
                    else
                    {
                        TooltipHandler.TipRegion(abilityButton, () => string.Concat(new string[]
                            {
                            power.AbilityDef.label,
                            "\n\n",
                            power.AbilityDef.description,
                            "\n\n",
                            "TM_CheckPointsForMoreInfo".Translate()
                            }), 398462);
                    }

                    //float xOffsetSkill = Text.CalcSize("TM_Effeciency".Translate()).x;
                    //float x3 = Text.CalcSize("TM_Versatility".Translate()).x;
                    Rect rect3 = new Rect(0f + MagicCardUtility.SpacingOffset, yOffset + 2f, MagicCardUtility.MagicCardSize.x, MagicCardUtility.ButtonSize * 1.15f);

                    //Rect skillRect = new Rect(rect3.x + rect3.width / 2f - xOffsetSkill, rect3.y, (rect3.width - 20f) / 3f, rect3.height);
                    //Rect rect6 = new Rect(rect3.width - x3 * 2f, rect3.y, rect3.width / 3f, rect3.height);

                    //bool flag9 = power.abilityDef.label == "Ray of Hope" || power.abilityDef.label == "Soothing Breeze" || power.abilityDef.label == "Frost Ray" || power.abilityDef.label == "AMP" || power.abilityDef.label == "Shadow" || power.abilityDef.label == "Magic Missile" || power.abilityDef.label == "Blink" || power.abilityDef.label == "Summon" || power.abilityDef.label == "Shield"; //add all other buffs or xml based upgrades

                    Rect rectLabel = new Rect(0f + 20f, abilityButton.yMin, 350f - 44f, MagicCardUtility.MagicButtonPointSize);
                    //GUI.color = Color.yellow;
                    Widgets.Label(rectLabel, power.AbilityDef.LabelCap);
                    //GUI.color = Color.white;
                    if (power.learned != true)
                    {
                        Widgets.DrawTextureFitted(abilityButton, power.Icon, 1f);
                        Text.Font = GameFont.Tiny;
                        Rect rectLearn = new Rect(abilityButton.xMin - 44f, abilityButton.yMin, 40f, MagicCardUtility.MagicButtonPointSize);
                        if (compMagic.Data.AbilityPoints >= power.learnCost)//&& !power.requiresScroll) // FIXME: the ability can be leveled for 1 point, why?
                        {
                            bool flagLearn = Widgets.ButtonText(rectLearn, "TM_Learn".Translate(), true, false, true) && compMagic.AbilityUser.Faction == Faction.OfPlayer;
                            if (flagLearn)
                            {
                                // power.levelUp(comp); TODO: delegate levelup action to the individual power
                                power.learned = true;
                                if (!(power.AbilityDef?.defName == "TM_TechnoBit"))
                                {
                                    compMagic.AddPawnAbility(enumerator.Current.AbilityDef);
                                }
                                if (power.AbilityDef?.defName == "TM_TechnoWeapon")
                                {
                                    compMagic.AddPawnAbility(TorannMagicDefOf.TM_NanoStimulant);
                                }
                                compMagic.Data.AbilityPoints -= power.learnCost;
                            }
                        }
                        //else if (power.requiresScroll)
                        //{
                        //    Rect rectToLearn = new Rect(rect.xMin - 268f, rect.yMin + 22f, 250f, MagicCardUtility.MagicButtonPointSize);
                        //    bool flagLearn = Widgets.ButtonText(rectToLearn, "TM_SpellLocked".Translate(power.abilityDef.LabelCap), false, false, false) && compMagic.AbilityUser.Faction == Faction.OfPlayer;
                        //}
                        else
                        {
                            string message = "" + enumerator.Current.learnCost + " points to " + "TM_Learn".Translate();
                            float size = Text.CalcSize(message).x;
                            Rect rectToLearn = new Rect(abilityButton.xMin - size, abilityButton.yMin, size, MagicCardUtility.MagicButtonPointSize);
                            Widgets.Label(rectToLearn, message);
                        }
                    }
                    else
                    {
                        string level = " " + power.level + " / " + power.maxLevel;
                        Vector2 levelSpace = Text.CalcSize(level);
                        Rect rightOfImage = new Rect(abilityButton.xMax, abilityButton.yMin, levelSpace.x, levelSpace.y); // MagicCardUtility.TextSize);
                        Widgets.DrawTextureFitted(abilityButton, power.Icon, 1f);
                        if (power.maxLevel > 0)
                        {
                            Widgets.Label(rightOfImage, level);
                        }
                        if (enumerator.Current.level >= power.maxLevel
                            || compMagic.Data.AbilityPoints < power.upgradeCost
                            || compMagic.AbilityUser.Faction != Faction.OfPlayer)
                        {
                        }
                        else
                        {
                            Rect belowLevel = new Rect(abilityButton.xMax, abilityButton.yMax - levelSpace.y, levelSpace.x, levelSpace.y); // MagicCardUtility.TextSize);
                            bool flag1 = Widgets.ButtonText(belowLevel, "+", true, false, true);
                            if (flag1)
                            {
                                // power.levelUp(comp); TODO: delegate levelup action to the individual power
                                compMagic.LevelUpPower(power);
                                compMagic.Data.AbilityPoints -= power.upgradeCost;
                            }
                        }
                    }

                    Text.Font = GameFont.Tiny;
                    float xOffset = rect3.x;

                    float xSpacing = (MagicCardUtility.MagicCardSize.x / power.Upgrades.Count) - MagicCardUtility.SpacingOffset;
                    for (int i = 0; i < power.Upgrades.Count; ++i)
                    {
                        if (power.Upgrades[i] == null) { Log.Message("Null upgrade key in power " + ((power as CustomPower)?.localDef.defName ?? power.AbilityDef.defName)); continue; }
                        MagicPowerSkill skill = compMagic.Data.Upgrades.TryGetValue(power.Upgrades[i]);
                        CustomSkillHandler(xOffset, compMagic, power, skill, rect3);
                        itnum++;
                        xOffset += xSpacing;
                    }
                    yOffset += MagicCardUtility.MagicButtonSize + MagicCardUtility.TextSize + 4f;//MagicCardUtility.SpacingOffset; //was 4f                    
                }
            }
        }

        public static void CustomSkillHandler(float xOffset, CompAbilityUserCustom compMagic, BasePower power, MagicPowerSkill skill, Rect rect3)
        {
            Rect rect4 = new Rect(xOffset + MagicCardUtility.MagicButtonPointSize, rect3.yMax, MagicCardUtility.MagicCardSize.x / 3f, rect3.height);
            Rect rect41 = new Rect(xOffset, rect4.y, MagicCardUtility.MagicButtonPointSize, MagicCardUtility.MagicButtonPointSize);
            Rect rect42 = new Rect(rect41.x, rect4.y, rect4.width - MagicCardUtility.MagicButtonPointSize, rect4.height / 2);
            //string description = skill.shouldTranslate() ? skill.desc.Translate().ToString() : skill.desc;
            //string description = skill.getLocalizedDescription();
            //TODO: find a way to add translation without modifying the language files directly?
            string description = false ? skill.desc.Translate().ToString() : skill.desc;
            string name = false ? skill.label.Translate().ToString() : skill.label;
            TooltipHandler.TipRegion(rect42, new TipSignal(() => description, rect4.GetHashCode()));
            Widgets.Label(rect4, name + ": " + skill.level + " / " + skill.levelMax);
            if (skill.level < skill.levelMax
                && compMagic.Data.AbilityPoints >= skill.costToLevel
                && power.learned)
            {
                bool flag12 = Widgets.ButtonText(rect41, "+", true, false, true) && compMagic.AbilityUser.Faction == Faction.OfPlayer;
                if (flag12)
                {
                    bool flag17 = compMagic.AbilityUser.story != null && compMagic.AbilityUser.story.DisabledWorkTagsBackstoryAndTraits == WorkTags.Violent && power.AbilityDef.MainVerb.isViolent;
                    if (flag17)
                    {
                        Messages.Message("IsIncapableOfViolenceLower".Translate(
                            compMagic.Pawn.Name.ToString()
                        ), MessageTypeDefOf.RejectInput);
                    }
                    else
                    {
                        skill.level++;
                        compMagic.Data.AbilityPoints -= skill.costToLevel;
                    }
                }
            }
        }

    }
}
