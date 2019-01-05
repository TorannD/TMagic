using System;
using System.Reflection;
using Verse;
using AlienRace;

namespace TorannMagic.ModCheck
{
    public static class AlienHumanoidRaces
    {
        public static bool TryGetBackstory_DisallowedTrait(ThingDef thingDef, string traitString)
        {
            bool traitIsAllowed = true;
            if (AlienHumanoidRaces.IsInitialized())
            {
                ThingDef_AlienRace alienDef = thingDef as ThingDef_AlienRace;
                if (alienDef != null && alienDef.alienRace != null)
                {
                    if (alienDef.alienRace.generalSettings.disallowedTraits.Contains(traitString))
                    {
                        traitIsAllowed = false;
                    }                    
                }
            }
            return traitIsAllowed;
        }

        public static bool IsInitialized()
        {
            bool initialized = false;
            foreach (ModContentPack p in LoadedModManager.RunningMods)
            {
                if (p.Name == "Humanoid Alien Races 2.0")
                {
                    initialized = true;
                }
            }
            return initialized;
        }
    }
}
