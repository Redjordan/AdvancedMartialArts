using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AdvancedMartialArts.Feats.GeneralFeats.Logic;
using Harmony12;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using Kingmaker.Visual.Sound;
using UnityEngine;

namespace AdvancedMartialArts.Feats.GeneralFeats
{
    class AnimalAllyFeatLine
    {
        public static void CreateAnimalAllyFeatLine()
        {
            Main.ApplyPatch(typeof(GetPortraitFolderPathPatch), "Test");

            BlueprintFeature skillFocusLoreNature = Main.library.Get<BlueprintFeature>("6507d2da389ed55448e0e1e5b871c013");

            BlueprintFeature natureSoul = Helpers.CreateFeature(
                "NatureSoul",
                "Nature Soul",
                "You are innately in tune with nature and venerate the power and mystery of the natural world. \n" +
                "Benefit: You get a + 2 bonus on all Knowledge(nature) checks and Survival checks.If you have 10 or more ranks in one of these skills, the bonus increases to + 4 for that skill.",
                Helpers.getGuid("NatureSoul"),
                skillFocusLoreNature.Icon,
                FeatureGroup.Feat,
                Helpers.Create<NatureSoulLogic>()
            );


            BlueprintFeature animalDomainProgressionSecondary = Main.library.Get<BlueprintFeature>("f13eb6be93dd5234c8126e5384040009");
            BlueprintFeature animalDomainProgression = Main.library.Get<BlueprintFeature>("23d2f87aa54c89f418e68e790dba11e0");
            BlueprintArchetype sacredHuntsmasterArchetype = Main.library.Get<BlueprintArchetype>("46eb929c8b6d7164188eb4d9bcd0a012");
            BlueprintCharacterClass inquisitorClass = Main.library.Get<BlueprintCharacterClass>("f1a70d9e1b0b41e49874e1fa9052a1ce");


            BlueprintFeature AnimalCompanionEmptyCompanion = Main.library.Get<BlueprintFeature>("472091361cf118049a2b4339c4ea836a");
            BlueprintFeature AnimalCompanionFeatureBear = Main.library.Get<BlueprintFeature>("f6f1cdcc404f10c4493dc1e51208fd6f");
            BlueprintFeature AnimalCompanionFeatureBoar = Main.library.Get<BlueprintFeature>("afb817d80b843cc4fa7b12289e6ebe3d");
            BlueprintFeature AnimalCompanionFeatureCentipede = Main.library.Get<BlueprintFeature>("f9ef7717531f5914a9b6ecacfad63f46");
            BlueprintFeature AnimalCompanionFeatureDog = Main.library.Get<BlueprintFeature>("f894e003d31461f48a02f5caec4e3359");
            BlueprintFeature AnimalCompanionFeatureEkun = Main.library.Get<BlueprintFeature>("e992949eba096644784592dc7f51a5c7");
            BlueprintFeature AnimalCompanionFeatureElk = Main.library.Get<BlueprintFeature>("aa92fea676be33d4dafd176d699d7996");
            BlueprintFeature AnimalCompanionFeatureLeopard = Main.library.Get<BlueprintFeature>("2ee2ba60850dd064e8b98bf5c2c946ba");
            BlueprintFeature AnimalCompanionFeatureMammoth = Main.library.Get<BlueprintFeature>("6adc3aab7cde56b40aa189a797254271");
            BlueprintFeature AnimalCompanionFeatureMonitor = Main.library.Get<BlueprintFeature>("ece6bde3dfc76ba4791376428e70621a");
            BlueprintFeature AnimalCompanionFeatureSmilodon = Main.library.Get<BlueprintFeature>("126712ef923ab204983d6f107629c895");
            BlueprintFeature AnimalCompanionFeatureWolf = Main.library.Get<BlueprintFeature>("67a9dc42b15d0954ca4689b13e8dedea");
            BlueprintFeature AnimalCompanionFeatureGiantSpider = AddSpiderCompanion();
            BlueprintFeature[] features = new BlueprintFeature[]
            {
                AnimalCompanionEmptyCompanion,
                AnimalCompanionFeatureBear,
                AnimalCompanionFeatureBoar,
                AnimalCompanionFeatureCentipede,
                AnimalCompanionFeatureDog,
                AnimalCompanionFeatureEkun,
                AnimalCompanionFeatureElk,
                AnimalCompanionFeatureLeopard,
                AnimalCompanionFeatureMammoth,
                AnimalCompanionFeatureMonitor,
                AnimalCompanionFeatureSmilodon,
                AnimalCompanionFeatureWolf,
                AnimalCompanionFeatureGiantSpider
            };

            AddAnimalCompanionToAllSelections(AnimalCompanionFeatureGiantSpider);

            BlueprintProgression domainAnimalCompanionProgression = Main.library.Get<BlueprintProgression>("125af359f8bc9a145968b5d8fd8159b8");


            BlueprintProgression animalAllyProgression = Main.library.CopyAndAdd(domainAnimalCompanionProgression, "animalAllyProgression", Helpers.getGuid("animalAllyProgression"));
            animalAllyProgression.Classes = (BlueprintCharacterClass[]) Array.Empty<BlueprintCharacterClass>();

            BlueprintFeatureSelection animalAlly = Helpers.CreateFeatureSelection(
                "AnimalAlly",
                "Animal Ally",
                "You gain an animal companion as if you were a druid of your character level –3 from the following list: badger, bird, camel, cat (small), dire rat, dog, horse, pony, snake (viper), or wolf. If you later gain an animal companion through another source (such as the Animal domain, divine bond, hunter’s bond, mount, or nature bond class features), the effective druid level granted by this feat stacks with that granted by other sources.",
                Helpers.getGuid("AnimalAlly"),
                skillFocusLoreNature.Icon,
                FeatureGroup.Feat,
                natureSoul.PrerequisiteFeature(),
                Helpers.PrerequisiteCharacterLevel(4),
                Helpers.Create<PrerequisiteNoArchetype>(x =>
                {
                    x.Archetype = sacredHuntsmasterArchetype;
                    x.CharacterClass = inquisitorClass;
                }),
                animalDomainProgression.PrerequisiteNoFeature(),
                animalDomainProgressionSecondary.PrerequisiteNoFeature(),
                Helpers.Create<AddFeatureOnApply>(x => x.Feature = animalAllyProgression),
                Helpers.Create<AnimalAllyAdjustToLevelLogic>()
            );

            animalAlly.SetFeatures(features);

            Main.library.AddFeats(natureSoul, animalAlly);
        }

