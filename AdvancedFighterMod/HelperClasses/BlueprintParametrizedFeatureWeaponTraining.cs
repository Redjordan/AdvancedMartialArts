using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedMartialArts.Feats.CombatFeats;
using Harmony12;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.Enums;
using Kingmaker.Localization;

namespace AdvancedMartialArts.HelperClasses
{
    public class BlueprintParametrizedFeatureWeaponTraining : BlueprintParametrizedFeature
    {
        public BlueprintFeature WeaponTraining;


        [HarmonyPatch(typeof(BlueprintParametrizedFeatureWeaponTraining), "CanSelect", typeof(UnitDescriptor), typeof(LevelUpState), typeof(FeatureSelectionState), typeof(IFeatureSelectionItem))]
        public static class BlueprintAbilityResourceGetMaxAmountPatch
        {
            private static bool Prefix(BlueprintParametrizedFeatureWeaponTraining __instance, UnitDescriptor unit, LevelUpState state, FeatureSelectionState selectionState, IFeatureSelectionItem item, ref bool __result)
            {
                if (__instance.GetType() == typeof(BlueprintParametrizedFeatureWeaponTraining))
                {
                    if (item.Param.GetValueOrDefault().WeaponCategory != null && !AdvancedWeaponTraining._weaponTrainingToWeaponCategory[__instance.WeaponTraining].Contains((WeaponCategory)item.Param.GetValueOrDefault().WeaponCategory))
                    {
                        __result = false;
                        return false;
                    }
                }

                return true;
            }
        }
    }
}