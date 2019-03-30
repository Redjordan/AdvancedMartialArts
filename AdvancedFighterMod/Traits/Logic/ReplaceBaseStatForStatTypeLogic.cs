using Harmony12;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;

namespace AdvancedMartialArts.Traits.Logic
{
    public class ReplaceBaseStatForStatTypeLogic : OwnedGameLogicComponent<UnitDescriptor>
    {
        public StatType StatTypeToReplaceBastStatFor;
        public StatType NewBaseStatType;
        private StatType? _oldStatType = null;

        public override void OnTurnOn()
        {
            ModifiableValue value = base.Owner.Stats.GetStat(StatTypeToReplaceBastStatFor);
            if(value.GetType() == typeof(ModifiableValueSkill))
            {
                if(_oldStatType == null)
                {
                    _oldStatType = ((ModifiableValueSkill)value).BaseStat.Type;
                }

                ModifiableValue oldStat = base.Owner.Stats.GetStat((StatType)_oldStatType);
                ModifiableValue newStat = base.Owner.Stats.GetStat(NewBaseStatType);

                Traverse traverse = Traverse.Create(value);
                traverse.Field("BaseStat").SetValue(newStat);
                newStat.AddDependentValue(value);
                oldStat.RemoveDependentValue(value);
                value.UpdateValue();
            }
        }

        public override void OnTurnOff()
        {
            ModifiableValue value = base.Owner.Stats.GetStat(StatTypeToReplaceBastStatFor);
            if(value.GetType() == typeof(ModifiableValueSkill) && _oldStatType != null)
            {
                ModifiableValue oldStat = base.Owner.Stats.GetStat((StatType)_oldStatType);
                ModifiableValue newStat = base.Owner.Stats.GetStat(NewBaseStatType);

                Traverse traverse = Traverse.Create(value);
                traverse.Field("BaseStat").SetValue(oldStat);
                oldStat.AddDependentValue(value);
                newStat.RemoveDependentValue(value);
                value.UpdateValue();
            }
        }
    }
}
