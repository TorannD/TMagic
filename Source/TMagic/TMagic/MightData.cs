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

        public List<MightPower> mightPowerStandalone;

        public List<MightPower> MightPowersStandalone  //skills needed for mightcpower reference during autocast
        {
            get
            {
                bool flag = this.mightPowerStandalone == null;
                if (flag)
                {
                    this.mightPowerStandalone = new List<MightPower>
                    {
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_TeachMight
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_GearRepair
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_InnerHealing
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_HeavyBlow
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_StrongBack
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_ThickSkin
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_FightersFocus
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_ThrowingKnife
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_BurningFury
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_PommelStrike
                        }),
                    };
                }
                return this.mightPowerStandalone;
            }
        }

        public List<MightPower> mightPowerM;
        public List<MightPowerSkill> mightPowerSkill_Chi;
        public List<MightPowerSkill> mightPowerSkill_MindOverBody;
        public List<MightPowerSkill> mightPowerSkill_Meditate;
        public List<MightPowerSkill> mightPowerSkill_TigerStrike;
        public List<MightPowerSkill> mightPowerSkill_DragonStrike;
        public List<MightPowerSkill> mightPowerSkill_ThunderStrike;

        public List<MightPower> MightPowersM
        {
            get
            {
                bool flag = this.mightPowerM == null;
                if (flag)
                {
                    this.mightPowerM = new List<MightPower>
                    {
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Chi
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_MindOverBody
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Meditate
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_TigerStrike
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_DragonStrike
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_ThunderStrike
                        }),
                    };
                }
                return this.mightPowerM;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_Chi
        {
            get
            {
                bool flag = this.mightPowerSkill_Chi== null;
                if (flag)
                {
                    this.mightPowerSkill_Chi = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_Chi_pwr", "TM_Chi_pwr_desc"), //
                        new MightPowerSkill("TM_Chi_eff", "TM_Chi_eff_desc"), //
                        new MightPowerSkill("TM_Chi_ver", "TM_Chi_ver_desc")  //
                    };
                }
                return this.mightPowerSkill_Chi;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_MindOverBody
        {
            get
            {
                bool flag = this.mightPowerSkill_MindOverBody == null;
                if (flag)
                {
                    this.mightPowerSkill_MindOverBody = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_MindOverBody_pwr", "TM_MindOverBody_pwr_desc"), //
                        new MightPowerSkill("TM_MindOverBody_ver", "TM_MindOverBody_ver_desc")
                    };
                }
                return this.mightPowerSkill_MindOverBody;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_Meditate
        {
            get
            {
                bool flag = this.mightPowerSkill_Meditate == null;
                if (flag)
                {
                    this.mightPowerSkill_Meditate = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_Meditate_pwr", "TM_Meditate_pwr_desc"),
                        new MightPowerSkill("TM_Meditate_eff", "TM_Meditate_eff_desc"),
                        new MightPowerSkill("TM_Meditate_ver", "TM_Meditate_ver_desc")
                    };
                }
                return this.mightPowerSkill_Meditate;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_TigerStrike
        {
            get
            {
                bool flag = this.mightPowerSkill_TigerStrike == null;
                if (flag)
                {
                    this.mightPowerSkill_TigerStrike = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_TigerStrike_pwr", "TM_TigerStrike_pwr_desc"),
                        new MightPowerSkill("TM_TigerStrike_eff", "TM_TigerStrike_eff_desc"),
                        new MightPowerSkill("TM_TigerStrike_ver", "TM_TigerStrike_ver_desc")
                    };
                }
                return this.mightPowerSkill_TigerStrike;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_DragonStrike
        {
            get
            {
                bool flag = this.mightPowerSkill_DragonStrike == null;
                if (flag)
                {
                    this.mightPowerSkill_DragonStrike = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_DragonStrike_pwr", "TM_DragonStrike_pwr_desc"), //
                        new MightPowerSkill("TM_DragonStrike_eff", "TM_DragonStrike_eff_desc"),
                        new MightPowerSkill("TM_DragonStrike_ver", "TM_DragonStrike_ver_desc") //
                    };
                }
                return this.mightPowerSkill_DragonStrike;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_ThunderStrike
        {
            get
            {
                bool flag = this.mightPowerSkill_ThunderStrike == null;
                if (flag)
                {
                    this.mightPowerSkill_ThunderStrike = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_ThunderStrike_pwr", "TM_ThunderStrike_pwr_desc"), //
                        new MightPowerSkill("TM_ThunderStrike_eff", "TM_ThunderStrike_eff_desc"),
                        new MightPowerSkill("TM_ThunderStrike_ver", "TM_ThunderStrike_ver_desc") //
                    };
                }
                return this.mightPowerSkill_ThunderStrike;
            }
        }

        public List<MightPower> mightPowerDK;
        public List<MightPowerSkill> mightPowerSkill_Shroud;
        public List<MightPowerSkill> mightPowerSkill_WaveOfFear;
        public List<MightPowerSkill> mightPowerSkill_Spite;
        public List<MightPowerSkill> mightPowerSkill_LifeSteal;
        public List<MightPowerSkill> mightPowerSkill_GraveBlade;

        public List<MightPower> MightPowersDK
        {
            get
            {
                bool flag = this.mightPowerDK == null;
                if (flag)
                {
                    this.mightPowerDK = new List<MightPower>
                    {
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Shroud
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_WaveOfFear
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Spite,
                            TorannMagicDefOf.TM_Spite_I,
                            TorannMagicDefOf.TM_Spite_II,
                            TorannMagicDefOf.TM_Spite_III
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_LifeSteal
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_GraveBlade,
                            TorannMagicDefOf.TM_GraveBlade_I,
                            TorannMagicDefOf.TM_GraveBlade_II,
                            TorannMagicDefOf.TM_GraveBlade_III
                        }),
                    };
                }
                return this.mightPowerDK;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_Shroud
        {
            get
            {
                bool flag = this.mightPowerSkill_Shroud == null;
                if (flag)
                {
                    this.mightPowerSkill_Shroud = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_Shroud_pwr", "TM_Shroud_pwr_desc"), //psionic punch / manipulation
                        new MightPowerSkill("TM_Shroud_eff", "TM_Shroud_eff_desc"), //psionic dash / movement
                        new MightPowerSkill("TM_Shroud_ver", "TM_Shroud_ver_desc")  //psionic preassure
                    };
                }
                return this.mightPowerSkill_Shroud;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_WaveOfFear
        {
            get
            {
                bool flag = this.mightPowerSkill_WaveOfFear == null;
                if (flag)
                {
                    this.mightPowerSkill_WaveOfFear = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_WaveOfFear_pwr", "TM_WaveOfFear_pwr_desc"), //applies skill powers to ability
                        new MightPowerSkill("TM_WaveOfFear_eff", "TM_WaveOfFear_eff_desc"), //increases how long ability is available and reduces stamina cost to acquire
                        new MightPowerSkill("TM_WaveOfFear_ver", "TM_WaveOfFear_ver_desc")
                    };
                }
                return this.mightPowerSkill_WaveOfFear;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_Spite
        {
            get
            {
                bool flag = this.mightPowerSkill_Spite == null;
                if (flag)
                {
                    this.mightPowerSkill_Spite = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_Spite_pwr", "TM_Spite_pwr_desc"),
                        new MightPowerSkill("TM_Spite_eff", "TM_Spite_eff_desc"),
                        new MightPowerSkill("TM_Spite_ver", "TM_Spite_ver_desc")
                    };
                }
                return this.mightPowerSkill_Spite;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_LifeSteal
        {
            get
            {
                bool flag = this.mightPowerSkill_LifeSteal == null;
                if (flag)
                {
                    this.mightPowerSkill_LifeSteal = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_LifeSteal_pwr", "TM_LifeSteal_pwr_desc"),
                        new MightPowerSkill("TM_LifeSteal_eff", "TM_LifeSteal_eff_desc"),
                        new MightPowerSkill("TM_LifeSteal_ver", "TM_LifeSteal_ver_desc")
                    };
                }
                return this.mightPowerSkill_LifeSteal;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_GraveBlade
        {
            get
            {
                bool flag = this.mightPowerSkill_GraveBlade == null;
                if (flag)
                {
                    this.mightPowerSkill_GraveBlade = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_GraveBlade_pwr", "TM_GraveBlade_pwr_desc"), //duration of possession, 
                        new MightPowerSkill("TM_GraveBlade_eff", "TM_GraveBlade_eff_desc"),
                        new MightPowerSkill("TM_GraveBlade_ver", "TM_GraveBlade_ver_desc") //applies mental states or effects , fewer debuffs during possession
                    };
                }
                return this.mightPowerSkill_GraveBlade;
            }
        }

        public List<MightPower> mightPowerP;
        public List<MightPowerSkill> mightPowerSkill_PsionicAugmentation;
        public List<MightPowerSkill> mightPowerSkill_PsionicBlast;
        public List<MightPowerSkill> mightPowerSkill_PsionicDash;
        public List<MightPowerSkill> mightPowerSkill_PsionicBarrier;
        public List<MightPowerSkill> mightPowerSkill_PsionicStorm;

        public List<MightPower> MightPowersP
        {
            get
            {
                bool flag = this.mightPowerP == null;
                if (flag)
                {
                    this.mightPowerP = new List<MightPower>
                    {
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_PsionicAugmentation
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_PsionicBarrier,
                            TorannMagicDefOf.TM_PsionicBarrier_Projected
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_PsionicBlast,
                            TorannMagicDefOf.TM_PsionicBlast_I,
                            TorannMagicDefOf.TM_PsionicBlast_II,
                            TorannMagicDefOf.TM_PsionicBlast_III
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_PsionicDash
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_PsionicStorm
                        }),
                    };
                }
                return this.mightPowerP;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_PsionicAugmentation
        {
            get
            {
                bool flag = this.mightPowerSkill_PsionicAugmentation == null;
                if (flag)
                {
                    this.mightPowerSkill_PsionicAugmentation = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_PsionicAugmentation_pwr", "TM_PsionicAugmentation_pwr_desc"), //psionic punch / manipulation
                        new MightPowerSkill("TM_PsionicAugmentation_eff", "TM_PsionicAugmentation_eff_desc"), //psionic dash / movement
                        new MightPowerSkill("TM_PsionicAugmentation_ver", "TM_PsionicAugmentation_ver_desc")  //psionic preassure
                    };
                }
                return this.mightPowerSkill_PsionicAugmentation;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_PsionicBarrier
        {
            get
            {
                bool flag = this.mightPowerSkill_PsionicBarrier == null;
                if (flag)
                {
                    this.mightPowerSkill_PsionicBarrier = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_PsionicBarrier_pwr", "TM_PsionicBarrier_pwr_desc"), //applies skill powers to ability
                        new MightPowerSkill("TM_PsionicBarrier_eff", "TM_PsionicBarrier_eff_desc"), //increases how long ability is available and reduces stamina cost to acquire
                        new MightPowerSkill("TM_PsionicBarrier_ver", "TM_PsionicBarrier_ver_desc")
                    };
                }
                return this.mightPowerSkill_PsionicBarrier;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_PsionicBlast
        {
            get
            {
                bool flag = this.mightPowerSkill_PsionicBlast == null;
                if (flag)
                {
                    this.mightPowerSkill_PsionicBlast = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_PsionicBlast_pwr", "TM_PsionicBlast_pwr_desc"), 
                        new MightPowerSkill("TM_PsionicBlast_eff", "TM_PsionicBlast_eff_desc"),
                        new MightPowerSkill("TM_PsionicBlast_ver", "TM_PsionicBlast_ver_desc") 
                    };
                }
                return this.mightPowerSkill_PsionicBlast;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_PsionicDash
        {
            get
            {
                bool flag = this.mightPowerSkill_PsionicDash == null;
                if (flag)
                {
                    this.mightPowerSkill_PsionicDash = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_PsionicDash_pwr", "TM_PsionicDash_pwr_desc"),
                        new MightPowerSkill("TM_PsionicDash_eff", "TM_PsionicDash_eff_desc"),
                        new MightPowerSkill("TM_PsionicDash_ver", "TM_PsionicDash_ver_desc") 
                    };
                }
                return this.mightPowerSkill_PsionicDash;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_PsionicStorm
        {
            get
            {
                bool flag = this.mightPowerSkill_PsionicStorm == null;
                if (flag)
                {
                    this.mightPowerSkill_PsionicStorm = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_PsionicStorm_pwr", "TM_PsionicStorm_pwr_desc"), //duration of possession, 
                        new MightPowerSkill("TM_PsionicStorm_eff", "TM_PsionicStorm_eff_desc"),
                        new MightPowerSkill("TM_PsionicStorm_ver", "TM_PsionicStorm_ver_desc") //applies mental states or effects , fewer debuffs during possession
                    };
                }
                return this.mightPowerSkill_PsionicStorm;
            }
        }

        public List<MightPower> mightPowerF;
        public List<MightPowerSkill> mightPowerSkill_Disguise;
        public List<MightPowerSkill> mightPowerSkill_Mimic;
        public List<MightPowerSkill> mightPowerSkill_Reversal;
        public List<MightPowerSkill> mightPowerSkill_Transpose;
        public List<MightPowerSkill> mightPowerSkill_Possess;

        public List<MightPower> MightPowersF
        {
            get
            {
                bool flag = this.mightPowerF == null;
                if (flag)
                {
                    this.mightPowerF = new List<MightPower>
                    {
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Disguise
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Mimic
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Reversal
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Transpose,
                            TorannMagicDefOf.TM_Transpose_I,
                            TorannMagicDefOf.TM_Transpose_II,
                            TorannMagicDefOf.TM_Transpose_III
                        }),
                        new MightPower(new List<AbilityDef>
                        {
                            TorannMagicDefOf.TM_Possess
                        }),
                    };
                }
                return this.mightPowerF;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_Disguise
        {
            get
            {
                bool flag = this.mightPowerSkill_Disguise == null;
                if (flag)
                {
                    this.mightPowerSkill_Disguise = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_Disguise_pwr", "TM_Disguise_pwr_desc"), //duration of skill
                        new MightPowerSkill("TM_Disguise_eff", "TM_Disguise_eff_desc"), 
                        new MightPowerSkill("TM_Disguise_ver", "TM_Disguise_ver_desc") //chance to avoid detection at different ranges
                    };
                }
                return this.mightPowerSkill_Disguise;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_Mimic
        {
            get
            {
                bool flag = this.mightPowerSkill_Mimic == null;
                if (flag)
                {
                    this.mightPowerSkill_Mimic = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_Mimic_pwr", "TM_Mimic_pwr_desc"), //applies skill powers to ability
                        new MightPowerSkill("TM_Mimic_eff", "TM_Mimic_eff_desc"), //increases how long ability is available and reduces stamina cost to acquire
                        new MightPowerSkill("TM_Mimic_ver", "TM_Mimic_ver_desc")
                    };
                }
                return this.mightPowerSkill_Mimic;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_Reversal
        {
            get
            {
                bool flag = this.mightPowerSkill_Reversal == null;
                if (flag)
                {
                    this.mightPowerSkill_Reversal = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_Reversal_pwr", "TM_Reversal_pwr_desc"), //increases duration of skill
                        new MightPowerSkill("TM_Reversal_eff", "TM_Reversal_eff_desc"),
                        new MightPowerSkill("TM_Reversal_ver", "TM_Reversal_ver_desc") //regenerative reversal
                    };
                }
                return this.mightPowerSkill_Reversal;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_Transpose
        {
            get
            {
                bool flag = this.mightPowerSkill_Transpose == null;
                if (flag)
                {
                    this.mightPowerSkill_Transpose = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_Transpose_eff", "TM_Transpose_eff_desc"),
                        new MightPowerSkill("TM_Transpose_ver", "TM_Transpose_ver_desc") //usable on enemies, usable on friendly beyond los, usable on enemy blos
                    };
                }
                return this.mightPowerSkill_Transpose;
            }
        }
        public List<MightPowerSkill> MightPowerSkill_Possess
        {
            get
            {
                bool flag = this.mightPowerSkill_Possess == null;
                if (flag)
                {
                    this.mightPowerSkill_Possess = new List<MightPowerSkill>
                    {
                        new MightPowerSkill("TM_Possess_pwr", "TM_Possess_pwr_desc"), //duration of possession, 
                        new MightPowerSkill("TM_Possess_eff", "TM_Possess_eff_desc"),
                        new MightPowerSkill("TM_Possess_ver", "TM_Possess_ver_desc") //applies mental states or effects , fewer debuffs during possession
                    };
                }
                return this.mightPowerSkill_Possess;
            }
        }

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

        public List<MightPower> AllMightPowers
        {
            get
            {
                List<MightPower> list = new List<MightPower>();
                list.Clear();
                list.AddRange(this.MightPowersM);
                list.AddRange(this.MightPowersDK);
                list.AddRange(this.MightPowersG);
                list.AddRange(this.MightPowersS);
                list.AddRange(this.MightPowersB);
                list.AddRange(this.MightPowersR);
                list.AddRange(this.MightPowersF);
                list.AddRange(this.MightPowersP);
                list.AddRange(this.MightPowersStandalone);
                return list;
            }
        }

        public MightPower ReturnMatchingMightPower(TMAbilityDef def)
        {
            for(int i = 0; i < AllMightPowers.Count; i++)
            {
                if(AllMightPowers[i].TMabilityDefs.Contains(def))
                {
                    return AllMightPowers[i];
                }
            }
            return null;
        }

        public IEnumerable<MightPower> Powers
        {
            get
            {                
                return this.MightPowersM.Concat(this.MightPowersDK.Concat(this.MightPowersG.Concat(this.MightPowersS.Concat(this.MightPowersB.Concat(this.mightPowerR.Concat(this.MightPowersF.Concat(this.mightPowerP.Concat(this.mightPowerStandalone))))))));
            }
        }

        public MightData()
        {
        }

        public MightData(CompAbilityUserMight newUser)
        {
            this.mightPawn = newUser.AbilityUser;
        }

        public void ClearData()
        {
            this.mightUserLevel = 0;
            this.mightUserXP = 0;
            this.mightPowerM.Clear();
            this.mightPowerB.Clear();
            this.mightPowerDK.Clear();
            this.mightPowerF.Clear();
            this.mightPowerG.Clear();
            this.mightPowerP.Clear();
            this.mightPowerR.Clear();
            this.mightPowerS.Clear();
            this.MightPowersStandalone.Clear();
            this.mightPawn = null;
            this.initialized = false;
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
            Scribe_Collections.Look<MightPower>(ref this.mightPowerStandalone, "mightPowerStandalone", (LookMode)2, new object[0]);
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
            Scribe_Collections.Look<MightPower>(ref this.mightPowerF, "mightPowerF", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_Disguise, "mightPowerSkill_Disguise", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_Mimic, "mightPowerSkill_Mimic", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_Reversal, "mightPowerSkill_Reversal", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_Transpose, "mightPowerSkill_Transpose", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_Possess, "mightPowerSkill_Possess", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPower>(ref this.mightPowerP, "mightPowerP", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_PsionicAugmentation, "mightPowerSkill_PsionicAugmentation", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_PsionicBarrier, "mightPowerSkill_PsionicBarrier", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_PsionicBlast, "mightPowerSkill_PsionicBlast", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_PsionicDash, "mightPowerSkill_PsionicDash", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_PsionicStorm, "mightPowerSkill_PsionicStorm", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPower>(ref this.mightPowerDK, "mightPowerDK", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_Shroud, "mightPowerSkill_Shroud", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_WaveOfFear, "mightPowerSkill_WaveOfFear", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_Spite, "mightPowerSkill_Spite", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_LifeSteal, "mightPowerSkill_LifeSteal", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_GraveBlade, "mightPowerSkill_GraveBlade", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPower>(ref this.mightPowerM, "mightPowerM", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_Chi, "mightPowerSkill_Chi", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_MindOverBody, "mightPowerSkill_MindOverBody", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_Meditate, "mightPowerSkill_Meditate", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_TigerStrike, "mightPowerSkill_TigerStrike", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_DragonStrike, "mightPowerSkill_DragonStrike", (LookMode)2, new object[0]);
            Scribe_Collections.Look<MightPowerSkill>(ref this.mightPowerSkill_ThunderStrike, "mightPowerSkill_ThunderStrike", (LookMode)2, new object[0]);
        }

    }
}