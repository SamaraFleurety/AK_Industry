using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace AK_Industry
{
    public class JobDriver_UseLotteryMachine : JobDriver
    {
        TargetIndex indexLotteryMachine = TargetIndex.A; 
        private TC_Lottery compPurifyPod => job.targetA.Thing.TryGetComp<TC_Lottery>();

        public override string GetReport()
        {
            return "正在抽奖";
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
            this.FailOnDespawnedOrNull(indexLotteryMachine);
            this.FailOnForbidden(indexLotteryMachine);

            //走到清洁舱处
            yield return Toils_Goto.GotoThing(indexLotteryMachine, PathEndMode.InteractionCell);

            Toil prepareToEnterToil = Toils_General.Wait(100);
            prepareToEnterToil.FailOnCannotTouch(indexLotteryMachine, PathEndMode.InteractionCell);
            prepareToEnterToil.WithProgressBarToilDelay(indexLotteryMachine);
            //prepareToEnterToil.play

            yield return prepareToEnterToil;

            Toil enter = ToilMaker.MakeToil();
            enter.initAction = delegate ()
            {
                compPurifyPod.LotteryGacha(pawn);
            };

            enter.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return enter;
        }
    }
}