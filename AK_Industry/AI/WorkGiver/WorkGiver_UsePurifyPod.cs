using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace AK_Industry
{
    public class WorkGiver_UsePurifyPod : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(AKIDefOf.AKI_Building_PurifyPod);
        public override Danger MaxPathDanger(Pawn pawn) => Danger.Deadly;
        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            Hediff orgDust = pawn.health.hediffSet.GetFirstHediffOfDef(AKIDefOf.AKI_Hediff_OrgDust);
            if (orgDust == null || orgDust.Severity < 20) return true;

            return false;
        }

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            foreach (Thing i in pawn.Map.listerThings.ThingsOfDef(AKIDefOf.AKI_Building_PurifyPod))
            {
                yield return i;
            }

        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (t.def != AKIDefOf.AKI_Building_PurifyPod) return false;

            if (ForbidUtility.IsForbidden(t, pawn) || FireUtility.IsBurning(t)) return false;

            TC_PurifyPod comp = t.TryGetComp<TC_PurifyPod>();
            if (comp == null || !comp.PowerOn || comp.Occupied) return false;

            if (!ReservationUtility.CanReserveAndReach(pawn, t, PathEndMode.InteractionCell, Danger.Deadly)) return false;

            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return new Job(AKIDefOf.AKI_Job_EnterPurifyPod, t);
        }
    }
}
