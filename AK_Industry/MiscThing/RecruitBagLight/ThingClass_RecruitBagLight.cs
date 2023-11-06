using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AK_Industry
{
    public class ThingClass_RecruitBagLight : ThingWithComps
    {
        public int lightStatus = 0;   //0是关闭，然后值越高稀有度越高
        private TC_RBLGlower CompGlower => this.GetComp<TC_RBLGlower>();

        public override Graphic Graphic => base.Graphic;

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
        {
            yield return new FloatMenuOption("RBLSwitch".Translate(), delegate ()
            {
                selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(AKIDefOf.AKI_Job_UseRecruitBagLamp, this, this, this));
            });
            yield return new FloatMenuOption("gacha".Translate(), delegate()
            {
                selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(AKIDefOf.AKI_Job_UseRecruitBagLamp, this, this, null));
            });
            yield return new FloatMenuOption("gachaBuck".Translate(), delegate()
            {
                selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(AKIDefOf.AKI_Job_UseRecruitBagLamp, this, null, this));
            });
        }

        public void CloseBag()
        {
            lightStatus = 0;
            Map.glowGrid.DeRegisterGlower(CompGlower);
        }

        public void GachaSingle()
        {
            int rd = UnityEngine.Random.Range(1, 101);
            if (rd >= 99) lightStatus = 4;
            else if (rd >= 90) lightStatus = 3;
            else if (rd >= 40) lightStatus = 2;
            else lightStatus = 1;
            CompGlower.ChangeLightMode(lightStatus);
            //Log.Message($"lights:{lightStatus}");
        }

        public void GachaBuck()
        {
            //34.9%是紫，46.8是金，18.3是彩
            int rd = UnityEngine.Random.Range(1, 101);
            if (rd >= 82) lightStatus = 4;
            else if (rd >= 35) lightStatus = 3;
            else if (rd >= 17) lightStatus = 2;
            else lightStatus = 1;
            CompGlower.ChangeLightMode(lightStatus);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref lightStatus, "ls", 0);
        }
    }
}
