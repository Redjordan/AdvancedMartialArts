using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;

namespace AdvancedMartialArts.Classes.Inquisitor.Logic
{
    class HolyBookLogic : RuleInitiatorLogicComponent<RulePrepareDamage>, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>

    {

        public WeaponCategory Category = WeaponCategory.LightMace;


        public override void OnEventAboutToTrigger(RulePrepareDamage evt)
        {
            if(evt.DamageBundle.Weapon.Blueprint.Category == Category)
            {
                foreach(BaseDamage baseDamage in evt.DamageBundle)
                    this.ApplyProperties(baseDamage as PhysicalDamage);
            }
        }

        private void ApplyProperties(PhysicalDamage damage)
        {
            damage.AddMaterial(PhysicalDamageMaterial.ColdIron);
        }

        public override void OnEventDidTrigger(RulePrepareDamage evt)
        {
        }

        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            if (evt.Weapon.Blueprint.Category == Category)
            {
                evt.AddBonus(1, Fact);
            }
        }

        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }
    }
}