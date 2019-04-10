using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedMartialArts.AltRacialTraits.Facts;
using Kingmaker;
using Kingmaker.UI.ServiceWindow;

namespace AdvancedMartialArts.HelperClasses
{
    [Harmony12.HarmonyPatch(typeof(SpellBookCharacteristics), "Setup", new Type[0])]
    static class SpellBookCharacteristics_Setup_Patch
    {
        static void Postfix(SpellBookCharacteristics __instance)
        {
            var self = __instance;
            try
            {
                var controller = Game.Instance.UI.SpellBookController;
                var spellbook = controller.CurrentSpellbook;
                if(spellbook != null && spellbook.CasterLevel > 0)
                {
                    int bonus = 0;
                    foreach(var feat in spellbook.Owner.Progression.Features.Enumerable)
                    {
                        foreach(var c in feat.SelectComponents<IncreaseCasterLevelUpToCharacterLevel>())
                        {
                            //bonus = Math.Max(bonus, c.GetBonus(spellbook));
                            bonus += c.GetBonus(spellbook);
                        }
                    }
                    if(bonus > 0)
                    {
                        self.CasterLevel.text = (spellbook.CasterLevel + bonus).ToString();
                        self.Concentration.text = (spellbook.GetConcentration() + bonus).ToString();
                    }
                }
            }
            catch(Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
