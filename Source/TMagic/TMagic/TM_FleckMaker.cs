using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace TorannMagic
{
    public static class TM_FleckMaker
    {
        public static void ThrowGenericFleck(FleckDef fleckDef, Vector3 loc, Map map, float scale, float solidTime, float fadeIn, float fadeOut, int rotationRate, float velocity, float velocityAngle, float lookAngle)
        {
            if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
            {
                return;
            }            

            FleckCreationData dataStatic = default(FleckCreationData);
            dataStatic.def = fleckDef;
            dataStatic.scale = scale;            
            dataStatic.spawnPosition = loc;
            dataStatic.rotationRate = rotationRate;
            dataStatic.velocityAngle = velocityAngle;
            dataStatic.velocitySpeed = velocity;
            dataStatic.solidTimeOverride = solidTime;
            dataStatic.rotation = lookAngle;
            map.flecks.CreateFleck(dataStatic);
        }
    }
}
