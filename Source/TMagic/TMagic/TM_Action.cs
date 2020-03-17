using UnityEngine;
using Verse;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;
using RimWorld;
using System;
using AbilityUser;
using HarmonyLib;
using TorannMagic.Enchantment;

namespace TorannMagic
{
    public static class TM_Action
    {
        public static class TM_Toils
        {
            public static void GotoAndWait(Pawn pawn, LocalTargetInfo target, int durationTicks)
            {
                if (pawn != null && target != null)
                {
                    if (pawn.drafter != null && !pawn.Drafted && pawn.GetPosture() == PawnPosture.Standing && pawn.jobs != null && pawn.CurJob != null && !pawn.CurJob.playerForced)
                    {
                        Job job = new Job(TorannMagicDefOf.JobDriver_TM_GotoAndWait, target, durationTicks, false);
                        pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                    }
                }
            }

            public static void SendPawnTo(Pawn pawn, LocalTargetInfo target)
            {
                if (pawn != null && target != null)
                {
                    if (pawn.drafter != null && !pawn.Drafted && pawn.GetPosture() == PawnPosture.Standing && pawn.jobs != null && pawn.CurJob != null && !pawn.CurJob.playerForced)
                    {
                        Job job = new Job(JobDefOf.Goto, target);
                        pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                    }
                }
            }

            public static void PawnWait(Pawn pawn, int duration)
            {
                duration *= 60; //seconds to ticks
                if (pawn != null)
                {
                    if (pawn.drafter != null && !pawn.Drafted && pawn.GetPosture() == PawnPosture.Standing && pawn.jobs != null && pawn.CurJob != null && !pawn.CurJob.playerForced)
                    {
                        Job job = new Job(JobDefOf.Wait);
                        job.expiryInterval = duration;
                        pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                    }
                }
            }
        }

        public static void DoMeleeReversal(DamageInfo dinfo, Pawn reflectingPawn)
        {
            Thing instigator = dinfo.Instigator;

            if (instigator is Pawn)
            {
                Pawn meleePawn = instigator as Pawn;
                if ((dinfo.Weapon != null && dinfo.Weapon.IsMeleeWeapon) || dinfo.WeaponBodyPartGroup != null)
                {
                    DamageInfo dinfo2 = new DamageInfo(dinfo.Def, dinfo.Amount, dinfo.ArmorPenetrationInt, dinfo.Angle, reflectingPawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, meleePawn);
                    meleePawn.TakeDamage(dinfo2);
                }
            }
        }

        public static void DoReversal(DamageInfo dinfo, Pawn reflectingPawn)
        {
            Thing instigator = dinfo.Instigator;

            if (instigator is Pawn)
            {
                Pawn shooterPawn = instigator as Pawn;
                if (dinfo.Weapon != null && !dinfo.Weapon.IsMeleeWeapon && dinfo.WeaponBodyPartGroup == null)
                {
                    TM_CopyAndLaunchProjectile.CopyAndLaunchThing(shooterPawn.equipment.PrimaryEq.PrimaryVerb.verbProps.defaultProjectile, reflectingPawn, instigator, shooterPawn, ProjectileHitFlags.All, null);
                }
            }
            if (instigator is Building)
            {
                Building turret = instigator as Building;
                ThingDef projectile = null;

                if (turret.def.building.turretGunDef != null)
                {
                    ThingDef turretGun = turret.def.building.turretGunDef;
                    for (int i = 0; i < turretGun.Verbs.Count; i++)
                    {
                        if (turretGun.Verbs[i].defaultProjectile != null)
                        {
                            projectile = turretGun.Verbs[i].defaultProjectile;
                        }
                    }
                }

                if (projectile != null)
                {
                    TM_CopyAndLaunchProjectile.CopyAndLaunchThing(projectile, reflectingPawn, instigator, turret, ProjectileHitFlags.All, null);
                }
            }

            //GiveReversalJob(dinfo);            
        }

        public static void DoReversalRandomTarget(DamageInfo dinfo, Pawn reflectingPawn, float minRange, float maxRange)
        {
            Thing instigator = dinfo.Instigator;

            if (instigator is Pawn)
            {
                Pawn shooterPawn = instigator as Pawn;
                if (dinfo.Weapon != null && !dinfo.Weapon.IsMeleeWeapon && dinfo.WeaponBodyPartGroup == null)
                {
                    Pawn randomTarget = null;
                    randomTarget = TM_Calc.FindNearbyEnemy(reflectingPawn, (int)maxRange);
                    if (randomTarget != null)
                    {
                        TM_CopyAndLaunchProjectile.CopyAndLaunchThing(shooterPawn.equipment.PrimaryEq.PrimaryVerb.verbProps.defaultProjectile, reflectingPawn, randomTarget, randomTarget, ProjectileHitFlags.All, null);
                    }
                }
            }
            if (instigator is Building)
            {
                Building turret = instigator as Building;
                ThingDef projectile = null;

                if (turret.def.building.turretGunDef != null)
                {
                    ThingDef turretGun = turret.def.building.turretGunDef;
                    for (int i = 0; i < turretGun.Verbs.Count; i++)
                    {
                        if (turretGun.Verbs[i].defaultProjectile != null)
                        {
                            projectile = turretGun.Verbs[i].defaultProjectile;
                        }
                    }
                }

                if (projectile != null)
                {
                    Thing target = null;
                    if ((turret.Position - reflectingPawn.Position).LengthHorizontal <= maxRange)
                    {
                        target = turret;
                    }
                    else
                    {
                        target = TM_Calc.FindNearbyEnemy(reflectingPawn, (int)maxRange);
                    }
                    TM_CopyAndLaunchProjectile.CopyAndLaunchThing(projectile, reflectingPawn, target, target, ProjectileHitFlags.All, null);
                }
            }
        }

        public static void DoAction_TechnoWeaponCopy(Pawn caster, Thing thing, ThingDef td = null)
        {
            CompAbilityUserMagic comp = caster.TryGetComp<CompAbilityUserMagic>();
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            bool destroyThingAtEnd = false;
            if(thing == null && td != null)
            {
                destroyThingAtEnd = true;
                thing = ThingMaker.MakeThing(td);
            }

            if (thing != null && thing.def != null && thing.def.IsRangedWeapon && (thing.def.techLevel >= TechLevel.Industrial || settingsRef.unrestrictedWeaponCopy) && (thing.def.Verbs.FirstOrDefault().verbClass.ToString() == "Verse.Verb_Shoot" || settingsRef.unrestrictedWeaponCopy))
            {
                int verVal = comp.MagicData.MagicPowerSkill_TechnoWeapon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoWeapon_ver").level;
                int pwrVal = comp.MagicData.MagicPowerSkill_TechnoWeapon.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoWeapon_pwr").level;
                ThingDef newThingDef = new ThingDef();
                ThingDef newProjectileDef = new ThingDef();
                if (comp.technoWeaponDefNum != -1)
                {
                    newThingDef = ThingDef.Named("TM_TechnoWeapon_Base" + comp.technoWeaponDefNum.ToString());
                }
                else
                {
                    int highNum = 0;
                    List<Pawn> mapPawns = caster.Map.mapPawns.AllPawns.ToList();
                    for (int i = 0; i < mapPawns.Count; i++)
                    {
                        if (!mapPawns[i].DestroyedOrNull() && mapPawns[i].RaceProps.Humanlike)
                        {
                            if (mapPawns[i].story.traits.HasTrait(TorannMagicDefOf.Technomancer) && mapPawns[i].GetComp<CompAbilityUserMagic>().IsMagicUser)
                            {
                                if (mapPawns[i].GetComp<CompAbilityUserMagic>().technoWeaponDefNum > highNum)
                                {
                                    highNum = mapPawns[i].GetComp<CompAbilityUserMagic>().technoWeaponDefNum;
                                }
                            }
                        }
                    }
                    if (ModOptions.Constants.GetTechnoWeaponCount() > highNum)
                    {
                        highNum = ModOptions.Constants.GetTechnoWeaponCount();
                    }
                    highNum++;
                    newThingDef = ThingDef.Named("TM_TechnoWeapon_Base" + highNum.ToString());
                    comp.technoWeaponDefNum = highNum;
                    ModOptions.Constants.SetTechnoWeaponCount(highNum);
                }
                comp.technoWeaponThing = thing;
                comp.technoWeaponThingDef = thing.def;
                newThingDef.label = thing.def.label + " (modified)";
                newThingDef.description = thing.def.description + "\n\nThis weapon has been modified by a Technomancer.";
                newThingDef.graphicData.texPath = thing.def.graphicData.texPath;
                newThingDef.soundInteract = thing.def.soundInteract;

                newThingDef.SetStatBaseValue(StatDefOf.RangedWeapon_DamageMultiplier, 1f + (.02f * pwrVal));
                newThingDef.SetStatBaseValue(StatDefOf.RangedWeapon_Cooldown, thing.GetStatValue(StatDefOf.RangedWeapon_Cooldown) * (1 - .02f * pwrVal));
                newThingDef.SetStatBaseValue(StatDefOf.AccuracyTouch, thing.GetStatValue(StatDefOf.AccuracyTouch) * (1 + .01f * pwrVal));
                newThingDef.SetStatBaseValue(StatDefOf.AccuracyShort, thing.GetStatValue(StatDefOf.AccuracyShort) * (1 + .01f * pwrVal));
                newThingDef.SetStatBaseValue(StatDefOf.AccuracyMedium, thing.GetStatValue(StatDefOf.AccuracyMedium) * (1 + .01f * pwrVal));
                newThingDef.SetStatBaseValue(StatDefOf.AccuracyLong, thing.GetStatValue(StatDefOf.AccuracyLong) * (1 + .01f * pwrVal));

                newThingDef.Verbs.FirstOrDefault().defaultProjectile = thing.def.Verbs.FirstOrDefault().defaultProjectile;
                newThingDef.Verbs.FirstOrDefault().range = thing.def.Verbs.FirstOrDefault().range * (1f + .02f * pwrVal);
                newThingDef.Verbs.FirstOrDefault().warmupTime = thing.def.Verbs.FirstOrDefault().warmupTime * (1f - .02f * pwrVal);
                newThingDef.Verbs.FirstOrDefault().burstShotCount = Mathf.RoundToInt(thing.def.Verbs.FirstOrDefault().burstShotCount * (1f + .02f * pwrVal));
                newThingDef.Verbs.FirstOrDefault().ticksBetweenBurstShots = Mathf.RoundToInt(thing.def.Verbs.FirstOrDefault().ticksBetweenBurstShots * (1f - .02f * pwrVal));
                newThingDef.Verbs.FirstOrDefault().soundCast = thing.def.Verbs.FirstOrDefault().soundCast;
                Thing technoWeapon = ThingMaker.MakeThing(newThingDef, null);

                try
                {
                    CompQuality twcq = technoWeapon.TryGetComp<CompQuality>();
                    QualityCategory qc = thing.TryGetComp<CompQuality>().Quality;
                    twcq.SetQuality(qc, ArtGenerationContext.Colony);
                }
                catch (NullReferenceException ex)
                {
                    //ignore
                }
                GenPlace.TryPlaceThing(technoWeapon, caster.Position, caster.Map, ThingPlaceMode.Direct, null, null);
                Job job = new Job(JobDefOf.Equip, technoWeapon);
                caster.jobs.TryTakeOrderedJob(job, JobTag.ChangingApparel);

            }
            else
            {
                Log.Message("cannot copy target thing or unable to restore techno weapon");
            }
            if(destroyThingAtEnd)
            {
                thing.Destroy(DestroyMode.Vanish);
            }
        }

