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
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    class DazzlingIntimidationLogic : OwnedGameLogicComponent<UnitDescriptor>, IGlobalSubscriber, IUnitEquipmentHandler, IUnitActiveEquipmentSetHandler
    {
        private ModifiableValue.Modifier _modifier = null;
        private int _oldValue = 0;
        private bool _hadPersuasionUseAbility = false;
        private bool _hadDazzlingDisplayAction = false;

        public static BlueprintAbility DazzlingDisplayAction = Main.library.Get<BlueprintAbility>("5f3126d4120b2b244a95cb2ec23d69fb");
        public static BlueprintAbility DazzlingDisplayActionMove = Main.library.Get<BlueprintAbility>("545b0811f3c24a518c02f749509623c4");
        public static BlueprintAbility PersuasionUseAbility = Main.library.Get<BlueprintAbility>("7d2233c3b7a0b984ba058a83b736e6ac");
        public static BlueprintAbility PersuasionUseAbilityMove = Main.library.Get<BlueprintAbility>("3af0e87e92a348daad1ec1934a7f811c");

        public BlueprintFeature WeaponTraining;

        private void CheckForWeaponTraining()
        {
            if (AdvancedWeaponTraining.WieldsWeaponFromFighterGroup(base.Owner, WeaponTraining))
            {
                int newValue = AdvancedWeaponTraining.GetWeaponTrainingRank(base.Owner, WeaponTraining);

                if (_oldValue != newValue)
                {
                    Deactivate();
                    Activate(newValue);
                }
            }
            else
            {
                Deactivate();
            }
        }

        private void Activate(int value)
        {
            if (_modifier == null && value != 0)
            {
                Deactivate();
                _modifier = base.Owner.Stats.CheckIntimidate.AddModifier(value, this, ModifierDescriptor.UntypedStackable);
                _oldValue = value;
                Ability ability = base.Owner.Abilities.GetAbility(PersuasionUseAbility);
                if (ability != null)
                {
                    _hadPersuasionUseAbility = true;
                    base.Owner.Abilities.RemoveFact(ability);
                    base.Owner.Abilities.AddFact(PersuasionUseAbilityMove, Helpers.GetMechanicsContext());
                }

                ability = base.Owner.Abilities.GetAbility(DazzlingDisplayAction);
                if (ability != null)
                {
                    _hadDazzlingDisplayAction = true;
                    base.Owner.Abilities.RemoveFact(ability);
                    base.Owner.Abilities.AddFact(DazzlingDisplayActionMove, Helpers.GetMechanicsContext());
                }
            }
        }

        private void Deactivate()
        {
            if (_modifier != null)
            {
                base.Owner.Stats.CheckIntimidate.RemoveModifier(_modifier);
                base.Owner.Abilities.RemoveFact(PersuasionUseAbilityMove);
                if (_hadPersuasionUseAbility)
                {
                    base.Owner.Abilities.AddFact(PersuasionUseAbility, Helpers.GetMechanicsContext());
                }

                if (_hadDazzlingDisplayAction)
                {
                    base.Owner.Abilities.AddFact(DazzlingDisplayAction, Helpers.GetMechanicsContext());
                }
            }

            _modifier = null;
            _oldValue = 0;
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