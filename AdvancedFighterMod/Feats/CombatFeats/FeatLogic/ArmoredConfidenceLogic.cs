using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedMartialArts.HelperClasses;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    class ArmoredConfidenceLogic : ArmorTypeLogic
    {
        private ModifiableValue.Modifier m_Modifier;
        public override void ActivateModifier()
        {
            if (m_Modifier == null)
            {
                int intimidateIncrease = 0;
                switch (base.Owner.Body.Armor.Armor.Blueprint.ProficiencyGroup)
                {
                    case ArmorProficiencyGroup.Heavy:
                        intimidateIncrease = 3;
                        break;
                    case ArmorProficiencyGroup.Medium:
                        intimidateIncrease = 2;
                        break;
                    case ArmorProficiencyGroup.Light:
                        intimidateIncrease = 1;
                        break;
                }
                var charLevel = CalcLevel();
                if (charLevel >= 7)
                {
                    ++intimidateIncrease;
                }
                if (charLevel >= 11)
                {
                    ++intimidateIncrease;
                }
                if (charLevel >= 15)
                {
                    ++intimidateIncrease;
                }
                if (charLevel >= 19)
                {
                    ++intimidateIncrease;
                }

                m_Modifier = base.Owner.Stats.CheckIntimidate.AddModifier(intimidateIncrease, this, ModifierDescriptor.Armor);

            }
        }

        public override void DeactivateModifier()
        {
            Main.logger.Log("ArmorTypeLogic DeactivateModifier!");
            if (m_Modifier != null)
            {
                m_Modifier.Remove();
            }
            m_Modifier = null;
        }
    }
}
