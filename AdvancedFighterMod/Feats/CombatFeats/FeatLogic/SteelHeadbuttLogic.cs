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
            Main.logger.Log("SteelHeadbutt ActivateModifier headbuttSlot: " + headbuttSlot);
            if(headbuttSlot == null)
            {
                headbuttSlot = -1;
                Main.logger.Log("SteelHeadbutt ActivateModifier");
                int armorEnhancement = GameHelper.GetItemEnhancementBonus(base.Owner.Body.Armor.Armor);
                Main.logger.Log("SteelHeadbutt armorEnhancement: " + armorEnhancement);
                string assetID = Helpers.getGuid("SteelHeadbuttType" + base.Owner.Body.Armor.Armor.Blueprint.ProficiencyGroup + "Enhancement" + armorEnhancement);
                Main.logger.Log("SteelHeadbutt assetID: " + assetID);

                BlueprintItemWeapon headbutt = Main.library.Get<BlueprintItemWeapon>(assetID);
                Main.logger.Log("SteelHeadbutt headbutt: " + headbutt);
                headbuttSlot = base.Owner.Body.AddAdditionalLimb(headbutt, true);
                Main.logger.Log("SteelHeadbutt headbuttSlot: " + headbuttSlot);
            }
        }

        public void DeactivateModifier()
        {
            Main.logger.Log("SteelHeadbutt DeactivateModifier headbuttSlot: " + headbuttSlot);
            if(headbuttSlot != null)
            {
                base.Owner.Body.RemoveAdditionalLimb((int)headbuttSlot);
            }
            headbuttSlot = null;
        }
    }
}
