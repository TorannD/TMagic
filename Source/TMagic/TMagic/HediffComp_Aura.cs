using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Harmony;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    public class HediffComp_Aura : HediffComp
    {

        private bool initialized = false;

        private int nextApplyTick = 0;

        private HediffDef hediffDef = null;

        public override void CompExposeData()
        {
            base.CompExposeData();
        }

        public string labelCap
        {
            get
            {
                return base.Def.LabelCap;
            }
        }

        public string label
        {
            get
            {
                return base.Def.label;
            }
        }

        private void Initialize()
        {
            bool spawned = base.Pawn.Spawned;
            CompAbilityUserMagic comp = this.Pawn.GetComp<CompAbilityUserMagic>();
            if (spawned && comp != null && comp.IsMagicUser)
            {
                DetermineHediff();
            }
            else
            {
                this.Pawn.health.RemoveHediff(this.parent);
            }
        }        

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            bool flag = base.Pawn != null && base.Pawn.Map != null;
            if (flag)
            {
                if (!initialized)
                {
                    initialized = true;
                    this.Initialize();
                }

                if (Find.TickManager.TicksGame > this.nextApplyTick && this.hediffDef != null)
                {
                    Pawn pawn = TM_Calc.FindNearbyFactionPawn(this.Pawn, this.Pawn.Faction, 100);
                    if (pawn != null && pawn.health != null)
                    {
                        if (pawn.health.hediffSet.HasHediff(this.hediffDef, false) || pawn.Faction != this.Pawn.Faction || pawn.RaceProps.Animal)
                        {
                            this.nextApplyTick = Find.TickManager.TicksGame + Rand.Range(80, 150);
                        }
                        else
                        {
                            HealthUtility.AdjustSeverity(pawn, this.hediffDef, 1f);
                            this.nextApplyTick = Find.TickManager.TicksGame + Rand.Range(4800, 5600);
                            MoteMaker.ThrowSmoke(pawn.DrawPos, pawn.Map, 1f);
                            MoteMaker.ThrowLightningGlow(pawn.DrawPos, pawn.Map, .8f);
                            CompAbilityUserMagic comp = this.Pawn.GetComp<CompAbilityUserMagic>();
                            comp.MagicUserXP += Rand.Range(10, 15);
                        }
                    }
                }

                if(Find.TickManager.TicksGame % 1200 == 0)
                {
                    DetermineHediff();
                }
            }
        }     

        public void DetermineHediff()
        {
            MagicPower abilityPower = null;            
            CompAbilityUserMagic comp = this.Pawn.GetComp<CompAbilityUserMagic>();
            if (parent.def == TorannMagicDefOf.TM_Shadow_AuraHD && comp != null)
            {
                abilityPower = comp.MagicData.MagicPowersA.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shadow);
                this.hediffDef = TorannMagicDefOf.Shadow;
                if (abilityPower == null)
                {
                    abilityPower = comp.MagicData.MagicPowersA.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shadow_I);
                    this.hediffDef = TorannMagicDefOf.Shadow_I;
                    if (abilityPower == null)
                    {
                        abilityPower = comp.MagicData.MagicPowersA.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shadow_II);
                        this.hediffDef = TorannMagicDefOf.Shadow_II;
                        if (abilityPower == null)
                        {
                            this.hediffDef = TorannMagicDefOf.Shadow_III;
                            abilityPower = comp.MagicData.MagicPowersA.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Shadow_III);
                        }
                    }
                }
            }
            if (parent.def == TorannMagicDefOf.TM_RayOfHope_AuraHD && comp != null)
            {
                abilityPower = comp.MagicData.MagicPowersIF.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_RayofHope);
                this.hediffDef = TorannMagicDefOf.RayofHope;
                if (abilityPower == null)
                {
                    abilityPower = comp.MagicData.MagicPowersIF.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_RayofHope_I);
                    this.hediffDef = TorannMagicDefOf.RayofHope_I;
                    if (abilityPower == null)
                    {
                        abilityPower = comp.MagicData.MagicPowersIF.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_RayofHope_II);
                        this.hediffDef = TorannMagicDefOf.RayofHope_II;
                        if (abilityPower == null)
                        {
                            this.hediffDef = TorannMagicDefOf.RayofHope_III;
                            abilityPower = comp.MagicData.MagicPowersIF.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_RayofHope_III);
                        }
                    }
                }
            }
            if (parent.def == TorannMagicDefOf.TM_SoothingBreeze_AuraHD && comp != null)
            {
                abilityPower = comp.MagicData.MagicPowersHoF.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Soothe);
                this.hediffDef = TorannMagicDefOf.SoothingBreeze;
                if (abilityPower == null)
                {
                    abilityPower = comp.MagicData.MagicPowersHoF.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Soothe_I);
                    this.hediffDef = TorannMagicDefOf.SoothingBreeze_I;
                    if (abilityPower == null)
                    {
                        abilityPower = comp.MagicData.MagicPowersHoF.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Soothe_II);
                        this.hediffDef = TorannMagicDefOf.SoothingBreeze_II;
                        if (abilityPower == null)
                        {
                            this.hediffDef = TorannMagicDefOf.SoothingBreeze_III;
                            abilityPower = comp.MagicData.MagicPowersHoF.FirstOrDefault((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Soothe_III);
                        }
                    }
                }
            }
            if (abilityPower != null)
            {
                this.parent.Severity = .5f + abilityPower.level;
            }
            else
            {
                this.Pawn.health.RemoveHediff(this.parent);
            }
        }
    }
}
