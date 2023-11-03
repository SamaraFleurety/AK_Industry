using Verse;
using RimWorld;
using UnityEngine;

namespace MGasEmitter
{
    public class MGasEmitter_ModSettings : ModSettings
    {
        public static bool autoAvoidDangerGas = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref autoAvoidDangerGas, "avoidGas", true);
        }
    }

    public class MGas_Mod : Mod
    {
        public MGas_Mod(ModContentPack content) : base(content)
        {
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.CheckboxLabeled("avoidgas".Translate(), ref MGasEmitter_ModSettings.autoAvoidDangerGas, "avoidgasdesc".Translate());
        }
    }
}
