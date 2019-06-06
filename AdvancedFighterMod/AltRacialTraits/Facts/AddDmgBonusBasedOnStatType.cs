using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;

namespace AdvancedMartialArts.AltRacialTraits.Facts
{
    class AddDmgBonusBasedOnStatType : RuleInitiatorLogicComponent<RuleCalculateWeaponStats>
    {
        public StatType Stat = StatType.Strength;
        public WeaponCategory Category = WeaponCategory.UnarmedStrike; 
        public float Multiplier = 0;

        public override void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            ModifiableValueAttributeStat stat = evt.Initiator.Descriptor.Stats.GetStat(this.Stat) as ModifiableValueAttributeStat;
            ModifiableValueAttributeStat valueAttributeStat = !evt.DamageBonusStat.HasValue ? (ModifiableValueAttributeStat)null : evt.Initiator.Descriptor.Stats.GetStat(evt.DamageBonusStat.Value) as ModifiableValueAttributeStat;
            if(stat.Type != valueAttributeStat.Type && (stat == null || valueAttributeStat != null && stat.Bonus <= valueAttributeStat.Bonus || evt.Weapon.Blueprint.Type.Category != this.Category))
                return;
            float bonusDmg = Multiplier * stat.Bonus;
            evt.AddBonusDamage((int)bonusDmg);
        }

        public override void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }
    }
}
