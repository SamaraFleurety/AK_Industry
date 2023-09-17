using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AK_Industry
{
    public class TCP_UseEffect_AddMemory : CompProperties_UseEffect
    {
        public ThoughtDef thoughtDef;

        public TCP_UseEffect_AddMemory()
        {
            compClass = typeof(TC_UseEffect_AddMemory);
        }
    }

    public class TC_UseEffect_AddMemory : CompUseEffect
    {
        TCP_UseEffect_AddMemory Props => props as TCP_UseEffect_AddMemory;

        public override void DoEffect(Pawn usedBy)
        {
            Log.Message("de a");
            base.DoEffect(usedBy); //这还真不是空的
            Log.Message("de b");
            if (usedBy.needs.mood != null) usedBy.needs.mood.thoughts.memories.TryGainMemory(Props.thoughtDef);
        }
    }
}
