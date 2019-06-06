using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedMartialArts.Feats.CombatFeats.FeatLogic;
using AdvancedMartialArts.Traits.Logic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.FactLogic;

namespace AdvancedMartialArts.Traits
{
    public class SocialTraits
    {
        private static LibraryScriptableObject library => Main.library;

        public static void Load()
        {
            Main.SafeLoad(CreateTraits, "CreateTraits");
        }

        private static void CreateTraits()
        {
            BlueprintFeatureSelection socialTraits = library.TryGet<BlueprintFeatureSelection>("9e41e60c929e45bc84ded046148c07ec");
            if(socialTraits!= null)
            {
                BlueprintFeatureSelection cleverWorldplayFeatureSelection = Helpers.CreateFeatureSelection(
                    "CleverWordplay",
                    "Clever Wordplay",
                    "Your cunning and logic are more than a match for another’s confidence and poise.\nBenefit: Choose one Charisma - based skill.You attempt checks with that skill using your Intelligence modifier instead of your Charisma modifier.",
                    Helpers.getGuid("CleverWordplay"),
                    null,
                    FeatureGroup.None);
                cleverWorldplayFeatureSelection.HideInUI = true;
                cleverWorldplayFeatureSelection.HideInCharacterSheetAndLevelUp = true;

                BlueprintFeature cleverWorldplayFeaturePersuasion = Helpers.CreateFeature("CleverWordplayPersuasion",
                    "Clever Wordplay (Persuasion)",
                    "Your cunning and logic are more than a match for another’s confidence and poise.\nBenefit: Choose one Charisma - based skill.You attempt checks with that skill using your Intelligence modifier instead of your Charisma modifier.",
                    Helpers.getGuid("CleverWordplayPersuasion"),
                    null,
                    FeatureGroup.None,
                    Helpers.Create<ReplaceBaseStatForStatTypeLogic>(x =>
                    {
                        x.StatTypeToReplaceBastStatFor = StatType.SkillPersuasion;
                        x.NewBaseStatType = StatType.Intelligence;
                    }));
                BlueprintFeature cleverWorldplayFeatureUMD = Helpers.CreateFeature("CleverWordplayUMD",
                    "Clever Wordplay (Use Magic device)",
                    "Your cunning and logic are more than a match for another’s confidence and poise.\nBenefit: Choose one Charisma - based skill.You attempt checks with that skill using your Intelligence modifier instead of your Charisma modifier.",
                    Helpers.getGuid("CleverWordplayUMD"),
                    null,
                    FeatureGroup.None,
                    Helpers.Create<ReplaceBaseStatForStatTypeLogic>(x =>
                    {
                        x.StatTypeToReplaceBastStatFor = StatType.SkillUseMagicDevice;
                        x.NewBaseStatType = StatType.Intelligence;
                    }));

                cleverWorldplayFeatureSelection.SetFeatures(new BlueprintFeature[]{cleverWorldplayFeaturePersuasion, cleverWorldplayFeatureUMD});

                socialTraits.SetFeatures(socialTraits.AllFeatures.AddToArray(cleverWorldplayFeatureSelection));
            }
        }
    }
}
