using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.PubSubSystem;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    public class FocusedWeaponLogic :  ParametrizedFeatureComponent, IInitiatorRulebookHandler<RuleCalculateWeaponStats>
    {
        readonly Dictionary<int, DiceFormula> _diceFormularPerLevelDictionary = new Dictionary<int, DiceFormula>();

        public FocusedWeaponLogic()
        {
            _diceFormularPerLevelDictionary[1] = new DiceFormula(1, DiceType.D6);
            _diceFormularPerLevelDictionary[2] = new DiceFormula(1, DiceType.D6);
            _diceFormularPerLevelDictionary[3] = new DiceFormula(1, DiceType.D6);
            _diceFormularPerLevelDictionary[4] = new DiceFormula(1, DiceType.D6);
            _diceFormularPerLevelDictionary[5] = new DiceFormula(1, DiceType.D8);
            _diceFormularPerLevelDictionary[6] = new DiceFormula(1, DiceType.D8);
            _diceFormularPerLevelDictionary[7] = new DiceFormula(1, DiceType.D8);
            _diceFormularPerLevelDictionary[8] = new DiceFormula(1, DiceType.D8);
            _diceFormularPerLevelDictionary[9] = new DiceFormula(1, DiceType.D8);
            _diceFormularPerLevelDictionary[10] = new DiceFormula(1, DiceType.D10);
            _diceFormularPerLevelDictionary[11] = new DiceFormula(1, DiceType.D10);
            _diceFormularPerLevelDictionary[12] = new DiceFormula(1, DiceType.D10);
            _diceFormularPerLevelDictionary[13] = new DiceFormula(1, DiceType.D10);
            _diceFormularPerLevelDictionary[14] = new DiceFormula(1, DiceType.D10);
            _diceFormularPerLevelDictionary[15] = new DiceFormula(2, DiceType.D6);
            _diceFormularPerLevelDictionary[16] = new DiceFormula(2, DiceType.D6);
            _diceFormularPerLevelDictionary[17] = new DiceFormula(2, DiceType.D6);
            _diceFormularPerLevelDictionary[18] = new DiceFormula(2, DiceType.D6);
            _diceFormularPerLevelDictionary[19] = new DiceFormula(2, DiceType.D6);
            _diceFormularPerLevelDictionary[20] = new DiceFormula(2, DiceType.D8);
        }

        public int CalcLevel()
        {
            return Owner.Progression.GetClassLevel(Helpers.fighterClass);
        }

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if(evt.Weapon.Blueprint.Type.Category == Param.GetValueOrDefault().WeaponCategory)
            {
                var newDiceFormular = _diceFormularPerLevelDictionary[CalcLevel()];
                if(newDiceFormular.MaxValue(0)> evt.Weapon.Damage.MaxValue(0))
                {
                    evt.WeaponDamageDiceOverride = newDiceFormular;
                }
            }
        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
            
        }
    }
}
