using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;
using RimWorld;

namespace AK_Industry
{
    public class WorkGiver_HarvestKohlBarrel : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(AKIDefOf.AKI_Building_KohlBarrel);
        public override PathEndMode PathEndMode => PathEndMode.Touch;

        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            List<Thing> list = pawn.Map.listerThings.ThingsOfDef(AKIDefOf.AKI_Building_KohlBarrel);
            foreach (Thing i in list)
            {
                if ((i as Building_KohlBarrel).progress >= 1) return false; 
            }
            return true;
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (t.IsBurning() || t.IsForbidden(pawn) || !pawn.CanReserve(t, 1, -1, null, forced)) return false;
            if (!(t is Building_KohlBarrel barrel) || barrel.progress < 1) return false;
            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return JobMaker.MakeJob(AKIDefOf.AKI_Job_HarvestKohlBarrel, t);
        }
    }
}
