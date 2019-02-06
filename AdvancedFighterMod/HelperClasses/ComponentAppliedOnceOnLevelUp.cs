using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Newtonsoft.Json;

namespace AdvancedMartialArts.HelperClasses
{
    public abstract class ComponentAppliedOnceOnLevelUp : OwnedGameLogicComponent<UnitDescriptor>, ILevelUpCompleteUIHandler
    {
        [JsonProperty]
        int appliedRank;

        public override void OnFactActivate()
        {
            try
            {
                Log.Write($"{GetType()}.OnFactActivate(), applied rank? {appliedRank}");
                int rank = Fact.GetRank();
                if(appliedRank >= rank) return;

                // If we're in the level-up UI, apply the component
                var levelUp = Game.Instance.UI.CharacterBuildController.LevelUpController;
                if(Owner == levelUp.Preview || Owner == levelUp.Unit)
                {
                    for(; appliedRank < rank; appliedRank++)
                    {
                        Apply(levelUp.State);
                    }
                }
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
        }

        // Optionally remove this fact to free some memory; useful if the fact is already applied
        // and there is no reason to track its overall rank.
        protected virtual bool RemoveAfterLevelUp => false;

        public void HandleLevelUpComplete(UnitEntityData unit, bool isChargen)
        {
            if(RemoveAfterLevelUp && unit.Descriptor == Owner)
            {
                Log.Write($"Removing fact {Fact.Blueprint.AssetGuid}");
                Owner.RemoveFact(Fact);
            }
        }

        protected abstract void Apply(LevelUpState state);
    }
}
