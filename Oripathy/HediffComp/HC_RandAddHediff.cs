using AK_DLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace AK_Industry
{
    public class HCP_RandAddHediff : HediffCompProperties
    {
        public int interval = 1;
        public TimeToTick intervalUnit = TimeToTick.hour;
        public Type saveChanceByFunc = null; //优先使用概率函数。如果函数为空就用下面静态值
        public float saveChance = 0f; //有多少概率会无事发生。1就是100%。
        public List<HediffStat> hediffStatsSet = new List<HediffStat>();

        public HCP_RandAddHediff()
        {
            this.compClass = typeof(HC_RandAddHediff);
        }
    }

    public class HC_RandAddHediff : HediffComp
    {
        public HCP_RandAddHediff Props => (HCP_RandAddHediff)this.props;

        private int tick = 0;

        private int[] weight;

        private bool arrayCached = false;

        private int arraySum;
        private List<HediffStat> HediffStats
        {
            get { return this.Props.hediffStatsSet; }
        }

        private int Interval
        {
            get { return this.Props.interval * (int)this.Props.intervalUnit; }
        }

        double SaveChance
        {
            get
            {
                if (Props.saveChanceByFunc == null) return Props.saveChance;
                else
                {
                    MathFunction func = (MathFunction)Activator.CreateInstance(Props.saveChanceByFunc);
                    Log.Message(func.Value(parent.Severity).ToString());
                    return func.Value(parent.Severity);
                }
            }
        }
        private void CaculateWeightArray()
        {
            this.weight = new int[this.HediffStats.Count];
            this.weight[0] = this.HediffStats[0].randWeight;
            for (int i = 1; i < this.HediffStats.Count; ++i)
            {
                this.weight[i] = this.weight[i - 1] + this.HediffStats[i].randWeight;
            }
            arraySum = this.weight.Last();
            this.arrayCached = true;
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            ++tick;
            if (tick >= Interval)
            {
                if (!arrayCached)
                {
                    this.CaculateWeightArray();
                }
                tick = 0;
                if (UnityEngine.Random.Range(0f, 1f) <= SaveChance) return;

                HediffStat hediff = HediffStats[AK_Tool.weightArrayRand(weight)];
                Log.Message("Add " + hediff.hediff.defName + " to " + this.Pawn.Name.ToString());
                AbilityEffect_AddHediff.AddHediff(this.Pawn, hediff.hediff, hediff.part, hediff.serverity);
            }
        }
    }
}
