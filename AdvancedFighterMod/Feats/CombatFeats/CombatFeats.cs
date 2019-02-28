using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedMartialArts.Feats.CombatFeats.FeatLogic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.FactLogic;

namespace AdvancedMartialArts.Feats.CombatFeats
{
    static class CombatFeats
    {

        private static LibraryScriptableObject library => Main.library;

        public static void Load()
        {
            Main.SafeLoad(CreateDivineFightingTechniques, "CreateDivineFightingTechniques");
        }

        private static void CreateDivineFightingTechniques()
        {
            BlueprintFeature desnaFeature = library.Get<BlueprintFeature>("2c0a3b9971327ba4d9d85354d16998c1"); // DesnaFeature
        BlueprintWeaponType _weaponType = Main.library.Get<BlueprintWeaponType>("5a939137fc039084580725b2b0845c3f"); // Starknife

        BlueprintFeature desnasShootingStar =  Helpers.CreateFeature(
                "DesnasShootingStar",
                "Desnas Shooting Star",
                "Among the divine fighting manuals of the Inner Sea, few are as ancient as Clamor of the Spheres, a collection of fighting techniques favored by Desna’s faithful. True to its name, the manual focuses on interpreting the chaos and sounds of combat, but nevertheless provides insightful and downright brilliant methods of defense with Desna’s favored weapon, using techniques that treat a fight with a starknife more as a beautiful dance than a battle. \nYou can add your Charisma bonus to attack rolls and damage rolls when wielding a starknife. If you do so, you don’t modify attack rolls and damage rolls with your starknife with your Strength modifier, your Dexterity modifier (if you have Weapon Finesse), or any other ability score (if you have an ability that allows you to modify attack rolls and damage rolls with that ability score)",
                Helpers.getGuid("DesnasShootingStar"),
                desnaFeature.Icon,
                FeatureGroup.CombatFeat,
                Helpers.Create<DesnasShootingStarLogic>(),
                desnaFeature.PrerequisiteFeature(),
                Helpers.Create<PrerequisiteAlignment>(x => x.Alignment = AlignmentMaskType.ChaoticGood),
                Helpers.Create<AddStartingEquipment>(x => x.CategoryItems = new []{ _weaponType.Category }));

            Helpers.AddCombatFeats(library, desnasShootingStar);

        }
    }
}
