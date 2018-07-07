using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace TorannMagic
{
    public class GameCondition_ManaDrain : GameCondition
    {
        IEnumerable<Pawn> victims;

        public override void Init()
        {
            Map map = base.SingleMap;
            
            if (map != null)
            {
                victims = map.mapPawns.FreeColonistsAndPrisoners;
            }
            int num = victims.Count<Pawn>();
            Pawn pawn;
            for (int i = 0; i < num; i++)
            {
                pawn = victims.ToArray<Pawn>()[i];
                if (pawn != null)
                {
                    CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
                    if (comp.IsMagicUser)
                    {
                        if ( comp.Mana.CurLevel == 1)
                        {
                            comp.Mana.CurLevel -= .01f;
                        }
                    }
                }
                victims.GetEnumerator().MoveNext();
            }
        }

        public GameCondition_ManaDrain()
        {

        }

    }
}
