using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic;

namespace AdvancedMartialArts.HelperClasses
{
    internal class RemoveFeature : OwnedGameLogicComponent<UnitDescriptor>
    {
        public BlueprintUnitFact Feature;

        public override void OnFactActivate()
        {
            this.Owner.RemoveFact(this.Feature);
        }

        public override void OnTurnOn()
        {
            this.Owner.RemoveFact(this.Feature);
        }

        public override void OnTurnOff()
        {
            this.Owner.AddFact(this.Feature);
        }
    }
}
