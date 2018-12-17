using System;
using UnityEngine;
using Verse;

namespace TorannMagic.ModOptions
{
    public class Controller : Mod
    {
        public static Controller Instance;

        private bool reset = false;
        private bool challenge = false;
        private bool easy = false;
        private bool classOptions = false;

        private Vector2 scrollPosition = Vector2.zero;

        private string deathExplosionDmgMin = "20.0";
        private string deathExplosionDmgMax = "50.0";

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
            float rowHeight = 28f;

            Widgets.BeginScrollView(canvas, ref scrollPosition, canvas, true);

            Rect rect1 = new Rect(canvas);
            rect1.width /= 2f;
            num++;
            num++;
            SettingsRef settingsRef = new SettingsRef();
            Rect rowRect = UIHelper.GetRowRect(rect1, rowHeight, num);        
            Settings.Instance.xpMultiplier = Widgets.HorizontalSlider(rowRect, Settings.Instance.xpMultiplier, .1f, 2f, false, "XPMultiplier".Translate() + " " + Settings.Instance.xpMultiplier, ".1", "2", .1f);
            Rect rowRectShiftRight = UIHelper.GetRowRect(rowRect, rowHeight, num);
            rowRectShiftRight.x += rowRect.width + 56f;
            rowRectShiftRight.width /= 2;
            classOptions = Widgets.ButtonText(rowRectShiftRight, "Class Options", true, false, true);
            if (classOptions)
            {
                Rect rect = new Rect(64f, 64f, 400, 400);
                ClassOptionsWindow newWindow = new ClassOptionsWindow();
                Find.WindowStack.Add(newWindow);                

            }
            num++;
            Rect rowRect2 = UIHelper.GetRowRect(rowRect, rowHeight, num);
            Settings.Instance.needMultiplier = Widgets.HorizontalSlider(rowRect2, Settings.Instance.needMultiplier, .1f, 4f, false, "NeedMultiplier".Translate() + " " + Settings.Instance.needMultiplier, ".1", "4", .1f);
            num++;
            Rect rowRect21 = UIHelper.GetRowRect(rowRect2, rowHeight, num);
            Settings.Instance.magicyteChance = Widgets.HorizontalSlider(rowRect21, Settings.Instance.magicyteChance, 0, .01f, false, "MagicyteChance".Translate() + " " + Settings.Instance.magicyteChance, "0%", "1%", .0001f);
            num++;
            Rect rowRect3 = UIHelper.GetRowRect(rowRect21, rowHeight, num);
            rowRect3.width = rowRect3.width * .7f;
            Settings.Instance.deathExplosionRadius = Widgets.HorizontalSlider(rowRect3, Settings.Instance.deathExplosionRadius, .1f, 6f, false, "DeathRadius".Translate() + " " + Settings.Instance.deathExplosionRadius, ".1", "6", .1f);
            Rect rowRect31 = new Rect(rowRect3.xMax + 4f, rowRect3.y, rowRect2.width/2, rowRect3.height);
            Widgets.TextFieldNumericLabeled<int>(rowRect31, "DeathExplosionMin".Translate(), ref Settings.Instance.deathExplosionMin, ref this.deathExplosionDmgMin, 0, 100);
            Rect rowRect32 = new Rect(rowRect31.xMax + 4f, rowRect3.y, rowRect2.width/2, rowRect3.height);
            Widgets.TextFieldNumericLabeled<int>(rowRect32, "DeathExplosionMax".Translate(), ref Settings.Instance.deathExplosionMax, ref this.deathExplosionDmgMax, 0, 200);
            num++;
            Rect rowRect4 = UIHelper.GetRowRect(rowRect3, rowHeight, num);
            Settings.Instance.baseMageChance = Widgets.HorizontalSlider(rowRect4, Settings.Instance.baseMageChance, 0f, 5f, false, "baseMageChance".Translate() + " " + Rarity(Settings.Instance.baseMageChance), "0", "5", .01f);
            num++;
            Rect rowRect5 = UIHelper.GetRowRect(rowRect4, rowHeight, num);
            Settings.Instance.baseFighterChance = Widgets.HorizontalSlider(rowRect5, Settings.Instance.baseFighterChance, 0f, 5f, false, "baseFighterChance".Translate() + " " + Rarity(Settings.Instance.baseFighterChance), "0", "5", .01f);
            Rect rowRect5ShiftRight = UIHelper.GetRowRect(rowRect5, rowHeight, num);
            rowRect5ShiftRight.x += rowRect5.width + 56f;
            Widgets.CheckboxLabeled(rowRect5ShiftRight, "TM_enableAutocast".Translate(), ref Settings.Instance.autocastEnabled, false);
            num++;
            Rect rowRect6 = UIHelper.GetRowRect(rowRect5, rowHeight, num);
            Settings.Instance.advMageChance = Widgets.HorizontalSlider(rowRect6, Settings.Instance.advMageChance, 0f, 2f, false, "advMageChance".Translate() + " " + Rarity(Settings.Instance.advMageChance), "0", "2", .01f);
            Rect rowRect6ShiftRight = UIHelper.GetRowRect(rowRect6, rowHeight, num);
            rowRect6ShiftRight.x += rowRect6.width + 56f;
            Settings.Instance.autocastMinThreshold = Widgets.HorizontalSlider(rowRect6ShiftRight, Settings.Instance.autocastMinThreshold, 0f, 1f, false, "TM_autocastUndraftedThreshold".Translate() + " " + (Settings.Instance.autocastMinThreshold * 100) + "%", "0", "1", .01f);
            num++;
            Rect rowRect66 = UIHelper.GetRowRect(rowRect6, rowHeight, num);
            Settings.Instance.advFighterChance = Widgets.HorizontalSlider(rowRect66, Settings.Instance.advFighterChance, 0f, 2f, false, "advFighterChance".Translate() + " " + Rarity(Settings.Instance.advFighterChance), "0", "2", .01f);
            Rect rowRect66ShiftRight = UIHelper.GetRowRect(rowRect66, rowHeight, num);
            rowRect66ShiftRight.x += rowRect66.width + 56f;
            Settings.Instance.autocastCombatMinThreshold = Widgets.HorizontalSlider(rowRect66ShiftRight, Settings.Instance.autocastCombatMinThreshold, 0f, 1f, false, "TM_autocastDraftedThreshold".Translate() + " " + (Settings.Instance.autocastCombatMinThreshold * 100) + "%", "0", "1", .01f);
            num++;
            Rect rowRect67 = UIHelper.GetRowRect(rowRect66, rowHeight, num);
            Settings.Instance.riftChallenge = Widgets.HorizontalSlider(rowRect67, Settings.Instance.riftChallenge, 0, 3, false, "riftChallenge".Translate() + " " + Challenge(Settings.Instance.riftChallenge), "0", "3", 1);
            Rect rowRect67ShiftRight = UIHelper.GetRowRect(rowRect67, rowHeight, num);
            rowRect67ShiftRight.x += rowRect67.width + 56f;
            Settings.Instance.autocastEvaluationFrequency = Mathf.RoundToInt(Widgets.HorizontalSlider(rowRect67ShiftRight, Settings.Instance.autocastEvaluationFrequency, 60, 600, false, "TM_autocastEvaluationFrequency".Translate() + " " + (Settings.Instance.autocastEvaluationFrequency / 60) + "seconds", "1", "10", .1f));
            num++;
            num++;
            Rect rowRect7 = UIHelper.GetRowRect(rowRect67, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect7, "AICanCast".Translate(), ref Settings.Instance.AICasting, false);
            Rect rowRect7ShiftRight = UIHelper.GetRowRect(rowRect7, rowHeight, num);
            rowRect7ShiftRight.x += rowRect7.width + 56f;
            Widgets.CheckboxLabeled(rowRect7ShiftRight, "AIHardMode".Translate(), ref Settings.Instance.AIHardMode, !settingsRef.AICasting);            
            num++;
            Rect rowRect9 = UIHelper.GetRowRect(rowRect7, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect9, "AIMarking".Translate(), ref Settings.Instance.AIMarking, false);
            Rect rowRect91 = UIHelper.GetRowRect(rowRect9, rowHeight, num);
            rowRect91.x += rowRect9.width + 56f;
            Widgets.CheckboxLabeled(rowRect91, "AIFighterMarking".Translate(), ref Settings.Instance.AIFighterMarking, false);
            num++;
            Rect rowRect92 = UIHelper.GetRowRect(rowRect9, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect92, "AIFriendlyMarking".Translate(), ref Settings.Instance.AIFriendlyMarking, false);
            Rect rowRect92ShiftRight = UIHelper.GetRowRect(rowRect92, rowHeight, num);
            rowRect92ShiftRight.x += rowRect92.width + 56f;
            Widgets.CheckboxLabeled(rowRect92ShiftRight, "showLevelUpMessage".Translate(), ref Settings.Instance.showLevelUpMessage, false);
            num++;
            Rect rowRect93 = UIHelper.GetRowRect(rowRect92, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect93, "showMagicGizmo".Translate(), ref Settings.Instance.showGizmo, false);
            Rect rowRect93ShiftRight = UIHelper.GetRowRect(rowRect93, rowHeight, num);
            rowRect93ShiftRight.x += rowRect93.width + 56f;
            Widgets.CheckboxLabeled(rowRect93ShiftRight, "showUndeadPawnChange".Translate(), ref Settings.Instance.changeUndeadPawnAppearance, false);
            num++;
            Rect rowRect10 = UIHelper.GetRowRect(rowRect93, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect10, "showAbilitiesOnMultiSelect".Translate(), ref Settings.Instance.showIconsMultiSelect, false);
            Rect rowRect10ShiftRight = UIHelper.GetRowRect(rowRect10, rowHeight, num);
            rowRect10ShiftRight.x += rowRect10.width + 56f;
            Widgets.CheckboxLabeled(rowRect10ShiftRight, "showUndeadAnimalChange".Translate(), ref Settings.Instance.changeUndeadAnimalAppearance, false);
            num++;
            Rect rowRect11 = UIHelper.GetRowRect(rowRect10, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect11, "unrestrictedBloodTypesForBloodMagic".Translate(), ref Settings.Instance.unrestrictedBloodTypes, false);
            num++;
            num++;
            Rect rowRect20 = UIHelper.GetRowRect(rowRect10, rowHeight, num);
            rowRect20.width = 120f;
            Rect rowRect20ShiftRight1 = UIHelper.GetRowRect(rowRect20, rowHeight, num);
            rowRect20ShiftRight1.x = rowRect20.width + 40f;
            Rect rowRect20ShiftRight2 = UIHelper.GetRowRect(rowRect20ShiftRight1, rowHeight, num);
            rowRect20ShiftRight2.x = rowRect20.width + rowRect20ShiftRight1.width + 40f;
            //GUI.color = Color.yellow;
            //GUI.backgroundColor = Color.yellow;
            reset = Widgets.ButtonText(rowRect20, "Default", true, false, true);
            if (reset)
            {
                Settings.Instance.xpMultiplier = 1f;
                Settings.Instance.needMultiplier = 1f;
                Settings.Instance.deathExplosionRadius = 3f;
                Settings.Instance.deathExplosionMin = 20;
                Settings.Instance.deathExplosionMax = 50;
                Settings.Instance.AICasting = true;
                Settings.Instance.AIHardMode = false;
                Settings.Instance.AIMarking = true;
                Settings.Instance.AIFighterMarking = false;
                Settings.Instance.AIFriendlyMarking = false;
                Settings.Instance.baseMageChance = 1f;
                Settings.Instance.baseFighterChance = 1f;
                Settings.Instance.advMageChance = 0.5f;
                Settings.Instance.advFighterChance = 0.5f;
                Settings.Instance.magicyteChance = .005f;
                Settings.Instance.showIconsMultiSelect = true;
                Settings.Instance.showGizmo = true;
                Settings.Instance.showLevelUpMessage = true;
                Settings.Instance.changeUndeadPawnAppearance = true;
                Settings.Instance.changeUndeadAnimalAppearance = true;
                Settings.Instance.riftChallenge = 1f;
                this.deathExplosionDmgMax = "50.0";
                this.deathExplosionDmgMin = "20.0";

                Settings.Instance.autocastEnabled = true;
                Settings.Instance.autocastMinThreshold = .7f;
                Settings.Instance.autocastCombatMinThreshold = .2f;
                Settings.Instance.autocastEvaluationFrequency = 180;
            }

