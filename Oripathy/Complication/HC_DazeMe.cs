using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AK_Industry
{
    //让持有者精神崩溃：茫然
    public class HCP_DazeMe : HediffCompProperties
    {
        public HCP_DazeMe()
        {
            this.compClass = typeof(HCP_DazeMe);
        }
    }

    public class HC_DazeMe : HediffComp
    {
        public override void CompPostTick(ref float severityAdjustment)
        {
            // Check if the pawn is still alive and has a valid mind.
            if (Pawn != null && !Pawn.Dead && !Pawn.Downed && Pawn.mindState != null && Pawn.MentalState != null && !Pawn.InMentalState)
            {
                // If the pawn is not in a mental state, apply a new dazed mental break.
                MentalStateDef dazedDef = DefDatabase<MentalStateDef>.GetNamed("Wander_Psychotic");
                if (dazedDef != null)
                {
                    Pawn.mindState.mentalStateHandler.TryStartMentalState(dazedDef, null, false, false, null, false);
                }
            }
            Pawn.health.hediffSet.hediffs.Remove(parent);
        }
    }
}
