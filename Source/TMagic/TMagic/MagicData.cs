using AbilityUser;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TorannMagic
{
    public class MagicData : IExposable
    {

        private Pawn magicPawn;
        private int magicUserLevel = 0;
        private int magicAbilityPoints = 0;
        private int magicUserXP = 1;
        private int ticksToLearnMagicXP = -1;
        public bool initialized = false;
        private Faction affiliation = null;
        private int ticksAffiliation = 0;
        private bool isNecromancer = false;
        private int dominationCount = 0;

        public List<MagicPower> magicPowerIF;
        public List<MagicPower> magicPowerHoF;
        public List<MagicPower> magicPowerSB;
        public List<MagicPower> magicPowerA;
        public List<MagicPower> magicPowerP;
        public List<MagicPower> magicPowerS;
        public List<MagicPower> magicPowerD;
        public List<MagicPower> magicPowerN;
        public List<MagicPower> magicPowerPR;

        public List<MagicPowerSkill> magicPowerSkill_global_regen;
        public List<MagicPowerSkill> magicPowerSkill_global_eff;
        public List<MagicPowerSkill> magicPowerSkill_global_spirit;

        public List<MagicPowerSkill> magicPowerSkill_RayofHope;
        public List<MagicPowerSkill> magicPowerSkill_Firebolt;
        public List<MagicPowerSkill> magicPowerSkill_Fireball;
        public List<MagicPowerSkill> magicPowerSkill_Fireclaw;
        public List<MagicPowerSkill> magicPowerSkill_Firestorm;

        public List<MagicPowerSkill> magicPowerSkill_Soothe;
        public List<MagicPowerSkill> magicPowerSkill_Rainmaker;
        public List<MagicPowerSkill> magicPowerSkill_Icebolt;
        public List<MagicPowerSkill> magicPowerSkill_FrostRay;
        public List<MagicPowerSkill> magicPowerSkill_Snowball;
        public List<MagicPowerSkill> magicPowerSkill_Blizzard;

        public List<MagicPowerSkill> magicPowerSkill_AMP;
        public List<MagicPowerSkill> magicPowerSkill_LightningBolt;
        public List<MagicPowerSkill> magicPowerSkill_LightningCloud;
        public List<MagicPowerSkill> magicPowerSkill_LightningStorm;
        public List<MagicPowerSkill> magicPowerSkill_EyeOfTheStorm;

        public List<MagicPowerSkill> magicPowerSkill_Shadow;
        public List<MagicPowerSkill> magicPowerSkill_Blink;
        public List<MagicPowerSkill> magicPowerSkill_Summon;
        public List<MagicPowerSkill> magicPowerSkill_MagicMissile;
        public List<MagicPowerSkill> magicPowerSkill_Teleport;
        public List<MagicPowerSkill> magicPowerSkill_FoldReality;

        public List<MagicPowerSkill> magicPowerSkill_Heal;
        public List<MagicPowerSkill> magicPowerSkill_Shield;
        public List<MagicPowerSkill> magicPowerSkill_ValiantCharge;
        public List<MagicPowerSkill> magicPowerSkill_Overwhelm;
        
        public List<MagicPowerSkill> magicPowerSkill_SummonMinion;        
        public List<MagicPowerSkill> magicPowerSkill_SummonPylon;
        public List<MagicPowerSkill> magicPowerSkill_SummonExplosive;
        public List<MagicPowerSkill> magicPowerSkill_SummonElemental;

        public List<MagicPowerSkill> magicPowerSkill_Poison;
        public List<MagicPowerSkill> magicPowerSkill_SootheAnimal;
        public List<MagicPowerSkill> magicPowerSkill_Regenerate;
        public List<MagicPowerSkill> magicPowerSkill_CureDisease;
        public List<MagicPowerSkill> magicPowerSkill_RegrowLimb;

        public List<MagicPowerSkill> magicPowerSkill_RaiseUndead;
        public List<MagicPowerSkill> magicPowerSkill_DeathMark;
        public List<MagicPowerSkill> magicPowerSkill_FogOfTorment;
        public List<MagicPowerSkill> magicPowerSkill_ConsumeCorpse;
        public List<MagicPowerSkill> magicPowerSkill_CorpseExplosion;

        public List<MagicPowerSkill> magicPowerSkill_AdvancedHeal;
        public List<MagicPowerSkill> magicPowerSkill_Purify;
        public List<MagicPowerSkill> magicPowerSkill_HealingCircle;
        public List<MagicPowerSkill> magicPowerSkill_BestowMight;
        public List<MagicPowerSkill> magicPowerSkill_Resurrection;

        public List<MagicPowerSkill> MagicPowerSkill_global_regen
        {
            get
            {
                bool flag = this.magicPowerSkill_global_regen == null;
                if (flag)
                {
                    this.magicPowerSkill_global_regen = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_global_regen_pwr", "TM_global_regen_pwr_desc")
                    };
                }
                return this.magicPowerSkill_global_regen;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_global_eff
        {
            get
            {
                bool flag = this.magicPowerSkill_global_eff == null;
                if (flag)
                {
                    this.magicPowerSkill_global_eff = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_global_eff_pwr", "TM_global_eff_pwr_desc")
                    };
                }
                return this.magicPowerSkill_global_eff;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_global_spirit
        {
            get
            {
                bool flag = this.magicPowerSkill_global_spirit == null;
                if (flag)
                {
                    this.magicPowerSkill_global_spirit = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_global_spirit_pwr", "TM_global_spirit_pwr_desc")
                    };
                }
                return this.magicPowerSkill_global_spirit;
            }
        }

        public List<MagicPower> MagicPowersIF
        {
            get
            {
                bool flag = this.magicPowerIF == null;
                if (flag)
                {
                    this.magicPowerIF = new List<MagicPower>
                    {
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_RayofHope,
                            TorannMagicDefOf.TM_RayofHope_I,
                            TorannMagicDefOf.TM_RayofHope_II,
                            TorannMagicDefOf.TM_RayofHope_III
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Firebolt
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Fireclaw
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Fireball
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Firestorm
                        }),
                    };
                }
                return this.magicPowerIF;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_RayofHope
        {
            get
            {
                bool flag = this.magicPowerSkill_RayofHope == null;
                if (flag)
                {
                    this.magicPowerSkill_RayofHope = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_RayofHope_eff", "TM_RayofHope_eff_desc")
                    };
                }
                return this.magicPowerSkill_RayofHope;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Firebolt
        {
            get
            {
                bool flag = this.magicPowerSkill_Firebolt == null;
                if (flag)
                {
                    this.magicPowerSkill_Firebolt = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Firebolt_pwr", "TM_Firebolt_pwr_desc"),
                        new MagicPowerSkill("TM_Firebolt_eff", "TM_Firebolt_eff_desc")
                    };
                }
                return this.magicPowerSkill_Firebolt;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Fireball
        {
            get
            {
                bool flag = this.magicPowerSkill_Fireball == null;
                if (flag)
                {
                    this.magicPowerSkill_Fireball = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Fireball_pwr", "TM_Fireball_pwr_desc"),
                        new MagicPowerSkill("TM_Fireball_eff", "TM_Fireball_eff_desc"),
                        new MagicPowerSkill("TM_Fireball_ver", "TM_Fireball_ver_desc")
                    };
                }
                return this.magicPowerSkill_Fireball;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Fireclaw
        {
            get
            {
                bool flag = this.magicPowerSkill_Fireclaw == null;
                if (flag)
                {
                    this.magicPowerSkill_Fireclaw = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Fireclaw_pwr", "TM_Fireclaw_pwr_desc"),
                        new MagicPowerSkill("TM_Fireclaw_eff", "TM_Fireclaw_eff_desc"),
                        new MagicPowerSkill("TM_Fireclaw_ver", "TM_Fireclaw_ver_desc")
                    };
                }
                return this.magicPowerSkill_Fireclaw;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Firestorm
        {
            get
            {
                bool flag = this.magicPowerSkill_Firestorm == null;
                if (flag)
                {
                    this.magicPowerSkill_Firestorm = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Firestorm_pwr", "TM_Firestorm_pwr_desc"),
                        new MagicPowerSkill("TM_Firestorm_eff", "TM_Firestorm_eff_desc"),
                        new MagicPowerSkill("TM_Firestorm_ver", "TM_Firestorm_ver_desc")
                    };
                }
                return this.magicPowerSkill_Firestorm;
            }
        }

        public List<MagicPower> MagicPowersHoF
        {
            get
            {
                bool flag = this.magicPowerHoF == null;
                if (flag)
                {
                    this.magicPowerHoF = new List<MagicPower>
                    {
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Soothe,
                            TorannMagicDefOf.TM_Soothe_I,
                            TorannMagicDefOf.TM_Soothe_II,
                            TorannMagicDefOf.TM_Soothe_III
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Rainmaker
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Icebolt
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_FrostRay,
                            TorannMagicDefOf.TM_FrostRay_I,
                            TorannMagicDefOf.TM_FrostRay_II,
                            TorannMagicDefOf.TM_FrostRay_III
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Snowball
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Blizzard
                        }),
                    };
                }
                return this.magicPowerHoF;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Soothe
        {
            get
            {
                bool flag = this.magicPowerSkill_Soothe == null;
                if (flag)
                {
                    this.magicPowerSkill_Soothe = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Soothe_eff", "TM_Soothe_eff_desc")
                    };
                }
                return this.magicPowerSkill_Soothe;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Icebolt
        {
            get
            {
                bool flag = this.magicPowerSkill_Icebolt == null;
                if (flag)
                {
                    this.magicPowerSkill_Icebolt = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Icebolt_pwr", "TM_Icebolt_pwr_desc"),
                        new MagicPowerSkill("TM_Icebolt_eff", "TM_Icebolt_eff_desc"),
                        new MagicPowerSkill("TM_Icebolt_ver", "TM_Icebolt_ver_desc")
                    };
                }
                return this.magicPowerSkill_Icebolt;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_FrostRay
        {
            get
            {
                bool flag = this.magicPowerSkill_FrostRay == null;
                if (flag)
                {
                    this.magicPowerSkill_FrostRay = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_FrostRay_eff", "TM_FrostRay_eff_desc" )
                    };
                }
                return this.magicPowerSkill_FrostRay;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Snowball
        {
            get
            {
                bool flag = this.magicPowerSkill_Snowball == null;
                if (flag)
                {
                    this.magicPowerSkill_Snowball = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Snowball_pwr", "TM_Snowball_pwr_desc" ),
                        new MagicPowerSkill("TM_Snowball_eff", "TM_Snowball_eff_desc" ),
                        new MagicPowerSkill("TM_Snowball_ver", "TM_Snowball_ver_desc" )
                    };
                }
                return this.magicPowerSkill_Snowball;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Rainmaker
        {
            get
            {
                bool flag = this.magicPowerSkill_Rainmaker == null;
                if (flag)
                {
                    this.magicPowerSkill_Rainmaker = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Rainmaker_eff", "TM_Rainmaker_eff_desc" )
                    };
                }
                return this.magicPowerSkill_Rainmaker;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Blizzard
        {
            get
            {
                bool flag = this.magicPowerSkill_Blizzard == null;
                if (flag)
                {
                    this.magicPowerSkill_Blizzard = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Blizzard_pwr", "TM_Blizzard_pwr_desc" ),
                        new MagicPowerSkill("TM_Blizzard_eff", "TM_Blizzard_eff_desc" ),
                        new MagicPowerSkill("TM_Blizzard_ver", "TM_Blizzard_ver_desc" )
                    };
                }
                return this.magicPowerSkill_Blizzard;
            }
        }

        public List<MagicPower> MagicPowersSB
        {
            get
            {
                bool flag = this.magicPowerSB == null;
                if (flag)
                {
                    this.magicPowerSB = new List<MagicPower>
                    {
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_AMP,
                            TorannMagicDefOf.TM_AMP_I,
                            TorannMagicDefOf.TM_AMP_II,
                            TorannMagicDefOf.TM_AMP_III
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_LightningBolt
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_LightningCloud
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_LightningStorm
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_EyeOfTheStorm
                        }),
                    };
                }
                return this.magicPowerSB;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_AMP
        {
            get
            {
                bool flag = this.magicPowerSkill_AMP == null;
                if (flag)
                {
                    this.magicPowerSkill_AMP = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_AMP_eff", "TM_AMP_eff_desc" )
                    };
                }
                return this.magicPowerSkill_AMP;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_LightningBolt
        {
            get
            {
                bool flag = this.magicPowerSkill_LightningBolt == null;
                if (flag)
                {
                    this.magicPowerSkill_LightningBolt = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_LightningBolt_pwr", "TM_LightningBolt_pwr_desc" ),
                        new MagicPowerSkill("TM_LightningBolt_eff", "TM_LightningBolt_eff_desc" ),
                        new MagicPowerSkill("TM_LightningBolt_ver", "TM_LightningBolt_ver_desc" )
                    };
                }
                return this.magicPowerSkill_LightningBolt;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_LightningCloud
        {
            get
            {
                bool flag = this.magicPowerSkill_LightningCloud == null;
                if (flag)
                {
                    this.magicPowerSkill_LightningCloud = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_LightningCloud_pwr", "TM_LightningCloud_pwr_desc" ),
                        new MagicPowerSkill("TM_LightningCloud_eff", "TM_LightningCloud_eff_desc" ),
                        new MagicPowerSkill("TM_LightningCloud_ver", "TM_LightningCloud_ver_desc" )
                    };
                }
                return this.magicPowerSkill_LightningCloud;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_LightningStorm
        {
            get
            {
                bool flag = this.magicPowerSkill_LightningStorm == null;
                if (flag)
                {
                    this.magicPowerSkill_LightningStorm = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_LightningStorm_pwr", "TM_LightningStorm_pwr_desc" ),
                        new MagicPowerSkill("TM_LightningStorm_eff", "TM_LightningStorm_eff_desc" ),
                        new MagicPowerSkill("TM_LightningStorm_ver", "TM_LightningStorm_ver_desc" )
                    };
                }
                return this.magicPowerSkill_LightningStorm;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_EyeOfTheStorm
        {
            get
            {
                bool flag = this.magicPowerSkill_EyeOfTheStorm == null;
                if (flag)
                {
                    this.magicPowerSkill_EyeOfTheStorm = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_EyeOfTheStorm_pwr", "TM_EyeOfTheStorm_pwr_desc" ),
                        new MagicPowerSkill("TM_EyeOfTheStorm_eff", "TM_EyeOfTheStorm_eff_desc" ),
                        new MagicPowerSkill("TM_EyeOfTheStorm_ver", "TM_EyeOfTheStorm_ver_desc" )
                    };
                }
                return this.magicPowerSkill_EyeOfTheStorm;
            }
        }

        public List<MagicPower> MagicPowersA
        {
            get
            {
                bool flag = this.magicPowerA == null;
                if (flag)
                {
                    this.magicPowerA = new List<MagicPower>
                    {
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Shadow,
                            TorannMagicDefOf.TM_Shadow_I,
                            TorannMagicDefOf.TM_Shadow_II,
                            TorannMagicDefOf.TM_Shadow_III
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_MagicMissile,
                            TorannMagicDefOf.TM_MagicMissile_I,
                            TorannMagicDefOf.TM_MagicMissile_II,
                            TorannMagicDefOf.TM_MagicMissile_III

                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Blink,
                            TorannMagicDefOf.TM_Blink_I,
                            TorannMagicDefOf.TM_Blink_II,
                            TorannMagicDefOf.TM_Blink_III
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Summon,
                            TorannMagicDefOf.TM_Summon_I,
                            TorannMagicDefOf.TM_Summon_II,
                            TorannMagicDefOf.TM_Summon_III
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Teleport
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_FoldReality
                        }),
                    };
                }
                return this.magicPowerA;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Shadow
        {
            get
            {
                bool flag = this.magicPowerSkill_Shadow == null;
                if (flag)
                {
                    this.magicPowerSkill_Shadow = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Shadow_eff", "TM_Shadow_eff_desc" )
                    };
                }
                return this.magicPowerSkill_Shadow;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_MagicMissile
        {
            get
            {
                bool flag = this.magicPowerSkill_MagicMissile == null;
                if (flag)
                {
                    this.magicPowerSkill_MagicMissile = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_MagicMissile_eff", "TM_MagicMissile_eff_desc" )
                    };
                }
                return this.magicPowerSkill_MagicMissile;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Blink
        {
            get
            {
                bool flag = this.magicPowerSkill_Blink == null;
                if (flag)
                {
                    this.magicPowerSkill_Blink = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Blink_eff", "TM_Blink_eff_desc" )
                    };
                }
                return this.magicPowerSkill_Blink;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Summon
        {
            get
            {
                bool flag = this.magicPowerSkill_Summon == null;
                if (flag)
                {
                    this.magicPowerSkill_Summon = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Summon_eff", "TM_Summon_eff_desc" )
                    };
                }
                return this.magicPowerSkill_Summon;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Teleport
        {
            get
            {
                bool flag = this.magicPowerSkill_Teleport == null;
                if (flag)
                {
                    this.magicPowerSkill_Teleport = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Teleport_pwr", "TM_Teleport_pwr_desc" ),
                        new MagicPowerSkill("TM_Teleport_eff", "TM_Teleport_eff_desc" ),
                        new MagicPowerSkill("TM_Teleport_ver", "TM_Teleport_ver_desc" )
                    };
                }
                return this.magicPowerSkill_Teleport;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_FoldReality
        {
            get
            {
                bool flag = this.magicPowerSkill_FoldReality == null;
                if (flag)
                {
                    this.magicPowerSkill_FoldReality = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_FoldReality_eff", "TM_FoldReality_eff_desc" )
                    };
                }
                return this.magicPowerSkill_FoldReality;
            }
        }

        public List<MagicPower> MagicPowersP
        {
            get
            {
                bool flag = this.magicPowerP == null;
                if (flag)
                {
                    this.magicPowerP = new List<MagicPower>
                    {
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Heal
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Shield,
                            TorannMagicDefOf.TM_Shield_I,
                            TorannMagicDefOf.TM_Shield_II,
                            TorannMagicDefOf.TM_Shield_III
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_ValiantCharge
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Overwhelm
                        }),

                    };
                }
                return this.magicPowerP;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Heal
        {
            get
            {
                bool flag = this.magicPowerSkill_Heal == null;
                if (flag)
                {
                    this.magicPowerSkill_Heal = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Heal_pwr", "TM_Heal_pwr_desc" ),
                        new MagicPowerSkill("TM_Heal_eff", "TM_Heal_eff_desc" ),
                        new MagicPowerSkill("TM_Heal_ver", "TM_Heal_ver_desc" )
                    };
                }
                return this.magicPowerSkill_Heal;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Shield
        {
            get
            {
                bool flag = this.magicPowerSkill_Shield == null;
                if (flag)
                {
                    this.magicPowerSkill_Shield = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Shield_eff", "TM_Shield_eff_desc" )
                    };
                }
                return this.magicPowerSkill_Shield;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_ValiantCharge
        {
            get
            {
                bool flag = this.magicPowerSkill_ValiantCharge == null;
                if (flag)
                {
                    this.magicPowerSkill_ValiantCharge = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_ValiantCharge_pwr", "TM_ValiantCharge_pwr_desc" ),
                        new MagicPowerSkill("TM_ValiantCharge_eff", "TM_ValiantCharge_eff_desc" ),
                        new MagicPowerSkill("TM_ValiantCharge_ver", "TM_ValiantCharge_ver_desc" )
                    };
                }
                return this.magicPowerSkill_ValiantCharge;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Overwhelm
        {
            get
            {
                bool flag = this.magicPowerSkill_Overwhelm == null;
                if (flag)
                {
                    this.magicPowerSkill_Overwhelm = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Overwhelm_pwr", "TM_Overwhelm_pwr_desc" ),
                        new MagicPowerSkill("TM_Overwhelm_eff", "TM_Overwhelm_eff_desc" ),
                        new MagicPowerSkill("TM_Overwhelm_ver", "TM_Overwhelm_ver_desc" )
                    };
                }
                return this.magicPowerSkill_Overwhelm;
            }
        }

        public List<MagicPower> MagicPowersS
        {
            get
            {
                bool flag = this.magicPowerS == null;
                if (flag)
                {
                    this.magicPowerS = new List<MagicPower>
                    {
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_SummonMinion
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_SummonPylon
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_SummonExplosive
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_SummonElemental
                        }),


                    };
                }
                return this.magicPowerS;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_SummonMinion
        {
            get
            {
                bool flag = this.magicPowerSkill_SummonMinion == null;
                if (flag)
                {
                    this.magicPowerSkill_SummonMinion = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_SummonMinion_pwr", "TM_SummonMinion_pwr_desc" ),
                        new MagicPowerSkill("TM_SummonMinion_eff", "TM_SummonMinion_eff_desc" ),
                        new MagicPowerSkill("TM_SummonMinion_ver", "TM_SummonMinion_ver_desc" )
                    };
                }
                return this.magicPowerSkill_SummonMinion;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_SummonPylon
        {
            get
            {
                bool flag = this.magicPowerSkill_SummonPylon == null;
                if (flag)
                {
                    this.magicPowerSkill_SummonPylon = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_SummonPylon_pwr", "TM_SummonPylon_pwr_desc" ),
                        new MagicPowerSkill("TM_SummonPylon_eff", "TM_SummonPylon_eff_desc" ),
                        new MagicPowerSkill("TM_SummonPylon_ver", "TM_SummonPylon_ver_desc" )
                    };
                }
                return this.magicPowerSkill_SummonPylon;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_SummonExplosive
        {
            get
            {
                bool flag = this.magicPowerSkill_SummonExplosive == null;
                if (flag)
                {
                    this.magicPowerSkill_SummonExplosive = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_SummonExplosive_pwr", "TM_SummonExplosive_pwr_desc" ),
                        new MagicPowerSkill("TM_SummonExplosive_eff", "TM_SummonExplosive_eff_desc" ),
                        new MagicPowerSkill("TM_SummonExplosive_ver", "TM_SummonExplosive_ver_desc" )
                    };
                }
                return this.magicPowerSkill_SummonExplosive;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_SummonElemental
        {
            get
            {
                bool flag = this.magicPowerSkill_SummonElemental == null;
                if (flag)
                {
                    this.magicPowerSkill_SummonElemental = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_SummonElemental_pwr", "TM_SummonElemental_pwr_desc" ),
                        new MagicPowerSkill("TM_SummonElemental_eff", "TM_SummonElemental_eff_desc" ),
                        new MagicPowerSkill("TM_SummonElemental_ver", "TM_SummonElemental_ver_desc" )
                    };
                }
                return this.magicPowerSkill_SummonElemental;
            }
        }

        public List<MagicPower> MagicPowersD
        {
            get
            {
                bool flag = this.magicPowerD == null;
                if (flag)
                {
                    this.magicPowerD = new List<MagicPower>
                    {
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Poison
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_SootheAnimal,
                            TorannMagicDefOf.TM_SootheAnimal_I,
                            TorannMagicDefOf.TM_SootheAnimal_II,
                            TorannMagicDefOf.TM_SootheAnimal_III
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Regenerate
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_CureDisease
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_RegrowLimb
                        }),
                    };
                }
                return this.magicPowerD;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Poison
        {
            get
            {
                bool flag = this.magicPowerSkill_Poison == null;
                if (flag)
                {
                    this.magicPowerSkill_Poison = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Poison_pwr", "TM_Poison_pwr_desc" ),
                        new MagicPowerSkill("TM_Poison_eff", "TM_Poison_eff_desc" ),
                        new MagicPowerSkill("TM_Poison_ver", "TM_Poison_ver_desc" )
                    };
                }
                return this.magicPowerSkill_Poison;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_SootheAnimal
        {
            get
            {
                bool flag = this.magicPowerSkill_SootheAnimal == null;
                if (flag)
                {
                    this.magicPowerSkill_SootheAnimal = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_SootheAnimal_pwr", "TM_SootheAnimal_pwr_desc" ),
                        new MagicPowerSkill("TM_SootheAnimal_eff", "TM_SootheAnimal_eff_desc" )
                    };
                }
                return this.magicPowerSkill_SootheAnimal;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Regenerate
        {
            get
            {
                bool flag = this.magicPowerSkill_Regenerate == null;
                if (flag)
                {
                    this.magicPowerSkill_Regenerate = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Regenerate_pwr", "TM_Regenerate_pwr_desc" ),
                        new MagicPowerSkill("TM_Regenerate_eff", "TM_Regenerate_eff_desc" ),
                        new MagicPowerSkill("TM_Regenerate_ver", "TM_Regenerate_ver_desc" )
                    };
                }
                return this.magicPowerSkill_Regenerate;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_CureDisease
        {
            get
            {
                bool flag = this.magicPowerSkill_CureDisease == null;
                if (flag)
                {
                    this.magicPowerSkill_CureDisease = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_CureDisease_pwr", "TM_CureDisease_pwr_desc" ),
                        new MagicPowerSkill("TM_CureDisease_eff", "TM_CureDisease_eff_desc" ),
                        new MagicPowerSkill("TM_CureDisease_ver", "TM_CureDisease_ver_desc" )
                    };
                }
                return this.magicPowerSkill_CureDisease;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_RegrowLimb
        {
            get
            {
                bool flag = this.magicPowerSkill_RegrowLimb == null;
                if (flag)
                {
                    this.magicPowerSkill_RegrowLimb = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_RegrowLimb_eff", "TM_RegrowLimb_eff_desc")
                    };
                }
                return this.magicPowerSkill_RegrowLimb;
            }
        }

        public List<MagicPower> MagicPowersN
        {
            get
            {
                bool flag = this.magicPowerN == null;
                if (flag)
                {
                    this.magicPowerN = new List<MagicPower>
                    {
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_RaiseUndead
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_DeathMark,
                            TorannMagicDefOf.TM_DeathMark_I,
                            TorannMagicDefOf.TM_DeathMark_II,
                            TorannMagicDefOf.TM_DeathMark_III
                        }),                        
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_FogOfTorment
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_ConsumeCorpse,
                            TorannMagicDefOf.TM_ConsumeCorpse_I,
                            TorannMagicDefOf.TM_ConsumeCorpse_II,
                            TorannMagicDefOf.TM_ConsumeCorpse_III
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_CorpseExplosion,
                            TorannMagicDefOf.TM_CorpseExplosion_I,
                            TorannMagicDefOf.TM_CorpseExplosion_II,
                            TorannMagicDefOf.TM_CorpseExplosion_III
                        }),
                    };
                }
                return this.magicPowerN;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_RaiseUndead
        {
            get
            {
                bool flag = this.magicPowerSkill_RaiseUndead == null;
                if (flag)
                {
                    this.magicPowerSkill_RaiseUndead = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_RaiseUndead_pwr", "TM_RaiseUndead_pwr_desc"),
                        new MagicPowerSkill("TM_RaiseUndead_eff", "TM_RaiseUndead_eff_desc"),
                        new MagicPowerSkill("TM_RaiseUndead_ver", "TM_RaiseUndead_ver_desc")
                    };
                }
                return this.magicPowerSkill_RaiseUndead;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_DeathMark
        {
            get
            {
                bool flag = this.magicPowerSkill_DeathMark == null;
                if (flag)
                {
                    this.magicPowerSkill_DeathMark = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_DeathMark_pwr", "TM_DeathMark_pwr_desc"),
                        new MagicPowerSkill("TM_DeathMark_eff", "TM_DeathMark_eff_desc"),
                        new MagicPowerSkill("TM_DeathMark_ver", "TM_DeathMark_ver_desc")
                    };
                }
                return this.magicPowerSkill_DeathMark;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_FogOfTorment
        {
            get
            {
                bool flag = this.magicPowerSkill_FogOfTorment == null;
                if (flag)
                {
                    this.magicPowerSkill_FogOfTorment = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_FogOfTorment_pwr", "TM_FogOfTorment_pwr_desc"),
                        new MagicPowerSkill("TM_FogOfTorment_eff", "TM_FogOfTorment_eff_desc"),
                        new MagicPowerSkill("TM_FogOfTorment_ver", "TM_FogOfTorment_ver_desc")
                    };
                }
                return this.magicPowerSkill_FogOfTorment;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_ConsumeCorpse
        {
            get
            {
                bool flag = this.magicPowerSkill_ConsumeCorpse == null;
                if (flag)
                {
                    this.magicPowerSkill_ConsumeCorpse = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_ConsumeCorpse_eff", "TM_ConsumeCorpse_eff_desc"),
                        new MagicPowerSkill("TM_ConsumeCorpse_ver", "TM_ConsumeCorpse_ver_desc")
                    };
                }
                return this.magicPowerSkill_ConsumeCorpse;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_CorpseExplosion
        {
            get
            {
                bool flag = this.magicPowerSkill_CorpseExplosion == null;
                if (flag)
                {
                    this.magicPowerSkill_CorpseExplosion = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_CorpseExplosion_pwr", "TM_CorpseExplosion_pwr_desc"),
                        new MagicPowerSkill("TM_CorpseExplosion_eff", "TM_CorpseExplosion_eff_desc"),
                        new MagicPowerSkill("TM_CorpseExplosion_ver", "TM_CorpseExplosion_ver_desc")
                    };
                }
                return this.magicPowerSkill_CorpseExplosion;
            }
        }

        public List<MagicPower> MagicPowersPR
        {
            get
            {
                bool flag = this.magicPowerPR == null;
                if (flag)
                {
                    this.magicPowerPR = new List<MagicPower>
                    {
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_AdvancedHeal
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Purify
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_HealingCircle,
                            TorannMagicDefOf.TM_HealingCircle_I,
                            TorannMagicDefOf.TM_HealingCircle_II,
                            TorannMagicDefOf.TM_HealingCircle_III
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_BestowMight,
                            TorannMagicDefOf.TM_BestowMight_I,
                            TorannMagicDefOf.TM_BestowMight_II,
                            TorannMagicDefOf.TM_BestowMight_III
                        }),
                        new MagicPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Resurrection
                        }),
                    };
                }
                return this.magicPowerPR;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_AdvancedHeal
        {
            get
            {
                bool flag = this.magicPowerSkill_AdvancedHeal == null;
                if (flag)
                {
                    this.magicPowerSkill_AdvancedHeal = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_AdvancedHeal_pwr", "TM_AdvancedHeal_pwr_desc"),
                        new MagicPowerSkill("TM_AdvancedHeal_eff", "TM_AdvancedHeal_eff_desc"),
                        new MagicPowerSkill("TM_AdvancedHeal_ver", "TM_AdvancedHeal_ver_desc")
                    };
                }
                return this.magicPowerSkill_AdvancedHeal;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Purify
        {
            get
            {
                bool flag = this.magicPowerSkill_Purify == null;
                if (flag)
                {
                    this.magicPowerSkill_Purify = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Purify_pwr", "TM_Purify_pwr_desc"),
                        new MagicPowerSkill("TM_Purify_eff", "TM_Purify_eff_desc"),
                        new MagicPowerSkill("TM_Purify_ver", "TM_Purify_ver_desc")
                    };
                }
                return this.magicPowerSkill_Purify;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_HealingCircle
        {
            get
            {
                bool flag = this.magicPowerSkill_HealingCircle == null;
                if (flag)
                {
                    this.magicPowerSkill_HealingCircle = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_HealingCircle_pwr", "TM_HealingCircle_pwr_desc"),
                        new MagicPowerSkill("TM_HealingCircle_eff", "TM_HealingCircle_eff_desc"),
                        new MagicPowerSkill("TM_HealingCircle_ver", "TM_HealingCircle_ver_desc")
                    };
                }
                return this.magicPowerSkill_HealingCircle;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_BestowMight
        {
            get
            {
                bool flag = this.magicPowerSkill_BestowMight == null;
                if (flag)
                {
                    this.magicPowerSkill_BestowMight = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_BestowMight_eff", "TM_BestowMight_eff_desc")
                    };
                }
                return this.magicPowerSkill_BestowMight;
            }
        }
        public List<MagicPowerSkill> MagicPowerSkill_Resurrection
        {
            get
            {
                bool flag = this.magicPowerSkill_Resurrection == null;
                if (flag)
                {
                    this.magicPowerSkill_Resurrection = new List<MagicPowerSkill>
                    {
                        new MagicPowerSkill("TM_Resurrection_eff", "TM_Resurrection_eff_desc"),
                        new MagicPowerSkill("TM_Resurrection_ver", "TM_Resurrection_ver_desc")
                    };
                }
                return this.magicPowerSkill_Resurrection;
            }
        }

        public bool IsNecromancer
        {
            get
            {
                return this.isNecromancer;
            }
            set
            {
                this.isNecromancer = value;
            }               
        }

        public int DominationCount
        {
            get
            {
                return this.dominationCount;
            }
            set
            {
                this.dominationCount = value;
            }
        }

        public bool Initialized
        {
            get
            {
                return this.initialized;
            }
            set
            {
                this.initialized = value;
            }
        }

        public int MagicUserLevel
        {
            get
            {
                return this.magicUserLevel;
            }
            set
            {
                this.magicUserLevel = value;
            }
        }

        public int MagicUserXP
        {
            get
            {
                return this.magicUserXP;
            }
            set
            {
                this.magicUserXP = value;
            }
        }

        public int MagicAbilityPoints
        {
            get
            {
                return this.magicAbilityPoints;
            }
            set
            {
                this.magicAbilityPoints = value;
            }
        }

        public int TicksToLearnMagicXP
        {
            get
            {
                return this.ticksToLearnMagicXP;
            }
            set
            {
                this.ticksToLearnMagicXP = value;
            }
        }

        public int TicksAffiliation
        {
            get
            {
                return this.ticksAffiliation;
            }
            set
            {
                this.ticksAffiliation = value;
            }
        }

        public Pawn MagicPawn
        {
            get
            {
                return this.magicPawn;
            }
        }

        public Faction Affiliation
        {
            get
            {
                return this.affiliation;
            }
            set
            {
                this.affiliation = value;
            }
        }

        public IEnumerable<MagicPower> Powers
        {
            get
            {
                return this.MagicPowersIF.Concat(this.MagicPowersHoF.Concat(this.MagicPowersSB.Concat(this.MagicPowersA.Concat(this.MagicPowersP.Concat(this.MagicPowersS.Concat(this.MagicPowersD.Concat(this.MagicPowersN.Concat(this.MagicPowersPR))))))));
            }
        }

        public MagicData()
        {
        }

        public MagicData(CompAbilityUserMagic newUser)
        {
            this.magicPawn = newUser.AbilityUser;
        }

        public void ExposeData()
        {
            Scribe_References.Look<Pawn>(ref this.magicPawn, "magicPawn", false);
            Scribe_Values.Look<int>(ref this.magicUserLevel, "magicUserLevel", 0, false);
            Scribe_Values.Look<int>(ref this.magicUserXP, "magicUserXP", 0, false);
            Scribe_Values.Look<bool>(ref this.initialized, "initialized", false, false);
            Scribe_Values.Look<int>(ref this.magicAbilityPoints, "magicAbilityPoints", 0, false);
            Scribe_Values.Look<int>(ref this.ticksToLearnMagicXP, "ticksToLearnMagicXP", -1, false);
            Scribe_Values.Look<int>(ref this.ticksAffiliation, "ticksAffiliation", -1, false);
            Scribe_Values.Look<int>(ref this.dominationCount, "dominationCount", 0, false);
            Scribe_Values.Look<bool>(ref this.isNecromancer, "isNecromancer", false, false);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_global_eff, "magicPowerSkill_global_eff", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_global_regen, "magicPowerSkill_global_regen", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_global_spirit, "magicPowerSkill_global_spirit", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPower>(ref this.magicPowerIF, "magicPowerIF", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_RayofHope, "magicPowerSkill_RayofHope", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Fireball, "magicPowerSkill_Fireball", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Firebolt, "magicPowerSkill_Firebolt", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Fireclaw, "magicPowerSkill_Fireclaw", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Firestorm, "magicPowerSkill_Firestorm", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPower>(ref this.magicPowerHoF, "magicPowerHoF", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Soothe, "magicPowerSkill_Soothe", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Icebolt, "magicPowerSkill_Icebolt", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_FrostRay, "magicPowerSkill_FrostRay", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Snowball, "magicPowerSkill_Snowball", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Rainmaker, "magicPowerSkill_Rainmaker", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Blizzard, "magicPowerSkill_Blizzard", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPower>(ref this.magicPowerSB, "magicPowerSB", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_AMP, "magicPowerSkill_AMP", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_LightningBolt, "magicPowerSkill_LightningBolt", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_LightningCloud, "magicPowerSkill_LightningCloud", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_LightningStorm, "magicPowerSkill_LightningStorm", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_EyeOfTheStorm, "magicPowerSkill_EyeOfTheStorm", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPower>(ref this.magicPowerA, "magicPowerA", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Shadow, "magicPowerSkill_Shadow", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_MagicMissile, "magicPowerSkill_MagicMissile", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Blink, "magicPowerSkill_Blink", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Summon, "magicPowerSkill_Summon", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Teleport, "magicPowerSkill_Teleport", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_FoldReality, "magicPowerSkill_FoldReality", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPower>(ref this.magicPowerP, "magicPowerP", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Heal, "magicPowerSkill_Heal", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Shield, "magicPowerSkill_Shield", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_ValiantCharge, "magicPowerSkill_ValiantCharge", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Overwhelm, "magicPowerSkill_Overwhelm", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPower>(ref this.magicPowerS, "magicPowerS", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_SummonMinion, "magicPowerSkill_SummonMinion", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_SummonPylon, "magicPowerSkill_SummonPylon", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_SummonExplosive, "magicPowerSkill_SummonExplosive", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_SummonElemental, "magicPowerSkill_SummonElemental", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPower>(ref this.magicPowerD, "magicPowerD", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Poison, "magicPowerSkill_Poison", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_SootheAnimal, "magicPowerSkill_SootheAnimal", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Regenerate, "magicPowerSkill_Regenerate", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_CureDisease, "magicPowerSkill_CureDisease", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_RegrowLimb, "magicPowerSkill_RegrowLimb", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPower>(ref this.magicPowerN, "magicPowerN", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_RaiseUndead, "magicPowerSkill_RaiseUndead", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_DeathMark, "magicPowerSkill_DeathMark", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_FogOfTorment, "magicPowerSkill_FogOfTorment", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_ConsumeCorpse, "magicPowerSkill_ConsumeCorpse", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_CorpseExplosion, "magicPowerSkill_CorpseExplosion", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPower>(ref this.magicPowerPR, "magicPowerPR", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_AdvancedHeal, "magicPowerSkill_AdvancedHeal", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Purify, "magicPowerSkill_Purify", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_HealingCircle, "magicPowerSkill_HealingCircle", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_BestowMight, "magicPowerSkill_BestowMight", LookMode.Deep, new object[0]);
            Scribe_Collections.Look<MagicPowerSkill>(ref this.magicPowerSkill_Resurrection, "magicPowerSkill_Resurrection", LookMode.Deep, new object[0]);
        }
    }
}
