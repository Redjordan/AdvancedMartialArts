using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Items;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.ActivatableAbilities.Restrictions;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    class ActivatableAbilityGroupSizeRestriction : ActivatableAbilityRestriction
    {
        public BlueprintActivatableAbility ActivatableAbility;
        public EnchantPoolType EnchantPool;

        public override bool IsAvailable()
        {
            int groupSize = base.Owner.Ensure<UnitPartActivatableAbility>().GetGroupSize(ActivatableAbility.Group);
            ItemEntity itemEntity = base.Owner.Body.PrimaryHand.HasWeapon ? (ItemEntity) base.Owner.Body.PrimaryHand.MaybeWeapon : (ItemEntity) base.Owner.Body.EmptyHandWeapon;

            int num1 = 0;
            if (itemEntity != null && itemEntity.Enchantments.Any<ItemEnchantment>())
            {
                foreach (WeaponEnhancementBonus enhancementBonus in itemEntity.Enchantments.SelectMany<ItemEnchantment, WeaponEnhancementBonus>((Func<ItemEnchantment, IEnumerable<WeaponEnhancementBonus>>) (e => e.SelectComponents<WeaponEnhancementBonus>())))
                    num1 += enhancementBonus.EnhancementBonus;
            }

            if (num1 == 0)
            {
                groupSize -= 1;
            }

            AddBondProperty propertyOfAbility = ActivatableAbility.Buff.GetComponent<AddBondProperty>();
            foreach (AddBondProperty selectFactComponent in base.Owner.Buffs.SelectFactComponents<AddBondProperty>())
            {
                if (selectFactComponent.Enchant == propertyOfAbility.Enchant)
                {
                    return true;
                }

                if (selectFactComponent.EnchantPool == this.EnchantPool && !itemEntity.HasEnchantment(selectFactComponent.Enchant))
                {
                    groupSize -= selectFactComponent.Enchant.EnchantmentCost;
                }
            }

            return groupSize >= propertyOfAbility.Enchant.EnchantmentCost;
        }
    }
}