        public static void DoAction_PistolSpecCopy(Pawn caster, ThingWithComps thing)
        {
            CompAbilityUserMight comp = caster.TryGetComp<CompAbilityUserMight>();
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();

            if (thing != null && thing.def != null && thing.def.IsRangedWeapon)
            {
                int pwrVal = comp.MightData.MightPowerSkill_PistolSpec.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PistolSpec_pwr").level;
                ThingDef newThingDef = new ThingDef();
                ThingDef newProjectileDef = new ThingDef();
                if (comp.specWpnRegNum != -1)
                {
                    newThingDef = ThingDef.Named("TM_PistolSpec_Base" + comp.specWpnRegNum.ToString());
                }
                else
                {
                    int highNum = 0;
                    if (ModOptions.Constants.GetPistolSpecCount() > highNum)
                    {
                        highNum = ModOptions.Constants.GetPistolSpecCount();
                    }
                    highNum++;
                    newThingDef = ThingDef.Named("TM_PistolSpec_Base" + highNum.ToString());
                    comp.specWpnRegNum = highNum;
                    ModOptions.Constants.SetPistolSpecCount(highNum);
                }
                newThingDef.label = thing.def.label + "++";
                newThingDef.description = thing.def.description + "\n\nThis weapon has improved stats in the hands of a specialist.";
                newThingDef.graphic = thing.def.graphic;
                newThingDef.graphicData = thing.def.graphicData;
                newThingDef.graphicData.texPath = thing.def.graphicData.texPath;
                newThingDef.equipmentType = EquipmentType.Primary;
                newThingDef.uiIcon = thing.def.uiIcon;
                newThingDef.soundInteract = thing.def.soundInteract;

                newThingDef.SetStatBaseValue(StatDefOf.RangedWeapon_DamageMultiplier, thing.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier) * (1f + (.03f * pwrVal)));
                newThingDef.SetStatBaseValue(StatDefOf.RangedWeapon_Cooldown, thing.GetStatValue(StatDefOf.RangedWeapon_Cooldown) * (1 - .025f * pwrVal));
                newThingDef.SetStatBaseValue(StatDefOf.AccuracyTouch, thing.GetStatValue(StatDefOf.AccuracyTouch));
                newThingDef.SetStatBaseValue(StatDefOf.AccuracyShort, thing.GetStatValue(StatDefOf.AccuracyShort) * (1 + .015f * pwrVal));
                newThingDef.SetStatBaseValue(StatDefOf.AccuracyMedium, thing.GetStatValue(StatDefOf.AccuracyMedium) * (1 + .005f * pwrVal));
                newThingDef.SetStatBaseValue(StatDefOf.AccuracyLong, thing.GetStatValue(StatDefOf.AccuracyLong));

                newThingDef.Verbs.FirstOrDefault().defaultProjectile = thing.def.Verbs.FirstOrDefault().defaultProjectile;
                newThingDef.Verbs.FirstOrDefault().range = thing.def.Verbs.FirstOrDefault().range * (1f + .01f * pwrVal);
                newThingDef.Verbs.FirstOrDefault().warmupTime = thing.def.Verbs.FirstOrDefault().warmupTime * (1f - .025f * pwrVal);
                newThingDef.Verbs.FirstOrDefault().burstShotCount = Mathf.RoundToInt(thing.def.Verbs.FirstOrDefault().burstShotCount * (1f + .02f * pwrVal));
                newThingDef.Verbs.FirstOrDefault().ticksBetweenBurstShots = Mathf.RoundToInt(thing.def.Verbs.FirstOrDefault().ticksBetweenBurstShots * (1f - .03f * pwrVal));
                newThingDef.Verbs.FirstOrDefault().soundCast = thing.def.Verbs.FirstOrDefault().soundCast;
                ThingWithComps specWeapon = (ThingWithComps)ThingMaker.MakeThing(newThingDef, null);

                try
                {
                    CompQuality twcq = specWeapon.TryGetComp<CompQuality>();
                    QualityCategory qc = thing.TryGetComp<CompQuality>().Quality;
                    twcq.SetQuality(qc, ArtGenerationContext.Colony);
                }
                catch (NullReferenceException ex)
                {
                    //ignore
                }

                try
                {
                    for (int i = 0; i < thing.AllComps.Count; i++)
                    {

                        if (specWeapon.AllComps[i] is CompEquippable)
                        {
                            //CompEquippable ce = thing.AllComps[i] as CompEquippable;
                            //ce.PrimaryVerb.loadID = String.Concat(ce.PrimaryVerb.loadID, "1");

                        }
                        else
                        {
                            specWeapon.AllComps.AddDistinct(thing.AllComps[i]);
                        }
                    }
                }
                catch (NullReferenceException ex)
                {
                    //ignore
                }
                specWeapon.HitPoints = thing.HitPoints;
                //if (caster.equipment.Primary != null)
                //{
                //    Log.Message("destroying " + caster.equipment.Primary + " in pistol copy");
                //    caster.equipment.Primary.Destroy(DestroyMode.Vanish);
                //}
                caster.equipment.AddEquipment(specWeapon);
                //GenPlace.TryPlaceThing(technoWeapon, caster.Position, caster.Map, ThingPlaceMode.Direct, null, null);
                //Job job = new Job(JobDefOf.Equip, technoWeapon);
                //caster.jobs.TryTakeOrderedJob(job, JobTag.ChangingApparel);

            }
            else
            {
                Log.Message("cannot copy target thing or unable to restore techno weapon");
            }
        }

