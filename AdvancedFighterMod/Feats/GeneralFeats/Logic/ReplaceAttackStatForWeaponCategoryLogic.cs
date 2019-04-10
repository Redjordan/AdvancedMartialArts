using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace AdvancedMartialArts.Feats.GeneralFeats.Logic
{
    class ReplaceAttackStatForWeaponCategoryLogic : OwnedGameLogicComponent<UnitDescriptor>, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>
    {
        public WeaponCategory WeaponCategory;
        public StatType StatType;
        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            if(evt.Weapon.Blueprint.Type.Category == WeaponCategory)
            {
                var statBonus = evt.Initiator.Descriptor.Stats.GetStat(StatType) as ModifiableValueAttributeStat;
                var existingStat = Owner.Unit.Descriptor.Stats.GetStat(evt.AttackBonusStat) as ModifiableValueAttributeStat;
                if(statBonus != null && (existingStat == null || statBonus.Bonus > existingStat.Bonus))
                {
                    evt.AttackBonusStat = StatType;
                }
            }
        }

        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            
        }
    }
}
