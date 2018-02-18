using System.Collections.Generic;
using Verse;
using RimWorld;
using System.Linq;

namespace TorannMagic
{
    public class Recipe_RegrowBodyPart : Recipe_InstallArtificialBodyPart
    {
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            if (billDoer != null)
            {
                if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill) || this.CheckDruidSurgeryFail(billDoer, pawn, ingredients, part, bill))
                {
                    return;
                }
                TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
                {
                    billDoer,
                    pawn
                });
                TM_MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, billDoer.Position, billDoer.Map);
            }
            else if (pawn.Map != null)
            {
                TM_MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(pawn, part, pawn.Position, pawn.Map);
            }
            else
            {
                pawn.health.RestorePart(part, null, true);
            }
            pawn.health.AddHediff(this.recipe.addsHediff, part, null);
        }

        public bool CheckDruidSurgeryFail(Pawn surgeon, Pawn patient, List<Thing> ingredients, BodyPartRecord part, Bill bill)
        {

            CompAbilityUserMagic comp = surgeon.GetComp<CompAbilityUserMagic>();

            if (this.recipe.defName == "RegrowArm" || this.recipe.defName == "RegrowLeg" || this.recipe.defName == "RegrowFoot" || this.recipe.defName == "RegrowHand")
            {
                string reason;
                if (comp.IsMagicUser)
                {
                    if (comp.spell_RegrowLimb == true)
                    {
                        MagicPowerSkill eff = surgeon.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_RegrowLimb.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_RegrowLimb_eff");
                        if (comp.Mana.CurLevel < (.9f - ((eff.level * .08f) * .9f)))
                        {
                            comp.Mana.CurLevel = comp.Mana.CurLevel / 2;

                            //TM_MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(patient, part, patient.Position, patient.Map);
                            reason = "TM_InsufficientManaForSurgery".Translate();
                            Find.LetterStack.ReceiveLetter("LetterLabelRegrowthSurgeryFail".Translate(), "LetterRegrowthSurgeryFail".Translate(new object[]
                            {
                                surgeon.LabelCap,
                                this.recipe.defName,
                                patient.Label,
                                reason,
                                surgeon.LabelShort
                            }), LetterDefOf.NegativeEvent, null);
                            return true;
                        }
                        else
                        {
                            comp.Mana.CurLevel -= (.9f - ((eff.level * .08f) * .9f));
                            TM_MoteMaker.ThrowRegenMote(patient.Position.ToVector3(), patient.Map, 1.2f);
                            TM_MoteMaker.ThrowRegenMote(patient.Position.ToVector3(), patient.Map, .8f);
                            TM_MoteMaker.ThrowRegenMote(patient.Position.ToVector3(), patient.Map, .8f);
                            return false;
                        }
                    }
                    else
                    {
                        comp.Mana.CurLevel = comp.Mana.CurLevel / 2;
                        //TM_MedicalRecipesUtility.RestorePartAndSpawnAllPreviousParts(patient, part, patient.Position, patient.Map);
                        reason = "TM_NoRegrowthSpell".Translate();
                        Find.LetterStack.ReceiveLetter("LetterLabelRegrowthSurgeryFail".Translate(), "LetterRegrowthSurgeryFail".Translate(new object[]
                        {
                            surgeon.LabelCap,
                            this.recipe.defName,
                            patient.Label,
                            reason,
                            surgeon.LabelShort
                        }), LetterDefOf.NegativeEvent, null);
                        return true;
                    }
                }
                reason = "TM_NotMagicUser".Translate();
                Find.LetterStack.ReceiveLetter("LetterLabelRegrowthSurgeryFail".Translate(), "LetterRegrowthSurgeryFail".Translate(new object[]
                    {
                            surgeon.LabelCap,
                            this.recipe.defName,
                            patient.Label,
                            reason,
                            surgeon.LabelShort
                    }), LetterDefOf.NegativeEvent, null);
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}
