using Verse;
using AbilityUser;
using UnityEngine;
using RimWorld;
using System.Linq;
using System.Collections.Generic;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    public class Verb_BladeSpin : Verb_UseAbility
    {
        MightPowerSkill pwr;
        MightPowerSkill ver;
        MightPowerSkill str;

        float weaponDPS = 0;
        float dmgMultiplier = 1;
        float pawnDPS = 0;
        float skillMultiplier = 1;
        ThingWithComps weaponComp;
        int dmgNum = 0;
        bool flag10;

        private static readonly Color bladeColor = new Color(160f, 160f, 160f);
        private static readonly Material bladeMat = MaterialPool.MatFrom("Spells/cleave", false);

        bool validTarg;
        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            if (targ.Thing != null && targ.Thing == this.caster)
            {
                return this.verbProps.targetParams.canTargetSelf;
            }
            if (targ.IsValid && targ.CenterVector3.InBounds(base.CasterPawn.Map) && !targ.Cell.Fogged(base.CasterPawn.Map) && targ.Cell.Walkable(base.CasterPawn.Map))
            {
                if ((root - targ.Cell).LengthHorizontal < this.verbProps.range)
                {
                    ShootLine shootLine;
                    validTarg = this.TryFindShootLineFromTo(root, targ, out shootLine);
                }
                else
                {
                    validTarg = false;
                }
            }
            else
            {
                validTarg = false;
            }
            return validTarg;
        }

        protected override bool TryCastShot()
        {
            
            if (this.CasterPawn.equipment.Primary != null && !this.CasterPawn.equipment.Primary.def.IsRangedWeapon)
            {          

                CellRect cellRect = CellRect.CenteredOn(base.CasterPawn.Position, 1);
                Map map = base.CasterPawn.Map;
                cellRect.ClipInsideMap(map);

                IntVec3 centerCell = cellRect.CenterCell;
                CompAbilityUserMight comp = this.CasterPawn.GetComp<CompAbilityUserMight>();
                pwr = comp.MightData.MightPowerSkill_BladeSpin.FirstOrDefault((MightPowerSkill x) => x.label == "TM_BladeSpin_pwr");
                ver = comp.MightData.MightPowerSkill_BladeSpin.FirstOrDefault((MightPowerSkill x) => x.label == "TM_BladeSpin_ver");
                str = comp.MightData.MightPowerSkill_global_strength.FirstOrDefault((MightPowerSkill x) => x.label == "TM_global_strength_pwr");

                weaponComp = base.CasterPawn.equipment.Primary;                
                weaponDPS = weaponComp.GetStatValue(StatDefOf.MeleeWeapon_AverageDPS, false) * .7f;
                dmgMultiplier = weaponComp.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier, false);
                pawnDPS = base.CasterPawn.GetStatValue(StatDefOf.MeleeDPS, false);
                skillMultiplier = (.6f + (.025f * str.level));
                dmgNum = Mathf.RoundToInt(skillMultiplier * dmgMultiplier * (pawnDPS + weaponDPS));
                ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
                if (!this.CasterPawn.IsColonistPlayerControlled && settingsRef.AIHardMode)
                {
                    dmgNum += 10;
                }

                SearchForTargets(base.CasterPawn.Position, (2f + (float)(.5f * ver.level)), map);
            }
            else
            {
                Messages.Message("MustHaveMeleeWeapon".Translate(new object[]
                {
                    this.CasterPawn.LabelCap
                }), MessageTypeDefOf.RejectInput);
                return false;
            }

            this.burstShotsLeft = 0;
            this.PostCastShot(flag10, out flag10);
            return flag10;

        }

        public void SearchForTargets(IntVec3 center, float radius, Map map)
        {
            Pawn victim = null;
            IntVec3 curCell;
            DrawBlade(base.CasterPawn.Position.ToVector3Shifted(), 4f + (float)(ver.level));
            IEnumerable<IntVec3> targets = GenRadial.RadialCellsAround(center, radius, true);
            for (int i = 0; i < targets.Count(); i++)
            {
                curCell = targets.ToArray<IntVec3>()[i];
                if (curCell.InBounds(base.CasterPawn.Map) && curCell.IsValid)
                {
                    victim = curCell.GetFirstPawn(map);
                }

                if (victim != null && victim != base.CasterPawn && victim.Faction != base.CasterPawn.Faction)
                {
                    for (int j = 0; j < 2+pwr.level; j++)
                    {
                        bool newTarg = false;
                        if (Rand.Chance(.5f + .04f*(pwr.level+ver.level)))
                        {
                            newTarg = true;
                        }
                        if (newTarg)
                        {
                            DrawStrike(center, victim.Position.ToVector3(), map);
                            damageEntities(victim, null, dmgNum, DamageDefOf.Cut);
                        }
                    }                    
                }
                targets.GetEnumerator().MoveNext();
            }
        }

        public void DrawStrike(IntVec3 center, Vector3 strikePos, Map map)
        {
            TM_MoteMaker.ThrowCrossStrike(strikePos, map, 1f);
            TM_MoteMaker.ThrowBloodSquirt(strikePos, map, 1.5f);
        }

        private void DrawBlade(Vector3 center, float magnitude)
        {

            Vector3 vector = center;
            vector.y = Altitudes.AltitudeFor(AltitudeLayer.MoteOverhead);
            Vector3 s = new Vector3(magnitude, magnitude, (1.5f * magnitude));
            Matrix4x4 matrix = default(Matrix4x4);

            for (int i = 0; i < 6; i++)
            {
                float angle = Rand.Range(0, 360);
                matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, Verb_BladeSpin.bladeMat, 0);
            }
        }

        public void damageEntities(Pawn victim, BodyPartRecord hitPart, int amt, DamageDef type)
        {
            DamageInfo dinfo;
            amt = (int)((float)amt * Rand.Range(.7f, 1.3f));
            dinfo = new DamageInfo(type, amt, (float)-1, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown);            
            dinfo.SetAllowDamagePropagation(false);
            victim.TakeDamage(dinfo);
        }
    }
}
