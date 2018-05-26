using Verse;
using AbilityUser;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RimWorld;
using Verse.AI;
using Verse.AI.Group;


namespace TorannMagic
{
    public class Projectile_Possess : Projectile_AbilityBase
    {
        private bool initialized = false;
        private int age = 0;
        private int duration = 1200;
        private int inventoryCount = 0;
        private IntVec3 oldPosition;
        private bool possessedFlag = false;
        Faction pFaction = null;
        Pawn hitPawn = null;
        Pawn caster = null;

        private int verVal;
        private int pwrVal;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.initialized, "initialized", false, false);
            Scribe_Values.Look<int>(ref this.age, "age", 0, false);
            Scribe_Values.Look<int>(ref this.duration, "duration", 1200, false);
            Scribe_Values.Look<int>(ref this.inventoryCount, "inventoryCount", 0, false);
            Scribe_Values.Look<Faction>(ref this.pFaction, "pFaction", null, false);
            Scribe_Values.Look<IntVec3>(ref this.oldPosition, "oldPosition", default(IntVec3), false);
            Scribe_References.Look<Pawn>(ref this.hitPawn, "hitPawn", false);
        }

        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            base.Impact(hitThing);
            ThingDef def = this.def;
                        
            if (!initialized)
            {
                caster = this.launcher as Pawn;
                hitPawn = hitThing as Pawn;
                this.oldPosition = caster.Position;
                MightPowerSkill pwr = caster.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Possess.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Possess_pwr");
                MightPowerSkill ver = caster.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Possess.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Possess_ver");
                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                pwrVal = pwr.level;
                verVal = ver.level;
                if (settingsRef.AIHardMode && !caster.IsColonist)
                {
                    pwrVal = 3;
                    verVal = 3;
                }
                this.duration += verVal * 300;
                if (hitThing != null && hitThing is Pawn && hitPawn.RaceProps.Humanlike)
                {
                    possessedFlag = (hitPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_CoOpPossessionHD) || hitPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_CoOpPossessionHD_I) || hitPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_CoOpPossessionHD_II) || hitPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_CoOpPossessionHD_III) ||
                        hitPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD) || hitPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD_I) || hitPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD_II) || hitPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD_III));
                    if (!hitPawn.Downed && !hitPawn.Dead && !possessedFlag)
                    {
                        this.pFaction = hitPawn.Faction;
                        if (this.pFaction != caster.Faction)
                        {
                            //possess enemy or neutral
                            int weaponCount = 0;
                            if (hitPawn.equipment.PrimaryEq != null)
                            {
                                weaponCount = 1;
                            }
                            this.inventoryCount = hitPawn.inventory.innerContainer.Count + hitPawn.apparel.WornApparelCount + weaponCount;
                            hitPawn.SetFaction(caster.Faction, null);
                            HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_DisguiseHD_II, 20f + 5f * verVal);
                            switch (pwrVal)
                            {
                                case 0:
                                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_PossessionHD, 20f + 5f * verVal);
                                    break;
                                case 1:
                                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_PossessionHD_I, 20f + 5f * verVal);
                                    break;
                                case 2:
                                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_PossessionHD_II, 20f + 5f * verVal);
                                    break;
                                case 3:
                                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_PossessionHD_III, 20f + 5f * verVal);
                                    break;
                            }
                            initialized = true;
                            MoteMaker.ThrowSmoke(caster.DrawPos, caster.Map, 1f);
                            MoteMaker.ThrowSmoke(caster.DrawPos, caster.Map, 1.2f);
                            MoteMaker.ThrowHeatGlow(caster.Position, caster.Map, .8f);
                            caster.DeSpawn();
                            if(!caster.IsColonist)
                            {
                                Lord lord = caster.GetLord();
                                lord.AddPawn(hitPawn);
                                LordJob_AssaultColony lordJob = new LordJob_AssaultColony(caster.Faction, false, false, false, true, false);
                            }
                        }
                        else
                        {
                            //possess friendly
                            HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_DisguiseHD_II, 20f + 5f * verVal);
                            switch (pwrVal)
                            {
                                case 0:
                                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_CoOpPossessionHD, 20f + 5f * verVal);
                                    break;
                                case 1:
                                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_CoOpPossessionHD_I, 20f + 5f * verVal);
                                    break;
                                case 2:
                                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_CoOpPossessionHD_II, 20f + 5f * verVal);
                                    break;
                                case 3:
                                    HealthUtility.AdjustSeverity(hitPawn, TorannMagicDefOf.TM_CoOpPossessionHD_III, 20f + 5f * verVal);
                                    break;
                            }
                            initialized = true;
                            MoteMaker.ThrowSmoke(caster.DrawPos, caster.Map, 1f);
                            MoteMaker.ThrowSmoke(caster.DrawPos, caster.Map, 1.2f);
                            MoteMaker.ThrowHeatGlow(caster.Position, caster.Map, .8f);
                            caster.DeSpawn();
                        }
                    }
                    else
                    {
                        Messages.Message("TM_CannotPossessNow".Translate(new object[]
                            {
                                caster.LabelShort,
                                hitPawn.LabelShort
                            }), MessageTypeDefOf.RejectInput);
                        this.age = this.duration;
                        this.Destroy(DestroyMode.Vanish);
                    }
                }
                else
                {
                    Messages.Message("TM_CannotPossess".Translate(new object[]
                            {
                                caster.LabelShort,
                                hitThing.LabelShort
                            }), MessageTypeDefOf.RejectInput);
                    this.age = this.duration;                    
                    this.Destroy(DestroyMode.Vanish);
                }
            }

            if(hitPawn.Downed || hitPawn.Dead)
            {
                this.age = this.duration;
            }
        }

        public override void Tick()
        {
            base.Tick();
            this.age++;
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            bool flag = this.age < duration;
            if (!flag)
            {
                if (hitPawn.RaceProps.Humanlike && !this.possessedFlag)
                {
                    if ((hitPawn.Downed || hitPawn.Dead) && !pFaction.HostileTo(caster.Faction) && pFaction != this.caster.Faction)
                    {
                        this.pFaction.SetHostileTo(this.caster.Faction, true);
                    }
                    bool flag2 = caster.Spawned;
                    if (!flag2)
                    {
                        GenSpawn.Spawn(caster, this.oldPosition, this.Map, Rot4.North, true);
                    }
                    bool flag3 = hitPawn.Faction != pFaction;
                    if (flag3)
                    {
                        hitPawn.SetFaction(pFaction, null);
                    }
                    int weaponCount = 0;
                    if (hitPawn.equipment.PrimaryEq != null)
                    {
                        weaponCount = 1;
                    }
                    int tempInvCount = hitPawn.inventory.innerContainer.Count + hitPawn.apparel.WornApparelCount + weaponCount;
                    if (tempInvCount < this.inventoryCount && !pFaction.HostileTo(caster.Faction) && pFaction != this.caster.Faction)
                    {
                        pFaction.SetHostileTo(this.caster.Faction, true);
                        Find.LetterStack.ReceiveLetter("LetterLabelPossessedCaughtStealing".Translate(), "TM_PossessedCaughtStealing".Translate(new object[]
                            {
                                hitPawn.Faction,
                                hitPawn.LabelShort
                            }), LetterDefOf.NegativeEvent, null);
                    }
                    if(hitPawn.IsColonist)
                    {
                        hitPawn.jobs.EndCurrentJob(JobCondition.InterruptForced, false);                        
                    }
                    RemoveHediffs();                    
                }
                base.Destroy(mode);
            }
        }

        public void RemoveHediffs()
        {
            Hediff disguiseHD = null;
            Hediff possessHD = null;
            Hediff possessCHD = null;
            using (IEnumerator<Hediff> enumerator = hitPawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Hediff rec = enumerator.Current;
                    if (rec.def == TorannMagicDefOf.TM_DisguiseHD_II)
                    {
                        disguiseHD = rec;
                    }
                    if (rec.def == TorannMagicDefOf.TM_CoOpPossessionHD || rec.def == TorannMagicDefOf.TM_CoOpPossessionHD_I || rec.def == TorannMagicDefOf.TM_CoOpPossessionHD_II || rec.def == TorannMagicDefOf.TM_CoOpPossessionHD_III)
                    {
                        possessCHD = rec;
                    }
                    if (rec.def == TorannMagicDefOf.TM_PossessionHD || rec.def == TorannMagicDefOf.TM_PossessionHD_I || rec.def == TorannMagicDefOf.TM_PossessionHD_II || rec.def == TorannMagicDefOf.TM_PossessionHD_III)
                    {
                        possessHD = rec;
                    }
                }
            }
            if(disguiseHD != null)
            {
                this.hitPawn.health.RemoveHediff(disguiseHD);
            }
            if(possessHD != null)
            {
                this.hitPawn.health.RemoveHediff(possessHD);
            }
            if (possessCHD != null)
            {
                this.hitPawn.health.RemoveHediff(possessCHD);
            }
            
        }
    }
}
