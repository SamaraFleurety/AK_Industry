using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace AK_Industry
{
    //使用了奇妙地偷懒方法：indexA是包（thing），如果indexB不为空那就是单抽，如果indexC不为空那就是十连，如果都不为空那就是关闭
    public class JobDriver_UseRecruitBagLamp : JobDriver
    {
        TargetIndex indexBag = TargetIndex.A;
        private ThingClass_RecruitBagLight thingBag => job.targetA.Thing as ThingClass_RecruitBagLight;

        public override string GetReport()
        {
            return "正在使用招募包";
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
            this.FailOnDespawnedOrNull(indexBag);
            this.FailOnForbidden(indexBag);

            //走到清洁舱处
            yield return Toils_Goto.GotoThing(indexBag, PathEndMode.InteractionCell);

            Toil closeBag = ToilMaker.MakeToil();
            closeBag.initAction = thingBag.CloseBag;

            if (job.targetB != null && job.targetC != null) yield break;

            Toil toilWait = Toils_General.Wait(100);
            toilWait.FailOnCannotTouch(indexBag, PathEndMode.InteractionCell);
            toilWait.WithProgressBarToilDelay(indexBag);
            //prepareToEnterToil.play

            yield return toilWait;

            Toil enter = ToilMaker.MakeToil();
            Action action = thingBag.GachaSingle;
            if (job.targetB == null && job.targetC != null) action = thingBag.GachaBuck;
            enter.initAction = action;

            enter.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return enter;
        }
    }
}
