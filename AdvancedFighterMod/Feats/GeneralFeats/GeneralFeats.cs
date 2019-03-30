using AdvancedMartialArts.Feats.GeneralFeats.Logic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdvancedMartialArts.Feats.GeneralFeats
{
    public class GeneralFeats
    {
        private static LibraryScriptableObject library => Main.library;
        public static void Load()
        {
            Main.SafeLoad(CreateGeneralFeats, "GeneralFeats");
        }

        private static void CreateGeneralFeats()
        {
            AnimalAllyFeatLine.CreateAnimalAllyFeatLine();
            CreateGuidedHandFeat();
        }

        private static void CreateGuidedHandFeat()
        {
            BlueprintFeature selectiveChannel = library.Get<BlueprintFeature>("fd30c69417b434d47b6b03b9c1f568ff");

            List<BlueprintFeature> guidedHandFeatures = new List<BlueprintFeature>();
            foreach(BlueprintFeature feature in Resources.FindObjectsOfTypeAll<BlueprintFeature>())
            {
                if(feature.Groups.Contains(FeatureGroup.Deities))
                {
                    AddFeatureOnClassLevel levelUpProficiencyFeature = feature.GetComponent<AddFeatureOnClassLevel>();
                    if(levelUpProficiencyFeature != null)
                    {
                        BlueprintFeature proficiencyFeature = levelUpProficiencyFeature.Feature;
                        WeaponCategory category;
                        if(proficiencyFeature.AssetGuid == "7812ad3672a4b9a4fb894ea402095167")
                        {
                            category = WeaponCategory.UnarmedStrike;
                        }
                        else
                        {
                            AddProficiencies addedProficiency = proficiencyFeature.GetComponent<AddProficiencies>();
                            category = addedProficiency.WeaponProficiencies[0];
                        }

                        guidedHandFeatures.Add(Helpers.CreateFeature(
                            "GuidedHand" + feature.AssetGuid,
                            "Guided Hand",
                            "With your deity’s favored weapon, you can use your Wisdom modifier instead of your Strength or Dexterity modifier on attack rolls.",
                            Helpers.getGuid("GuidedHand" + feature.AssetGuid),
                            feature.Icon,
                            FeatureGroup.Feat,
                            feature.PrerequisiteFeature(),
                            Helpers.Create<PrerequisiteProficiency>(x => x.WeaponProficiencies = new WeaponCategory[] { category }),
                            selectiveChannel.PrerequisiteFeature(),
                            Helpers.Create<ReplaceAttackStatForWeaponCategoryLogic>(x =>
                            {
                                x.StatType = StatType.Wisdom;
                                x.WeaponCategory = category;
                            })
                        ));
                    }

                }
            }

            library.AddFeats(guidedHandFeatures.ToArray());
        }
    }
}
