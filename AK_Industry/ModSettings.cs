using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using UnityEngine;

namespace AK_Industry
{
    public class AKI_ModSettings : ModSettings
    {
        public static bool autoAvoidDangerGas = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref autoAvoidDangerGas, "avoidGas", true);
        }
    }

    public class AKI_Mod : Mod
    {
        public AKI_Mod(ModContentPack content) : base(content)
        {
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.CheckboxLabeled("avoidgas".Translate(), ref AKI_ModSettings.autoAvoidDangerGas, "avoidgasdesc".Translate());
        }
    }
}