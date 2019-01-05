using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;
using RimWorld;
using GiddyUpCore;
using GiddyUpCore.Storage;

namespace TorannMagic.ModCheck
{
    public static class GiddyUp
    {
        public static void ForceDismount(Pawn pawn)
        {
            ExtendedPawnData epd = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(pawn);
            if (epd != null && epd.mount != null)
            {
                epd.mount.jobs.EndCurrentJob(Verse.AI.JobCondition.InterruptForced, true);
            }
        }
    }
}
