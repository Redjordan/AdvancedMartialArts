using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;

namespace AdvancedMartialArts.Race
{
    public class Races
    {
        private static LibraryScriptableObject library => Main.library;
        public static void Load()
        {
            Main.SafeLoad(AddGoblinRace, "AddGoblinRace");
        }

        private static void AddGoblinRace()
        {
            BlueprintRace Gnome = library.Get<BlueprintRace>("ef35a22c9a27da345a4528f0d5889157");
            BlueprintRace Goblin = library.Get<BlueprintRace>("9d168ca7100e9314385ce66852385451");
            Goblin.FemaleOptions.Hair = Gnome.FemaleOptions.Hair;
            Goblin.MaleOptions.Hair = Gnome.MaleOptions.Hair;
            BlueprintRace[] races = Game.Instance.BlueprintRoot.Progression.CharacterRaces;
            if (!races.Contains(Goblin))
            {
                Game.Instance.BlueprintRoot.Progression.CharacterRaces = races.AddToArray(Goblin);
            }

            BlueprintFeature stealthy = library.Get<BlueprintFeature>("610652378253d3845bb70f005c084daa");
            stealthy.ComponentsArray = new BlueprintComponent[]
            {
                StatType.SkillStealth.CreateAddStatBonus(4, ModifierDescriptor.Racial)
            };
        }
    }
}
