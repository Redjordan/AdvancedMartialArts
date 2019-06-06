using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace AdvancedMartialArts.HelperClasses
{
    [AllowMultipleComponents]
    internal class PrerequisiteKnowsSpell : Prerequisite
    {
        [NotNull]
        public BlueprintAbility Ability;

        public BlueprintCharacterClass CharacterClass = null;


        public override bool Check(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
        {
            foreach(Spellbook Spellbook in unit.Spellbooks)
            {
                if((CharacterClass == null || Spellbook.Blueprint.CharacterClass == CharacterClass) && Spellbook.IsKnown(Ability))
                {
                    return true;
                }
            }
            return false;
        }

        public override string GetUIText()
        {
            if(CharacterClass != null)
            {
                return "Knows the spell " + Ability.Name + " as part of being a " + CharacterClass.Name;
            }

            return "Knows the spell " + Ability.Name;

        }
    }
}
