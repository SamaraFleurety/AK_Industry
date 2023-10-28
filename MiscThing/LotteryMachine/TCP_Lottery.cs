using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using AK_DLL;
using Verse.AI;

namespace AK_Industry
{
    public class LotteryCandidate : ItemOnSpawn
    {
        public int weight = 1;
    }

    public class TCP_Lottery : CompProperties
    {
        public TCP_Lottery()
        {
            compClass = typeof(TC_Lottery);
        }

        public List<LotteryCandidate> candidate = new List<LotteryCandidate>();

        public int cost = 1;
    }

    public class TC_Lottery : ThingComp
    {
        private TCP_Lottery Props => props as TCP_Lottery;

        private List<LotteryCandidate> Candidate => Props.candidate;
        private Map ActualMap => parent.Map;
        private int Cost => Props.cost;

        private int[] weight;

        private bool arrayCached = false;

        private List<Thing> pendingSilver = new List<Thing>();
        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            CompPowerTrader compPower = parent.GetComp<CompPowerTrader>();
            CompRefuelable compRefuelable = parent.GetComp<CompRefuelable>();
            if (compPower != null && !compPower.PowerOn)
            {
                yield return new FloatMenuOption("CannotUseNoPower".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0); yield break;
            }
            if (selPawn.health.Dead || selPawn == null && selPawn.CanReach(parent, PathEndMode.Touch, Danger.Deadly))
            {
                yield return new FloatMenuOption("AK_PawnNull".Translate(), null); yield break;
            }
            if (HasEnoughSilver())
            {
                yield return new FloatMenuOption("gacha".Translate(), delegate ()
                {
                    selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(AKIDefOf.AKI_Job_UseLotteryMachine, this.parent));
                });
            }
            else
            {
                yield return new FloatMenuOption("AK_NoSilver".Translate(), null); yield break;
            }
        }

        public void LotteryGacha(Pawn p)
        {
            if (!arrayCached)
            {
                CaculateLotteryWeightArray();
            }
            LotteryCandidate prize = Candidate[AK_Tool.weightArrayRand(weight)];
            Thing thingPrize = ThingMaker.MakeThing(prize.item);
            thingPrize.stackCount = prize.amount;
            GenPlace.TryPlaceThing(thingPrize, parent.Position, ActualMap, ThingPlaceMode.Near);
            LotteryCost();
        }

        private bool HasEnoughSilver()
        {
            int cnt = 0;
            pendingSilver.Clear();
            foreach (Building_OrbitalTradeBeacon item in Building_OrbitalTradeBeacon.AllPowered(ActualMap))
            {
                foreach (IntVec3 tradeableCell in item.TradeableCells)
                {
                    List<Thing> thingList = tradeableCell.GetThingList(ActualMap);
                    for (int i = 0; i < thingList.Count; i++)
                    {
                        Thing t = thingList[i];
                        if (t.def == ThingDefOf.Silver)
                        {
                            cnt += t.stackCount;
                            pendingSilver.Add(t);
                            if (cnt >= Cost) return true;
                        }
                    }
                }
            }
            return false;
        }

        private void LotteryCost()
        {
            if (Cost < 0) return;
            int c = Cost;
            for (int i = 0; i < pendingSilver.Count; ++i)
            {
                pendingSilver[i].stackCount -= Math.Min(c, pendingSilver[i].stackCount);
                if (pendingSilver[i].stackCount == 0) pendingSilver[i].Destroy();
                c -= pendingSilver[i].stackCount;
                if (c <= 0) return;
            }
        }

        private void CaculateLotteryWeightArray()
        {
            this.weight = new int[Candidate.Count];
            this.weight[0] = Candidate[0].weight;
            for (int i = 1; i < Candidate.Count; ++i)
            {
                this.weight[i] = this.weight[i - 1] + Candidate[i].weight;
            }
            //arraySum = this.weight.Last();
            this.arrayCached = true;
        }
    }

}
