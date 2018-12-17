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
        public bool showGizmo = Settings.Instance.showGizmo;
        public bool showLevelUpMessage = Settings.Instance.showLevelUpMessage;
        public bool changeUndeadPawnAppearance = Settings.Instance.changeUndeadPawnAppearance;
        public bool changeUndeadAnimalAppearance = Settings.Instance.changeUndeadAnimalAppearance;
        public bool unrestrictedBloodTypes = Settings.Instance.unrestrictedBloodTypes;

        //autocast
        public bool autocastEnabled = Settings.Instance.autocastEnabled;
        public float autocastMinThreshold = Settings.Instance.autocastMinThreshold;
        public float autocastCombatMinThreshold = Settings.Instance.autocastCombatMinThreshold;
        public float autocastEvaluationFrequency = Settings.Instance.autocastEvaluationFrequency;

        //Class options
        public bool Arcanist = Settings.Instance.Arcanist;
        public bool FireMage = Settings.Instance.FireMage;
        public bool IceMage = Settings.Instance.IceMage;
        public bool LitMage = Settings.Instance.LitMage;
        public bool Druid = Settings.Instance.Druid;
        public bool Paladin = Settings.Instance.Paladin;
        public bool Necromancer = Settings.Instance.Necromancer;
        public bool Bard = Settings.Instance.Bard;
        public bool Priest = Settings.Instance.Priest;
        public bool Demonkin = Settings.Instance.Demonkin;
        public bool Geomancer = Settings.Instance.Geomancer;
        public bool Summoner = Settings.Instance.Summoner;
        public bool Technomancer = Settings.Instance.Technomancer;
        public bool BloodMage = Settings.Instance.BloodMage;
        public bool Enchanter = Settings.Instance.Enchanter;

        public bool Gladiator = Settings.Instance.Gladiator;
        public bool Bladedancer = Settings.Instance.Bladedancer;
        public bool Sniper = Settings.Instance.Sniper;
        public bool Ranger = Settings.Instance.Ranger;
        public bool Faceless = Settings.Instance.Faceless;
        public bool Psionic = Settings.Instance.Psionic;
        public bool DeathKnight = Settings.Instance.DeathKnight;

    }
}
