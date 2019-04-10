using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedMartialArts.Traits.Logic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;

namespace AdvancedMartialArts.Traits
{
    class RegionalTraits
    {
        private static LibraryScriptableObject library => Main.library;

        public static void Load()
        {
            Main.SafeLoad(CreateTraits, "CreateTraits");
        }

        private static void CreateTraits()
        {
            BlueprintFeatureSelection regionalTraits = library.TryGet<BlueprintFeatureSelection>("6158dd4ad2544c27bc3a9b48c2e8a2ca");
            if(regionalTraits != null)
            {
                BlueprintFeature riverRatFeatureSelection = Helpers.CreateFeature(
                    "RiverRat",
                    "River Rat",
                    "You learned to swim right after you learned to walk. When you were a youth, a gang of river pirates put you to work swimming in nighttime rivers and canals with a dagger between your teeth so you could sever the anchor ropes of merchant vessels.\nBenefits: You gain a + 1 trait bonus on damage rolls with a dagger and a + 1 trait bonus on Athletics checks. Athletics is always a class skill for you.",
                    Helpers.getGuid("RiverRat"),
                    null,
                    FeatureGroup.None,
                    Helpers.Create<AddClassSkill>(a => a.Skill = StatType.SkillAthletics),
                    Helpers.CreateAddStatBonus(StatType.SkillAthletics, 1, ModifierDescriptor.Trait)
                    );

                regionalTraits.SetFeatures(regionalTraits.AllFeatures.AddToArray(riverRatFeatureSelection));
            }
        }
    }
}
