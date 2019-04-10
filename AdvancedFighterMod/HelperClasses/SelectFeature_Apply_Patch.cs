using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;

namespace AdvancedMartialArts.HelperClasses
{
    // This adds support for a feat adding additional selections  (e.g. Additional Traits, Dragon Magic).
    //
    // The game doesn't natively support this, except via BlueprintProgression. However,
    // BlueprintProgression doesn't work for things you select later, because it only adds
    // the current level's features. Essentially, progressions are only designed to work for
    // class features awarded at fixed levels (typically 1st level). There isn't a notion of
    // progressions that are relative to the level you picked them at.
    //
    // So to support adding selections, we patch SelectFeature.Apply to add the follow-up features.
    //
    // However that wouldn't work for cases where a feat can change the progression level, as with
    // Greater Eldritch Heritage.
    //
    // TODO: alternative design2: use IUnitGainFactHandler. I think I tried that and it didn't work,
    // but don't recall why (unit not active during chargen?).
    [Harmony12.HarmonyPatch(typeof(SelectFeature), "Apply", new Type[] { typeof(LevelUpState), typeof(UnitDescriptor) })]
    public static class SelectFeature_Apply_Patch
    {
        internal static Dictionary<BlueprintFeature, Action<LevelUpState, UnitDescriptor>> onApplyFeature = new Dictionary<BlueprintFeature, Action<LevelUpState, UnitDescriptor>>();

        static SelectFeature_Apply_Patch() => Main.ApplyPatch(typeof(SelectFeature_Apply_Patch), "Features that add selections");

        static void Postfix(SelectFeature __instance, LevelUpState state, UnitDescriptor unit)
        {
            try
            {
                var self = __instance;
                var item = self.Item;
                if(item == null) return;

                Action<LevelUpState, UnitDescriptor> action;
                if(onApplyFeature.TryGetValue(item.Feature, out action))
                {
                    action(state, unit);
                }
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
