using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedMartialArts.HelperClasses;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;

namespace AdvancedMartialArts.AltRacialTraits.Facts
{
    [AllowedOn(typeof(BlueprintParametrizedFeature))]
    public class IncreaseCasterLevelUpToCharacterLevel : OwnedGameLogicComponent<UnitDescriptor>, IInitiatorRulebookHandler<RuleCalculateAbilityParams>
    {
        public int MaxBonus = 2;
        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            var spellbook = evt.Spellbook;
            if(spellbook == null) return;

            int bonus = GetBonus(spellbook);
            Log.Write($"Increase caster level of {evt.Spell?.name} by {bonus}");
            evt.AddBonusCasterLevel(bonus);
        }

        public void OnEventDidTrigger(RuleCalculateAbilityParams evt) { }

        internal int GetBonus(Spellbook spellbook)
        {
            return Math.Min(spellbook.Owner.Progression.CharacterLevel - spellbook.CasterLevel, MaxBonus);
        }

        static IncreaseCasterLevelUpToCharacterLevel()
        {
            Main.ApplyPatch(typeof(SpellBookCharacteristics_Setup_Patch), "Increase Caster Level Up To Character Level in spellbook UI");
        }
    }
}
