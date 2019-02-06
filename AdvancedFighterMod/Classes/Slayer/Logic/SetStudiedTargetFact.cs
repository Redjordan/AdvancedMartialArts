using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;

namespace AdvancedMartialArts.Classes.Slayer.Logic
{
    class SetStudiedTargetFact : OwnedGameLogicComponent<UnitDescriptor>
    {

        public override void OnTurnOn()
        {
            this.Owner.Ensure<UnitPartStudiedTarget>().SetFact(this.Fact, this.Fact.GetRank());
        }

        public override void OnTurnOff()
        {
            this.Owner.Ensure<UnitPartStudiedTarget>().RemoveEntry(this.Fact);
        }
    }
}
