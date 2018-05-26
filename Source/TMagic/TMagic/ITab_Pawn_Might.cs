using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace TorannMagic
{
    public class ITab_Pawn_Might : ITab  //code by Jecrell
    {
        private Pawn PawnToShowInfoAbout
        {
            get
            {
                Pawn pawn = null;
                bool flag = base.SelPawn != null;
                if (flag)
                {
                    pawn = base.SelPawn;
                }
                else
                {
                    Corpse corpse = base.SelThing as Corpse;
                    bool flag2 = corpse != null;
                    if (flag2)
                    {
                        pawn = corpse.InnerPawn;
                    }
                }
                bool flag3 = pawn == null;
                Pawn result;
                if (flag3)
                {
                    Log.Error("Character tab found no selected pawn to display.");
                    result = null;
                }
                else
                {
                    result = pawn;
                }
                return result;
            }
        }

        public override bool IsVisible
        {
            get
            {
                
                bool flag = base.SelPawn.story != null;
                if (base.SelPawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator))
                {
                    return flag && true;
                }
                if(base.SelPawn.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper))
                {
                    return flag && true;
                }
                if (base.SelPawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer))
                {
                    return flag && true;
                }
                if (base.SelPawn.story.traits.HasTrait(TorannMagicDefOf.Ranger))
                {
                    return flag && true;
                }
                if (base.SelPawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                {
                    return flag && true;
                }

                return false;
                
            }
        }

        public ITab_Pawn_Might()
        {
            this.size = MightCardUtility.mightCardSize + new Vector2(17f, 17f) * 2f;
            this.labelKey = "TM_TabMight";
        }

        protected override void FillTab()
        {
            Rect rect = new Rect(17f, 17f, MightCardUtility.mightCardSize.x, MightCardUtility.mightCardSize.y);
            MightCardUtility.DrawMightCard(rect, this.PawnToShowInfoAbout);
        }

    }
}