        private static void AddAnimalCompanionToAllSelections(BlueprintFeature blueprintFeature)
        {
            BlueprintFeatureSelection[] animalCompaionFeatureSelections = new BlueprintFeatureSelection[]
            {
                Main.library.Get<BlueprintFeatureSelection>("a540d7dfe1e2a174a94198aba037274c"), // AnimalCompanionSelectionSylvanSorcerer
                Main.library.Get<BlueprintFeatureSelection>("ee63330662126374e8785cc901941ac7"), // AnimalCompanionSelectionRanger
                Main.library.Get<BlueprintFeatureSelection>("90406c575576aee40a34917a1b429254"), // AnimalCompanionSelectionBase
                Main.library.Get<BlueprintFeatureSelection>("2995b36659b9ad3408fd26f137ee2c67"), // AnimalCompanionSelectionSacredHuntsmaster
                Main.library.Get<BlueprintFeatureSelection>("571f8434d98560c43935e132df65fe76"), // AnimalCompanionSelectionDruid
                Main.library.Get<BlueprintFeatureSelection>("2ecd6c64683b59944a7fe544033bb533"), // AnimalCompanionSelectionDomain
                Main.library.Get<BlueprintFeatureSelection>("738b59d0b58187f4d846b0caaf0f80d7")  // AnimalCompanionSelectionMadDog
            };

            foreach (var animalCompaionFeatureSelection in animalCompaionFeatureSelections)
            {
                animalCompaionFeatureSelection.SetFeatures(animalCompaionFeatureSelection.AllFeatures.AddToArray(blueprintFeature));
            }
        }

