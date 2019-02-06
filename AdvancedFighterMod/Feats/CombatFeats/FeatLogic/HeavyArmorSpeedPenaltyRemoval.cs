using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    public class HeavyArmorSpeedPenaltyRemoval : OwnedGameLogicComponent<UnitDescriptor>
    {
        public override void OnTurnOn()
        {
            base.OnTurnOn();
            base.Owner.State.Features.ImmuneToArmorSpeedPenalty.Retain();
            if(base.Owner.Body.Armor.HasArmor)
            {
                base.Owner.Body.Armor.Armor.RecalculateStats();
            }
        }

        public override void OnTurnOff()
        {
            base.OnTurnOff();
            base.Owner.State.Features.ImmuneToArmorSpeedPenalty.Release();
            if(base.Owner.Body.Armor.HasArmor)
            {
                base.Owner.Body.Armor.Armor.RecalculateStats();
            }
        }
    }

}
