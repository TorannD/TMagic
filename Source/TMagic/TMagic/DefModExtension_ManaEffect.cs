using System;
using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TorannMagic
{
    public class DefModExtension_ManaEffect : DefModExtension
    {
        public float maxMP = 0;
        public float mpRegenRate = 0;
        public float coolDown = 0;
        public float mpCost = 0;
        public float arcaneRes = 0;
        public float arcaneDmg = 0;

        public void applyManaEffects(
            ref float maxMP,
            ref float mpRegenRate,
            ref float coolDown,
            ref float mpCost,
            ref float arcaneRes,
            ref float arcaneDmg)
        {
            maxMP += this.maxMP;
            mpRegenRate += this.mpRegenRate;
            coolDown += this.coolDown;
            mpCost += this.mpCost;
            arcaneRes += this.arcaneRes;
            arcaneDmg += this.arcaneDmg;
        }
    }
}
