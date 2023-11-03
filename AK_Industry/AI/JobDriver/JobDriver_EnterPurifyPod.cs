using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace AK_Industry
{
    public class JobDriver_EnterPurifyPod : JobDriver
    {
        TargetIndex indexPurifyPod = TargetIndex.A;

        private TC_PurifyPod compPurifyPod => job.targetA.Thing.TryGetComp<TC_PurifyPod>();

        public override string GetReport()
        {
            return "正在清洁源石尘";
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            Pawn pawn = base.pawn;
            LocalTargetInfo targetA = base.job.targetA;
            Job job = base.job;
            return ReservationUtility.Reserve(pawn, targetA, job, 1, -1, (ReservationLayerDef)null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedOrNull(indexPurifyPod);
            this.FailOnForbidden(indexPurifyPod);
            this.FailOn(delegate ()
            {
                return compPurifyPod.Occupied || !compPurifyPod.PowerOn;
            });

            //走到清洁舱处
            yield return Toils_Goto.GotoThing(indexPurifyPod, PathEndMode.InteractionCell);

            Toil prepareToEnterToil = Toils_General.Wait(70);
            prepareToEnterToil.FailOnCannotTouch(indexPurifyPod, PathEndMode.InteractionCell);
            prepareToEnterToil.WithProgressBarToilDelay(indexPurifyPod);
            //prepareToEnterToil.play

            yield return prepareToEnterToil;

            Toil enter = ToilMaker.MakeToil();
            enter.initAction = delegate ()
            {
                compPurifyPod.TryAcceptPawn(pawn);
            };

            enter.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return enter;
        }
    }
}
