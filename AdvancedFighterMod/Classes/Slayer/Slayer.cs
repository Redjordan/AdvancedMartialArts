using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedMartialArts.Feats.CombatFeats.FeatLogic;
using AdvancedMartialArts.Classes.Slayer.Logic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;

namespace AdvancedMartialArts.Classes.Slayer
{
    public static class Slayer
    {
        private static LibraryScriptableObject library => Main.library;

        private static BlueprintCharacterClass slayer;
        internal static BlueprintCharacterClass[] slayerArray;

        public static void Load()
        {
            if (slayer != null) return;
            BlueprintCharacterClass rogue = library.Get<BlueprintCharacterClass>("299aa766dee3cbf4790da4efb8c72484");
            BlueprintCharacterClass ranger = library.Get<BlueprintCharacterClass>("cda0615668a6df14eb36ba19ee881af6");

            slayer = Helpers.Create<BlueprintCharacterClass>();
            slayerArray = new BlueprintCharacterClass[] {slayer};
            slayer.name = "SlayerClass";
            library.AddAsset(slayer, Helpers.getGuid("SlayerClass"));

            slayer.LocalizedName = Helpers.CreateString("Slayer.Name", "Slayer");
            slayer.LocalizedDescription = Helpers.CreateString("Slayer.Description", "Skilled at tracking down targets, slayers are consummate hunters, living for the chase and the deadly stroke that brings it to a close. Slayers spend most of their time honing their weapon skills, studying the habits and anatomy of foes, and practicing combat maneuvers.");
            slayer.m_Icon = ranger.Icon;
            slayer.SkillPoints = 4;
            slayer.HitDie = DiceType.D10;
            slayer.BaseAttackBonus = ranger.BaseAttackBonus;
            slayer.FortitudeSave = ranger.FortitudeSave;
            slayer.ReflexSave = ranger.ReflexSave;
            slayer.WillSave = ranger.WillSave;
            slayer.ClassSkills = new StatType[]
            {
                StatType.SkillMobility,
                StatType.SkillAthletics,
                StatType.SkillPersuasion,
                StatType.SkillKnowledgeWorld,
                StatType.SkillPerception,
                StatType.SkillStealth
            };

            slayer.StartingGold = ranger.StartingGold; // all classes start with 411.
            slayer.PrimaryColor = ranger.PrimaryColor;
            slayer.SecondaryColor = ranger.SecondaryColor;

            slayer.RecommendedAttributes = new StatType[] {StatType.Strength, StatType.Dexterity};
            slayer.NotRecommendedAttributes = new StatType[] {StatType.Charisma};

            slayer.StartingItems = ranger.StartingItems;
            slayer.EquipmentEntities = ranger.EquipmentEntities;
            slayer.MaleEquipmentEntities = ranger.MaleEquipmentEntities;
            slayer.FemaleEquipmentEntities = ranger.FemaleEquipmentEntities;

            var progression = Helpers.CreateProgression("SlayerProgression",
                slayer.Name,
                slayer.Description,
                Helpers.getGuid("SlayerProgression"),
                slayer.Icon,
                FeatureGroup.None);
            progression.Classes = slayerArray;
            var entries = new List<LevelEntry>();


            var proficiencies = library.CopyAndAdd<BlueprintFeature>(
                "c5e479367d07d62428f2fe92f39c0341", // RangerProficiencies
                "SlayerProficiencies",
                Helpers.getGuid("SlayerProficiencies"));
            proficiencies.SetName("Slayer Proficiencies");
            proficiencies.SetDescription("A slayer is proficient with all simple and martial weapons, as well as with light armor, medium armor, and shields (except tower shields).");

            CreateStudiedTarget();
            BlueprintFeatureSelection slayerTalents = getSlayerTalentSelection();
            BlueprintFeature sneakAttack = library.Get<BlueprintFeature>("9b9eac6709e1c084cb18c3a366e0ec87");


            BlueprintFeature studiedTargetImprovement = library.Get<BlueprintFeature>(Helpers.getGuid("StudiedTargetProgression"));
            entries.Add(Helpers.LevelEntry(1,
                proficiencies,
                library.Get<BlueprintFeature>(Helpers.getGuid("StudiedTargetMove")),
                studiedTargetImprovement,
                library.Get<BlueprintFeature>("d3e6275cfa6e7a04b9213b7b292a011c"), // ray calculate feature
                library.Get<BlueprintFeature>("62ef1cdb90f1d654d996556669caf7fa") // touch calculate feature
            ));
            entries.Add(Helpers.LevelEntry(2, slayerTalents));
            entries.Add(Helpers.LevelEntry(3, sneakAttack, library.Get<BlueprintFeature>(Helpers.getGuid("setStudiedTargetOnSneakAttack"))));
            entries.Add(Helpers.LevelEntry(4, slayerTalents));

            entries.Add(Helpers.LevelEntry(5,studiedTargetImprovement));
            entries.Add(Helpers.LevelEntry(6, slayerTalents, sneakAttack));
            entries.Add(Helpers.LevelEntry(7, library.Get<BlueprintFeature>(Helpers.getGuid("StudiedTargetSwift"))));
            entries.Add(Helpers.LevelEntry(8, slayerTalents));
            entries.Add(Helpers.LevelEntry(9, sneakAttack));
            entries.Add(Helpers.LevelEntry(10, 
                studiedTargetImprovement, 
                slayerTalents,
                library.Get<BlueprintFeature>("a33b99f95322d6741af83e9381b2391c"))); // Advanced Talents
            entries.Add(Helpers.LevelEntry(12, slayerTalents, sneakAttack));
            entries.Add(Helpers.LevelEntry(14, slayerTalents, library.Get<BlueprintFeature>("385260ca07d5f1b4e907ba22a02944fc"))); // Quarry
            entries.Add(Helpers.LevelEntry(15, studiedTargetImprovement, sneakAttack));
            entries.Add(Helpers.LevelEntry(16, slayerTalents));
            entries.Add(Helpers.LevelEntry(18, slayerTalents, sneakAttack));
            entries.Add(Helpers.LevelEntry(19, library.Get<BlueprintFeature>("25e009b7e53f86141adee3a1213af5af"))); // ImprovedQuarry
            entries.Add(Helpers.LevelEntry(20, studiedTargetImprovement, slayerTalents));

            progression.LevelEntries = entries.ToArray();
            slayer.Progression = progression;
            slayer.RegisterClass();
            addFavoredClassBonus(slayer);
        }