        public static void DoAction_RifleSpecCopy(Pawn caster, ThingWithComps thing)
        {
            CompAbilityUserMight comp = caster.TryGetComp<CompAbilityUserMight>();
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();

            if (thing != null && thing.def != null && thing.def.IsRangedWeapon)
            {
                int pwrVal = comp.MightData.MightPowerSkill_RifleSpec.FirstOrDefault((MightPowerSkill x) => x.label == "TM_RifleSpec_pwr").level;
                ThingDef newThingDef = new ThingDef();
                ThingDef newProjectileDef = new ThingDef();
                if (comp.specWpnRegNum != -1)
                {
                    newThingDef = ThingDef.Named("TM_RifleSpec_Base" + comp.specWpnRegNum.ToString());
                }
                else
                {
                    int highNum = 0;
                    if (ModOptions.Constants.GetRifleSpecCount() > highNum)
                    {
                        highNum = ModOptions.Constants.GetRifleSpecCount();
                    }
                    highNum++;
                    newThingDef = ThingDef.Named("TM_RifleSpec_Base" + highNum.ToString());
                    comp.specWpnRegNum = highNum;
                    ModOptions.Constants.SetRifleSpecCount(highNum);
                }
                newThingDef.label = thing.def.label + "++";
                newThingDef.description = thing.def.description + "\n\nThis weapon has improved stats in the hands of a specialist.";
                newThingDef.graphic = thing.def.graphic;
                newThingDef.graphicData = thing.def.graphicData;
                newThingDef.graphicData.texPath = thing.def.graphicData.texPath;
                newThingDef.equipmentType = EquipmentType.Primary;
                newThingDef.uiIcon = thing.def.uiIcon;
                newThingDef.soundInteract = thing.def.soundInteract;

                newThingDef.SetStatBaseValue(StatDefOf.RangedWeapon_DamageMultiplier, thing.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier) * (1f + (.02f * pwrVal)));
                newThingDef.SetStatBaseValue(StatDefOf.RangedWeapon_Cooldown, thing.GetStatValue(StatDefOf.RangedWeapon_Cooldown) * (1 - .01f * pwrVal));
                newThingDef.SetStatBaseValue(StatDefOf.AccuracyTouch, thing.GetStatValue(StatDefOf.AccuracyTouch));
                newThingDef.SetStatBaseValue(StatDefOf.AccuracyShort, thing.GetStatValue(StatDefOf.AccuracyShort) * (1 + .01f * pwrVal));
                newThingDef.SetStatBaseValue(StatDefOf.AccuracyMedium, thing.GetStatValue(StatDefOf.AccuracyMedium) * (1 + .01f * pwrVal));
                newThingDef.SetStatBaseValue(StatDefOf.AccuracyLong, thing.GetStatValue(StatDefOf.AccuracyLong) * (1 + .01f * pwrVal));

                newThingDef.Verbs.FirstOrDefault().defaultProjectile = thing.def.Verbs.FirstOrDefault().defaultProjectile;
                newThingDef.Verbs.FirstOrDefault().range = thing.def.Verbs.FirstOrDefault().range * (1f + .02f * pwrVal);
                newThingDef.Verbs.FirstOrDefault().warmupTime = thing.def.Verbs.FirstOrDefault().warmupTime * (1f - .01f * pwrVal);
                newThingDef.Verbs.FirstOrDefault().burstShotCount = Mathf.RoundToInt(thing.def.Verbs.FirstOrDefault().burstShotCount * (1f + .03f * pwrVal));
                newThingDef.Verbs.FirstOrDefault().ticksBetweenBurstShots = Mathf.RoundToInt(thing.def.Verbs.FirstOrDefault().ticksBetweenBurstShots * (1f - .02f * pwrVal));
                newThingDef.Verbs.FirstOrDefault().soundCast = thing.def.Verbs.FirstOrDefault().soundCast;
                ThingWithComps specWeapon = (ThingWithComps)ThingMaker.MakeThing(newThingDef, null);

                try
                {
                    CompQuality twcq = specWeapon.TryGetComp<CompQuality>();
                    QualityCategory qc = thing.TryGetComp<CompQuality>().Quality;
                    twcq.SetQuality(qc, ArtGenerationContext.Colony);
                }
                catch (NullReferenceException ex)
                {
                    //ignore
                }

                try
                {
                    for (int i = 0; i < thing.AllComps.Count; i++)
                    {

                        if (specWeapon.AllComps[i] is CompEquippable)
                        {
                            //CompEquippable ce = thing.AllComps[i] as CompEquippable;
                            //ce.PrimaryVerb.loadID = String.Concat(ce.PrimaryVerb.loadID, "1");

                        }
                        else
                        {
                            specWeapon.AllComps.AddDistinct(thing.AllComps[i]);
                        }
                    }
                }
                catch (NullReferenceException ex)
                {
                    //ignore
                }
                specWeapon.HitPoints = thing.HitPoints;
                //if (caster.equipment.Primary != null)
                //{
                //    caster.equipment.Primary.Destroy(DestroyMode.Vanish);
                //}
                caster.equipment.AddEquipment(specWeapon);
                //GenPlace.TryPlaceThing(technoWeapon, caster.Position, caster.Map, ThingPlaceMode.Direct, null, null);
                //Job job = new Job(JobDefOf.Equip, technoWeapon);
                //caster.jobs.TryTakeOrderedJob(job, JobTag.ChangingApparel);

            }
            else
            {
                Log.Message("cannot copy target thing or unable to restore techno weapon");
            }
        }

        //public static void DoAction_ShotgunSpecCopy(Pawn caster, ThingDef thingDef, int wpnQuality, ThingDef stuff = null)
        public static void DoAction_ShotgunSpecCopy(Pawn caster, ThingWithComps thing)
        {
            CompAbilityUserMight comp = caster.TryGetComp<CompAbilityUserMight>();
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();

            if (thing != null && thing.def != null && thing.def.IsRangedWeapon)
            {
                int pwrVal = comp.MightData.MightPowerSkill_ShotgunSpec.FirstOrDefault((MightPowerSkill x) => x.label == "TM_ShotgunSpec_pwr").level;

                //ThingWithComps thing = (ThingWithComps)ThingMaker.MakeThing(thingDef, stuff);
                //CompQuality cq = thing.TryGetComp<CompQuality>();
                //if(cq != null)
                //{
                //    cq.SetQuality((QualityCategory)wpnQuality, ArtGenerationContext.Colony);
                //}

                ThingDef newThingDef = new ThingDef();
                ThingDef newProjectileDef = new ThingDef();
                if (comp.specWpnRegNum != -1)
                {
                    newThingDef = ThingDef.Named("TM_ShotgunSpec_Base" + comp.specWpnRegNum.ToString());
                }
                else
                {
                    int highNum = 0;
                    if (ModOptions.Constants.GetShotgunSpecCount() > highNum)
                    {
                        highNum = ModOptions.Constants.GetShotgunSpecCount();
                    }
                    highNum++;
                    newThingDef = ThingDef.Named("TM_ShotgunSpec_Base" + highNum.ToString());
                    comp.specWpnRegNum = highNum;
                    ModOptions.Constants.SetShotgunSpecCount(highNum);
                }
                newThingDef.label = thing.def.label + "++";
                newThingDef.description = thing.def.description + "\n\nThis weapon has improved stats in the hands of a specialist.";
                newThingDef.equipmentType = EquipmentType.Primary;
                newThingDef.graphic = thing.def.graphic;
                newThingDef.graphicData = thing.def.graphicData;
                newThingDef.graphicData.texPath = thing.def.graphicData.texPath;
                newThingDef.uiIcon = thing.def.uiIcon;
                newThingDef.soundInteract = thing.def.soundInteract;

                newThingDef.SetStatBaseValue(StatDefOf.RangedWeapon_DamageMultiplier, thing.GetStatValue(StatDefOf.RangedWeapon_DamageMultiplier) * (1f + (.02f * pwrVal)));
                newThingDef.SetStatBaseValue(StatDefOf.RangedWeapon_Cooldown, thing.GetStatValue(StatDefOf.RangedWeapon_Cooldown) * (1 - .03f * pwrVal));
                newThingDef.SetStatBaseValue(StatDefOf.AccuracyTouch, thing.GetStatValue(StatDefOf.AccuracyTouch) * (1 + .01f * pwrVal));
                newThingDef.SetStatBaseValue(StatDefOf.AccuracyShort, thing.GetStatValue(StatDefOf.AccuracyShort) * (1 + .015f * pwrVal));
                newThingDef.SetStatBaseValue(StatDefOf.AccuracyMedium, thing.GetStatValue(StatDefOf.AccuracyMedium) * (1 + .005f * pwrVal));
                newThingDef.SetStatBaseValue(StatDefOf.AccuracyLong, thing.GetStatValue(StatDefOf.AccuracyLong));
                                
                newThingDef.Verbs.FirstOrDefault().defaultProjectile = thing.def.Verbs.FirstOrDefault().defaultProjectile;
                newThingDef.Verbs.FirstOrDefault().range = thing.def.Verbs.FirstOrDefault().range * (1f + .01f * pwrVal);
                newThingDef.Verbs.FirstOrDefault().warmupTime = thing.def.Verbs.FirstOrDefault().warmupTime * (1f - .03f * pwrVal);
                newThingDef.Verbs.FirstOrDefault().burstShotCount = Mathf.RoundToInt(thing.def.Verbs.FirstOrDefault().burstShotCount);
                newThingDef.Verbs.FirstOrDefault().ticksBetweenBurstShots = Mathf.RoundToInt(thing.def.Verbs.FirstOrDefault().ticksBetweenBurstShots * (1f - .02f * pwrVal));
                newThingDef.Verbs.FirstOrDefault().soundCast = thing.def.Verbs.FirstOrDefault().soundCast;
                ThingWithComps specWeapon = (ThingWithComps)ThingMaker.MakeThing(newThingDef, null);

                try
                {
                    CompQuality twcq = specWeapon.TryGetComp<CompQuality>();
                    QualityCategory qc = thing.TryGetComp<CompQuality>().Quality;
                    twcq.SetQuality(qc, ArtGenerationContext.Colony);
                }
                catch (NullReferenceException ex)
                {
                    //ignore
                }

                try
                {
                    for (int i = 0; i < thing.AllComps.Count; i++)
                    {
                        
                        if(specWeapon.AllComps[i] is CompEquippable)
                        {
                            //CompEquippable ce = thing.AllComps[i] as CompEquippable;
                            //ce.PrimaryVerb.loadID = String.Concat(ce.PrimaryVerb.loadID, "1");
                            
                        }
                        else
                        {
                            specWeapon.AllComps.AddDistinct(thing.AllComps[i]);
                        }
                    }
                }
                catch (NullReferenceException ex)
                {
                    //ignore
                }
                specWeapon.HitPoints = thing.HitPoints;
                //if(caster.equipment.Primary != null)
                //{
                //    caster.equipment.Primary.Destroy(DestroyMode.Vanish);
                //}
                caster.equipment.AddEquipment(specWeapon);
                //GenPlace.TryPlaceThing(technoWeapon, caster.Position, caster.Map, ThingPlaceMode.Direct, null, null);
                //Job job = new Job(JobDefOf.Equip, technoWeapon);
                //caster.jobs.TryTakeOrderedJob(job, JobTag.ChangingApparel);

            }
            else
            {
                Log.Message("cannot copy target thing or unable to restore techno weapon");
            }
        }

        public static void DoAction_ApplySplashDamage(DamageInfo dinfo, Pawn caster, Pawn target, Map map, int ver = 0)
        {
            bool multiplePawns = false;
            bool flag = !dinfo.InstantPermanentInjury;
            CompAbilityUserMight comp = caster.GetComp<CompAbilityUserMight>();
            MightPowerSkill eff = comp.MightData.MightPowerSkill_DragonStrike.FirstOrDefault((MightPowerSkill x) => x.label == "TM_DragonStrike_eff");
            MightPowerSkill globalSkill = comp.MightData.MightPowerSkill_global_seff.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_seff_pwr");
            float actualStaminaCost = .1f * (1 - (.1f * eff.level) * (1 - (.03f * globalSkill.level)));
            if (flag && comp != null && comp.Stamina.CurLevel >= actualStaminaCost)
            {
                bool flag2 = dinfo.Instigator != null;
                if (flag2)
                {
                    bool flag3 = caster != null && caster.PositionHeld != default(IntVec3) && !caster.Downed;
                    if (flag3)
                    {
                        System.Random random = new System.Random();
                        int rnd = GenMath.RoundRandom(random.Next(0, 100));
                        if (rnd < (ver * 15))
                        {
                            target.TakeDamage(dinfo);
                            MoteMaker.ThrowMicroSparks(target.Position.ToVector3(), map);
                        }
                        target.TakeDamage(dinfo);
                        MoteMaker.ThrowMicroSparks(target.Position.ToVector3(), map);
                        for (int i = 0; i < 8; i++)
                        {
                            IntVec3 intVec = target.PositionHeld + GenAdj.AdjacentCells[i];
                            Pawn cleaveVictim = new Pawn();
                            cleaveVictim = intVec.GetFirstPawn(map);
                            if (cleaveVictim != null && cleaveVictim.Faction != caster.Faction)
                            {
                                cleaveVictim.TakeDamage(dinfo);
                                MoteMaker.ThrowMicroSparks(cleaveVictim.Position.ToVector3(), map);
                                multiplePawns = true;
                                rnd = GenMath.RoundRandom(random.Next(0, 100));
                                if (rnd < (ver * 15))
                                {
                                    cleaveVictim.TakeDamage(dinfo);
                                    MoteMaker.ThrowMicroSparks(cleaveVictim.Position.ToVector3(), map);
                                }
                            }
                        }
                    }
                }
            }
            if (multiplePawns)
            {
                Vector3 angle = TM_Calc.GetVector(caster.DrawPos, target.DrawPos);
                TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_DragonStrike, target.DrawPos, caster.Map, 1.1f, .1f, .01f, .1f, 650, 0, 0, (Quaternion.AngleAxis(-25, Vector3.up) * angle).ToAngleFlat());
                if (comp != null)
                {
                    comp.Stamina.CurLevel -= actualStaminaCost;
                    comp.MightUserXP += (int)(.1f * 180);
                }
            }
        }

        public static void DoAction_SabotagePawn(Pawn targetPawn, Pawn caster, float rnd, int pwrVal, float arcaneDmg, Thing instigator)
        {
            if (Rand.Chance(TM_Calc.GetSpellSuccessChance(caster, targetPawn, true)))
            {
                if (rnd <= .33f)
                {
                    TM_Action.DamageEntities(targetPawn, null, (Rand.Range(8, 15) + pwrVal) * arcaneDmg, TMDamageDefOf.DamageDefOf.TM_ElectricalBurn, instigator);
                }
                else if (rnd <= .66f)
                {
                    if (targetPawn.mindState != null)
                    {
                        targetPawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, "logic circuits sabotaged", true, false, null, true);
                    }
                    else
                    {
                        targetPawn.TryStartAttack(TM_Calc.FindNearbyPawn(targetPawn, 10));
                    }
                }
                else if (rnd <= 1f)
                {
                    int rndCount = Rand.Range(2, 5);
                    for (int j = 0; j < rndCount; j++)
                    {
                        TM_Action.DamageEntities(targetPawn, null, (Rand.Range(3, 5) + pwrVal) * arcaneDmg, TMDamageDefOf.DamageDefOf.TM_ElectricalBurn, instigator);
                    }
                }
            }
            else
            {
                MoteMaker.ThrowText(targetPawn.DrawPos, targetPawn.Map, "TM_ResistedSpell".Translate(), -1);
            }
        }

        public static void DoAction_HealPawn(Pawn caster, Pawn pawn, int bodypartCount, float amountToHeal)
        {
            int num = bodypartCount;
            using (IEnumerator<BodyPartRecord> enumerator = pawn.health.hediffSet.GetInjuredParts().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    BodyPartRecord rec = enumerator.Current;
                    bool flag2 = num > 0;

                    if (flag2)
                    {
                        int num2 = bodypartCount;
                        IEnumerable<Hediff_Injury> injury_hediff = pawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                        Func<Hediff_Injury, bool> partInjured;

                        partInjured = ((Hediff_Injury injury) => injury.Part == rec);

                        foreach (Hediff_Injury current in injury_hediff.Where(partInjured))
                        {
                            bool flag4 = num2 > 0;
                            if (flag4)
                            {
                                bool flag5 = current.BleedRate > 0;
                                if (flag5)
                                {
                                    current.Heal(amountToHeal);
                                    num--;
                                    num2--;
                                }
                            }
                        }

                        foreach (Hediff_Injury current in injury_hediff.Where(partInjured))
                        {
                            bool flag4 = num2 > 0;
                            if (flag4)
                            {
                                bool flag5 = !current.IsPermanent();
                                if (flag5)
                                {
                                    current.Heal(amountToHeal);
                                    num--;
                                    num2--;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static Thing SingleSpawnLoop(Pawn caster, SpawnThings spawnables, IntVec3 position, Map map, int duration, bool temporary, bool hostile = false, Faction spawnableFaction = null)
        {
            bool flag = spawnables.def != null;
            Thing thing = null;
            if (flag)
            {
                Faction faction = TM_Action.ResolveFaction(caster, spawnables, spawnableFaction, hostile);
                bool flag2 = spawnables.def.race != null;
                if (flag2)
                {
                    bool flag3 = spawnables.kindDef == null;
                    if (flag3)
                    {
                        Log.Error("Missing kinddef");
                    }
                    else
                    {
                        thing = TM_Action.SpawnPawn(caster, spawnables, faction, position, duration, map);
                    }
                }
                else
                {
                    ThingDef def = spawnables.def;
                    ThingDef stuff = null;
                    bool madeFromStuff = def.MadeFromStuff;
                    if (madeFromStuff)
                    {
                        stuff = ThingDefOf.Steel;
                    }
                    thing = ThingMaker.MakeThing(def, stuff);
                    if (thing != null)
                    {
                        if (thing.def.defName != "Portfuel")
                        {
                            thing.SetFaction(faction, null);
                        }
                        CompSummoned bldgComp = thing.TryGetComp<CompSummoned>();
                        bldgComp.TicksToDestroy = duration;
                        bldgComp.Temporary = temporary;
                        GenSpawn.Spawn(thing, position, map, Rot4.North, WipeMode.Vanish, false);
                    }
                }
            }
            return thing;
        }

        public static Faction ResolveFaction(Pawn caster, SpawnThings spawnables, Faction spawnAbleFaction, bool hostile = false)
        {
            FactionDef val = FactionDefOf.PlayerColony;
            Faction obj = null;

            if (!hostile)
            {
                return spawnAbleFaction;
            }

            obj = ((caster != null) ? caster.Faction : null);
            Faction val2 = obj;
            if (hostile)
            {
                if (obj != null && !val2.IsPlayer)
                {
                    return val2;
                }
                if (spawnables.factionDef != null)
                {
                    val = spawnables.factionDef;
                }
                if (spawnables.kindDef != null && spawnables.kindDef.defaultFactionType != null)
                {
                    val = spawnables.kindDef.defaultFactionType;
                }
                if (val != null)
                {
                    return FactionUtility.DefaultFactionFrom(val);
                }
                else
                {
                    return Find.FactionManager.RandomEnemyFaction(true, true, true);
                }
            }
            if (caster != null && caster.Faction != null)
            {
                return caster.Faction;
            }
            return Find.FactionManager.AllFactionsVisible.RandomElement();

        }

        public static TMPawnSummoned SpawnPawn(Pawn caster, SpawnThings spawnables, Faction faction, IntVec3 position, int duration, Map map)
        {
            TMPawnSummoned newPawn = (TMPawnSummoned)PawnGenerator.GeneratePawn(spawnables.kindDef, faction);
            newPawn.validSummoning = true;
            newPawn.Spawner = caster;
            newPawn.Temporary = spawnables.temporary;
            newPawn.TicksToDestroy = duration;
            //Faction val = default(Faction);
            //int num;
            //if (newPawn.Faction != Faction.OfPlayerSilentFail)
            //{
            //    Faction obj = null;

            //    obj = ((caster != null) ? caster.Faction : null);

            //    val = obj;
            //    num = ((obj != null) ? 1 : 0);
            //}
            //else
            //{
            //    num = 0;
            //}
            //if (num != 0)
            //{
            //    newPawn.SetFaction(val, null);
            //}
            GenSpawn.Spawn(newPawn, position, map, 0);

            if (newPawn.Faction != null && newPawn.Faction != Faction.OfPlayer)
            {
                Lord lord = null;
                if (newPawn.Map.mapPawns.SpawnedPawnsInFaction(faction).Any((Pawn p) => p != newPawn))
                {
                    Predicate<Thing> validator = (Thing p) => p != newPawn && ((Pawn)p).GetLord() != null;
                    Pawn p2 = (Pawn)GenClosest.ClosestThing_Global(newPawn.Position, newPawn.Map.mapPawns.SpawnedPawnsInFaction(faction), 99999f, validator, null);
                    lord = p2.GetLord();
                }
                bool flag4 = lord == null;
                if (flag4)
                {
                    LordJob_DefendPoint lordJob = new LordJob_DefendPoint(newPawn.Position);
                    lord = LordMaker.MakeNewLord(faction, lordJob, newPawn.Map, null);
                }
                else
                {
                    try
                    {
                        newPawn.mindState.duty = new PawnDuty(DutyDefOf.Defend);
                    }
                    catch
                    {
                        Log.Message("error attempting to assign a duty to summoned object");
                    }
                }
                lord.AddPawn(newPawn);
            }
            return newPawn;
        }

        public static Pawn PolymorphPawn(Pawn caster, Pawn original, Pawn polymorphFactionPawn, SpawnThings spawnables, IntVec3 position, bool temporary, int duration, Faction fac = null)
        {
            Pawn polymorphPawn = null;
            bool flag = spawnables.def != null;
            if (flag)
            {
                Faction faction = TM_Action.ResolveFaction(polymorphFactionPawn, spawnables, fac);
                bool flag2 = spawnables.def.race != null;
                if (flag2)
                {
                    bool flag3 = spawnables.kindDef == null;
                    if (flag3)
                    {
                        Log.Error("Missing kinddef");
                    }
                    else
                    {
                        try
                        {
                            if (ModCheck.Validate.GiddyUp.Core_IsInitialized())
                            {
                                ModCheck.GiddyUp.ForceDismount(original);
                            }
                        }
                        catch
                        {

                        }

                        Pawn newPawn = new Pawn();

                        newPawn = (Pawn)PawnGenerator.GeneratePawn(spawnables.kindDef, faction);
                        newPawn.AllComps.Add(new CompPolymorph());
                        CompPolymorph compPoly = newPawn.GetComp<CompPolymorph>();
                        //CompProperties_Polymorph props = new CompProperties_Polymorph();
                        //compPoly.Initialize(props);

                        if (compPoly != null)
                        {
                            compPoly.ParentPawn = newPawn;
                            compPoly.Spawner = caster;
                            compPoly.Temporary = temporary;
                            compPoly.TicksToDestroy = duration;
                            compPoly.Original = original;
                        }
                        else
                        {
                            Log.Message("CompPolymorph was null.");
                        }

                        try
                        {
                            GenSpawn.Spawn(newPawn, position, original.Map);
                            polymorphPawn = newPawn;

                            polymorphPawn.drafter = new Pawn_DraftController(polymorphPawn);
                            polymorphPawn.equipment = new Pawn_EquipmentTracker(polymorphPawn);
                            polymorphPawn.story = new Pawn_StoryTracker(polymorphPawn);
                            if (original.workSettings != null)
                            {
                                polymorphPawn.workSettings = new Pawn_WorkSettings(polymorphPawn);
                                DefMap<WorkTypeDef, int> priorities = Traverse.Create(root: original.workSettings).Field(name: "priorities").GetValue<DefMap<WorkTypeDef, int>>();
                                priorities = new DefMap<WorkTypeDef, int>();
                                priorities.SetAll(0);
                                Traverse.Create(root: polymorphPawn.workSettings).Field(name: "priorities").SetValue(priorities);
                            }

                            //polymorphPawn.apparel = new Pawn_ApparelTracker(polymorphPawn);
                            //polymorphPawn.mindState = new Pawn_MindState(polymorphPawn);
                            //polymorphPawn.thinker = new Pawn_Thinker(polymorphPawn);
                            //polymorphPawn.jobs = new Pawn_JobTracker(polymorphPawn);
                            //polymorphPawn.records = new Pawn_RecordsTracker(polymorphPawn);
                            //polymorphPawn.skills = new Pawn_SkillTracker(polymorphPawn);
                            //PawnComponentsUtility.AddAndRemoveDynamicComponents(polymorphPawn, true);

                            polymorphPawn.Name = original.Name;
                            polymorphPawn.gender = original.gender;

                            if (original.health.hediffSet.HasHediff(HediffDef.Named("TM_SoulBondPhysicalHD")) || original.health.hediffSet.HasHediff(HediffDef.Named("TM_SoulBondMentalHD")))
                            {
                                TM_Action.TransferSoulBond(original, polymorphPawn);
                            }
                        }
                        catch (NullReferenceException ex)
                        {
                            Log.Message("TM_Exception".Translate(
                                caster.LabelShort,
                                ex.ToString()
                                ));
                            polymorphPawn = null;
                        }
                        if (polymorphPawn != null && newPawn.Faction != null && newPawn.Faction != Faction.OfPlayer)
                        {
                            Lord lord = null;
                            if (newPawn.Map.mapPawns.SpawnedPawnsInFaction(faction).Any((Pawn p) => p != newPawn))
                            {
                                Predicate<Thing> validator = (Thing p) => p != newPawn && ((Pawn)p).GetLord() != null;
                                Pawn p2 = (Pawn)GenClosest.ClosestThing_Global(newPawn.Position, newPawn.Map.mapPawns.SpawnedPawnsInFaction(faction), 99999f, validator, null);
                                lord = p2.GetLord();
                            }
                            bool flag4 = lord == null;
                            if (flag4)
                            {
                                LordJob_DefendPoint lordJob = new LordJob_DefendPoint(newPawn.Position);
                                lord = LordMaker.MakeNewLord(faction, lordJob, original.Map, null);
                            }
                            try
                            {
                                lord.AddPawn(newPawn);
                            }
                            catch (NullReferenceException ex)
                            {
                                if (lord != null)
                                {
                                    LordJob_AssaultColony lordJob = new LordJob_AssaultColony(faction, false, false, false, false);
                                    lord = LordMaker.MakeNewLord(faction, lordJob, original.Map, null);                                    
                                }
                            }

                        }
                    }
                }
                else
                {
                    Log.Message("Missing race");
                }
            }
            return polymorphPawn;
        }

        public static void TransferSoulBond(Pawn bondedPawn, Pawn polymorphedPawn)
        {
            Hediff bondHediff = null;
            bondHediff = bondedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_SoulBondPhysicalHD"), false);
            if (bondHediff != null)
            {
                HediffComp_SoulBondHost comp = bondHediff.TryGetComp<HediffComp_SoulBondHost>();
                if (comp != null)
                {
                    comp.polyHost = polymorphedPawn;
                }
            }
            bondHediff = null;

            bondHediff = bondedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_SoulBondMentalHD"), false);
            if (bondHediff != null)
            {
                HediffComp_SoulBondHost comp = bondHediff.TryGetComp<HediffComp_SoulBondHost>();
                if (comp != null)
                {
                    comp.polyHost = polymorphedPawn;
                }
            }
        }

        public static SpawnThings AssignRandomCreatureDef(SpawnThings spawnthing, int combatPowerMin, int combatPowerMax)
        {
            IEnumerable<PawnKindDef> enumerable = from def in DefDatabase<PawnKindDef>.AllDefs
                                                  where (def.combatPower >= combatPowerMin && def.combatPower <= combatPowerMax && def.race != null && def.race.race != null && def.race.race.thinkTreeMain.ToString() == "Animal")
                                                  select def;

            foreach (PawnKindDef current in enumerable)
            {
                //Log.Message("random creature includes " + current.defName + " race of " + current.race.defName);
            }
            PawnKindDef assignDef = enumerable.RandomElement();
            spawnthing.kindDef = assignDef;
            spawnthing.def = assignDef.race;
            return spawnthing;
        }

        public static void DamageEntities(Thing victim, BodyPartRecord hitPart, float amt, DamageDef type, Thing instigator)
        {
            DamageInfo dinfo;
            dinfo = new DamageInfo(type, amt, 0, (float)-1, instigator, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown);
            dinfo.SetAllowDamagePropagation(false);
            victim.TakeDamage(dinfo);
        }

        public static void DamageEntities(Thing victim, BodyPartRecord hitPart, float amt, float armorPenetration, DamageDef type, Thing instigator)
        {
            DamageInfo dinfo;
            dinfo = new DamageInfo(type, amt, armorPenetration, (float)-1, instigator, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown);
            dinfo.SetAllowDamagePropagation(false);
            victim.TakeDamage(dinfo);
        }

        public static void DamageUndead(Pawn undead, float amt, Thing instigator)
        {
            DamageInfo dinfo = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_Holy, Rand.Range(amt * 1f, amt * 1.4f), 1, -1, instigator);
            for (int i = 0; i < 4; i++)
            {
                if (undead != null && !undead.Destroyed && !undead.Dead)
                {
                    TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_Holy"), undead.DrawPos, undead.Map, Rand.Range(.5f, .8f), .1f, (.1f * i), .5f - (.1f * i), Rand.Range(-400, 400), Rand.Range(.5f, 1f), Rand.Range(0, 360), Rand.Range(0, 360));
                }
            }
            SoundInfo info = SoundInfo.InMap(new TargetInfo(undead.Position, undead.Map, false), MaintenanceType.None);
            TorannMagicDefOf.TM_FireWooshSD.PlayOneShot(info);
            TM_Action.DoEffecter(TorannMagicDefOf.TM_HolyImplosion, undead.Position, undead.Map);
            undead.TakeDamage(dinfo);
        }

        public static void DoEffecter(EffecterDef effecterDef, IntVec3 position, Map map)
        {
            Effecter effecter = effecterDef.Spawn();
            effecter.Trigger(new TargetInfo(position, map, false), new TargetInfo(position, map, false));
            effecter.Cleanup();
        }

        /// <summary>
        /// Actions used to perform Chronomancer ability "Recall"
        /// Loaded in static to allow immediate execution when downed or dead
        /// </summary>
        /// <param name="pawn"></param>
        /// <param name="comp"></param>
        /// <param name="deathTrigger"></param>
        public static void DoRecall(Pawn pawn, CompAbilityUserMagic comp, bool deathTrigger)
        {
            try
            {
                if (ModCheck.Validate.GiddyUp.Core_IsInitialized())
                {
                    ModCheck.GiddyUp.ForceDismount(pawn);
                }
            }
            catch
            {

            }
            MoteMaker.ThrowSmoke(pawn.DrawPos, pawn.Map, 1.4f);
            TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_AlterFate, pawn.DrawPos, pawn.Map, 1.6f, .2f, .1f, .8f, -500, 0, 0, Rand.Range(0, 360));
            Effecter RecallFromEffect = TorannMagicDefOf.TM_RecallFromED.Spawn();
            RecallFromEffect.Trigger(new TargetInfo(pawn), new TargetInfo(pawn));
            RecallFromEffect.Cleanup();
            RecallHediffs(pawn, comp);
            RecallNeeds(pawn, comp);
            RecallPosition(pawn, comp);
            ResetPowers(pawn, comp);
            comp.recallSet = false;
            comp.recallSpell = false;
            comp.RemovePawnAbility(TorannMagicDefOf.TM_Recall);
            TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_AlterFate, pawn.DrawPos, pawn.Map, 1.6f, .2f, .1f, .8f, 500, 0, 0, Rand.Range(0, 360));
            Effecter RecallToEffect = TorannMagicDefOf.TM_RecallToED.Spawn();
            RecallToEffect.Trigger(new TargetInfo(pawn), new TargetInfo(pawn));
            RecallToEffect.Cleanup();
            HealthUtility.AdjustSeverity(pawn, TorannMagicDefOf.TM_HediffInvulnerable, .02f);
            if (pawn.Dead)
            {
                ResurrectionUtility.Resurrect(pawn);
                deathTrigger = true;
            }
            if (deathTrigger && Rand.Chance(.5f))
            {
                pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Psychotic);
            }
        }

        private static void RecallHediffs(Pawn pawn, CompAbilityUserMagic comp)
        {
            if (comp.recallInjuriesList != null && comp.recallHediffList != null)
            {
                for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
                {
                    if (!pawn.health.hediffSet.hediffs[i].IsPermanent() && pawn.health.hediffSet.hediffs[i].def != TorannMagicDefOf.TM_MagicUserHD && !pawn.health.hediffSet.hediffs[i].def.defName.Contains("TM_HediffEnchantment") && !pawn.health.hediffSet.hediffs[i].def.defName.Contains("TM_Artifact"))
                    {
                        if (!(pawn.health.hediffSet.hediffs[i] is Hediff_MissingPart) && !(pawn.health.hediffSet.hediffs[i] is Hediff_AddedPart))
                        {
                            //Log.Message("removing " + pawn.health.hediffSet.hediffs[i].Label + " at severity " + pawn.health.hediffSet.hediffs[i].Severity);
                            Hediff hediff = pawn.health.hediffSet.hediffs[i];
                            pawn.health.RemoveHediff(hediff);
                            i--;
                        }
                    }
                }
                for (int i = 0; i < comp.recallHediffList.Count; i++)
                {
                    //Log.Message("adding " + comp.recallHediffList[i].Label + " at severity " + comp.recallHediffList[i].Severity);
                    pawn.health.AddHediff(comp.recallHediffList[i]);
                }
                for (int i = 0; i < comp.recallInjuriesList.Count; i++)
                {
                    //Log.Message("adding injury " + comp.recallInjuriesList[i].Label + " at severity " + comp.recallInjuriesList[i].Severity);
                    pawn.health.AddHediff(comp.recallInjuriesList[i]);
                }
                comp.recallHediffList.Clear();
                comp.recallInjuriesList.Clear();
            }

        }

        private static void RecallNeeds(Pawn pawn, CompAbilityUserMagic comp)
        {
            for (int i = 0; i < pawn.needs.AllNeeds.Count; i++)
            {
                bool hasNeed = false;
                for (int j = 0; j < comp.recallNeedValues.Count; j++)
                {
                    if (comp.recallNeedDefnames[j] == pawn.needs.AllNeeds[i].def.defName)
                    {
                        //Log.Message("setting " + pawn.needs.AllNeeds[i].def.defName + " from " + pawn.needs.AllNeeds[i].CurLevel + " to " + comp.recallNeedValues[j]);
                        pawn.needs.AllNeeds[i].CurLevel = comp.recallNeedValues[j];
                        hasNeed = true;
                    }
                }
                if (!hasNeed)
                {
                    //Log.Message("removing need " + pawn.needs.AllNeeds[i].def.defName);
                    pawn.needs.AllNeeds.Remove(pawn.needs.AllNeeds[i]);
                }
            }
            pawn.needs.AddOrRemoveNeedsAsAppropriate();
            comp.recallNeedDefnames.Clear();
            comp.recallNeedValues.Clear();
        }

        private static void RecallPosition(Pawn pawn, CompAbilityUserMagic comp)
        {
            bool draftFlag = pawn.Drafted;
            bool selectFlag = Find.Selector.IsSelected(pawn);
            pawn.DeSpawn();
            GenPlace.TryPlaceThing(pawn, comp.recallPosition, comp.recallMap, ThingPlaceMode.Near);
            pawn.drafter.Drafted = draftFlag;
            if (selectFlag)
            {
                CameraJumper.TryJumpAndSelect(pawn);
            }
            comp.recallPosition = default(IntVec3);
        }

        private static void ResetPowers(Pawn pawn, CompAbilityUserMagic comp)
        {
            foreach (PawnAbility current in comp.AbilityData.Powers)
            {
                if (current.Def != TorannMagicDefOf.TM_Recall && current.Def != TorannMagicDefOf.TM_TimeMark)
                {
                    current.CooldownTicksLeft = 0;
                }
                else if (current.Def == TorannMagicDefOf.TM_TimeMark)
                {
                    current.CooldownTicksLeft = Mathf.RoundToInt(current.MaxCastingTicks * comp.coolDown);
                }
            }
        }

        public static void ConsumeManaXP(Pawn p, float mp, float xpMultiplier = 1f, bool applyArcaneWeakness = true)
        {
            if (p != null)
            {
                CompAbilityUserMagic comp = p.GetComp<CompAbilityUserMagic>();
                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                if (comp != null && comp.Mana != null)
                {
                    int xpNum = (int)((mp * 300) * comp.xpGain * settingsRef.xpMultiplier * xpMultiplier);
                    comp.MagicUserXP += xpNum;
                    MoteMaker.ThrowText(p.DrawPos, p.MapHeld, "XP +" + xpNum, -1f);
                    mp *= comp.mpCost;
                    if (applyArcaneWeakness)
                    {
                        comp.Mana.UseMagicPower(mp);
                    }
                    else
                    {
                        comp.Mana.CurLevel = Mathf.Clamp(comp.Mana.CurLevel - mp, 0, comp.Mana.MaxLevel);
                    }
                }
            }
        }

        public static void PawnActionDelay(Pawn pawn, int duration, LocalTargetInfo target, Verb verb)
        {
            //pawn.stances.SetStance(new Stance_Busy(duration, target, verb));
            pawn.stances.SetStance(new Stance_Cooldown(duration, target, verb));
        }

        public static void ForceFactionDiscoveryAndRelation(FactionDef fDef)
        {
            foreach(FactionDef allDefs in DefDatabase<FactionDef>.AllDefs)
            {
                if(allDefs == fDef)
                {
                    List<Faction> allFactions = Find.FactionManager.AllFactions.ToList();
                    bool flagList = false;
                    bool flagRelation = false;
                    for(int i = 0; i < allFactions.Count;i++)
                    {
                        if (allFactions[i].def.defName == fDef.defName)
                        {
                            flagList = true;
                            if(allFactions[i].RelationWith(Faction.OfPlayer, true) != null)
                            {
                                flagRelation = true;
                            }
                        }
                    }
                    if(!flagList)
                    {
                        Faction f = FactionGenerator.NewGeneratedFaction(fDef);
                        Find.FactionManager.Add(f);                        
                        //foreach (Map map in Find.Maps)
                        //{
                        //    map.pawnDestinationReservationManager.RegisterFaction(f);
                        //}
                    }
                    if(!flagRelation)
                    {
                        Faction f = Find.FactionManager.FirstFactionOfDef(fDef);
                        if (fDef.CanEverBeNonHostile)
                        {
                            f.TryAffectGoodwillWith(Faction.OfPlayerSilentFail, -200, false, false, null, null);
                        }
                        else
                        {
                            f.TryAffectGoodwillWith(Faction.OfPlayerSilentFail, fDef.startingGoodwill.RandomInRange, false, false, null, null);
                        }
                    }
                }
            }
        }

        public static void PromoteWanderer(Pawn pawn)
        {
            RemoveTrait(pawn, TorannMagicDefOf.Gifted);
            pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Wanderer, 4, false));
            pawn.needs.AddOrRemoveNeedsAsAppropriate();
        }

        public static void PromoteWayfarer(Pawn pawn)
        {
            RemoveTrait(pawn, TorannMagicDefOf.PhysicalProdigy);
            pawn.story.traits.GainTrait(new Trait(TorannMagicDefOf.TM_Wayfarer, 4, false));
            pawn.needs.AddOrRemoveNeedsAsAppropriate();
        }

        public static void RemoveTrait(Pawn pawn, TraitDef trait)
        {
            List<Trait> allTraits = pawn.story.traits.allTraits;

            for (int i = 0; i < allTraits.Count; i++)
            {
                if (allTraits[i].def == trait)
                {
                    allTraits.Remove(allTraits[i]);
                    break;
                }
            }
        }

        public static void TransmutateEffects(IntVec3 position, Pawn p)
        {
            Vector3 rndPos = position.ToVector3Shifted();
            MoteMaker.ThrowHeatGlow(position, p.Map, 1f);
            for (int i = 0; i < 6; i++)
            {
                rndPos.x += Rand.Range(-.5f, .5f);
                rndPos.z += Rand.Range(-.5f, .5f);
                rndPos.y += Rand.Range(.3f, 1.3f);
                MoteMaker.ThrowSmoke(rndPos, p.Map, Rand.Range(.7f, 1.1f));
                MoteMaker.ThrowLightningGlow(position.ToVector3Shifted(), p.Map, 1.4f);
            }
        }

        public static bool DoWildSurge(Pawn p, CompAbilityUserMagic comp, MagicAbility ability, TMAbilityDef abilityDef, LocalTargetInfo target, bool canDoBad = true)
        {
            bool completeJob = false;

            float rnd = Rand.Range(0f, 1f);
            float pwrVal = (comp.MagicData.MagicPowerSkill_ChaosTradition.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_ChaosTradition_pwr").level * .1f);
            float good = .2f + pwrVal;
            float bad = (.5f - pwrVal) + good;
            string surgeText = "";
            if (rnd < good)
            {
                int rndGood = Rand.RangeInclusive(0, 2);
                switch (rndGood)
                {
                    case 0:
                        List<Pawn> nearPawns = TM_Calc.FindPawnsNearTarget(p, 4, p.Position, false);
                        if (nearPawns != null && nearPawns.Count > 0)
                        {
                            for (int i = 0; i < nearPawns.Count; i++)
                            {
                                TM_Action.DoAction_HealPawn(p, nearPawns[i], 1, 10);
                            }
                        }
                        TM_MoteMaker.MakePowerBeamMote(p.Position, p.Map, 4 * 6f, 1.2f, 2f, .3f, 1f);
                        surgeText = "Healing Wave";
                        completeJob = true;
                        break;
                    case 1:
                        comp.Mana.CurLevel += comp.ActualManaCost(abilityDef);
                        TM_MoteMaker.ThrowRegenMote(p.DrawPos, p.Map, 1.2f);
                        surgeText = "Zero Cost";
                        completeJob = true;
                        break;
                    case 2:
                        ability.CooldownTicksLeft = 0;
                        TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_PowerWave"), p.DrawPos, p.Map, .8f, .2f, .1f, .1f, 0, 1f, 0, Rand.Chance(.5f) ? 0 : 180);
                        surgeText = "Ability Reset";
                        completeJob = true;
                        break;
                }
            }
            else if ((rnd < bad) && canDoBad)
            {
                int rndBad = Rand.RangeInclusive(0, 3);
                switch (rndBad)
                {
                    case 0:
                        GenExplosion.DoExplosion(p.Position, p.Map, 5f, DamageDefOf.Bomb, p, Rand.Range(8, 12), 1f, null, null, null, null, null, 0, 1, false, null, 0, 1, 0, true);
                        surgeText = "Explosion";
                        break;
                    case 1:
                        List<Pawn> allPawns = p.Map.mapPawns.AllPawnsSpawned;
                        for (int i = 0; i < allPawns.Count; i++)
                        {
                            if (TM_Calc.IsMagicUser(allPawns[i]))
                            {
                                CompAbilityUserMagic apComp = allPawns[i].TryGetComp<CompAbilityUserMagic>();
                                if (apComp != null)
                                {
                                    for (int j = 0; j < apComp.AbilityData.AllPowers.Count; j++)
                                    {
                                        MagicAbility ma = apComp.AbilityData.AllPowers[j] as MagicAbility;
                                        if (ma != null)
                                        {
                                            ma.CooldownTicksLeft = (int)(ma.Def.MainVerb.SecondsToRecharge * 60);
                                        }
                                    }
                                }
                            }
                        }
                        surgeText = "Delay Magic";
                        break;
                    case 2:
                        HealthUtility.AdjustSeverity(p, TorannMagicDefOf.TM_MuteHD, Rand.Range(5, 8));
                        TM_Action.TransmutateEffects(p.Position, p);
                        surgeText = "Muted";
                        break;
                    case 3:
                        List<IntVec3> cellList = new List<IntVec3>();
                        cellList.Clear();
                        cellList = GenRadial.RadialCellsAround(p.Position, 6, true).ToList();
                        for (int i = 0; i < cellList.Count; i++)
                        {
                            if (cellList[i].IsValid && cellList[i].InBounds(p.Map))
                            {
                                List<Thing> thingList = cellList[i].GetThingList(p.Map);
                                if (thingList != null && thingList.Count > 0)
                                {
                                    for (int j = 0; j < thingList.Count; j++)
                                    {
                                        Pawn pawn = thingList[j] as Pawn;
                                        if (pawn != null && pawn != p)
                                        {
                                            RemoveFireAt(thingList[j].Position, pawn.Map);
                                            if (Rand.Chance(TM_Calc.GetSpellSuccessChance(p, pawn, false)))
                                            {
                                                IntVec3 targetCell = pawn.Position;
                                                targetCell.z++;
                                                DoTimeDelayLaunch(targetCell, p, pawn, 1, Mathf.RoundToInt(Rand.Range(1400, 1800)));
                                            }
                                            else
                                            {
                                                MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, "TM_ResistedSpell".Translate(), -1);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        surgeText = "Time Field";
                        break;
                }
            }
            else
            {
                int rndNeutral = Rand.RangeInclusive(0, 3);
                switch (rndNeutral)
                {
                    case 0:
                        ModOptions.Constants.SetPawnInFlight(true);
                        bool draftFlag = p.Drafted;
                        IntVec3 initPos = p.Position;
                        Map map = p.Map;                                               
                        IntVec3 moveTo = TM_Calc.TryFindSafeCell(p, p.Position, 16, 3, 10);
                        p.DeSpawn();
                        if (moveTo != default(IntVec3))
                        {
                            GenSpawn.Spawn(p, moveTo, map);
                        }
                        else
                        {
                            GenSpawn.Spawn(p, initPos, map);
                        }
                        p.drafter.Drafted = draftFlag;                        
                        if (p.IsColonist)
                        {
                            p.drafter.Drafted = true;
                            CameraJumper.TryJumpAndSelect(p);
                        }
                        surgeText = "Teleportation";
                        ModOptions.Constants.SetPawnInFlight(false);
                        break;
                    case 1:
                        if(target != null && target != p && target.Cell != null && target.Cell.IsValid && target.Cell != default(IntVec3))
                        {
                            float angle = (Quaternion.AngleAxis(90, Vector3.up) * TM_Calc.GetVector(p.Position, target.Cell)).ToAngleFlat();
                            for (int i = 0; i < 6; i++)
                            {                                
                                TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_Flowers, p.DrawPos, p.Map, Rand.Range(.3f, 1f), Rand.Range(1f, 1.5f), .1f, Rand.Range(.2f, .5f), Rand.Range(-100,100),Rand.Range(1,3), angle, Rand.Range(0,360));
                                MoteMaker.ThrowLightningGlow(p.DrawPos, p.Map, Rand.Range(.5f, 1f));
                                TM_MoteMaker.ThrowGenericMote(ThingDefOf.Mote_Smoke, p.DrawPos, p.Map, Rand.Range(.5f, 1f), 3f, .1f, Rand.Range(.5f, 1f), Rand.Range(-20, 20), Rand.Range(.5f, .8f), angle, Rand.Range(0, 360));
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                float angle = Rand.Range(0, 360);
                                TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_Flowers, p.DrawPos, p.Map, Rand.Range(.3f, 1f), Rand.Range(1f, 1.5f), .1f, Rand.Range(.2f, .5f), Rand.Range(-100, 100), Rand.Range(1, 3), angle, Rand.Range(0, 360));
                                MoteMaker.ThrowLightningGlow(p.DrawPos, p.Map, Rand.Range(.5f, 1f));
                                TM_MoteMaker.ThrowGenericMote(ThingDefOf.Mote_Smoke, p.DrawPos, p.Map, Rand.Range(.5f, 1f), 3f, .1f, Rand.Range(.5f, 1f), Rand.Range(-20, 20), Rand.Range(.5f, .8f), angle, Rand.Range(0, 360));
                            }
                        }
                        surgeText = "Flowers";
                        break;
                    case 2:
                        if (p.Map != null)
                        {
                            List<Pawn> allPawns = p.Map.mapPawns.AllPawnsSpawned;
                            if (allPawns != null && allPawns.Count > 0)
                            {
                                for (int i = 0; i < allPawns.Count; i++)
                                {
                                    if(allPawns[i].needs != null && allPawns[i].needs.food != null)
                                    {
                                        HealthUtility.AdjustSeverity(allPawns[i], HediffDefOf.FoodPoisoning, Rand.Range(.3f, .7f));
                                    }
                                }
                            }
                        }
                        surgeText = "Mass Poison";
                        break;
                    case 3:
                        if (p.Map != null)
                        {
                            List<Pawn> allPawns = p.Map.mapPawns.AllPawnsSpawned;
                            if (allPawns != null && allPawns.Count > 0)
                            {
                                for (int i = 0; i < allPawns.Count; i++)
                                {
                                    if (allPawns[i].needs != null && allPawns[i].needs.rest != null)
                                    {
                                        Need need = allPawns[i].needs.TryGetNeed(NeedDefOf.Rest);
                                        if(need != null)
                                        {
                                            need.CurLevel = 0;
                                        }
                                    }
                                }
                            }
                        }
                        surgeText = "Mass Exhaustion";
                        break;
                }
            }
            MoteMaker.ThrowText(p.DrawPos, p.Map, "TM_WildSurge".Translate(surgeText), -1);

            return completeJob;
        }

        public static void DoTimeDelayLaunch(IntVec3 targetCell, Pawn caster, Pawn pawn, int force, int duration)
        {
            bool flag = targetCell != null && targetCell != default(IntVec3);
            if (flag)
            {
                if (pawn != null && pawn.Position.IsValid && pawn.Spawned && pawn.Map != null && !pawn.Downed && !pawn.Dead)
                {
                    if (ModCheck.Validate.GiddyUp.Core_IsInitialized())
                    {
                        ModCheck.GiddyUp.ForceDismount(pawn);
                    }
                    FlyingObject_TimeDelay flyingObject = (FlyingObject_TimeDelay)GenSpawn.Spawn(ThingDef.Named("FlyingObject_TimeDelay"), pawn.Position, pawn.Map);
                    flyingObject.speed = .01f;
                    flyingObject.duration = duration;
                    flyingObject.Launch(caster, targetCell, pawn);
                }
            }       
        }

        private static void RemoveFireAt(IntVec3 position, Map map)
        {
            List<Thing> thingList = position.GetThingList(map);
            if (thingList != null && thingList.Count > 0)
            {
                for (int i = 0; i < thingList.Count; i++)
                {
                    if (thingList[i].def == ThingDefOf.Fire)
                    {
                        //Log.Message("removing fire at " + position);
                        MoteMaker.ThrowHeatGlow(position, map, .6f);
                        thingList[i].Destroy(DestroyMode.Vanish);
                        i--;
                    }
                }
            }
        }

        public static void DisplayShieldHit(Pawn shieldedPawn, DamageInfo dinfo)
        {
            DisplayShield(shieldedPawn, dinfo);
        }

        private static void DisplayShield(Pawn shieldedPawn, DamageInfo dinfo)
        {
            Vector3 impactAngleVect;
            SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot(new TargetInfo(shieldedPawn.Position, shieldedPawn.Map, false));
            impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
            Vector3 loc = shieldedPawn.TrueCenter() + impactAngleVect.RotatedBy(180f) * 0.5f;
            float num = Mathf.Min(10f, 2f + (float)dinfo.Amount / 10f);
            MoteMaker.MakeStaticMote(loc, shieldedPawn.Map, ThingDefOf.Mote_ExplosionFlash, num);
            int num2 = (int)num;
            for (int i = 0; i < num2; i++)
            {
                MoteMaker.ThrowDustPuff(loc, shieldedPawn.Map, Rand.Range(0.8f, 1.2f));
                DrawShieldHit(shieldedPawn, dinfo.Amount, impactAngleVect);
            }
        }

        private static void DrawShieldHit(Pawn shieldedPawn, float magnitude, Vector3 impactAngleVect)
        {
            bool flag = !shieldedPawn.Dead && !shieldedPawn.Downed;
            if (flag)
            {
                float num = Mathf.Lerp(1.2f, 1.55f, magnitude);
                Vector3 vector = shieldedPawn.Drawer.DrawPos;
                vector.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);

                float angle = (float)Rand.Range(0, 360);
                Vector3 s = new Vector3(1.7f, 1f, 1.7f);
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
                if (shieldedPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_HediffShield) || shieldedPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_HTLShieldHD))
                {
                    Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.whiteShieldMat, 0);
                }
                else if (shieldedPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DemonScornHD) || shieldedPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DemonScornHD_I) || shieldedPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DemonScornHD_II) || shieldedPawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DemonScornHD_III))
                {
                    Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.demonShieldMat, 0);
                }
                else
                {
                    Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.manaShieldMat, 0);
                }
            }
        }
    }
}