            challenge = Widgets.ButtonText(rowRect20ShiftRight1, "Challenge me!", true, false, true);
            if (challenge)
            {
                Settings.Instance.xpMultiplier = .75f;
                Settings.Instance.needMultiplier = .75f;
                Settings.Instance.deathExplosionRadius = 5f;
                Settings.Instance.deathExplosionMin = 30;
                Settings.Instance.deathExplosionMax = 60;
                Settings.Instance.AICasting = true;
                Settings.Instance.AIHardMode = true;
                Settings.Instance.AIMarking = false;
                Settings.Instance.AIFighterMarking = false;
                Settings.Instance.AIFriendlyMarking = false;
                Settings.Instance.baseMageChance = .8f;
                Settings.Instance.baseFighterChance = .8f;
                Settings.Instance.advMageChance = 1.5f;
                Settings.Instance.advFighterChance = 1.5f;
                Settings.Instance.magicyteChance = .003f;
                Settings.Instance.showIconsMultiSelect = true;
                Settings.Instance.showGizmo = true;
                Settings.Instance.showLevelUpMessage = false;
                Settings.Instance.changeUndeadPawnAppearance = true;
                Settings.Instance.changeUndeadAnimalAppearance = true;
                Settings.Instance.riftChallenge = 3f;
                this.deathExplosionDmgMax = "60.0";
                this.deathExplosionDmgMin = "30.0";

                Settings.Instance.autocastEnabled = true;
                Settings.Instance.autocastMinThreshold = .8f;
                Settings.Instance.autocastCombatMinThreshold = .2f;
                Settings.Instance.autocastEvaluationFrequency = 300;
            }

