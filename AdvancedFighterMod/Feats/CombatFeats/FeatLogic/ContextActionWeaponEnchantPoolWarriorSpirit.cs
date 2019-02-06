using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    class ContextActionWeaponEnchantPoolWarriorSpirit : ContextActionWeaponEnchantPool
    {
        public BlueprintFeature Feature;

        public override void RunAction()
        {
            MechanicsContext context = ElementsContext.GetData<MechanicsContext.Data>()?.Context;
            EnchantPoolDataDescription description = new EnchantPoolDataDescription();
            if (context == null)
            {
                UberDebug.LogError((UnityEngine.Object) this, (object) "Unable to apply buff: no context found", (object[]) Array.Empty<object>());
            }
            else
            {
                Rounds rounds = this.DurationValue.Calculate(context);
                UnitEntityData maybeCaster = context.MaybeCaster;
                if (maybeCaster == null)
                {
                    UberDebug.LogError((UnityEngine.Object) this, (object) "Can't apply buff: target is null", (object[]) Array.Empty<object>());
                }
                else
                {
                    maybeCaster.Ensure<UnitPartEnchantPoolData>().ClearEnchantPool(this.EnchantPool);
                    ItemEntity itemEntity = maybeCaster.Body.PrimaryHand.HasWeapon ? (ItemEntity) maybeCaster.Body.PrimaryHand.MaybeWeapon : (ItemEntity) maybeCaster.Body.EmptyHandWeapon;
                    if (itemEntity == null)
                        return;
                    int num1 = 0;
                    int groupSize = maybeCaster.Descriptor.Progression.Features.GetRank(Feature);
                    Main.logger.Log("groupSize" + groupSize);
                    description.EnchantedItem = itemEntity;
                    description.EnchantPool = this.EnchantPool;
                    if (itemEntity.Enchantments.Any<ItemEnchantment>())
                    {
                        foreach (WeaponEnhancementBonus enhancementBonus in itemEntity.Enchantments.SelectMany<ItemEnchantment, WeaponEnhancementBonus>((Func<ItemEnchantment, IEnumerable<WeaponEnhancementBonus>>) (e => e.SelectComponents<WeaponEnhancementBonus>())))
                            num1 += enhancementBonus.EnhancementBonus;
                    }

                    foreach (AddBondProperty selectFactComponent in maybeCaster.Buffs.SelectFactComponents<AddBondProperty>())
                    {
                        if (selectFactComponent.EnchantPool == this.EnchantPool && !itemEntity.HasEnchantment(selectFactComponent.Enchant))
                        {
                            groupSize -= selectFactComponent.Enchant.EnchantmentCost;
                            description.Enchantments.Add(itemEntity.AddEnchantment(selectFactComponent.Enchant, context, new Rounds?(rounds)));
                        }
                    }

                    int num2 = Math.Min(Math.Max(0, 5 - num1), groupSize);
                    if (num2 > 0)
                        description.Enchantments.Add(itemEntity.AddEnchantment(this.DefaultEnchantments[num2 - 1], context, new Rounds?(rounds)));
                    maybeCaster.Ensure<UnitPartEnchantPoolData>().RecordEnchantPool(description);
                }
            }
        }
    }
}