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
    public class JobDriver_HarvestKohlBarrel : JobDriver
    {
        private TargetIndex indexBarrel = TargetIndex.A;
        private Building_KohlBarrel thingBarrel => (Building_KohlBarrel)job.GetTarget(indexBarrel).Thing;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(thingBarrel, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(indexBarrel);
            this.FailOnBurningImmobile(indexBarrel);
            yield return Toils_Goto.GotoThing(indexBarrel, PathEndMode.Touch);
            yield return Toils_General.Wait(200).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(indexBarrel, PathEndMode.Touch)
                .FailOn(() => thingBarrel.progress < 1)
                .WithProgressBarToilDelay(indexBarrel);
            Toil toil = ToilMaker.MakeToil();

            toil.initAction = delegate ()
            {
                Thing thing = ThingMaker.MakeThing(ThingDefOf.Beer);
                thing.stackCount = 20;
                GenPlace.TryPlaceThing(thing, pawn.Position, base.Map, ThingPlaceMode.Near);

                thing = ThingMaker.MakeThing(AKIDefOf.AKI_Item_Isobutanol);
                thing.stackCount = 1;
                GenPlace.TryPlaceThing(thing, pawn.Position, base.Map, ThingPlaceMode.Near);

                thingBarrel.Reset();
            };
            yield return toil;
        }
    }
}
