using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedMartialArts.Feats.GeneralFeats.Logic;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;

namespace AdvancedMartialArts.Feats.GeneralFeats
{
    class AnimalAllyFeatLine
    {
        public static void CreateAnimalAllyFeatLine()
        {
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
            BlueprintFeature[] features = new BlueprintFeature[]
            {
                AnimalCompanionEmptyCompanion,
                AnimalCompanionFeatureBear,
                AnimalCompanionFeatureBoar ,
                AnimalCompanionFeatureCentipede ,
                AnimalCompanionFeatureDog ,
                AnimalCompanionFeatureEkun,
                AnimalCompanionFeatureElk ,
                AnimalCompanionFeatureLeopard ,
                AnimalCompanionFeatureMammoth ,
                AnimalCompanionFeatureMonitor ,
                AnimalCompanionFeatureSmilodon ,
                AnimalCompanionFeatureWolf
            };

            BlueprintProgression domainAnimalCompanionProgression = Main.library.Get<BlueprintProgression>("125af359f8bc9a145968b5d8fd8159b8");


            BlueprintProgression animalAllyProgression = Main.library.CopyAndAdd(domainAnimalCompanionProgression, "animalAllyProgression", Helpers.getGuid("animalAllyProgression"));
            animalAllyProgression.Classes = (BlueprintCharacterClass[])Array.Empty<BlueprintCharacterClass>();

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
    }
}