using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace TorannMagic.Thoughts
{
    public class InteractionWorker_MagicLore : InteractionWorker
    {

        public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks)
        {
            CompAbilityUserMagic compInit = initiator.GetComp<CompAbilityUserMagic>();
            CompAbilityUserMagic compRec = recipient.GetComp<CompAbilityUserMagic>();
            base.Interacted(initiator, recipient, extraSentencePacks);
            int num = compInit.MagicUserLevel - compRec.MagicUserLevel;
            int num2 = (int)(20f + Rand.Range(3f, 10f)*(float)num);
            compRec.MagicUserXP += num2;
            MoteMaker.ThrowText(recipient.DrawPos, recipient.MapHeld, "XP +" + num2, -1f);
        }

        public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
        {
            CompAbilityUserMagic compInit = initiator.GetComp<CompAbilityUserMagic>();
            CompAbilityUserMagic compRec = recipient.GetComp<CompAbilityUserMagic>();
            bool flag = !initiator.IsColonist || !recipient.IsColonist;
            float result;
            if (flag)
            {
                result = 0f;
            }
            else
            {
                bool flag2 = !compInit.IsMagicUser;
                if (flag2)
                {
                    result = 0f;
                }
                else
                {
                    bool flag3 = !compRec.IsMagicUser;
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
                                int levelInit = compInit.MagicUserLevel;
                                int levelRec = compRec.MagicUserLevel;
                                if (levelInit <= levelRec)
                                {
                                    result = 0f;
                                }
                                else
                                {
                                    bool flag5 = initiator.relations.OpinionOf(recipient) > 0 || recipient.relations.OpinionOf(initiator) > 0;
                                    if (flag5)
                                    {
                                        result = Rand.Range(0.8f, 1f);
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
