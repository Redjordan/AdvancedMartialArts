using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;

namespace AdvancedMartialArts.AltRacialTraits.Facts
{
    class AddStatBonusForFeatures : OwnedGameLogicComponent<UnitDescriptor>
    {
        public BlueprintFeature[] features;
        public StatType StatType;
        public ModifierDescriptor Descriptor;
        public int Value;
        private ModifiableValue.Modifier _modifier;

        public override void OnTurnOn()
        {
            foreach (var blueprintFeature in features)
            {
                if (base.Owner.Progression.Features.HasFact((BlueprintFact) blueprintFeature) && _modifier == null)
                {
                    _modifier = base.Owner.Stats.GetStat(StatType).AddModifier(Value, this, Descriptor);
                }
            }
        }

        public override void OnTurnOff()
        {
            if (_modifier != null)
            {
                base.Owner.Stats.GetStat(StatType).RemoveModifier(_modifier);
                _modifier = null;
            }
        }
    }
}
