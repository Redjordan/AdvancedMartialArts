using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    class EffortlessDualWieldingLogic : OwnedGameLogicComponent<UnitDescriptor>, IGlobalSubscriber, IUnitEquipmentHandler, IUnitActiveEquipmentSetHandler
    {
        public BlueprintFeature WeaponTraining;
        private ModifiableValue.Modifier _modifier = null;

        private void CheckWeapons()
        {
            WeaponGroupAttackBonus weaponGroupAttackBonus = WeaponTraining.GetComponent<WeaponGroupAttackBonus>();
            if(base.Owner.Body.SecondaryHand.MaybeWeapon != null &&
                AdvancedWeaponTraining._weaponCategoryToWeaponTraining[base.Owner.Body.SecondaryHand.MaybeWeapon.Blueprint.Category] == weaponGroupAttackBonus.WeaponGroup &&
                !base.Owner.Body.SecondaryHand.MaybeWeapon.Blueprint.IsLight)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }

        private void Activate()
        {
            if (_modifier == null)
            {
                _modifier = base.Owner.Stats.AdditionalAttackBonus.AddModifier(2, this, ModifierDescriptor.UntypedStackable);
            }
        }

        private void Deactivate()
        {
            if (_modifier != null)
            {
                base.Owner.Stats.AdditionalAttackBonus.RemoveModifier(_modifier);
            }

            _modifier = null;
        }

        public override void OnTurnOn()
        {
            CheckWeapons();
        }

        public override void OnTurnOff()
        {
            Deactivate();
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem)
        {
            CheckWeapons();
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit)
        {
            CheckWeapons();
        }
    }
}