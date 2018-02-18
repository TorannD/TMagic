using System;
using Verse;


namespace TorannMagic.ModOptions
{
    public class Settings : Verse.ModSettings
    {
        public float xpMultiplier = 1f;
        public float needMultiplier = 1f;
        public float deathExplosionRadius = 3f;
        public bool AICasting = true;
        public bool AIHardMode = false;
        public bool AIMarking = true;
        public float baseMageChance = 1f;
        public float baseFighterChance = 1f;
        public float advMageChance = 0.5f;
        public float advFighterChance = 0.5f;

        public static Settings Instance;

        public Settings()
        {
            Settings.Instance = this;
        }

        public override void ExposeData()
        {
            Scribe_Values.Look<float>(ref this.xpMultiplier, "xpMultiplier", 1f, false);
            Scribe_Values.Look<float>(ref this.needMultiplier, "needMultiplier", 1f, false);
            Scribe_Values.Look<float>(ref this.deathExplosionRadius, "deathExplosionRadius", 3f, false);
            Scribe_Values.Look<bool>(ref this.AICasting, "AICasting", true, false);
            Scribe_Values.Look<bool>(ref this.AIHardMode, "AIHardMode", false, false);
            Scribe_Values.Look<bool>(ref this.AIMarking, "AIMarking", false, false);
            Scribe_Values.Look<float>(ref this.baseMageChance, "baseMageChance", 1f, false);
            Scribe_Values.Look<float>(ref this.baseFighterChance, "baseFighterChance", 1f, false);
            Scribe_Values.Look<float>(ref this.advMageChance, "advMageChance", 0.5f, false);
            Scribe_Values.Look<float>(ref this.advFighterChance, "advFighterChance", 0.5f, false);
        }
    }
}
