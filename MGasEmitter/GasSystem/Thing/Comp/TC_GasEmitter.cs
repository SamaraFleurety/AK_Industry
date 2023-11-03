using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MGasEmitter
{
	public class TC_GasEmitter : ThingComp
	{
		public bool manualSwitch = true;

		private TCP_GasEmitter ExactProps
		{
			get
			{
				return this.props as TCP_GasEmitter;
			}
		}

		public override void CompTick()
		{
			CompTickRare();
		}

		public override void CompTickRare()
		{
			base.CompTickRare();
			this.updateRegionIntervel--;
			if (this.updateRegionIntervel <= 0)
			{
				updateRegionIntervel = ExactProps.interval;
				if (shouldSpawnGas())
                {
					affectedCells = MGasEmitter.ScanArea(base.parent.Position, base.parent.Map, this.ExactProps.areaFillRadius);
					MGasEmitter.DoSpawnCycle(this.affectedCells, ExactProps.gasDef, ExactProps.countOfThingsToSpawnPerCell, base.parent.Map);
				}
				//this.ScanArea(base.parent.Position, base.parent.Map, this.ExactProps.areaFillRadius);
			}
			//this.DoSpawnCycle(this.affectedCells);
		}
		public override void CompTickLong()
		{
			CompTickRare();
		}

		private bool shouldSpawnGas()
        {
			if (!manualSwitch || !lowerThanHPThreshold()) return false;
			return true;
        }

		public bool lowerThanHPThreshold()
        {
			if ((float)parent.HitPoints / (float)parent.MaxHitPoints <= ExactProps.HPThreshold) return true;
			return false;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
			Scribe_Values.Look(ref manualSwitch, "open", true);
        }

        private List<IntVec3> affectedCells = new List<IntVec3>();

		private int updateRegionIntervel = 0;

	}
}
