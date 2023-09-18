using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace AK_Industry
{
    //没有使用
    public class JobDriver_EnterPurifyPod : JobDriver
    {
        TargetIndex thingIndexPurifyPod = TargetIndex.A;

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
            this.FailOnDespawnedOrNull(thingIndexPurifyPod);
            this.FailOnForbidden(thingIndexPurifyPod);
            this.FailOn(delegate ()
            {
                return compPurifyPod.Occupied || !compPurifyPod.PowerOn;
            });

            //走到清洁舱处
            yield return Toils_Goto.GotoThing(thingIndexPurifyPod, PathEndMode.InteractionCell);

            Toil prepareToEnterToil = Toils_General.Wait(70);
            prepareToEnterToil.FailOnCannotTouch(thingIndexPurifyPod, PathEndMode.InteractionCell);
            prepareToEnterToil.WithProgressBarToilDelay(thingIndexPurifyPod);
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
