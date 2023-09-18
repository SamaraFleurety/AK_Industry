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
            base.DoEffect(usedBy); //这还真不是空的
            if (usedBy.needs.mood != null) usedBy.needs.mood.thoughts.memories.TryGainMemory(Props.thoughtDef);
        }
    }
}
