using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace AK_Industry
{
    public class ThingClass_RecruitBagLight : Building
    {
        public int lightStatus = 0;   //0是关闭，然后值越高稀有度越高
        private TC_RBLGlower CompGlower => this.GetComp<TC_RBLGlower>();

        private GraphicData actualGraphicData = null;

        private string TexPathBase => "Things/Building/RecruitBagLamp";

        private GraphicData ActualGraphicData
        {
            get
            {
                if (actualGraphicData == null)
                {
                    actualGraphicData = new GraphicData
                    {
                        graphicClass = this.def.graphicData.graphicClass,
                        drawSize = this.def.graphicData.drawSize,
                        shaderType = this.def.graphicData.shaderType
                    };
                }
                actualGraphicData.texPath = TexPathBase + AK_DLL.TypeDef.romanNumber[lightStatus];
                MethodInfo method = typeof(GraphicData).GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance);
                method.Invoke(actualGraphicData, new object[0]);
                return actualGraphicData;
            }
        }

        public override Graphic Graphic => ActualGraphicData.GraphicColoredFor(this);

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
            if (Map != null) this.DirtyMapMesh(Map);
        }

        public void GachaSingle()
        {
            int rd = UnityEngine.Random.Range(1, 101);
            if (rd >= 99) lightStatus = 4;
            else if (rd >= 90) lightStatus = 3;
            else if (rd >= 40) lightStatus = 2;
            else lightStatus = 1;
            CompGlower.ChangeLightMode(lightStatus);
            if (Map != null) this.DirtyMapMesh(Map);
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
            if (Map != null) this.DirtyMapMesh(Map);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref lightStatus, "ls", 0);
        }
    }
}
