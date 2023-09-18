using Verse;
using RimWorld;

namespace AK_Industry
{
    public static class TypeDef
    {
    }

    [DefOf]
    public static class AKIDefOf
    {
        public static HediffDef AKI_Hediff_OripathyCrystal;
        public static HediffDef AKI_Hediff_OripathyCrystalLethal;
        public static HediffDef AKI_Hediff_OripathyCrystalTrans;

        public static HediffDef AKI_Hediff_OrgDust;
        public static HediffDef AKI_Hediff_OrgDustActive;

        public static ThingDef AKI_Item_CleaningKit;
        public static ThingDef AKI_Item_CleaningKitFloral;

        public static ThingDef AKI_Building_PurifyPod;

        public static JobDef AKI_Job_CleanOrgDust;
        public static JobDef AKI_Job_EnterPurifyPod;

        //public static ThoughtDef AKI_Thought_UsedPurifyPod;
        //public static ThoughtDef AKI_Thought_UsePurifyPodInterrupted;
    }
}
