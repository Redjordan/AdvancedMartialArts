using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;

namespace AdvancedMartialArts.Traits.Logic
{
    internal class RiverRatDaggerTraitBonusLogic : OwnedGameLogicComponent<UnitDescriptor>, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, IInitiatorRulebookSubscriber
    {

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            WeaponCategory category = evt.Weapon.Blueprint.Type.Category;
            if(category != WeaponCategory.Dagger)
                return;
            evt.AddBonusDamage(1);
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }
    }
}
