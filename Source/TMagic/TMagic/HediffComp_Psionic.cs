using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;
using RimWorld;
using UnityEngine;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    class HediffComp_Psionic : HediffComp
    {

        private bool initialized = false;
        private int pwrVal = 0;
        private int effVal = 0;
        private int verVal = 0;

        private bool doPsionicAttack = false;
        private int ticksTillPsionicStrike = 0;
        private int nextPsionicAttack = 0;
        Pawn threat;

        public int PwrVal
        {
            get
            {
                return this.pwrVal;
            }
            set
            {
                this.pwrVal = value;
            }
        }

        public int EffVal
        {
            get
            {
                return this.effVal;
            }
            set
            {
                this.effVal = value;
            }
        }

        public int VerVal
        {
            get
            {
                return this.verVal;
            }
            set
            {
                this.verVal = value;
            }
        }

        public string LabelCap
        {
            get
            {
                return base.Def.LabelCap;
            }
        }

        public string Label
        {
            get
            {
                return base.Def.label;
            }
        }

        private void Initialize()
        {
            bool spawned = base.Pawn.Spawned;
            
            if (spawned)
            {
                this.parent.Severity = 90f;
                MoteMaker.ThrowLightningGlow(base.Pawn.TrueCenter(), base.Pawn.Map, 1f);
                DeterminePsionicHD();
            }
        }

        private void DeterminePsionicHD()
        {
            CompAbilityUserMight comp = this.Pawn.GetComp<CompAbilityUserMight>();
            this.PwrVal = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_PsionicAugmentation.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicAugmentation_pwr").level;
            this.EffVal = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_PsionicAugmentation.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicAugmentation_eff").level;
            this.VerVal = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_PsionicAugmentation.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicAugmentation_ver").level;
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            if (this.Pawn.Spawned && !this.Pawn.Dead && !this.Pawn.Downed)
            {
                base.CompPostTick(ref severityAdjustment);
                if (base.Pawn != null & base.parent != null)
                {
                    if (!initialized)
                    {
                        initialized = true;
                        this.Initialize();
                    }
                }

                CompAbilityUserMight comp = this.Pawn.GetComp<CompAbilityUserMight>();
                if (this.doPsionicAttack)
                {
                    this.ticksTillPsionicStrike--;
                    if (this.ticksTillPsionicStrike <= 0)
                    {                        
                        this.doPsionicAttack = false;
                        MightPowerSkill ver = comp.MightData.MightPowerSkill_PsionicAugmentation.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicAugmentation_ver");
                        if (!threat.Destroyed && !threat.Dead)
                        {
                            TM_MoteMaker.MakePowerBeamMotePsionic(threat.DrawPos.ToIntVec3(), threat.Map, 2f, 2f, .7f, .1f, .6f);
                            DamageInfo dinfo2 = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_PsionicInjury, Rand.Range(6, 12) + (2 * ver.level), 0, -1, this.Pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, this.threat);
                            this.threat.TakeDamage(dinfo2);
                        }
                    }
                }

                if (Find.TickManager.TicksGame % 60 == 0)
                {
                    severityAdjustment += (this.Pawn.GetStatValue(StatDefOf.PsychicSensitivity, false)*Rand.Range(.04f, .12f));
                    if (Find.Selector.FirstSelectedObject == this.Pawn)
                    {
                        HediffStage hediffStage = this.Pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_PsionicHD"), false).CurStage;
                        hediffStage.label = this.parent.Severity.ToString("0.00") + "%";
                    }

                }

                if (comp.usePsionicAugmentationToggle)
                {
                    if (Find.TickManager.TicksGame % 600 == 0 && !this.Pawn.Drafted)
                    {
                        if (this.parent.Severity >= 95 && this.Pawn.CurJob.targetA.Thing != null)
                        {
                            DeterminePsionicHD();
                            if ((this.Pawn.Position - this.Pawn.CurJob.targetA.Thing.Position).LengthHorizontal > 20 && (this.Pawn.Position - this.Pawn.CurJob.targetA.Thing.Position).LengthHorizontal < 300 && this.Pawn.CurJob.locomotionUrgency >= LocomotionUrgency.Jog && this.Pawn.CurJob.bill == null)
                            {
                                this.parent.Severity -= 10f;
                                if (this.EffVal == 0)
                                {
                                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_PsionicSpeedHD"), 1f + .02f * this.EffVal);
                                }
                                else if (this.EffVal == 1)
                                {
                                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_PsionicSpeedHD_I"), 1f + .02f * this.EffVal);
                                }
                                else if (this.EffVal == 2)
                                {
                                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_PsionicSpeedHD_II"), 1f + .02f * this.EffVal);
                                }
                                else
                                {
                                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_PsionicSpeedHD_III"), 1f + .02f * this.EffVal);
                                }
                                for (int i = 0; i < 12; i++)
                                {
                                    float direction = Rand.Range(0, 360);
                                    TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Psi"), this.Pawn.DrawPos, this.Pawn.Map, Rand.Range(.1f, .4f), 0.2f, .02f, .1f, 0, Rand.Range(8, 10), direction, direction);
                                }
                            }

                            if (this.Pawn.CurJob.targetA.Thing != null && (this.Pawn.Position - this.Pawn.CurJob.targetA.Thing.Position).LengthHorizontal < 2 && this.Pawn.CurJob.bill != null)
                            {
                                this.parent.Severity -= 6f;
                                if (this.PwrVal == 0)
                                {
                                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_PsionicManipulationHD"), 1f + .02f * this.PwrVal);
                                }
                                else if (this.PwrVal == 1)
                                {
                                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_PsionicManipulationHD_I"), 1f + .02f * this.PwrVal);
                                }
                                else if (this.PwrVal == 2)
                                {
                                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_PsionicManipulationHD_II"), 1f + .02f * this.PwrVal);
                                }
                                else
                                {
                                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_PsionicManipulationHD_III"), 1f + .02f * this.PwrVal);
                                }
                                for (int i = 0; i < 12; i++)
                                {
                                    float direction = Rand.Range(0, 360);
                                    TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Psi"), this.Pawn.DrawPos, this.Pawn.Map, Rand.Range(.1f, .4f), 0.2f, .02f, .1f, 0, Rand.Range(8, 10), direction, direction);
                                }
                            }
                        }
                    }

                    if (this.parent.Severity >= 20)
                    {
                        DeterminePsionicHD();
                        if (Find.TickManager.TicksGame % 180 == 0 && (this.Pawn.Drafted || !this.Pawn.IsColonist) && this.Pawn.equipment.Primary != null && !this.Pawn.equipment.Primary.def.IsRangedWeapon)
                        {
                            if (this.Pawn.CurJob.targetA.Thing != null && this.Pawn.CurJob.targetA.Thing is Pawn)
                            {
                                float targetDistance = (this.Pawn.Position - this.Pawn.CurJob.targetA.Thing.Position).LengthHorizontal;
                                if (targetDistance > 3 && targetDistance < (12 + EffVal))
                                {
                                    for (int i = 0; i < 12; i++)
                                    {
                                        float direction = Rand.Range(0, 360);
                                        TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Psi"), this.Pawn.DrawPos, this.Pawn.Map, Rand.Range(.1f, .4f), 0.2f, .02f, .1f, 0, Rand.Range(8, 10), direction, direction);
                                    }
                                    FlyingObject_PsionicLeap flyingObject = (FlyingObject_PsionicLeap)GenSpawn.Spawn(ThingDef.Named("FlyingObject_PsionicLeap"), this.Pawn.Position, this.Pawn.Map);
                                    flyingObject.Launch(this.Pawn, this.Pawn.CurJob.targetA.Thing, this.Pawn);
                                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_PsionicHD"), -3f);
                                    comp.Stamina.CurLevel -= .03f;
                                    comp.MightUserXP += Rand.Range(3, 5);
                                }
                            }
                        }

                        if (this.nextPsionicAttack < Find.TickManager.TicksGame && this.Pawn.Drafted)
                        {
                            if (this.Pawn.CurJob.def != TorannMagicDefOf.JobDriver_PsionicBarrier && VerVal > 0)
                            {
                                this.threat = GetNearbyTarget(20 + (2 * VerVal));
                                if (threat != null)
                                {
                                    //start psionic attack; ends after delay
                                    SoundInfo info = SoundInfo.InMap(new TargetInfo(this.Pawn.Position, this.Pawn.Map, false), MaintenanceType.None);
                                    TorannMagicDefOf.TM_Implosion.PlayOneShot(info);
                                    Effecter psionicAttack = TorannMagicDefOf.TM_GiantExplosion.Spawn();
                                    psionicAttack.Trigger(new TargetInfo(threat.Position, threat.Map, false), new TargetInfo(threat.Position, threat.Map, false));
                                    psionicAttack.Cleanup();
                                    for (int i = 0; i < 12; i++)
                                    {
                                        float direction = Rand.Range(0, 360);
                                        TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Psi"), this.Pawn.DrawPos, this.Pawn.Map, Rand.Range(.1f, .4f), 0.2f, .02f, .1f, 0, Rand.Range(8, 10), direction, direction);
                                    }
                                    float weaponModifier = 1;
                                    if (this.Pawn.equipment.Primary != null)
                                    {
                                        if(this.Pawn.equipment.Primary.def.IsRangedWeapon)
                                        {
                                            StatModifier wpnMass = this.Pawn.equipment.Primary.def.statBases.FirstOrDefault((StatModifier x) => x.stat.defName == "Mass");
                                            weaponModifier = Mathf.Clamp(wpnMass.value, .8f, 6);                                            
                                        }
                                        else //assume melee weapon
                                        {
                                            StatModifier wpnMass = this.Pawn.equipment.Primary.def.statBases.FirstOrDefault((StatModifier x) => x.stat.defName == "Mass");
                                            weaponModifier = Mathf.Clamp(wpnMass.value, .6f, 5);
                                        }
                                    }
                                    else //unarmed
                                    {
                                        weaponModifier = .4f;
                                    }
                                    this.nextPsionicAttack = Find.TickManager.TicksGame + (int)(Mathf.Clamp((600 - (60 * verVal)) * weaponModifier, 120, 900));
                                    float energyCost = Mathf.Clamp((10f - VerVal) * weaponModifier, 2f, 12f);
                                    HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_PsionicHD"), -energyCost);
                                    comp.MightUserXP += Rand.Range(6, 10);
                                    this.doPsionicAttack = true;
                                    this.ticksTillPsionicStrike = 24;
                                }
                            }
                        }
                    }
                }
            }
        }

        private float GetAngleFromTo(Vector3 from, Vector3 to)
        {
            Vector3 heading = (to - from);
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;
            float directionAngle = (Quaternion.AngleAxis(90, Vector3.up) * direction).ToAngleFlat();
            return directionAngle;
        }

        public override bool CompShouldRemove
        {
            get
            {
                return base.CompShouldRemove;
            }
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look<bool>(ref this.initialized, "initialized", false, false);
            //Scribe_Values.Look<int>(ref this.age, "age", 0, false);
            Scribe_Values.Look<int>(ref this.pwrVal, "pwrVal", 0, false);
            Scribe_Values.Look<int>(ref this.effVal, "effVal", 0, false);
            Scribe_Values.Look<int>(ref this.verVal, "verVal", 0, false);
        }

        public Pawn GetNearbyTarget(float radius)
        {
            List<Pawn> allPawns = this.Pawn.Map.mapPawns.AllPawnsSpawned;
            for (int i = 0; i < allPawns.Count(); i++)
            {
                if (!allPawns[i].DestroyedOrNull() && allPawns[i] != this.Pawn)
                {
                    if (!allPawns[i].Dead && !allPawns[i].Downed && !allPawns[i].IsPrisonerInPrisonCell())
                    {
                        if ((allPawns[i].Position - this.Pawn.Position).LengthHorizontal <= radius)
                        {
                            if (allPawns[i].Faction != null && allPawns[i].Faction != this.Pawn.Faction)
                            {
                               // Log.Message("checking " + allPawns[i].LabelShort + " for hostility: " + FactionUtility.HostileTo(this.Pawn.Faction, allPawns[i].Faction));
                                //Log.Message(this.Pawn.Faction.RelationWith(allPawns[i].Faction, false).ToString());
                                if (FactionUtility.HostileTo(this.Pawn.Faction, allPawns[i].Faction))
                                {
                                    return allPawns[i];
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
