using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;

namespace AdvancedMartialArts.Feats.GeneralFeats
{
    public class GeneralFeats
    {
        private static LibraryScriptableObject library => Main.library;
        public static void Load()
        {
            Main.SafeLoad(CreateGeneralFeats, "GeneralFeats");
        }

        private static void CreateGeneralFeats()
        {
            AnimalAllyFeatLine.CreateAnimalAllyFeatLine();
        }
    }
}
