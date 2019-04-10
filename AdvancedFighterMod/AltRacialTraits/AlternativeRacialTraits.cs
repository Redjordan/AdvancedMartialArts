using AdvancedMartialArts.AltRacialTraits.Facts;
using AdvancedMartialArts.HelperClasses;
using Harmony12;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;

namespace AdvancedMartialArts.AltRacialTraits
{
    internal class AlternativeRacialTraits
    {
        private static LibraryScriptableObject library => Main.library;
        private static BlueprintFeature KeepNormalRacialTrait1;
        private static BlueprintFeature KeepNormalRacialTrait2;
        private static BlueprintFeature KeepNormalRacialTrait3;

        private static BlueprintItemWeapon bite;
        private static BlueprintItemWeapon claw;

        private static BlueprintFeature UndoSelectionFeature;
        private static FeatureGroup _featureGroup = FeatureGroup.Racial;

        public static void Load()
        {
            Main.SafeLoad(CreateTraits, "CreateTraits");
        }

        private static void CreateTraits()
        {

            KeepNormalRacialTrait1 = Helpers.CreateFeature(
                "0KeepNormalRacialTrait",
                "(Reset selection or keep current Racial Traits)",
                "Choose this to reset the current selection or skip choosing an alternate racial trait.",
                Helpers.getGuid("0KeepNormalRacialTrait1"),
                null,
                FeatureGroup.None);
            KeepNormalRacialTrait1.HideInCharacterSheetAndLevelUp = true;
            KeepNormalRacialTrait1.HideInUI = true;

            KeepNormalRacialTrait2 = Helpers.CreateFeature(
                "0KeepNormalRacialTrait",
                "(Reset selection or keep current Racial Traits)",
                "Choose this to reset the current selection or skip choosing an alternate racial trait.",
                Helpers.getGuid("0KeepNormalRacialTrait2"),
                null,
                FeatureGroup.None);
            KeepNormalRacialTrait2.HideInCharacterSheetAndLevelUp = true;
            KeepNormalRacialTrait2.HideInUI = true;

            KeepNormalRacialTrait3 = Helpers.CreateFeature(
                "0KeepNormalRacialTrait",
                "(Reset selection or keep current Racial Traits)",
                "Choose this to reset the current selection or skip choosing an alternate racial trait.",
                Helpers.getGuid("0KeepNormalRacialTrait3"),
                null,
                FeatureGroup.None);
            KeepNormalRacialTrait3.HideInCharacterSheetAndLevelUp = true;
            KeepNormalRacialTrait3.HideInUI = true;


            string bite1d6Id = "a000716f88c969c499a535dadcf09286";
            bite = library.CopyAndAdd<BlueprintItemWeapon>(bite1d6Id, "Bite",
                Helpers.getGuid("TieflingBite"));

            string claw1d4Id = "118fdd03e569a66459ab01a20af6811a";
            claw = library.CopyAndAdd<BlueprintItemWeapon>(claw1d4Id, "Claw",
                Helpers.getGuid("TieflingClaw"));

            addDwarfAlternativeRacialTraitList();
            addElfAlternativeRacialTraitList();
            addHalfOrcAlternativeRacialTraitList();
            addAasimarAlternativeRacialTraitList();
            addTieflingAlternativeRacialTraitList();
            addGoblinAlternativeRacialTraitList();
            addHalfElfAlternativeRacialTraitList();
            addGnomeAlternativeRacialTraitList();
            addHalflingAlternativeRacialTraitList();
        }

        private static void addDwarfAlternativeRacialTraitList()
        {
            BlueprintRace dwarf = library.Get<BlueprintRace>("c4faf439f0e70bd40b5e36ee80d06be7");
            List<BlueprintFeature> features = new List<BlueprintFeature>();
            BlueprintFeature SlowAndSteady = Main.library.Get<BlueprintFeature>("786588ad1694e61498e77321d4b07157");
            BlueprintFeature Stability = Main.library.Get<BlueprintFeature>("2f254c6068d58b643b8de2fc7ec32dbb");
            BlueprintFeature Hardy = Main.library.Get<BlueprintFeature>("f75d3b6110f04d1409564b9d7647db60");
            BlueprintFeature DwarfDefensiveTrainingGiants = Main.library.Get<BlueprintFeature>("f268a00e42618144e86c9db76af7f3e9");
            BlueprintFeature HatredGoblinoidOrc = Main.library.Get<BlueprintFeature>("6cde66a7da5a2024c906d887db735223");
            BlueprintFeature KeenSenses = Main.library.Get<BlueprintFeature>("9c747d24f6321f744aa1bb4bd343880d");
            BlueprintFeature DwarvenWeaponFamiliarity = Main.library.Get<BlueprintFeature>("a1619e8d27fe97c40ba443f6f8ab1763");


            _featureGroup = FeatureGroup.Trait;
            features.Add(Helpers.CreateFeature("IronCitizen",
                "Iron Citizen",
                "Dwarves with this racial trait gain a +2 bonus on Persuasion and Perception checks, and Persuasion is a class skill for such dwarves. This replaces stability.",
                Helpers.getGuid("IronCitizen"),
                null,
                _featureGroup,
                    dwarf.PrerequisiteFeature(),
                Stability.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = Stability),
                StatType.SkillPersuasion.CreateAddStatBonus(2, ModifierDescriptor.Racial),
                StatType.SkillPerception.CreateAddStatBonus(2, ModifierDescriptor.Racial),
                Helpers.Create<AddClassSkill>(x => x.Skill = StatType.SkillPersuasion)
            ));


            features.Add(Helpers.CreateFeature("Lorekeeper",
                "Lorekeeper",
                "Dwarves keep extensive records about their history and the world around them. Dwarves with this racial trait receive a +2 racial bonus on Knowledge (World) checks. This racial trait replaces greed.",
                Helpers.getGuid("Lorekeeper"),
                null,
                _featureGroup,
                dwarf.PrerequisiteFeature(),
                StatType.SkillKnowledgeWorld.CreateAddStatBonus(2, ModifierDescriptor.Racial)
            ));

            BlueprintFeature endurance = library.Get<BlueprintFeature>("54ee847996c25cd4ba8773d7b8555174");
            features.Add(Helpers.CreateFeature("Wanderer",
                "Wanderer",
                "You gain Endurance as a bonus feat, and Athletics is a class skill for them. This racial trait replaces hardy.",
                Helpers.getGuid("Wanderer"),
                null,
                _featureGroup,
                dwarf.PrerequisiteFeature(),
                Hardy.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = Hardy),
                Helpers.CreateAddFacts(endurance),
                Helpers.Create<AddClassSkill>(x => x.Skill = StatType.SkillAthletics)
            ));


            BlueprintBuff SpellResistanceBuff = library.Get<BlueprintBuff>("50a77710a7c4914499d0254e76a808e5");
            BlueprintActivatableAbility SpellResistanceAbility = library.Get<BlueprintActivatableAbility>("4be5757b85af47545a5789f1d03abda9");

            ContextRankConfig MagicResit = Helpers.Create<ContextRankConfig>();
            Traverse traverse = Traverse.Create(MagicResit);
            traverse.Field("m_BaseValueType").SetValue(ContextRankBaseValueType.CharacterLevel);
            traverse.Field("m_BaseValueType").SetValue(ContextRankBaseValueType.CharacterLevel);

            BlueprintBuff buff = Helpers.CreateBuff(
                "MagicResistantBuff",
                "Magic Resistant",
                "Activated Dwarven magic resistance",
                Helpers.getGuid("MagicResistantBuff"),
                SpellResistanceBuff.Icon,
                SpellResistanceBuff.FxOnStart,
                Helpers.Create<AddSpellResistanceBuff>()
            );

            BlueprintActivatableAbility ability = Helpers.CreateActivatableAbility(
                "MagicResistantAbility",
                "Magic Resistant",
                "Activate Dwarven magic resistance",
                Helpers.getGuid("MagicResistantAbility"),
                SpellResistanceBuff.Icon,
                buff,
                AbilityActivationType.WithUnitCommand,
                UnitCommand.CommandType.Standard,
                SpellResistanceAbility.ActivateWithUnitAnimation
            );

