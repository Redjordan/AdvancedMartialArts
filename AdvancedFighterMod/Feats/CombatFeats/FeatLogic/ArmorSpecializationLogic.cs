using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    internal class ArmorSpecializationLogic : OwnedGameLogicComponent<UnitDescriptor>, IGlobalSubscriber, IUnitEquipmentHandler, IUnitActiveEquipmentSetHandler
    {
        public BlueprintArmorType BlueprintArmorType;
        private ModifiableValue.Modifier m_Modifier;


        public override void OnTurnOn()
        {
            base.OnTurnOn();
            CheckArmor();
        }

        public override void OnTurnOff()
        {
            base.OnTurnOff();
            DeactivateModifier();
        }

        public int CalcLevel()
        {
            return Owner.Progression.GetClassLevel(Helpers.fighterClass);
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem)
        {
            if(slot.Owner == base.Owner)
            {
                CheckArmor();
            }
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit)
        {
            CheckArmor();
        }
        public void CheckArmor()
        {
            if(base.Owner.Body.Armor.HasArmor && base.Owner.Body.Armor.Armor.Blueprint.Type == BlueprintArmorType)
            {
                ActivateModifier();
            }
            else
            {
                DeactivateModifier();
            }
        }

        public void ActivateModifier()
        {
            if(m_Modifier == null)
            {
                int charLevel = CalcLevel();
                int acIncrease = charLevel / 4;
                switch(base.Owner.Body.Armor.Armor.Blueprint.ProficiencyGroup)
                {
                    case ArmorProficiencyGroup.Heavy:
                        if(acIncrease > 5)
                        {
                            acIncrease = 5;
                        }
                        break;
                    case ArmorProficiencyGroup.Medium:
                        if(acIncrease > 4)
                        {
                            acIncrease = 4;
                        }
                        break;
                    case ArmorProficiencyGroup.Light:
                        if(acIncrease > 3)
                        {
                            acIncrease = 3;
                        }
                        break;
                }

                m_Modifier = base.Owner.Stats.AC.AddModifier(acIncrease, this, ModifierDescriptor.UntypedStackable);
            }
        }

        public void DeactivateModifier()
        {
            if(m_Modifier != null)
            {
                m_Modifier.Remove();
            }
            m_Modifier = null;
        }
    }
}
