using UnityEngine;
using Verse;

namespace TorannMagic
{
    public static class TM_Calc
    {
        public static Vector3 GetVector(IntVec3 from, IntVec3 to)
        {
            Vector3 heading = (to - from).ToVector3();
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;
            return direction;
        }
    }
}
