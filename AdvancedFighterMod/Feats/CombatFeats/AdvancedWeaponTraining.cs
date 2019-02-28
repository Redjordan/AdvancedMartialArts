using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AdvancedMartialArts.Feats.CombatFeats.FeatLogic;
using AdvancedMartialArts.HelperClasses;
using Harmony12;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Kingdom;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using UnityEngine;

namespace AdvancedMartialArts.Feats.CombatFeats
{
    internal class AdvancedWeaponTraining
    {
        private static LibraryScriptableObject library => Main.library;

        public static Dictionary<WeaponCategory, WeaponFighterGroup> _weaponCategoryToWeaponTraining = new Dictionary<WeaponCategory, WeaponFighterGroup>();
        public static Dictionary<BlueprintFeature, HashSet<WeaponCategory>> _weaponTrainingToWeaponCategory = new Dictionary<BlueprintFeature, HashSet<WeaponCategory>>();

        public static List<BlueprintFeature> _weaponTrainingBluePrints = new List<BlueprintFeature>();

        public static Dictionary<WeaponFighterGroup, List<StatType>> _weaponTrainingToStatType = new Dictionary<WeaponFighterGroup, List<StatType>>(); // Versatile Training


        public static BlueprintFeature BraveryFeature = library.Get<BlueprintFeature>("f6388946f9f472f4585591b80e9f2452");
        public static BlueprintFeatureSelection WeaponTraining = library.Get<BlueprintFeatureSelection>("b8cecf4e5e464ad41b79d5b42b76b399");


        public static void Load()
        {
            Main.SafeLoad(CreateAdvancedWeaponTraining, "Advanced Weapon Training");

            Main.ApplyPatch(typeof(WarriorSpiritPoolLogic.BlueprintAbilityResourceGetMaxAmountPatch), "Test");
            Main.ApplyPatch(typeof(BlueprintParametrizedFeatureWeaponTraining.BlueprintAbilityResourceGetMaxAmountPatch), "Test");
        }

        private static void CreateAdvancedWeaponTraining()
        {
            var fighter = Helpers.fighterClass;

            BlueprintFeature twoHandedWeaponFeature = library.Get<BlueprintFeature>("88da2a5dfc505054f933bb81014e864f");
            twoHandedWeaponFeature.AddComponent(twoHandedWeaponFeature.PrerequisiteFeature());


            BlueprintFeatureSelection advancedWeaponTraining = Helpers.CreateFeatureSelection("AdvancedWeaponTraining",
                "Advanced Weapon Training",
                "Beginning at 9th level, instead of selecting an additional fighter weapon group, a fighter can choose an advanced weapon training option for one fighter weapon group that he previously selected with the weapon training class feature.",
                "e10b3aca8e8c4075b96ac6b5f27dae27",
                WeaponTraining.Icon,
                FeatureGroup.CombatFeat,
                Helpers.PrerequisiteFeature(WeaponTraining, true),
                Helpers.PrerequisiteFeature(twoHandedWeaponFeature, true),
                Helpers.PrerequisiteClassLevel(fighter, 5)
            );

            var features = getAdvancedWeaponTraining(fighter);

            var goBackFeature = Helpers.CreateFeature("0GoBack", "(Go Back)", "Helps to navigate in deep selection trees. Has no other use.", Helpers.getGuid("0GoBack"), null, FeatureGroup.None, Helpers.Create<UndoSelectionLogic>());
            var prerequisite = Helpers.Create<PrerequisiteMeetsPrerequisiteForAny>();
            prerequisite.Features = features.ToArray();
            goBackFeature.AddComponent(prerequisite);

            advancedWeaponTraining.AddComponent(prerequisite);
            features.Add(goBackFeature);
            advancedWeaponTraining.SetFeatures(features);

            library.AddCombatFeats(advancedWeaponTraining);

            BlueprintFeatureSelection advancedWeaponTrainingForSelection = Helpers.CreateFeatureSelection("AdvancedWeaponTrainingLVL9",
                "Advanced Weapon Training",
                "Beginning at 9th level, instead of selecting an additional fighter weapon group, a fighter can choose an advanced weapon training option for one fighter weapon group that he previously selected with the weapon training class feature.",
                "914b143cd76b40dc91feb87d7c404b23",
                WeaponTraining.Icon,
                FeatureGroup.CombatFeat,
                Helpers.PrerequisiteFeature(WeaponTraining),
                Helpers.PrerequisiteClassLevel(fighter, 9)
            );

            BlueprintFeatureSelection advancedWeaponTrainingForSelectionLVL13 = Helpers.CreateFeatureSelection("AdvancedWeaponTrainingLVL13",
                "Advanced Weapon Training",
                "Beginning at 9th level, instead of selecting an additional fighter weapon group, a fighter can choose an advanced weapon training option for one fighter weapon group that he previously selected with the weapon training class feature.",
                Helpers.getGuid("AdvancedWeaponTrainingLVL13"),
                WeaponTraining.Icon,
                FeatureGroup.CombatFeat,
                Helpers.PrerequisiteFeature(WeaponTraining),
                Helpers.PrerequisiteClassLevel(fighter, 13)
            );

            BlueprintFeatureSelection advancedWeaponTrainingForSelectionLVL17 = Helpers.CreateFeatureSelection("AdvancedWeaponTrainingLVL17",
                "Advanced Weapon Training",
                "Beginning at 9th level, instead of selecting an additional fighter weapon group, a fighter can choose an advanced weapon training option for one fighter weapon group that he previously selected with the weapon training class feature.",
                Helpers.getGuid("AdvancedWeaponTrainingLVL17"),
                WeaponTraining.Icon,
                FeatureGroup.CombatFeat,
                Helpers.PrerequisiteFeature(WeaponTraining),
                Helpers.PrerequisiteClassLevel(fighter, 17)
            );
            advancedWeaponTrainingForSelection.SetFeatures(features);
            advancedWeaponTrainingForSelection.AddComponent(prerequisite);
            advancedWeaponTrainingForSelectionLVL13.SetFeatures(features);
            advancedWeaponTrainingForSelectionLVL13.AddComponent(prerequisite);
            advancedWeaponTrainingForSelectionLVL17.SetFeatures(features);
            advancedWeaponTrainingForSelectionLVL17.AddComponent(prerequisite);


            WeaponTraining.SetFeatures(WeaponTraining.AllFeatures.AddToArray(advancedWeaponTrainingForSelection, advancedWeaponTrainingForSelectionLVL13, advancedWeaponTrainingForSelectionLVL17));
        }

