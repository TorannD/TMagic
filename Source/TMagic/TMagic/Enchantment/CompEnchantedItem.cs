using System;
using Verse;
using RimWorld;
using AbilityUser;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TorannMagic.Enchantment
{
    public class CompEnchantedItem : ThingComp
    {
        public List<MagicAbility> MagicAbilities = new List<MagicAbility>();

        public CompAbilityUserMagic CompAbilityUserMagicTarget = null;

        private Graphic Overlay;

        public CompProperties_EnchantedItem Props
        {
            get
            {
                return (CompProperties_EnchantedItem)this.props;
            }
        }

        public void GetOverlayGraphic()
        {
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            bool flag = this.parent.def.tickerType == TickerType.Never;
            if (flag)
            {
                this.parent.def.tickerType = TickerType.Rare;
            }
            base.PostSpawnSetup(respawningAfterLoad);
            //this.GetOverlayGraphic();
            Find.TickManager.RegisterAllTickabilityFor(this.parent);
        }

        //public override void PostDrawExtraSelectionOverlays()
        //{
        //    bool flag = this.Overlay == null;
        //    if (flag)
        //    {
        //        Log.Message("NoOverlay");
        //    }
        //    bool flag2 = this.Overlay != null;
        //    if (flag2)
        //    {
        //        Vector3 drawPos = this.parent.DrawPos;
        //        drawPos.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
        //        Vector3 s = new Vector3(2f, 2f, 2f);
        //        Matrix4x4 matrix = default(Matrix4x4);
        //        matrix.SetTRS(drawPos, Quaternion.AngleAxis(0f, Vector3.up), s);
        //        Graphics.DrawMesh(MeshPool.plane10, matrix, this.Overlay.MatSingle, 0);
        //    }
        //}

        public override void PostExposeData()
        {
            base.PostExposeData();
            this.Props.ExposeData();
        }

        public override string GetDescriptionPart()
        {
            string text = string.Empty;
            bool flag = this.Props.MagicAbilities.Count == 1;
            if (flag)
            {
                text += "Item Ability:";
            }
            else
            {
                bool flag2 = this.Props.MagicAbilities.Count > 1;
                if (flag2)
                {
                    text += "Item Abilities:";
                }
            }
            foreach (TMAbilityDef current in this.Props.MagicAbilities)
            {
                text += "\n\n";
                text = text + current.label.CapitalizeFirst() + " - ";
                text += current.GetDescription();
            }
            return text;
        }
    }
}
