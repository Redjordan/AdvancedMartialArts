using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    class DesnasShootingStarLogic : OwnedGameLogicComponent<UnitDescriptor>, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>
    {
        private readonly BlueprintWeaponType _weaponType = Main.library.Get<BlueprintWeaponType>("5a939137fc039084580725b2b0845c3f"); // Starknife

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (evt.Weapon.Blueprint.Type == _weaponType)
            {
                var charisma = evt.Initiator.Descriptor.Stats.Charisma;
                var existingStat = !evt.DamageBonusStat.HasValue ? null : (Owner.Unit.Descriptor.Stats.GetStat(evt.DamageBonusStat.Value) as ModifiableValueAttributeStat);
                if(charisma != null && (existingStat == null || charisma.Bonus > existingStat.Bonus))
                {
                    evt.OverrideDamageBonusStat(StatType.Charisma);
                }
            }
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }

        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            if(evt.Weapon.Blueprint.Type == _weaponType)
            {
                var charisma = evt.Initiator.Descriptor.Stats.Charisma;
                var existingStat = Owner.Unit.Descriptor.Stats.GetStat(evt.AttackBonusStat) as ModifiableValueAttributeStat;
                if(charisma != null && (existingStat == null || charisma.Bonus > existingStat.Bonus))
                {
                    evt.AttackBonusStat = StatType.Charisma;
                }
            }
        }

        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }
    }
}
