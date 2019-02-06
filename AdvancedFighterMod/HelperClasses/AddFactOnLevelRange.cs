using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedMartialArts
{
    // Adds a feature based on a level range and conditional.
    //
    // This makes it easier to implement complex conditions (e.g. Oracle Dragon Mystery Resistances)
    // without needing to create nested BlueprintFeatures and/or BlueprintProgressions.
    //
    // Essentially this combines AddFeatureIfHasFact with two AddFeatureOnClassLevels, to express:
    //
    //     if (MinLevel <= ClassLevel && ClassLevel <= MaxLevelInclusive &&
    //         (CheckedFact == null || HasFeature(CheckedFact) != Not)) {
    //         AddFact(Feature);
    //     }
    //
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class AddFactOnLevelRange : AddFactOnLevelUpCondtion
    {
        // The class to use for `MinLevel` and `MaxLevelInclusive`.
        // Optionally `AdditionalClasses` and `Archetypes` can be specified for more classes/archetypes.
        public BlueprintCharacterClass Class;

        // Optionally specifies the feature to check for.        
        public BlueprintUnitFact CheckedFact;

        // If `CheckedFact` is supplied, this indicates whether we want it to be present or
        // not present.
        public bool Not;

        public BlueprintCharacterClass[] AdditionalClasses = Array.Empty<BlueprintCharacterClass>();

        public BlueprintArchetype[] Archetypes = Array.Empty<BlueprintArchetype>();

        protected override int CalcLevel() => ReplaceCasterLevelOfAbility.CalculateClassLevel(Class, AdditionalClasses, Owner, Archetypes);

        protected override bool IsFeatureShouldBeApplied(int level)
        {
            return base.IsFeatureShouldBeApplied(level) && (CheckedFact == null || Owner.HasFact(CheckedFact) != Not);
        }
    }
}