        private static List<BlueprintFeature> getAdvancedWeaponTraining(BlueprintCharacterClass fighter)
        {
            var weaponFocus = library.Get<BlueprintParametrizedFeature>("1e1f627d26ad36f43bbd26cc2bf8ac7e");
            var weaponFinesse = library.Get<BlueprintFeature>("90e54424d682d104ab36436bd527af09");
            var features = new List<BlueprintFeature>();
            FillWeaponFocusAndWeaponTrainingDictionaries();
            CreateAbilityMoveActions();
            var noFeature = Helpers.PrerequisiteNoFeature(null);


            BlueprintFeature twoWeaponFightingBlueprintFeature = library.Get<BlueprintFeature>("ac8aaf29054f5b74eb18f2af950e752d");
            foreach (var weaponTraining in _weaponTrainingBluePrints)
            {
                string weaponTrainingName = getWeaponTrainingName(weaponTraining, true);
                string weaponTrainingDisplayName = getWeaponTrainingName(weaponTraining);


                string newAssetID = Helpers.getGuid("FocusedWeapon" + weaponTrainingName);

                var focusedWeaponSpecific = Helpers.Create<BlueprintParametrizedFeatureWeaponTraining>(x => x.WeaponTraining = weaponTraining);
                Helpers.SetFeatureInfo(focusedWeaponSpecific, "FocusedWeapon" + weaponTrainingName,
                    "Focused Weapon (" + weaponTrainingDisplayName + ")", "The fighter selects one weapon for which he has Weapon Focus and that belongs to the associated fighter weapon group. The fighter can deal damage with this weapon based on the damage of the warpriest’s sacred weapon class feature, treating his fighter level as his warpriest level. The fighter must have Weapon Focus with the selected weapon in order to choose this option.",
                    newAssetID,
                    WeaponTraining.Icon,
                    FeatureGroup.Feat,
                    weaponFocus.PrerequisiteFeature(),
                    weaponTraining.PrerequisiteFeature(),
                    Helpers.Create<FocusedWeaponLogic>());
                focusedWeaponSpecific.ParameterType = FeatureParameterType.WeaponCategory;
                focusedWeaponSpecific.Prerequisite = weaponFocus;
                focusedWeaponSpecific.Ranks = 1;

                features.Add(focusedWeaponSpecific);


                features.Add(Helpers.CreateFeature(
                    "ArmedBravery" + weaponTrainingName,
                    "Armed Bravery (" + weaponTrainingDisplayName + ")",
                    "The fighter applies his bonus from bravery to Will saving throws. In addition, the DC of Intimidate checks to demoralize him increases by an amount equal to twice his bonus from bravery. The fighter must have the bravery class feature in order to select this option.",
                    Helpers.getGuid("ArmedBravery" + weaponTrainingName),
                    WeaponTraining.Icon,
                    FeatureGroup.CombatFeat,
                    weaponTraining.PrerequisiteFeature(),
                    BraveryFeature.PrerequisiteFeature(),
                    Helpers.Create<ArmedBraveryLogic>(x => x.WeaponTraining = weaponTraining)
                ));

                features.Add(Helpers.CreateFeature(
                    "CombatManeuverDefense" + weaponTrainingName,
                    "Combat Maneuver Defense (" + weaponTrainingDisplayName + ")",
                    "When the fighter is wielding weapons from the associated weapon group, his weapon training bonus applies to his CMD against all combat maneuvers attempted against him, instead of just against disarm and sunder combat maneuvers.",
                    Helpers.getGuid("CombatManeuverDefense" + weaponTrainingName),
                    WeaponTraining.Icon,
                    FeatureGroup.CombatFeat,
                    weaponTraining.PrerequisiteFeature(),
                    WeaponTraining.PrerequisiteFeature(),
                    Helpers.Create<CombatManeuverDefenseLogic>(x => x.WeaponTraining = weaponTraining)
                ));

                features.Add(Helpers.CreateFeature(
                    "DazzlingIntimidation" + weaponTrainingName,
                    "Dazzling Intimidation (" + weaponTrainingDisplayName + ")",
                    "The fighter applies his weapon training bonus to Intimidate checks and can attempt an Intimidate check to demoralize an opponent as a move action instead of a standard action. If he has the Dazzling Display feat, he can use it as a standard action instead of a full-round action.",
                    Helpers.getGuid("DazzlingIntimidation" + weaponTrainingName),
                    WeaponTraining.Icon,
                    FeatureGroup.CombatFeat,
                    weaponTraining.PrerequisiteFeature(),
                    WeaponTraining.PrerequisiteFeature(),
                    Helpers.Create<DazzlingIntimidationLogic>(x => x.WeaponTraining = weaponTraining)
                ));

                features.Add(Helpers.CreateFeature(
                    "DefensiveWeaponTraining" + weaponTrainingName,
                    "Defensive Weapon Training (" + weaponTrainingDisplayName + ")",
                    "The fighter gains a +1 shield bonus to his Armor Class. The fighter adds half his weapon’s enhancement bonus (if any) to this shield bonus. When his weapon training bonus for weapons from the associated fighter weapon group reaches +4, this shield bonus increases to +2. This shield bonus is lost if the fighter is immobilized or helpless.",
                    Helpers.getGuid("DefensiveWeaponTraining" + weaponTrainingName),
                    WeaponTraining.Icon,
                    FeatureGroup.CombatFeat,
                    weaponTraining.PrerequisiteFeature(),
                    WeaponTraining.PrerequisiteFeature(),
                    Helpers.Create<DefensiveWeaponTrainingLogic>(x => x.WeaponTraining = weaponTraining)
                ));

                features.Add(Helpers.CreateFeature(
                    "EffortlessDualWielding" + weaponTrainingName,
                    "Effortless Dual-Wielding (" + weaponTrainingDisplayName + ")",
                    "The fighter treats all one-handed weapons that belong to the associated weapon group as though they were light weapons when determining his penalties on attack rolls for fighting with two weapons.",
                    Helpers.getGuid("EffortlessDualWielding" + weaponTrainingName),
                    WeaponTraining.Icon,
                    FeatureGroup.CombatFeat,
                    WeaponTraining.PrerequisiteFeature(),
                    weaponTraining.PrerequisiteFeature(),
                    twoWeaponFightingBlueprintFeature.PrerequisiteFeature(),
                    Helpers.Create<EffortlessDualWieldingLogic>(x => x.WeaponTraining = weaponTraining)
                ));

                BlueprintFeatureSelection versatileTraining = Helpers.CreateFeatureSelection(
                    "VersatileTraining" + weaponTrainingName,
                    "Versatile Training (" + weaponTrainingDisplayName + ")",
                    "The fighter can use his base attack bonus in place of his ranks in two skills of his choice that are associated with the fighter weapon group he has chosen with this option (see below). The fighter need not be wielding an associated weapon to use this option. When using versatile training, the fighter substitutes his total base attack bonus (including his base attack bonus gained through levels in other classes) for his ranks in these skills, but adds the skill’s usual ability score modifier and any other bonuses or penalties that would modify those skills. Once the skills have been selected, they cannot be changed and the fighter can immediately retrain all of his skill ranks in the selected skills at no additional cost in money or time. In addition, the fighter adds all skills chosen with this option to his list of class skills. A fighter can choose this option up to two times. The Bluff and Intimidate skills are associated with all fighter weapon groups. The various fighter weapon groups also have the following associated skills: axes (Climb, Survival), bows (Knowledge [engineering], Perception), close (Sense Motive, Stealth), crossbows (Perception, Stealth), double (Acrobatics, Sense Motive), firearms (Perception, Sleight of Hand), flails (Acrobatics, Sleight of Hand), hammers (Diplomacy, Ride), heavy blades (Diplomacy, Ride), light blades (Diplomacy, Sleight of Hand), monk (Acrobatics, Escape Artist), natural (Climb, Fly, Swim), polearms (Diplomacy, Sense Motive), siege engines (Disable Device, Profession [driver]), spears (Handle Animal, Ride), and thrown (Acrobatics, Perception)",
                    Helpers.getGuid("VersatileTraining" + weaponTrainingName),
                    WeaponTraining.Icon,
                    FeatureGroup.CombatFeat,
                    WeaponTraining.PrerequisiteFeature(),
                    weaponTraining.PrerequisiteFeature());

                List<BlueprintFeature> versatileTrainingFeatures = new List<BlueprintFeature>();
                WeaponGroupAttackBonus weaponGroupAttackBonus = weaponTraining.GetComponent<WeaponGroupAttackBonus>();
                if (weaponGroupAttackBonus != null)
                {
                    foreach (var statType in _weaponTrainingToStatType[weaponGroupAttackBonus.WeaponGroup])
                    {
                        versatileTrainingFeatures.Add(Helpers.CreateFeature(
                            "VersatileTraining" + weaponTrainingName + statType,
                            "Versatile Training (" + statType + ")",
                            "The fighter treats all one-handed weapons that belong to the associated weapon group as though they were light weapons when determining his penalties on attack rolls for fighting with two weapons.",
                            Helpers.getGuid("VersatileTraining" + weaponTrainingDisplayName + statType),
                            WeaponTraining.Icon,
                            FeatureGroup.CombatFeat,
                            WeaponTraining.PrerequisiteFeature(),
                            weaponTraining.PrerequisiteFeature(),
                            Helpers.Create<SetSkillRankToBabLogic>(x => x.type = statType)));
                    }
                }

                versatileTraining.AddComponent(Helpers.Create<PrerequisiteMeetsPrerequisiteForAny>(x => x.Features = versatileTrainingFeatures.ToArray()));
                versatileTraining.SetFeatures(versatileTrainingFeatures);
                features.Add(versatileTraining);

                var fightersFinesse = Helpers.CreateFeature(
                    "FightersFinesse" + weaponTrainingName,
                    "Fighter’s Finesse (" + weaponTrainingDisplayName + ")",
                    "The fighter gains the benefits of the Weapon Finesse feat with all melee weapons that belong to the associated fighter weapon group (even if they cannot normally be used with Weapon Finesse). The fighter must have the Weapon Finesse feat before choosing this option.",
                    Helpers.getGuid("FightersFinesse" + weaponTrainingName),
                    WeaponTraining.Icon,
                    FeatureGroup.CombatFeat,
                    WeaponTraining.PrerequisiteFeature(),
                    weaponTraining.PrerequisiteFeature(),
                    weaponFinesse.PrerequisiteFeature(),
                    Helpers.Create<FightersFinesseLogic>(x => x.WeaponTraining = weaponTraining));
                features.Add(fightersFinesse);

                features.Add(Helpers.CreateFeature(
                    "TrainedGrace" + weaponTrainingName,
                    "Trained Grace (" + weaponTrainingDisplayName + ")",
                    "When the fighter uses Weapon Finesse to make a melee attack with a weapon, using his Dexterity modifier on attack rolls and his Strength modifier on damage rolls, he doubles his weapon training bonus on damage rolls. The fighter must have Weapon Finesse in order to choose this option.",
                    Helpers.getGuid("TrainedGrace" + weaponTrainingName),
                    WeaponTraining.Icon,
                    FeatureGroup.CombatFeat,
                    WeaponTraining.PrerequisiteFeature(),
                    weaponTraining.PrerequisiteFeature(),
                    weaponFinesse.PrerequisiteFeature(),
                    Helpers.Create<TrainedGraceLogic>(x =>
                    {
                        x.WeaponFighterGroupFeature = weaponTraining;
                        x.FightersFinesseFeature = fightersFinesse;
                    })));

                features.Add(Helpers.CreateFeature(
                    "FightersReflexes" + weaponTrainingName,
                    "Fighter’s Reflexes (" + weaponTrainingDisplayName + ")",
                    "The fighter applies his weapon training bonus to Reflex saving throws. He loses this bonus when he is flat-footed or denied his Dexterity bonus to AC.",
                    Helpers.getGuid("FightersReflexes" + weaponTrainingName),
                    WeaponTraining.Icon,
                    FeatureGroup.CombatFeat,
                    WeaponTraining.PrerequisiteFeature(),
                    weaponTraining.PrerequisiteFeature(),
                    Helpers.Create<FightersReflexesLogic>(x => x.WeaponTraining = weaponTraining)));

                features.Add(Helpers.CreateFeature(
                    "FightersTactics" + weaponTrainingName,
                    "Fighter’s Tactics (" + weaponTrainingDisplayName + ")",
                    "All of the fighter’s allies are treated as if they had the same teamwork feats as the fighter for the purpose of determining whether the fighter receives a bonus from his teamwork feats. His allies do not receive any bonuses from these feats unless they actually have the feats themselves. The allies’ positioning and actions must still meet the prerequisites listed in the teamwork feat for the fighter to receive the listed bonus.",
                    Helpers.getGuid("FightersTactics" + weaponTrainingName),
                    WeaponTraining.Icon,
                    FeatureGroup.CombatFeat,
                    WeaponTraining.PrerequisiteFeature(),
                    weaponTraining.PrerequisiteFeature(),
                    Helpers.Create<FightersTacticsLogic>(x => x.WeaponTraining = weaponTraining)));
            }

            features.AddRange(AddWarriorSpirit());

            return features;
        }


