using System;
using UnityEngine;
using Verse;

namespace TorannMagic.ModOptions
{
    public class Controller : Mod
    {
        public static Controller Instance;

        private bool reset = false;

        public override string SettingsCategory()
        {
            return "A RimWorld of Magic";
        }

        public Controller(ModContentPack content) : base(content)
        {
            Controller.Instance = this;
            Settings.Instance = base.GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect canvas)
        {
            int num = 0;
            float rowHeight = 40f;
            Rect rect1 = new Rect(canvas);
            rect1.width /= 2f;
            num++;
            SettingsRef settingsRef = new SettingsRef();
            Rect rowRect = UIHelper.GetRowRect(rect1, rowHeight, num);            

            Settings.Instance.xpMultiplier = Widgets.HorizontalSlider(rowRect, Settings.Instance.xpMultiplier, .1f, 2f, false, "XPMultiplier".Translate() + " " + Settings.Instance.xpMultiplier, ".1", "2", .1f);
            //Rect rowRect1 = rowRect;
            //rowRect1.x = rowRect.x + rect1.width;
            //Widgets.TextFieldNumericLabeled<float>(rowRect1, "CaravanSpeedMultiplier".Translate(), ref Settings.Instance.caravanSpeedMultiplier, ref this.caravanSpeedMultiplierBuffer, .1f, 100f);
            num++;
            Rect rowRect2 = UIHelper.GetRowRect(rowRect, rowHeight, num);
            Settings.Instance.needMultiplier = Widgets.HorizontalSlider(rowRect2, Settings.Instance.needMultiplier, .1f, 4f, false, "NeedMultiplier".Translate() + " " + Settings.Instance.needMultiplier, ".1", "4", .1f);
            num++;
            Rect rowRect3 = UIHelper.GetRowRect(rowRect2, rowHeight, num);
            Settings.Instance.deathExplosionRadius = Widgets.HorizontalSlider(rowRect3, Settings.Instance.deathExplosionRadius, 2f, 6f, false, "DeathRadius".Translate() + " " + Settings.Instance.deathExplosionRadius, "2", "6", .1f);
            num++;
            Rect rowRect4 = UIHelper.GetRowRect(rowRect3, rowHeight, num);
            Settings.Instance.baseMageChance = Widgets.HorizontalSlider(rowRect4, Settings.Instance.baseMageChance, 0f, 5f, false, "baseMageChance".Translate() + " " + Rarity(Settings.Instance.baseMageChance), "0", "5", .01f);
            num++;
            Rect rowRect5 = UIHelper.GetRowRect(rowRect4, rowHeight, num);
            Settings.Instance.baseFighterChance = Widgets.HorizontalSlider(rowRect5, Settings.Instance.baseFighterChance, 0f, 5f, false, "baseFighterChance".Translate() + " " + Rarity(Settings.Instance.baseFighterChance), "0", "5", .01f);
            num++;
            Rect rowRect6 = UIHelper.GetRowRect(rowRect5, rowHeight, num);
            Settings.Instance.advMageChance = Widgets.HorizontalSlider(rowRect6, Settings.Instance.advMageChance, 0f, 2f, false, "advMageChance".Translate() + " " + Rarity(Settings.Instance.advMageChance), "0", "2", .01f);
            num++;
            Rect rowRect66 = UIHelper.GetRowRect(rowRect6, rowHeight, num);
            Settings.Instance.advFighterChance = Widgets.HorizontalSlider(rowRect66, Settings.Instance.advFighterChance, 0f, 2f, false, "advFighterChance".Translate() + " " + Rarity(Settings.Instance.advFighterChance), "0", "2", .01f);
            num++;
            Rect rowRect7 = UIHelper.GetRowRect(rowRect66, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect7, "AICanCast".Translate(), ref Settings.Instance.AICasting, false);
            num++;
            Rect rowRect8 = UIHelper.GetRowRect(rowRect7, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect8, "AIHardMode".Translate(), ref Settings.Instance.AIHardMode, !settingsRef.AICasting);
            num++;
            Rect rowRect9 = UIHelper.GetRowRect(rowRect8, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect9, "AIMarking".Translate(), ref Settings.Instance.AIMarking, false);
            num++;
            Rect rowRect10 = UIHelper.GetRowRect(rowRect9, rowHeight, num);
            GUI.color = Color.yellow;
            reset = Widgets.ButtonText(rowRect10, "Reset", false, false, true);
            if (reset)
            {
                Settings.Instance.xpMultiplier = 1f;
                Settings.Instance.needMultiplier = 1f;
                Settings.Instance.deathExplosionRadius = 3f;
                Settings.Instance.AICasting = true;
                Settings.Instance.AIHardMode = false;
                Settings.Instance.AIMarking = true;
                Settings.Instance.baseMageChance = 1f;
                Settings.Instance.baseFighterChance = 1f;
                Settings.Instance.advMageChance = 0.5f;
                Settings.Instance.advFighterChance = 0.5f;
            }
        }

        public static class UIHelper
        {
            public static Rect GetRowRect(Rect inRect, float rowHeight, int row)
            {
                float y = rowHeight * (float)row;
                Rect result = new Rect(inRect.x, y, inRect.width, rowHeight);
                return result;
            }
        }

        private string Rarity(float val)
        {
            string rarity = "";
            if (val == 0)
            {
                rarity = "None";
            }
            else if (val < .2f)
            {
                rarity = "Very Rare";
            }
            else if (val < .5f)
            {
                rarity = "Rare";
            }
            else if (val < 1f)
            {
                rarity = "Uncommon";
            }
            else if (val < 2f)
            {
                rarity = "Common";
            }
            else
            {
                rarity = "Frequent";
            }
            return rarity;
        }
    }
}
