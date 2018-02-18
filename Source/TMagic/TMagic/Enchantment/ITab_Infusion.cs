using RimWorld;
using System;
using System.Text;
using UnityEngine;
using Verse;

namespace TorannMagic.Enchantment
{
    internal class ITab_Infusion : ITab
    {
        private static readonly Vector2 WinSize = new Vector2(400f, 550f);

        private static CompInfusion SelectedCompInfusion
        {
            get
            {
                Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
                if (singleSelectedThing != null)
                {
                    return ThingCompUtility.TryGetComp<CompInfusion>(singleSelectedThing);
                }
                return null;
            }
        }

        public override bool IsVisible
        {
            get
            {
                return ITab_Infusion.SelectedCompInfusion != null && ITab_Infusion.SelectedCompInfusion.Infused;
            }
        }

        public ITab_Infusion()
        {
            this.size = ITab_Infusion.WinSize;
            this.labelKey = "TabInfusion";
        }

        protected override void FillTab()
        {
            Rect rect = GenUI.ContractedBy(new Rect(0f, 0f, ITab_Infusion.WinSize.x, ITab_Infusion.WinSize.y), 10f);
            Rect rect2 = rect;
            Text.Font = GameFont.Small;
            string rectLabel = ITab_Infusion.GetRectLabel();
            Widgets.Label(rect2, rectLabel);
            Rect rect3 = rect;
            rect3.yMin += Text.CalcHeight(rectLabel, rect.width);
            Text.Font = GameFont.Tiny;
            QualityCategory qualityCategory;
            QualityUtility.TryGetQuality(ITab_Infusion.SelectedCompInfusion.parent, out qualityCategory);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(GenText.CapitalizeFirst(QualityUtility.GetLabel(qualityCategory))).Append(" ").Append(ResourceBank.StringQuality).Append(" ");
            if (ITab_Infusion.SelectedCompInfusion.parent.Stuff != null)
            {
                stringBuilder.Append(ITab_Infusion.SelectedCompInfusion.parent.Stuff.LabelAsStuff).Append(" ");
            }
            stringBuilder.Append(ITab_Infusion.SelectedCompInfusion.parent.def.label);
            string text = stringBuilder.ToString();
            Widgets.Label(rect3, text);
            GUI.color = Color.white;
            Text.Anchor = TextAnchor.UpperLeft;
            Rect rect4 = rect;
            rect4.yMin += rect3.yMin + Text.CalcHeight(text, rect.width);
            Text.Font = GameFont.Tiny;
            Widgets.Label(rect4, ITab_Infusion.SelectedCompInfusion.parent.GetInfusionDesc());
        }

        private static string GetRectLabel()
        {
            InfusionSet infusions = ITab_Infusion.SelectedCompInfusion.Infusions;
            EnchantmentDef enchantment = infusions.enchantment;

            Color color;
            color = MathUtility.Max(enchantment.tier, 0).InfusionColor();            
            GUI.color = color;
            return GenText.CapitalizeFirst(ITab_Infusion.SelectedCompInfusion.parent.GetInfusedLabel(true, true));
        }
    }
}