        private static void FillWeaponFocusAndWeaponTrainingDictionaries()
        {
            Dictionary<WeaponFighterGroup, List<WeaponCategory>> weaponFighterGroupToWeaponCategory = new Dictionary<WeaponFighterGroup, List<WeaponCategory>>();
            foreach (var weaponType in Resources.FindObjectsOfTypeAll<BlueprintWeaponType>())
            {
                _weaponCategoryToWeaponTraining[weaponType.Category] = weaponType.FighterGroup;
                if (weaponFighterGroupToWeaponCategory.TryGetValue(weaponType.FighterGroup, out var value))
                {
                    value.Add(weaponType.Category);
                }
                else
                {
                    List<WeaponCategory> categories = new List<WeaponCategory>();
                    categories.Add(weaponType.Category);
                    weaponFighterGroupToWeaponCategory[weaponType.FighterGroup] = categories;
                }
            }

            _weaponTrainingBluePrints.Add(library.Get<BlueprintFeature>("1b18d6a1297950f4bba9d121cfc735e9")); // WeaponTrainingAxes
            _weaponTrainingBluePrints.Add(library.Get<BlueprintFeature>("2a0ce0186af38ed419f47fce16f93c2a")); // WeaponTrainingHeavyBlades
            _weaponTrainingBluePrints.Add(library.Get<BlueprintFeature>("4923409590bdb604590e04da4253ab78")); // WeaponTrainingLightBlades
            _weaponTrainingBluePrints.Add(library.Get<BlueprintFeature>("e0401ecade57d4144978dbd714c4069f")); // WeaponTrainingBows
            _weaponTrainingBluePrints.Add(library.Get<BlueprintFeature>("9cdfc2a236ee6d349ad6d8a2170477d5")); // WeaponTrainingCrossbows
            _weaponTrainingBluePrints.Add(library.Get<BlueprintFeature>("a7a7ad500d4e2a847b450b85cbe68d65")); // WeaponTrainingDouble
            _weaponTrainingBluePrints.Add(library.Get<BlueprintFeature>("8bb8579622b823c4285d851274a009c3")); // WeaponTrainingHammers
            _weaponTrainingBluePrints.Add(library.Get<BlueprintFeature>("3ab76d4a8aa9e4c459add32139080206")); // WeaponTrainingNatural
            _weaponTrainingBluePrints.Add(library.Get<BlueprintFeature>("c062c6d16aecddc4ab67d9c783b2ad46")); // WeaponTrainingPolearms
            _weaponTrainingBluePrints.Add(library.Get<BlueprintFeature>("d5c04077fc063e44784384a00377b7cf")); // WeaponTrainingSpears
            _weaponTrainingBluePrints.Add(library.Get<BlueprintFeature>("bd75a95b36a3cd8459513ee1932c8c22")); // WeaponTrainingClose
            BlueprintFeature twoWeaponFeature = library.Get<BlueprintFeature>("88da2a5dfc505054f933bb81014e864f");
            _weaponTrainingBluePrints.Add(twoWeaponFeature);

            CreateWeaponTrainingForWeaponFighterGroup(WeaponFighterGroup.None, weaponFighterGroupToWeaponCategory[WeaponFighterGroup.None]);
            CreateWeaponTrainingForWeaponFighterGroup(WeaponFighterGroup.Thrown, weaponFighterGroupToWeaponCategory[WeaponFighterGroup.Thrown]);
            //CreateWeaponTrainingForWeaponFighterGroup(WeaponFighterGroup.Flails, weaponFighterGroupToWeaponCategory[WeaponFighterGroup.Flails]);
            CreateWeaponTrainingForWeaponFighterGroup(WeaponFighterGroup.Monk, weaponFighterGroupToWeaponCategory[WeaponFighterGroup.Monk]);


            _weaponTrainingToStatType[WeaponFighterGroup.Axes] = new List<StatType>() {StatType.SkillAthletics, StatType.SkillLoreNature, StatType.CheckBluff, StatType.CheckIntimidate};
            _weaponTrainingToStatType[WeaponFighterGroup.BladesHeavy] = new List<StatType>() {StatType.CheckDiplomacy, StatType.SkillAthletics, StatType.CheckBluff, StatType.CheckIntimidate};
            _weaponTrainingToStatType[WeaponFighterGroup.BladesLight] = new List<StatType>() {StatType.CheckDiplomacy, StatType.SkillThievery, StatType.CheckBluff, StatType.CheckIntimidate};
            _weaponTrainingToStatType[WeaponFighterGroup.Bows] = new List<StatType>() {StatType.SkillKnowledgeWorld, StatType.SkillPerception, StatType.CheckBluff, StatType.CheckIntimidate};
            _weaponTrainingToStatType[WeaponFighterGroup.Close] = new List<StatType>() {StatType.SkillStealth, StatType.CheckBluff, StatType.CheckIntimidate};
            _weaponTrainingToStatType[WeaponFighterGroup.Crossbows] = new List<StatType>() {StatType.SkillPerception, StatType.SkillStealth, StatType.CheckBluff, StatType.CheckIntimidate};
            _weaponTrainingToStatType[WeaponFighterGroup.Double] = new List<StatType>() {StatType.SkillMobility, StatType.CheckBluff, StatType.CheckIntimidate};
            _weaponTrainingToStatType[WeaponFighterGroup.Flails] = new List<StatType>() {StatType.SkillMobility, StatType.SkillThievery, StatType.CheckBluff, StatType.CheckIntimidate};
            _weaponTrainingToStatType[WeaponFighterGroup.Hammers] = new List<StatType>() {StatType.CheckDiplomacy, StatType.SkillAthletics, StatType.CheckBluff, StatType.CheckIntimidate};
            _weaponTrainingToStatType[WeaponFighterGroup.Monk] = new List<StatType>() {StatType.SkillMobility, StatType.CheckBluff, StatType.CheckIntimidate};
            _weaponTrainingToStatType[WeaponFighterGroup.Natural] = new List<StatType>() {StatType.SkillAthletics, StatType.SkillMobility, StatType.CheckBluff, StatType.CheckIntimidate};
            _weaponTrainingToStatType[WeaponFighterGroup.None] = new List<StatType>() {StatType.CheckBluff, StatType.CheckIntimidate};
            _weaponTrainingToStatType[WeaponFighterGroup.Polearms] = new List<StatType>() {StatType.CheckDiplomacy, StatType.CheckBluff, StatType.CheckIntimidate};
            _weaponTrainingToStatType[WeaponFighterGroup.Spears] = new List<StatType>() {StatType.SkillLoreNature, StatType.SkillAthletics, StatType.CheckBluff, StatType.CheckIntimidate};
            _weaponTrainingToStatType[WeaponFighterGroup.Thrown] = new List<StatType>() {StatType.SkillMobility, StatType.SkillPerception, StatType.CheckBluff, StatType.CheckIntimidate};


            Dictionary<WeaponFighterGroup, BlueprintFeature> WeaponTrainingByGroup = new Dictionary<WeaponFighterGroup, BlueprintFeature>();
            foreach (var weaponTrainingBluePrint in _weaponTrainingBluePrints)
            {
                WeaponGroupAttackBonus weaponGroupAttackBonus = weaponTrainingBluePrint.GetComponent<WeaponGroupAttackBonus>();
                if (weaponGroupAttackBonus != null)
                {
                    WeaponTrainingByGroup[weaponGroupAttackBonus.WeaponGroup] = weaponTrainingBluePrint;
                }
            }

            foreach (var weaponType in Resources.FindObjectsOfTypeAll<BlueprintWeaponType>())
            {
                _weaponCategoryToWeaponTraining[weaponType.Category] = weaponType.FighterGroup;
                if (_weaponTrainingToWeaponCategory.TryGetValue(WeaponTrainingByGroup[weaponType.FighterGroup], out var value))
                {
                    value.Add(weaponType.Category);
                }
                else
                {
                    HashSet<WeaponCategory> categories = new HashSet<WeaponCategory>();
                    categories.Add(weaponType.Category);
                    _weaponTrainingToWeaponCategory[WeaponTrainingByGroup[weaponType.FighterGroup]] = categories;
                }

                if (weaponType.IsTwoHanded)
                {
                    if (_weaponTrainingToWeaponCategory.TryGetValue(twoWeaponFeature, out value))
                    {
                        value.Add(weaponType.Category);
                    }
                    else
                    {
                        HashSet<WeaponCategory> categories = new HashSet<WeaponCategory>();
                        categories.Add(weaponType.Category);
                        _weaponTrainingToWeaponCategory[twoWeaponFeature] = categories;
                    }
                }
            }
        }

