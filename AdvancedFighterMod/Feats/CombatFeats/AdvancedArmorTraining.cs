using Harmony12;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.RuleSystem;
using System.Collections.Generic;
using AdvancedMartialArts.Feats.CombatFeats.FeatLogic;
using AdvancedMartialArts.HelperClasses;
using Kingmaker.Blueprints.Classes.Prerequisites;
using UnityEngine;

namespace AdvancedMartialArts.Feats.CombatFeats
{
    public static class AdvancedArmorTraining
    {
        private static LibraryScriptableObject library => Main.library;

        public static void Load()
        {
            Main.SafeLoad(CreateAdvancedArmorTraining, "Advanced Armor Training");
        }

        private static void CreateAdvancedArmorTraining()
        {
            addSteelHeadbutt();
            BlueprintFeature armorTraining = library.Get<BlueprintFeature>("3c380607706f209499d951b29d3c44f3");

            BlueprintCharacterClass fighter = Helpers.fighterClass;

            BlueprintFeatureSelection advancedArmorTraining = Helpers.CreateFeatureSelection("AdvancedArmorTraining", "Advanced Armor Training",
                "Advanced Armor Training: Beginning at 7th level, instead of increasing the benefits provided by armor training(reducing his armor’s check penalty by 1 and increasing its maximum Dexterity bonus by 1), a fighter can choose an advanced armor training option(see Advanced Armor Training below).If the fighter does so, he still gains the ability to move at his normal speed while wearing medium armor at 3rd level, and while wearing heavy armor at 7th level.",
                "c8584adbde3d4cc58fdf217b5a0082f6",
                armorTraining.Icon,
                FeatureGroup.CombatFeat,
                Helpers.PrerequisiteFeature(armorTraining),
                Helpers.PrerequisiteClassLevel(fighter, 3)
                );

            List<BlueprintFeature> features = getAdvancedArmorTrainings(armorTraining, fighter);

            advancedArmorTraining.SetFeatures(features);
            library.AddCombatFeats(advancedArmorTraining);


            BlueprintFeatureSelection armorTrainingWithAdvandedArmorTraining = Helpers.CreateFeatureSelection("ArmorTrainingSelection", "Armor Training",
                "Starting at 3rd level, a fighter learns to be more maneuverable while wearing armor. Whenever he is wearing armor, he reduces the armor check penalty by 1 (to a minimum of 0) and increases the maximum Dexterity bonus allowed by his armor by 1. Every four levels thereafter (7th, 11th, and 15th), these bonuses increase by +1 each time, to a maximum –4 reduction of the armor check penalty and a +4 increase of the maximum Dexterity bonus allowed. \n\n" +
                "In addition, a fighter can also move at his normal speed while wearing medium armor. " +
                "At 7th level, a fighter can move at his normal speed while wearing heavy armor.",
                "f2dc771ab182476e9e92b2259d3190fb",
                armorTraining.Icon,
                FeatureGroup.None,
                Helpers.PrerequisiteFeature(armorTraining),
                Helpers.PrerequisiteClassLevel(fighter, 3),
                Helpers.Create<HeavyArmorSpeedPenaltyRemoval>());

            armorTrainingWithAdvandedArmorTraining.Ranks = 5;
            armorTrainingWithAdvandedArmorTraining.SetFeatures(new BlueprintFeature[] { armorTraining, advancedArmorTraining });
            BlueprintProgression fighterProgression = library.Get<BlueprintProgression>("b50e94b57be32f74892f381ae2a8905a");


            foreach(var levelEntry in fighterProgression.LevelEntries)
            {
                if(levelEntry.Level == 3 || levelEntry.Level == 7 || levelEntry.Level == 11 || levelEntry.Level == 15)
                {
                    levelEntry.Features.Add(armorTrainingWithAdvandedArmorTraining);
                    levelEntry.Features.Remove(armorTraining);
                }
            }
            foreach(var uiGroup in fighterProgression.UIGroups)
            {
                if(uiGroup.Features.Contains(armorTraining))
                {
                    uiGroup.Features.Add(armorTrainingWithAdvandedArmorTraining);
                }
            }

            BlueprintArchetype TwoHandedFighterArchetype = library.Get<BlueprintArchetype>("84643e02a764bff4a9c1aba333a53c89");
            RemoveAdvancedArmorTrainingFromArchetypes(TwoHandedFighterArchetype, armorTrainingWithAdvandedArmorTraining, armorTraining);

            BlueprintArchetype SwordlordArchetype = library.Get<BlueprintArchetype>("d80a67a264f206e4b8d2fcf7e560d48f");
            RemoveAdvancedArmorTrainingFromArchetypes(SwordlordArchetype, armorTrainingWithAdvandedArmorTraining, armorTraining);
        }

