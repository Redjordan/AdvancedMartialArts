using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;

namespace AdvancedMartialArts.Feats.GeneralFeats.Logic
{
    class AnimalAllyAdjustToLevelLogic : OwnedGameLogicComponent<UnitDescriptor>
    {
        private readonly BlueprintFeature _animalCompanionRank = Main.library.Get<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d");

        private void ManageBonus()
        {
            int charLevel = base.Owner.Progression.CharacterLevel;
            int rank = base.Owner.Progression.Features.GetRank(_animalCompanionRank);
            if (rank < charLevel - 3)
            {
                for(int i = 0; i < charLevel - 3; i++)
                {
                    base.Owner.Progression.Features.AddFeature(_animalCompanionRank, Helpers.GetMechanicsContext());
                }
            }
        }


        public override void OnTurnOn()
        {
            ManageBonus();
        }

        public override void OnTurnOff()
        {
            ManageBonus();
        }
    }
}