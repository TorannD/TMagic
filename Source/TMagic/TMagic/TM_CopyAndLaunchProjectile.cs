using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;
using UnityEngine;

namespace TorannMagic
{
    public class TM_CopyAndLaunchProjectile : Projectile
    {
        public static void CopyAndLaunchThing(ThingDef projectileToCopy, Thing launcher, LocalTargetInfo target, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, Thing equipment = null)
        {            
            Projectile newProjectile = (Projectile)GenSpawn.Spawn(projectileToCopy, launcher.Position, launcher.Map, WipeMode.Vanish);
            newProjectile.Launch(launcher, target, intendedTarget, hitFlags, equipment);            
        }

        public static void CopyAndLaunchProjectile(Projectile projectileToCopy, Thing launcher, LocalTargetInfo target, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, Thing equipment = null)
        {            
            Projectile newProjectile = (Projectile)GenSpawn.Spawn(projectileToCopy, launcher.Position, launcher.Map, WipeMode.Vanish);
            newProjectile.Launch(launcher, target, intendedTarget, hitFlags, equipment);            
        }
    }
}
