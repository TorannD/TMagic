using System;
using Verse;


namespace TorannMagic.ModOptions
{
    public class Settings : Verse.ModSettings
    {
        public float xpMultiplier = 1f;
        public float needMultiplier = 1f;
        public float deathExplosionRadius = 3f;
        public int deathExplosionMin = 20;
        public int deathExplosionMax = 50;
        public bool AICasting = true;
        public bool AIHardMode = false;
        public bool AIMarking = true;
        public bool AIFighterMarking = false;
        public bool AIFriendlyMarking = false;
        public float baseMageChance = 1f;
        public float baseFighterChance = 1f;
        public float advMageChance = 0.5f;
        public float advFighterChance = 0.5f;
        public float magicyteChance = .005f;
        public bool showIconsMultiSelect = true;
        public float riftChallenge = 1f;
        public bool showGizmo = true;
        public bool showLevelUpMessage = true;
        public bool changeUndeadPawnAppearance = true;
        public bool changeUndeadAnimalAppearance = true;

        //autocast options
        public bool autocastEnabled = true;
        public float autocastMinThreshold = 0.7f;
        public float autocastCombatMinThreshold = 0.2f;
        public int autocastEvaluationFrequency = 180;

        //class options
        public bool Arcanist = true;
        public bool FireMage = true;
        public bool IceMage = true;
        public bool LitMage = true;
        public bool Druid = true;
        public bool Paladin = true;
        public bool Necromancer = true;
        public bool Bard = true;
        public bool Priest = true;
        public bool Demonkin = true;
        public bool Geomancer = true;
        public bool Summoner = true;
        public bool Technomancer = true;

        public bool Gladiator = true;
        public bool Bladedancer = true;
        public bool Sniper = true;
        public bool Ranger = true;
        public bool Faceless = true;
        public bool Psionic = true;        

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
            Scribe_Values.Look<int>(ref this.deathExplosionMin, "deathExplosionMin", 20, false);
            Scribe_Values.Look<int>(ref this.deathExplosionMax, "deathExplosionMax", 50, false);
            Scribe_Values.Look<bool>(ref this.AICasting, "AICasting", true, false);
            Scribe_Values.Look<bool>(ref this.AIHardMode, "AIHardMode", false, false);
            Scribe_Values.Look<bool>(ref this.AIMarking, "AIMarking", false, false);
            Scribe_Values.Look<bool>(ref this.AIFighterMarking, "AIFighterMarking", false, false);
            Scribe_Values.Look<bool>(ref this.AIFriendlyMarking, "AIFriendlyMarking", false, false);
            Scribe_Values.Look<float>(ref this.baseMageChance, "baseMageChance", 1f, false);
            Scribe_Values.Look<float>(ref this.baseFighterChance, "baseFighterChance", 1f, false);
            Scribe_Values.Look<float>(ref this.advMageChance, "advMageChance", 0.5f, false);
            Scribe_Values.Look<float>(ref this.advFighterChance, "advFighterChance", 0.5f, false);
            Scribe_Values.Look<float>(ref this.magicyteChance, "magicyteChance", 0.005f, false);
            Scribe_Values.Look<bool>(ref this.showIconsMultiSelect, "showIconsMultiSelect", true, false);
            Scribe_Values.Look<float>(ref this.riftChallenge, "riftChallenge", 1f, false);
            Scribe_Values.Look<bool>(ref this.showGizmo, "showGizmo", true, false);
            Scribe_Values.Look<bool>(ref this.showLevelUpMessage, "showLevelUpMessage", true, false);
            Scribe_Values.Look<bool>(ref this.changeUndeadPawnAppearance, "changeUndeadPawnAppearance", true, false);
            Scribe_Values.Look<bool>(ref this.changeUndeadAnimalAppearance, "changeUndeadAnimalAppearance", true, false);

            Scribe_Values.Look<bool>(ref this.autocastEnabled, "autocastEnabled", true, false);
            Scribe_Values.Look<float>(ref this.autocastMinThreshold, "autocastMinThreshold", 0.7f, false);
            Scribe_Values.Look<float>(ref this.autocastCombatMinThreshold, "autocastCombatMinThreshold", 0.2f, false);
            Scribe_Values.Look<int>(ref this.autocastEvaluationFrequency, "autocastEvaluationFrequency", 180, false);

            Scribe_Values.Look<bool>(ref this.Arcanist, "Arcanist", true, false);
            Scribe_Values.Look<bool>(ref this.FireMage, "FireMage", true, false);
            Scribe_Values.Look<bool>(ref this.IceMage, "IceMage", true, false);
            Scribe_Values.Look<bool>(ref this.LitMage, "LitMage", true, false);
            Scribe_Values.Look<bool>(ref this.Geomancer, "Geomancer", true, false);
            Scribe_Values.Look<bool>(ref this.Druid, "Druid", true, false);
            Scribe_Values.Look<bool>(ref this.Paladin, "Paladin", true, false);
            Scribe_Values.Look<bool>(ref this.Priest, "Priest", true, false);
            Scribe_Values.Look<bool>(ref this.Bard, "Bard", true, false);
            Scribe_Values.Look<bool>(ref this.Summoner, "Summoner", true, false);
            Scribe_Values.Look<bool>(ref this.Necromancer, "Necromancer", true, false);
            Scribe_Values.Look<bool>(ref this.Technomancer, "Technomancer", true, false);
            Scribe_Values.Look<bool>(ref this.Demonkin, "Demonkin", true, false);
            Scribe_Values.Look<bool>(ref this.Gladiator, "Gladiator", true, false);
            Scribe_Values.Look<bool>(ref this.Bladedancer, "Bladedancer", true, false);
            Scribe_Values.Look<bool>(ref this.Sniper, "Sniper", true, false);
            Scribe_Values.Look<bool>(ref this.Ranger, "Ranger", true, false);
            Scribe_Values.Look<bool>(ref this.Faceless, "Faceless", true, false);
            Scribe_Values.Look<bool>(ref this.Psionic, "Psionic", true, false);
        }
    }
}
