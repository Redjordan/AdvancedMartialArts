using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony12;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic;

namespace AdvancedMartialArts.Feats.CombatFeats.FeatLogic
{
    class WarriorSpiritPoolLogic : BlueprintAbilityResource
    {
        public BlueprintFeature WeaponTraining;

        [HarmonyPatch(typeof(BlueprintAbilityResource), "GetMaxAmount", typeof(UnitDescriptor))]
        public static class BlueprintAbilityResourceGetMaxAmountPatch
        {
            private static bool Prefix(BlueprintAbilityResource __instance, UnitDescriptor unit, ref int __result)
            {
                if (__instance.GetType() == typeof(WarriorSpiritPoolLogic))
                {
                    __result = 0;
                    if (unit.Progression.GetClassLevel(Helpers.fighterClass) >= 5)
                    {
                        int basePool = 1;

                        int weaponTrainingValue = AdvancedWeaponTraining.GetWeaponTrainingRank(unit, ((WarriorSpiritPoolLogic)__instance).WeaponTraining);

                        __result = basePool + weaponTrainingValue;
                    }

                    return false;
                }

                return true;
            }
        }
    }
}