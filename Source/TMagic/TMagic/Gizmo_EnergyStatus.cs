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
        private static readonly Texture2D FullCountTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.2f, 0.24f));

        private static readonly Texture2D EmptyShieldBarTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

        public Pawn pawn;

        public override float GetWidth(float maxWidth)
        {
            return 100f;            
        }

        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
        {
            CompAbilityUserMagic compMagic = pawn.GetComp<CompAbilityUserMagic>();
            CompAbilityUserMight compMight = pawn.GetComp<CompAbilityUserMight>();

            bool isMage = compMagic.IsMagicUser && !pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless);
            bool isFighter = compMight.IsMightUser;
            bool isPsionic = pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_PsionicHD"), false);
            bool isBloodMage = pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_BloodHD"), false);
            Hediff hediff = null;
            for(int h =0; h < pawn.health.hediffSet.hediffs.Count; h++)
            {
                if(pawn.health.hediffSet.hediffs[h].def.defName.Contains("TM_HateHD"))
                {
                    hediff = pawn.health.hediffSet.hediffs[h];
                }
            }
            bool isDeathKnight = hediff != null;            
            //bool isLich = pawn.story.traits.HasTrait(TorannMagicDefOf.Lich);
            float barCount = 1;            
            if(isFighter)
            {
                barCount++;
            }
            if(isMage)
            {
                barCount++;
            }
            if(isPsionic)
            {
                barCount++;
            }
            if (isDeathKnight)
            {
                barCount++;
            }
            if(isBloodMage)
            {
                barCount++;
            }
            float barHeight;
            float initialShift=0;
            if (barCount == 2)
            {
                initialShift = 14f;
            }
            else if (barCount >= 3)
            {
                initialShift = 6f;
            }
            if (barCount > 1 && (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_PsionicHD"), false) || compMight.Stamina != null || compMagic.Mana != null))
            {
                Rect overRect = new Rect(topLeft.x+2, topLeft.y, this.GetWidth(100), 75); //overall rect size (shell)
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
                        fillPercent = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_PsionicHD"), false).Severity / 100f;
                        Widgets.FillableBar(rect2, fillPercent, Gizmo_EnergyStatus.FullPsionicTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                        Widgets.Label(rect2, "" + (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_PsionicHD"), false).Severity).ToString("F0") + " / 100");
                        yShift += (barHeight) + 5f;
                    }
                    if (isDeathKnight)
                    {
                        rect2.y += yShift;
                        fillPercent = hediff.Severity / 100f;
                        Widgets.FillableBar(rect2, fillPercent, Gizmo_EnergyStatus.FullDeathKnightTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                        Widgets.Label(rect2, "" + hediff.Severity.ToString("F0") + " / 100");
                        yShift += (barHeight) + 5f;
                    }
                    if (isBloodMage)
                    {
                        rect2.y += yShift;
                        fillPercent = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_BloodHD"), false).Severity / 100f;
                        Widgets.FillableBar(rect2, fillPercent, Gizmo_EnergyStatus.FullBloodMageTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                        Widgets.Label(rect2, "" + (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_BloodHD"), false).Severity).ToString("F0") + " / 100");
                        yShift += (barHeight) + 5f;
                    }
                    Rect rect3 = rect; // bar rect, starts at bottom of label rect
                    if (isFighter)
                    {
                        rect3.y += yShift; //shift downward without changing height
                        fillPercent = compMight.Stamina.CurInstantLevel / compMight.maxSP;
                        Widgets.FillableBar(rect3, fillPercent, Gizmo_EnergyStatus.FullStaminaTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                        Widgets.Label(rect3, "" + (compMight.Stamina.CurInstantLevel * 100).ToString("F0") + " / " + (compMight.maxSP * 100).ToString("F0"));
                        yShift += (barHeight) + 5f;
                    }
                    Rect rect4 = rect; // bar rect, starts at bottom of label rect
                    if (isMage)
                    {
                        rect4.y += yShift; //shift downward without changing height
                        fillPercent = compMagic.Mana.CurInstantLevel / compMagic.maxMP;
                        Widgets.FillableBar(rect4, fillPercent, Gizmo_EnergyStatus.FullManaTex, Gizmo_EnergyStatus.EmptyShieldBarTex, false);
                        Widgets.Label(rect4, "" + (compMagic.Mana.CurInstantLevel * 100).ToString("F0") + " / " + (compMagic.maxMP * 100).ToString("F0"));
                    }
                    Text.Font = GameFont.Small;
                    Text.Anchor = TextAnchor.UpperLeft;
                }, true, false, 1f);
            }
            return new GizmoResult(GizmoState.Clear);
            
        }
    }
}
