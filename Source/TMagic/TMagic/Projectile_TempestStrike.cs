using System.Linq;
using Verse;
using AbilityUser;
using UnityEngine;
using RimWorld;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    class Projectile_TempestStrike : Projectile_AbilityBase
    {
        private int rotationOffset = 0;
        public bool shouldSpin = true;
        private bool spinCheck = true;

        protected override void Impact(Thing hitThing)
        {
            Map map = base.Map;
            Pawn pawn = this.launcher as Pawn;
            base.Impact(hitThing);
            ThingDef def = this.def;
            try
            {
                if (pawn != null)
                {
                    Pawn victim = hitThing as Pawn;
                    CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();

                    if (victim != null && comp != null)
                    {
                        TM_Action.DamageEntities(victim, null, GetWeaponDmg(pawn), this.def.projectile.damageDef, pawn);
                        TM_MoteMaker.ThrowBloodSquirt(victim.DrawPos, victim.Map, .8f);
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                //
            }
        }

        public override void Draw()
        {
            if (spinCheck)
            {
                if (this.launcher is Pawn)
                {
                    Pawn pawn = this.launcher as Pawn;
                    if (pawn.equipment != null && pawn.equipment.Primary != null)
                    {
                        ThingWithComps weaponComp = pawn.equipment.Primary;
                        if(weaponComp.def.IsRangedWeapon)
                        {
                            shouldSpin = false;
                        }
                    }
                    spinCheck = false;
                }
            }
            if (shouldSpin)
            {
                this.rotationOffset += 49;
            }
            if (this.rotationOffset > 360)
            {
                this.rotationOffset = this.rotationOffset - 360;
            }
            Mesh mesh = MeshPool.GridPlane(this.def.graphicData.drawSize);
            Graphics.DrawMesh(mesh, DrawPos, (Quaternion.AngleAxis(rotationOffset, Vector3.up) * ExactRotation), def.DrawMatSingle, 0);
            
            Comps_PostDraw();
        }

        public static int GetWeaponDmg(Pawn pawn)
        {
            if (pawn != null)
            {
                CompAbilityUserMight comp = pawn.GetComp<CompAbilityUserMight>();
                if (comp != null)
                {
                    MightPowerSkill str = comp.MightData.MightPowerSkill_global_strength.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_strength_pwr");
                    int dmgNum = 0;                    
                    if (pawn.equipment != null && pawn.equipment.Primary != null)
                    {
                        ThingWithComps weaponComp = pawn.equipment.Primary;
                        if (weaponComp.def.IsMeleeWeapon)
                        {
                            float weaponDPS = weaponComp.GetStatValue(StatDefOf.MeleeWeapon_AverageDPS, false);
                            float dmgMultiplier = weaponComp.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier, false);
                            float pawnDPS = pawn.GetStatValue(StatDefOf.MeleeDPS, false);
                            float skillMultiplier = (.6f) * comp.mightPwr;
                            dmgNum = Mathf.RoundToInt(skillMultiplier * dmgMultiplier * (pawnDPS + weaponDPS));
                        }
                        if (weaponComp.def.IsRangedWeapon)
                        {                            
                            int value = Mathf.RoundToInt(weaponComp.GetStatValue(StatDefOf.MarketValue));                           

                            int dmg;
                            if (value > 1000)
                            {
                                value -= 1000;
                                dmg = (int)((20 + (value / 120)));
                            }
                            else
                            {
                                dmg = (10 + (int)((value / 50)));
                            }
                            float weaponDPS = dmg;
                            float skillMultiplier = (.6f) * comp.mightPwr;
                            dmgNum = Mathf.RoundToInt(skillMultiplier * weaponDPS);
                        }
                    }
                    else
                    {
                        float pawnDPS = pawn.GetStatValue(StatDefOf.MeleeDPS, false);
                        float skillMultiplier = (.6f) * comp.mightPwr;
                        dmgNum = Mathf.RoundToInt(skillMultiplier * (pawnDPS));
                    }
                    if (comp != null && pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Wayfarer))
                    {
                        if (comp.MightData.MightPowerSkill_FieldTraining.FirstOrDefault((MightPowerSkill x) => x.label == "TM_FieldTraining_pwr").level >= 8)
                        {
                            dmgNum = Mathf.RoundToInt(dmgNum * 1.2f);
                        }
                    }
                    return dmgNum;
                }
                return 0;
            }
            return 0;
        }

    }    
}