        private static void RemoveAdvancedArmorTrainingFromArchetypes(BlueprintArchetype archetype, BlueprintFeatureSelection armorTrainingWithAdvandedArmorTraining, BlueprintFeature armorTraining)
        {
            foreach(var levelEntry in archetype.RemoveFeatures)
            {
                if(levelEntry.Level == 3 || levelEntry.Level == 7 || levelEntry.Level == 11 || levelEntry.Level == 15)
                {
                    levelEntry.Features.Remove(armorTraining);
                    levelEntry.Features.Add(armorTrainingWithAdvandedArmorTraining);
                }
            }
        }

        private static List<BlueprintFeature> getAdvancedArmorTrainings(BlueprintFeature armorTraining, BlueprintCharacterClass fighter)
        {
            List<BlueprintFeature> list = new List<BlueprintFeature>();

            list.Add(Helpers.CreateFeature("ArmoredConfidence", "Armored Confidence",
                "While wearing armor, the fighter gains a bonus on Intimidate checks based upon the type of armor he is wearing: +1 for light armor, +2 for medium armor, or +3 for heavy armor. This bonus increases by 1 at 7th level and every 4 fighter levels thereafter, to a maximum of +4 at 19th level. In addition, the fighter adds half his armored confidence bonus to the DC of Intimidate checks to demoralize him.",
                "0d9cafb651a6427b950fb4a049126561",
                armorTraining.Icon,
                FeatureGroup.CombatFeat,
                Helpers.PrerequisiteFeature(armorTraining),
                Helpers.PrerequisiteClassLevel(fighter, 3),
                Helpers.Create<ArmoredConfidenceLogic>()));
            list.Add(Helpers.CreateFeature("ArmoredJuggernaut", "Armored Juggernaut",
                "When wearing heavy armor, the fighter gains DR 1/—. At 7th level, the fighter gains DR 1/— when wearing medium armor, and DR 2/— when wearing heavy armor. At 11th level, the fighter gains DR 1/— when wearing light armor, DR 2/— when wearing medium armor, and DR 3/— when wearing heavy armor. If the fighter is 19th level and has the armor mastery class feature, these DR values increase by 5. The DR from this ability stacks with that provided by adamantine armor, but not with other forms of damage reduction. This damage reduction does not apply if the fighter is helpless, stunned, or unconscious.",
                "74485919028943a5affacab602164ef7",
                armorTraining.Icon,
                FeatureGroup.CombatFeat,
                Helpers.PrerequisiteFeature(armorTraining),
                Helpers.PrerequisiteClassLevel(fighter, 3), Helpers.Create<ArmoredJuggernautLogic>()));

            BlueprintFeatureSelection adaptableTraining = Helpers.CreateFeatureSelection("AdaptableTraining", "Adaptable Training",
                "The fighter can use his base attack bonus in place of his ranks in one skill of his choice from the following list: Mobility or Athletics. The fighter need not be wearing armor or using a shield to use this option. When using adaptable training, the fighter substitutes his total base attack bonus (including his base attack bonus gained through levels in other classes) for his ranks in this skill, but adds the skill’s usual ability score modifier and any other bonuses or penalties that would modify that skill. Once a skill has been selected, it cannot be changed and the fighter can immediately retrain all of his ranks in the selected skill at no additional cost in money or time. In addition, the fighter adds all skills chosen with this option to his list of class skills. A fighter can choose this option once.",
                "94db2d5bc745417db94c68c463ab490d",
                armorTraining.Icon,
                FeatureGroup.CombatFeat,
                Helpers.PrerequisiteFeature(armorTraining),
                Helpers.PrerequisiteClassLevel(fighter, 3));

            BlueprintFeature adaptableTrainingMobility = Helpers.CreateFeature("AdaptableTrainingMobility", "Adaptable Training (Mobility)",
                "The fighter uses his base attack bonus in place of his ranks in Mobility. The fighter need not be wearing armor or using a shield to use this option. When using adaptable training, the fighter substitutes his total base attack bonus (including his base attack bonus gained through levels in other classes) for his ranks in this skill, but adds the skill’s usual ability score modifier and any other bonuses or penalties that would modify that skill. Once a skill has been selected, it cannot be changed and the fighter can immediately retrain all of his ranks in the selected skill at no additional cost in money or time. In addition, the fighter adds all skills chosen with this option to his list of class skills. A fighter can choose this option once.",
                "0f7a885d28024b03b992e15ff453db00",
                armorTraining.Icon,
                FeatureGroup.CombatFeat,
                Helpers.PrerequisiteFeature(armorTraining),
                Helpers.PrerequisiteClassLevel(fighter, 3),
                Helpers.Create<SetSkillRankToBabLogic>(b => b.type = Kingmaker.EntitySystem.Stats.StatType.SkillMobility));
            BlueprintFeature adaptableTrainingAthletics = Helpers.CreateFeature("AdaptableTrainingAthletics", "Adaptable Training (Athletics)",
               "The fighter uses his base attack bonus in place of his ranks in Athletics. The fighter need not be wearing armor or using a shield to use this option. When using adaptable training, the fighter substitutes his total base attack bonus (including his base attack bonus gained through levels in other classes) for his ranks in this skill, but adds the skill’s usual ability score modifier and any other bonuses or penalties that would modify that skill. Once a skill has been selected, it cannot be changed and the fighter can immediately retrain all of his ranks in the selected skill at no additional cost in money or time. In addition, the fighter adds all skills chosen with this option to his list of class skills. A fighter can choose this option once.",
               "91b76275af7441afb0c88811f25f7131",
               armorTraining.Icon,
               FeatureGroup.CombatFeat,
               Helpers.PrerequisiteFeature(armorTraining),
               Helpers.PrerequisiteClassLevel(fighter, 3),
               Helpers.Create<SetSkillRankToBabLogic>(b => b.type = Kingmaker.EntitySystem.Stats.StatType.SkillAthletics));
            adaptableTraining.AddComponent(adaptableTraining.PrerequisiteNoFeature());
            adaptableTraining.SetFeatures(adaptableTrainingMobility, adaptableTrainingAthletics);
            list.Add(adaptableTraining);

            BlueprintFeatureSelection armorSpecialization = Helpers.CreateFeatureSelection("ArmorSpecialization", "Armor Specialization",
               "The fighter selects one specific type of armor with which he is proficient, such as chain shirts or scale mail. While wearing the selected type of armor, the fighter adds one-quarter of his fighter level to the armor’s armor bonus, up to a maximum bonus of +3 for light armor, +4 for medium armor, or +5 for heavy armor. This increase to the armor bonus doesn’t increase the benefit that the fighter gains from feats, class abilities, or other effects that are determined by his armor’s base armor bonus, including other advanced armor training options. A fighter can choose this option multiple times. Each time he chooses it, he applies its benefit to a different type of armor.",
               "59d7a65500f34c4997bfb27ec93c15ec",
               armorTraining.Icon,
               FeatureGroup.CombatFeat,
               Helpers.PrerequisiteFeature(armorTraining),
               Helpers.PrerequisiteClassLevel(fighter, 3));

            Dictionary<string, BlueprintFeature> armorSpecializationFeatureList = new Dictionary<string, BlueprintFeature>();

            foreach(BlueprintArmorType armorType in Resources.FindObjectsOfTypeAll<BlueprintArmorType>())
            {
                string guidString = Helpers.getGuid("ArmorSpecialization" + armorType.DefaultName);
                if((armorType.ProficiencyGroup == ArmorProficiencyGroup.Heavy || 
                   armorType.ProficiencyGroup == ArmorProficiencyGroup.Medium || 
                   armorType.ProficiencyGroup == ArmorProficiencyGroup.Light) &&
                   !armorSpecializationFeatureList.ContainsKey(guidString))
                {
                    BlueprintFeature armorSpecializationForType = Helpers.CreateFeature("ArmorSpecialization" + armorType.DefaultName, "Armor Specialization (" + armorType.DefaultName + ")",
                   "Armor Specialization in " + armorType.DefaultName,
                    guidString,
                    armorTraining.Icon,
                    FeatureGroup.CombatFeat,
                    Helpers.PrerequisiteFeature(armorTraining),
                    Helpers.PrerequisiteClassLevel(fighter, 3),
                    Helpers.Create<ArmorSpecializationLogic>(b => b.BlueprintArmorType = armorType));
                    armorSpecializationFeatureList.Add(guidString, armorSpecializationForType);
                }
            }
            armorSpecialization.SetFeatures(armorSpecializationFeatureList.Values);

            list.Add(armorSpecialization);

            list.Add(Helpers.CreateFeature("SteelHeadbutt", "Steel Headbutt",
                   "While wearing medium or heavy armor, a fighter can deliver a headbutt with his helm as part of a full attack action. This headbutt is in addition to his normal attacks, and is made using the fighter’s base attack bonus – 5. A helmet headbutt deals 1d3 points of damage if the fighter is wearing medium armor, or 1d4 points of damage if he is wearing heavy armor (1d2 and 1d3, respectively, for Small creatures), plus an amount of damage equal to 1/2 the fighter’s Strength modifier. Treat this attack as a weapon attack made using the same special material (if any) as the armor. The armor’s enhancement bonus does modify the headbutt attack.",
                    "07cf0a9960f14e8e8e93a449d3a750aa",
                    armorTraining.Icon,
                    FeatureGroup.CombatFeat,
                    Helpers.PrerequisiteFeature(armorTraining),
                    Helpers.PrerequisiteClassLevel(fighter, 3),
                    Helpers.Create<SteelHeadbuttLogic>()));

            return list;
        }

