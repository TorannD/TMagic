using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;
using AbilityUser;
using Verse.AI;
using UnityEngine;
using System.Text;
using CompDeflector;

namespace TorannMagic
{
    [CompilerGenerated]
    [Serializable]
    [StaticConstructorOnStartup]
    public class CompAbilityUserMight : CompAbilityUser
    {
        public string LabelKey = "TM_Might";        

        public bool mightPowersInitialized = false;
        public bool firstMightTick = false;
        private int age = -1;
        private int fortitudeMitigationDelay = 0;
        private int mightXPRate = 1200;
        private int lastMightXPGain = 0;
        private int autocastTick = 0;

        private float G_Sprint_eff = 0.20f;
        private float G_Grapple_eff = 0.10f;
        private float G_Cleave_eff = 0.10f;
        private float G_Whirlwind_eff = 0.10f;
        private float S_Headshot_eff = 0.10f;
        private float S_DisablingShot_eff = 0.10f;
        private float S_AntiArmor_eff = .10f;
        private float B_SeismicSlash_eff = 0.10f;
        private float B_BladeSpin_eff = 0.10f;
        private float B_PhaseStrike_eff = 0.08f;
        private float R_AnimalFriend_eff = 0.15f;
        private float R_ArrowStorm_eff = 0.08f;
        private float F_Disguise_eff = 0.10f;
        private float F_Mimic_eff = 0.08f;
        private float F_Reversal_eff = 0.10f;
        private float F_Transpose_eff = 0.08f;
        private float F_Possess_eff = 0.06f;
        private float P_PsionicBarrier_eff = 0.10f;
        private float P_PsionicBlast_eff = 0.08f;
        private float P_PsionicDash_eff = 0.10f;
        private float P_PsionicStorm_eff = 0.10f;

        private float global_seff = 0.03f;

        public bool skill_Sprint = false;
        public bool skill_GearRepair = false;
        public bool skill_InnerHealing = false;
        public bool skill_HeavyBlow = false;
        public bool skill_StrongBack = false;
        public bool skill_ThickSkin = false;
        public bool skill_FightersFocus = false;
        public bool skill_Teach = false;


        public float maxSP = 1;
        public float spRegenRate = 1;
        public float coolDown = 1;
        public float spCost = 1;
        public float xpGain = 1;
        public float arcaneRes = 1;
        public float mightPwr = 1;
        private int resMitigationDelay = 0;

        public bool animalBondingDisabled = false;
        private int animalFriendDisabledPeriod;

        public bool usePsionicAugmentationToggle = true;
        public List<Thing> combatItems = new List<Thing>();

        public Verb_Deflected deflectVerb;
        DamageInfo reversal_dinfo;
        Thing reversalTarget = null;
        public Pawn bondedPet = null;

        public Verb_UseAbility lastVerbUsed = null;
        public int lastTickVerbUsed = 0;

        public TMAbilityDef mimicAbility = null;

        private MightData mightData = null;
        public MightData MightData
        {
            get
            {
                bool flag = this.mightData == null && this.IsMightUser;
                if (flag)
                {
                    this.mightData = new MightData(this);
                }
                return this.mightData;
            }
        }        

