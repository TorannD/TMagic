using RimWorld;
using System;
using Verse;

namespace TorannMagic
{
    class ThoughtWorker_MinionAlways : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            bool flag = p.kindDef.defName == "TM_Minion";
            ThoughtState result;
            if (flag)
            {
                result = ThoughtState.ActiveAtStage(0);
            }
            else
            {
                result = ThoughtState.Inactive;
            }
            return result;
        }
    }
}