        private static BlueprintFeature CreateWeaponTrainingForWeaponFighterGroup(WeaponFighterGroup fighterGroup, List<WeaponCategory> weaponCategories)
        {
            string newAssetID = Helpers.getGuid("WeaponFighterGroup" + fighterGroup);

            string affectedWeapons = String.Join(", ", new List<WeaponCategory>(weaponCategories).Distinct());
            var oldGroup = library.Get<BlueprintFeature>("1b18d6a1297950f4bba9d121cfc735e9");

            var group = Helpers.CreateFeature("WeaponTraining" + fighterGroup.ToString(),
                "Weapon Training (" + fighterGroup.ToString() + ")",
                "Whenever a fighter attacks with a weapon from this group, he gains a +1 bonus on attack and damage rolls.\nThis group includes " + affectedWeapons + ".",
                newAssetID,
                oldGroup.Icon,
                FeatureGroup.WeaponTraining,
                Helpers.Create<WeaponGroupAttackBonus>(x =>
                {
                    x.AttackBonus = 1;
                    x.WeaponGroup = fighterGroup;
                }),
                Helpers.Create<WeaponGroupDamageBonus>(x =>
                {
                    x.DamageBonus = 1;
                    x.WeaponGroup = fighterGroup;
                })
            );
            group.Ranks = 5;
            _weaponTrainingBluePrints.Add(group);
            WeaponTraining.SetFeatures(WeaponTraining.AllFeatures.AddToArray(group));

            return group;
        }

