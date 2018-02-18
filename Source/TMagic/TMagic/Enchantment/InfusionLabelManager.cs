using System;
using System.Collections.Generic;

namespace TorannMagic.Enchantment
{
    public static class InfusionLabelManager
    {
        public static List<CompInfusion> Drawee
        {
            get;
            set;
        }

        static InfusionLabelManager()
        {
            InfusionLabelManager.Drawee = new List<CompInfusion>();
        }

        public static void ReInit()
        {
            InfusionLabelManager.Drawee.Clear();
        }

        public static void Register(CompInfusion compInfusion)
        {
            InfusionLabelManager.Drawee.Add(compInfusion);
        }

        public static void DeRegister(CompInfusion compInfusion)
        {
            InfusionLabelManager.Drawee.Remove(compInfusion);
        }
    }
}
