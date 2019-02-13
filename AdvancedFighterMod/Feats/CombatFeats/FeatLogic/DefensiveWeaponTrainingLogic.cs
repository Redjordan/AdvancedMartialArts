using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    class DefensiveWeaponTrainingLogic : OwnedGameLogicComponent<UnitDescriptor>, IGlobalSubscriber, IUnitEquipmentHandler, IUnitActiveEquipmentSetHandler
    {
        private ModifiableValue.Modifier Modifier = null;
        private int OldValue = 0;

        public BlueprintFeature WeaponTraining;

        private void CheckForWeaponTraining()
        {
            if (AdvancedWeaponTraining.WieldsWeaponFromFighterGroup(base.Owner, WeaponTraining))
            {
                int WeaponTrainingValue = AdvancedWeaponTraining.GetWeaponTrainingRank(base.Owner, WeaponTraining);

                int acBonus = 1;
                if(WeaponTrainingValue >= 4)
                {
                    acBonus += 1;
                }

                int weaponEnhancementBonus = AdvancedWeaponTraining.GetWeaponEnhancementBonus(base.Owner);
                acBonus += weaponEnhancementBonus / 2;

                if(OldValue != acBonus)
                {
                    Deactivate();
                    Activate(acBonus);
                }
            }
            else
            {
                Deactivate();
            }

        }

        private void Activate(int value)
        {
            if (Modifier == null)
            {
                Deactivate();
                Modifier = base.Owner.Stats.AC.AddModifier(value, this, ModifierDescriptor.Shield);
                OldValue = value;
            }
        }

        private void Deactivate()
        {
            if (Modifier != null)
            {
                base.Owner.Stats.AC.RemoveModifier(Modifier);
            }

            Modifier = null;
            OldValue = 0;
        }

        public override void OnTurnOn()
        {
            CheckForWeaponTraining();
        }

        public override void OnTurnOff()
        {
            CheckForWeaponTraining();
        }

        public override void OnRecalculate()
        {
            CheckForWeaponTraining();
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem)
        {
            if (slot.GetType() == typeof(HandSlot))
            {
                CheckForWeaponTraining();
            }
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit)
        {
            CheckForWeaponTraining();
        }
    }
}