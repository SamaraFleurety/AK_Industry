using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AK_Industry
{
    public class TCP_UseEffect_CleanOrgDust : CompProperties_UseEffect
    {
        public TCP_UseEffect_CleanOrgDust()
        {
            compClass = typeof(TC_UseEffect_CleanOrgDust);
        }
    }

    public class TC_UseEffect_CleanOrgDust : CompUseEffect
    {
        public override void DoEffect(Pawn usedBy)
        {
            base.DoEffect(usedBy);
            CleanOrgDust(usedBy);
        }

        public static void CleanOrgDust(Pawn p)
        {
            Hediff h = p.health.hediffSet.GetFirstHediffOfDef(AKIDefOf.AKI_Hediff_OrgDust);
            if (h != null) h.Severity -= 1000;
            h = p.health.hediffSet.GetFirstHediffOfDef(AKIDefOf.AKI_Hediff_OrgDustActive);
            if (h != null) h.Severity -= 1000;
        }
    }
}
