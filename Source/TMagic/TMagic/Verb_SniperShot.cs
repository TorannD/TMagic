using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using AbilityUser;


namespace TorannMagic
{
    public class Verb_SniperShot : Verb_UseAbility
    {
        protected override bool TryCastShot()
        {
            if ( this.CasterPawn.equipment.Primary !=null && this.CasterPawn.equipment.Primary.def.IsRangedWeapon)
            {
                Thing wpn = this.CasterPawn.equipment.Primary;
                if (wpn.def.weaponTags.Contains("Neolithic"))
                {
                    if (this.CasterPawn.IsColonistPlayerControlled)
                    {
                        Messages.Message("MustHaveGunpowderWeapon".Translate(new object[]
                    {
                    this.CasterPawn.LabelCap
                    }), MessageTypeDefOf.RejectInput);
                    }
                    return false;
                }
                else
                {
                    base.TryCastShot();
                    return true;
                }

                
            }
            else
            {
                Messages.Message("MustHaveRangedWeapon".Translate(new object[]
                {
                    this.CasterPawn.LabelCap
                }), MessageTypeDefOf.RejectInput);
                return false;
            }
        }
    }
}