        private static void CreateAbilityMoveActions()
        {
            BlueprintAbility persuasionUseAbility = library.Get<BlueprintAbility>("7d2233c3b7a0b984ba058a83b736e6ac");
            BlueprintAbility persuasionUseAbilityMove = Main.library.CopyAndAdd(persuasionUseAbility, "DemoralizeMove", "3af0e87e92a348daad1ec1934a7f811c");

            persuasionUseAbilityMove.ActionType = UnitCommand.CommandType.Move;
            Traverse traverse = Traverse.Create(persuasionUseAbilityMove);
            traverse.Field("m_AssetGuid").SetValue("3af0e87e92a348daad1ec1934a7f811c");

            BlueprintAbility dazzlingDisplayAction = Main.library.Get<BlueprintAbility>("5f3126d4120b2b244a95cb2ec23d69fb");
            BlueprintAbility dazzlingDisplayActionMove = Main.library.CopyAndAdd(dazzlingDisplayAction, "DazzlingDisplayAction", "545b0811f3c24a518c02f749509623c4");

            dazzlingDisplayActionMove.ActionType = UnitCommand.CommandType.Move;
            traverse = Traverse.Create(dazzlingDisplayActionMove);
            traverse.Field("m_AssetGuid").SetValue("545b0811f3c24a518c02f749509623c4");
        }

