using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;
using AdvancedMartialArts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Blueprints.Classes;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    public class ArmoredJuggernautLogic : AddDamageResistancePhysical, IGlobalSubscriber, IUnitEquipmentHandler, IUnitActiveEquipmentSetHandler, IUnitGainLevelHandler
    {

        public ArmoredJuggernautLogic()
        {
            BypassedByAlignment = false;
            BypassedByMeleeWeapon = false;
            BypassedByWeaponType = false;
            BypassedByReality = false;
            BypassedByAlignment = false;
            BypassedByMagic = false;
            BypassedByForm = false;
            BypassedByMaterial = false;
            UsePool = false;
            SetValue();
        }
        public int CalcLevel()
        {
            return Owner.Progression.GetClassLevel(Helpers.fighterClass);
        }

        public override void OnFactActivate()
        {
            SetValue();
        }


        private void SetValue()
        {
            if(base.Owner != null)
            {
                var charLevel = CalcLevel();
                var armorProfienceGroup = base.Owner.Body.Armor.Armor.Blueprint.ProficiencyGroup;
                var dmgResistanceValue = 0;
                switch (armorProfienceGroup)
                {
                    case ArmorProficiencyGroup.Heavy:
                        dmgResistanceValue = 1;
                        break;
                    case ArmorProficiencyGroup.Medium:
                        dmgResistanceValue = 0;
                        break;
                    case ArmorProficiencyGroup.Light:
                        dmgResistanceValue = -1;
                        break;
                }
                if (charLevel >= 7)
                {
                    ++dmgResistanceValue;
                }
                if (charLevel >= 11)
                {
                    ++dmgResistanceValue;
                }
                if (dmgResistanceValue < 0)
                {
                    dmgResistanceValue = 0;
                }
                Value = dmgResistanceValue;
            }
        }

        public void CheckArmor()
        {
            if (base.Owner.Body.Armor.HasArmor)
            {
                ActivateModifier();
            }
            else
            {
                DeactivateModifier();
            }
        }

        private void DeactivateModifier()
        {
            SetValue();
            base.Owner.Get<UnitPartDamageReduction>()?.Remove(base.Fact);
        }

        private void ActivateModifier()
        {
            SetValue();
            base.Owner.Ensure<UnitPartDamageReduction>().Add(base.Fact);
        }

        public override void OnTurnOn()
        {
            CheckArmor();
        }

        public override void OnTurnOff()
        {
            CheckArmor();
        }

        public void HandleEquipmentSlotUpdated(ItemSlot slot, ItemEntity previousItem)
        {
            CheckArmor();
        }

        public void HandleUnitChangeActiveEquipmentSet(UnitDescriptor unit)
        {
            CheckArmor();
        }

        public void HandleUnitGainLevel(UnitDescriptor unit, BlueprintCharacterClass @class)
        {
            SetValue();
        }
    }
}
