using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    class SetSkillRankToBabLogic : OwnedGameLogicComponent<UnitDescriptor>, IUnitGainLevelHandler
    {

        public StatType type;
        private bool activated = false;

        public override void OnFactActivate()
        {
            if (!activated)
            {
                var LevelUp = Game.Instance.UI.CharacterBuildController.LevelUpController;
                if (base.Owner == LevelUp.Preview || base.Owner == LevelUp.Unit)
                {
                    var @class = LevelUp.State.SelectedClass;

                    LevelUp.State.ExtraSkillPoints += base.Owner.Stats.GetStat(type).BaseValue;

                    base.Owner.Stats.GetStat(type).BaseValue = base.Owner.Stats.BaseAttackBonus.BaseValue;
                    activated = true;
                }
                ModifiableValueSkill stat = this.Owner.Stats.GetStat<ModifiableValueSkill>(type);
                stat?.ClassSkill.Retain();
                stat?.UpdateValue();
            }
        }

        public override void OnFactDeactivate()
        {
            if (activated)
            {
                base.Owner.Stats.GetStat(type).BaseValue = 0;
            }
            ModifiableValueSkill stat = this.Owner.Stats.GetStat<ModifiableValueSkill>(type);
            stat?.ClassSkill.Release();
            stat?.UpdateValue();
            base.OnFactDeactivate();
        }

        public void HandleUnitGainLevel(UnitDescriptor unit, BlueprintCharacterClass @class)
        {
            base.Owner.Stats.GetStat(type).OnChanged -= checkSkillRanks;
            int currentLevel = base.Owner.Progression.GetClassLevel(@class);
            int currentLevelBABFromclass = @class.BaseAttackBonus.GetBonus(currentLevel);
            int previeusLevelBABFromclass = @class.BaseAttackBonus.GetBonus(currentLevel - 1);
            int nextLevelBAB = base.Owner.Stats.BaseAttackBonus.BaseValue - previeusLevelBABFromclass + currentLevelBABFromclass;

            base.Owner.Stats.GetStat(type).BaseValue = nextLevelBAB;

            base.Owner.Stats.GetStat(type).OnChanged += checkSkillRanks;
        }

        public override void OnRecalculate()
        {
            base.Owner.Stats.GetStat(type).BaseValue = base.Owner.Stats.BaseAttackBonus.BaseValue;
            base.OnRecalculate();
        }

        public override void OnTurnOff()
        {
        }

        public override void OnTurnOn()
        {
        }

        private void checkSkillRanks(ModifiableValue value, int i)
        {

            var LevelUp = Game.Instance.UI.CharacterBuildController.LevelUpController;
            if (base.Owner == LevelUp.Preview || base.Owner == LevelUp.Unit)
            {
                var @class = LevelUp.State.SelectedClass;

                int currentLevel = base.Owner.Progression.GetClassLevel(@class);
                int currentLevelBABFromclass = @class.BaseAttackBonus.GetBonus(currentLevel);
                int previeusLevelBABFromclass = @class.BaseAttackBonus.GetBonus(currentLevel - 1);
                int nextLevelBAB = base.Owner.Stats.BaseAttackBonus.BaseValue - previeusLevelBABFromclass + currentLevelBABFromclass;

                if (value.BaseValue > nextLevelBAB)
                {
                    LevelUp.State.ExtraSkillPoints += (value.BaseValue - nextLevelBAB);
                    value.BaseValue = nextLevelBAB;
                }
            }
        }
    }
}
