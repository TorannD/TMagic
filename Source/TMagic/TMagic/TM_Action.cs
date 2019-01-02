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

namespace TorannMagic
{
    public static class TM_Action
    {

        public static void DoMeleeReversal(DamageInfo dinfo, Pawn reflectingPawn)
        {
            Thing instigator = dinfo.Instigator;

            if (instigator is Pawn)
            {
                Pawn meleePawn = instigator as Pawn;
                if (dinfo.Weapon.IsMeleeWeapon || dinfo.WeaponBodyPartGroup != null)
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
                if (!dinfo.Weapon.IsMeleeWeapon && dinfo.WeaponBodyPartGroup == null)
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
                if (!dinfo.Weapon.IsMeleeWeapon && dinfo.WeaponBodyPartGroup == null)
                {
                    Pawn randomTarget = null;
                    randomTarget = TM_Calc.FindNearbyEnemy(reflectingPawn, (int)maxRange);
                    if (randomTarget != null)
                    {
                        TM_CopyAndLaunchProjectile.CopyAndLaunchThing(shooterPawn.equipment.PrimaryEq.PrimaryVerb.verbProps.defaultProjectile, reflectingPawn, randomTarget, randomTarget, ProjectileHitFlags.All, null);
                    }
                }
            }
            if(instigator is Building)
            {
                Building turret = instigator as Building;
                ThingDef projectile = null;

                if (turret.def.building.turretGunDef != null)
                {
                    ThingDef turretGun = turret.def.building.turretGunDef;
                    for (int i = 0; i < turretGun.Verbs.Count; i++)
                    {
                        if(turretGun.Verbs[i].defaultProjectile != null)
                        {
                            projectile = turretGun.Verbs[i].defaultProjectile;
                        }
                    }
                }

                if (projectile != null)
                {
                    Thing target = null;
                    if((turret.Position - reflectingPawn.Position).LengthHorizontal <= maxRange)
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

        public static void DoAction_TechnoWeaponCopy(Pawn caster, Thing thing)
        {
            CompAbilityUserMagic comp = caster.TryGetComp<CompAbilityUserMagic>();

            if (thing != null && thing.def != null && thing.def.IsRangedWeapon && thing.def.techLevel >= TechLevel.Industrial && thing.def.Verbs.FirstOrDefault().verbClass.ToString() == "Verse.Verb_Shoot")
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

        public static Thing SingleSpawnLoop(Pawn caster, SpawnThings spawnables, IntVec3 position, Map map, int duration, bool temporary)
        {
            bool flag = spawnables.def != null;
            Thing thing = null;
            if (flag)
            {
                Faction faction = TM_Action.ResolveFaction(caster, spawnables);
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
                        TM_Action.SpawnPawn(caster, spawnables, faction, position);
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

        public static Faction ResolveFaction(Pawn caster, SpawnThings spawnables)
        {
            FactionDef val = FactionDefOf.PlayerColony;
            Faction obj = null;

            obj = ((caster != null) ? caster.Faction : null);            
            Faction val2 = obj;
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
            return FactionUtility.DefaultFactionFrom(val);
        }

        public static PawnSummoned SpawnPawn(Pawn caster, SpawnThings spawnables, Faction faction, IntVec3 position)
        {
            PawnSummoned newPawn = (PawnSummoned)PawnGenerator.GeneratePawn(spawnables.kindDef, faction);
            newPawn.Spawner = caster;
            newPawn.Temporary = spawnables.temporary;
            Faction val = default(Faction);
            int num;
            if (newPawn.Faction != Faction.OfPlayerSilentFail)
            {
                Faction obj = null;

                obj = ((caster != null) ? caster.Faction : null);
                
                val = obj;
                num = ((obj != null) ? 1 : 0);
            }
            else
            {
                num = 0;
            }
            if (num != 0)
            {
                newPawn.SetFaction(val, null);
            }
            GenSpawn.Spawn(newPawn, position, Find.CurrentMap, 0);

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

        public static Pawn PolymorphPawn(Pawn caster, Pawn original, Pawn polymorphFactionPawn, SpawnThings spawnables, IntVec3 position, bool temporary, int duration)
        {
            Pawn polymorphPawn = null;
            bool flag = spawnables.def != null;
            if (flag)
            {
                Faction faction = TM_Action.ResolveFaction(polymorphFactionPawn, spawnables);
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
                        Pawn newPawn = new Pawn();
                        
                        newPawn = (Pawn)PawnGenerator.GeneratePawn(spawnables.kindDef, faction);
                        newPawn.AllComps.Add(new CompPolymorph());
                        CompPolymorph compPoly = newPawn.GetComp<CompPolymorph>();
                        CompProperties_Polymorph props = new CompProperties_Polymorph();
                        compPoly.Initialize(props);
                        
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

                            //polymorphPawn.apparel = new Pawn_ApparelTracker(polymorphPawn);
                            //polymorphPawn.mindState = new Pawn_MindState(polymorphPawn);
                            //polymorphPawn.thinker = new Pawn_Thinker(polymorphPawn);
                            //polymorphPawn.jobs = new Pawn_JobTracker(polymorphPawn);
                            //polymorphPawn.records = new Pawn_RecordsTracker(polymorphPawn);
                            //polymorphPawn.skills = new Pawn_SkillTracker(polymorphPawn);
                            PawnComponentsUtility.AddAndRemoveDynamicComponents(polymorphPawn, true);

                            polymorphPawn.Name = original.Name;
                            polymorphPawn.gender = original.gender;

                            if (original.health.hediffSet.HasHediff(HediffDef.Named("TM_SoulBondPhysicalHD")) || original.health.hediffSet.HasHediff(HediffDef.Named("TM_SoulBondMentalHD")))
                            {
                                TM_Action.TransferSoulBond(original, polymorphPawn);
                            }
                        }
                        catch(NullReferenceException ex)
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
                            lord.AddPawn(newPawn);
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
            DamageInfo dinfo = new DamageInfo(TMDamageDefOf.DamageDefOf.TM_Holy, Rand.Range(amt*.8f, amt*1.2f), 1, -1, instigator);
            for (int i =0; i <4; i++)
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
    }
}