        public static int GetWeaponTrainingRank(UnitDescriptor owner, BlueprintFeature feature)
        {
            return owner.Progression.Features.GetRank(feature);
        }

        public static int GetWeaponEnhancementBonus(UnitDescriptor owner)
        {
            int enhancementBonusPrimary = GetWeaponEnhancementBonus(owner.Body.PrimaryHand.MaybeWeapon?.Enchantments);

            int enhancementBonusSecondary = GetWeaponEnhancementBonus(owner.Body.SecondaryHand.MaybeWeapon?.Enchantments);

            return enhancementBonusPrimary < enhancementBonusSecondary ? enhancementBonusSecondary : enhancementBonusPrimary;
        }

        private static int GetWeaponEnhancementBonus(List<ItemEnchantment> enchantments)
        {
            foreach (ItemEnchantment enchantment in enchantments)
            {
                if (enchantment.Blueprint.GetComponent<WeaponEnhancementBonus>() != null)
                {
                    return enchantment.Blueprint.GetComponent<WeaponEnhancementBonus>().EnhancementBonus;
                }
            }

            return 0;
        }

        public static bool WieldsWeaponFromFighterGroup(UnitDescriptor owner, BlueprintFeature weaponTraining)
        {
            WeaponGroupAttackBonus weaponGroupAttackBonus = weaponTraining.GetComponent<WeaponGroupAttackBonus>();
            if (weaponGroupAttackBonus != null)
            {
                return (owner.Body.PrimaryHand.MaybeWeapon != null && owner.Body.PrimaryHand.MaybeWeapon.Blueprint.FighterGroup == weaponGroupAttackBonus.WeaponGroup) ||
                       (owner.Body.SecondaryHand.MaybeWeapon != null && owner.Body.SecondaryHand.MaybeWeapon.Blueprint.FighterGroup == weaponGroupAttackBonus.WeaponGroup);
            }

            WeaponParametersAttackBonus weaponParametersAttackBonus = weaponTraining.GetComponent<WeaponParametersAttackBonus>();
            if (weaponParametersAttackBonus != null)
            {
                return weaponParametersAttackBonus.OnlyTwoHanded && owner.Body.PrimaryHand.MaybeWeapon != null && owner.Body.PrimaryHand.MaybeWeapon.Blueprint.IsTwoHanded;
            }

            return false;
        }