            //easy = Widgets.ButtonText(rowRect20ShiftRight1, "Easy", true, false, true);
            if (easy)
            {
                Settings.Instance.xpMultiplier = 1.5f;
                Settings.Instance.needMultiplier = 2f;
                Settings.Instance.deathExplosionRadius = 1f;
                Settings.Instance.deathExplosionMin = 5;
                Settings.Instance.deathExplosionMax = 10;
                Settings.Instance.AICasting = false;
                Settings.Instance.AIHardMode = false;
                Settings.Instance.AIMarking = false;
                Settings.Instance.AIFighterMarking = false;
                Settings.Instance.AIFriendlyMarking = false;
                Settings.Instance.baseMageChance = 2f;
                Settings.Instance.baseFighterChance = 2f;
                Settings.Instance.advMageChance = 1f;
                Settings.Instance.advFighterChance = 1f;
                Settings.Instance.magicyteChance = .01f;
                Settings.Instance.showIconsMultiSelect = true;
                Settings.Instance.showGizmo = true;
                Settings.Instance.showLevelUpMessage = true;
                Settings.Instance.changeUndeadPawnAppearance = true;
                Settings.Instance.changeUndeadAnimalAppearance = true;
                Settings.Instance.riftChallenge = 1f;
                this.deathExplosionDmgMax = "5.0";
                this.deathExplosionDmgMin = "10.0";

                Settings.Instance.autocastEnabled = true;
                Settings.Instance.autocastMinThreshold = .6f;
                Settings.Instance.autocastCombatMinThreshold = .05f;
                Settings.Instance.autocastEvaluationFrequency = 180;
            }

            Widgets.EndScrollView();

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

        private string Challenge(float val)
        {
            string rarity = "";
            if (val == 0)
            {
                rarity = "None (never happens)";
            }
            else if (val == 1)
            {
                rarity = "Easy";
            }
            else if (val == 2)
            {
                rarity = "Normal";
            }
            else
            {
                rarity = "Hard";
            }

            return rarity;
        }
    }
}
