using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;

namespace AdvancedMartialArts.Feats.GeneralFeats.Logic
{
    class NatureSoulLogic : OwnedGameLogicComponent<UnitDescriptor>, IGlobalSubscriber, ILevelUpCompleteUIHandler
    {
        private ModifiableValue.Modifier _modifier = null;

        public override void OnTurnOn()
        {
            int baseRank = base.Owner.Stats.SkillLoreNature.BaseValue;
            int value = baseRank >= 10 ? 4 : 2;
            if(_modifier == null)
            {
                _modifier = base.Owner.Stats.SkillLoreNature.AddModifier(value, this, ModifierDescriptor.Feat);
            }
        }

        public override void OnTurnOff()
        {
            if(_modifier != null)
            {
                base.Owner.Stats.SkillLoreNature.RemoveModifier(_modifier);
            }

            _modifier = null;
        }

        public void HandleLevelUpComplete(UnitEntityData unit, bool isChargen)
        {
            int baseRank = base.Owner.Stats.SkillLoreNature.BaseValue;
            int value = baseRank >= 10 ? 4 : 2;
            if(_modifier == null || _modifier.ModValue != value)
            {
                base.Owner.Stats.SkillLoreNature.RemoveModifier(_modifier);
                _modifier = base.Owner.Stats.SkillLoreNature.AddModifier(value, this, ModifierDescriptor.Feat);
            }
        }
    }
}