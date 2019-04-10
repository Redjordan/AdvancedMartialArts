using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Newtonsoft.Json;
using UnityEngine;

namespace AdvancedMartialArts.AltRacialTraits.Facts
{
    class AddFeatureParametrizedCustom : OwnedGameLogicComponent<UnitDescriptor>
    {
        private Feature feature;

        public BlueprintParametrizedFeature parametrizedFeature;
        public SpellSchool SpellSchool;

        public override void OnFactActivate()
        {
            feature = this.Owner.AddFact<Kingmaker.UnitLogic.Feature>((BlueprintUnitFact)parametrizedFeature, (MechanicsContext)null, SpellSchool);
        }

        public override void OnFactDeactivate()
        {
            if (feature != null)
            {
                this.Owner.RemoveFact(feature);
                feature = null;
            }
        }
    }
}
