using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    internal class SteelHeadbuttLogic : OwnedGameLogicComponent<UnitDescriptor>, IGlobalSubscriber, IUnitEquipmentHandler, IUnitActiveEquipmentSetHandler
    {
        private int? headbuttSlot = null;

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
            if(base.Owner.Body.Armor.HasArmor &&
                (base.Owner.Body.Armor.Armor.Blueprint.ProficiencyGroup == ArmorProficiencyGroup.Heavy ||
                base.Owner.Body.Armor.Armor.Blueprint.ProficiencyGroup == ArmorProficiencyGroup.Medium))
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
            if(headbuttSlot == null)
            {
                headbuttSlot = -1;
                int armorEnhancement = GameHelper.GetItemEnhancementBonus(base.Owner.Body.Armor.Armor);
                string assetID = Helpers.getGuid("SteelHeadbuttType" + base.Owner.Body.Armor.Armor.Blueprint.ProficiencyGroup + "Enhancement" + armorEnhancement);

                BlueprintItemWeapon headbutt = Main.library.Get<BlueprintItemWeapon>(assetID);
                headbuttSlot = base.Owner.Body.AddAdditionalLimb(headbutt, true);
            }
        }

        public void DeactivateModifier()
        {
            if(headbuttSlot != null)
            {
                base.Owner.Body.RemoveAdditionalLimb((int)headbuttSlot);
            }
            headbuttSlot = null;
        }
    }
}
