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
    public class WorkGiver_FillKohlBarrel : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(AKIDefOf.AKI_Building_KohlBarrel);
        public override PathEndMode PathEndMode => PathEndMode.Touch;

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_KohlBarrel barrel = t as Building_KohlBarrel;
            //非待填充的桶
            if (barrel == null || !barrel.PowerOn || barrel.progress > 0) return false;

            if (t.IsForbidden(pawn) || !pawn.CanReserve(t, 1, -1, null, forced))
            {
                return false;
            }
            if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
            {
                return false;
            }
            if (t.IsBurning()) return false;

            if (!barrel.FilledBeer() && FindBeer(pawn) == null)
            {
                JobFailReason.Is("NoBeer".Translate());
                return false;
            }
            if (FindOrg(pawn) == null)
            {
                JobFailReason.Is("NoOrgT1".Translate());
                return false;
            }

            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_KohlBarrel barrel = t as Building_KohlBarrel;
            Thing beer = FindBeer(pawn);
            Thing org = FindOrg(pawn);
            return JobMaker.MakeJob(AKIDefOf.AKI_Job_FillKohlBarrel, t, beer, org);
        }

        Predicate<Thing> Validator(Pawn p) => (Thing x) => (!x.IsForbidden(p) && p.CanReserve(x));

        public Thing FindBeer(Pawn p)
        {
            return GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(ThingDefOf.Beer), PathEndMode.ClosestTouch, TraverseParms.For(p), 100, Validator(p));
        }

        public Thing FindOrg(Pawn p)
        {
            return GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(AKIDefOf.AKI_Item_OriginiumRefineI), PathEndMode.ClosestTouch, TraverseParms.For(p), 100, Validator(p));
        }
    }
    
}
