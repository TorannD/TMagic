using System;
using Verse;

namespace TorannMagic.ModOptions
{
    public class SettingsRef
    {
        public float xpMultiplier = Settings.Instance.xpMultiplier;
        public float needMultiplier = Settings.Instance.needMultiplier;
        public float deathExplosionRadius = Settings.Instance.deathExplosionRadius;
        public bool AICasting = Settings.Instance.AICasting;
        public bool AIHardMode = Settings.Instance.AIHardMode;
        public bool AIMarking = Settings.Instance.AIMarking;
        public bool AIFighterMarking = Settings.Instance.AIFighterMarking;
        public bool AIFriendlyMarking = Settings.Instance.AIFriendlyMarking;
        public float baseMageChance = Settings.Instance.baseMageChance;
        public float baseFighterChance = Settings.Instance.baseFighterChance;
        public float advMageChance = Settings.Instance.advMageChance;
        public float advFighterChance = Settings.Instance.advFighterChance;
        public int deathExplosionMin = Settings.Instance.deathExplosionMin;
        public int deathExplosionMax = Settings.Instance.deathExplosionMax;
        public float magicyteChance = Settings.Instance.magicyteChance;
        public bool showIconsMultiSelect = Settings.Instance.showIconsMultiSelect;
        public float riftChallenge = Settings.Instance.riftChallenge;

    }
}
