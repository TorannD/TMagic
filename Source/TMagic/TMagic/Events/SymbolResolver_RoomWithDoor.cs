using RimWorld.BaseGen;
using System;
using Verse;

namespace TorannMagic
{
    public class SymbolResolver_RoomWithDoor : SymbolResolver
    {
        public override bool CanResolve(ResolveParams rp)
        {
            return base.CanResolve(rp);
        }

        public override void Resolve(ResolveParams rp)
        {
            char[] array;
            if (rp.TryGetCustom<char[]>("hasDoor", out array))
            {
                if (rp.rect.Width < 3 && rp.rect.Height < 3)
                {
                    return;
                }
                ResolveParams resolveParams = rp;
                char[] array2 = array;
                int i = 0;
                while (i < array2.Length)
                {
                    char c = array2[i];
                    if (c == 'N')
                    {
                        resolveParams.thingRot = new Rot4?(Rot4.North);
                        resolveParams.rect = new CellRect(rp.rect.minX + 1, rp.rect.maxZ, rp.rect.Width - 2, 1);
                        goto IL_1AA;
                    }
                    if (c == 'S')
                    {
                        resolveParams.thingRot = new Rot4?(Rot4.South);
                        resolveParams.rect = new CellRect(rp.rect.minX + 1, rp.rect.minZ, rp.rect.Width - 2, 1);
                        goto IL_1AA;
                    }
                    if (c == 'E')
                    {
                        resolveParams.thingRot = new Rot4?(Rot4.East);
                        resolveParams.rect = new CellRect(rp.rect.maxX, rp.rect.minZ + 1, 1, rp.rect.Height - 2);
                        goto IL_1AA;
                    }
                    if (c == 'W')
                    {
                        resolveParams.thingRot = new Rot4?(Rot4.West);
                        resolveParams.rect = new CellRect(rp.rect.minX, rp.rect.minZ + 1, 1, rp.rect.Height - 2);
                        goto IL_1AA;
                    }
                    IL_1BB:
                    i++;
                    continue;
                    IL_1AA:
                    BaseGen.symbolStack.Push("wallDoor", resolveParams);
                    goto IL_1BB;
                }
            }
            //float num;
            //rp.TryGetCustom<float>("madAnimalChance", ref num);
            //float num2;
            //rp.TryGetCustom<float>("luciferiumGasChance", ref num2);
            //float num3;
            //rp.TryGetCustom<float>("psionicLandmineChance", ref num3);
            //float num4;
            //rp.TryGetCustom<float>("smallGoldChance", ref num4);
            //float num5;
            //rp.TryGetCustom<float>("smallSilverChance", ref num5);
            //RectActionTrigger rectActionTrigger = ThingMaker.MakeThing(ThingDefOfReconAndDiscovery.RectActionTrigger, null) as RectActionTrigger;
            //rectActionTrigger.Rect = rp.rect;
            //if (Rand.Chance(num))
            //{
            //    rectActionTrigger.actionDef = ActionDefOfReconAndDiscovery.MadAnimal;
            //}
            //else if (Rand.Chance(num2))
            //{
            //    rectActionTrigger.actionDef = ActionDefOfReconAndDiscovery.LuciferiumGas;
            //}
            //else if (Rand.Chance(num3))
            //{
            //    rectActionTrigger.actionDef = ActionDefOfReconAndDiscovery.PsionicLandmine;
            //}
            //else if (Rand.Chance(num4))
            //{
            //    rectActionTrigger.actionDef = ActionDefOfReconAndDiscovery.SmallGold;
            //}
            //else if (Rand.Chance(num5))
            //{
            //    rectActionTrigger.actionDef = ActionDefOfReconAndDiscovery.SmallSilver;
            //}
            //else
            //{
            //    rectActionTrigger.actionDef = ActionDefOfReconAndDiscovery.BaseActivatedAction;
            //}
            //if (rectActionTrigger.actionDef != null)
            //{
            //    ResolveParams resolveParams2 = rp;
            //    resolveParams2.SetCustom<RectActionTrigger>("trigger", rectActionTrigger, false);
            //    BaseGen.symbolStack.Push("placeTrigger", resolveParams2);
            //}
            BaseGen.symbolStack.Push("emptyRoom", rp);
        }
    }
}
