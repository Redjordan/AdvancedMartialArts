using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    class FightersTacticsLogic : OwnedGameLogicComponent<UnitDescriptor>, IGlobalSubscriber, IUnitEquipmentHandler, IUnitActiveEquipmentSetHandler
    {
        private ModifiableValue.Modifier Modifier = null;

        public BlueprintFeature WeaponTraining;


        private void ManageModifier()
        {
            if (AdvancedWeaponTraining.WieldsWeaponFromFighterGroup(base.Owner, WeaponTraining))
            {
            }
            else
            {
                base.Owner.State.Features.SoloTactics.Release();
            }
        }

        private void Activate(int value)
        {
            if (Modifier == null)
            {
                Modifier = base.Owner.Stats.SaveReflex.AddModifier(value, this, ModifierDescriptor.UntypedStackable);
            }
        }

        private void Deactivate()
        {
            if (Modifier != null)
            {
                base.Owner.Stats.SaveReflex.RemoveModifier(Modifier);
            }

            Modifier = null;
        }

        public override void OnTurnOn()
        {
            ManageModifier();
        }

        public override void OnTurnOff()
        {
            ManageModifier();
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem)
        {
            ManageModifier();
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit)
        {
            ManageModifier();
        }
    }
}