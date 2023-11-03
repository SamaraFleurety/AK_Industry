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
    public class JobDriver_FillKohlBarrel : JobDriver
    {
        private TargetIndex indexBarrel = TargetIndex.A;
        private TargetIndex indexBeer = TargetIndex.B;
        private TargetIndex indexOrg = TargetIndex.C;

        private int Duration = 200;

        private Building_KohlBarrel thingBarrel => (Building_KohlBarrel)job.GetTarget(indexBarrel).Thing;
        private Thing thingBeer => job.GetTarget(indexBeer).Thing;
        private Thing thingOrg => job.GetTarget(indexOrg).Thing;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (pawn.Reserve(thingBarrel, job, 1, -1, null, errorOnFailed))
            {
                if (thingBarrel.FilledBeer() && !pawn.Reserve(thingBeer, job, 1, -1, null, errorOnFailed)) return false;
                return pawn.Reserve(thingOrg, job, 1, -1, null, errorOnFailed);
            }
            return false;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnBurningImmobile(TargetIndex.A);
            AddEndCondition(() => (!thingBarrel.Filled()) ? JobCondition.Ongoing : JobCondition.Succeeded);
            yield return Toils_General.DoAtomic(delegate
            {
                job.count = thingBarrel.SpaceToFill;
            });

            if (!thingBarrel.FilledBeer())
            {
                foreach (Toil toil in FillBarrelWith(indexBeer, thingBeer, delegate ()
                 {
                     thingBarrel.AddBeer(thingBeer);
                 }))
                {
                    yield return toil;
                }
            }

            foreach(Toil toil in FillBarrelWith(indexOrg, thingOrg, delegate()
            {
                thingBarrel.AddOrg(thingOrg);
            }))
            {
                yield return toil;
            }
        }

        private IEnumerable<Toil> FillBarrelWith(TargetIndex index, Thing t, Action fillEffect)
        {
            Toil reserveThing = Toils_Reserve.Reserve(index);
            yield return reserveThing;
            yield return Toils_Goto.GotoThing(index, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(index).FailOnSomeonePhysicallyInteracting(index);
            yield return Toils_Haul.StartCarryThing(index, putRemainderInQueue: false, subtractNumTakenFromJobCount: true).FailOnDestroyedNullOrForbidden(index);
            yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveThing, index, TargetIndex.None, takeFromValidStorage: true);
            yield return Toils_Goto.GotoThing(indexBarrel, PathEndMode.Touch);
            yield return Toils_General.Wait(200).FailOnDestroyedNullOrForbidden(index).FailOnDestroyedNullOrForbidden(indexBarrel)
                .FailOnCannotTouch(indexBarrel, PathEndMode.Touch)
                .WithProgressBarToilDelay(indexBarrel);
            Toil addThing = ToilMaker.MakeToil();
            addThing.initAction = fillEffect;
            addThing.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return addThing;
        }
    }
}
