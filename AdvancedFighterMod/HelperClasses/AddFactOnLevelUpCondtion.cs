using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedMartialArts
{
    // A customizable "add fact on level", that also can interact with the level-up UI
    // in a similar way to progressions and features the player picked.
    // (This is similar to how the level-up UI calls LevelUpHelper.AddFeatures).
    public abstract class AddFactOnLevelUpCondtion : OwnedGameLogicComponent<UnitDescriptor>, IUnitGainLevelHandler
    {
        // The min and max (inclusive) levels in which to apply this feature.
        public int MinLevel = 1, MaxLevelInclusive = 20;

        // The feature to add, if the condition(s) are met.
        public BlueprintUnitFact Feature;

        [JsonProperty]
        private Fact appliedFact;

        public override void OnFactActivate() => Apply();

        public override void OnFactDeactivate()
        {
            Owner.RemoveFact(appliedFact);
            appliedFact = null;
        }

        public override void PostLoad()
        {
            base.PostLoad();
            if (appliedFact != null && !Owner.HasFact(appliedFact))
            {
                appliedFact.Dispose();
                appliedFact = null;
                if (BlueprintRoot.Instance.PlayerUpgradeActions.AllowedForRestoreFeatures.Contains(Feature))
                {
                    Apply();
                }
            }
        }

        protected abstract int CalcLevel();

        protected virtual bool IsFeatureShouldBeApplied(int level)
        {
            Log.Write($"AddFactOnLevelUpCondtion::IsFeatureShouldBeApplied({level}), MinLevel {MinLevel}, MaxLevelInclusive {MaxLevelInclusive}");
            return level >= MinLevel && level <= MaxLevelInclusive;
        }

        public void HandleUnitGainLevel(UnitDescriptor unit, BlueprintCharacterClass @class)
        {
            if (unit == Owner) Apply();
        }

        private Fact Apply()
        {
            Log.Write($"AddFactOnLevelUpCondtion::Apply(), name: {Fact.Blueprint.name}");
            var level = CalcLevel();
            if (IsFeatureShouldBeApplied(level))
            {
                if (appliedFact == null)
                {
                    appliedFact = Owner.AddFact(Feature, null, (Fact as Feature)?.Param);
                    OnAddLevelUpFeature(level);
                }
            }
            else if (appliedFact != null)
            {
                Owner.RemoveFact(appliedFact);
                appliedFact = null;
            }
            return appliedFact;
        }

        private void OnAddLevelUpFeature(int level)
        {
            Log.Write($"AddFactOnLevelUpCondtion::OnAddLevelUpFeature(), name: {Fact.Blueprint.name}");
            var fact = appliedFact;
            if (fact == null) return;

            var feature = fact.Blueprint as BlueprintFeature;
            if (feature == null) return;

            // If we're in the level-up UI, update selections/progressions as needed.
            var unit = Owner;
            var levelUp = Game.Instance.UI.CharacterBuildController.LevelUpController;
            if (unit == levelUp.Preview || unit == levelUp.Unit)
            {
                var selection = feature as BlueprintFeatureSelection;
                if (selection != null)
                {
                    Log.Write($"{GetType().Name}: add selection ${selection.name}");
                    levelUp.State.AddSelection(null, selection, selection, level);
                }
                var progression = feature as BlueprintProgression;
                if (progression != null)
                {
                    Log.Write($"{GetType().Name}: update progression ${selection.name}");
                    LevelUpHelper.UpdateProgression(levelUp.State, unit, progression);
                }
            }
        }
    }
}
