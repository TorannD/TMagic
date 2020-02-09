using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;

namespace TorannMagic.ModOptions
{
    public class ClassOptionsWindow : Window
    {

        public override Vector2 InitialSize => new Vector2(520f, 640f);
        public static float HeaderSize = 28f;
        public static float TextSize = 22f;

        public Vector2 scrollPosition = Vector2.zero;

        public ClassOptionsWindow()
        {
            base.closeOnCancel = true;
            base.doCloseButton = true;
            base.doCloseX = true;
            base.absorbInputAroundWindow = true;
            base.forcePause = true;            
        }

        public override void DoWindowContents(Rect inRect)
        {
            int num = 0;
            float rowHeight = 28f;
            Rect sRect = new Rect(inRect.x, inRect.y, inRect.width - 36f, inRect.height + 360f);
            scrollPosition = GUI.BeginScrollView(inRect, scrollPosition, sRect, false, true);
            //GUI.BeginGroup(inRect);
            
            Text.Font = GameFont.Medium;
            float x = Text.CalcSize("TM_ClassOptions".Translate()).x;
            Rect headerRect = new Rect(inRect.width / 2f - (x / 2), inRect.y, inRect.width, ClassOptionsWindow.HeaderSize);
            Widgets.Label(headerRect, "TM_ClassOptions".Translate());
            Text.Font = GameFont.Small;
            GUI.color = Color.yellow;
            x = Text.CalcSize("TM_ClassWarning".Translate()).x;
            Rect warningRect = new Rect(inRect.width / 2f - (x / 2), inRect.y + ClassOptionsWindow.HeaderSize + 4f, inRect.width, ClassOptionsWindow.TextSize);
            Widgets.Label(warningRect, "TM_ClassWarning".Translate());
            x = Text.CalcSize("TM_RequiresRestart".Translate()).x;
            Rect restartRect = new Rect(inRect.width / 2f - (x / 2), inRect.y + ClassOptionsWindow.HeaderSize + ClassOptionsWindow.TextSize + 4f, inRect.width, ClassOptionsWindow.TextSize);
            Widgets.Label(restartRect, "TM_RequiresRestart".Translate());
            GUI.color = Color.white;
            Rect rect1 = new Rect(inRect);
            rect1.width /= 3f;
            num+=3;
            GUI.color = Color.magenta;
            Rect classRect = Controller.UIHelper.GetRowRect(rect1, rowHeight, num);
            Widgets.Label(classRect, "TM_EnabledMages".Translate());
            Rect classRectShiftRight = Controller.UIHelper.GetRowRect(classRect, rowHeight, num);
            classRectShiftRight.x += classRect.width + 140f;
            GUI.color = Color.green;
            Widgets.Label(classRectShiftRight, "TM_EnabledFighters".Translate());
            num++;
            GUI.color = Color.white;
            Rect rowRect0 = Controller.UIHelper.GetRowRect(classRect, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect0, "TM_Wanderer".Translate(), ref Settings.Instance.Wanderer, false);
            Rect rowRect0ShiftRight = Controller.UIHelper.GetRowRect(rowRect0, rowHeight, num);
            rowRect0ShiftRight.x += rowRect0.width + 140f;
            Widgets.CheckboxLabeled(rowRect0ShiftRight, "TM_Wayfarer".Translate(), ref Settings.Instance.Wayfayer, false);
            num++;
            Rect rowRect = Controller.UIHelper.GetRowRect(rowRect0, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect, "TM_Arcanist".Translate(), ref Settings.Instance.Arcanist, false);
            Rect rowRectShiftRight = Controller.UIHelper.GetRowRect(rowRect, rowHeight, num);
            rowRectShiftRight.x += rowRect.width + 140f;
            Widgets.CheckboxLabeled(rowRectShiftRight, "TM_Gladiator".Translate(), ref Settings.Instance.Gladiator, false);
            num++;
            Rect rowRect1 = Controller.UIHelper.GetRowRect(rowRect, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect1, "TM_FireMage".Translate(), ref Settings.Instance.FireMage, false);
            Rect rowRect1ShiftRight = Controller.UIHelper.GetRowRect(rowRect1, rowHeight, num);
            rowRect1ShiftRight.x += rowRect.width + 140f;
            Widgets.CheckboxLabeled(rowRect1ShiftRight, "TM_Bladedancer".Translate(), ref Settings.Instance.Bladedancer, false);
            num++;
            Rect rowRect2 = Controller.UIHelper.GetRowRect(rowRect1, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect2, "TM_IceMage".Translate(), ref Settings.Instance.IceMage, false);
            Rect rowRect2ShiftRight = Controller.UIHelper.GetRowRect(rowRect2, rowHeight, num);
            rowRect2ShiftRight.x += rowRect1.width + 140f;
            Widgets.CheckboxLabeled(rowRect2ShiftRight, "TM_Sniper".Translate(), ref Settings.Instance.Sniper, false);
            num++;
            Rect rowRect3 = Controller.UIHelper.GetRowRect(rowRect2, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect3, "TM_LitMage".Translate(), ref Settings.Instance.LitMage, false);
            Rect rowRect3ShiftRight = Controller.UIHelper.GetRowRect(rowRect3, rowHeight, num);
            rowRect3ShiftRight.x += rowRect.width + 140f;
            Widgets.CheckboxLabeled(rowRect3ShiftRight, "TM_Ranger".Translate(), ref Settings.Instance.Ranger, false);
            num++;
            Rect rowRect4 = Controller.UIHelper.GetRowRect(rowRect3, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect4, "TM_Geomancer".Translate(), ref Settings.Instance.Geomancer, false);
            Rect rowRect4ShiftRight = Controller.UIHelper.GetRowRect(rowRect4, rowHeight, num);
            rowRect4ShiftRight.x += rowRect.width + 140f;
            Widgets.CheckboxLabeled(rowRect4ShiftRight, "TM_Faceless".Translate(), ref Settings.Instance.Faceless, false);
            num++;
            Rect rowRect5 = Controller.UIHelper.GetRowRect(rowRect4, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect5, "TM_Druid".Translate(), ref Settings.Instance.Druid, false);
            Rect rowRect5ShiftRight = Controller.UIHelper.GetRowRect(rowRect5, rowHeight, num);
            rowRect5ShiftRight.x += rowRect.width + 140f;
            Widgets.CheckboxLabeled(rowRect5ShiftRight, "TM_Psionic".Translate(), ref Settings.Instance.Psionic, false);
            num++;
            Rect rowRect6 = Controller.UIHelper.GetRowRect(rowRect5, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect6, "TM_Paladin".Translate(), ref Settings.Instance.Paladin, false);
            Rect rowRect6ShiftRight = Controller.UIHelper.GetRowRect(rowRect6, rowHeight, num);
            rowRect6ShiftRight.x += rowRect.width + 140f;
            Widgets.CheckboxLabeled(rowRect6ShiftRight, "TM_DeathKnight".Translate(), ref Settings.Instance.DeathKnight, false);
            num++;
            Rect rowRect7 = Controller.UIHelper.GetRowRect(rowRect6, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect7, "TM_Priest".Translate(), ref Settings.Instance.Priest, false);
            Rect rowRect7ShiftRight = Controller.UIHelper.GetRowRect(rowRect7, rowHeight, num);
            rowRect7ShiftRight.x += rowRect.width + 140f;
            Widgets.CheckboxLabeled(rowRect7ShiftRight, "TM_Monk".Translate(), ref Settings.Instance.Monk, false);
            num++;
            Rect rowRect8 = Controller.UIHelper.GetRowRect(rowRect7, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect8, "TM_Bard".Translate(), ref Settings.Instance.Bard, false);
            num++;
            Rect rowRect9 = Controller.UIHelper.GetRowRect(rowRect8, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect9, "TM_Summoner".Translate(), ref Settings.Instance.Summoner, false);
            num++;
            Rect rowRect10 = Controller.UIHelper.GetRowRect(rowRect9, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect10, "TM_Necromancer".Translate(), ref Settings.Instance.Necromancer, false);
            num++;
            Rect rowRect11 = Controller.UIHelper.GetRowRect(rowRect10, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect11, "TM_Demonkin".Translate(), ref Settings.Instance.Demonkin, false);
            num++;
            Rect rowRect12 = Controller.UIHelper.GetRowRect(rowRect11, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect12, "TM_Technomancer".Translate(), ref Settings.Instance.Technomancer, false);
            num++;
            Rect rowRect13 = Controller.UIHelper.GetRowRect(rowRect12, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect13, "TM_BloodMage".Translate(), ref Settings.Instance.BloodMage, false);
            num++;
            Rect rowRect14 = Controller.UIHelper.GetRowRect(rowRect13, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect14, "TM_Enchanter".Translate(), ref Settings.Instance.Enchanter, false);
            num++;
            Rect rowRect15 = Controller.UIHelper.GetRowRect(rowRect14, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect15, "TM_Chronomancer".Translate(), ref Settings.Instance.Chronomancer, false);
            num++;
            Rect rowRect16 = Controller.UIHelper.GetRowRect(rowRect15, rowHeight, num);
            Widgets.CheckboxLabeled(rowRect16, "TM_ChaosMage".Translate(), ref Settings.Instance.ChaosMage, false);

            //GUI.EndGroup();
            GUI.EndScrollView();
        }        
    }   
}
