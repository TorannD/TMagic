using RimWorld;
using RimWorld.Planet;
using System;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace TorannMagic.Enchantment
{
    public class CompInfusion : ThingComp
    {
        private static readonly SoundDef InfusionSound = SoundDef.Named("Infusion_Infused");

        private bool isNew;

        private string enchantmentDefName;

        private EnchantmentDef enchantment;

        private string infusedLabel;

        private Color infusedLabelColor;

        public InfusionSet Infusions
        {
            get
            {
                return new InfusionSet(this.enchantment);
            }
        }

        public bool Infused
        {
            get
            {
                return this.enchantment != null;
            }
        }

        public string InfusedLabel
        {
            get
            {
                return this.infusedLabel;
            }
        }

        public Color InfusedLabelColor
        {
            get
            {
                return this.infusedLabelColor;
            }
        }

        public void InitializeInfusionEnchantment(InfusionTier tier)
        {
            this.InitializeInfusion(InfusionType.Enchantment, tier, out this.enchantment);
        }

        public void InitializeInfusion(InfusionType type, InfusionTier tier, out EnchantmentDef enchantment)
        {
            if (!GenCollection.TryRandomElement<EnchantmentDef>(from t in DefDatabase<EnchantmentDef>.AllDefs
                                                     where t.tier == tier && t.type == type && t.MatchItemType(this.parent.def)
                                                     select t, out enchantment))
            {
                Log.Warning(string.Concat(new object[]
                {
                    "Infused :: Couldn't find any ",
                    type,
                    "InfusionDef! Tier: ",
                    tier
                }));
            }
            this.isNew = true;
        }

        private void throwMote()
        {
            CompQuality compQuality = ThingCompUtility.TryGetComp<CompQuality>(this.parent);
            if (compQuality == null)
            {
                return;
            }
            string label = QualityUtility.GetLabel(compQuality.Quality);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(label + " ");
            if (this.parent.Stuff != null)
            {
                stringBuilder.Append(this.parent.Stuff.LabelAsStuff + " ");
            }
            stringBuilder.Append(this.parent.def.label);
            Messages.Message(Translator.Translate(ResourceBank.StringInfusionMessage, new object[]
            {
                stringBuilder
            }), new GlobalTargetInfo(this.parent), MessageTypeDefOf.SilentInput);
            SoundStarter.PlayOneShotOnCamera(CompInfusion.InfusionSound, null);
            MoteMaker.ThrowText(this.parent.Position.ToVector3Shifted(), this.parent.Map, ResourceBank.StringInfused, GenInfusionColor.Legendary, -1f);
            this.isNew = false;
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (this.Infused)
            {
                if (this.enchantment != null)
                {
                    this.infusedLabelColor = this.enchantment.tier.InfusionColor();
                }
                this.infusedLabel = string.Empty;
                if (this.enchantment != null)
                {
                    this.infusedLabel += this.enchantment.labelShort;
                }
                InfusionLabelManager.Register(this);
                if (this.isNew)
                {
                    this.throwMote();
                }
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            if (this.enchantment != null)
            {
                this.enchantmentDefName = this.enchantment.defName;
            }
            Scribe_Values.Look<string>(ref this.enchantmentDefName, "enchantment", null, false);
            if (this.enchantment == null)
            {
                this.enchantment = EnchantmentDef.Named(this.enchantmentDefName);
            }
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
            if (this.Infused)
            {
                InfusionLabelManager.DeRegister(this);
            }
        }

        public override bool AllowStackWith(Thing other)
        {
            return false;
        }

        public override string GetDescriptionPart()
        {
            return base.GetDescriptionPart() + "\n" + this.parent.GetInfusionDesc();
        }

        public override string TransformLabel(string label)
        {
            this.isNew = false;
            if (this.Infused)
            {
                return this.parent.GetInfusedLabel(true, true);
            }
            return base.TransformLabel(label);
        }
    }
}