        private static void addSteelHeadbutt()
        {


            foreach(ArmorProficiencyGroup ArmorProficiency in new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Heavy, ArmorProficiencyGroup.Medium })
            {
                string newAssetID = Helpers.getGuid("SteelHeadbuttType" + ArmorProficiency);
                DiceType diceType = ArmorProficiency == ArmorProficiencyGroup.Heavy ? DiceType.D4 : DiceType.D3;
                BlueprintWeaponType BlueprintWeaponTypeBite = library.Get<BlueprintWeaponType>("952e30e6cb40b454789a9db6e5f6dd09");
                BlueprintWeaponType BlueprintWeaponTypeHeadbutt = library.CopyAndAdd(BlueprintWeaponTypeBite, "SteelHeadbuttType" + ArmorProficiency, newAssetID);

                Traverse traverse = Traverse.Create(BlueprintWeaponTypeHeadbutt);
                traverse.Field("m_AssetGuid").SetValue(newAssetID);
                traverse.Field("m_IsNatural").SetValue(false);
                traverse.Field("m_DefaultNameText").SetValue(new FakeLocalizedString("Steel Headbutt"));
                traverse.Field("m_DescriptionText").SetValue(new FakeLocalizedString("While wearing medium or heavy armor, a fighter can deliver a headbutt with his helm as part of a full attack action. This headbutt is in addition to his normal attacks, and is made using the fighter’s base attack bonus – 5. A helmet headbutt deals 1d3 points of damage if the fighter is wearing medium armor, or 1d4 points of damage if he is wearing heavy armor (1d2 and 1d3, respectively, for Small creatures), plus an amount of damage equal to 1/2 the fighter’s Strength modifier. Treat this attack as a weapon attack made using the same special material (if any) as the armor. The armor’s enhancement bonus does modify the headbutt attack."));
                traverse.Field("m_BaseDamage").SetValue(new DiceFormula(1, diceType));


                for(int i = 0; i <= 5; ++i)
                {
                    newAssetID = Helpers.getGuid("SteelHeadbuttType" + ArmorProficiency + "Enhancement" + i);
                    BlueprintItemWeapon BlueprintItemWeaponBite = library.Get<BlueprintItemWeapon>("35dfad6517f401145af54111be04d6cf");
                    BlueprintItemWeapon BlueprintItemWeaponHeadbutt = library.CopyAndAdd(BlueprintItemWeaponBite, "SteelHeadbuttType" + ArmorProficiency + "Enhancement" + i, newAssetID);

                    traverse = Traverse.Create(BlueprintItemWeaponHeadbutt);
                    traverse.Field("m_AssetGuid").SetValue(newAssetID);
                    traverse.Field("m_Type").SetValue(BlueprintWeaponTypeBite);
                    traverse.Field("SpendCharges").SetValue(false);
                    BlueprintItemWeaponBite.DamageType.Physical.Enhancement = i;
                    BlueprintItemWeaponBite.DamageType.Physical.EnhancementTotal = i;
                }
            }


        }
    }
}
