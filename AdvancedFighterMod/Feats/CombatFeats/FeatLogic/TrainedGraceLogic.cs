using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    class TrainedGraceLogic : RuleInitiatorLogicComponent<RuleCalculateWeaponStats>
    {
        public BlueprintFeature WeaponFighterGroupFeature;
        public BlueprintFeature FightersFinesseFeature;

        public override void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if(AdvancedWeaponTraining.WieldsWeaponFromFighterGroup(this.Owner, WeaponFighterGroupFeature) && (
                   evt.Weapon.Blueprint.Category.HasSubCategory(WeaponSubCategory.Finessable) ||
                   base.Owner.Progression.Features.GetRank(FightersFinesseFeature) > 0))
            {
                ModifiableValueAttributeStat stat1 = this.Owner.Stats.GetStat(evt.Weapon.Blueprint.AttackBonusStat) as ModifiableValueAttributeStat;
                ModifiableValueAttributeStat stat2 = this.Owner.Stats.GetStat(StatType.Dexterity) as ModifiableValueAttributeStat;
                bool flag = stat2 != null && stat1 != null && stat2.Bonus >= stat1.Bonus;
                if(!flag)
                    return;
                int value = base.Owner.Progression.Features.GetRank(WeaponFighterGroupFeature);
                evt.AddBonusDamage(value);
            }
        }

        public override void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }
    }
}
