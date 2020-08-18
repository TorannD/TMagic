using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TorannMagic.TMDefs
{
    public class TM_Autocast
    {
        public AutocastType type = AutocastType.Null;

        public bool mightUser = false;
        public bool magicUser = false;
        public bool drafted = false;
        public bool undrafted = false;
        public float minRange = 0;
    }

    public enum AutocastType
    {
        CastOnSelf,
        CastOnEnemy,
        Null
    }
}
