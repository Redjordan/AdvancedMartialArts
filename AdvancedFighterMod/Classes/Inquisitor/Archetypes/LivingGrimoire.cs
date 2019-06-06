using AdvancedMartialArts.Classes.Inquisitor.Logic;
using AdvancedMartialArts.Feats.CombatFeats.FeatLogic;
using AdvancedMartialArts.HelperClasses;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvancedMartialArts.Classes.Inquisitor.Archetypes
{
    internal class LivingGrimoire
    {
        private static LibraryScriptableObject library => Main.library;

        private static BlueprintArchetype livingGrimoire;
        internal static BlueprintArchetype[] livingGrimoireArray;

        public static void Load()
        {
            BlueprintCharacterClass inquisitor = library.Get<BlueprintCharacterClass>("f1a70d9e1b0b41e49874e1fa9052a1ce"); // Inquisitor
            BlueprintProgression inquisitorProgression = library.Get<BlueprintProgression>("4e945c2fe5e252f4ea61eee7fb560017"); // InquisitorProgression

            livingGrimoire = Helpers.Create<BlueprintArchetype>(l =>
            {
                l.name = "LivingGrimoireArchetype";
                l.LocalizedName = Helpers.CreateString("LivingGrimoire.Name", "Living Grimoire");
                l.LocalizedDescription = Helpers.CreateString("LivingGrimoire.Description", "The living grimoire literally wields the sacred word of his deity, using his holy tome to smite the foes of his god with divine might. Unlike most inquisitors, a living grimoire focuses on careful study of divine scripture, valuing knowledge over intuition.");

            });

            livingGrimoireArray = new BlueprintArchetype[] { livingGrimoire };
            library.AddAsset(livingGrimoire, Helpers.getGuid("LivingGrimoireArchetype"));
            livingGrimoire.SetIcon(inquisitor.Icon);
            livingGrimoire.BaseAttackBonus = inquisitor.BaseAttackBonus;
            livingGrimoire.FortitudeSave = inquisitor.FortitudeSave;
            livingGrimoire.ReflexSave = inquisitor.ReflexSave;
            livingGrimoire.WillSave = inquisitor.WillSave;

            Helpers.SetField(livingGrimoire, "m_ParentClass", inquisitor);

            livingGrimoire.LocalizedName = Helpers.CreateString("LivingGrimoire.Name", "Living Grimoire");

            BlueprintSpellbook MagusSpellbook = library.Get<BlueprintSpellbook>("5d8d04e76dff6c5439de99af0d57be63"); // MagusSpellbook
            BlueprintSpellbook InquisitorSpellbook = library.Get<BlueprintSpellbook>("57fab75111f377248810ece84193a5a5"); // InquisitorSpellbook
            BlueprintSpellbook spellbook = Helpers.Create<BlueprintSpellbook>();
            spellbook.name = "LivingGrimoireSpellbook";
            library.AddAsset(spellbook, Helpers.getGuid("LivingGrimoireSpellbook"));
            spellbook.Name = livingGrimoire.LocalizedName;
            spellbook.SpellsPerDay = MagusSpellbook.SpellsPerDay;
            spellbook.SpellList = InquisitorSpellbook.SpellList;
            spellbook.CharacterClass = inquisitor;
            spellbook.CastingAttribute = StatType.Intelligence;
            spellbook.Spontaneous = false;
            spellbook.SpellsPerLevel = 2;
            spellbook.AllSpellsKnown = false;
            spellbook.CanCopyScrolls = true;
            spellbook.IsArcane = false;
            spellbook.CantripsType = CantripsType.Orisions;


            livingGrimoire.ReplaceSpellbook = spellbook;

            BlueprintItem[] items = new BlueprintItem[]
            {
                (BlueprintItem) ((IList<BlueprintCategoryDefaults.CategoryDefaultEntry>) Game.Instance.BlueprintRoot.Progression.CategoryDefaults.Entries).FirstOrDefault<BlueprintCategoryDefaults.CategoryDefaultEntry>((Func<BlueprintCategoryDefaults.CategoryDefaultEntry, bool>) (p => p.Key == WeaponCategory.LightMace))?.DefaultWeapon,
                library.Get<BlueprintItem>("f4cef3ba1a15b0f4fa7fd66b602ff32b"),
                library.Get<BlueprintItem>("d7963e1fcf260c148877afd3252dbc91"),
                library.Get<BlueprintItem>("ec619484fb1a13441b30f6d08e1c5b6f")
            };

            livingGrimoire.StartingItems = items;


            BlueprintFeature HolyBookFeature = Helpers.CreateFeature(
                "HolyBook",
                "Holy Book",
                "At 1st level, a living grimoire forms a supernatural bond with a large ironbound tome containing the holy text of his deity and learns to use it as a weapon.\n When wielding the holy book as a weapon, he deals base damage as if it were a cold iron light mace(but see Sacred Word below), is considered proficient with the book, takes no improvised weapon penalty, and gains a + 1 bonus on attack rolls with the book.The tome serves as his holy symbol and divine focus, and can be enchanted as a magic weapon. \nHe can replace his bonded tome with another book at any time, though he must perform a 24 - hour binding ritual to attune himself to the new book. \n Dev note: This ability works with all light maces.",
                Helpers.getGuid("HolyBook"),
                null,
                    FeatureGroup.None,
                Helpers.Create<HolyBookLogic>()
                );

            BlueprintFeature SacredWordFeature = Helpers.CreateFeature(
                "SacredWord",
                "Sacred Word",
                "At 1st (level, a living grimoire learns to charge his holy book with the power of his faith. The inquisitor gains the benefits of the warpriest’s sacred weapon class ability, but the benefits apply only to his bonded holy book. Like a warpriest’s sacred weapon, the living grimoire’s book deals damage based on the inquisitor’s level, not the book’s base damage (unless the inquisitor chooses to use the book’s base damage).\nAt 4th level, the living grimoire gains the ability to enhance his holy book with divine power as a swift action.This ability grants the holy book a + 1 enhancement bonus.For every 4 inquisitor levels the living grimoire has beyond 4th, this bonus increases by 1(to a maximum of + 5 at 20th level).\nThese bonuses stack with any existing bonuses the holy book might have, to a maximum of + 5.The living grimoire can enhance his holy book to have any of the special abilities listed in the warpriest’s sacred weapon ability, subject to the same alignment restrictions, but adds bane to the general special ability list.Adding any of these special abilities to the holy book consumes an amount of enhancement bonus equal to the special ability’s base price modifier.The holy book must have at least a + 1 enhancement bonus before the living grimoire can add any special abilities to it.The living grimoire can use this ability a number of rounds per day equal to his inquisitor level, but these rounds don’t need to be consecutive.As with the warpriest sacred weapon ability, he determines the enhancement bonus and special abilities the first time he uses the ability each day, and they cannot be changed until the next day. \nThis ability replaces judgment.\n Dev note: This ability works with all light maces.",
                Helpers.getGuid("SacredWordFeature"),
                null,
                FeatureGroup.None,
                Helpers.Create<FocusedWeaponLogic>(x =>
                {
                    x.Category = WeaponCategory.LightMace;
                    x.Class = inquisitor;
                })
            );

            BlueprintFeature SacredWordFeature1d8 = Helpers.CreateFeature(
                "SacredWord_1d8",
                "Sacred Word 1d8",
                "The damage of your holy book (light maces) increases to 1d8.",
                Helpers.getGuid("SacredWord_1d8"),
                null,
                FeatureGroup.None
            );

            BlueprintFeature SacredWordFeature1d10 = Helpers.CreateFeature(
                "SacredWord_1d10",
                "Sacred Word 1d10",
                "The damage of your holy book (light maces) increases to 1d10.",
                Helpers.getGuid("SacredWord_1d10"),
                null,
                FeatureGroup.None
            );

            BlueprintFeature SacredWordFeature2d6 = Helpers.CreateFeature(
                "SacredWord_2d6",
                "Sacred Word 2d6",
                "The damage of your holy book (light maces) increases to 2d6.",
                Helpers.getGuid("SacredWord_2d6"),
                null,
                FeatureGroup.None
            );

            BlueprintFeature SacredWordFeature2d8 = Helpers.CreateFeature(
                "SacredWord_2d8",
                "Sacred Word 2d8",
                "The damage of your holy book (light maces) increases to 2d8.",
                Helpers.getGuid("SacredWord_2d8"),
                null,
                FeatureGroup.None
            );


            BlueprintBuff InspireCourageBuff = library.Get<BlueprintBuff>("b4027a834204042409248889cc8abf67");
            BlueprintActivatableAbility InspireCourageToggleAbility = library.Get<BlueprintActivatableAbility>("5250fe10c377fdb49be449dfe050ba70"); // InspireCourageToggleAbility

            BlueprintItemEnchantment[] defaultEnchantments = new BlueprintItemEnchantment[]
            {
                library.Get<BlueprintItemEnchantment>("d704f90f54f813043a525f304f6c0050"),
                library.Get<BlueprintItemEnchantment>("9e9bab3020ec5f64499e007880b37e52"),
                library.Get<BlueprintItemEnchantment>("d072b841ba0668846adeb007f623bd6c"),
                library.Get<BlueprintItemEnchantment>("6a6a0901d799ceb49b33d4851ff72132"),
                library.Get<BlueprintItemEnchantment>("746ee366e50611146821d61e391edf16")
            };

            BlueprintBuff SacredWordBuff = Helpers.CreateBuff(
                "SacredWordBuff",
                "Sacred Word",
                "",
                Helpers.getGuid("SacredWordBuff"),
                null,
                InspireCourageBuff.FxOnStart,
                Helpers.Create<AddSacredWordBonus>(x =>
                {
                    x.DefaultEnchantments = defaultEnchantments;
                    x.DurationValue = Helpers.CreateContextDuration(rate: DurationRate.Rounds, bonus: 1);
                    x.Group = ActivatableAbilityGroup.DivineWeaponProperty;
                    x.EnchantPool = EnchantPoolType.DivineWeaponBond;
                })
            );

            BlueprintAbilityResource abilityResource = Helpers.CreateAbilityResource(
                "SacredWordResource",
                "Sacred Word Resource",
                "",
                Helpers.getGuid("SacredWordResource"),
                null
                );
            abilityResource.SetIncreasedByLevel(1, 1, new BlueprintCharacterClass[] { inquisitor });

            BlueprintAbility WeaponBondSwitchAbility = library.Get<BlueprintAbility>("7ff088ab58c69854b82ea95c2b0e35b4"); //WeaponBondSwitchAbility
            BlueprintActivatableAbility SacredWordToggleAbility = Helpers.CreateActivatableAbility(
                "SacredWordSwitchAbility",
                "Sacred Word",
                "",
                Helpers.getGuid("SacredWordSwitchAbility"),
                WeaponBondSwitchAbility.Icon,
                SacredWordBuff,
                AbilityActivationType.WithUnitCommand,
                UnitCommand.CommandType.Swift,
                InspireCourageToggleAbility.ActivateWithUnitAnimation,
                abilityResource.CreateActivatableResourceLogic(ActivatableAbilityResourceLogic.ResourceSpendType.NewRound)
                );
            SacredWordToggleAbility.DeactivateIfCombatEnded = true;
            SacredWordToggleAbility.DeactivateIfOwnerDisabled = true;


            BlueprintActivatableAbility SacredWordFrost = library.CopyAndAdd<BlueprintActivatableAbility>("b338e43a8f81a2f43a73a4ae676353a5", "SacredWordFrost", Helpers.getGuid("SacredWordFrost")); // ArcaneWeaponFrostChoice
            BlueprintActivatableAbility SacredWordShock = library.CopyAndAdd<BlueprintActivatableAbility>("a3a9e9a2f909cd74e9aee7788a7ec0c6", "SacredWordShock", Helpers.getGuid("SacredWordShock")); // ArcaneWeaponShockChoice
            BlueprintActivatableAbility SacredWordAnarchic = library.CopyAndAdd<BlueprintActivatableAbility>("8ed07b0cc56223c46953348f849f3309", "SacredWordAnarchic", Helpers.getGuid("SacredWordAnarchic")); // ArcaneWeaponAnarchicChoice
            BlueprintActivatableAbility SacredWordGhostTouch = library.CopyAndAdd<BlueprintActivatableAbility>("688d42200cbb2334c8e27191c123d18f", "SacredWordGhostTouch", Helpers.getGuid("SacredWordGhostTouch")); // ArcaneWeaponGhostTouchChoice
            BlueprintActivatableAbility SacredWordUnholy = library.CopyAndAdd<BlueprintActivatableAbility>("561803a819460f34ea1fe079edabecce", "SacredWordUnholy", Helpers.getGuid("SacredWordUnholy")); // ArcaneWeaponUnholyChoice
            BlueprintActivatableAbility SacredWordBane = library.CopyAndAdd<BlueprintActivatableAbility>("3a909d1effa3bbc4084f2b5ac95f5306", "SacredWordBane", Helpers.getGuid("SacredWordBane")); // ArcaneWeaponUnholyChoice
            SacredWordFrost.Group = ActivatableAbilityGroup.DivineWeaponProperty;
            SacredWordShock.Group = ActivatableAbilityGroup.DivineWeaponProperty;
            SacredWordAnarchic.Group = ActivatableAbilityGroup.DivineWeaponProperty;
            SacredWordGhostTouch.Group = ActivatableAbilityGroup.DivineWeaponProperty;
            SacredWordUnholy.Group = ActivatableAbilityGroup.DivineWeaponProperty;
            SacredWordBane.Group = ActivatableAbilityGroup.DivineWeaponProperty;
            SacredWordBane.WeightInGroup = 1;



            BlueprintFeature SacredWordFeatureLevel4 = Helpers.CreateFeature(
               "SacredWord_FeatureLevel4",
               "Sacred Word",
               "At 4th level, the living grimoire gains the ability to enhance his holy book with divine power as a swift action.This ability grants the holy book a + 1 enhancement bonus.For every 4 inquisitor levels the living grimoire has beyond 4th, this bonus increases by 1(to a maximum of + 5 at 20th level).\nThese bonuses stack with any existing bonuses the holy book might have, to a maximum of + 5.The living grimoire can enhance his holy book to have any of the special abilities listed in the warpriest’s sacred weapon ability, subject to the same alignment restrictions, but adds bane to the general special ability list.Adding any of these special abilities to the holy book consumes an amount of enhancement bonus equal to the special ability’s base price modifier.The holy book must have at least a + 1 enhancement bonus before the living grimoire can add any special abilities to it.The living grimoire can use this ability a number of rounds per day equal to his inquisitor level, but these rounds don’t need to be consecutive.As with the warpriest sacred weapon ability, he determines the enhancement bonus and special abilities the first time he uses the ability each day, and they cannot be changed until the next day. \nThis ability replaces judgment.\n Dev note: This ability works with all light maces.",
               Helpers.getGuid("SacredWord_FeatureLevel4"),
               null,
               FeatureGroup.None,
               Helpers.CreateAddFacts(new BlueprintUnitFact[]
               {
                    SacredWordToggleAbility,
                    library.Get<BlueprintActivatableAbility>("7902941ef70a0dc44bcfc174d6193386"), //WeaponBondFlamingChoice
                    library.Get<BlueprintActivatableAbility>("27d76f1afda08a64d897cc81201b5218"), //WeaponBondKeenChoice
                    SacredWordFrost,
                    SacredWordShock,
                    SacredWordGhostTouch,

               }),
               Helpers.Create<AddAbilityResources>(x =>
               {
                   x.UseThisAsResource = false;
                   x.Resource = library.Get<BlueprintAbilityResource>("3683d1af071c1744185ff93cba9db10b"); // WeaponBondResourse
                   x.Amount = 0;
                   x.RestoreAmount = true;
                   x.RestoreOnLevelUp = false;
               }),
               abilityResource.CreateAddAbilityResource(),
               Helpers.Create<IncreaseResourcesByClass>(x =>
               {
                   x.CharacterClass = inquisitor;
                   x.Resource = abilityResource;
                   x.BaseValue = 1;
               })

            );

            BlueprintFeature SacredWordPlus2 = Helpers.CreateFeature(
                "SacredWord_Plus2",
                "Sacred Word",
                "Increases the enchantment bonus from Sacred Word by 1. Adds the enchantments Anarchic, Axiomatic, Disruption, Holy, Unholy",
                Helpers.getGuid("SacredWord_Plus2"),
                null,
                FeatureGroup.None,
                Helpers.Create<IncreaseActivatableAbilityGroupSize>(x => x.Group = ActivatableAbilityGroup.DivineWeaponProperty),
                Helpers.CreateAddFacts(new BlueprintUnitFact[]
                {
                    library.Get<BlueprintActivatableAbility>("d76e8a80ab14ac942b6a9b8aaa5860b1"), //WeaponBondAxiomaticChoice
                    library.Get<BlueprintActivatableAbility>("ce0ece459ebed9941bb096f559f36fa8"), //WeaponBondHolyChoice
                    library.Get<BlueprintActivatableAbility>("8c714fbd564461e4588330aeed2fbe1d"), //WeaponBondDisruptionChoice
                    SacredWordAnarchic,
                    SacredWordUnholy,
                    SacredWordBane
                }));

            BlueprintFeature SacredWordPlus3 = Helpers.CreateFeature(
                "SacredWord_Plus3",
                "Sacred Word",
                "Increases the enchantment bonus from Sacred Word by 1.",
                Helpers.getGuid("SacredWord_Plus3"),
                null,
                FeatureGroup.None,
                Helpers.Create<IncreaseActivatableAbilityGroupSize>(x => x.Group = ActivatableAbilityGroup.DivineWeaponProperty)
                );

            BlueprintFeature SacredWordPlus4 = Helpers.CreateFeature(
                "SacredWord_Plus4",
                "Sacred Word",
                "Increases the enchantment bonus from Sacred Word by 1. Adds the enchantment Brilliant Energy",
                Helpers.getGuid("SacredWord_Plus4"),
                null,
                FeatureGroup.None,
                Helpers.Create<IncreaseActivatableAbilityGroupSize>(x => x.Group = ActivatableAbilityGroup.DivineWeaponProperty),
                Helpers.CreateAddFacts(new BlueprintUnitFact[]
                {
                    library.Get<BlueprintActivatableAbility>("f1eec5cc68099384cbfc6964049b24fa"), //WeaponBondBrilliantEnergyChoice
                }));

            BlueprintFeature SacredWordPlus5 = Helpers.CreateFeature(
                "SacredWord_Plus5",
                "Sacred Word",
                "Increases the enchantment bonus from Sacred Word by 1.",
                Helpers.getGuid("SacredWord_Plus5"),
                null,
                FeatureGroup.None,
                Helpers.Create<IncreaseActivatableAbilityGroupSize>(x => x.Group = ActivatableAbilityGroup.DivineWeaponProperty)
              );

            BlueprintFeatureSelection BlessedScriptSelection = Helpers.CreateFeatureSelection(
                "BlessedScript",
                "Blessed Script",
                "At 5th level, a living grimoire can permanently tattoo one spell of 2nd level or lower from his holy book onto his body. The tattooed spell cannot have an expensive material component or focus. The living grimoire can cast his tattooed spells as a spell-like ability once per day.",
                Helpers.getGuid("BlessedScript"),
                null,
                FeatureGroup.None
                );

            List<BlueprintFeatureSelection> BlessedScriptSpellLevelSelection = new List<BlueprintFeatureSelection>();
            for(int i = 1; i < 7; i++)
            {
                BlueprintFeatureSelection BlessedScriptSpellSelection = Helpers.CreateFeatureSelection(
                    "BlessedScriptLevel" + (i),
                    "Blessed Script Level " + (i),
                    "At 5th level, a living grimoire can permanently tattoo one spell of 2nd level or lower from his holy book onto his body. The tattooed spell cannot have an expensive material component or focus. The living grimoire can cast his tattooed spells as a spell-like ability once per day.",
                    Helpers.getGuid("BlessedScriptLevel" + (i)),
                    null,
                    FeatureGroup.None,
                        inquisitor.PrerequisiteClassLevel((i - 1) * 4)
                );
                List<BlueprintFeature> SpellSelection = new List<BlueprintFeature>();
                foreach(BlueprintAbility Spell in InquisitorSpellbook.SpellList.GetSpells(i))
                {
                    if(Spell.MaterialComponent.Item != null)
                    {
                        continue;
                    }

                    BlueprintAbilityResource BlessedScriptAbilityResource = Helpers.CreateAbilityResource(
                        "BlessedScriptAbilityResource" + Spell.name,
                        "",
                        "",
                        Helpers.getGuid("BlessedScriptAbilityResource" + Spell.name),
                        null);
                    BlessedScriptAbilityResource.SetFixedResource(1);


                    BlueprintAbility ability = library.CopyAndAdd(Spell, "BlessedScriptAbility" + Spell.name, Helpers.getGuid("BlessedScriptAbility" + Spell.name));
                    SpellComponent spellComponent = ability.GetComponent<SpellComponent>();
                    if(spellComponent != null)
                    {
                        ability.RemoveComponent(spellComponent);
                    }
                    ability.AddComponent(Helpers.Create<AbilityResourceLogic>(x =>
                    {
                        x.Amount = 1;
                        x.IsSpendResource = true;
                        x.CostIsCustom = false;
                        x.RequiredResource = BlessedScriptAbilityResource;
                    }));

                    BlueprintFeature BlessedScriptSpell = Helpers.CreateFeature(
                        "BlessedScript" + Spell.name,
                        "Blessed Script (" + Spell.Name + ")",
                        "You have tattooed the spell \"" + Spell.Name + "\" onto your body. You can  can cast his tattooed spells as a spell-like ability once per day.",
                        Helpers.getGuid("BlessedScript" + Spell.name),
                        null,
                        FeatureGroup.None,
                        Helpers.Create<PrerequisiteKnowsSpell>(x =>
                        {
                            x.Ability = Spell;
                            x.CharacterClass = inquisitor;
                        }),
                        Helpers.CreateAddFacts(ability),
                        Helpers.CreateAddAbilityResource(BlessedScriptAbilityResource)
                    );
                    BlessedScriptSpell.AddComponent(BlessedScriptSpell.PrerequisiteNoFeature());
                    SpellSelection.Add(BlessedScriptSpell);
                }
                BlessedScriptSpellSelection.SetFeatures(SpellSelection);
                BlessedScriptSpellLevelSelection.Add(BlessedScriptSpellSelection);
            }
            BlessedScriptSelection.SetFeatures(BlessedScriptSpellLevelSelection);


            BlueprintBuff TrueJudgmentCooldownBuff = library.Get<BlueprintBuff>("65058aafc91a12042b158527f9d0506a");

            ContextValue CooldownBuff = new ContextValue();
            CooldownBuff.Value = 24;

            BlueprintAbilityResource WordOfGodAbilityResource = Helpers.CreateAbilityResource(
                "WordOfGodAbilityResource",
                "",
                "",
                Helpers.getGuid("WordOfGodAbilityResource"),
                null);
            WordOfGodAbilityResource.SetFixedResource(7);

            AbilityEffectRunAction abilityEffectRunAction = Helpers.Create<AbilityEffectRunAction>();
            ContextActionSavingThrow savingThrow = Helpers.Create<ContextActionSavingThrow>();
            savingThrow.Type = SavingThrowType.Fortitude;

            ContextActionConditionalSaved saved = Helpers.Create<ContextActionConditionalSaved>();
            saved.Succeed = new ActionList
            {
                Actions = new GameAction[]
                {
                    Helpers.Create<ContextActionApplyBuff>(ap =>
                    {
                        ap.Buff = TrueJudgmentCooldownBuff;
                        ap.DurationValue = Helpers.CreateContextDuration(CooldownBuff, DurationRate.Hours);
                    })
                }
            };
            saved.Failed = new ActionList {Actions = new GameAction[] {Helpers.Create<ContextActionKillTarget>()}};
            savingThrow.Actions = new ActionList {Actions = new GameAction[] {saved}};


            Conditional conditional = Helpers.Create<Conditional>();
            conditional.ConditionsChecker = new ConditionsChecker();

            ContextConditionHasBuffFromCaster condition = Helpers.Create<ContextConditionHasBuffFromCaster>();
            condition.Buff = TrueJudgmentCooldownBuff;

            conditional.ConditionsChecker.Conditions = new Condition[] { condition };
            conditional.IfFalse = new ActionList {Actions = new GameAction[] {savingThrow}};

            abilityEffectRunAction.Actions = new ActionList {Actions = new GameAction[] {conditional}};

            BlueprintAbility TrueJudgmentAbility = library.Get<BlueprintAbility>("d69715dc0de8f8b44ac9f20188c7c22e");

            BlueprintAbility WordOfGodAbility = Helpers.CreateAbility(
                "WordOfGodAbility",
                "Word of God",
                "At 20th level, a living grimoire can smite his foes with the holy word of his deity. Up to seven times per day, the inquisitor can make a single melee attack with his holy book against a target. If the attack hits, it deals damage normally and the target must succeed at a Fortitude save or die (DC = 10 + 1/2 the living grimoire’s inquisitor level + his Intelligence modifier). Regardless of whether the save is successful, the target creature is immune to the living grimoire’s word of god ability for 24 hours. Once the living grimoire uses this ability, he can’t use it again for 1d4 rounds.\nThis ability replaces true judgment.",
                Helpers.getGuid("WordOfGodAbility"),
                TrueJudgmentAbility.Icon,
                AbilityType.Supernatural,
                UnitCommand.CommandType.Standard,
                AbilityRange.Weapon,
                "Instant",
                "Fortitude",
                abilityEffectRunAction
                ,
                Helpers.Create<AbilityResourceLogic>(x =>
                {
                    x.Amount = 1;
                    x.IsSpendResource = true;
                    x.CostIsCustom = false;
                    x.RequiredResource = WordOfGodAbilityResource;
                })
            );
            WordOfGodAbility.CanTargetEnemies = true;
            WordOfGodAbility.CanTargetSelf = false;

            BlueprintFeature TrueJudgmentFeature = library.Get<BlueprintFeature>("f069b6557a2013544ac3636219186632"); // TrueJudgmentFeature

            BlueprintFeature WordOfGod = Helpers.CreateFeature("WordOfGod",
                "Word of God",
                "At 20th level, a living grimoire can smite his foes with the holy word of his deity. Up to seven times per day, the inquisitor can make a single melee attack with his holy book against a target. If the attack hits, it deals damage normally and the target must succeed at a Fortitude save or die (DC = 10 + 1/2 the living grimoire’s inquisitor level + his Intelligence modifier). Regardless of whether the save is successful, the target creature is immune to the living grimoire’s word of god ability for 24 hours. Once the living grimoire uses this ability, he can’t use it again for 1d4 rounds.\nThis ability replaces true judgment.",
                Helpers.getGuid("WordOfGod"),
                TrueJudgmentFeature.Icon,
                FeatureGroup.None,
                Helpers.CreateAddFacts(WordOfGodAbility),
                Helpers.Create<ReplaceAbilitiesStat>(x =>
                {
                    x.Stat = StatType.Intelligence;
                    x.Ability = new BlueprintAbility[] { WordOfGodAbility };
                }),
                Helpers.Create<ReplaceCasterLevelOfAbility>(x =>
                {
                    x.Spell = WordOfGodAbility;
                    x.Class = inquisitor;
                }),
                WordOfGodAbilityResource.CreateAddAbilityResource()
                );

            List<LevelEntry> addFeatures = new List<LevelEntry>();
            addFeatures.Add(Helpers.LevelEntry(1, HolyBookFeature, SacredWordFeature));
            addFeatures.Add(Helpers.LevelEntry(4, SacredWordFeatureLevel4));
            addFeatures.Add(Helpers.LevelEntry(5, BlessedScriptSelection, SacredWordFeature1d8));
            addFeatures.Add(Helpers.LevelEntry(8, SacredWordPlus2, BlessedScriptSelection));
            addFeatures.Add(Helpers.LevelEntry(10,  SacredWordFeature1d10));
            addFeatures.Add(Helpers.LevelEntry(12, SacredWordPlus3, BlessedScriptSelection));
            addFeatures.Add(Helpers.LevelEntry(15, SacredWordFeature2d6));
            addFeatures.Add(Helpers.LevelEntry(16, SacredWordPlus4, BlessedScriptSelection));
            addFeatures.Add(Helpers.LevelEntry(20, SacredWordPlus5, WordOfGod, SacredWordFeature2d8));


            BlueprintFeature JudgmentAdditionalUse = library.Get<BlueprintFeature>("ee50875819478774b8968701893b52f5");//JudgmentAdditionalUse
            BlueprintFeature SecondJudgment = library.Get<BlueprintFeature>("33bf0404b70d65f42acac989ec5295b2"); // SecondJudgment
            BlueprintFeature ThirdJudgment = library.Get<BlueprintFeature>("490c7e92b22cc8a4bb4885a027b355db"); // ThirdJudgment

            List<LevelEntry> removeFeatures = new List<LevelEntry>();
            removeFeatures.Add(Helpers.LevelEntry(1, library.Get<BlueprintFeature>("981def910b98200499c0c8f85a78bde8")));//JudgmentFeature
            removeFeatures.Add(Helpers.LevelEntry(2, library.Get<BlueprintFeature>("6be8b4031d8b9fc4f879b72b5428f1e0")));//CunningInitiative
            removeFeatures.Add(Helpers.LevelEntry(4, JudgmentAdditionalUse));
            removeFeatures.Add(Helpers.LevelEntry(5, library.Get<BlueprintFeature>("7ddf7fbeecbe78342b83171d888028cf")));//InquisitorBaneNormalFeatureAdd
            removeFeatures.Add(Helpers.LevelEntry(7, JudgmentAdditionalUse));
            removeFeatures.Add(Helpers.LevelEntry(8, SecondJudgment));
            removeFeatures.Add(Helpers.LevelEntry(10, JudgmentAdditionalUse));
            removeFeatures.Add(Helpers.LevelEntry(12, library.Get<BlueprintFeature>("6e694114b2f9e0e40a6da5d13736ff33")));//InquisitorBaneGreaterFeature
            removeFeatures.Add(Helpers.LevelEntry(13, JudgmentAdditionalUse));
            removeFeatures.Add(Helpers.LevelEntry(16, JudgmentAdditionalUse, ThirdJudgment));
            removeFeatures.Add(Helpers.LevelEntry(19, JudgmentAdditionalUse));
            removeFeatures.Add(Helpers.LevelEntry(20, TrueJudgmentFeature));

            livingGrimoire.AddFeatures = addFeatures.ToArray();
            livingGrimoire.RemoveFeatures = removeFeatures.ToArray();

            UIGroup SacredWordGroup = new UIGroup();
            SacredWordGroup.Features.Add(SacredWordFeature);
            SacredWordGroup.Features.Add(SacredWordFeatureLevel4);
            SacredWordGroup.Features.Add(SacredWordPlus2);
            SacredWordGroup.Features.Add(SacredWordPlus3);
            SacredWordGroup.Features.Add(SacredWordPlus4);
            SacredWordGroup.Features.Add(SacredWordPlus5);
            SacredWordGroup.Features.Add(SacredWordFeature1d8);
            SacredWordGroup.Features.Add(SacredWordFeature1d10);
            SacredWordGroup.Features.Add(SacredWordFeature2d6);
            SacredWordGroup.Features.Add(SacredWordFeature2d8);
            inquisitorProgression.UIGroups = inquisitorProgression.UIGroups.AddToArray(SacredWordGroup);
            UIGroup BlessedScriptGroup = new UIGroup();
            BlessedScriptGroup.Features.Add(BlessedScriptSelection);
            inquisitorProgression.UIGroups = inquisitorProgression.UIGroups.AddToArray(BlessedScriptGroup);

            List<BlueprintArchetype> archetypes = inquisitor.Archetypes.ToList();
            archetypes.Insert(0, livingGrimoire);
            inquisitor.Archetypes = archetypes.ToArray();
        }
    }
}
