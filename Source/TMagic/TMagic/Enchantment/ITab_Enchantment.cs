using RimWorld;
using System;
using System.Text;
using UnityEngine;
using Verse;

namespace TorannMagic.Enchantment
{
    internal class ITab_Enchantment : ITab
    {
        private static readonly Vector2 WinSize = new Vector2(400f, 550f);

        private static CompEnchantedItem SelectedCompEnchantment
        {
            get
            {
                Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
                if (singleSelectedThing != null)
                {
                    return ThingCompUtility.TryGetComp<CompEnchantedItem>(singleSelectedThing);
                }
                return null;
            }
        }

        public override bool IsVisible
        {
            get
            {
                return ITab_Enchantment.SelectedCompEnchantment != null && ITab_Enchantment.SelectedCompEnchantment.Props.HasEnchantment;
            }
        }

        public ITab_Enchantment()
        {
            this.size = ITab_Enchantment.WinSize;
            this.labelKey = "TabEnchantment";
        }

        protected override void FillTab()
        {
            CompEnchantedItem enchantedItem = ThingCompUtility.TryGetComp<CompEnchantedItem>(Find.Selector.SingleSelectedThing);
            Rect rect = GenUI.ContractedBy(new Rect(0f, 0f, ITab_Enchantment.WinSize.x, ITab_Enchantment.WinSize.y), 10f);
            Rect rect2 = rect;
            Text.Font = GameFont.Small;
            string rectLabel = "Enchantments:"; 
            Widgets.Label(rect2, rectLabel);
            int num = 2;
            Text.Font = GameFont.Tiny;
            Rect rect3 = GetRowRect(rect2, num);            
            if(enchantedItem.Props.maxMP !=0)
            {
                GUI.color = GenEnchantmentColor.EnchantmentColor(enchantedItem.Props.maxMPTier);
                rectLabel = enchantedItem.Props.MaxMPLabel;
                Widgets.Label(rect3, rectLabel);
                num++;
            }
            Rect rect4 = GetRowRect(rect3, num);
            if (enchantedItem.Props.mpCost != 0)
            {
                GUI.color = GenEnchantmentColor.EnchantmentColor(enchantedItem.Props.mpCostTier);
                rectLabel = enchantedItem.Props.MPCostLabel;
                Widgets.Label(rect4, rectLabel);
                num++;
            }
            Rect rect5 = GetRowRect(rect4, num);
            if (enchantedItem.Props.mpRegenRate != 0)
            {
                GUI.color = GenEnchantmentColor.EnchantmentColor(enchantedItem.Props.mpRegenRateTier);
                rectLabel = enchantedItem.Props.MPRegenRateLabel;
                Widgets.Label(rect5, rectLabel);
                num++;
            }
            Rect rect6 = GetRowRect(rect5,  num);
            if (enchantedItem.Props.coolDown != 0)
            {
                GUI.color = GenEnchantmentColor.EnchantmentColor(enchantedItem.Props.coolDownTier);
                rectLabel = enchantedItem.Props.CoolDownLabel;
                Widgets.Label(rect6, rectLabel);
                num++;
            }
            Rect rect7 = GetRowRect(rect6,  num);
            if (enchantedItem.Props.xpGain != 0)
            {
                GUI.color = GenEnchantmentColor.EnchantmentColor(enchantedItem.Props.xpGainTier);
                rectLabel = enchantedItem.Props.XPGainLabel;
                Widgets.Label(rect7, rectLabel);
                num++;
            }
            //rect3.yMin += Text.CalcHeight(rectLabel, rect.width);

            //QualityCategory qualityCategory;
            //QualityUtility.TryGetQuality(ITab_Enchantment.SelectedCompEnchantment.parent, out qualityCategory);
            //StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append(GenText.CapitalizeFirst(QualityUtility.GetLabel(qualityCategory))).Append(" ").Append(ResourceBank.StringQuality).Append(" ");
            //if (ITab_Enchantment.SelectedCompEnchantment.parent.Stuff != null)
            //{
            //    stringBuilder.Append(ITab_Enchantment.SelectedCompEnchantment.parent.Stuff.LabelAsStuff).Append(" ");
            //}
            //stringBuilder.Append(ITab_Enchantment.SelectedCompEnchantment.parent.def.label);
            //string text = stringBuilder.ToString();
            //Widgets.Label(rect3, text);
            //GUI.color = Color.white;
            //Text.Anchor = TextAnchor.UpperLeft;
            //Rect rect4 = rect;
            //rect4.yMin += rect3.yMin + Text.CalcHeight(text, rect.width);
            //Text.Font = GameFont.Tiny;
            //Widgets.Label(rect4, ITab_Enchantment.SelectedCompEnchantment.parent.GetInfusionDesc());
        }

        //private static string GetRectLabel()
        //{
        //    InfusionSet infusions = ITab_Enchantment.SelectedCompInfusion.Infusions;
        //    EnchantmentDef enchantment = infusions.enchantment;

        //    Color color;
        //    color = MathUtility.Max(enchantment.tier, 0).InfusionColor();
        //    GUI.color = color;
        //    return GenText.CapitalizeFirst(ITab_Enchantment.SelectedCompInfusion.parent.GetInfusedLabel(true, true));
        //}

        public static Rect GetRowRect(Rect inRect, int row)
        {
            float y = 20f * (float)row;
            Rect result = new Rect(inRect.x, y, inRect.width, 18f);
            return result;
        }
    }
}