        private static BlueprintFeatureSelection getSlayerTalentSelection()
        {
            BlueprintFeatureSelection rogueTalents = library.Get<BlueprintFeatureSelection>("c074a5d615200494b8f2a9c845799d93");



            BlueprintFeatureSelection slayerTalents = Helpers.CreateFeatureSelection(
                "rogueTalents",
                "Slayer Talents",
                "As a slayer gains experience, he learns a number of talents that aid him and confound his foes. Starting at 2nd level and every 2 levels thereafter, a slayer gains one slayer talent. Unless otherwise noted, a slayer cannot select an individual talent more than once.",
                Helpers.getGuid("rogueTalents"),
                rogueTalents.Icon,
                FeatureGroup.RogueTalent,
                Helpers.PrerequisiteClassLevel(slayer, 1));

            List<BlueprintFeature> features = new List<BlueprintFeature>(rogueTalents.AllFeatures);
            var goBackFeature = library.Get<BlueprintFeature>(Helpers.getGuid("0GoBack"));

            BlueprintFeatureSelection slayerStyleLevelTwo = library.CopyAndAdd<BlueprintFeatureSelection>("c6d0da9124735a44f93ac31df803b9a9", "Ranger Style", Helpers.getGuid("SlayerStyleLevelTwo"));
            slayerStyleLevelTwo.AddComponent(Helpers.PrerequisiteClassLevel(slayer, 2));
            slayerStyleLevelTwo.AddComponent(slayerStyleLevelTwo.PrerequisiteNoFeature());
            slayerStyleLevelTwo.SetFeatures(slayerStyleLevelTwo.AllFeatures.AddToArray(goBackFeature));
            features.Add(slayerStyleLevelTwo);


            BlueprintFeatureSelection slayerStyleLevelSix = library.CopyAndAdd<BlueprintFeatureSelection>("61f82ba786fe05643beb3cd3910233a8", "Ranger Style", Helpers.getGuid("SlayerStyleLevelSix"));
            slayerStyleLevelSix.AddComponent(Helpers.PrerequisiteClassLevel(slayer, 6));
            slayerStyleLevelSix.AddComponent(slayerStyleLevelSix.PrerequisiteNoFeature());
            slayerStyleLevelSix.SetFeatures(slayerStyleLevelSix.AllFeatures.AddToArray(goBackFeature));
            features.Add(slayerStyleLevelSix);

            BlueprintFeatureSelection slayerStyleLevelTen = library.CopyAndAdd<BlueprintFeatureSelection>("78177315fc63b474ea3cbb8df38fafcd", "Ranger Style", Helpers.getGuid("SlayerStyleLevelTen"));
            slayerStyleLevelTen.AddComponent(Helpers.PrerequisiteClassLevel(slayer, 10));
            slayerStyleLevelTen.AddComponent(slayerStyleLevelTen.PrerequisiteNoFeature());
            slayerStyleLevelTen.SetFeatures(slayerStyleLevelTen.AllFeatures.AddToArray(goBackFeature));
            features.Add(slayerStyleLevelTen);


            slayerTalents.SetFeatures(features);

            return slayerTalents;
        }

