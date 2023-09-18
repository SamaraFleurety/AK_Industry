using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using Verse.AI;

namespace AK_Industry
{
    //需要thing的tick是normal
    public class TCP_PurifyPod : CompProperties
    {
        public int cycleTick = 1000;
        public ThoughtDef thoughtComplete = null;
        public ThoughtDef thoughtInterrupted = null;

        public TCP_PurifyPod()
        {
            compClass = typeof(TC_PurifyPod);
        }
    }

    //周期生命： WorkGiver或者手动清洗->生成Job->JobDriver执行至TryAcceptPawn->Tick->Complete/Interrupt
    public class TC_PurifyPod : ThingComp, IThingHolder, ISuspendableThingHolder, IThingHolderWithDrawnPawn
    {
        #region 实现接口
        public bool IsContentsSuspended => true;

        public float HeldPawnDrawPos_Y => parent.DrawPos.y - 3f / 74f;

        public float HeldPawnBodyAngle => parent.Rotation.Opposite.AsAngle;

        public PawnPosture HeldPawnPosture => PawnPosture.LayingOnGroundFaceUp;
        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return innerContainer;
        }

        #endregion

        public TC_PurifyPod()
        {
            innerContainer = new ThingOwner<Thing>(this);
        }

        private ThingOwner innerContainer;

        private int currentCycleTicksRemaining;
        private int currentCyclePowerCutTicks;


        public bool PowerOn => parent.TryGetComp<CompPowerTrader>().PowerOn;

        public bool Occupied => Occupant != null;

        public Pawn Occupant
        {
            get
            {
                if (innerContainer.Count != 1)
                {
                    return null;
                }
                return innerContainer[0] as Pawn;
            }
        }

        public TC_GasEmitter compGasEmiiter => parent.TryGetComp<TC_GasEmitter>();

        public TCP_PurifyPod Props => props as TCP_PurifyPod;

        public bool TryAcceptPawn(Pawn p)
        {
            if (Occupied) return false;

            bool flag = p.DeSpawnOrDeselect();
            if (p.holdingOwner != null) p.holdingOwner.TryTransferToContainer(p, innerContainer);
            else innerContainer.TryAdd(p);

            if (flag) Find.Selector.Select(p, false, false);

            currentCycleTicksRemaining = Props.cycleTick;
            currentCyclePowerCutTicks = 0;

            return true;
        }

        public override void CompTick()
        {
            //没Pawn直接ret，没必要继续
            if (!Occupied) return;
            innerContainer.ThingOwnerTick();
            if (PowerOn)
            {
                currentCycleTicksRemaining -= 1;
                if (currentCycleTicksRemaining <= 0)
                {
                    CycleCompleted();
                }
            }
            else
            {
                //因为断电而没能洗完
                currentCyclePowerCutTicks++;
                if (currentCyclePowerCutTicks >= 600)
                {
                    currentCyclePowerCutTicks = 0;
                    EjectContents(true);
                }
            }

        }

        private void CycleCompleted()
        {
            EjectContents(false);
            EffectPostComplete();
        }

        public virtual void EffectPostComplete()
        {
            TC_UseEffect_CleanOrgDust.CleanOrgDust(Occupant);
        }

        public void EjectContents(bool interrupted, Map destMap = null)
        {
            if (destMap == null) destMap = parent.Map;
            Pawn p = Occupant;

            innerContainer.TryDropAll(parent.InteractionCell, destMap, ThingPlaceMode.Near);

            if (p != null)
            {
                FilthMaker.TryMakeFilth(parent.InteractionCell, destMap, ThingDefOf.Filth_Water, new IntRange(3, 6).RandomInRange);
                if (p.needs.mood != null)
                {
                    if (interrupted && Props.thoughtInterrupted != null) p.needs.mood.thoughts.memories.TryGainMemory(Props.thoughtInterrupted);
                    else if (Props.thoughtComplete != null) p.needs.mood.thoughts.memories.TryGainMemory(Props.thoughtComplete);
                }
            }
        }

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            string failMessage;
            if (PowerOn)
            {
                if (!Occupied)
                {
                    yield return new FloatMenuOption("AKI_usepurifypod".Translate(),
                       delegate
                       {
                           selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(AKIDefOf.AKI_Job_EnterPurifyPod, parent));
                       }
                       ); yield break;
                }
                else
                {
                    failMessage = "AKI_CannotUseOccupied".Translate();
                }
            }
            else
            {
                failMessage = "CannotUseNoPower".Translate();
            }
            yield return new FloatMenuOption(failMessage, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0); yield break;
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            if (mode == DestroyMode.Deconstruct || mode == DestroyMode.KillFinalize)
            {
                EjectContents(true, previousMap);
            }
            innerContainer.ClearAndDestroyContents();
            base.PostDestroy(mode, previousMap);
        }

        public override void PostDeSpawn(Map map)
        {
            EjectContents(true, map);
        }
        public override void PostDraw() { }
    }
}
