using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace AdvancedMartialArts.HelperClasses
{
    [AllowMultipleComponents]
    class PrerequisiteMeetsPrerequisiteForAny : Prerequisite
    {
        [NotNull]
        public BlueprintFeature[] Features;

        public override bool Check(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
        {
            BlueprintFeature[] features = Features;
            foreach(BlueprintFeature blueprintFeature in features)
            {
                if (blueprintFeature.MeetsPrerequisites(selectionState, unit, state))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
