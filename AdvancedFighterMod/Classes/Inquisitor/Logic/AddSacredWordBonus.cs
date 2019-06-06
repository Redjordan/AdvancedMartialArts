using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedMartialArts.Classes.Inquisitor.Logic
{
    internal class AddSacredWordBonus : OwnedGameLogicComponent<UnitDescriptor>
    {
        public BlueprintItemEnchantment[] DefaultEnchantments = new BlueprintItemEnchantment[5];
        public EnchantPoolType EnchantPool;
        public ActivatableAbilityGroup Group;
        public ContextDurationValue DurationValue;

        public override void OnTurnOn()
        {
            Main.logger.Log("AddSacredWordBonus OnTurnOn");
            EnchantPoolDataDescription description = new EnchantPoolDataDescription();

            Rounds rounds = new Rounds(1);
            UnitEntityData maybeCaster = base.Owner.Unit;

            maybeCaster.Ensure<UnitPartEnchantPoolData>().ClearEnchantPool(this.EnchantPool);
            ItemEntity itemEntity = maybeCaster.Body.PrimaryHand.HasWeapon ? (ItemEntity)maybeCaster.Body.PrimaryHand.MaybeWeapon : (ItemEntity)maybeCaster.Body.EmptyHandWeapon;
            if(itemEntity == null)
                return;
            int num1 = 0;
            int groupSize = maybeCaster.Ensure<UnitPartActivatableAbility>().GetGroupSize(this.Group);
            description.EnchantedItem = itemEntity;
            description.EnchantPool = this.EnchantPool;
            if(itemEntity.Enchantments.Any<ItemEnchantment>())
            {
                foreach(WeaponEnhancementBonus enhancementBonus in itemEntity.Enchantments.SelectMany<ItemEnchantment, WeaponEnhancementBonus>((Func<ItemEnchantment, IEnumerable<WeaponEnhancementBonus>>)(e => e.SelectComponents<WeaponEnhancementBonus>())))
                    num1 += enhancementBonus.EnhancementBonus;
            }

            foreach(AddBondProperty selectFactComponent in maybeCaster.Buffs.SelectFactComponents<AddBondProperty>())
            {
                if(selectFactComponent.EnchantPool == this.EnchantPool && !itemEntity.HasEnchantment(selectFactComponent.Enchant))
                {
                    groupSize -= selectFactComponent.Enchant.EnchantmentCost;
                    description.Enchantments.Add(itemEntity.AddEnchantment(selectFactComponent.Enchant, null, new Rounds?(rounds)));
                }
            }

            int num2 = Math.Min(Math.Max(0, 5 - num1), groupSize);
            if(num2 > 0)
                description.Enchantments.Add(itemEntity.AddEnchantment(this.DefaultEnchantments[num2 - 1], null, new Rounds?(rounds)));
            maybeCaster.Ensure<UnitPartEnchantPoolData>().RecordEnchantPool(description);
            Main.logger.Log("AddSacredWordBonus end");


        }

        public override void OnTurnOff()
        {
            Main.logger.Log("AddSacredWordBonus OnTurnOff");
            MechanicsContext context = ElementsContext.GetData<MechanicsContext.Data>()?.Context;
            EnchantPoolDataDescription description = new EnchantPoolDataDescription();
            if(context == null)
            {
                UberDebug.LogError((UnityEngine.Object)this, (object)"Unable to apply buff: no context found", (object[])Array.Empty<object>());
            }
            else
            {
                UnitEntityData maybeCaster = context.MaybeCaster;
                maybeCaster.Remove<UnitPartEnchantPoolData>();
            }

        }
    }
}