        private static BlueprintFeature AddSpiderCompanion()
        {
            PortraitData portraitData = new PortraitData("AdvancedMartialArtsSpider");

            BlueprintPortrait portrait = Helpers.Create<BlueprintPortrait>();
            portrait.Data = portraitData;
            Main.library.AddAsset(portrait, Helpers.getGuid("GiantSpiderPortrait"));

            BlueprintUnitFact reducedReachFact = Main.library.Get<BlueprintUnitFact>("c33f2d68d93ceee488aa4004347dffca");
            BlueprintFeature weaponFinesse = Main.library.Get<BlueprintFeature>("90e54424d682d104ab36436bd527af09");

            BlueprintFeature animalCompanionUpgradeCentipede = Main.library.Get<BlueprintFeature>("c938099ca0438b242b3edecfa9083e9f");
            BlueprintUnit animalCompanionUnitCentipede = Main.library.Get<BlueprintUnit>("f9df16ffd0c8cec4d99a0ae6f025a3f8");

            BlueprintUnit giantSpider = Main.library.CopyAndAdd<BlueprintUnit>("c4b33e5fd3d3a6f46b2aade647b0bf25", "GiantSpiderCompanion", Helpers.getGuid("GiantSpiderCompanion"));

            giantSpider.Brain = animalCompanionUnitCentipede.Brain;
            giantSpider.ComponentsArray = animalCompanionUnitCentipede.ComponentsArray;
            giantSpider.AddFacts = giantSpider.AddFacts.AddToArray(weaponFinesse);
            giantSpider.Faction = Main.library.Get<BlueprintFaction>("d8de50cc80eb4dc409a983991e0b77ad"); // Neutral faction

            Helpers.SetField(giantSpider, "m_Portrait", portrait);

            BlueprintUnitAsksList giantSpiderBarks = Main.library.CopyAndAdd<BlueprintUnitAsksList>("7d340f75a57c47d45b0e79200a6b5eac", "SpiderAnimalCompanionBarks", Helpers.getGuid("SpiderAnimalCompanionBarks"));
            UnitAsksComponent component = giantSpiderBarks.GetComponent<UnitAsksComponent>();
            foreach (var componentAnimationBark in component.AnimationBarks)
            {
                if (componentAnimationBark.AnimationEvent == MappedAnimationEventType.AlertSound1 || componentAnimationBark.AnimationEvent == MappedAnimationEventType.AlertSound2)
                {
                    componentAnimationBark.Cooldown = 10f;
                    componentAnimationBark.DelayMin = 5f;
                }
            }

            ChangeUnitSize unitSize = Helpers.Create<ChangeUnitSize>(x => x.SizeDelta = 1);

            FieldInfo typeField = unitSize.GetType().GetField("m_Type", BindingFlags.NonPublic | BindingFlags.Instance);
            object delta = unitSize.GetType().GetNestedType("ChangeType", BindingFlags.NonPublic).GetField("Delta").GetValue(unitSize);

            typeField.SetValue(unitSize, delta);

            AddMechanicsFeature addMechanicsFeature = Helpers.Create<AddMechanicsFeature>();

            Traverse traverse = Traverse.Create(addMechanicsFeature);
            traverse.Field("m_Feature").SetValue(AddMechanicsFeature.MechanicsFeatureType.IterativeNaturalAttacks);


            typeField.SetValue(unitSize, delta);

            BlueprintFeature animalCompanionFeatureSpider = Main.library.CopyAndAdd<BlueprintFeature>("f9ef7717531f5914a9b6ecacfad63f46", "AnimalCompanionFeatureGiantSpider", Helpers.getGuid("AnimalCompanionFeatureGiantSpider"));
            animalCompanionFeatureSpider.SetNameDescription("Animal Companion — Giant Spider", "Size Medium\nSpeed 30 ft.\nAC +1 natural armor\nAttack bite (1d6 plus poison)\nAbility Scores Str 11, Dex 17, Con 12, Int 1, Wis 10, Cha 2\nSpecial Attacks poison (frequency 1 round (4); effect 1d2 Str damage; cure 1 save; Con-based DC)\nCMD +8 vs. trip.\nAt 7th level size becomes Large, Str +2, Dex +8, Con +4, +2 natural armor.");

            AddPet addPetFact = animalCompanionFeatureSpider.ComponentsArray.OfType<AddPet>().First();
            animalCompanionFeatureSpider.RemoveComponent(addPetFact);
            addPetFact = UnityEngine.Object.Instantiate(addPetFact);
            animalCompanionFeatureSpider.AddComponent(addPetFact);

            addPetFact.Pet = giantSpider;
            addPetFact.UpgradeFeature = Helpers.CreateFeature(
                "AnimalCompanionUpgradeGiantSpider",
                "",
                "",
                Helpers.getGuid("AnimalCompanionUpgradeGiantSpider"),
                animalCompanionUpgradeCentipede.Icon,
                FeatureGroup.None,
                unitSize,
                Helpers.Create<AddStatBonus>(x =>
                {
                    x.Stat = StatType.AC;
                    x.Value = 2;
                    x.Descriptor = ModifierDescriptor.NaturalArmor;
                }),
                Helpers.Create<AddStatBonus>(x =>
                {
                    x.Stat = StatType.Strength;
                    x.Value = 2;
                }),
                Helpers.Create<AddStatBonus>(x =>
                {
                    x.Stat = StatType.Dexterity;
                    x.Value = 8;
                }),
                Helpers.Create<AddStatBonus>(x =>
                {
                    x.Stat = StatType.Constitution;
                    x.Value = 4;
                }),
                addMechanicsFeature,
                Helpers.CreateAddFacts(reducedReachFact)
            );
            addPetFact.UpgradeLevel = 7;
            return animalCompanionFeatureSpider;
        }

        [HarmonyPatch(typeof(CustomPortraitsManager), "GetPortraitFolderPath", typeof(string))]
        private static class GetPortraitFolderPathPatch
        {
            private static bool Prefix(CustomPortraitsManager __instance, string id, ref string __result)
            {
                if (id.Contains("AdvancedMartialArts"))
                {
                    __result = Path.Combine(Directory.GetCurrentDirectory() , "Mods\\AdvancedMartialArts\\Resources" , id);
                    return false;
                }

                return true;
            }
        }
    }
}