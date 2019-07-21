using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using UnityEngine;
using RimWorld;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    internal class Gizmo_EnergyStatus : Gizmo
    {
        //public HediffComp_Shield shield;

        private static readonly Texture2D FullStaminaTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.0f, 0.5f, 0.0f));
        private static readonly Texture2D FullManaTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.55f, 0.03f, 1f));
        private static readonly Texture2D FullPsionicTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.0f, 0.5f, 1f));
        private static readonly Texture2D FullDeathKnightTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.6f, 0.0f, 0f));
        private static readonly Texture2D FullBloodMageTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.4f, 0.0f, 0f));
        private static readonly Texture2D FullChiTex = SolidColorMaterials.NewSolidColorTexture(new Color(1, .75f, 0));
        private static readonly Texture2D FullCountTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));
        private static readonly Texture2D FullNecroticTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.32f, 0.4f, 0.0f));

        private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

        public Pawn pawn;
        public Enchantment.CompEnchantedItem iComp = null;

        public override float GetWidth(float maxWidth)
        {
            return 100f;            
        }

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
        {
            if (!pawn.DestroyedOrNull() && !pawn.Dead)
            {
                CompAbilityUserMagic compMagic = pawn.GetComp<CompAbilityUserMagic>();
                CompAbilityUserMight compMight = pawn.GetComp<CompAbilityUserMight>();

                bool isMage = compMagic.IsMagicUser && !pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless);
                bool isFighter = compMight.IsMightUser;
                bool isPsionic = pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_PsionicHD"), false);
                bool isBloodMage = pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_BloodHD"), false);
                bool isMonk = pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_ChiHD, false);
                bool isEnchantedItem = this.iComp != null;
                Hediff hediff = null;
                for (int h = 0; h < pawn.health.hediffSet.hediffs.Count; h++)
                {
                    if (pawn.health.hediffSet.hediffs[h].def.defName.Contains("TM_HateHD"))
                    {
                        hediff = pawn.health.hediffSet.hediffs[h];
                    }
                }
                bool isDeathKnight = hediff != null;
                //bool isLich = pawn.story.traits.HasTrait(TorannMagicDefOf.Lich);
                float barCount = 1;
                float boostSev = 100;
                if (isFighter)
                {
                    barCount++;
                }
                if (isMage)
                {
                    barCount++;
                }
                if (isPsionic)
                {
                    barCount++;
                    Hediff hediffBoost = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_Artifact_PsionicBoostHD);
                    if (hediffBoost != null)
                    {
                        boostSev += hediffBoost.Severity;
                    }
                }
                if (isDeathKnight)
                {
                    barCount++;
                    Hediff hediffBoost = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_Artifact_HateBoostHD);
                    if (hediffBoost != null)
                    {
                        boostSev += hediffBoost.Severity;
                    }
                }
                if (isBloodMage)
                {
                    barCount++;
                    Hediff hediffBoost = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_Artifact_BloodBoostHD);
                    if (hediffBoost != null)
                    {
                        boostSev += hediffBoost.Severity;
                    }
                }
                if(isMonk)
                {
                    barCount++;
                }
                if(isEnchantedItem)
                {
                    barCount++;
                }
                float barHeight;
                float initialShift = 0;
                if (barCount == 2)
                {
                    initialShift = 14f;
                }
                else if (barCount >= 3)
                {
                    initialShift = 6f;
                }
                if (barCount > 1 && ((isFighter && compMight.Stamina != null) || (isMage && compMagic.Mana != null) || (isEnchantedItem && iComp.NecroticEnergy > 0)))
                {
                    Rect overRect = new Rect(topLeft.x + 2, topLeft.y, this.GetWidth(100), 75); //overall rect size (shell)
                    Find.WindowStack.ImmediateWindow(984698, overRect, WindowLayer.GameUI, delegate
                    {
                        barHeight = ((75 - 5) / barCount);
                        Rect rect = overRect.AtZero().ContractedBy(6f); //inner, smaller rect
                        rect.height = barHeight;
                        Rect rect2 = rect; //label rect, starts at top             
                        Text.Font = GameFont.Tiny;
                        float fillPercent = 0;
                        float yShift = initialShift;
                        Text.Anchor = TextAnchor.MiddleCenter;
                        if (isPsionic)
                        {
                            rect2.y += yShift;
                            try
                            {
                                fillPercent = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_PsionicHD"), false).Severity / (boostSev);
                                Widgets.FillableBar(rect2, fillPercent, Gizmo_EnergyStatus.FullPsionicTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                                Widgets.Label(rect2, "" + (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_PsionicHD"), false).Severity).ToString("F0") + " / " + boostSev.ToString("F0"));
                            }
                            catch
                            {
                                fillPercent = 0f;
                                Widgets.FillableBar(rect2, fillPercent, Gizmo_EnergyStatus.FullPsionicTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                                Widgets.Label(rect2, "");
                            }
                            yShift += (barHeight) + 5f;
                        }
                        if (isDeathKnight)
                        {
                            rect2.y += yShift;
                            try
                            {
                                fillPercent = hediff.Severity / boostSev;
                                Widgets.FillableBar(rect2, fillPercent, Gizmo_EnergyStatus.FullDeathKnightTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                                Widgets.Label(rect2, "" + hediff.Severity.ToString("F0") + " / " + boostSev.ToString("F0"));
                            }
                            catch
                            {
                                fillPercent = 0f;
                                Widgets.FillableBar(rect2, fillPercent, Gizmo_EnergyStatus.FullDeathKnightTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                                Widgets.Label(rect2, "");
                            }
                            yShift += (barHeight) + 5f;
                        }
                        if (isBloodMage)
                        {
                            rect2.y += yShift;
                            try
                            {
                                fillPercent = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_BloodHD"), false).Severity / boostSev;
                                Widgets.FillableBar(rect2, fillPercent, Gizmo_EnergyStatus.FullBloodMageTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                                Widgets.Label(rect2, "" + (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_BloodHD"), false).Severity).ToString("F0") + " / " + boostSev.ToString("F0"));
                            }
                            catch
                            {
                                fillPercent = 0f;
                                Widgets.FillableBar(rect2, fillPercent, Gizmo_EnergyStatus.FullBloodMageTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                                Widgets.Label(rect2, "");
                            }
                            yShift += (barHeight) + 5f;
                        }
                        if (isMonk)
                        {
                            rect2.y += yShift;
                            try
                            {
                                fillPercent = pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_ChiHD, false).Severity / boostSev;
                                Widgets.FillableBar(rect2, fillPercent, Gizmo_EnergyStatus.FullChiTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                                Widgets.Label(rect2, "" + (pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_ChiHD, false).Severity).ToString("F0") + " / " + boostSev.ToString("F0"));
                            }
                            catch
                            {
                                fillPercent = 0f;
                                Widgets.FillableBar(rect2, fillPercent, Gizmo_EnergyStatus.FullChiTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                                Widgets.Label(rect2, "");
                            }
                            yShift += (barHeight) + 5f;
                        }
                        Rect rect3 = rect; // bar rect, starts at bottom of label rect
                        if (isFighter)
                        {
                            rect3.y += yShift; //shift downward without changing height
                            try
                            {
                                fillPercent = compMight.Stamina.CurInstantLevel / compMight.maxSP;
                                Widgets.FillableBar(rect3, fillPercent, Gizmo_EnergyStatus.FullStaminaTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                                Widgets.Label(rect3, "" + (compMight.Stamina.CurInstantLevel * 100).ToString("F0") + " / " + (compMight.maxSP * 100).ToString("F0"));
                            }
                            catch
                            {
                                fillPercent = 0f;
                                Widgets.FillableBar(rect3, fillPercent, Gizmo_EnergyStatus.FullStaminaTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                                Widgets.Label(rect3, "");
                            }
                            
                            yShift += (barHeight) + 5f;
                        }
                        Rect rect4 = rect; // bar rect, starts at bottom of label rect
                        if (isMage)
                        {
                            rect4.y += yShift; //shift downward without changing height
                            try
                            {
                                fillPercent = compMagic.Mana.CurInstantLevel / compMagic.maxMP;
                                Widgets.FillableBar(rect4, fillPercent, Gizmo_EnergyStatus.FullManaTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                                Widgets.Label(rect4, "" + (compMagic.Mana.CurInstantLevel * 100).ToString("F0") + " / " + (compMagic.maxMP * 100).ToString("F0"));
                            }
                            catch
                            {
                                fillPercent = 0f;
                                Widgets.FillableBar(rect4, fillPercent, Gizmo_EnergyStatus.FullManaTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                                Widgets.Label(rect4, "");
                            }
                            yShift += (barHeight) + 5f;
                        }
                        Rect rect5 = rect; // bar rect, starts at bottom of label rect
                        if (isEnchantedItem)
                        {
                            rect5.y += yShift; //shift downward without changing height
                            try
                            {
                                fillPercent = iComp.NecroticEnergy / 100f;
                                Widgets.FillableBar(rect5, fillPercent, Gizmo_EnergyStatus.FullNecroticTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                                Widgets.Label(rect5, "" + (iComp.NecroticEnergy).ToString("F0") + " / " + (100).ToString("F0"));
                            }
                            catch
                            {
                                fillPercent = 0f;
                                Widgets.FillableBar(rect5, fillPercent, Gizmo_EnergyStatus.FullNecroticTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                                Widgets.Label(rect5, "");
                            }
                            yShift += (barHeight) + 5f;
                        }
                        Text.Font = GameFont.Small;
                        Text.Anchor = TextAnchor.UpperLeft;
                    }, true, false, 1f);
                }
            }
            else
            {
                Rect overRect = new Rect(topLeft.x + 2, topLeft.y, this.GetWidth(100), 75); //overall rect size (shell)
                float barHeight;
                float initialShift = 0;
                Find.WindowStack.ImmediateWindow(984698, overRect, WindowLayer.GameUI, delegate
                {
                    barHeight = ((75 - 5) / 1);
                    Rect rect = overRect.AtZero().ContractedBy(6f); //inner, smaller rect
                    rect.height = barHeight;
                    Rect rect2 = rect; //label rect, starts at top             
                    Text.Font = GameFont.Tiny;
                    float fillPercent = 0;
                    float yShift = initialShift;
                    Text.Anchor = TextAnchor.MiddleCenter;
                    rect2.y += yShift;
                    fillPercent = 0f;
                    Widgets.FillableBar(rect2, fillPercent, Gizmo_EnergyStatus.FullPsionicTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                    Widgets.Label(rect2, "" );
                    yShift += (barHeight) + 5f;
                }, true, false, 1f);
            }
            return new GizmoResult(GizmoState.Clear);
        }
    }
}
