using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AK_Industry
{
    class TypeDef
    {
    }

    [DefOf]
    public static class AKIDefOf
    {
        public static HediffDef AKI_Hediff_OripathyCrystal;
        public static HediffDef AKI_Hediff_OripathyCrystalLethal;
        public static HediffDef AKI_Hediff_OripathyCrystalTrans;

        public static HediffDef AKI_Hediff_OrgDust;

        public static ThingDef AKI_Item_CleaningKit;
        public static ThingDef AKI_Item_CleaningKitFloral;

        public static JobDef AKI_Job_CleanOrgDust;
    }
}
