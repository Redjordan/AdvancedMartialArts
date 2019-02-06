using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedMartialArts.HelperClasses;
using Kingmaker;
using Kingmaker.UnitLogic.Class.LevelUp;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    public class UndoSelectionLogic : ComponentAppliedOnceOnLevelUp
    {
        protected override void Apply(LevelUpState state)
        {
            Log.Write($"{GetType().Name}: trying to unselect");
            var selection = state.Selections.FirstOrDefault(s => s.SelectedItem?.Feature == Fact.Blueprint);
            if(selection != null)
            {
                Log.Write($"Unselecting selection {selection.Index}");
                Game.Instance.UI.CharacterBuildController.LevelUpController.UnselectFeature(selection);
            }
        }

        protected override bool RemoveAfterLevelUp => true;
    }
}
