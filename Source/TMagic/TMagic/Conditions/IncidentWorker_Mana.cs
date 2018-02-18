using RimWorld;
using Verse;

namespace TorannMagic
{
    public class IncidentWorker_Mana : IncidentWorker_MakeGameCondition
    {        

        protected override bool CanFireNowSub(IIncidentTarget target)
        {

            if (!base.CanFireNowSub(target))
            {
                return false;
            }
            Map map = (Map)target;
            return true;
        }
    }
}
