using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TorannMagic.ModOptions
{
    public abstract class Constants
    {
        private static bool pawnInFlight = false;

        public static bool GetPawnInFlight()
        {
            return pawnInFlight;
        }

        public static bool SetPawnInFlight(bool inFlight)
        {
            pawnInFlight = inFlight;
            return true;
        }
    }
}
