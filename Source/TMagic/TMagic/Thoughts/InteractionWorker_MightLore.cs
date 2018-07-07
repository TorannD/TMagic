using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace TorannMagic.Thoughts
{
    public class InteractionWorker_MightLore : InteractionWorker
    {

        public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef)
        {
            letterText = null;
            letterLabel = null;
            letterDef = null;
            CompAbilityUserMight compInit = initiator.GetComp<CompAbilityUserMight>();
            CompAbilityUserMight compRec = recipient.GetComp<CompAbilityUserMight>();
            //base.Interacted(initiator, recipient, extraSentencePacks);
            int num = compInit.MightUserLevel - compRec.MightUserLevel;
            int num2 = (int)(20f + Rand.Range(3f, 10f)*(float)num);
            compRec.MightUserXP += num2;
            MoteMaker.ThrowText(recipient.DrawPos, recipient.MapHeld, "XP +" + num2, -1f);
        }

        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            CompAbilityUserMight compInit = initiator.GetComp<CompAbilityUserMight>();
            CompAbilityUserMight compRec = recipient.GetComp<CompAbilityUserMight>();
            bool flag = !initiator.IsColonist || !recipient.IsColonist;
            float result;
            if (flag)
            {
                result = 0f;
            }
            else
            {
                bool flag2 = !compInit.IsMightUser;
                if (flag2)
                {
                    result = 0f;
                }
                else
                {
                    bool flag3 = !compRec.IsMightUser;
                    if (flag3)
                    {
                        result = 0f;
                    }
                    else
                    {
                        if (initiator.jobs.curDriver.asleep)
                        {
                            result = 0f;
                        }
                        else
                        {
                            if (recipient.jobs.curDriver.asleep)
                            {
                                result = 0f;
                            }
                            else
                            {
                                int levelInit = compInit.MightUserLevel;
                                int levelRec = compRec.MightUserLevel;
                                if (levelInit <= levelRec)
                                {
                                    result = 0f;
                                }
                                else
                                {
                                    bool flag5 = initiator.relations.OpinionOf(recipient) > 0;
                                    if (flag5)
                                    {
                                        result = Rand.Range(0.6f, 0.8f);
                                    }
                                    else
                                    {
                                        result = 0f;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
