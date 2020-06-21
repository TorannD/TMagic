using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using AbilityUser;
using Verse;
using Verse.AI;
using Verse.Sound;
using AbilityUserAI;

namespace TorannMagic
{
    [CompilerGenerated]
    [Serializable]
    [StaticConstructorOnStartup]
    public class CompAbilityUserCustom : CompAbilityUser
    {
        public string LabelKey = "TM_Custom";

        public int customIndex = -2;
        public TMDefs.TM_CustomClass customClass = null;

        public bool temporaryPowersForNonColonists = true;

        private int autocastTick = 0;
        private int nextAICastAttemptTick = 0;
        public float weaponDamage = 1;


        private CustomData data = null;
        public CustomData Data
        {
            get
            {
                bool flag = this.data == null;
                if (flag)
                {
                    this.data = new CustomData(customClass?.customPowers?.ConvertAll((PowerDef def) => new CustomPower(def)));
                }
                return this.data;
            }
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
        }

        public override void PostDraw()
        {
            if (IsInitialized)
            {
                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                if (settingsRef.AIFriendlyMarking && base.AbilityUser.IsColonist)
                {
                    //DrawMageMark();
                }
                if (settingsRef.AIMarking && !base.AbilityUser.IsColonist)
                {
                    //DrawMageMark();
                }

                //TODO: draw ability-specific things like TechnoBit, DemonScorn wings and MageLight ?
            }

            base.PostDraw();
        }

