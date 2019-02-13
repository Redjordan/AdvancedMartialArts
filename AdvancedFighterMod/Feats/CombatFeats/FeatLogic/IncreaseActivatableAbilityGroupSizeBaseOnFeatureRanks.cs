using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Parts;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    class IncreaseActivatableAbilityGroupSizeBaseOnFeatureRanks : OwnedGameLogicComponent<UnitDescriptor>, IGlobalSubscriber, IUnitEquipmentHandler, IUnitActiveEquipmentSetHandler
    {
        public ActivatableAbilityGroup Group;
        public BlueprintFeature Feature;


        private void ManageGroupSize()
        {
            int currentGroupSize = base.Owner.Ensure<UnitPartActivatableAbility>().GetGroupSize(this.Group);
            int currentRanks = base.Owner.Progression.Features.GetRank(Feature);
            for(int i = 0; i < currentGroupSize; i++)
            {
                this.Owner.Ensure<UnitPartActivatableAbility>().DecreaseGroupSize(this.Group);
            }

            for (int i = 0; i < currentRanks; i++)
            {
                this.Owner.Ensure<UnitPartActivatableAbility>().IncreaseGroupSize(this.Group);
            }
        }

        public override void OnTurnOn()
        {
            ManageGroupSize();
        }

        public override void OnTurnOff()
        {
            ManageGroupSize();
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem)
        {
            ManageGroupSize();
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit)
        {
            ManageGroupSize();
        }
    }
}