        public override void PostDraw()
        {
            base.PostDraw();
            if(this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD, false) || this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD_I, false) || this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD_II, false) || this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_PossessionHD_III, false) ||
                this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_CoOpPossessionHD, false) || this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_CoOpPossessionHD_I, false) || this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_CoOpPossessionHD_II, false) || this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_CoOpPossessionHD_III, false))
            {
                DrawDeceptionTicker(true);
            }
            else if(this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD, false) || this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_I, false) || this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_II, false) || this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_III, false))
            {
                DrawDeceptionTicker(false);
            }

            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (settingsRef.AIFriendlyMarking && this.AbilityUser.IsColonist && this.IsMightUser)
            {
                DrawFighterMark();                
            }
            if (settingsRef.AIMarking && !base.AbilityUser.IsColonist && this.IsMightUser)
            {
                DrawFighterMark();                
            }
        }

        public void DrawFighterMark()
        {
            float num = Mathf.Lerp(1.2f, 1.55f, 1f);
            Vector3 vector = this.AbilityUser.Drawer.DrawPos;
            vector.x = vector.x + .45f;
            vector.z = vector.z + .45f;
            vector.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
            float angle = 0f;
            Vector3 s = new Vector3(.28f, 1f, .28f);
            Matrix4x4 matrix = default(Matrix4x4);
            matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
            if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Gladiator))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.gladiatorMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.sniperMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Bladedancer))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.bladedancerMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Ranger))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.rangerMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Faceless))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.facelessMarkMat, 0);
            }
            else if (this.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic))
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.psionicMarkMat, 0);
            }
            else
            {
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.fighterMarkMat, 0);
            }
        }

        public void DrawDeceptionTicker(bool possessed)
        {
            if (possessed)
            {
                float num = Mathf.Lerp(1.2f, 1.55f, 1f);
                Vector3 vector = this.AbilityUser.Drawer.DrawPos;
                vector.x = vector.x - .25f;
                vector.z = vector.z + .8f;
                vector.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
                float angle = 0f;
                Vector3 s = new Vector3(.45f, 1f, .4f);
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.possessionMask, 0);
                if (this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD, false) || this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_I, false) || this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_II, false) || this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_DisguiseHD_III, false))
                {
                    vector.z = vector.z + .35f;
                    matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
                    Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.deceptionEye, 0);
                }
            }
            else
            {
                float num = Mathf.Lerp(1.2f, 1.55f, 1f);
                Vector3 vector = this.AbilityUser.Drawer.DrawPos;
                vector.x = vector.x - .25f;
                vector.z = vector.z + .8f;
                vector.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
                float angle = 0f;
                Vector3 s = new Vector3(.45f, 1f, .4f);
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, TM_RenderQueue.deceptionEye, 0);
            }
        }

        public static List<TMAbilityDef> MightAbilities = null;    
        
        public int LevelUpSkill_global_refresh(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_global_refresh.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }        
        public int LevelUpSkill_global_seff(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_global_seff.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }        
        public int LevelUpSkill_global_strength(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_global_strength.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_global_endurance(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_global_endurance.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_Sprint(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Sprint.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Fortitude(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Fortitude.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Grapple(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Grapple.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Cleave(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Cleave.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Whirlwind(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Whirlwind.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_SniperFocus(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_SniperFocus.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Headshot(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Headshot.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_DisablingShot(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_DisablingShot.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_AntiArmor(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_AntiArmor.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_BladeFocus(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_BladeFocus.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_BladeArt(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_BladeArt.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_SeismicSlash(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_SeismicSlash.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_BladeSpin(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_BladeSpin.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_PhaseStrike(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_PhaseStrike.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_RangerTraining(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_RangerTraining.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_BowTraining(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_BowTraining.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_PoisonTrap(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_PoisonTrap.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_AnimalFriend(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_AnimalFriend.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_ArrowStorm(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_ArrowStorm.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_Disguise(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Disguise.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Mimic(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Reversal(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Reversal.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Transpose(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Transpose.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_Possess(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Possess.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }

        public int LevelUpSkill_PsionicAugmentation(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_PsionicAugmentation.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_PsionicBarrier(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_PsionicBarrier.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_PsionicBlast(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_PsionicBlast.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_PsionicDash(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_PsionicDash.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }
        public int LevelUpSkill_PsionicStorm(string skillName)
        {
            int result = 0;
            MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_PsionicStorm.FirstOrDefault((MightPowerSkill x) => x.label == skillName);
            bool flag = mightPowerSkill != null;
            if (flag)
            {
                result = mightPowerSkill.level;
            }
            return result;
        }

        public override void CompTick()
        {
            bool flag = base.AbilityUser != null;
            if (flag)
            {
                bool spawned = base.AbilityUser.Spawned;
                if (spawned)
                {
                    bool isMightUser = this.IsMightUser;
                    if (isMightUser)
                    {
                        bool flag3 = !this.MightData.Initialized;
                        if (flag3)
                        {
                            this.PostInitializeTick();
                        }
                        base.CompTick();
                        this.age++;
                        if(Find.TickManager.TicksGame % 20 == 0)
                        {
                            ResolveSustainedSkills();
                            if (reversalTarget != null)
                            {
                                ResolveReversalDamage();
                            }
                        }
                        if (Find.TickManager.TicksGame % 60 == 0)
                        {
                            ResolveClassSkills();
                            ResolveClassPassions();
                        }
                        if (this.autocastTick < Find.TickManager.TicksGame)  //180 default
                        {
                            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                            if (this.Pawn.IsColonist && !this.Pawn.Dead && !this.Pawn.Downed && this.Pawn.Map != null)
                            {
                                ResolveAutoCast();
                            }
                            this.autocastTick = Find.TickManager.TicksGame + (int)Rand.Range(.8f * settingsRef.autocastEvaluationFrequency, 1.2f * settingsRef.autocastEvaluationFrequency);
                        }
                        if (this.Stamina.CurLevel >  (.99f * this.Stamina.MaxLevel))
                        {                            
                            if (this.age > (lastMightXPGain + mightXPRate))
                            {
                                MightData.MightUserXP++;
                                lastMightXPGain = this.age;
                            }
                        }
                        bool flag4 = Find.TickManager.TicksGame % 30 == 0;
                        if (flag4)
                        {
                            bool flag5 = this.MightUserXP > this.MightUserXPTillNextLevel;
                            if (flag5)
                            {
                                this.LevelUp(false);
                            }
                        }
                    }
                    if(Find.TickManager.TicksGame % 30 == 0)
                    {
                        bool flag6 = this.Pawn.TargetCurrentlyAimingAt != null;
                        if (flag6)
                        {
                            if (this.Pawn.TargetCurrentlyAimingAt.Thing is Pawn)
                            {
                                Pawn targetPawn = this.Pawn.TargetCurrentlyAimingAt.Thing as Pawn;
                                if (targetPawn.RaceProps.Humanlike)
                                {
                                    bool flag7 = (this.Pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_DisguiseHD")) || this.Pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_DisguiseHD_I")) || this.Pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_DisguiseHD_II")) || this.Pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_DisguiseHD_III")));
                                    if (targetPawn.Faction != this.Pawn.Faction && flag7)
                                    {
                                        using (IEnumerator<Hediff> enumerator = this.Pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                                        {
                                            while (enumerator.MoveNext())
                                            {
                                                Hediff rec = enumerator.Current;
                                                if (rec.def == TorannMagicDefOf.TM_DisguiseHD || rec.def == TorannMagicDefOf.TM_DisguiseHD_I || rec.def == TorannMagicDefOf.TM_DisguiseHD_II || rec.def == TorannMagicDefOf.TM_DisguiseHD_III)
                                                {
                                                    this.Pawn.health.RemoveHediff(rec);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Find.TickManager.TicksGame % 600 == 0)
                    {
                        if (this.Pawn.Map == null)
                        {
                            if (this.IsMightUser)
                            {
                                int num;
                                if (AbilityData?.AllPowers != null)
                                {
                                    AbilityData obj = AbilityData;
                                    num = ((obj != null && obj.AllPowers.Count > 0) ? 1 : 0);
                                }
                                else
                                {
                                    num = 0;
                                }
                                if (num != 0)
                                {
                                    foreach (PawnAbility allPower in AbilityData.AllPowers)
                                    {
                                        allPower.CooldownTicksLeft -= 600;
                                        if (allPower.CooldownTicksLeft <= 0)
                                        {
                                            allPower.CooldownTicksLeft = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (IsInitialized)
            {
                //custom code
            }
        }

        public void PostInitializeTick()
        {
            bool flag = base.AbilityUser != null;
            if (flag)
            {
                bool spawned = base.AbilityUser.Spawned;
                if (spawned)
                {
                    bool flag2 = base.AbilityUser.story != null;
                    if (flag2)
                    {
                        //this.MightData.Initialized = true;
                        this.Initialize();
                        this.ResolveMightTab();
                        this.ResolveMightPowers();
                        this.ResolveStamina();
                    }
                }
            }
        }

        public bool IsMightUser
        {
            get
            {
                bool flag = base.AbilityUser != null;
                if (flag)
                {
                    bool flag3 = base.AbilityUser.story != null;
                    if (flag3)
                    {
                        bool flag4 = base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Gladiator) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Ranger) || base.AbilityUser.story.traits.HasTrait(TorannMagicDefOf.Faceless);
                        if (flag4)
                        {                            
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public int MightUserLevel
        {
            get
            {
                return this.MightData.MightUserLevel;
            }
            set
            {
                bool flag = value > this.MightData.MightUserLevel;
                if (flag)
                {
                    this.MightData.MightAbilityPoints++;
                    bool flag2 = this.MightData.MightUserXP < value * 500;
                    if (flag2)
                    {
                        this.MightData.MightUserXP = value * 500;
                    }
                }
                this.MightData.MightUserLevel = value;
            }
        }

        public int MightUserXP
        {
            get
            {
                return this.MightData.MightUserXP;
            }
            set
            {
                this.MightData.MightUserXP = value;
            }
        }

        public float XPLastLevel
        {
            get
            {
                float result = 0f;
                bool flag = this.MightUserLevel > 0;
                if (flag)
                {
                    result = (float)(this.MightUserLevel * 500);
                }
                return result;
            }
        }

        public float XPTillNextLevelPercent
        {
            get
            {
                return ((float)this.MightData.MightUserXP - this.XPLastLevel) / ((float)this.MightUserXPTillNextLevel - this.XPLastLevel);
            }
        }

        public int MightUserXPTillNextLevel
        {
            get
            {
                return (this.MightData.MightUserLevel + 1) * 500; 
            }
        }

        public void LevelUp(bool hideNotification = false)
        {
            this.MightUserLevel++;
            bool flag = !hideNotification;
            if (flag)
            {
                Messages.Message("TM_MightLevelUp".Translate(new object[]
                {
                    this.parent.Label
                }), MessageTypeDefOf.PositiveEvent);
            }
        }

        public void LevelUpPower(MightPower power)
        {
            foreach (AbilityDef current in power.TMabilityDefs)
            {
                base.RemovePawnAbility(current);
            }
            power.level++;
            base.AddPawnAbility(power.nextLevelAbilityDef, true, -1f);
            this.UpdateAbilities();
        }

        public Need_Stamina Stamina
        {
            get
            {
                return base.AbilityUser.needs.TryGetNeed<Need_Stamina>();
            }
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            bool flag = CompAbilityUserMight.MightAbilities == null;
            if (flag)
            {
                if (this.mightPowersInitialized == false)
                {
                    Pawn abilityUser = base.AbilityUser;
                    bool flag2;
                    MightData.MightUserLevel = 0;
                    MightData.MightAbilityPoints = 0;

                    flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Gladiator);
                    if (flag2)
                    {
                        //Log.Message("Initializing Gladiator Abilities");
                        this.AddPawnAbility(TorannMagicDefOf.TM_Sprint);
                        //this.AddPawnAbility(TorannMagicDefOf.TM_Fortitude);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Grapple);
                        //this.AddPawnAbility(TorannMagicDefOf.TM_Cleave);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Whirlwind);
                    }
                    flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper);
                    if (flag2)
                    {
                        //Log.Message("Initializing Sniper Abilities");
                        //this.AddPawnAbility(TorannMagicDefOf.TM_SniperFocus);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Headshot);
                        this.AddPawnAbility(TorannMagicDefOf.TM_DisablingShot);
                        this.AddPawnAbility(TorannMagicDefOf.TM_AntiArmor);
                    }
                    flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Bladedancer);
                    if (flag2)
                    {
                       // Log.Message("Initializing Bladedancer Abilities");
                       // this.AddPawnAbility(TorannMagicDefOf.TM_BladeFocus);
                        //this.AddPawnAbility(TorannMagicDefOf.TM_BladeArt);
                        this.AddPawnAbility(TorannMagicDefOf.TM_SeismicSlash);
                        this.AddPawnAbility(TorannMagicDefOf.TM_BladeSpin);
                        this.AddPawnAbility(TorannMagicDefOf.TM_PhaseStrike);
                    }
                    flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Ranger);
                    if (flag2)
                    {
                        //Log.Message("Initializing Ranger Abilities");
                        //this.AddPawnAbility(TorannMagicDefOf.TM_RangerTraining);
                       // this.AddPawnAbility(TorannMagicDefOf.TM_BowTraining);
                        this.AddPawnAbility(TorannMagicDefOf.TM_PoisonTrap);
                        this.AddPawnAbility(TorannMagicDefOf.TM_AnimalFriend);
                        this.AddPawnAbility(TorannMagicDefOf.TM_ArrowStorm);
                    }
                    flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Faceless);
                    if (flag2)
                    {
                        //Log.Message("Initializing Faceless Abilities");
                        this.AddPawnAbility(TorannMagicDefOf.TM_Disguise);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Mimic);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Reversal);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Transpose);
                        this.AddPawnAbility(TorannMagicDefOf.TM_Possess);
                    }
                    this.mightPowersInitialized = true;
                    flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic);
                    if (flag2)
                    {
                        //Log.Message("Initializing Psionic Abilities");
                        this.AddPawnAbility(TorannMagicDefOf.TM_PsionicBarrier);
                        this.AddPawnAbility(TorannMagicDefOf.TM_PsionicBlast);
                        this.AddPawnAbility(TorannMagicDefOf.TM_PsionicDash);
                        this.AddPawnAbility(TorannMagicDefOf.TM_PsionicStorm);
                    }
                    this.mightPowersInitialized = true;
                    //base.UpdateAbilities();

                }
                //this.UpdateAbilities();
                //base.UpdateAbilities();
            }
        }

        public void InitializeSkill()  //used for class independant skills
        {
            Pawn abilityUser = base.AbilityUser;

            if (this.skill_Sprint == true && !abilityUser.story.traits.HasTrait(TorannMagicDefOf.Gladiator))
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint);
                this.AddPawnAbility(TorannMagicDefOf.TM_Sprint);
            }
            if (this.skill_GearRepair == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_GearRepair);
                this.AddPawnAbility(TorannMagicDefOf.TM_GearRepair);
            }
            if (this.skill_InnerHealing == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_InnerHealing);
                this.AddPawnAbility(TorannMagicDefOf.TM_InnerHealing);
            }
            if (this.skill_StrongBack == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_StrongBack);
                this.AddPawnAbility(TorannMagicDefOf.TM_StrongBack);
            }
            if (this.skill_HeavyBlow == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_HeavyBlow);
                this.AddPawnAbility(TorannMagicDefOf.TM_HeavyBlow);
            }
            if (this.skill_ThickSkin == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_ThickSkin);
                this.AddPawnAbility(TorannMagicDefOf.TM_ThickSkin);
            }
            if (this.skill_FightersFocus == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_FightersFocus);
                this.AddPawnAbility(TorannMagicDefOf.TM_FightersFocus);
            }
            if (this.skill_Teach == true)
            {
                this.RemovePawnAbility(TorannMagicDefOf.TM_TeachMight);
                this.AddPawnAbility(TorannMagicDefOf.TM_TeachMight);
            }

        }

        public void FixPowers()
        {
            Pawn abilityUser = base.AbilityUser;
            if (this.mightPowersInitialized == true)
            {
                bool flag2;
                flag2 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Gladiator);
                if (flag2)
                {
                    Log.Message("Fixing Gladiator Abilities");
                    foreach (MightPower currentG in this.MightData.MightPowersG)
                    {
                        if (currentG.abilityDef.defName == "TM_Sprint" || currentG.abilityDef.defName == "TM_Sprint_I" || currentG.abilityDef.defName == "TM_Sprint_II" || currentG.abilityDef.defName == "TM_Sprint_III")
                        {
                            if (currentG.level == 0)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_I);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_II);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_III);
                            }
                            else if (currentG.level == 1)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_II);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_III);
                            }
                            else if (currentG.level == 2)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_I);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_III);
                            }
                            else if (currentG.level == 3)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_II);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Sprint_I);
                            }
                            else
                            {
                                Log.Message("Ability level not found to fix");
                            }
                        }
                        if (currentG.abilityDef.defName == "TM_Grapple" || currentG.abilityDef.defName == "TM_Grapple_I" || currentG.abilityDef.defName == "TM_Grapple_II" || currentG.abilityDef.defName == "TM_Grapple_III")
                        {
                            if (currentG.level == 0)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_I);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_II);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_III);
                            }
                            else if (currentG.level == 1)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_II);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_III);
                            }
                            else if (currentG.level == 2)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_I);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_III);
                            }
                            else if (currentG.level == 3)
                            {
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_II);
                                this.RemovePawnAbility(TorannMagicDefOf.TM_Grapple_I);
                            }
                            else
                            {
                                Log.Message("Ability level not found to fix");
                            }
                        }
                    }            
                }
            }
            //this.UpdateAbilities();
            //base.UpdateAbilities();
        }

        public override bool TryTransformPawn()
        {
            return this.IsMightUser;
        }

        public bool TryAddPawnAbility(TMAbilityDef ability)
        {
            //add check to verify no ability is already added
            bool result = false;
            base.AddPawnAbility(ability, true, -1f);
            result = true;
            return result;
        }        

        public void ClearPowers()
        {
            int tmpLvl = this.MightUserLevel;
            int tmpExp = this.MightUserXP;
            base.IsInitialized = false;
            this.mightData = null;
            this.CompTick();
            this.MightUserLevel = tmpLvl;
            this.MightUserXP = tmpExp;
            this.mightData.MightAbilityPoints = tmpLvl;
        }

        private void ClearPower(MightPower current)
        {
            Log.Message("Removing ability: " + current.abilityDef.defName.ToString());
            base.RemovePawnAbility(current.abilityDef);
            base.UpdateAbilities();
        }

        public void ClearTraits()
        {
            List<Trait> traits = this.AbilityUser.story.traits.allTraits;
            for (int i = 0; i < traits.Count; i++)
            {
                if (traits[i].def.defName == "Gladiator")
                {
                    Log.Message("Removing trait " + traits[i].def.defName);
                    traits.Remove(traits[i]);
                    i--;
                }
                if (traits[i].def.defName == "TM_Sniper")
                {
                    Log.Message("Removing trait " + traits[i].def.defName);
                    traits.Remove(traits[i]);
                    i--;
                }
                if (traits[i].def.defName == "Bladedancer")
                {
                    Log.Message("Removing trait " + traits[i].def.defName);
                    traits.Remove(traits[i]);
                    i--;
                }
                if (traits[i].def.defName == "Ranger")
                {
                    Log.Message("Removing trait " + traits[i].def.defName);
                    traits.Remove(traits[i]);
                    i--;
                }
                if (traits[i].def.defName == "TM_Psionic")
                {
                    Log.Message("Removing trait " + traits[i].def.defName);
                    traits.Remove(traits[i]);
                    i--;
                }
            }
        }

        private void LoadPowers(Pawn pawn)
        {
            if (pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator))
            {
                foreach (MightPower currentG in this.MightData.MightPowersG)
                {
                    Log.Message("Removing ability: " + currentG.abilityDef.defName.ToString());
                    currentG.level = 0;
                    base.RemovePawnAbility(currentG.abilityDef);
                }
                
            }
        }

        public int MightAttributeEffeciencyLevel(string attributeName)
        {
            int result = 0;

            if (attributeName == "TM_Sprint_eff")
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Sprint.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = mightPowerSkill != null;
                if (flag)
                {
                    result = mightPowerSkill.level;
                }
            }
            if (attributeName == "TM_Fortitude_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_Fortitude.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Grapple_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_Grapple.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Cleave_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_Cleave.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Whirlwind_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_Whirlwind.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Headshot_eff")
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Headshot.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = mightPowerSkill != null;
                if (flag)
                {
                    result = mightPowerSkill.level;
                }
            }
            if (attributeName == "TM_DisablingShot_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_DisablingShot.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_AntiArmor_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_AntiArmor.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_SeismicSlash_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_SeismicSlash.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_BladeSpin_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_BladeSpin.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_PhaseStrike_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_PhaseStrike.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_AnimalFriend_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_AnimalFriend.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_ArrowStorm_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_ArrowStorm.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Disguise_eff")
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Disguise.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = mightPowerSkill != null;
                if (flag)
                {
                    result = mightPowerSkill.level;
                }
            }
            if (attributeName == "TM_Mimic_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Reversal_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_Reversal.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Transpose_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_Transpose.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_Possess_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_Possess.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_PsionicBarrier_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_PsionicBarrier.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_PsionicBlast_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_PsionicBlast.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_PsionicDash_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_PsionicDash.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }
            if (attributeName == "TM_PsionicStorm_eff")
            {
                MightPowerSkill magicPowerSkill = this.MightData.MightPowerSkill_PsionicStorm.FirstOrDefault((MightPowerSkill x) => x.label == attributeName);
                bool flag = magicPowerSkill != null;
                if (flag)
                {
                    result = magicPowerSkill.level;
                }
            }

            return result;
        }

        public float ActualStaminaCost(TMAbilityDef mightDef)
        {
            float adjustedStaminaCost = mightDef.staminaCost;
            if (mightDef == TorannMagicDefOf.TM_Sprint || mightDef == TorannMagicDefOf.TM_Sprint_I || mightDef == TorannMagicDefOf.TM_Sprint_II || mightDef == TorannMagicDefOf.TM_Sprint_III)
            {
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.G_Sprint_eff * (float)this.MightAttributeEffeciencyLevel("TM_Sprint_eff"));
            }
            if (mightDef == TorannMagicDefOf.TM_Grapple || mightDef == TorannMagicDefOf.TM_Grapple_I || mightDef == TorannMagicDefOf.TM_Grapple_II || mightDef == TorannMagicDefOf.TM_Grapple_III)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Grapple.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Grapple_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.G_Grapple_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_Cleave)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Cleave.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Cleave_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.G_Cleave_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_Whirlwind)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Whirlwind.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Whirlwind_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.G_Whirlwind_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_Headshot)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Headshot.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Headshot_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.S_Headshot_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_DisablingShot || mightDef == TorannMagicDefOf.TM_DisablingShot_I || mightDef == TorannMagicDefOf.TM_DisablingShot_II || mightDef == TorannMagicDefOf.TM_DisablingShot_III)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_DisablingShot.FirstOrDefault((MightPowerSkill x) => x.label == "TM_DisablingShot_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.S_DisablingShot_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_AntiArmor)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_AntiArmor.FirstOrDefault((MightPowerSkill x) => x.label == "TM_AntiArmor_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.S_AntiArmor_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_SeismicSlash)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_SeismicSlash.FirstOrDefault((MightPowerSkill x) => x.label == "TM_SeismicSlash_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.B_SeismicSlash_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_BladeSpin)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_BladeSpin.FirstOrDefault((MightPowerSkill x) => x.label == "TM_BladeSpin_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.B_BladeSpin_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_PhaseStrike || mightDef == TorannMagicDefOf.TM_PhaseStrike_I || mightDef == TorannMagicDefOf.TM_PhaseStrike_II || mightDef == TorannMagicDefOf.TM_PhaseStrike_III)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_PhaseStrike.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PhaseStrike_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.B_PhaseStrike_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_AnimalFriend)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_AnimalFriend.FirstOrDefault((MightPowerSkill x) => x.label == "TM_AnimalFriend_eff");
                if (this.bondedPet != null)
                {
                    adjustedStaminaCost = (mightDef.staminaCost - (mightDef.staminaCost * (this.R_AnimalFriend_eff * (float)mightPowerSkill.level))/ 2);
                }
                else
                {
                    adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.R_AnimalFriend_eff * (float)mightPowerSkill.level);
                }
            }
            if (mightDef == TorannMagicDefOf.TM_ArrowStorm || mightDef == TorannMagicDefOf.TM_ArrowStorm_I || mightDef == TorannMagicDefOf.TM_ArrowStorm_II || mightDef == TorannMagicDefOf.TM_ArrowStorm_III)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_ArrowStorm.FirstOrDefault((MightPowerSkill x) => x.label == "TM_ArrowStorm_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.R_ArrowStorm_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_Disguise)
            {
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.F_Disguise_eff * (float)this.MightAttributeEffeciencyLevel("TM_Disguise_eff"));
            }
            if (mightDef == TorannMagicDefOf.TM_Transpose || mightDef == TorannMagicDefOf.TM_Transpose_I || mightDef == TorannMagicDefOf.TM_Transpose_II || mightDef == TorannMagicDefOf.TM_Transpose_III)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Transpose.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Transpose_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.F_Transpose_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_Mimic)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.F_Mimic_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_Reversal)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Reversal.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Reversal_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.F_Reversal_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_Possess)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_Possess.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Possess_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.F_Possess_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_PsionicBarrier || mightDef == TorannMagicDefOf.TM_PsionicBarrier_Projected)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_PsionicBarrier.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicBarrier_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.P_PsionicBarrier_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_PsionicBlast || mightDef == TorannMagicDefOf.TM_PsionicBlast_I || mightDef == TorannMagicDefOf.TM_PsionicBlast_II || mightDef == TorannMagicDefOf.TM_PsionicBlast_III)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_PsionicBlast.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicBlast_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.P_PsionicBlast_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_PsionicDash)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_PsionicDash.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicDash_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.P_PsionicDash_eff * (float)mightPowerSkill.level);
            }
            if (mightDef == TorannMagicDefOf.TM_PsionicStorm)
            {
                MightPowerSkill mightPowerSkill = this.MightData.MightPowerSkill_PsionicStorm.FirstOrDefault((MightPowerSkill x) => x.label == "TM_PsionicStorm_eff");
                adjustedStaminaCost = mightDef.staminaCost - mightDef.staminaCost * (this.P_PsionicStorm_eff * (float)mightPowerSkill.level);
            }

            MightPowerSkill globalSkill = this.MightData.MightPowerSkill_global_seff.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_seff_pwr");
            if (globalSkill != null)
            {
                return (adjustedStaminaCost - (adjustedStaminaCost * (global_seff * globalSkill.level)));
            }
            else
            {
                return adjustedStaminaCost;
            }

        }

        public override List<HediffDef> IgnoredHediffs()
        {
            return new List<HediffDef>
            {
                TorannMagicDefOf.TM_MagicUserHD
            };
        }

        public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
        {
            Pawn abilityUser = base.AbilityUser;
            absorbed = false;
            //bool flag = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Gladiator) || abilityUser.story.traits.HasTrait;
            //if (isGladiator)
            //{
            List<Hediff> list = new List<Hediff>();
            List<Hediff> arg_32_0 = list;
            IEnumerable<Hediff> arg_32_1;
            if (abilityUser == null)
            {
                arg_32_1 = null;
            }
            else
            {
                Pawn_HealthTracker expr_1A = abilityUser.health;
                if (expr_1A == null)
                {
                    arg_32_1 = null;
                }
                else
                {
                    HediffSet expr_26 = expr_1A.hediffSet;
                    arg_32_1 = ((expr_26 != null) ? expr_26.hediffs : null);
                }
            }
            arg_32_0.AddRange(arg_32_1);
            Pawn expr_3E = abilityUser;
            int? arg_84_0;
            if (expr_3E == null)
            {
                arg_84_0 = null;
            }
            else
            {
                Pawn_HealthTracker expr_52 = expr_3E.health;
                if (expr_52 == null)
                {
                    arg_84_0 = null;
                }
                else
                {
                    HediffSet expr_66 = expr_52.hediffSet;
                    arg_84_0 = ((expr_66 != null) ? new int?(expr_66.hediffs.Count<Hediff>()) : null);
                }
            }
            bool flag = (arg_84_0 ?? 0) > 0;
            if (flag)
            {
                foreach (Hediff current in list)
                {
                    if (current.def.defName == "TM_HediffInvulnerable")
                    {
                        absorbed = true;
                        MoteMaker.MakeStaticMote(AbilityUser.Position, AbilityUser.Map, ThingDefOf.Mote_ExplosionFlash, 10);
                        dinfo.SetAmount(0);
                        return;
                    }
                    if(current.def.defName == "TM_PsionicHD")
                    {
                        if(dinfo.Def == TMDamageDefOf.DamageDefOf.TM_PsionicInjury)
                        {
                            absorbed = true;
                            dinfo.SetAmount(0);
                            return;
                        }
                    }
                    if (current.def.defName == "TM_ReversalHD")
                    {
                        Pawn instigator = dinfo.Instigator as Pawn;
                        if (instigator != null)
                        {
                            if (instigator.equipment.PrimaryEq != null)
                            {
                                if (instigator.equipment.PrimaryEq.PrimaryVerb != null)
                                {
                                    absorbed = true;
                                    Vector3 drawPos = AbilityUser.DrawPos;
                                    drawPos.x += ((instigator.DrawPos.x - drawPos.x) / 20f) + Rand.Range(-.2f, .2f);
                                    drawPos.z += ((instigator.DrawPos.z - drawPos.z) / 20f) + Rand.Range(-.2f, .2f);
                                    TM_MoteMaker.ThrowSparkFlashMote(drawPos, this.Pawn.Map, 2f);                                    
                                    DoReversal(dinfo);
                                    dinfo.SetAmount(0);
                                    MightPowerSkill ver = this.MightData.MightPowerSkill_Reversal.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Reversal_ver");
                                    if(ver.level > 0)
                                    {
                                        SiphonReversal(ver.level);
                                    }
                                    return;
                                }
                            }
                        }
                    }
                    if (current.def.defName == "TM_HediffEnchantment_phantomShift" && Rand.Chance(.2f))
                    {
                        absorbed = true;
                        MoteMaker.MakeStaticMote(AbilityUser.Position, AbilityUser.Map, ThingDefOf.Mote_ExplosionFlash, 8);
                        MoteMaker.ThrowSmoke(abilityUser.Position.ToVector3Shifted(), abilityUser.Map, 1.2f);
                        dinfo.SetAmount(0);
                        return;
                    }
                    if (arcaneRes != 0 && resMitigationDelay < this.age)
                    {
                        if (current.def.defName == "TM_HediffEnchantment_arcaneRes")
                        {
                            if (dinfo.Def.defName.Contains("TM_") || dinfo.Def.defName == "FrostRay" || dinfo.Def.defName == "Snowball" || dinfo.Def.defName == "Iceshard" || dinfo.Def.defName == "Firebolt")
                            {
                                absorbed = true;
                                int actualDmg = Mathf.RoundToInt(dinfo.Amount - (dinfo.Amount * arcaneRes));
                                resMitigationDelay = this.age + 10;
                                dinfo.SetAmount(actualDmg);
                                abilityUser.TakeDamage(dinfo);
                                return;
                            }
                        }
                    }
                    if (fortitudeMitigationDelay < this.age )
                    {
                        if (current.def.defName == "TM_HediffFortitude")
                        {
                            MightPowerSkill pwr = this.MightData.MightPowerSkill_Fortitude.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Fortitude_pwr");
                            MightPowerSkill ver = this.MightData.MightPowerSkill_Fortitude.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Fortitude_ver");
                            absorbed = true;
                            int mitigationAmt = 2 + (2 * pwr.level);
                            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                            if (settingsRef.AIHardMode && !abilityUser.IsColonist)
                            {
                                mitigationAmt = 8;
                            }
                            float actualDmg;
                            float dmgAmt = dinfo.Amount;
                            this.Stamina.GainNeed((.005f * dmgAmt) + (.002f * (float)ver.level));
                            if (dmgAmt < mitigationAmt)
                            {
                                actualDmg = 0;
                                return;
                            }
                            else
                            {
                                actualDmg = dmgAmt - mitigationAmt;
                            }
                            fortitudeMitigationDelay = this.age + 10;
                            dinfo.SetAmount(actualDmg);
                            abilityUser.TakeDamage(dinfo);
                            return;
                        }
                        base.PostPreApplyDamage(dinfo, out absorbed);
                    }
                }
            }
            list.Clear();
            list = null;
            //}
            base.PostPreApplyDamage(dinfo, out absorbed);
        }

        public void DoReversal(DamageInfo dinfo)
        {
            Thing instigator = dinfo.Instigator;
            
            if(instigator is Pawn)
            {
                Pawn shooterPawn = instigator as Pawn;
                if (!dinfo.Weapon.IsMeleeWeapon && dinfo.WeaponBodyPartGroup == null)
                {
                    TM_CopyAndLaunchProjectile.CopyAndLaunchThing(shooterPawn.equipment.PrimaryEq.PrimaryVerb.verbProps.defaultProjectile, this.Pawn, instigator, shooterPawn, ProjectileHitFlags.IntendedTarget, null);
                }
            }
            
            //GiveReversalJob(dinfo);            
        }

        public void SiphonReversal(int verVal)
        {
            Pawn pawn = this.Pawn;
            CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();
            comp.Stamina.CurLevel += (.015f * verVal);         
            int num = 1 + verVal;
            using (IEnumerator<BodyPartRecord> enumerator = pawn.health.hediffSet.GetInjuredParts().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    BodyPartRecord rec = enumerator.Current;
                    bool flag2 = num > 0;
                    if (flag2)
                    {
                        int num2 = 1 + verVal;
                        ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                        if (!pawn.IsColonist && settingsRef.AIHardMode)
                        {
                            num2 = 5;
                        }
                        IEnumerable<Hediff_Injury> arg_BB_0 = pawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                        Func<Hediff_Injury, bool> arg_BB_1;
                        arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                        foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                        {
                            bool flag4 = num2 > 0;
                            if (flag4)
                            {
                                bool flag5 = current.CanHealNaturally() && !current.IsPermanent();
                                if (flag5)
                                {
                                    if (!pawn.IsColonist)
                                    {
                                        current.Heal(20.0f + (float)verVal * 3f); // power affects how much to heal
                                    }
                                    else
                                    {
                                        current.Heal((2.0f + (float)verVal * 1f)); // power affects how much to heal
                                    }
                                    TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3Shifted(), pawn.Map, .6f);
                                    TM_MoteMaker.ThrowRegenMote(pawn.Position.ToVector3Shifted(), pawn.Map, .4f);
                                    num--;
                                    num2--;
                                }
                            }
                        }
                    }
                }
            }
            
        }

        public void GiveReversalJob(DamageInfo dinfo)  // buggy AF due to complications with CompDeflector
        {
            try
            {
                Pawn pawn;
                bool flag = (pawn = (dinfo.Instigator as Pawn)) != null && dinfo.Weapon != null;
                if (flag)
                {
                    if (dinfo.Weapon.IsMeleeWeapon || dinfo.WeaponBodyPartGroup != null)
                    {                        
                        reversal_dinfo = new DamageInfo(dinfo.Def, dinfo.Amount, dinfo.ArmorPenetrationInt, dinfo.Angle - 180, this.Pawn, dinfo.HitPart, dinfo.Weapon, DamageInfo.SourceCategory.ThingOrUnknown);
                        reversalTarget = dinfo.Instigator;
                    }
                    else
                    {
                        Job job = new Job(CompDeflectorDefOf.CastDeflectVerb)
                        {
                            playerForced = true,
                            locomotionUrgency = LocomotionUrgency.Sprint
                        };
                        bool flag2 = pawn.equipment != null;
                        if (flag2)
                        {
                            CompEquippable primaryEq = pawn.equipment.PrimaryEq;
                            bool flag3 = primaryEq != null;
                            if (flag3)
                            {
                                bool flag4 = primaryEq.PrimaryVerb != null;
                                if (flag4)
                                {
                                    Verb_Deflected verb_Deflected = (Verb_Deflected)this.CopyAndReturnNewVerb(primaryEq.PrimaryVerb);
                                    //verb_Deflected = this.ReflectionHandler(verb_Deflected);
                                    //Log.Message("verb deflected with properties is " + verb_Deflected.ToString()); //throwing an error, so nothing is happening in jobdriver_castdeflectverb
                                    pawn = dinfo.Instigator as Pawn;
                                    job.targetA = pawn;
                                    job.verbToUse = verb_Deflected;
                                    job.killIncappedTarget = pawn.Downed;
                                    this.Pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                                }
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {
            }
        }

        public Verb CopyAndReturnNewVerb(Verb newVerb = null)
        {
            if (newVerb != null)
            {
                deflectVerb = newVerb as Verb_Deflected;
                deflectVerb = (Verb_Deflected)Activator.CreateInstance(typeof(Verb_Deflected));
                deflectVerb.caster = this.Pawn;
                

                //Initialize VerbProperties
                var newVerbProps = new VerbProperties
                {
                    //Copy values over to a new verb props
                    
                    hasStandardCommand = newVerb.verbProps.hasStandardCommand,
                    defaultProjectile = newVerb.verbProps.defaultProjectile,
                    range = newVerb.verbProps.range,
                    muzzleFlashScale = newVerb.verbProps.muzzleFlashScale,                    
                    warmupTime = 0,
                    defaultCooldownTime = 0,
                    soundCast = SoundDefOf.MetalHitImportant,
                    impactMote = newVerb.verbProps.impactMote,
                    label = newVerb.verbProps.label,
                    ticksBetweenBurstShots = 0,
                    rangedFireRulepack = RulePackDef.Named("TM_Combat_Reflection"),
                    accuracyLong = 70f * Rand.Range(1f, 2f),
                    accuracyMedium = 80f * Rand.Range(1f, 2f),
                    accuracyShort = 90f * Rand.Range(1f, 2f)
                };

                //Apply values
                deflectVerb.verbProps = newVerbProps;
            }
            else
            {
                if (deflectVerb != null) return deflectVerb;
                deflectVerb = (Verb_Deflected)Activator.CreateInstance(typeof(Verb_Deflected));
                deflectVerb.caster = this.Pawn;
                deflectVerb.verbProps = newVerb.verbProps;
            }
            return deflectVerb;
        }

        public Verb_Deflected ReflectionHandler(Verb_Deflected newVerb)
        {
            VerbProperties verbProperties = new VerbProperties
            {
                hasStandardCommand = newVerb.verbProps.hasStandardCommand,
                defaultProjectile = newVerb.verbProps.defaultProjectile,
                range = newVerb.verbProps.range,
                muzzleFlashScale = newVerb.verbProps.muzzleFlashScale,
                warmupTime = 0f,
                defaultCooldownTime = 0f,
                soundCast = SoundDefOf.MetalHitImportant,
                accuracyLong = 70f * Rand.Range(1f, 2f),
                accuracyMedium = 80f * Rand.Range(1f, 2f),
                accuracyShort = 90f * Rand.Range(1f, 2f)
            };

            newVerb.verbProps = verbProperties;
            return newVerb;
        }

        public void ResolveReversalDamage()
        {
            reversalTarget.TakeDamage(reversal_dinfo);
            reversalTarget = null;
        }

        public void ResolveAutoCast()
        {
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (settingsRef.autocastEnabled && !this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless) && this.Pawn.CurJob != null && this.Pawn.CurJob.def.defName != "TMCastAbilityVerb" && this.Pawn.CurJob.def.defName != "Ingest" && this.Pawn.GetPosture() == PawnPosture.Standing)
            {
                //Log.Message("pawn " + this.Pawn.LabelShort + " current job is " + this.Pawn.CurJob.def.defName);
                //non-combat (undrafted) spells    
                bool castSuccess = false;
                if (!this.Pawn.Drafted && this.Stamina.CurLevelPercentage >= settingsRef.autocastMinThreshold)
                {
                    //Hunting only
                    if (this.Pawn.CurJob.def.defName == "Hunt" && this.Pawn.CurJob.targetA != null)
                    {
                        if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) && !this.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                        {
                            PawnAbility ability = null;
                            foreach (MightPower current in this.MightData.MightPowersR)
                            {
                                if (current.abilityDef != null)
                                {
                                    if ((current.abilityDef == TorannMagicDefOf.TM_ArrowStorm || current.abilityDef == TorannMagicDefOf.TM_ArrowStorm_I || current.abilityDef == TorannMagicDefOf.TM_ArrowStorm_II || current.abilityDef == TorannMagicDefOf.TM_ArrowStorm_III))
                                    {
                                        if (this.Pawn.equipment.Primary != null && this.Pawn.equipment.Primary.def.IsRangedWeapon)
                                        {
                                            Thing wpn = this.Pawn.equipment.Primary;

                                            if (wpn.def.Verbs.FirstOrDefault<VerbProperties>().defaultProjectile.projectile.damageDef.defName == "Arrow" || wpn.def.defName.Contains("Bow") || wpn.def.defName.Contains("bow") || wpn.def.Verbs.FirstOrDefault<VerbProperties>().defaultProjectile.projectile.damageDef.defName.Contains("Arrow") || wpn.def.Verbs.FirstOrDefault<VerbProperties>().defaultProjectile.projectile.damageDef.defName.Contains("arrow"))
                                            {
                                                if (current.level == 0)
                                                {
                                                    MightPower mightPower = this.MightData.MightPowersR.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ArrowStorm);
                                                    if (mightPower != null && mightPower.learned && mightPower.autocast)
                                                    {
                                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ArrowStorm);
                                                        AutoCast.CombatAbility_OnTarget.TryExecute(this, TorannMagicDefOf.TM_ArrowStorm, ability, mightPower, this.Pawn.CurJob.targetA, 4, out castSuccess);
                                                        if (castSuccess) goto AutoCastExit;
                                                    }
                                                }
                                                else if (current.level == 1)
                                                {
                                                    MightPower mightPower = this.MightData.MightPowersR.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ArrowStorm);
                                                    if (mightPower != null && mightPower.learned && mightPower.autocast)
                                                    {
                                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ArrowStorm_I);
                                                        AutoCast.CombatAbility_OnTarget.TryExecute(this, TorannMagicDefOf.TM_ArrowStorm_I, ability, mightPower, this.Pawn.CurJob.targetA, 4, out castSuccess);
                                                        if (castSuccess) goto AutoCastExit;
                                                    }

                                                }
                                                else if (current.level == 2)
                                                {
                                                    MightPower mightPower = this.MightData.MightPowersR.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ArrowStorm_I);
                                                    if (mightPower != null && mightPower.learned && mightPower.autocast)
                                                    {
                                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ArrowStorm_II);
                                                        AutoCast.CombatAbility_OnTarget.TryExecute(this, TorannMagicDefOf.TM_ArrowStorm_II, ability, mightPower, this.Pawn.CurJob.targetA, 4, out castSuccess);
                                                        if (castSuccess) goto AutoCastExit;
                                                    }
                                                }
                                                else
                                                {
                                                    MightPower mightPower = this.MightData.MightPowersR.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ArrowStorm_II);
                                                    if (mightPower != null && mightPower.learned && mightPower.autocast)
                                                    {
                                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ArrowStorm_III);
                                                        AutoCast.CombatAbility_OnTarget.TryExecute(this, TorannMagicDefOf.TM_ArrowStorm_III, ability, mightPower, this.Pawn.CurJob.targetA, 4, out castSuccess);
                                                        if (castSuccess) goto AutoCastExit;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper) && !this.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                        {
                            PawnAbility ability = null;
                            foreach (MightPower current in this.MightData.MightPowersS)
                            {
                                if (current.abilityDef != null)
                                {
                                    if (this.Pawn.equipment.Primary != null && this.Pawn.equipment.Primary.def.IsRangedWeapon)
                                    {
                                        if (current.abilityDef == TorannMagicDefOf.TM_Headshot)
                                        {
                                            MightPower mightPower = this.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Headshot);
                                            if (mightPower != null && mightPower.autocast)
                                            {
                                                ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Headshot);
                                                AutoCast.CombatAbility_OnTarget.TryExecute(this, TorannMagicDefOf.TM_Headshot, ability, mightPower, this.Pawn.CurJob.targetA, 4, out castSuccess);
                                                if (castSuccess) goto AutoCastExit;
                                            }
                                        }                                        
                                    }
                                }
                            }
                        }
                        if (this.skill_Teach)
                        {
                            MightPower mightPower = this.MightData.MightPowersStandalone.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_TeachMight);
                            if (mightPower.autocast)
                            {
                                PawnAbility ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_TeachMight);
                                AutoCast.TeachMight.Evaluate(this, TorannMagicDefOf.TM_TeachMight, ability, mightPower, out castSuccess);
                                if (castSuccess) goto AutoCastExit;
                            }
                        }
                    }
                }

                //combat (drafted) spells
                if (this.Pawn.Drafted && this.Stamina.CurLevelPercentage >= settingsRef.autocastCombatMinThreshold && this.Pawn.CurJob.def != JobDefOf.Goto)
                {
                    if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) && !this.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                    {
                        PawnAbility ability = null;
                        foreach (MightPower current in this.MightData.MightPowersB)
                        {
                            if (current.abilityDef != null)
                            {
                                if (current.abilityDef == TorannMagicDefOf.TM_BladeSpin)
                                {
                                    MightPower mightPower = this.MightData.MightPowersB.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_BladeSpin);
                                    if (mightPower != null && mightPower.autocast && this.Pawn.equipment.Primary != null && !this.Pawn.equipment.Primary.def.IsRangedWeapon)
                                    {
                                        ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_BladeSpin);
                                        MightPowerSkill ver = this.MightData.MightPowerSkill_SeismicSlash.FirstOrDefault((MightPowerSkill x) => x.label == "TM_SeismicSlash_ver");
                                        AutoCast.AoECombat.Evaluate(this, TorannMagicDefOf.TM_BladeSpin, ability, mightPower, 2, Mathf.RoundToInt(2+(.5f*ver.level)), this.Pawn.Position, true, out castSuccess);
                                        if (castSuccess) goto AutoCastExit;
                                    }
                                }
                            }
                        }
                    }
                    if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator) && !this.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                    {
                        PawnAbility ability = null;
                        foreach (MightPower current in this.MightData.MightPowersG)
                        {
                            if (current.abilityDef != null)
                            {
                                if ((current.abilityDef == TorannMagicDefOf.TM_Grapple || current.abilityDef == TorannMagicDefOf.TM_Grapple_I || current.abilityDef == TorannMagicDefOf.TM_Grapple_II || current.abilityDef == TorannMagicDefOf.TM_Grapple_III))
                                {
                                    if (current.level == 0)
                                    {
                                        MightPower mightPower = this.MightData.MightPowersG.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Grapple);
                                        if (mightPower != null && mightPower.learned && mightPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Grapple);
                                            AutoCast.CombatAbility.Evaluate(this, TorannMagicDefOf.TM_Grapple, ability, mightPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else if (current.level == 1)
                                    {
                                        MightPower mightPower = this.MightData.MightPowersG.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Grapple);
                                        if (mightPower != null && mightPower.learned && mightPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Grapple_I);
                                            AutoCast.CombatAbility.Evaluate(this, TorannMagicDefOf.TM_Grapple_I, ability, mightPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }

                                    }
                                    else if (current.level == 2)
                                    {
                                        MightPower mightPower = this.MightData.MightPowersG.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Grapple_I);
                                        if (mightPower != null && mightPower.learned && mightPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Grapple_II);
                                            AutoCast.CombatAbility.Evaluate(this, TorannMagicDefOf.TM_Grapple_II, ability, mightPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    else
                                    {
                                        MightPower mightPower = this.MightData.MightPowersG.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Grapple_II);
                                        if (mightPower != null && mightPower.learned && mightPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Grapple_III);
                                            AutoCast.CombatAbility.Evaluate(this, TorannMagicDefOf.TM_Grapple_III, ability, mightPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) && !this.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                    {
                        PawnAbility ability = null;
                        foreach (MightPower current in this.MightData.MightPowersR)
                        {
                            if (current.abilityDef != null)
                            {
                                if ((current.abilityDef == TorannMagicDefOf.TM_ArrowStorm || current.abilityDef == TorannMagicDefOf.TM_ArrowStorm_I || current.abilityDef == TorannMagicDefOf.TM_ArrowStorm_II || current.abilityDef == TorannMagicDefOf.TM_ArrowStorm_III))
                                {
                                    if (this.Pawn.equipment.Primary != null && this.Pawn.equipment.Primary.def.IsRangedWeapon)
                                    {
                                        Thing wpn = this.Pawn.equipment.Primary;

                                        if (wpn.def.Verbs.FirstOrDefault<VerbProperties>().defaultProjectile.projectile.damageDef.defName == "Arrow" || wpn.def.defName.Contains("Bow") || wpn.def.defName.Contains("bow") || wpn.def.Verbs.FirstOrDefault<VerbProperties>().defaultProjectile.projectile.damageDef.defName.Contains("Arrow") || wpn.def.Verbs.FirstOrDefault<VerbProperties>().defaultProjectile.projectile.damageDef.defName.Contains("arrow"))
                                        {
                                            if (current.level == 0)
                                            {
                                                MightPower mightPower = this.MightData.MightPowersR.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ArrowStorm);
                                                if (mightPower != null && mightPower.learned && mightPower.autocast)
                                                {
                                                    ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ArrowStorm);
                                                    AutoCast.CombatAbility.Evaluate(this, TorannMagicDefOf.TM_ArrowStorm, ability, mightPower, out castSuccess);
                                                    if (castSuccess) goto AutoCastExit;
                                                }
                                            }
                                            else if (current.level == 1)
                                            {
                                                MightPower mightPower = this.MightData.MightPowersR.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ArrowStorm);
                                                if (mightPower != null && mightPower.learned && mightPower.autocast)
                                                {
                                                    ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ArrowStorm_I);
                                                    AutoCast.CombatAbility.Evaluate(this, TorannMagicDefOf.TM_ArrowStorm_I, ability, mightPower, out castSuccess);
                                                    if (castSuccess) goto AutoCastExit;
                                                }

                                            }
                                            else if (current.level == 2)
                                            {
                                                MightPower mightPower = this.MightData.MightPowersR.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ArrowStorm_I);
                                                if (mightPower != null && mightPower.learned && mightPower.autocast)
                                                {
                                                    ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ArrowStorm_II);
                                                    AutoCast.CombatAbility.Evaluate(this, TorannMagicDefOf.TM_ArrowStorm_II, ability, mightPower, out castSuccess);
                                                    if (castSuccess) goto AutoCastExit;
                                                }
                                            }
                                            else
                                            {
                                                MightPower mightPower = this.MightData.MightPowersR.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_ArrowStorm_II);
                                                if (mightPower != null && mightPower.learned && mightPower.autocast)
                                                {
                                                    ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_ArrowStorm_III);
                                                    AutoCast.CombatAbility.Evaluate(this, TorannMagicDefOf.TM_ArrowStorm_III, ability, mightPower, out castSuccess);
                                                    if (castSuccess) goto AutoCastExit;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper) && !this.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                    {
                        PawnAbility ability = null;
                        foreach (MightPower current in this.MightData.MightPowersS)
                        {
                            if (current.abilityDef != null)
                            {
                                if(this.Pawn.equipment.Primary != null && this.Pawn.equipment.Primary.def.IsRangedWeapon)
                                {
                                    if (current.abilityDef == TorannMagicDefOf.TM_AntiArmor)
                                    {
                                        MightPower mightPower = this.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_AntiArmor);
                                        if (mightPower != null && mightPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_AntiArmor);
                                            AutoCast.CombatAbility.Evaluate(this, TorannMagicDefOf.TM_AntiArmor, ability, mightPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    if (current.abilityDef == TorannMagicDefOf.TM_Headshot)
                                    {
                                        MightPower mightPower = this.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_Headshot);
                                        if (mightPower != null && mightPower.autocast)
                                        {
                                            ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_Headshot);
                                            AutoCast.CombatAbility.Evaluate(this, TorannMagicDefOf.TM_Headshot, ability, mightPower, out castSuccess);
                                            if (castSuccess) goto AutoCastExit;
                                        }
                                    }
                                    if ((current.abilityDef == TorannMagicDefOf.TM_DisablingShot || current.abilityDef == TorannMagicDefOf.TM_DisablingShot_I || current.abilityDef == TorannMagicDefOf.TM_DisablingShot_II || current.abilityDef == TorannMagicDefOf.TM_DisablingShot_III))
                                    {
                                        if (current.level == 0)
                                        {
                                            MightPower mightPower = this.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_DisablingShot);
                                            if (mightPower != null && mightPower.learned && mightPower.autocast)
                                            {
                                                ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_DisablingShot);
                                                AutoCast.CombatAbility.Evaluate(this, TorannMagicDefOf.TM_DisablingShot, ability, mightPower, out castSuccess);
                                                if (castSuccess) goto AutoCastExit;
                                            }
                                        }
                                        else if (current.level == 1)
                                        {
                                            MightPower mightPower = this.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_DisablingShot);
                                            if (mightPower != null && mightPower.learned && mightPower.autocast)
                                            {
                                                ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_DisablingShot_I);
                                                AutoCast.CombatAbility.Evaluate(this, TorannMagicDefOf.TM_DisablingShot_I, ability, mightPower, out castSuccess);
                                                if (castSuccess) goto AutoCastExit;
                                            }

                                        }
                                        else if (current.level == 2)
                                        {
                                            MightPower mightPower = this.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_DisablingShot_I);
                                            if (mightPower != null && mightPower.learned && mightPower.autocast)
                                            {
                                                ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_DisablingShot_II);
                                                AutoCast.CombatAbility.Evaluate(this, TorannMagicDefOf.TM_DisablingShot_II, ability, mightPower, out castSuccess);
                                                if (castSuccess) goto AutoCastExit;
                                            }
                                        }
                                        else
                                        {
                                            MightPower mightPower = this.MightData.MightPowersS.FirstOrDefault<MightPower>((MightPower x) => x.abilityDef == TorannMagicDefOf.TM_DisablingShot_II);
                                            if (mightPower != null && mightPower.learned && mightPower.autocast)
                                            {
                                                ability = this.AbilityData.Powers.FirstOrDefault((PawnAbility x) => x.Def == TorannMagicDefOf.TM_DisablingShot_III);
                                                AutoCast.CombatAbility.Evaluate(this, TorannMagicDefOf.TM_DisablingShot_III, ability, mightPower, out castSuccess);
                                                if (castSuccess) goto AutoCastExit;
                                            }
                                        }
                                    }                                    
                                }
                            }
                        }
                    }
                }
                AutoCastExit:;
            }
        }

        public void ResolveClassSkills()               
        {
            if (this.MightUserLevel >= 20 && this.skill_Teach == false)
            {
                this.AddPawnAbility(TorannMagicDefOf.TM_TeachMight);
                this.skill_Teach = true;
            }

            if (this.IsMightUser && !this.Pawn.Dead && !this.Pawn.Downed)
            {
                if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer))
                {
                    MightPowerSkill bladefocus_pwr = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_BladeFocus.FirstOrDefault((MightPowerSkill x) => x.label == "TM_BladeFocus_pwr");

                    List<Trait> traits = this.Pawn.story.traits.allTraits;
                    for (int i = 0; i < traits.Count; i++)
                    {
                        if (traits[i].def.defName == "Bladedancer")
                        {
                            if (traits[i].Degree != bladefocus_pwr.level)
                            {
                                traits.Remove(traits[i]);
                                this.Pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Bladedancer"), bladefocus_pwr.level, false));
                                MoteMaker.ThrowHeatGlow(this.Pawn.Position, this.Pawn.Map, 2);
                            }
                        }
                    }
                }

                if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator))
                {
                    if (!this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_HediffFortitude))
                    {
                        HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_HediffFortitude, -5f);
                        HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_HediffFortitude, 1f);
                    }
                }

                if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger))
                {
                    MightPowerSkill rangertraining_pwr = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_RangerTraining.FirstOrDefault((MightPowerSkill x) => x.label == "TM_RangerTraining_pwr");

                    List<Trait> traits = this.Pawn.story.traits.allTraits;
                    for (int i = 0; i < traits.Count; i++)
                    {
                        if (traits[i].def.defName == "Ranger")
                        {

                            if (traits[i].Degree != rangertraining_pwr.level)
                            {
                                traits.Remove(traits[i]);
                                this.Pawn.story.traits.GainTrait(new Trait(TraitDef.Named("Ranger"), rangertraining_pwr.level, false));
                                MoteMaker.ThrowHeatGlow(this.Pawn.Position, this.Pawn.Map, 2);
                            }
                        }
                    }
                }

                if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper))
                {
                    MightPowerSkill sniperfocus_pwr = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_SniperFocus.FirstOrDefault((MightPowerSkill x) => x.label == "TM_SniperFocus_pwr");

                    List<Trait> traits = this.Pawn.story.traits.allTraits;
                    for (int i = 0; i < traits.Count; i++)
                    {
                        if (traits[i].def.defName == "TM_Sniper")
                        {
                            if (traits[i].Degree != sniperfocus_pwr.level)
                            {
                                traits.Remove(traits[i]);
                                this.Pawn.story.traits.GainTrait(new Trait(TraitDef.Named("TM_Sniper"), sniperfocus_pwr.level, false));
                                MoteMaker.ThrowHeatGlow(base.Pawn.Position, this.Pawn.Map, 2);
                            }
                        }
                    }
                }

                if(this.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic))
                {
                    if (!this.Pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_PsionicHD"), false))
                    {
                        HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_PsionicHD"), 1f);
                    }
                }

                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) && !this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_BladeArtHD))
                {
                    MightPowerSkill bladeart_pwr = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_BladeArt.FirstOrDefault((MightPowerSkill x) => x.label == "TM_BladeArt_pwr");

                    //HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BladeArtHD, -5f);
                    HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BladeArtHD, (.5f) + bladeart_pwr.level);
                    if (!this.Pawn.IsColonist && settingsRef.AIHardMode)
                    {
                        HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BladeArtHD, 4);
                    }
                }
                else if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) && !this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_BowTrainingHD))
                {
                    MightPowerSkill bowtraining_pwr = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_BowTraining.FirstOrDefault((MightPowerSkill x) => x.label == "TM_BowTraining_pwr");

                    //HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BowTrainingHD, -5f);
                    HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BowTrainingHD, (.5f) + bowtraining_pwr.level);
                    if (!this.Pawn.IsColonist && settingsRef.AIHardMode)
                    {
                        HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BowTrainingHD, 4);
                    }

                }
                else
                {
                    using (IEnumerator<Hediff> enumerator = this.Pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Hediff rec = enumerator.Current;

                            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) && rec.def == TorannMagicDefOf.TM_BladeArtHD && this.Pawn.IsColonist)
                            {
                                MightPowerSkill bladeart_pwr = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_BladeArt.FirstOrDefault((MightPowerSkill x) => x.label == "TM_BladeArt_pwr");
                                if (rec.Severity < (float)(.5f + bladeart_pwr.level) || rec.Severity > (float)(.6f + bladeart_pwr.level))
                                {
                                    HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BladeArtHD, -5f);
                                    HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BladeArtHD, (.5f) + bladeart_pwr.level);
                                    MoteMaker.ThrowDustPuff(this.Pawn.Position.ToVector3Shifted(), this.Pawn.Map, .6f);
                                    MoteMaker.ThrowHeatGlow(this.Pawn.Position, this.Pawn.Map, 1.6f);
                                }
                            }

                            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) && rec.def == TorannMagicDefOf.TM_BowTrainingHD && this.Pawn.IsColonist)
                            {
                                MightPowerSkill bowtraining_pwr = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_BowTraining.FirstOrDefault((MightPowerSkill x) => x.label == "TM_BowTraining_pwr");
                                if (rec.Severity < (float)(.5f + bowtraining_pwr.level) || rec.Severity > (float)(.6f + bowtraining_pwr.level))
                                {
                                    HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BowTrainingHD, -5f);
                                    HealthUtility.AdjustSeverity(this.Pawn, TorannMagicDefOf.TM_BowTrainingHD, (.5f) + bowtraining_pwr.level);
                                    MoteMaker.ThrowDustPuff(this.Pawn.Position.ToVector3Shifted(), this.Pawn.Map, .6f);
                                    MoteMaker.ThrowHeatGlow(this.Pawn.Position, this.Pawn.Map, 1.6f);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ResolveClassPassions()
        {
            SkillRecord skill;
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer))
            {                
                skill = this.Pawn.skills.GetSkill(SkillDefOf.Melee);
                if (skill.passion != Passion.Major)
                {
                    skill.passion = Passion.Major;
                }
                skill = this.Pawn.skills.GetSkill(SkillDefOf.Shooting);
                if (skill.passion != Passion.None)
                {
                    skill.passion = Passion.None;
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper))
            {
                skill = this.Pawn.skills.GetSkill(SkillDefOf.Melee);
                if (skill.passion != Passion.None)
                {
                    skill.passion = Passion.None;
                }
                skill = this.Pawn.skills.GetSkill(SkillDefOf.Shooting);
                if (skill.passion != Passion.Major)
                {
                    skill.passion = Passion.Major;
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator))
            {
                skill = this.Pawn.skills.GetSkill(SkillDefOf.Melee);
                if (skill.passion == Passion.None)
                {
                    skill.passion = Passion.Minor;
                }
                skill = this.Pawn.skills.GetSkill(SkillDefOf.Shooting);
                if (skill.passion == Passion.None)
                {
                    skill.passion = Passion.Minor;
                }
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger))
            {
                skill = this.Pawn.skills.GetSkill(SkillDefOf.Melee);
                if (skill.passion == Passion.None)
                {
                    skill.passion = Passion.Minor;
                }
                skill = this.Pawn.skills.GetSkill(SkillDefOf.Shooting);
                if (skill.passion == Passion.None)
                {
                    skill.passion = Passion.Minor;
                }
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            return base.CompGetGizmosExtra();
        }


        private void ResolveSustainedSkills()
        {
            float _maxSP = 0;
            float _spRegenRate = 0;
            float _coolDown = 0;
            float _spCost = 0;
            float _xpGain = 0;
            float _arcaneRes = 0;
            float _arcaneDmg = 0;
            bool _arcaneSpectre = false;
            bool _phantomShift = false;

            List<Apparel> apparel = this.Pawn.apparel.WornApparel;
            for (int i = 0; i < this.Pawn.apparel.WornApparelCount; i++)
            {
                Enchantment.CompEnchantedItem item = apparel[i].GetComp<Enchantment.CompEnchantedItem>();
                if (item != null)
                {
                    if (item.HasEnchantment)
                    {
                        _maxSP += item.maxMP;
                        _spRegenRate += item.mpRegenRate;
                        _coolDown += item.coolDown;
                        _xpGain += item.xpGain;
                        _spCost += item.mpCost;
                        _arcaneRes += item.arcaneRes;
                        _arcaneDmg += item.arcaneDmg;
                        if (item.arcaneSpectre == true)
                        {
                            _arcaneSpectre = true;
                        }
                        if (item.phantomShift == true)
                        {
                            _phantomShift = true;
                        }
                    }
                }
            }
            if (this.Pawn.equipment.Primary != null)
            {
                Enchantment.CompEnchantedItem item = this.Pawn.equipment.Primary.GetComp<Enchantment.CompEnchantedItem>();
                if (item != null)
                {
                    if (item.HasEnchantment)
                    {
                        _maxSP += item.maxMP;
                        _spRegenRate += item.mpRegenRate;
                        _coolDown += item.coolDown;
                        _xpGain += item.xpGain;
                        _spCost += item.mpCost;
                        _arcaneRes += item.arcaneRes;
                        _arcaneDmg += item.arcaneDmg;
                    }
                }
            }

            using (IEnumerator<Hediff> enumerator = this.Pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Hediff rec = enumerator.Current;
                    if (rec.def.defName == ("TM_HediffSprint"))
                    {
                        MightPowerSkill eff = this.Pawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Sprint.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Sprint_eff");
                        _maxSP -= .3f * (1 - (.1f * eff.level));
                        //Catch negative values
                        if (this.maxSP < 0)
                        {
                            this.Pawn.health.RemoveHediff(rec);
                            Log.Message("Removed " + rec.def.LabelCap + ", insufficient stamina to maintain.");
                        }
                    }
                    if (rec.def.defName == "TM_HediffGearRepair")
                    {
                        _maxSP -= .2f;
                        //Catch negative values
                        if (this.maxSP < 0)
                        {
                            this.Pawn.health.RemoveHediff(rec);
                            Log.Message("Removed " + rec.def.LabelCap + ", insufficient stamina to maintain.");
                        }
                    }
                    if (rec.def.defName == "TM_HediffInnerHealing")
                    {
                        _maxSP -= .1f;
                        //Catch negative values
                        if (this.maxSP < 0)
                        {
                            this.Pawn.health.RemoveHediff(rec);
                            Log.Message("Removed " + rec.def.LabelCap + ", insufficient stamina to maintain.");
                        }
                    }
                    if (rec.def.defName == "TM_HediffHeavyBlow")
                    {
                        _maxSP -= .3f;
                        //Catch negative values
                        if (this.maxSP < 0)
                        {
                            this.Pawn.health.RemoveHediff(rec);
                            Log.Message("Removed " + rec.def.LabelCap + ", insufficient stamina to maintain.");
                        }
                    }
                    if (rec.def.defName == "TM_HediffStrongBack")
                    {
                        _maxSP -= .1f;
                        //Catch negative values
                        if (this.maxSP < 0)
                        {
                            this.Pawn.health.RemoveHediff(rec);
                            Log.Message("Removed " + rec.def.LabelCap + ", insufficient stamina to maintain.");
                        }
                    }
                    if (rec.def.defName == "TM_HediffThickSkin")
                    {
                        _maxSP -= .3f;
                        //Catch negative values
                        if (this.maxSP < 0)
                        {
                            this.Pawn.health.RemoveHediff(rec);
                            Log.Message("Removed " + rec.def.LabelCap + ", insufficient stamina to maintain.");
                        }
                    }
                    if (rec.def.defName == "TM_HediffFightersFocus")
                    {
                        _maxSP -= .15f;
                        //Catch negative values
                        if (this.maxSP < 0)
                        {
                            this.Pawn.health.RemoveHediff(rec);
                            Log.Message("Removed " + rec.def.LabelCap + ", insufficient stamina to maintain.");
                        }
                    }
                }
            }
            if (this.bondedPet != null)
            {
                _maxSP -= .30f;
                if (this.bondedPet.Dead || this.bondedPet.Destroyed)
                {
                    this.Pawn.needs.mood.thoughts.memories.TryGainMemory(TorannMagicDefOf.RangerPetDied, null);
                    this.bondedPet = null;
                }
                else if (this.bondedPet.Faction != null && this.bondedPet.Faction != this.Pawn.Faction)
                {
                    //sold? punish evil
                    this.Pawn.needs.mood.thoughts.memories.TryGainMemory(TorannMagicDefOf.RangerSoldBondedPet, null);
                    this.bondedPet = null;
                }
            }
            if(this.Pawn.needs.mood.thoughts.memories.NumMemoriesOfDef(ThoughtDef.Named("RangerSoldBondedPet")) > 0)
            {
                if(this.animalBondingDisabled == false)
                {
                    this.RemovePawnAbility(TorannMagicDefOf.TM_AnimalFriend);
                    this.animalBondingDisabled = true;
                }
            }
            else
            {
                if(this.animalBondingDisabled == true)
                {
                    this.AddPawnAbility(TorannMagicDefOf.TM_AnimalFriend);
                    this.animalBondingDisabled = false;
                }
            }
            if(MightData.MightAbilityPoints < 0)
            {
                MightData.MightAbilityPoints = 0;
            }

            MightPowerSkill endurance = this.MightData.MightPowerSkill_global_endurance.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_endurance_pwr");
            MightPowerSkill fitness = this.MightData.MightPowerSkill_global_refresh.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_refresh_pwr");
            MightPowerSkill coordination = this.MightData.MightPowerSkill_global_seff.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_seff_pwr");
            MightPowerSkill strength = this.MightData.MightPowerSkill_global_strength.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_strength_pwr");
            _spRegenRate += (fitness.level * .05f);
            _spCost += (coordination.level * -.025f);
            _arcaneRes += ((1 - this.Pawn.GetStatValue(StatDefOf.PsychicSensitivity, false)) / 2);
            _arcaneDmg += ((this.Pawn.GetStatValue(StatDefOf.PsychicSensitivity, false) - 1) / 4);
            this.maxSP = 1 + (.04f * endurance.level) + _maxSP;
            this.spRegenRate = 1f + _spRegenRate;
            this.coolDown = 1f + _coolDown;
            this.xpGain = 1f + _xpGain;
            this.spCost = 1f + _spCost;
            this.arcaneRes = 1 + _arcaneRes;
            this.mightPwr = 1 + _arcaneDmg + (.05f * strength.level);
            if (_maxSP != 0)
            {
                HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_maxMP"), .5f);
            }
            if (_spRegenRate != 0)
            {
                HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_mpRegenRate"), .5f);
            }
            if (_coolDown != 0)
            {
                HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_coolDown"), .5f);
            }
            if (_xpGain != 0)
            {
                HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_xpGain"), .5f);
            }
            if (_spCost != 0)
            {
                HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_mpCost"), .5f);
            }
            if (_arcaneRes != 0)
            {
                HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_arcaneRes"), .5f);
            }
            if (this.mightPwr != 1)
            {
                HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_arcaneDmg"), .5f);
            }
            if (_arcaneSpectre == true)
            {
                HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_arcaneSpectre"), .5f);
            }
            if (_phantomShift == true)
            {
                HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_phantomShift"), .5f);
            }
            if (this.Pawn.story.traits.HasTrait(TorannMagicDefOf.Lich) && !this.Pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_LichHD")))
            {
                HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_LichHD"), .5f);
            }

            using (IEnumerator<Hediff> enumerator = this.Pawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Hediff rec = enumerator.Current;
                    if (rec.def.defName == "TM_HediffEnchantment_maxMP" && this.maxSP == 1)
                    {
                        Pawn.health.RemoveHediff(rec);
                    }
                    if (rec.def.defName == "TM_HediffEnchantment_coolDown" && this.coolDown == 1)
                    {
                        Pawn.health.RemoveHediff(rec);
                    }
                    if (rec.def.defName == "TM_HediffEnchantment_mpCost" && this.spCost == 1)
                    {
                        Pawn.health.RemoveHediff(rec);
                    }
                    if (rec.def.defName == "TM_HediffEnchantment_mpRegenRate" && this.spRegenRate == 1)
                    {
                        Pawn.health.RemoveHediff(rec);
                    }
                    if (rec.def.defName == "TM_HediffEnchantment_xpGain" && this.xpGain == 1)
                    {
                        Pawn.health.RemoveHediff(rec);
                    }
                    if (_arcaneRes != 0)
                    {
                        HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_arcaneRes"), .5f);
                    }
                    if (_arcaneDmg != 0)
                    {
                        HealthUtility.AdjustSeverity(this.Pawn, HediffDef.Named("TM_HediffEnchantment_arcaneDmg"), .5f);
                    }
                    if (rec.def.defName == "TM_HediffEnchantment_arcaneSpectre" && _arcaneSpectre == false)
                    {
                        Pawn.health.RemoveHediff(rec);
                    }
                    if (rec.def.defName == "TM_HediffEnchantment_phantomShift" && _phantomShift == false)
                    {
                        Pawn.health.RemoveHediff(rec);
                    }
                }
            }
        }

        public void ResolveStamina()
        {
            bool flag = this.Stamina == null;
            if (flag)
            {
                Hediff firstHediffOfDef = base.AbilityUser.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_MightUserHD, false);
                bool flag2 = firstHediffOfDef != null;
                if (flag2)
                {
                    firstHediffOfDef.Severity = 1f;
                }
                else
                {
                    Hediff hediff = HediffMaker.MakeHediff(TorannMagicDefOf.TM_MightUserHD, base.AbilityUser, null);
                    hediff.Severity = 1f;
                    base.AbilityUser.health.AddHediff(hediff, null, null);
                }
            }
        }
        public void ResolveMightPowers()
        {
            bool flag = this.mightPowersInitialized;
            if (!flag)
            {
                this.mightPowersInitialized = true;
            }
        }
        public void ResolveMightTab()
        {
            InspectTabBase inspectTabsx = base.AbilityUser.GetInspectTabs().FirstOrDefault((InspectTabBase x) => x.labelKey == "TM_TabMight");
            IEnumerable<InspectTabBase> inspectTabs = base.AbilityUser.GetInspectTabs();
            bool flag = inspectTabs != null && inspectTabs.Count<InspectTabBase>() > 0;
            if (flag)
            {         
                if (inspectTabsx == null)
                {
                    try
                    {
                        base.AbilityUser.def.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(typeof(ITab_Pawn_Might)));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Concat(new object[]
                        {
                            "Could not instantiate inspector tab of type ",
                            typeof(ITab_Pawn_Might),
                            ": ",
                            ex
                        }));
                    }
                }
            }
        }

        public override void PostExposeData()
        {
            //base.PostExposeData();
            Scribe_Values.Look<bool>(ref this.mightPowersInitialized, "mightPowersInitialized", false, false);
            Scribe_Collections.Look<Thing>(ref this.combatItems, "combatItems", LookMode.Reference);
            Scribe_References.Look<Pawn>(ref this.bondedPet, "bondedPet", false);
            Scribe_Values.Look<bool>(ref this.skill_GearRepair, "skill_GearRepair", false, false);
            Scribe_Values.Look<bool>(ref this.skill_InnerHealing, "skill_InnerHealing", false, false);
            Scribe_Values.Look<bool>(ref this.skill_HeavyBlow, "skill_HeavyBlow", false, false);
            Scribe_Values.Look<bool>(ref this.skill_Sprint, "skill_Sprint", false, false);
            Scribe_Values.Look<bool>(ref this.skill_StrongBack, "skill_StrongBack", false, false);
            Scribe_Values.Look<bool>(ref this.skill_ThickSkin, "skill_ThickSkin", false, false);
            Scribe_Values.Look<bool>(ref this.skill_FightersFocus, "skill_FightersFocus", false, false);
            Scribe_Values.Look<bool>(ref this.skill_Teach, "skill_Teach", false, false);
            Scribe_Deep.Look<MightData>(ref this.mightData, "mightData", new object[]
            {
                this
            });
            bool flag11 = Scribe.mode == LoadSaveMode.PostLoadInit;
            if (flag11)
            {
                Pawn abilityUser = base.AbilityUser;
                bool flag40 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Gladiator);
                if (flag40)
                {
                    bool flag14 = !this.MightData.MightPowersG.NullOrEmpty<MightPower>();
                    if (flag14)
                    {
                        foreach (MightPower current3 in this.MightData.MightPowersG)
                        {
                            bool flag15 = current3.abilityDef != null;
                            if (flag15)
                            {
                                if ((current3.abilityDef == TorannMagicDefOf.TM_Sprint || current3.abilityDef == TorannMagicDefOf.TM_Sprint_I || current3.abilityDef == TorannMagicDefOf.TM_Sprint_II || current3.abilityDef == TorannMagicDefOf.TM_Sprint_III))
                                {
                                    if (current3.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Sprint);
                                    }
                                    else if (current3.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Sprint_I);
                                    }
                                    else if (current3.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Sprint_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Sprint_III);
                                    }
                                }
                                if ((current3.abilityDef == TorannMagicDefOf.TM_Grapple || current3.abilityDef == TorannMagicDefOf.TM_Grapple_I || current3.abilityDef == TorannMagicDefOf.TM_Grapple_II || current3.abilityDef == TorannMagicDefOf.TM_Grapple_III))
                                {
                                    if (current3.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Grapple);
                                    }
                                    else if (current3.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Grapple_I);
                                    }
                                    else if (current3.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Grapple_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Grapple_III);
                                    }
                                }
                            }
                        }
                    }
                }
                if (flag40)
                {
                   // Log.Message("Loading Gladiator Abilities");
                    //this.AddPawnAbility(TorannMagicDefOf.TM_Fortitude);
                    //this.AddPawnAbility(TorannMagicDefOf.TM_Cleave);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Whirlwind);
                }
                bool flag41 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper);
                if (flag41)
                {
                    bool flag17 = !this.MightData.MightPowersS.NullOrEmpty<MightPower>();
                    if (flag17)
                    {
                        foreach (MightPower current4 in this.MightData.MightPowersS)
                        {
                            bool flag18 = current4.abilityDef != null;
                            if (flag18)
                            {
                                if ((current4.abilityDef == TorannMagicDefOf.TM_DisablingShot || current4.abilityDef == TorannMagicDefOf.TM_DisablingShot_I || current4.abilityDef == TorannMagicDefOf.TM_DisablingShot_II || current4.abilityDef == TorannMagicDefOf.TM_DisablingShot_III))
                                {
                                    if (current4.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_DisablingShot);
                                    }
                                    else if (current4.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_DisablingShot_I);
                                    }
                                    else if (current4.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_DisablingShot_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_DisablingShot_III);
                                    }
                                }
                            }
                        }
                    }
                }
                if (flag41)
                {
                    //Log.Message("Loading Sniper Abilities");
                    //this.AddPawnAbility(TorannMagicDefOf.TM_SniperFocus);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Headshot);
                    this.AddPawnAbility(TorannMagicDefOf.TM_AntiArmor);
                }
                bool flag42 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Bladedancer);
                if (flag42)
                {
                    bool flag19 = !this.MightData.MightPowersB.NullOrEmpty<MightPower>();
                    if (flag19)
                    {
                        foreach (MightPower current5 in this.MightData.MightPowersB)
                        {
                            bool flag20 = current5.abilityDef != null;
                            if (flag20)
                            {
                                if ((current5.abilityDef == TorannMagicDefOf.TM_PhaseStrike || current5.abilityDef == TorannMagicDefOf.TM_PhaseStrike_I || current5.abilityDef == TorannMagicDefOf.TM_PhaseStrike_II || current5.abilityDef == TorannMagicDefOf.TM_PhaseStrike_III))
                                {
                                    if (current5.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_PhaseStrike);
                                    }
                                    else if (current5.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_PhaseStrike_I);
                                    }
                                    else if (current5.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_PhaseStrike_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_PhaseStrike_III);
                                    }
                                }
                            }
                        }
                    }
                }
                if (flag42)
                {
                    //Log.Message("Loading Bladedancer Abilities");
                    //this.AddPawnAbility(TorannMagicDefOf.TM_BladeFocus);
                    //this.AddPawnAbility(TorannMagicDefOf.TM_BladeArt);
                    this.AddPawnAbility(TorannMagicDefOf.TM_SeismicSlash);
                    this.AddPawnAbility(TorannMagicDefOf.TM_BladeSpin);
                }
                bool flag43 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Ranger);
                if (flag43)
                {
                    bool flag21 = !this.MightData.MightPowersR.NullOrEmpty<MightPower>();
                    if (flag21)
                    {
                        foreach (MightPower current6 in this.MightData.MightPowersR)
                        {
                            bool flag22 = current6.abilityDef != null;
                            if (flag22)
                            {
                                if ((current6.abilityDef == TorannMagicDefOf.TM_ArrowStorm || current6.abilityDef == TorannMagicDefOf.TM_ArrowStorm_I || current6.abilityDef == TorannMagicDefOf.TM_ArrowStorm_II || current6.abilityDef == TorannMagicDefOf.TM_ArrowStorm_III))
                                {
                                    if (current6.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ArrowStorm);
                                    }
                                    else if (current6.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ArrowStorm_I);
                                    }
                                    else if (current6.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ArrowStorm_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_ArrowStorm_III);
                                    }
                                }
                            }
                        }
                    }
                }
                if (flag43)
                {
                    //Log.Message("Loading Ranger Abilities");
                    //this.AddPawnAbility(TorannMagicDefOf.TM_RangerTraining);
                    //this.AddPawnAbility(TorannMagicDefOf.TM_BowTraining);
                    this.AddPawnAbility(TorannMagicDefOf.TM_PoisonTrap);
                    this.AddPawnAbility(TorannMagicDefOf.TM_AnimalFriend);
                }

                bool flag44 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.Faceless);
                if (flag44)
                {
                    bool flag21 = !this.MightData.MightPowersF.NullOrEmpty<MightPower>();
                    if (flag21)
                    {
                        foreach (MightPower current7 in this.MightData.MightPowersF)
                        {
                            bool flag22 = current7.abilityDef != null;
                            if (flag22)
                            {
                                if ((current7.abilityDef == TorannMagicDefOf.TM_Transpose || current7.abilityDef == TorannMagicDefOf.TM_Transpose_I || current7.abilityDef == TorannMagicDefOf.TM_Transpose_II || current7.abilityDef == TorannMagicDefOf.TM_Transpose_III))
                                {
                                    if (current7.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Transpose);
                                    }
                                    else if (current7.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Transpose_I);
                                    }
                                    else if (current7.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Transpose_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_Transpose_III);
                                    }
                                }
                            }
                        }
                    }
                }
                if (flag44)
                {
                    //Log.Message("Loading Faceless Abilities");
                    this.AddPawnAbility(TorannMagicDefOf.TM_Disguise);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Mimic);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Reversal);
                    this.AddPawnAbility(TorannMagicDefOf.TM_Possess);                   
                }

                bool flag45 = abilityUser.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic);
                if (flag45)
                {
                    bool flag21 = !this.MightData.MightPowersP.NullOrEmpty<MightPower>();
                    if (flag21)
                    {
                        foreach (MightPower current8 in this.MightData.MightPowersP)
                        {
                            bool flag22 = current8.abilityDef != null;
                            if (flag22)
                            {
                                if ((current8.abilityDef == TorannMagicDefOf.TM_PsionicBlast || current8.abilityDef == TorannMagicDefOf.TM_PsionicBlast_I || current8.abilityDef == TorannMagicDefOf.TM_PsionicBlast_II || current8.abilityDef == TorannMagicDefOf.TM_PsionicBlast_III))
                                {
                                    if (current8.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_PsionicBlast);
                                    }
                                    else if (current8.level == 1)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_PsionicBlast_I);
                                    }
                                    else if (current8.level == 2)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_PsionicBlast_II);
                                    }
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_PsionicBlast_III);
                                    }
                                }
                                if ((current8.abilityDef == TorannMagicDefOf.TM_PsionicBarrier || current8.abilityDef == TorannMagicDefOf.TM_PsionicBarrier_Projected))
                                {
                                    if (current8.level == 0)
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_PsionicBarrier);
                                    }                                    
                                    else
                                    {
                                        base.AddPawnAbility(TorannMagicDefOf.TM_PsionicBarrier_Projected);
                                    }
                                }
                            }
                        }
                    }
                }
                if (flag45)
                {
                    //Log.Message("Loading Psionic Abilities");
                    //this.AddPawnAbility(TorannMagicDefOf.TM_PsionicBarrier);
                    this.AddPawnAbility(TorannMagicDefOf.TM_PsionicDash);
                    this.AddPawnAbility(TorannMagicDefOf.TM_PsionicStorm);
                }

                this.InitializeSkill();
                //base.UpdateAbilities();
            }
            
        }

    }
}
