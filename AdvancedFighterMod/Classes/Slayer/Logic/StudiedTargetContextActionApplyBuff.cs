using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;

namespace AdvancedMartialArts.Classes.Slayer.Logic
{
    class StudiedTargetContextActionApplyBuff : ContextActionApplyBuff
    {

        public override void RunAction()
        {
            MechanicsContext context = ElementsContext.GetData<MechanicsContext.Data>()?.Context;
            if(context == null)
            {
                UberDebug.LogError((UnityEngine.Object)this, (object)"Unable to apply buff: no context found", (object[])Array.Empty<object>());
            }
            else
            {
                UnitEntityData caster = context.MaybeCaster;
                UnitEntityData target = this.Target.Unit;

                if (caster != null && target != null)
                {
                    caster.Descriptor.Ensure<UnitPartStudiedTarget>().SetTarget(target.Descriptor);
                }
            }
        }

    }
}
