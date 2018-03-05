using AbilityUser;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace TorannMagic
{
    public class MightData : IExposable
    {
        private Pawn mightPawn;        
        private int mightUserLevel = 0;
        private int mightAbilityPoints = 0;
        private int mightUserXP = 1;
        private int ticksToLearnMightXP = -1;
        public bool initialized = false;
        private Faction affiliation = null;
        private int ticksAffiliation = 0;

        public List<MightPower> mightPowerG;
        public List<MightPowerSkill> mightPowerSkill_Sprint;
        public List<MightPowerSkill> mightPowerSkill_Fortitude;
        public List<MightPowerSkill> mightPowerSkill_Grapple;
        public List<MightPowerSkill> mightPowerSkill_Cleave;
        public List<MightPowerSkill> mightPowerSkill_Whirlwind;

        public List<MightPower> MightPowersG
        {
            get
            {
                bool flag = this.mightPowerG == null;
                if (flag)
                {
                    this.mightPowerG = new List<MightPower>
                    {
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Sprint,
                            TorannMagicDefOf.TM_Sprint_I,
                            TorannMagicDefOf.TM_Sprint_II,
                            TorannMagicDefOf.TM_Sprint_III
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Fortitude
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Grapple,
                            TorannMagicDefOf.TM_Grapple_I,
                            TorannMagicDefOf.TM_Grapple_II,
                            TorannMagicDefOf.TM_Grapple_III
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Cleave
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Whirlwind
                        }),
                    };
                }
                return this.mightPowerG;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_Sprint
        {
            get
            {
                bool flag = this.mightPowerSkill_Sprint == null;
                if (flag)
                {
                    this.mightPowerSkill_Sprint = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_Sprint_pwr", "TM_Sprint_pwr_desc"),
                        new MightPowerSkill("TM_Sprint_eff", "TM_Sprint_eff_desc")
                    };
                }
                return this.mightPowerSkill_Sprint;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_Fortitude
        {
            get
            {
                bool flag = this.mightPowerSkill_Fortitude == null;
                if (flag)
                {
                    this.mightPowerSkill_Fortitude = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_Fortitude_pwr", "TM_Fortitude_pwr_desc"),
                        new MightPowerSkill("TM_Fortitude_ver", "TM_Fortitude_ver_desc")
                    };
                }
                return this.mightPowerSkill_Fortitude;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_Grapple
        {
            get
            {
                bool flag = this.mightPowerSkill_Grapple == null;
                if (flag)
                {
                    this.mightPowerSkill_Grapple = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_Grapple_eff", "TM_Grapple_eff_desc")
                    };
                }
                return this.mightPowerSkill_Grapple;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_Cleave
        {
            get
            {
                bool flag = this.mightPowerSkill_Cleave == null;
                if (flag)
                {
                    this.mightPowerSkill_Cleave = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_Cleave_pwr", "TM_Cleave_pwr_desc"),
                        new MightPowerSkill("TM_Cleave_eff", "TM_Cleave_eff_desc"),
                        new MightPowerSkill("TM_Cleave_ver", "TM_Cleave_ver_desc")
                    };
                }
                return this.mightPowerSkill_Cleave;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_Whirlwind
        {
            get
            {
                bool flag = this.mightPowerSkill_Whirlwind == null;
                if (flag)
                {
                    this.mightPowerSkill_Whirlwind = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_Whirlwind_pwr", "TM_Whirlwind_pwr_desc"),
                        new MightPowerSkill("TM_Whirlwind_eff", "TM_Whirlwind_eff_desc"),
                        new MightPowerSkill("TM_Whirlwind_ver", "TM_Whirlwind_ver_desc")
                    };
                }
                return this.mightPowerSkill_Whirlwind;
            }
        }

        public List<MightPower> mightPowerS;
        public List<MightPowerSkill> mightPowerSkill_SniperFocus;
        public List<MightPowerSkill> mightPowerSkill_Headshot;
        public List<MightPowerSkill> mightPowerSkill_DisablingShot;
        public List<MightPowerSkill> mightPowerSkill_AntiArmor;

        public List<MightPower> MightPowersS
        {
            get
            {
                bool flag = this.mightPowerS == null;
                if (flag)
                {
                    this.mightPowerS = new List<MightPower>
                    {
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_SniperFocus
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Headshot
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_DisablingShot,
                            TorannMagicDefOf.TM_DisablingShot_I,
                            TorannMagicDefOf.TM_DisablingShot_II,
                            TorannMagicDefOf.TM_DisablingShot_III
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_AntiArmor
                        }),
                    };
                }
                return this.mightPowerS;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_SniperFocus
        {
            get
            {
                bool flag = this.mightPowerSkill_SniperFocus == null;
                if (flag)
                {
                    this.mightPowerSkill_SniperFocus = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_SniperFocus_pwr", "TM_SniperFocus_pwr_desc")
                    };
                }
                return this.mightPowerSkill_SniperFocus;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_Headshot
        {
            get
            {
                bool flag = this.mightPowerSkill_Headshot == null;
                if (flag)
                {
                    this.mightPowerSkill_Headshot = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_Headshot_pwr", "TM_Headshot_pwr_desc"),
                        new MightPowerSkill("TM_Headshot_eff", "TM_Headshot_eff_desc"),
                        new MightPowerSkill("TM_Headshot_ver", "TM_Headshot_ver_desc")
                    };
                }
                return this.mightPowerSkill_Headshot;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_DisablingShot
        {
            get
            {
                bool flag = this.mightPowerSkill_DisablingShot == null;
                if (flag)
                {
                    this.mightPowerSkill_DisablingShot = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_DisablingShot_eff", "TM_DisablingShot_eff_desc"),
                        new MightPowerSkill("TM_DisablingShot_ver", "TM_DisablingShot_ver_desc")
                    };
                }
                return this.mightPowerSkill_DisablingShot;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_AntiArmor
        {
            get
            {
                bool flag = this.mightPowerSkill_AntiArmor == null;
                if (flag)
                {
                    this.mightPowerSkill_AntiArmor = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_AntiArmor_pwr", "TM_AntiArmor_pwr_desc"),
                        new MightPowerSkill("TM_AntiArmor_eff", "TM_AntiArmor_eff_desc"),
                        new MightPowerSkill("TM_AntiArmor_ver", "TM_AntiArmor_ver_desc")
                    };
                }
                return this.mightPowerSkill_AntiArmor;
            }
        }

        public List<MightPower> mightPowerB;
        public List<MightPowerSkill> mightPowerSkill_BladeFocus;
        public List<MightPowerSkill> mightPowerSkill_BladeArt;
        public List<MightPowerSkill> mightPowerSkill_SeismicSlash;
        public List<MightPowerSkill> mightPowerSkill_BladeSpin;
        public List<MightPowerSkill> mightPowerSkill_PhaseStrike;

        public List<MightPower> MightPowersB
        {
            get
            {
                bool flag = this.mightPowerB == null;
                if (flag)
                {
                    this.mightPowerB = new List<MightPower>
                    {
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_BladeFocus
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_BladeArt
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_SeismicSlash
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_BladeSpin
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_PhaseStrike,
                            TorannMagicDefOf.TM_PhaseStrike_I,
                            TorannMagicDefOf.TM_PhaseStrike_II,
                            TorannMagicDefOf.TM_PhaseStrike_III
                        }),
                    };
                }
                return this.mightPowerB;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_BladeFocus
        {
            get
            {
                bool flag = this.mightPowerSkill_BladeFocus == null;
                if (flag)
                {
                    this.mightPowerSkill_BladeFocus = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_BladeFocus_pwr", "TM_BladeFocus_pwr_desc")
                    };
                }
                return this.mightPowerSkill_BladeFocus;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_BladeArt
        {
            get
            {
                bool flag = this.mightPowerSkill_BladeArt == null;
                if (flag)
                {
                    this.mightPowerSkill_BladeArt = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_BladeArt_pwr", "TM_BladeArt_pwr_desc")
                    };
                }
                return this.mightPowerSkill_BladeArt;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_SeismicSlash
        {
            get
            {
                bool flag = this.mightPowerSkill_SeismicSlash == null;
                if (flag)
                {
                    this.mightPowerSkill_SeismicSlash = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_SeismicSlash_pwr", "TM_SeismicSlash_pwr_desc"),
                        new MightPowerSkill("TM_SeismicSlash_eff", "TM_SeismicSlash_eff_desc"),
                        new MightPowerSkill("TM_SeismicSlash_ver", "TM_SeismicSlash_ver_desc")
                    };
                }
                return this.mightPowerSkill_SeismicSlash;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_BladeSpin
        {
            get
            {
                bool flag = this.mightPowerSkill_BladeSpin == null;
                if (flag)
                {
                    this.mightPowerSkill_BladeSpin = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_BladeSpin_pwr", "TM_BladeSpin_pwr_desc"),
                        new MightPowerSkill("TM_BladeSpin_eff", "TM_BladeSpin_eff_desc"),
                        new MightPowerSkill("TM_BladeSpin_ver", "TM_BladeSpin_ver_desc")
                    };
                }
                return this.mightPowerSkill_BladeSpin;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_PhaseStrike
        {
            get
            {
                bool flag = this.mightPowerSkill_PhaseStrike == null;
                if (flag)
                {
                    this.mightPowerSkill_PhaseStrike = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_PhaseStrike_pwr", "TM_PhaseStrike_pwr_desc"),
                        new MightPowerSkill("TM_PhaseStrike_eff", "TM_PhaseStrike_eff_desc"),
                        new MightPowerSkill("TM_PhaseStrike_ver", "TM_PhaseStrike_ver_desc")
                    };
                }
                return this.mightPowerSkill_PhaseStrike;
            }
        }

        public List<MightPower> mightPowerR;
        public List<MightPowerSkill> mightPowerSkill_RangerTraining;
        public List<MightPowerSkill> mightPowerSkill_BowTraining;
        public List<MightPowerSkill> mightPowerSkill_PoisonTrap;
        public List<MightPowerSkill> mightPowerSkill_AnimalFriend;
        public List<MightPowerSkill> mightPowerSkill_ArrowStorm;

        public List<MightPower> MightPowersR
        {
            get
            {
                bool flag = this.mightPowerR == null;
                if (flag)
                {
                    this.mightPowerR = new List<MightPower>
                    {
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_RangerTraining
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_BowTraining
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_PoisonTrap
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_AnimalFriend
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_ArrowStorm,
                            TorannMagicDefOf.TM_ArrowStorm_I,
                            TorannMagicDefOf.TM_ArrowStorm_II,
                            TorannMagicDefOf.TM_ArrowStorm_III
                        }),
                    };
                }
                return this.mightPowerR;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_RangerTraining
        {
            get
            {
                bool flag = this.mightPowerSkill_RangerTraining == null;
                if (flag)
                {
                    this.mightPowerSkill_RangerTraining = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_RangerTraining_pwr", "TM_RangerTraining_pwr_desc")
                    };
                }
                return this.mightPowerSkill_RangerTraining;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_BowTraining
        {
            get
            {
                bool flag = this.mightPowerSkill_BowTraining == null;
                if (flag)
                {
                    this.mightPowerSkill_BowTraining = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_BowTraining_pwr", "TM_BowTraining_pwr_desc")
                    };
                }
                return this.mightPowerSkill_BowTraining;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_PoisonTrap
        {
            get
            {
                bool flag = this.mightPowerSkill_PoisonTrap == null;
                if (flag)
                {
                    this.mightPowerSkill_PoisonTrap = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_PoisonTrap_ver", "TM_PoisonTrap_ver_desc")
                    };
                }
                return this.mightPowerSkill_PoisonTrap;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_AnimalFriend
        {
            get
            {
                bool flag = this.mightPowerSkill_AnimalFriend == null;
                if (flag)
                {
                    this.mightPowerSkill_AnimalFriend = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_AnimalFriend_pwr", "TM_AnimalFriend_pwr_desc"),
                        new MightPowerSkill("TM_AnimalFriend_eff", "TM_AnimalFriend_eff_desc"),
                        new MightPowerSkill("TM_AnimalFriend_ver", "TM_AnimalFriend_ver_desc")
                    };
                }
                return this.mightPowerSkill_AnimalFriend;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_ArrowStorm
        {
            get
            {
                bool flag = this.mightPowerSkill_ArrowStorm == null;
                if (flag)
                {
                    this.mightPowerSkill_ArrowStorm = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_ArrowStorm_pwr", "TM_ArrowStorm_pwr_desc"),
                        new MightPowerSkill("TM_ArrowStorm_eff", "TM_ArrowStorm_eff_desc"),
                        new MightPowerSkill("TM_ArrowStorm_ver", "TM_ArrowStorm_ver_desc")
                    };
                }
                return this.mightPowerSkill_ArrowStorm;
            }
        }

        public List<MightPowerSkill> mightPowerSkill_global_refresh;
        public List<MightPowerSkill> mightPowerSkill_global_seff;
        public List<MightPowerSkill> mightPowerSkill_global_strength;
        public List<MightPowerSkill> mightPowerSkill_global_endurance;

        public List<MightPowerSkill> MightPowerSkill_global_refresh
        {
            get
            {
                bool flag = this.mightPowerSkill_global_refresh == null;
                if (flag)
                {
                    this.mightPowerSkill_global_refresh = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_global_refresh_pwr", "TM_global_refresh_pwr_desc")
                    };
                }
                return this.mightPowerSkill_global_refresh;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_global_seff
        {
            get
            {
                bool flag = this.mightPowerSkill_global_seff == null;
                if (flag)
                {
                    this.mightPowerSkill_global_seff = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_global_seff_pwr", "TM_global_seff_pwr_desc")
                    };
                }
                return this.mightPowerSkill_global_seff;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_global_strength
        {
            get
            {
                bool flag = this.mightPowerSkill_global_strength == null;
                if (flag)
                {
                    this.mightPowerSkill_global_strength = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_global_strength_pwr", "TM_global_strength_pwr_desc")
                    };
                }
                return this.mightPowerSkill_global_strength;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_global_endurance
        {
            get
            {
                bool flag = this.mightPowerSkill_global_endurance == null;
                if (flag)
                {
                    this.mightPowerSkill_global_endurance = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_global_endurance_pwr", "TM_global_endurance_pwr_desc")
                    };
                }
                return this.mightPowerSkill_global_endurance;
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

        public int MightUserLevel
        {
            get
            {
                return this.mightUserLevel;
            }
            set
            {
                this.mightUserLevel = value;
            }
        }

        public int MightUserXP
        {
            get
            {
                return this.mightUserXP;
            }
            set
            {
                this.mightUserXP = value;
            }
        }

        public int MightAbilityPoints
        {
            get
            {
                return this.mightAbilityPoints;
            }
            set
            {
                this.mightAbilityPoints = value;
            }
        }

        public int TicksToLearnMightXP
        {
            get
            {
                return this.ticksToLearnMightXP;
            }
            set
            {
                this.ticksToLearnMightXP = value;
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

        public Pawn MightPawn
        {
            get
            {
                return this.mightPawn;
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

        public IEnumerable<MightPower> Powers
        {
            get
            {
                return this.MightPowersG.Concat(this.MightPowersS.Concat(this.MightPowersB.Concat(this.mightPowerR)));
            }
        }

        public MightData()
        {
        }

        public MightData(CompAbilityUserMight newUser)
        {
            this.mightPawn = newUser.AbilityUser;
        }

        public void ExposeData()
        {
            Scribe_References.Look<Pawn>(ref this.mightPawn, "mightPawn", false);
            Scribe_Values.Look<int>(ref this.mightUserLevel, "mightUserLevel", 0, false);
            Scribe_Values.Look<int>(ref this.mightUserXP, "mightUserXP", 0, false);
            Scribe_Values.Look<bool>(ref this.initialized, "initialized", false, false);
            Scribe_Values.Look<int>(ref this.mightAbilityPoints, "mightAbilityPoints", 0, false);
            Scribe_Values.Look<int>(ref this.ticksToLearnMightXP, "ticksToLearnMightXP", -1, false);
            Scribe_Values.Look<int>(ref this.ticksAffiliation, "ticksAffiliation", -1, false);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_global_refresh, "mightPowerSkill_global_refresh", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_global_seff, "mightPowerSkill_global_seff", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_global_strength, "mightPowerSkill_global_strength", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_global_endurance, "mightPowerSkill_global_endurance", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPower>(ref this.mightPowerG, "mightPowerG", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_Sprint, "mightPowerSkill_Sprint", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_Fortitude, "mightPowerSkill_Fortitude", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_Grapple, "mightPowerSkill_Grapple", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_Cleave, "mightPowerSkill_Cleave", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_Whirlwind, "mightPowerSkill_Whirlwind", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPower>(ref this.mightPowerS, "mightPowerS", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_SniperFocus, "mightPowerSkill_SniperFocus", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_Headshot, "mightPowerSkill_Headshot", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_DisablingShot, "mightPowerSkill_DisablingShot", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_AntiArmor, "mightPowerSkill_AntiArmor", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPower>(ref this.mightPowerB, "mightPowerB", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_BladeFocus, "mightPowerSkill_BladeFocus", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_BladeArt, "mightPowerSkill_BladeArt", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_SeismicSlash, "mightPowerSkill_SeismicSlash", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_BladeSpin, "mightPowerSkill_BladeSpin", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_PhaseStrike, "mightPowerSkill_PhaseStrike", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPower>(ref this.mightPowerR, "mightPowerR", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_RangerTraining, "mightPowerSkill_RangerTraining", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_BowTraining, "mightPowerSkill_BowTraining", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_PoisonTrap, "mightPowerSkill_PoisonTrap", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_AnimalFriend, "mightPowerSkill_AnimalFriend", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_ArrowStorm, "mightPowerSkill_ArrowStorm", (LookMode)2, new object[0]);
        }

    }
}
