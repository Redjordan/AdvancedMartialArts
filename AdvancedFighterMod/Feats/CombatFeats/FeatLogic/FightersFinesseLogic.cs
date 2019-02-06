using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    class FightersFinesseLogic : RuleInitiatorLogicComponent<RuleCalculateAttackBonusWithoutTarget>
    {
        public BlueprintFeature WeaponTraining;

        public override void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            if(AdvancedWeaponTraining.WieldsWeaponFromFighterGroup(base.Owner, WeaponTraining) && !evt.Weapon.Blueprint.Category.HasSubCategory(WeaponSubCategory.Finessable))
            {
                ModifiableValueAttributeStat stat1 = this.Owner.Stats.GetStat(evt.AttackBonusStat) as ModifiableValueAttributeStat;
                ModifiableValueAttributeStat stat2 = this.Owner.Stats.GetStat(StatType.Dexterity) as ModifiableValueAttributeStat;
                bool flag = stat2 != null && stat1 != null && stat2.Bonus >= stat1.Bonus;
                if (!flag)
                    return;
                evt.AttackBonusStat = StatType.Dexterity;
            }
        }

        public override void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }
    }
}