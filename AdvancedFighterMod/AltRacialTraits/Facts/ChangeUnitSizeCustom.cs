using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;

namespace AdvancedMartialArts.AltRacialTraits.Facts
{
    internal class ChangeUnitSizeCustom : OwnedGameLogicComponent<UnitDescriptor>
    {
        public Size Size;

        public override void OnFactActivate()
        {
            base.Owner.State.Size = Size;
        }

        public override void OnFactDeactivate()
        {
            base.Owner.State.Size = base.Owner.OriginalSize;
        }

        public override void OnTurnOn()
        {
            base.Owner.State.Size = Size;

        }

        public override void OnTurnOff()
        {
            base.Owner.State.Size = base.Owner.OriginalSize;
        }
    }
}