            features.Add(Helpers.CreateFeature("MagicResistant",
                "Magic Resistant",
                "Some of the older dwarven clans are particularly resistant to magic. Dwarves with this racial trait gain spell resistance equal to 5 + their character level. This resistance can be lowered for 1 round as a standard action. Dwarves with this racial trait take a –2 penalty on all concentration checks made in relation to arcane spells. This racial trait replaces hardy.",
                Helpers.getGuid("MagicResistant"),
                null,
                _featureGroup,
                dwarf.PrerequisiteFeature(),
                Hardy.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = Hardy),
                Helpers.CreateAddFacts(ability),
                Helpers.Create<ConcentrationBonus>(x => x.Value = -2)
            ));

            features.Add(Helpers.CreateFeature("SlagChild",
                "Slag Child",
                "Dwarves from dishonored families must append “-slag,” “-slagsun,” or “-slagdam” to their surnames to indicate their shameful status. These dwarves are commonly banished or ostracized; they are forced to eke out a living at the fringes of dwarven settlements or in bleak wilderness areas. They gain a +2 racial bonus on Stealth and Lore Nature checks. This racial trait replaces defensive training and hatred.",
                Helpers.getGuid("Slag Child"),
                null,
                _featureGroup,
                dwarf.PrerequisiteFeature(),
                DwarfDefensiveTrainingGiants.PrerequisiteFeature(),
                HatredGoblinoidOrc.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = DwarfDefensiveTrainingGiants),
                Helpers.Create<RemoveFeature>(r => r.Feature = HatredGoblinoidOrc),
                StatType.SkillStealth.CreateAddStatBonus(2, ModifierDescriptor.Racial),
                StatType.SkillLoreNature.CreateAddStatBonus(2, ModifierDescriptor.Racial)
            ));

            BlueprintFeature toughness = library.Get<BlueprintFeature>("d09b20029e9abfe4480b356c92095623");
            features.Add(Helpers.CreateFeature("Unstoppable",
                "Unstoppable",
                "Some dwarves train from a young age to outlast orcs on the battlefield. They gain Toughness as a bonus feat and a +1 racial bonus on Fortitude saves. This racial trait replaces hardy.",
                Helpers.getGuid("Unstoppable"),
                null,
                _featureGroup,
                dwarf.PrerequisiteFeature(),
                Hardy.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = Hardy),
                Helpers.CreateAddFacts(toughness),
                StatType.SaveFortitude.CreateAddStatBonus(1, ModifierDescriptor.Racial)
            ));

            addAlternativeRacialTraitsSelection(dwarf, features);
        }

        private static void addElfAlternativeRacialTraitList()
        {
            BlueprintRace elf = library.Get<BlueprintRace>("25a5878d125338244896ebd3238226c8");
            List<BlueprintFeature> features = new List<BlueprintFeature>();
            BlueprintFeature KeenSenses = Main.library.Get<BlueprintFeature>("9c747d24f6321f744aa1bb4bd343880d");
            BlueprintFeature ElvenMagic = Main.library.Get<BlueprintFeature>("55edf82380a1c8540af6c6037d34f322");
            BlueprintFeature ElvenImmunities = Main.library.Get<BlueprintFeature>("2483a523984f44944a7cf157b21bf79c");
            BlueprintFeature ElvenWeaponFamiliarity = Main.library.Get<BlueprintFeature>("03fd1e043fc678a4baf73fe67c3780ce");


            features.Add(Helpers.CreateFeature("AquaticMastery",
                "Aquatic Mastery",
                "Some aquatic elves are able to wield the power of the sea with great prowess. Elves with this racial trait increase the DC of any spell with the cold descriptor they cast by 1. This replaces weapon familiarity.",
                Helpers.getGuid("AquaticMastery"),
                null,
                _featureGroup,
                elf.PrerequisiteFeature(),
                ElvenWeaponFamiliarity.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = ElvenWeaponFamiliarity),
                Helpers.Create<IncreaseSpellDescriptorCasterLevel>(x =>
                {
                    x.BonusCasterLevel = 1;
                    x.Descriptor = new SpellDescriptorWrapper(SpellDescriptor.Cold);
                })
            ));

            features.Add(Helpers.CreateFeature("ArcaneFocus",
                "Arcane Focus",
                "Some elven families have such long traditions of producing wizards (and other arcane spellcasters) that they raise their children with the assumption each is destined to be a powerful magic-user, with little need for mundane concerns such as skill with weapons. Elves with this racial trait gain a +2 racial bonus on concentration checks made to cast arcane spells defensively. This racial trait replaces weapon familiarity.",
                Helpers.getGuid("ArcaneFocus"),
                null,
                _featureGroup,
                    elf.PrerequisiteFeature(),
                ElvenWeaponFamiliarity.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = ElvenWeaponFamiliarity),
                Helpers.Create<ConcentrationBonus>(x => x.Value = 2)
             ));

            List<BlueprintProgression> draconicProgressions = new List<BlueprintProgression>
            {
                library.Get<BlueprintProgression>("7bd143ead2d6c3a409aad6ee22effe34"), //BloodlineDraconicBlackProgression
                library.Get<BlueprintProgression>("8a7f100c02d0b254d8f5f3affc8ef386"), //BloodlineDraconicBlueProgression
                library.Get<BlueprintProgression>("5f9ecbee67db8364985e9d0500eb25f1"), //BloodlineDraconicBrassProgression
                library.Get<BlueprintProgression>("7e0f57d8d00464441974e303b84238ac"), //BloodlineDraconicBronzeProgression
                library.Get<BlueprintProgression>("b522759a265897b4f8f7a1a180a692e4"), //BloodlineDraconicCopperProgression
                library.Get<BlueprintProgression>("6c67ef823db8d7d45bb0ef82f959743d"), //BloodlineDraconicGoldProgression
                library.Get<BlueprintProgression>("7181be57d1cc3bc40bc4b552e4e4ce24"), //BloodlineDraconicGreenProgression
                library.Get<BlueprintProgression>("8c6e5b3cf12f71e43949f52c41ae70a8"), //BloodlineDraconicRedProgression
                library.Get<BlueprintProgression>("c7d2f393e6574874bb3fc728a69cc73a"), //BloodlineDraconicSilverProgression
                library.Get<BlueprintProgression>("b0f79497a0d1f4f4b8293e82c8f8fa0c") //BloodlineDraconicWhiteProgression
            };
            List<BlueprintProgression> allDraconicProgressions = new List<BlueprintProgression>(draconicProgressions);

            foreach(BlueprintProgression blueprintProgression in draconicProgressions)
            {
                BlueprintProgression progression = library.TryGet<BlueprintProgression>(Helpers.MergeIds(blueprintProgression.AssetGuid, "933224357f8d48be837a3083e33a18a8"));
                if(progression != null)
                {
                    allDraconicProgressions.Add(progression);
                }
            }

            features.Add(Helpers.CreateFeature("DragonMagic",
                "Dragon Magic",
                "Some elves have potent draconic blood. Elves with this racial trait who take a draconic bloodline as a class feature have their Charisma scores increased by 2 points.",
                Helpers.getGuid("DragonMagic"),
                null,
                _featureGroup,
                elf.PrerequisiteFeature(),
                Helpers.Create<AddStatBonusForFeatures>(x =>
                {
                    x.StatType = StatType.Charisma;
                    x.Descriptor = ModifierDescriptor.Racial;
                    x.Value = 2;
                    x.features = allDraconicProgressions.ToArray();
                }),
                Helpers.Create<RecalculateOnFactsChange>(x => x.CheckedFacts = allDraconicProgressions.ToArray())
            ));

            features.Add(Helpers.CreateFeature("DraconicConsular",
                "Draconic Consular",
                "Elves serving in the company of dragons are trained to assist and represent their draconic allies. Elves with this racial trait gain a +1 bonus on Persuasion and Knowledge (arcana) checks. This racial trait replaces keen senses.",
                Helpers.getGuid("DraconicConsular"),
                null,
                _featureGroup,
                elf.PrerequisiteFeature(),
                KeenSenses.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = KeenSenses),
                StatType.SkillPersuasion.CreateAddStatBonus(1, ModifierDescriptor.Racial),
                StatType.SkillKnowledgeArcana.CreateAddStatBonus(1, ModifierDescriptor.Racial)
            ));

            BlueprintParametrizedFeature SpellFocus = library.Get<BlueprintParametrizedFeature>("16fa59cc9a72a6043b566b49184f53fe");
            SpellSchool[] IllustriousUrbaniteSchools = new SpellSchool[]
            {
                SpellSchool.Conjuration,
                SpellSchool.Illusion,
                SpellSchool.Transmutation
            };

            foreach(SpellSchool spellSchool in IllustriousUrbaniteSchools)
            {
                features.Add(Helpers.CreateFeature("IllustriousUrbanite" + spellSchool,
                    "Illustrious Urbanite (" + spellSchool + ")",
                    "City elves have a remarkable ability to combine magic harmoniously with their surroundings. They gain Spell Focus with conjuration, illusion, or transmutation spells as a bonus feat. This racial trait replaces keen senses.",
                    Helpers.getGuid("IllustriousUrbanite" + spellSchool),
                    SpellFocus.Icon,
                    _featureGroup,
                    elf.PrerequisiteFeature(),
                    KeenSenses.PrerequisiteFeature(),
                    Helpers.Create<RemoveFeature>(r => r.Feature = KeenSenses),
                    Helpers.Create<AddFeatureParametrizedCustom>(x =>
                    {
                        x.parametrizedFeature = SpellFocus;
                        x.SpellSchool = spellSchool;
                    })
                ));
            }


            SpellSchool[] OverwhelmingMagicSchools = new SpellSchool[]
            {
                SpellSchool.Necromancy,
                SpellSchool.Evocation,
                SpellSchool.Enchantment,
                SpellSchool.Divination,
                SpellSchool.Abjuration,
            };

            foreach(SpellSchool spellSchool in OverwhelmingMagicSchools)
            {
                features.Add(Helpers.CreateFeature("OverwhelmingMagic" + spellSchool,
                    "Overwhelming Magic (" + spellSchool + ")",
                    "Some elves obsess over the fundamentals of magic, training for decades to add layers of potent spellwork before they ever begin practicing true spells. This builds a foundation that makes their magic increasingly difficult to resist. These elves gain Spell Focus as a bonus feat. This racial trait replaces weapon familiarity.",
                    Helpers.getGuid("OverwhelmingMagic" + spellSchool),
                    SpellFocus.Icon,
                    _featureGroup,
                    elf.PrerequisiteFeature(),
                    KeenSenses.PrerequisiteFeature(),
                    Helpers.Create<RemoveFeature>(r => r.Feature = KeenSenses),
                    Helpers.Create<AddFeatureParametrizedCustom>(x =>
                    {
                        x.parametrizedFeature = SpellFocus;
                        x.SpellSchool = spellSchool;
                    })
                ));
            }


            features.Add(Helpers.CreateFeature("LongLimbed",
                "Long-Limbed",
                "Elves with this racial trait have a base move speed of 35 feet. This racial trait replaces weapon familiarity.",
                Helpers.getGuid("LongLimbed"),
                null,
                _featureGroup,
                elf.PrerequisiteFeature(),
                ElvenWeaponFamiliarity.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = ElvenWeaponFamiliarity),
                Helpers.Create<BuffMovementSpeed>(x => x.Value = 5)
            ));

            features.Add(Helpers.CreateFeature("Loremasters",
                "Loremasters",
                "Some elves are steeped in lore older than most civilizations. They gain a +2 racial bonus on Knowledge (World), and Knowledge (Arcana) checks. This racial trait replaces elven magic and keen senses.",
                Helpers.getGuid("Loremasters"),
                null,
                _featureGroup,
                elf.PrerequisiteFeature(),
                KeenSenses.PrerequisiteFeature(),
                ElvenMagic.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = KeenSenses),
                Helpers.Create<RemoveFeature>(r => r.Feature = ElvenMagic),
                StatType.SkillKnowledgeWorld.CreateAddStatBonus(2, ModifierDescriptor.Racial),
                StatType.SkillKnowledgeArcana.CreateAddStatBonus(2, ModifierDescriptor.Racial)
            ));

            features.Add(Helpers.CreateFeature("Moonkissed",
                "Moonkissed",
                "Elves with this alternate racial trait gain a +1 racial bonus on saving throws. This replaces elven immunities and keen senses.",
                Helpers.getGuid("Moonkissed"),
                null,
                _featureGroup,
                elf.PrerequisiteFeature(),
                KeenSenses.PrerequisiteFeature(),
                ElvenImmunities.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = KeenSenses),
                Helpers.Create<RemoveFeature>(r => r.Feature = ElvenImmunities),
                StatType.SaveFortitude.CreateAddStatBonus(1, ModifierDescriptor.Racial),
                StatType.SaveReflex.CreateAddStatBonus(1, ModifierDescriptor.Racial),
                StatType.SaveWill.CreateAddStatBonus(1, ModifierDescriptor.Racial)
            ));


            features.Add(Helpers.CreateFeature("Woodcraft",
                 "Woodcraft",
                 "Elves know the deep secrets of the wild like no others, especially secrets of the forests. Elves with this racial trait gain a +2 racial bonus on Lore (nature). This racial trait replaces elven magic.",
                 Helpers.getGuid("Woodcraft"),
                 null,
                 _featureGroup,
                 elf.PrerequisiteFeature(),
                 ElvenMagic.PrerequisiteFeature(),
                 Helpers.Create<RemoveFeature>(r => r.Feature = ElvenMagic),
                 StatType.SkillLoreNature.CreateAddStatBonus(2, ModifierDescriptor.Racial)
             ));

            addAlternativeRacialTraitsSelection(elf, features);
        }

        private static void addHalfElfAlternativeRacialTraitList()
        {
            BlueprintRace halfElf = library.Get<BlueprintRace>("b3646842ffbd01643ab4dac7479b20b0");
            List<BlueprintFeature> features = new List<BlueprintFeature>();
            BlueprintFeature KeenSenses = Main.library.Get<BlueprintFeature>("9c747d24f6321f744aa1bb4bd343880d");
            BlueprintFeature ElvenImmunities = Main.library.Get<BlueprintFeature>("2483a523984f44944a7cf157b21bf79c");
            BlueprintFeatureSelection Adaptability = Main.library.Get<BlueprintFeatureSelection>("26a668c5a8c22354bac67bcd42e09a3f");

            BlueprintFeatureSelection alternativeRacialTraitsSelection2 = Helpers.CreateFeatureSelection(
                "AlternativeRacialTraitsSelection2" + halfElf.Name,
                "Alternative Racial Traits",
                "Select alternative racial traits",
                Helpers.getGuid("AlternativeRacialTraitsSelection2" + halfElf.Name),
                null, FeatureGroup.ChannelEnergy);

            BlueprintFeatureSelection alternativeRacialTraitsSelection3 = Helpers.CreateFeatureSelection(
                "AlternativeRacialTraitsSelection3" + halfElf.Name,
                "Alternative Racial Traits",
                "Select alternative racial traits",
                Helpers.getGuid("AlternativeRacialTraitsSelection3" + halfElf.Name),
                null, FeatureGroup.ChannelEnergy);

            BlueprintFeatureSelection alternativeRacialTraitsSelection = Helpers.CreateFeatureSelection(
                "AlternativeRacialTraitsSelection" + halfElf.Name,
                "Alternative Racial Traits",
                "Select alternative racial traits",
                Helpers.getGuid("AlternativeRacialTraitsSelection" + halfElf.Name),
                null, FeatureGroup.ChannelEnergy);

            List<BlueprintFeature> AncestralArmsFeatures = new List<BlueprintFeature>();
            WeaponSubCategory[] values = (WeaponSubCategory[])Enum.GetValues(typeof(WeaponCategory));
            foreach(WeaponCategory weaponCategory in values)
            {
                if(weaponCategory.HasSubCategory(WeaponSubCategory.Exotic) || weaponCategory.HasSubCategory(WeaponSubCategory.Martial))
                {
                    AncestralArmsFeatures.Add(Helpers.CreateFeature("AncestralArms" + weaponCategory,
                        "Ancestral Arms (" + weaponCategory + ")",
                        "Weapon Proficiencies in " + weaponCategory,
                        Helpers.getGuid("AncestralArms" + weaponCategory),
                        null,
                        _featureGroup,
                        Helpers.Create<AddProficiencies>(x => x.WeaponProficiencies = new WeaponCategory[] { weaponCategory })
                    ));
                }
            }

            BlueprintFeatureSelection AncestralArmsSelection = Helpers.CreateFeatureSelection("AncestralArmsSelection",
                "Ancestral Arms",
                "Some half-elves receive training in an unusual weapon. Half-elves with this racial trait receive Exotic Weapon Proficiency or Martial Weapon Proficiency with one weapon as a bonus feat at 1st level. This racial trait replaces the adaptability racial trait.",
                Helpers.getGuid("AncestralArmsSelection"),
                null,
                FeatureGroup.ChannelEnergy,
                halfElf.PrerequisiteFeature()
            );
            AncestralArmsSelection.HideInUI = true;
            AncestralArmsSelection.HideInCharacterSheetAndLevelUp = true;
            AncestralArmsSelection.AllFeatures = AncestralArmsFeatures.ToArray();

            List<BlueprintFeature> AdaptabilityReplacingFeatures = new List<BlueprintFeature>();
            AdaptabilityReplacingFeatures.Add(Helpers.CreateFeature("AncestralArms",
                "Ancestral Arms",
                "Some half-elves receive training in an unusual weapon. Half-elves with this racial trait receive Exotic Weapon Proficiency or Martial Weapon Proficiency with one weapon as a bonus feat at 1st level. This racial trait replaces the adaptability racial trait.",
                Helpers.getGuid("AncestralArms"),
                null,
                _featureGroup,
                halfElf.PrerequisiteFeature(),
                Adaptability.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = Adaptability)
            ));


            AdaptabilityReplacingFeatures.Add(Helpers.CreateFeature("DualMinded",
                "Dual Minded",
                "The mixed ancestry of some half-elves makes them resistant to mental attacks. Half-elves with this racial trait gain a +2 bonus on all Will saving throws. This racial trait replaces the adaptability racial trait.",
                Helpers.getGuid("DualMinded"),
                null,
                _featureGroup,
                halfElf.PrerequisiteFeature(),
                Adaptability.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = Adaptability),
                StatType.SaveWill.CreateAddStatBonus(2, ModifierDescriptor.Racial)
            ));

            AdaptabilityReplacingFeatures.Add(Helpers.CreateFeature("KindredRaised",
                "Kindred-Raised",
                "While most think of people with one human and one elven parent when they think of half-elves, some half-elves are raised by two half-elven parents. Such half-elves feel less like outsiders, making them more confident, but less adaptable without the exposure to a human parent. They gain a +2 bonus to Charisma and one other ability score of their choice. This racial trait replaces the half-elf’s usual racial ability score modifiers, as well as adaptability, elven immunities and keen senses.",
                Helpers.getGuid("KindredRaised"),
                null,
                _featureGroup,
                halfElf.PrerequisiteFeature(),
                KeenSenses.PrerequisiteFeature(),
                Adaptability.PrerequisiteFeature(),
                ElvenImmunities.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = KeenSenses),
                Helpers.Create<RemoveFeature>(r => r.Feature = Adaptability),
                Helpers.Create<RemoveFeature>(r => r.Feature = ElvenImmunities),
                StatType.Charisma.CreateAddStatBonus(2, ModifierDescriptor.Racial)
            ));

            features.Add(Helpers.CreateFeature("Multidisciplined",
                "Multidisciplined",
                "Born to two races, half-elves have a knack for combining different magical traditions. If a half-elf with this racial trait has spellcasting abilities from at least two different classes, the effects of spells she casts from all her classes are calculated as though her caster level were 1 level higher, to a maximum of her character level. This racial trait replaces multitalented.",
                Helpers.getGuid("Multidisciplined"),
                null,
                _featureGroup,
                halfElf.PrerequisiteFeature(),
                Helpers.Create<IncreaseCasterLevelUpToCharacterLevel>(x => x.MaxBonus = 1)
            ));

            features.Add(Helpers.CreateFeature("Sophisticate",
                "Sophisticate",
                "Half-elves who strive to embody the culture in which they live develop a keen instinct for the ebb and flow of fashions, fads, and political trends. They gain a +2 racial bonus on Knowledge (World) checks. This racial trait replaces elven immunities.",
                Helpers.getGuid("Sophisticate"),
                null,
                _featureGroup,
                halfElf.PrerequisiteFeature(),
                ElvenImmunities.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = ElvenImmunities),
                StatType.SkillKnowledgeWorld.CreateAddStatBonus(2, ModifierDescriptor.Racial)
            ));

            features.Add(Helpers.CreateFeature("Wary",
                "Wary",
                "Many half-elves have spent their long lives moving from place to place, often driven out by the hostility of others. Such experiences have made them wary of others’ motivations. Half-elves with this trait gain a +1 racial bonus on Perception and Persuasion checks. This racial trait replaces the keen senses racial trait.",
                Helpers.getGuid("Wary"),
                null,
                _featureGroup,
                halfElf.PrerequisiteFeature(),
                KeenSenses.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = KeenSenses),
                StatType.SkillPerception.CreateAddStatBonus(1, ModifierDescriptor.Racial),
                StatType.SkillPersuasion.CreateAddStatBonus(1, ModifierDescriptor.Racial)
            ));

            BlueprintFeature ElvenWeaponFamiliarity = Main.library.Get<BlueprintFeature>("03fd1e043fc678a4baf73fe67c3780ce");
            AdaptabilityReplacingFeatures.Add(Helpers.CreateFeature("WeaponFamiliarityHalfElf",
                "Weapon Familiarity",
                "Half-elves raised among elves often feel pitied and mistrusted by their longer-lived kin, and yet they receive training in elf weapons. They gain the elf ‘s weapon familiarity trait. This racial trait replaces adaptability.",
                Helpers.getGuid("WeaponFamiliarityHalfElf"),
                null,
                _featureGroup,
                halfElf.PrerequisiteFeature(),
                Adaptability.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = Adaptability),
                Helpers.CreateAddFacts(ElvenWeaponFamiliarity)
            ));

            foreach(BlueprintFeature blueprintFeature in AdaptabilityReplacingFeatures)
            {

                SelectFeature_Apply_Patch.onApplyFeature.Add(blueprintFeature, (state, unit) =>
                {
                    if(blueprintFeature.Name == "AncestralArms")
                    {
                        AncestralArmsSelection.AddSelection(state, unit, 1);
                    }
                    FeatureSelectionState AdaptabilitySelectionState = state.FindSelection(Adaptability, true);
                    state.Selections.Remove(AdaptabilitySelectionState);

                    FeatureSelectionState selection1 = state.FindSelection(alternativeRacialTraitsSelection, true);
                    FeatureSelectionState selection2 = state.FindSelection(alternativeRacialTraitsSelection2, true);
                    FeatureSelectionState selection3 = state.FindSelection(alternativeRacialTraitsSelection3, true);
                    if(selection1 != null && selection2 == null && selection1.SelectedItem != null && (BlueprintFeature)selection1.SelectedItem != KeepNormalRacialTrait1)
                    {
                        alternativeRacialTraitsSelection2.AddSelection(state, unit, 1);
                    }

                    selection2 = state.FindSelection(alternativeRacialTraitsSelection2, true);
                    if(selection2 != null && selection3 == null && selection2.SelectedItem != null && (BlueprintFeature)selection2.SelectedItem != KeepNormalRacialTrait2)
                    {
                        alternativeRacialTraitsSelection3.AddSelection(state, unit, 1);
                    }
                });
            }

            foreach(BlueprintFeature blueprintFeature in features)
            {
                SelectFeature_Apply_Patch.onApplyFeature.Add(blueprintFeature, (state, unit) =>
                {
                    FeatureSelectionState selection1 = state.FindSelection(alternativeRacialTraitsSelection, true);
                    FeatureSelectionState selection2 = state.FindSelection(alternativeRacialTraitsSelection2, true);
                    FeatureSelectionState selection3 = state.FindSelection(alternativeRacialTraitsSelection3, true);
                    if(selection1 != null && selection2 == null && selection1.SelectedItem != null && (BlueprintFeature)selection1.SelectedItem != KeepNormalRacialTrait1)
                    {
                        alternativeRacialTraitsSelection2.AddSelection(state, unit, 1);
                    }

                    selection2 = state.FindSelection(alternativeRacialTraitsSelection2, true);
                    if(selection2 != null && selection3 == null && selection2.SelectedItem != null && (BlueprintFeature)selection2.SelectedItem != KeepNormalRacialTrait2)
                    {
                        alternativeRacialTraitsSelection3.AddSelection(state, unit, 1);
                    }
                });
            }
            features.AddRange(AdaptabilityReplacingFeatures);

            alternativeRacialTraitsSelection.AllFeatures = features.ToArray().AddToArray(KeepNormalRacialTrait1);
            alternativeRacialTraitsSelection2.AllFeatures = features.ToArray().AddToArray(KeepNormalRacialTrait2);
            alternativeRacialTraitsSelection3.AllFeatures = features.ToArray().AddToArray(KeepNormalRacialTrait3);

            ApplyClassMechanics_Apply_Patch.onChargenApply.Add((state, unit) =>
            {
                if(unit.Progression.Race == halfElf)
                {
                    alternativeRacialTraitsSelection.AddSelection(state, unit, 1);
                }
            });
        }

        private static void addGnomeAlternativeRacialTraitList()
        {
            BlueprintRace gnome = library.Get<BlueprintRace>("ef35a22c9a27da345a4528f0d5889157");
            List<BlueprintFeature> features = new List<BlueprintFeature>();
            BlueprintFeature KeenSenses = library.Get<BlueprintFeature>("9c747d24f6321f744aa1bb4bd343880d");
            BlueprintFeature HatredReptilian = library.Get<BlueprintFeature>("020bd2ae61de90341b7f874b788c160a");
            BlueprintFeature DwarfDefensiveTrainingGiants = library.Get<BlueprintFeature>("f268a00e42618144e86c9db76af7f3e9");
            BlueprintFeature IllusionResistance = library.Get<BlueprintFeature>("d030df93ea56d2b468650c4acf608f00");
            BlueprintFeature GnomeMagic = library.Get<BlueprintFeature>("deaf46650a9d2dd4f85736ca552f9fb1");
            BlueprintFeature Obsessive = library.Get<BlueprintFeature>("428899c30699b9c44a6c5ee4f74b5f57");
            BlueprintFeature SlowSpeedGnome = library.Get<BlueprintFeature>("09bc9ccb8ee0ffe4b8827066b1ed7e11");

            StatType[] statTypes = new StatType[]
            {
                StatType.SkillKnowledgeArcana,
                StatType.SkillKnowledgeWorld,
                StatType.SkillLoreNature,
                StatType.SkillLoreReligion

            };
            foreach(StatType statType in statTypes)
            {
                features.Add(Helpers.CreateFeature("Academician" + statType,
                    "Academician (" + statType + ")",
                    "While many view aasimars’ beauty and celestial powers as a gift, in some communities an aasimar might be persecuted for being different and fall into darkness. The forces of evil delight in such a perversion of their celestial counterparts’ gifts. The aasimar gains the maw or claw tiefling alternate racial trait. This racial trait replaces the spell-like ability racial trait.",
                    Helpers.getGuid("Academician" + statType),
                    null,
                    _featureGroup,
                    gnome.PrerequisiteFeature(),
                    Obsessive.PrerequisiteFeature(),
                    Helpers.Create<RemoveFeature>(x => x.Feature = Obsessive),
                    statType.CreateAddStatBonus(2, ModifierDescriptor.Racial)
                ));
            }
            features.Add(Helpers.CreateFeature("Pyromaniac",
                "Pyromaniac",
                "Gnomes with this racial trait are treated as one level higher when casting spells with the fire descriptor. This racial trait replaces gnome magic.",
                Helpers.getGuid("Pyromaniac"),
                null,
                _featureGroup,
                gnome.PrerequisiteFeature(),
                GnomeMagic.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(x => x.Feature = GnomeMagic),
                Helpers.Create<IncreaseSpellDescriptorCasterLevel>(x =>
                {
                    x.BonusCasterLevel = 1;
                    x.Descriptor = new SpellDescriptorWrapper(SpellDescriptor.Fire);
                })
            ));

            features.Add(Helpers.CreateFeature("UtilitarianMagic",
                "Utilitarian Magic",
                "Some gnomes develop practical magic to assist them with their obsessive projects. These gnomes add 1 to the DC of any saving throws against transmutation spells they cast. This racial trait replaces gnome magic.",
                Helpers.getGuid("UtilitarianMagic"),
                null,
                _featureGroup,
                gnome.PrerequisiteFeature(),
                GnomeMagic.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(x => x.Feature = GnomeMagic),
                Helpers.Create<IncreaseSpellSchoolDC>(x =>
                {
                    x.BonusDC = 1;
                    x.School = SpellSchool.Transmutation;
                })
            ));

            features.Add(Helpers.CreateFeature("FellMagic",
                "Fell Magic",
                "Gnomes add +1 to the DC of any saving throws against necromancy spells that they cast. This racial trait replaces gnome magic.",
                Helpers.getGuid("FellMagic"),
                null,
                _featureGroup,
                gnome.PrerequisiteFeature(),
                GnomeMagic.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(x => x.Feature = GnomeMagic),
                Helpers.Create<IncreaseSpellSchoolDC>(x =>
                {
                    x.BonusDC = 1;
                    x.School = SpellSchool.Transmutation;
                })
            ));


            addAlternativeRacialTraitsSelection(gnome, features);
        }

        private static void addHalflingAlternativeRacialTraitList()
        {
            BlueprintRace halfling = library.Get<BlueprintRace>("b0c3ef2729c498f47970bb50fa1acd30");
            List<BlueprintFeature> features = new List<BlueprintFeature>();
            BlueprintFeature Fearless = library.Get<BlueprintFeature>("39144c817b70467499cc32e3cff59d81");
            BlueprintFeature SlowSpeedGnome = library.Get<BlueprintFeature>("09bc9ccb8ee0ffe4b8827066b1ed7e11");
            BlueprintFeature SureFooted = library.Get<BlueprintFeature>("0fe5db70b50cd894c849fc764c80bbb9");
            BlueprintFeature KeenSenses = library.Get<BlueprintFeature>("9c747d24f6321f744aa1bb4bd343880d");
            BlueprintFeature HalflingLuck = library.Get<BlueprintFeature>("84ffa66048d26b14c800a425199f9886");

            StatType[] statTypes = new StatType[]
            {
                StatType.Strength,
                StatType.Dexterity,
                StatType.Constitution
            };
            foreach(StatType statType in statTypes)
            {
                BlueprintFeature addStatBonusToAnimalCompanion = Helpers.CreateFeature("CaretakerAnimalBonus" + statType,
                    "Caretaker (" + statType + ")",
                    "Humans often entrust halfling families with the care of children and animals, a task that has helped them develop keen insight. Such halflings gain a +2 racial bonus on Sense Motive checks. In addition, when they acquire an animal companion, bonded mount, cohort, or familiar, that creature gains a +2 bonus to one ability score of the character’s choice. This racial trait replaces halfling luck, sure-footed, and weapon familiarity.",
                    Helpers.getGuid("CaretakerAnimalBonus" + statType),
                    null,
                    FeatureGroup.Racial,
                    statType.CreateAddStatBonus(2, ModifierDescriptor.Racial));

                features.Add(Helpers.CreateFeature("Caretaker" + statType,
                    "Caretaker (" + statType + ")",
                    "Humans often entrust halfling families with the care of children and animals, a task that has helped them develop keen insight. Such halflings gain a +2 racial bonus on Sense Motive checks. In addition, when they acquire an animal companion, bonded mount, cohort, or familiar, that creature gains a +2 bonus to one ability score of the character’s choice. This racial trait replaces halfling luck, sure-footed, and weapon familiarity.",
                    Helpers.getGuid("Caretaker" + statType),
                    null,
                    _featureGroup,
                    halfling.PrerequisiteFeature(),
                    HalflingLuck.PrerequisiteFeature(),
                    SureFooted.PrerequisiteFeature(),
                    Helpers.Create<RemoveFeature>(x => x.Feature = HalflingLuck),
                    Helpers.Create<RemoveFeature>(x => x.Feature = SureFooted),
                    StatType.SkillPerception.CreateAddStatBonus(2, ModifierDescriptor.Racial),
                    Helpers.Create<AddFeatureToCompanion>(x => x.Feature = addStatBonusToAnimalCompanion)
                ));
            }

            features.Add(Helpers.CreateFeature("EvasiveNomad",
                "Evasive Nomad",
                "Halflings with this racial trait gain a +2 racial bonus on Reflex saves, but they take a –2 penalty on saving throws against fear effects. This replaces fearless.",
                Helpers.getGuid("EvasiveNomad"),
                null,
                _featureGroup,
                halfling.PrerequisiteFeature(),
                Fearless.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(x => x.Feature = Fearless),
                StatType.SaveReflex.CreateAddStatBonus(2, ModifierDescriptor.Racial),
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(x =>
                {
                    x.ModifierDescriptor = ModifierDescriptor.Racial;
                    x.SpellDescriptor = new SpellDescriptorWrapper(SpellDescriptor.Fear);
                    x.Value = -2;
                    x.OnlyPositiveValue = false;
                })
            ));

            features.Add(Helpers.CreateFeature("FleetFoot",
                "Fleet of Foot",
                "Some halflings are quicker than their kin but less cautious. Halflings with this racial trait move at normal speed and have a base speed of 30 feet. This racial trait replaces slow speed and sure-footed.",
                Helpers.getGuid("FleetFoot"),
                null,
                _featureGroup,
                halfling.PrerequisiteFeature(),
                SlowSpeedGnome.PrerequisiteFeature(),
                SureFooted.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(x => x.Feature = SlowSpeedGnome),
                Helpers.Create<RemoveFeature>(x => x.Feature = SureFooted)
            ));

            features.Add(Helpers.CreateFeature("Shadowplay",
                "Shadowplay",
                "Characters with this trait cast spells from the illusion school at +1 caster level. Halflings can take this trait in place of weapon familiarity.",
                Helpers.getGuid("Shadowplay "),
                null,
                _featureGroup,
                halfling.PrerequisiteFeature(),
                Helpers.Create<IncreaseSpellSchoolCasterLevel>(x => x.School = SpellSchool.Illusion)
            ));

            features.Add(Helpers.CreateFeature("Wanderlust",
                "Wanderlust",
                "Halflings love travel and maps. Halflings with this racial trait receive a +2 bonus on Knowledge (World) and Lore (Nature) checks. This racial trait replaces fearless.",
                Helpers.getGuid("Wanderlust "),
                null,
                _featureGroup,
                halfling.PrerequisiteFeature(),
                Fearless.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(x => x.Feature = Fearless),
                StatType.SkillKnowledgeWorld.CreateAddStatBonus(2, ModifierDescriptor.Racial),
                StatType.SkillLoreNature.CreateAddStatBonus(2, ModifierDescriptor.Racial)
            ));

            addAlternativeRacialTraitsSelection(halfling, features);
        }

        private static void addAasimarAlternativeRacialTraitList()
        {
            BlueprintRace aasimar = library.Get<BlueprintRace>("b7f02ba92b363064fb873963bec275ee");
            List<BlueprintFeature> features = new List<BlueprintFeature>();
            BlueprintFeature AasimarHaloFeature = library.Get<BlueprintFeature>("d3f14f00f675a6341a41d2194186835c");
            BlueprintFeature CelestialResistance = library.Get<BlueprintFeature>("679d817b0fe6c394089051a819c05c1c");

            BlueprintFeature[] heritageList = new BlueprintFeature[]
            {
                library.Get<BlueprintFeature>("4285d0c6b57444c46a302899e0149b09"), // AgathionHeritage
                library.Get<BlueprintFeature>("ceedc840b113c3348a2f32b434df5fef"), // AngelHeritage
                library.Get<BlueprintFeature>("72c04df144dbb644583184a7828c69b9"), // ArchonHeritage
                library.Get<BlueprintFeature>("0f9170125bc1bac478c62ef1433fa1ec"), // AzataHeritage
                library.Get<BlueprintFeature>("3b545be3f5cc9fd4081937c226360625"), // ClassicHeritage
                library.Get<BlueprintFeature>("b3494115041bdc1428b4443bf6bf68c3"), // GarudaHeritage
                library.Get<BlueprintFeature>("4ce39942e203de74198e3d2fd0608b96"), // PeriHeritage
            };

            StatType[] abilityTypes = new StatType[]
            {
                StatType.Strength,
                StatType.Dexterity,
                StatType.Constitution,
                StatType.Intelligence,
                StatType.Wisdom,
                StatType.Charisma
            };

            List<BlueprintAbility> spellLikeAbilities = new List<BlueprintAbility>();
            foreach(BlueprintFeature heritageFeature in heritageList)
            {
                AddFacts spellLikeAbilityFact = heritageFeature.GetComponent<AddFacts>();
                spellLikeAbilities.Add((BlueprintAbility)spellLikeAbilityFact.Facts[0]);
            }


            BlueprintFeature lostPromiseClaw = Helpers.CreateFeature("LostPromiseClaw",
                "Lost Promise (Claw)",
                "While many view aasimars’ beauty and celestial powers as a gift, in some communities an aasimar might be persecuted for being different and fall into darkness. The forces of evil delight in such a perversion of their celestial counterparts’ gifts. The aasimar gains the maw or claw tiefling alternate racial trait. This racial trait replaces the spell-like ability racial trait.",
                Helpers.getGuid("LostPromiseClaw"),
                null,
                _featureGroup,
                    aasimar.PrerequisiteFeature(),
                Helpers.Create<BlueprintAbilityPrerequisite>(x => x._abilityList = spellLikeAbilities),
                Helpers.Create<EmptyHandWeaponOverride>(r => r.Weapon = claw)
            );
            foreach(BlueprintAbility ability in spellLikeAbilities)
            {
                lostPromiseClaw.AddComponent(Helpers.Create<RemoveFeature>(r => r.Feature = ability));
            }

            BlueprintFeature lostPromiseBite = Helpers.CreateFeature("LostPromiseBite",
                "Lost Promise (Bite)",
                "While many view aasimars’ beauty and celestial powers as a gift, in some communities an aasimar might be persecuted for being different and fall into darkness. The forces of evil delight in such a perversion of their celestial counterparts’ gifts. The aasimar gains the maw or claw tiefling alternate racial trait. This racial trait replaces the spell-like ability racial trait.",
                Helpers.getGuid("LostPromiseBite"),
                null,
                _featureGroup,
                    aasimar.PrerequisiteFeature(),
                Helpers.Create<BlueprintAbilityPrerequisite>(x => x._abilityList = spellLikeAbilities),
                Helpers.Create<AddAdditionalLimb>(x => x.Weapon = bite)
            );
            foreach(BlueprintAbility ability in spellLikeAbilities)
            {
                lostPromiseBite.AddComponent(Helpers.Create<RemoveFeature>(r => r.Feature = ability));
            }

            features.Add(lostPromiseClaw);
            features.Add(lostPromiseBite);

            foreach(StatType abilityType in abilityTypes)
            {
                BlueprintFeature abilityTypeFeature = Helpers.CreateFeature("VariantAbility" + abilityType,
                    "Variant Ability: +2 " + abilityType,
                    "Replace the aasimar spell like ability with a +2 Racial Bonus to " + abilityType,
                    Helpers.getGuid("VariantAbilityAasimar" + abilityType),
                    null,
                    _featureGroup,
                    aasimar.PrerequisiteFeature(),
                    abilityType.CreateAddStatBonus(2, ModifierDescriptor.Racial),
                    Helpers.Create<BlueprintAbilityPrerequisite>(x => x._abilityList = spellLikeAbilities)
                );
                foreach(BlueprintAbility ability in spellLikeAbilities)
                {
                    abilityTypeFeature.AddComponent(Helpers.Create<RemoveFeature>(r => r.Feature = ability));
                }
                features.Add(abilityTypeFeature);
            }

            addAlternativeRacialTraitsSelection(aasimar, features);
        }

        private static void addTieflingAlternativeRacialTraitList()
        {
            BlueprintRace tiefling = library.Get<BlueprintRace>("5c4e42124dc2b4647af6e36cf2590500");
            List<BlueprintFeature> features = new List<BlueprintFeature>();
            BlueprintFeature FiendishResistance = library.Get<BlueprintFeature>("c4029234bd17ee9488941ad7a14333f6");

            BlueprintFeature[] heritageList = new BlueprintFeature[]
            {
                library.Get<BlueprintFeature>("9f377814338e7264bbd499b099e6ef86"), // TieflingHeritageAsura
                library.Get<BlueprintFeature>("c82bc8134f3a6e24994b8ef70fb4014a"), // TieflingHeritageClassic
                library.Get<BlueprintFeature>("b32fd17ae27982648a30cf076790b0e8"), // TieflingHeritageDaemon
                library.Get<BlueprintFeature>("a53d760a364cd90429e16aa1e7048d0a"), // TieflingHeritageDemodand
                library.Get<BlueprintFeature>("c09ffb2657f6c2b41b5ed0ed607b362a"), // TieflingHeritageDemon
                library.Get<BlueprintFeature>("02a2c984494a9734ba8b01927dcf96e2"), // TieflingHeritageDevil
                library.Get<BlueprintFeature>("e3d324eb309cdf44a87d666c7a27715c"), // TieflingHeritageDiv
                library.Get<BlueprintFeature>("c5b7ad498cca069499dc71b97cbe51fb"), // TieflingHeritageKyton
                library.Get<BlueprintFeature>("35bb558fbd0f4b046b172a0bcf7ed682"), // TieflingHeritageOni
                library.Get<BlueprintFeature>("9fcf9688effb01343aa44124e000f7ed"), // TieflingHeritageQlippoth
                library.Get<BlueprintFeature>("ea6a099d0e963e145a7268e4343d5924"), // TieflingHeritageRakshasa
            };

            StatType[] abilityTypes = new StatType[]
            {
                StatType.Intelligence,
                StatType.Wisdom,
                StatType.Charisma
            };

            List<BlueprintAbility> spellLikeAbilities = new List<BlueprintAbility>();
            foreach(BlueprintFeature heritageFeature in heritageList)
            {
                AddFacts spellLikeAbilityFact = heritageFeature.GetComponent<AddFacts>();
                spellLikeAbilities.Add((BlueprintAbility)spellLikeAbilityFact.Facts[0]);
            }

            features.Add(Helpers.CreateFeature("ScaledSkinCold",
                "Scaled Skin (Cold)",
                "The skin of these tieflings provides some energy resistance, but is also as hard as armor. Choose one of the following energy types: cold, electricity, or fire. A tiefling with this trait gains resistance 5 in the chosen energy type and also gains a +1 natural armor bonus to AC. This racial trait replaces fiendish resistance.",
                Helpers.getGuid("ScaledSkinCold"),
                null,
                _featureGroup,
                tiefling.PrerequisiteFeature(),
                FiendishResistance.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(feature => feature.Feature = FiendishResistance),
                StatType.AC.CreateAddStatBonus(1, ModifierDescriptor.NaturalArmor),
                Helpers.Create<AddDamageResistanceEnergy>(x =>
                {
                    x.Type = DamageEnergyType.Cold;
                    x.ValueMultiplier = new ContextValue();
                    x.ValueMultiplier.Value = 5;
                })
            ));

            features.Add(Helpers.CreateFeature("ScaledSkinElectricity",
                "Scaled Skin (Electricity)",
                "The skin of these tieflings provides some energy resistance, but is also as hard as armor. Choose one of the following energy types: cold, electricity, or fire. A tiefling with this trait gains resistance 5 in the chosen energy type and also gains a +1 natural armor bonus to AC. This racial trait replaces fiendish resistance.",
                Helpers.getGuid("ScaledSkinElectricity"),
                null,
                _featureGroup,
                tiefling.PrerequisiteFeature(),
                FiendishResistance.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(feature => feature.Feature = FiendishResistance),
                StatType.AC.CreateAddStatBonus(1, ModifierDescriptor.NaturalArmor),
                Helpers.Create<AddDamageResistanceEnergy>(x =>
                {
                    x.Type = DamageEnergyType.Electricity;
                    x.ValueMultiplier = new ContextValue();
                    x.ValueMultiplier.Value = 5;
                })
            ));

            features.Add(Helpers.CreateFeature("ScaledSkinFire",
                "Scaled Skin (Fire)",
                "The skin of these tieflings provides some energy resistance, but is also as hard as armor. Choose one of the following energy types: cold, electricity, or fire. A tiefling with this trait gains resistance 5 in the chosen energy type and also gains a +1 natural armor bonus to AC. This racial trait replaces fiendish resistance.",
                Helpers.getGuid("ScaledSkinFire"),
                null,
                _featureGroup,
                tiefling.PrerequisiteFeature(),
                FiendishResistance.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(feature => feature.Feature = FiendishResistance),
                StatType.AC.CreateAddStatBonus(1, ModifierDescriptor.NaturalArmor),
                Helpers.Create<AddDamageResistanceEnergy>(x =>
                {
                    x.Type = DamageEnergyType.Fire;
                    x.ValueMultiplier = new ContextValue();
                    x.ValueMultiplier.Value = 5;
                })
            ));

            BlueprintFeature lostPromiseClaw = Helpers.CreateFeature("MawClawClaw",
               "Maw or Claw (Claw)",
               "Some tieflings take on the more bestial aspects of their fiendish ancestors. These tieflings exhibit either powerful, toothy maws or dangerous claws. The tiefling can choose a bite attack that deals 1d6 points of damage or two claws that each deal 1d4 points of damage. These attacks are primary natural attacks. This racial trait replaces the spell-like ability racial trait.",
               Helpers.getGuid("MawClawClaw"),
               null,
               _featureGroup,
                   tiefling.PrerequisiteFeature(),
               Helpers.Create<BlueprintAbilityPrerequisite>(x => x._abilityList = spellLikeAbilities),
               Helpers.Create<EmptyHandWeaponOverride>(r => r.Weapon = claw)
            );
            foreach(BlueprintAbility ability in spellLikeAbilities)
            {
                lostPromiseClaw.AddComponent(Helpers.Create<RemoveFeature>(r => r.Feature = ability));
            }

            BlueprintFeature lostPromiseBite = Helpers.CreateFeature("MawClawBite",
                "Maw or Claw (Bite)",
                "Some tieflings take on the more bestial aspects of their fiendish ancestors. These tieflings exhibit either powerful, toothy maws or dangerous claws. The tiefling can choose a bite attack that deals 1d6 points of damage or two claws that each deal 1d4 points of damage. These attacks are primary natural attacks. This racial trait replaces the spell-like ability racial trait.",
                Helpers.getGuid("MawClawBite"),
                null,
                _featureGroup,
                tiefling.PrerequisiteFeature(),
                Helpers.Create<BlueprintAbilityPrerequisite>(x => x._abilityList = spellLikeAbilities),
                Helpers.Create<AddAdditionalLimb>(x => x.Weapon = bite)
            );
            foreach(BlueprintAbility ability in spellLikeAbilities)
            {
                lostPromiseBite.AddComponent(Helpers.Create<RemoveFeature>(r => r.Feature = ability));
            }

            features.Add(lostPromiseClaw);
            features.Add(lostPromiseBite);

            foreach(StatType abilityType in abilityTypes)
            {
                BlueprintFeature abilityTypeFeature = Helpers.CreateFeature("VariantAbility" + abilityType,
                    "Variant Ability: +2 " + abilityType,
                    "Replace the aasimar spell like ability with a +2 Racial Bonus to " + abilityType + ". Replaces the tiefling’s spell-like ability racial trait.",
                    Helpers.getGuid("VariantAbilityTiefling" + abilityType),
                    null,
                    _featureGroup,
                    tiefling.PrerequisiteFeature(),
                    abilityType.CreateAddStatBonus(2, ModifierDescriptor.Racial),
                    Helpers.Create<BlueprintAbilityPrerequisite>(x => x._abilityList = spellLikeAbilities)
                );
                foreach(BlueprintAbility ability in spellLikeAbilities)
                {
                    abilityTypeFeature.AddComponent(Helpers.Create<RemoveFeature>(r => r.Feature = ability));
                }

                features.Add(abilityTypeFeature);
            }
            BlueprintFeature overSizedLimbs = Helpers.CreateFeature("OverSizedLimbs",
                "Over-sized limbs",
                "You have over-sized limbs, allowing you to use Large weapons without penalty. Replaces the tiefling’s spell-like ability racial trait.",
                Helpers.getGuid("OverSizedLimbs"),
                null,
                _featureGroup,
                tiefling.PrerequisiteFeature(),
                Helpers.Create<WeaponSizeChange>(x => x.SizeCategoryChange = 1),
                Helpers.Create<BlueprintAbilityPrerequisite>(x => x._abilityList = spellLikeAbilities)

            );
            foreach(BlueprintAbility ability in spellLikeAbilities)
            {
                overSizedLimbs.AddComponent(Helpers.Create<RemoveFeature>(r => r.Feature = ability));
            }
            features.Add(overSizedLimbs);

            addAlternativeRacialTraitsSelection(tiefling, features);
        }

        private static void addHalfOrcAlternativeRacialTraitList()
        {
            BlueprintRace halfOrc = library.Get<BlueprintRace>("1dc20e195581a804890ddc74218bfd8e");
            List<BlueprintFeature> features = new List<BlueprintFeature>();
            BlueprintFeature halfOrcFerocity = library.Get<BlueprintFeature>("c99f3405d1ef79049bd90678a666e1d7");
            BlueprintFeature halfOrcIntimidating = library.Get<BlueprintFeature>("885f478dff2e39442a0f64ceea6339c9");
            BlueprintFeature orcWeaponFamiliarity = library.Get<BlueprintFeature>("6ab6c271d1558344cbc746350243d17d");

            features.Add(Helpers.CreateFeature("SacredTattoo",
                "Sacred Tattoo",
                "Many half-orcs decorate themselves with tattoos, piercings, and ritual scarification, which they consider sacred markings. Half-orcs with this racial trait gain a +1 luck bonus on all saving throws. This racial trait replaces orc ferocity.",
                Helpers.getGuid("SacredTattoo"),
                null,
                _featureGroup,
                halfOrc.PrerequisiteFeature(),
                halfOrcFerocity.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = halfOrcFerocity),
                StatType.SaveReflex.CreateAddStatBonus(1, ModifierDescriptor.Luck),
                StatType.SaveFortitude.CreateAddStatBonus(1, ModifierDescriptor.Luck),
            StatType.SaveWill.CreateAddStatBonus(1, ModifierDescriptor.Luck)
            ));

            BlueprintFeature longswordProficiency = library.Get<BlueprintFeature>("62e27ffd9d53e14479f73da29760f64e");
            BlueprintFeature heavyFlailProficiency = library.Get<BlueprintFeature>("a22e30bd35fbb704cab2d7e3c00717c1");
            BlueprintFeature flailProficiency = library.Get<BlueprintFeature>("6d273f46bce2e0f47a0958810dc4c7d9");

            // This is supposed to add whip proficiencies, Chain Fighter is supposed to give proficiencies with dire flails and spiked chains none of these are in game so the are combined into one
            features.Add(Helpers.CreateFeature("CityRaised", "City-Raised",
                "Half-orcs with this trait know little of their orc ancestry and were raised among humans and other half-orcs in a large city. City-raised half-orcs are proficient with flails and longswords, flails and heavy flails and receive a +2 racial bonus on Knowledge (World) checks. This racial trait replaces weapon familiarity. \n(Dev note: combines with Chain Fighter due to lack of whips, dire flails and spiked chains)",
                Helpers.getGuid("CityRaised"),
                null,
                FeatureGroup.Racial,
                halfOrc.PrerequisiteFeature(),
                orcWeaponFamiliarity.PrerequisiteFeature(),
                Helpers.Create<AddFeatureOnApply>(r => r.Feature = longswordProficiency),
                Helpers.Create<AddFeatureOnApply>(r => r.Feature = heavyFlailProficiency),
                Helpers.Create<AddFeatureOnApply>(r => r.Feature = flailProficiency),
                StatType.SkillKnowledgeWorld.CreateAddStatBonus(2, ModifierDescriptor.Racial),
                Helpers.Create<RemoveFeature>(r => r.Feature = orcWeaponFamiliarity)));

            BlueprintFeature endurance = library.Get<BlueprintFeature>("54ee847996c25cd4ba8773d7b8555174");
            features.Add(Helpers.CreateFeature("ShamansApprentice", "Shaman’s Apprentice",
                "Only the most stalwart survive the years of harsh treatment that an apprenticeship to an orc shaman entails. Half-orcs with this trait gain Endurance as a bonus feat. This racial trait replaces the intimidating trait.",
                Helpers.getGuid("ShamansApprentice"),
                null,
                _featureGroup,
                halfOrcIntimidating.PrerequisiteFeature(),
                halfOrc.PrerequisiteFeature(),
                Helpers.Create<AddFeatureOnApply>(r => r.Feature = endurance),
                Helpers.Create<RemoveFeature>(r => r.Feature = halfOrcIntimidating)
            ));

            string bite1d4Id = "35dfad6517f401145af54111be04d6cf";
            BlueprintItemWeapon bite = library.CopyAndAdd<BlueprintItemWeapon>(bite1d4Id, "Half-Orc Bite",
                Helpers.getGuid("HalfOrcBite"));
            bite.Type.DamageType.Physical.Form = PhysicalDamageForm.Piercing;

            features.Add(Helpers.CreateFeature("Toothy", "Toothy",
                "Some half-orcs’ tusks are large and sharp, granting a bite attack. This is a primary natural attack that deals 1d4 points of piercing damage. This racial trait replaces orc ferocity.",
                Helpers.getGuid("Toothy"),
                null,
                _featureGroup,
                halfOrc.PrerequisiteFeature(),
                halfOrcFerocity.PrerequisiteFeature(),
                Helpers.Create<AddAdditionalLimb>(r => r.Weapon = bite),
                Helpers.Create<RemoveFeature>(r => r.Feature = halfOrcFerocity)
            ));

            features.Add(Helpers.CreateFeature("Bestial", "Bestial",
                "The orc blood of some half-orcs manifests in the form of particularly prominent orc features, exacerbating their bestial appearances but improving their already keen senses. They gain a +2 racial bonus on Perception checks. This racial trait replaces orc ferocity.",
                Helpers.getGuid("Bestial"),
                null,
                _featureGroup,
                halfOrc.PrerequisiteFeature(),
                halfOrcFerocity.PrerequisiteFeature(),
                StatType.SkillPerception.CreateAddStatBonus(2, ModifierDescriptor.Racial),
                Helpers.Create<RemoveFeature>(r => r.Feature = halfOrcFerocity)));

            features.Add(Helpers.CreateFeature("BurningAssurance", "Burning Assurance",
                "Half-orcs acquire as a result of prejudice, and their self-confidence puts others at ease. Desert half-orcs with this racial trait gain a +2 racial bonus on Diplomacy checks. This replaces Intimidating.",
                Helpers.getGuid("BurningAssurance"),
                null,
                _featureGroup,
                halfOrc.PrerequisiteFeature(),
                halfOrcIntimidating.PrerequisiteFeature(),
                StatType.CheckDiplomacy.CreateAddStatBonus(2, ModifierDescriptor.Racial),
                Helpers.Create<RemoveFeature>(r => r.Feature = halfOrcIntimidating)));

            addAlternativeRacialTraitsSelection(halfOrc, features);
        }

        private static void addGoblinAlternativeRacialTraitList()
        {
            BlueprintRace goblin = library.Get<BlueprintRace>("9d168ca7100e9314385ce66852385451");
            List<BlueprintFeature> features = new List<BlueprintFeature>();
            BlueprintFeature stealthy = library.Get<BlueprintFeature>("610652378253d3845bb70f005c084daa");


            string bite1d4Id = "35dfad6517f401145af54111be04d6cf";
            BlueprintItemWeapon bite = library.CopyAndAdd<BlueprintItemWeapon>(bite1d4Id, "Goblin Bite",
                Helpers.getGuid("GoblinBite"));
            bite.Type.DamageType.Physical.Form = PhysicalDamageForm.Piercing;


            BlueprintFeature caveCrawlerFeature = Helpers.CreateFeature("CaveCrawler",
                "Cave Crawler",
                "Some goblins are born and raised in caves and rarely see the light of day. Goblins with this trait gain a +8 racial bonus on Athletics checks. Goblins with this racial trait have a base speed of 20 feet.",
                Helpers.getGuid("CaveCrawler"),
                null,
                _featureGroup,
                goblin.PrerequisiteFeature(),
                StatType.Speed.CreateAddStatBonus(-10, ModifierDescriptor.Racial),
                StatType.SkillAthletics.CreateAddStatBonus(8, ModifierDescriptor.Racial)
            );
            caveCrawlerFeature.AddComponent(caveCrawlerFeature.PrerequisiteNoFeature());
            features.Add(caveCrawlerFeature);

            features.Add(Helpers.CreateFeature("HardHeadBigTeeth",
                "Hard Head, Big Teeth",
                "Goblins are known for their balloon-like heads and enormous maws, but some have even more exaggeratedly large heads filled with razor-sharp teeth. Goblins with this trait gain a bite attack as a primary natural attack that deals 1d4 points of damage. This racial trait replaces skilled.",
                Helpers.getGuid("HardHeadBigTeeth"),
                null,
                _featureGroup,
                goblin.PrerequisiteFeature(),
                stealthy.PrerequisiteFeature(),
                Helpers.Create<RemoveFeature>(r => r.Feature = stealthy),
                Helpers.Create<AddAdditionalLimb>(r => r.Weapon = bite)
            ));

            features.Add(Helpers.CreateFeature("CityScavenger", "City Scavenger",
                "Goblins who live within the boundaries of human cities survive by scavenging for refuse and hunting stray animals. Goblins with this trait gain a +2 racial bonus on Perception and Lore Nature checks. This racial trait replaces skilled.",
                Helpers.getGuid("CityScavenger"),
                null,
                FeatureGroup.Racial,
                goblin.PrerequisiteFeature(),
                stealthy.PrerequisiteFeature(),
                StatType.SkillPerception.CreateAddStatBonus(2, ModifierDescriptor.Racial),
                StatType.SkillLoreNature.CreateAddStatBonus(2, ModifierDescriptor.Racial),
                Helpers.Create<RemoveFeature>(r => r.Feature = stealthy)));

            features.Add(Helpers.CreateFeature("OverSizedEars", "Over-Sized Ears",
                "While goblins’ ears are never dainty, these goblins have freakishly large ears capable of picking up even the smallest sounds. Goblins with this racial trait gain a +4 bonus on Perception checks. This racial trait replaces skilled.",
                Helpers.getGuid("OverSizedEars"),
                null,
                FeatureGroup.Racial,
                goblin.PrerequisiteFeature(),
                stealthy.PrerequisiteFeature(),
                StatType.SkillPerception.CreateAddStatBonus(4, ModifierDescriptor.Racial),
                Helpers.Create<RemoveFeature>(r => r.Feature = stealthy)));

            features.Add(Helpers.CreateFeature("TreeRunner", "Tree Runner",
                "In trackless rain forests and marshes, it can be difficult to find dry ground to build on. goblin tribes living in such areas have learned to live in the treetops. These goblins gain a +4 racial bonus on Mobility checks. This racial trait replaces skilled.",
                Helpers.getGuid("TreeRunner"),
                null,
                FeatureGroup.Racial,
                goblin.PrerequisiteFeature(),
                stealthy.PrerequisiteFeature(),
                StatType.SkillMobility.CreateAddStatBonus(4, ModifierDescriptor.Racial),
                Helpers.Create<RemoveFeature>(r => r.Feature = stealthy)));

            BlueprintFeature glaiveProficiency = library.Get<BlueprintFeature>("38d4d143e7f293249b72694ddb1e0a32");
            BlueprintFeature shortswordProficiency = library.Get<BlueprintFeature>("9e828934974f0fc4bbf7542eb0446e45");

            features.Add(Helpers.CreateFeature("WeaponFamiliarity", "Weapon Familiarity",
                "Goblins’ traditional weapons are the dogslicer (Shortsword) and the horsechopper(Glaive), weapons designed specifically to bring down their most hated foes. Goblins with this trait are proficient with the dogslicer (Shortsword) and the horsechopper(Glaive). This racial trait replaces skilled.",
                Helpers.getGuid("WeaponFamiliarity"),
                null,
                FeatureGroup.Racial,
                goblin.PrerequisiteFeature(),
                stealthy.PrerequisiteFeature(),
                Helpers.Create<AddFeatureOnApply>(r => r.Feature = glaiveProficiency),
                Helpers.Create<AddFeatureOnApply>(r => r.Feature = shortswordProficiency)));

            addAlternativeRacialTraitsSelection(goblin, features);
        }

        private static void addAlternativeRacialTraitsSelection(BlueprintRace race, List<BlueprintFeature> blueprintFeatures)
        {
            BlueprintFeatureSelection alternativeRacialTraitsSelection2 = Helpers.CreateFeatureSelection(
                "AlternativeRacialTraitsSelection2" + race.Name,
                "Alternative Racial Traits",
                "Select alternative racial traits",
                Helpers.getGuid("AlternativeRacialTraitsSelection2" + race.Name),
                null, FeatureGroup.ChannelEnergy);

            BlueprintFeatureSelection alternativeRacialTraitsSelection3 = Helpers.CreateFeatureSelection(
                "AlternativeRacialTraitsSelection3" + race.Name,
                "Alternative Racial Traits",
                "Select alternative racial traits",
                Helpers.getGuid("AlternativeRacialTraitsSelection3" + race.Name),
                null, FeatureGroup.ChannelEnergy);

            BlueprintFeatureSelection alternativeRacialTraitsSelection = Helpers.CreateFeatureSelection(
                "AlternativeRacialTraitsSelection" + race.Name,
                "Alternative Racial Traits",
                "Select alternative racial traits",
                Helpers.getGuid("AlternativeRacialTraitsSelection" + race.Name),
                null, FeatureGroup.ChannelEnergy);

            foreach(BlueprintFeature blueprintFeature in blueprintFeatures)
            {
                SelectFeature_Apply_Patch.onApplyFeature.Add(blueprintFeature, (state, unit) =>
                {
                    FeatureSelectionState selection1 = state.FindSelection(alternativeRacialTraitsSelection, true);
                    FeatureSelectionState selection2 = state.FindSelection(alternativeRacialTraitsSelection2, true);
                    FeatureSelectionState selection3 = state.FindSelection(alternativeRacialTraitsSelection3, true);
                    if(selection1 != null && selection2 == null && selection1.SelectedItem != null && (BlueprintFeature)selection1.SelectedItem != KeepNormalRacialTrait1)
                    {
                        alternativeRacialTraitsSelection2.AddSelection(state, unit, 1);
                    }

                    selection2 = state.FindSelection(alternativeRacialTraitsSelection2, true);
                    if(selection2 != null && selection3 == null && selection2.SelectedItem != null && (BlueprintFeature)selection2.SelectedItem != KeepNormalRacialTrait2)
                    {
                        alternativeRacialTraitsSelection3.AddSelection(state, unit, 1);
                    }
                });
            }

            alternativeRacialTraitsSelection.AllFeatures = blueprintFeatures.ToArray().AddToArray(KeepNormalRacialTrait1);
            alternativeRacialTraitsSelection2.AllFeatures = blueprintFeatures.ToArray().AddToArray(KeepNormalRacialTrait2);
            alternativeRacialTraitsSelection3.AllFeatures = blueprintFeatures.ToArray().AddToArray(KeepNormalRacialTrait3);

            ApplyClassMechanics_Apply_Patch.onChargenApply.Add((state, unit) =>
            {
                if(unit.Progression.Race == race)
                {
                    alternativeRacialTraitsSelection.AddSelection(state, unit, 1);
                }
            });
        }
    }
}