        private static void CreateStudiedTarget()
        {
            var detectMagic = library.Get<BlueprintFeature>("ee0b69e90bac14446a4cf9a050f87f2e");


            BlueprintAbility studiedTarget = Helpers.CreateAbility(
                "StudiedTargetAbilityMove",
                "Studied Target (Move-action)",
                "A slayer can study an opponent he can see as a move action. The slayer then gains a +1 bonus on Bluff, Knowledge, Perception, Sense Motive, and Survival checks attempted against that opponent, and a +1 bonus on weapon attack and damage rolls against it. The DCs of slayer class abilities against that opponent increase by 1. A slayer can only maintain these bonuses against one opponent at a time; these bonuses remain in effect until either the opponent is dead or the slayer studies a new target.\n" +
                "If a slayer deals sneak attack damage to a target, he can study that target as an immediate action, allowing him to apply his studied target bonuses against that target(including to the normal weapon damage roll).\n" +
                "At 5th, 10th, 15th, and 20th levels, the bonuses on weapon attack rolls, damage rolls, and skill checks and to slayer DCs against a studied target increase by 1.In addition, at each such interval, the slayer is able to maintain these bonuses against an additional studied target at the same time.The slayer may discard this connection to a studied target as a free action, allowing him to study another target in its place.\n" +
                "At 7th level, a slayer can study an opponent as a move or swift action.",
                Helpers.getGuid("StudiedTargetAbilityMove"),
                detectMagic.Icon,
                AbilityType.Extraordinary,
                UnitCommand.CommandType.Move,
                AbilityRange.Unlimited,
                "until either the opponent is dead or the slayer studies a new target",
                "none",
                Helpers.CreateRunActions(Helpers.Create<StudiedTargetContextActionApplyBuff>())
                );
            studiedTarget.CanTargetEnemies = true;
            studiedTarget.EffectOnEnemy = AbilityEffectOnUnit.None;

            BlueprintFeature feature = Helpers.CreateFeature(
                "StudiedTargetMove", "Studied Target (Move-action)",
                "A slayer can study an opponent he can see as a move action. The slayer then gains a +1 bonus on Bluff, Knowledge, Perception, Sense Motive, and Survival checks attempted against that opponent, and a +1 bonus on weapon attack and damage rolls against it. The DCs of slayer class abilities against that opponent increase by 1. A slayer can only maintain these bonuses against one opponent at a time; these bonuses remain in effect until either the opponent is dead or the slayer studies a new target.\n" +
                "If a slayer deals sneak attack damage to a target, he can study that target as an immediate action, allowing him to apply his studied target bonuses against that target(including to the normal weapon damage roll).\n" +
                "At 5th, 10th, 15th, and 20th levels, the bonuses on weapon attack rolls, damage rolls, and skill checks and to slayer DCs against a studied target increase by 1.In addition, at each such interval, the slayer is able to maintain these bonuses against an additional studied target at the same time.The slayer may discard this connection to a studied target as a free action, allowing him to study another target in its place.\n" +
                "At 7th level, a slayer can study an opponent as a move or swift action.",
                Helpers.getGuid("StudiedTargetMove"),
                detectMagic.Icon,
                FeatureGroup.FavoriteEnemy,
                Helpers.CreateAddFacts(studiedTarget)
                );
            feature.HideInCharacterSheetAndLevelUp = true;

            BlueprintAbility studiedTargetSwift = Helpers.CreateAbility(
                "StudiedTargetAbilitySwift",
                "Studied Target (Swift-action)",
                "A slayer can study an opponent he can see as a move action. The slayer then gains a +1 bonus on Bluff, Knowledge, Perception, Sense Motive, and Survival checks attempted against that opponent, and a +1 bonus on weapon attack and damage rolls against it. The DCs of slayer class abilities against that opponent increase by 1. A slayer can only maintain these bonuses against one opponent at a time; these bonuses remain in effect until either the opponent is dead or the slayer studies a new target.\n" +
                "If a slayer deals sneak attack damage to a target, he can study that target as an immediate action, allowing him to apply his studied target bonuses against that target(including to the normal weapon damage roll).\n" +
                "At 5th, 10th, 15th, and 20th levels, the bonuses on weapon attack rolls, damage rolls, and skill checks and to slayer DCs against a studied target increase by 1.In addition, at each such interval, the slayer is able to maintain these bonuses against an additional studied target at the same time.The slayer may discard this connection to a studied target as a free action, allowing him to study another target in its place.\n" +
                "At 7th level, a slayer can study an opponent as a move or swift action.",
                Helpers.getGuid("StudiedTargetAbilitySwift"),
                detectMagic.Icon,
                AbilityType.Extraordinary,
                UnitCommand.CommandType.Swift,
                AbilityRange.Unlimited,
                "until either the opponent is dead or the slayer studies a new target",
                "none",
                Helpers.CreateRunActions(Helpers.Create<StudiedTargetContextActionApplyBuff>())
                );
            studiedTarget.CanTargetEnemies = true;
            studiedTarget.EffectOnEnemy = AbilityEffectOnUnit.None;

            BlueprintFeature featureSwift = Helpers.CreateFeature(
                "StudiedTargetSwift", "Studied Target (Swift-action)",
                "At 7th level, a slayer can study an opponent as a move or swift action.",
                Helpers.getGuid("StudiedTargetSwift"),
                detectMagic.Icon,
                FeatureGroup.FavoriteEnemy,
                Helpers.CreateAddFacts(studiedTarget)
                );
            featureSwift.HideInCharacterSheetAndLevelUp = true;

            BlueprintFeature progressionBlueprintFeature = Helpers.CreateFeature(
                "StudiedTargetProgression",
                "StudiedTarget" ,
                "A slayer can study an opponent he can see as a move action. The slayer then gains a +1 bonus on Bluff, Knowledge, Perception, Sense Motive, and Survival checks attempted against that opponent, and a +1 bonus on weapon attack and damage rolls against it. The DCs of slayer class abilities against that opponent increase by 1. A slayer can only maintain these bonuses against one opponent at a time; these bonuses remain in effect until either the opponent is dead or the slayer studies a new target.\n" +
                "If a slayer deals sneak attack damage to a target, he can study that target as an immediate action, allowing him to apply his studied target bonuses against that target(including to the normal weapon damage roll).\n" +
                "At 5th, 10th, 15th, and 20th levels, the bonuses on weapon attack rolls, damage rolls, and skill checks and to slayer DCs against a studied target increase by 1.In addition, at each such interval, the slayer is able to maintain these bonuses against an additional studied target at the same time.The slayer may discard this connection to a studied target as a free action, allowing him to study another target in its place.\n" +
                "At 7th level, a slayer can study an opponent as a move or swift action.",
                Helpers.getGuid("StudiedTargetProgression"),
                detectMagic.Icon,
                FeatureGroup.None,
                Helpers.Create<SetStudiedTargetFact>()
                );
            progressionBlueprintFeature.Ranks = 5;
            progressionBlueprintFeature.ReapplyOnLevelUp = true;

            BlueprintFeature setStudiedTargetOnSneakAttack = Helpers.CreateFeature(
                "setStudiedTargetOnSneakAttack", "Studied Target (Sneak Attack)",
                "If a slayer deals sneak attack damage to a target, he studies that target as an immediate action.",
                Helpers.getGuid("setStudiedTargetOnSneakAttack"),
                detectMagic.Icon,
                FeatureGroup.FavoriteEnemy,
                Helpers.Create<SetStudiedTargetOnSneakAttack>()
            );
            setStudiedTargetOnSneakAttack.HideInCharacterSheetAndLevelUp = true;
        }

        private static void addFavoredClassBonus(BlueprintCharacterClass favoredClass)
        {
            BlueprintProgression progression = library.TryGet<BlueprintProgression>(Helpers.MergeIds(Helpers.fighterClass.AssetGuid, "081651146ada4d0a88f6e9190ac6b01a"));
            BlueprintFeatureSelection FavoredClassFeature = library.TryGet<BlueprintFeatureSelection>("bc4c271ef0954eceb808d84978c500f7");

            if(progression != null && FavoredClassFeature != null)
            {
                BlueprintProgression newProgression = library.CopyAndAdd(progression, $"FavoredClass{favoredClass.name}Progression", Helpers.getGuid($"FavoredClass{favoredClass.name}Progression"));
                newProgression.SetName($"Favored Class — {favoredClass.Name}");
                newProgression.SetIcon(favoredClass.Icon);
                newProgression.Classes = new BlueprintCharacterClass[]{favoredClass};
                newProgression.ExclusiveProgression = favoredClass;

                FavoredClassFeature.SetFeatures(FavoredClassFeature.AllFeatures.AddToArray(newProgression));
            }

        }
    }
}