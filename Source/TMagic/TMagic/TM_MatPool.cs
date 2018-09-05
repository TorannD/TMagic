using System;
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
        public static readonly Texture2D Icon_Undead = ContentFinder<Texture2D>.Get("UI/undead_icon", true);
        public static readonly Material PsionicBarrier = MaterialPool.MatFrom("Other/PsionicBarrier", ShaderDatabase.Transparent);
        public static readonly Material psiLightning = MaterialPool.MatFrom("Other/PsiBolt", ShaderDatabase.Transparent);

        public static readonly Material psiMote = MaterialPool.MatFrom("Motes/PsiMote", ShaderDatabase.MoteGlow);
        public static readonly Material singleForkLightning = MaterialPool.MatFrom("Spells/LightningBolt_back1", ShaderDatabase.MoteGlow);
        public static readonly Material doubleForkLightning = MaterialPool.MatFrom("Spells/LightningBolt", ShaderDatabase.MoteGlow);
        public static readonly Material multiForkLightning = MaterialPool.MatFrom("Spells/LightningBolt_w", ShaderDatabase.MoteGlow);
        public static readonly Material standardLightning = MatLoader.LoadMat("Weather/LightningBolt", -1);
    }
}