        private static List<BlueprintFeature> AddWarriorSpirit()
        {
            BlueprintItemEnchantment[] defaultEnchantments = new BlueprintItemEnchantment[]
            {
                library.Get<BlueprintItemEnchantment>("d704f90f54f813043a525f304f6c0050"),
                library.Get<BlueprintItemEnchantment>("9e9bab3020ec5f64499e007880b37e52"),
                library.Get<BlueprintItemEnchantment>("d072b841ba0668846adeb007f623bd6c"),
                library.Get<BlueprintItemEnchantment>("6a6a0901d799ceb49b33d4851ff72132"),
                library.Get<BlueprintItemEnchantment>("746ee366e50611146821d61e391edf16")
            };

            BlueprintActivatableAbility[] MagusFacts = new BlueprintActivatableAbility[]
            {
                library.Get<BlueprintActivatableAbility>("05b7cbe45b1444a4f8bf4570fb2c0208"),
                library.Get<BlueprintActivatableAbility>("b338e43a8f81a2f43a73a4ae676353a5"),
                library.Get<BlueprintActivatableAbility>("a3a9e9a2f909cd74e9aee7788a7ec0c6"),
                library.Get<BlueprintActivatableAbility>("24fe1f546e07987418557837b0e0f8f5"),
                library.Get<BlueprintActivatableAbility>("85742dd6788c6914f96ddc4628b23932")
            };
            BlueprintActivatableAbility[] facts = new BlueprintActivatableAbility[MagusFacts.Length];

            for (int i = 0; i < MagusFacts.Length; i++)
            {
                BlueprintActivatableAbility magusAbility = MagusFacts[i];
                BlueprintActivatableAbility temp = library.CopyAndAdd(magusAbility, magusAbility.Name, Helpers.getGuid(magusAbility.AssetGuid + "WarriorSpirit"));
                temp.AddComponent(Helpers.Create<ActivatableAbilityGroupSizeRestriction>(x =>
                {
                    x.EnchantPool = EnchantPoolType.ArcanePool;
                    x.ActivatableAbility = temp;
                }));
                facts[i] = temp;
            }

            ContextDurationValue contextDurationValue = Helpers.CreateContextDuration(rate: DurationRate.Minutes, bonus: 1);

            BlueprintAbility arcaneWeaponSwitchAbility = library.Get<BlueprintAbility>("3c89dfc82c2a3f646808ea250eb91b91");
            BlueprintAbilityResource arcanePoolResourse = library.Get<BlueprintAbilityResource>("effc3e386331f864e9e06d19dc218b37");


            var divineFavor = library.Get<BlueprintAbility>("9d5d2d3ffdd73c648af3eb3e585b1113");

            List<BlueprintFeature> features = new List<BlueprintFeature>();

            foreach (var weaponTraining in _weaponTrainingBluePrints)
            {
                string weaponTrainingName = getWeaponTrainingName(weaponTraining, true);
                string weaponTrainingDisplayName = getWeaponTrainingName(weaponTraining);

                BlueprintAbilityResource resource = Helpers.Create<WarriorSpiritPoolLogic>(x => x.WeaponTraining = weaponTraining);
                resource.name = "WarriorSpiritResource" + weaponTrainingName;
                resource.LocalizedName = Helpers.CreateString("WarriorSpiritResource.Name", "Warrior Spirit Resource (" + weaponTrainingDisplayName + ")");
                resource.LocalizedDescription = Helpers.CreateString("WarriorSpiritResource.Name", "Each day, he designates one such weapon and gains a number of points of spiritual energy equal to 1 + his weapon training bonus.");
                resource.SetIcon(arcaneWeaponSwitchAbility.Icon);

                Traverse traverseOne = Traverse.Create(arcanePoolResourse);
                Traverse traverseTwo = Traverse.Create(resource);
                traverseTwo.Field("m_MaxAmount").SetValue(traverseOne.Field("m_MaxAmount").GetValue());

                Main.library.AddAsset(resource, Helpers.getGuid("WarriorSpiritResource" + weaponTrainingName));

                ContextActionWeaponEnchantPoolWarriorSpirit contextActionWeaponEnchantPool = Helpers.Create<ContextActionWeaponEnchantPoolWarriorSpirit>();
                contextActionWeaponEnchantPool.DurationValue = contextDurationValue;
                contextActionWeaponEnchantPool.EnchantPool = EnchantPoolType.ArcanePool;
                contextActionWeaponEnchantPool.Group = ActivatableAbilityGroup.ArcaneWeaponProperty;
                contextActionWeaponEnchantPool.DefaultEnchantments = defaultEnchantments;
                contextActionWeaponEnchantPool.Feature = weaponTraining;


                BlueprintAbility warriorSpiritSwitchAbility = Helpers.CreateAbility(
                    "WarriorSpiritSwitchAbility" + weaponTrainingName,
                    "Warrior Spirit " + getWeaponTrainingName(weaponTraining),
                    "The fighter can forge a spiritual bond with a weapon that belongs to the associated weapon group, allowing him to unlock the weapon’s potential. Each day, he designates one such weapon and gains a number of points of spiritual energy equal to 1 + his weapon training bonus. While wielding this weapon, he can spend 1 point of spiritual energy to grant the weapon an enhancement bonus equal to his weapon training bonus. Enhancement bonuses gained by this advanced weapon training option stack with those of the weapon, to a maximum of +5. The fighter can also imbue the weapon with any one weapon special ability with an equivalent enhancement bonus less than or equal to his maximum bonus by reducing the granted enhancement bonus by the amount of the equivalent enhancement bonus. The item must have an enhancement bonus of at least +1 (from the item itself or from warrior spirit) to gain a weapon special ability. In either case, these bonuses last for 1 minute.",
                    Helpers.getGuid("WarriorSpiritSwitchAbility" + weaponTrainingName),
                    arcaneWeaponSwitchAbility.Icon,
                    AbilityType.Supernatural,
                    arcaneWeaponSwitchAbility.ActionType,
                    arcaneWeaponSwitchAbility.Range,
                    "1 min",
                    arcaneWeaponSwitchAbility.LocalizedSavingThrow,
                    divineFavor.GetComponent<AbilitySpawnFx>(), // TODO!!!!!
                    Helpers.CreateContextRankConfig(
                        ContextRankBaseValueType.FeatureRank,
                        ContextRankProgression.AsIs,
                        AbilityRankType.Default,
                        feature: weaponTraining),
                    Helpers.CreateRunActions(contextActionWeaponEnchantPool),
                    Helpers.Create<AbilityResourceLogic>(x =>
                    {
                        x.Amount = 1;
                        x.RequiredResource = resource;
                        x.IsSpendResource = true;
                        x.CostIsCustom = false;
                    })
                );

                features.Add(Helpers.CreateFeature(
                    "WarriorSpiritFeature" + weaponTrainingName,
                    "Warrior Spirit (" + weaponTrainingDisplayName + ")",
                    "The fighter can forge a spiritual bond with a weapon that belongs to the associated weapon group, allowing him to unlock the weapon’s potential. Each day, he designates one such weapon and gains a number of points of spiritual energy equal to 1 + his weapon training bonus. While wielding this weapon, he can spend 1 point of spiritual energy to grant the weapon an enhancement bonus equal to his weapon training bonus. Enhancement bonuses gained by this advanced weapon training option stack with those of the weapon, to a maximum of +5. The fighter can also imbue the weapon with any one weapon special ability with an equivalent enhancement bonus less than or equal to his maximum bonus by reducing the granted enhancement bonus by the amount of the equivalent enhancement bonus. The item must have an enhancement bonus of at least +1 (from the item itself or from warrior spirit) to gain a weapon special ability. In either case, these bonuses last for 1 minute.",
                    Helpers.getGuid("WarriorSpiritFeature" + weaponTrainingName),
                    arcaneWeaponSwitchAbility.Icon,
                    FeatureGroup.CombatFeat,
                    warriorSpiritSwitchAbility.CreateAddFact(),
                    resource.CreateAddAbilityResource(),
                    Helpers.CreateAddFacts(facts),
                    weaponTraining.PrerequisiteFeature(),
                    Helpers.Create<IncreaseActivatableAbilityGroupSizeBaseOnFeatureRanks>(x =>
                    {
                        x.Group = ActivatableAbilityGroup.ArcaneWeaponProperty;
                        x.Feature = weaponTraining;
                    })
                ));
            }

            return features;
        }

