using RimWorld;
using System;
using System.Text;
using Verse;
using UnityEngine;
using System.Collections.Generic;

namespace TorannMagic.Enchantment
{
    public class StatPart_InfusionModifier : StatPart
    {
        public StatPart_InfusionModifier(StatDef parentStat)
        {
            this.parentStat = parentStat;            
        }

        public override void TransformValue(StatRequest req, ref float val)
        {
            if (!req.HasThing)
            {
                return;
            }
            if (req.Thing.def.race != null && req.Thing.def.race.Humanlike)
            {
                this.TransformValueForPawn(req, ref val);
            }
            else if (req.Thing.def.HasComp(typeof(CompInfusion)))
            {
                this.TransformValueForThing(req, ref val);
            }
        }

        public void TransformValueForPawn(StatRequest req, ref float val)
        {
            Pawn pawn = req.Thing as Pawn;
            if (pawn == null)
            {
                return;
            }
            InfusionSet inf;
            if (pawn.equipment.Primary != null && pawn.equipment.Primary.TryGetInfusions(out inf))
            {
                this.TransformValue(inf, ref val);
            }
            foreach (Apparel current in pawn.apparel.WornApparel)
            {
                InfusionSet inf2;
                if (current.TryGetInfusions(out inf2))
                {
                    this.TransformValue(inf2, ref val);
                }
            }
        }

        private void TransformValueForThing(StatRequest req, ref float val)
        {
            InfusionSet inf;
            if (!req.Thing.TryGetInfusions(out inf))
            {
                return;
            }
            this.TransformValue(inf, ref val);
        }

        private void TransformValue(InfusionSet inf, ref float val)
        {
            EnchantmentDef enchantment = inf.enchantment;
            StatMod statMod;
            if (enchantment != null && enchantment.TryGetStatValue(this.parentStat, out statMod))
            {
                val += statMod.offset;
                val *= statMod.multiplier;
            }
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (!req.HasThing)
            {
                return null;
            }
            if (req.Thing.def.race != null && req.Thing.def.race.Humanlike)
            {
                return this.ExplanationPartForPawn(req);
            }
            if (req.Thing.def.HasComp(typeof(CompInfusion)))
            {
                return this.ExplanationPartForThing(req);
            }
            return null;
        }

        private string ExplanationPartForPawn(StatRequest req)
        {
            Pawn pawn = req.Thing as Pawn;
            if (pawn == null)
            {
                return null;
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(ResourceBank.StringInfusionDescBonus);
            InfusionSet infusions;
            if (pawn.equipment.Primary.TryGetInfusions(out infusions))
            {
                stringBuilder.Append(this.WriteExplanation(pawn.equipment.Primary, infusions));
            }
            foreach (Apparel current in pawn.apparel.WornApparel)
            {
                if (current.TryGetInfusions(out infusions))
                {
                    stringBuilder.Append(this.WriteExplanation(current, infusions));
                }
            }
            return stringBuilder.ToString();
        }

        private string ExplanationPartForThing(StatRequest req)
        {
            InfusionSet infusions;
            return (!req.Thing.TryGetInfusions(out infusions)) ? null : this.WriteExplanation(req.Thing, infusions);
        }

        private string WriteExplanation(Thing thing, InfusionSet infusions)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (infusions.enchantment != null)
            {
                stringBuilder.Append(this.WriteExplanationDetail(thing, infusions.enchantment.defName));
            }
            return stringBuilder.ToString();
        }

        private string WriteExplanationDetail(Thing infusedThing, string val)
        {
            EnchantmentDef def = EnchantmentDef.Named(val);
            StringBuilder stringBuilder = new StringBuilder();
            StatMod statMod;
            if (!def.TryGetStatValue(this.parentStat, out statMod))
            {
                return null;
            }
            if (statMod.offset.FloatEqual(0f) && statMod.multiplier.FloatEqual(1f))
            {
                return null;
            }
            if (statMod.offset.FloatNotEqual(0f))
            {
                stringBuilder.Append("    " + GenText.CapitalizeFirst(def.label) + ": ");
                stringBuilder.Append((statMod.offset <= 0f) ? "-" : "+");
                stringBuilder.AppendLine(this.parentStat.ValueToString(Mathf.Abs(statMod.offset), (ToStringNumberSense)1));
            }
            if (statMod.multiplier.FloatNotEqual(1f))
            {
                stringBuilder.Append("    " + GenText.CapitalizeFirst(def.label) + ": x");
                stringBuilder.AppendLine(GenText.ToStringPercent(statMod.multiplier));
            }
            return stringBuilder.ToString();
        }
    }
}
