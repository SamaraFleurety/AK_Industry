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
    public class JobDriver_CleanOrgDust : JobDriver
    {
        TargetIndex cleanTool = TargetIndex.A;

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
            this.FailOnDespawnedOrNull(cleanTool);
            this.FailOnForbidden(cleanTool);

            yield return Toils_Goto.GotoCell(cleanTool, PathEndMode.OnCell);

            //yield return toil

        }
    }
}
