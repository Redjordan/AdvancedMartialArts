using Harmony12;
using Kingmaker.Localization;

namespace AdvancedMartialArts.HelperClasses
{
    internal class FakeLocalizedString : LocalizedString
    {
        private readonly string fakeValue;

        public FakeLocalizedString(string fakeValue)
        {
            this.fakeValue = fakeValue;
            Traverse.Create(this).Field("m_Key").SetValue(fakeValue);
        }

        [HarmonyPatch(typeof(LocalizedString), "LoadString")]
        private static class LocalizedStringLoadStringPatch
        {
            private static bool Prefix(LocalizedString __instance, ref string __result)
            {
                if(__instance is FakeLocalizedString fake)
                {
                    __result = fake.fakeValue;
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(LocalizedString), "IsSet")]
        private static class LocalizedStringIsSetPatch
        {
            private static bool Prefix(LocalizedString __instance, ref bool __result)
            {
                if(__instance is FakeLocalizedString fake)
                {
                    __result = !string.IsNullOrEmpty(fake.fakeValue);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(LocalizedString), "IsEmpty")]
        private static class LocalizedStringIsEmptyPatch
        {
            private static bool Prefix(LocalizedString __instance, ref bool __result)
            {
                if(__instance is FakeLocalizedString fake)
                {
                    __result = string.IsNullOrEmpty(fake.fakeValue);
                    return false;
                }
                return true;
            }
        }

    }
}
