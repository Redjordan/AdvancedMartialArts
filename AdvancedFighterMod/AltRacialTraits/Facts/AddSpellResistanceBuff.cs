using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Parts;

namespace AdvancedMartialArts.AltRacialTraits.Facts
{
    internal class AddSpellResistanceBuff : OwnedGameLogicComponent<UnitDescriptor>
    {
        private int? m_AppliedId;
        public override void OnTurnOn()
        {
            int resistance = 5 + base.Owner.Progression.CharacterLevel;
            this.m_AppliedId = new int?(this.Owner.Ensure<UnitPartSpellResistance>().AddResistance(resistance, new AlignmentComponent?(), new SpellDescriptor?()));
        }

        public override void OnTurnOff()
        {
            if(!this.m_AppliedId.HasValue)
                return;
            this.Owner.Get<UnitPartSpellResistance>()?.Remove(this.m_AppliedId.Value);
            this.m_AppliedId = new int?();
        }
    }


}
