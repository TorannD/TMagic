using RimWorld.Planet;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace TorannMagic
{
    class IncidentWorker_QuestArcaneStash: IncidentWorker
    {

        private static readonly FloatRange TotalMarketValueRange = new FloatRange(2000f, 3000f);

        private List<SitePartDef> possibleSitePartsInt = new List<SitePartDef>();

        private List<SitePartDef> PossibleSiteParts
        {
            get
            {
                this.possibleSitePartsInt.Clear();
                this.possibleSitePartsInt.Add(SitePartDefOf.Manhunters);
                this.possibleSitePartsInt.Add(SitePartDefOf.Outpost);
                this.possibleSitePartsInt.Add(SitePartDefOf.Turrets);
                this.possibleSitePartsInt.Add(null);
                return this.possibleSitePartsInt;
            }
        }

        protected override bool CanFireNowSub(IIncidentTarget target)
        {
            
            int num;
            Faction faction;
            return base.CanFireNowSub(target) && (Find.FactionManager.RandomAlliedFaction(false, false, false, TechLevel.Undefined) != null && TileFinder.TryFindNewSiteTile(out num, 8, 30, false, true, -1)) && SiteMakerHelper.TryFindRandomFactionFor(SiteCoreDefOf.ItemStash, null, out faction, true, null);
        }

        private bool CanFindMage(Map map, out Pawn pawn)
        {
            pawn = null;

            
            bool result = false;
            //foreach (Pawn current in map.mapPawns.FreeColonists)
            //{
                
            //    if (current.story.traits.HasTrait(TorannMagicDefOf.Arcanist) || current.story.traits.HasTrait(TorannMagicDefOf.InnerFire) || current.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost) || current.story.traits.HasTrait(TorannMagicDefOf.StormBorn) || current.story.traits.HasTrait(TorannMagicDefOf.Paladin))
            //    {
            //        result = true;
            //        pawn = current;
            //    }
            //}
            return result;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;
            Pawn pawn;
            bool result;
            if (!this.CanFindMage(map, out pawn))
            {
                return false;
            }
            else
            {
                Site site = this.MakeSite();
                if (site == null)
                {
                    result = false;
                }
                else
                {
                    base.SendStandardLetter(site, new string[]
                    {
                    pawn.NameStringShort
                    });
                    result = true;
                }
            }
            return result;
            
            
        }

        private Site MakeSite()
        {
            Site result;
            int tile;
            if (!TileFinder.TryFindNewSiteTile(out tile))
            {
                result = null;
            }
            else
            {
                Site site = (Site)WorldObjectMaker.MakeWorldObject(TorannMagicDefOf.ArcaneAdventure);
                site.Tile = tile;
                site.SetFaction(Faction.OfMechanoids);
                site.core = TorannMagicDefOf.ArcaneStash;
                site.parts.Add(TorannMagicDefOf.ArcaneDefenders);
                site.parts.Add(TorannMagicDefOf.ArcaneStashTreasure);
                site.parts.Add(TorannMagicDefOf.EnemyRaidOnArrival);
                site.parts.Add(SitePartDefOf.Outpost);
                site.parts.Add(SitePartDefOf.Turrets);
                System.Random random = new System.Random();
                int rnd = GenMath.RoundRandom(random.Next(0, 10));
                if (rnd < 5)
                {
                    site.parts.Add(TorannMagicDefOf.ArcaneDefenders);
                    
                }
                rnd = GenMath.RoundRandom(random.Next(0, 10));
                if (Rand.Value < 5)
                {
                    site.parts.Add(TorannMagicDefOf.EnemyRaidOnArrival);
                    
                }
                rnd = GenMath.RoundRandom(random.Next(0, 10));
                if (Rand.Value < 2)
                {
                    site.parts.Add(TorannMagicDefOf.ArcaneDefenders);
                    
                }
                Find.WorldObjects.Add(site);
                result = site;
            }
            return result;
            
        }
        

    }
}
