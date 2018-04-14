using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace TorannMagic.ModOptions
{
    [StaticConstructorOnStartup]
    public class ModCompatibilityCheck
    {
        public static bool PrisonLaborIsActive
        {
            get
            {
                return ModsConfig.ActiveModsInLoadOrder.Any(m => m.Name == "Prison Labor");
            }
        }
    }
}
