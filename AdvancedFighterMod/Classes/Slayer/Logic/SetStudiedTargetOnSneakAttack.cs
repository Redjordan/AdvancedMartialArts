using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;

namespace AdvancedMartialArts.Classes.Slayer.Logic
{
    class SetStudiedTargetOnSneakAttack : RuleInitiatorLogicComponent<RuleCalculateDamage>
    {
        public override void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            var initiator = evt.Initiator;
            var target = evt.Target;
            var rule = Rulebook.Trigger(new RuleCheckTargetFlatFooted(initiator, target));

            if(evt.Target.CombatState.IsFlanked || rule.IsFlatFooted)
            {
                base.Owner.Ensure<UnitPartStudiedTarget>().SetTarget(target.Descriptor);
            }
        }

        public override void OnEventDidTrigger(RuleCalculateDamage evt)
        {
        }
    }
}
