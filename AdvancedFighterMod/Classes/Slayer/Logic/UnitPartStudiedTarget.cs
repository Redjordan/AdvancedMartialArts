using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;

namespace AdvancedMartialArts.Classes.Slayer.Logic
{
    class UnitPartStudiedTarget : UnitPart, IRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackRoll>, IInitiatorRulebookHandler<RuleAttackRoll>, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IInitiatorRulebookSubscriber
    {
        public UnitDescriptor Descriptor;
        public Fact Source;
        private int _bonus;

        public void SetTarget(UnitDescriptor descriptor)
        {
            this.Descriptor = descriptor;
        }

        public void SetFact(Fact source, int bonus)
        {
            this.Source = source;
            this._bonus = bonus;
        }

        public void RemoveEntry(Fact source)
        {
            if (source == this.Source)
            {
                Descriptor = null;
            }
        }

        public bool HasEntry(UnitDescriptor descriptor)
        {
            return this.Descriptor == descriptor;
        }

        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (evt.Target.Descriptor == Descriptor)
            {
                evt.AddTemporaryModifier(evt.Initiator.Stats.AdditionalAttackBonus.AddModifier(_bonus, Source, (string) null, ModifierDescriptor.FavoredEnemy));
            }
        }

        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if (evt.Target.Descriptor == Descriptor)
            {
                evt.AddTemporaryModifier(evt.Initiator.Stats.AdditionalDamage.AddModifier(_bonus, Source, (string) null, ModifierDescriptor.FavoredEnemy));
            }
        }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
        }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
        }
    }
}