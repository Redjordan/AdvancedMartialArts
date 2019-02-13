using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Object = UnityEngine.Object;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    class ArmedBraveryLogic : RuleInitiatorLogicComponent<RuleSavingThrow>
    {
        public static BlueprintAbility DazzlingDisplayAction = Main.library.Get<BlueprintAbility>("5f3126d4120b2b244a95cb2ec23d69fb");
        public static BlueprintAbility PersuasionUseAbility = Main.library.Get<BlueprintAbility>("7d2233c3b7a0b984ba058a83b736e6ac");

        private ModifiableValue.Modifier modifier = null;
        private int IntimidateBonus = 0;

        public BlueprintFeature WeaponTraining;


        private void CheckWeapons()
        {
            if (AdvancedWeaponTraining.WieldsWeaponFromFighterGroup(base.Owner, WeaponTraining))
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }

        private void Activate()
        {
            if (modifier == null)
            {
                int value = base.Owner.Progression.Features.GetRank(AdvancedWeaponTraining.BraveryFeature);
                modifier = base.Owner.Stats.SaveWill.AddModifier(value, this, ModifierDescriptor.UntypedStackable);
                IntimidateBonus = value * 2;
            }
        }

        private void Deactivate()
        {
            if (modifier != null)
            {
                base.Owner.Stats.SaveWill.RemoveModifier(modifier);
            }

            modifier = null;
            IntimidateBonus = 0;
        }

        public override void OnTurnOn()
        {
            CheckWeapons();
            base.OnTurnOn();
        }

        public override void OnTurnOff()
        {
            CheckWeapons();
            base.OnTurnOff();
        }

        public override void OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            if (AdvancedWeaponTraining.WieldsWeaponFromFighterGroup(base.Owner, WeaponTraining))
            {
                BlueprintAbility sourceAbility = evt.Reason.Context?.SourceAbility;
                UnitEntityData maybeCaster = evt.Reason.Context?.MaybeCaster;
                if (!((UnityEngine.Object) sourceAbility != (Object) null) || !sourceAbility == DazzlingDisplayAction || sourceAbility == PersuasionUseAbility)
                    return;
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(this.IntimidateBonus, (GameLogicComponent) this, ModifierDescriptor.UntypedStackable));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(this.IntimidateBonus, (GameLogicComponent) this, ModifierDescriptor.UntypedStackable));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(this.IntimidateBonus, (GameLogicComponent) this, ModifierDescriptor.UntypedStackable));
            }
        }

        public override void OnEventDidTrigger(RuleSavingThrow evt)
        {
        }
    }
}