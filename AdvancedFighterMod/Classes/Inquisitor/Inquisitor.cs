using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedMartialArts.AltRacialTraits.Facts;
using AdvancedMartialArts.Classes.Inquisitor.Archetypes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;

namespace AdvancedMartialArts.Classes.Inquisitor
{
    class Inquisitor
    {
        private static LibraryScriptableObject library => Main.library;

        public static void Load()
        {
            LivingGrimoire.Load();
        }

    }
}
