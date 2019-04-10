using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Collections.Generic;

namespace AdvancedMartialArts.HelperClasses
{
    public class BlueprintAbilityPrerequisite : Prerequisite
    {
        public List<BlueprintAbility> _abilityList;

        public override bool Check(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
        {
            foreach(Kingmaker.UnitLogic.Abilities.Ability unitAbility in unit.Abilities)
            {
                if(_abilityList.Contains(unitAbility.Blueprint))
                {
                    return true;
                }
            }

            return false;
        }

        public override string GetUIText()
        {
            string text = "Has access to any of the following abilities: \n";
            foreach(BlueprintAbility blueprintAbility in _abilityList)
            {
                text += blueprintAbility.Name + "\n";
            }

            return text;
        }
    }
}