        public override void CompTick()
        {
            base.CompTick();
            bool flag = base.AbilityUser != null;
            if (flag && IsInitialized)
            {
                bool flag4 = Find.TickManager.TicksGame % 30 == 0;
                if (flag4)
                {
                    bool flag5 = this.Data.UserXP > this.Data.XPForLevel(Data.UserLevel + 1);
                    if (flag5)
                    {
                        this.LevelUp();
                    }
                    if (Find.TickManager.TicksGame % 60 == 0)
                    {
                        if (this.Pawn.IsColonist && this.temporaryPowersForNonColonists)
                        {
                            ResolveFactionChange();
                        }
                    }
                }
                bool spawned = base.AbilityUser.Spawned;
                if (spawned)
                {
                    if (Find.TickManager.TicksGame % 60 == 0)
                    {
                        if (this.Pawn.IsColonist)
                        {
                            // TODO: resolve sustained abilities such as power nodes, minions, time mark, etc...
                        }
                        //TODO: update resources like Mana or Blood

                    }
                    ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                    if (this.autocastTick < Find.TickManager.TicksGame)  //180 default
                    {
                        //TODO: check for autocast
                        this.autocastTick = Find.TickManager.TicksGame + (int)Rand.Range(.8f * settingsRef.autocastEvaluationFrequency, 1.2f * settingsRef.autocastEvaluationFrequency);
                    }
                    if (!this.Pawn.IsColonist && settingsRef.AICasting && settingsRef.AIAggressiveCasting && Find.TickManager.TicksGame > this.nextAICastAttemptTick) //Aggressive AI Casting
                    {
                        this.nextAICastAttemptTick = Find.TickManager.TicksGame + Rand.Range(300, 500);
                        if (this.Pawn.jobs != null && this.Pawn.CurJobDef != TorannMagicDefOf.TMCastAbilitySelf && this.Pawn.CurJobDef != TorannMagicDefOf.TMCastAbilityVerb)
                        {
                            ResolveEnnemyAutocast();
                        }
                    }
                    //TODO: resolve passive abilities like TechnoOverdrive, Warlock Empathy and Succubus Lovin
                    if (Find.TickManager.TicksGame % 299 == 0) //cache weapon damage for tooltip and damage calculations
                    {
                        this.weaponDamage = TM_Calc.GetSkillDamage(this.Pawn);
                    }
                }
                else
                {
                    // update cooldowns while not on a map
                    if (Find.TickManager.TicksGame % 600 == 0)
                    {
                        if (this.Pawn.Map == null)
                        {
                            if (AbilityData?.AllPowers != null)
                            {
                                foreach (PawnAbility power in AbilityData.AllPowers)
                                {
                                    int cd = power.CooldownTicksLeft;
                                    cd = cd < 600 ? cd : 600;
                                    power.CooldownTicksLeft -= cd;
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            bool flag = base.AbilityUser != null;
            //Log.Message("Post Spawn Setup for " + this.AbilityUser?.Name + " init status:" + IsInitialized);
            if (flag && IsInitialized)
            {
                this.ResolveInspectorTab();
                bool spawned = base.AbilityUser.Spawned;
                if (spawned)
                {
                    bool flag2 = base.AbilityUser.story != null;
                    if (flag2)
                    {
                        this.ResolveOngoingAbilities();
                    }
                }
            }
        }

        public override bool TryTransformPawn()
        {
            return TM_ClassUtility.IsCustomClassIndex(this.AbilityUser.story.traits.allTraits) >= 0;
        }

        public override void Initialize()
        {
            //Log.Message("Initializing " + this.AbilityUser.Name);
            if (this.customClass == null)
            {
                this.customIndex = TM_ClassUtility.IsCustomClassIndex(this.AbilityUser.story.traits.allTraits);
                //Log.Message("Initializing " + this.AbilityUser.Name + " with class " + this.customIndex);
                if (this.customIndex >= 0)
                {
                    this.customClass = TM_ClassUtility.CustomClasses()[this.customIndex];
                    //Log.Message("This Custom class has " + customClass.customPowers.Count + " powers " + this.customIndex);
                    this.data = new CustomData(customClass.customPowers?.ConvertAll((PowerDef def) => new CustomPower(def)));
                    base.Initialize();
                }
                //else
                //{
                //    this.parent.AllComps.Remove(this);
                //}
            }
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            this.temporaryPowersForNonColonists = !this.Pawn.IsColonist;
            AssignAbilities(temporaryPowersForNonColonists); // non-colonists have access to all starting abilities.
            ResolveInspectorTab();
        }

        private void ResolveOngoingAbilities()
        {
            // TODO: resolve ongoing abilities that have fleeting data, such as druid's Fertile Lands
        }

        public void DrawMageMark()
        {
            if (this.customClass != null)
            {
                Material mat = TM_RenderQueue.mageMarkMat;
                if (this.customClass.classIconPath != "")
                {
                    mat = MaterialPool.MatFrom("Other/" + this.customClass.classIconPath.ToString());
                }
                if (this.customClass.classIconColor != null)
                {
                    mat.color = this.customClass.classIconColor;
                }
                float num = Mathf.Lerp(1.2f, 1.55f, 1f);
                Vector3 vector = this.AbilityUser.Drawer.DrawPos;
                vector.x = vector.x + .45f;
                vector.z = vector.z + .45f;
                vector.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
                float angle = 0f;
                Vector3 s = new Vector3(.28f, 1f, .28f);
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, mat, 0);
            }
        }

        public void ResolveFactionChange()
        {
            if (temporaryPowersForNonColonists)
            {
                RemovePowers();
                AssignAbilities();
                this.temporaryPowersForNonColonists = false;
            }
        }

        public void LevelUp(bool hideNotification = false)
        {
            if (!(this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless) || this.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Wayfarer)))
            {
                if (this.Data.UserLevel < 150)//customClass.maxLevel)
                {
                    this.Data.UserLevel++;
                    bool flag = !hideNotification;
                    if (flag)
                    {
                        ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                        if (Pawn.IsColonist && settingsRef.showLevelUpMessage)
                        {
                            Messages.Message( this.Pawn.Name + " increased his skill level in " + this.customClass.classTrait.label, //TranslatorFormattedStringExtensions.Translate("TM_MagicLevelUp",this.parent.Label),
                            this.Pawn, MessageTypeDefOf.PositiveEvent);
                        }
                    }
                }
                else
                {
                    this.Data.UserXP = this.Data.XPForLevel(this.Data.UserLevel);
                }
            }
        }

        public void LevelUpPower(BasePower power)
        {
            int savedTicks = this.AbilityData.Powers.Find((PawnAbility a) => a.Def == power.AbilityDef)?.CooldownTicksLeft ?? -1;
            base.RemovePawnAbility(power.AbilityDef);
            power.level++;
            //if ((power.AbilityDef as TMAbilityDef)?.shouldInitialize ?? false)
            //{ //TODO: handle passive upgrades. shouldInitialize is not reliable because upgraded spells have it set to false, it is used as "isInitialAbility".
                base.AddPawnAbility(power.AbilityDef, true, savedTicks);
            Log.Message(power.AbilityDef.defName);
            //}
        }

        private void ResolveEnnemyAutocast()
        {
            IEnumerable<AbilityUserAIProfileDef> enumerable = this.Pawn.EligibleAIProfiles();
            if (enumerable != null && enumerable.Count() > 0)
            {
                foreach (AbilityUserAIProfileDef item in enumerable)
                {
                    if (item != null)
                    {
                        AbilityAIDef useThisAbility = null;
                        if (item.decisionTree != null)
                        {
                            useThisAbility = item.decisionTree.RecursivelyGetAbility(this.Pawn);
                        }
                        if (useThisAbility != null)
                        {
                            ThingComp val = this.Pawn.AllComps.First((ThingComp comp) => ((object)comp).GetType() == item.compAbilityUserClass);
                            CompAbilityUser compAbilityUser = val as CompAbilityUser;
                            if (compAbilityUser != null)
                            {
                                PawnAbility pawnAbility = compAbilityUser.AbilityData.AllPowers.First((PawnAbility ability) => ability.Def == useThisAbility.ability);
                                string reason = "";
                                if (pawnAbility.CanCastPowerCheck(AbilityContext.AI, out reason))
                                {
                                    LocalTargetInfo target = useThisAbility.Worker.TargetAbilityFor(useThisAbility, this.Pawn);
                                    if (target.IsValid)
                                    {
                                        pawnAbility.UseAbility(AbilityContext.Player, target);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AssignAbilities(bool learnAll = false)
        {
            Pawn abilityUser = base.AbilityUser;
            for (int z = 0; z < this.Data.Powers.Count; z++)
            {
                BasePower p = this.Data.Powers[z];
                if (p.AbilityDef is TMAbilityDef)
                {
                    TMAbilityDef ability = (TMAbilityDef)p.AbilityDef;
                    // TODO: learning method : scrolls, chance, Inspiration?
                    if (!(learnAll || !abilityUser.health.hediffSet.HasHediff(TorannMagicDefOf.TM_Uncertainty, false) || Rand.Chance(ability.learnChance)))
                    {
                        p.learned = false;
                    }

                    if (p.learned)// && ability.shouldInitialize)
                    { //TODO: handle passive abilities. shouldInitialize is not reliable because upgraded spells have it set to false, it is used as "isInitialAbility".
                        this.AddPawnAbility(ability);
                    }
                }
            }
            if (this.customClass.classHediff != null)
            {
                HealthUtility.AdjustSeverity(abilityUser, this.customClass.classHediff, this.customClass.hediffSeverity);
            }
        }

        public void RemovePowers()
        {
            for (int i = 0; i < this.Data.Powers.Count; i++)
            {
                BasePower mp = this.Data.Powers[i];
                for (int j = 0; j < mp.Abilities.Count; j++)
                {
                    this.RemovePawnAbility(mp.Abilities[j]);
                }
                mp.learned = false;
            }
        }

        public void ResetSkills()
        {
            int skillpoints = this.Data.Upgrades.Values.Sum((MagicPowerSkill x) => x.level * x.costToLevel);

            int magicPts = this.Data.AbilityPoints;

            this.data.ResetAllSkills();

            this.Data.AbilityPoints = magicPts + skillpoints;
        }

        public void RemoveTraits()
        {
            List<Trait> traits = this.AbilityUser.story.traits.allTraits;
            for (int i = 0; i < traits.Count; i++)
            {
                if (this.customClass != null)
                {
                    traits.Remove(this.AbilityUser.story.traits.GetTrait(this.customClass.classTrait));
                    this.customClass = null;
                    this.customIndex = -2;
                }
            }
        }

        public void RemoveTMagicHediffs()
        {
            // TODO
            List<Hediff> allHediffs = this.Pawn.health.hediffSet.GetHediffs<Hediff>().ToList();
            for (int i = 0; i < allHediffs.Count(); i++)
            {
                Hediff hediff = allHediffs[i];
                if (hediff.def.defName.StartsWith("TM_"))
                {
                    this.Pawn.health.RemoveHediff(hediff);
                }

            }
        }

        public void RemoveAbilityUser()
        {
            this.RemovePowers();
            this.RemoveTMagicHediffs();
            this.RemoveTraits();
            this.data = new CustomData();
            base.IsInitialized = false;
        }

        //public override List<HediffDef> IgnoredHediffs()
        //{
        //    return new List<HediffDef>
        //    {
        //        TorannMagicDefOf.TM_MightUserHD,
        //        TorannMagicDefOf.TM_MagicUserHD
        //    };
        //}

        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            //TODO: move the damage absorbtion code for each abilities in their own hediff
            base.PostPreApplyDamage(dinfo, out absorbed);
        }

        public void ResolveInspectorTab()
        {
            InspectTabBase inspectTabsx = base.AbilityUser.GetInspectTabs().FirstOrDefault((InspectTabBase x) => x.labelKey == "TM_TabCustom");
            IEnumerable<InspectTabBase> inspectTabs = base.AbilityUser.GetInspectTabs();
            bool flag = inspectTabs != null && inspectTabs.Count<InspectTabBase>() > 0;
            if (flag)
            {
                if (inspectTabsx == null)
                {
                    try
                    {
                        base.AbilityUser.def.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(typeof(ITab_Pawn_Custom)));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Concat(new object[]
                        {
                            "Could not instantiate inspector tab of type ",
                            typeof(ITab_Pawn_Custom),
                            ": ",
                            ex
                        }));
                    }
                }
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look<bool>(ref this.temporaryPowersForNonColonists, "temporaryPowersForNonColonists", true, false);
            Scribe_Deep.Look<CustomData>(ref this.data, "customClassData", new object[] { });
            bool flag11 = Scribe.mode == LoadSaveMode.PostLoadInit;
            if (flag11)
            {
                if (this.customIndex >= 0)
                {
                    this.customClass = TM_ClassUtility.CustomClasses()[this.customIndex];
                }
            }
        }

    }
}
