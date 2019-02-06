using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.PubSubSystem;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Parts;

namespace AdvancedMartialArts.HelperClasses
{

    [ComponentName("Add AC if owner has specific armor")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    [AllowMultipleComponents]
    public abstract class ArmorTypeLogic : OwnedGameLogicComponent<UnitDescriptor>, IGlobalSubscriber, IUnitEquipmentHandler, IUnitActiveEquipmentSetHandler
    {
        public ArmorProficiencyGroup armorProficiencyGroup  = ArmorProficiencyGroup.None;

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
            if (slot.Owner == base.Owner)
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
            if (base.Owner.Body.Armor.HasArmor && (armorProficiencyGroup == ArmorProficiencyGroup.None || base.Owner.Body.Armor.Armor.ArmorType() == armorProficiencyGroup))
            {
                ActivateModifier();
            }
            else
            {
                DeactivateModifier();
            }
        }

        public abstract void ActivateModifier();

        public abstract void DeactivateModifier();
        
    }
}
