﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    public static class TM_MatPool
    {
        public static readonly Material blackLightning = MaterialPool.MatFrom("Other/ArcaneBolt", true);
        public static readonly Material redLightning = MaterialPool.MatFrom("Other/DemonBolt", true);
    }
}