        private static String getWeaponTrainingName(BlueprintFeature weaponTraining, bool removeBlanks = false)
        {
            String toReturn = "";
            WeaponGroupAttackBonus weaponGroupAttackBonus = weaponTraining.GetComponent<WeaponGroupAttackBonus>();

            if (weaponGroupAttackBonus != null)
            {
                switch (weaponGroupAttackBonus.WeaponGroup)
                {
                    case WeaponFighterGroup.Axes:
                        toReturn = "Axes";
                        break;
                    case WeaponFighterGroup.BladesHeavy:
                        toReturn = "Blades, Heavy";
                        break;
                    case WeaponFighterGroup.BladesLight:
                        toReturn = "Blades, Light";
                        break;
                    case WeaponFighterGroup.Bows:
                        toReturn = "Bows";
                        break;
                    case WeaponFighterGroup.Close:
                        toReturn = "Close";
                        break;
                    case WeaponFighterGroup.Crossbows:
                        toReturn = "Crossbows";
                        break;
                    case WeaponFighterGroup.Double:
                        toReturn = "Double";
                        break;
                    case WeaponFighterGroup.Flails:
                        toReturn = "Flails";
                        break;
                    case WeaponFighterGroup.Hammers:
                        toReturn = "Hammers";
                        break;
                    case WeaponFighterGroup.Monk:
                        toReturn = "Monk";
                        break;
                    case WeaponFighterGroup.Natural:
                        toReturn = "Natural";
                        break;
                    case WeaponFighterGroup.None:
                        toReturn = "Unknown";
                        break;
                    case WeaponFighterGroup.Polearms:
                        toReturn = "Polearms";
                        break;
                    case WeaponFighterGroup.Spears:
                        toReturn = "Spears";
                        break;
                    case WeaponFighterGroup.Thrown:
                        toReturn = "Thrown";
                        break;
                }
            }
            else
            {
                WeaponParametersAttackBonus weaponParametersAttackBonus = weaponTraining.GetComponent<WeaponParametersAttackBonus>();
                if (weaponParametersAttackBonus.OnlyTwoHanded)
                {
                    toReturn = "Two-Handed";
                }
            }

            if (removeBlanks)
            {
                toReturn = toReturn.Trim();
            }

            return toReturn;
        }
    }
}