using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using AK_Industry.Module;

namespace AK_Industry
{
	public class TC_GasEmitter : ThingComp
	{
		private TCP_GasEmitter ExactProps
		{
			get
			{
				return this.props as TCP_GasEmitter;
			}
		}

		public override void CompTick()
		{
			Log.Error("源石尘组件不应该使用normal tickertype");
		}

		public override void CompTickRare()
		{
			base.CompTickRare();
			this.updateRegionIntervel--;
			if (this.updateRegionIntervel <= 0)
			{
				//this.ScanArea(base.parent.Position, base.parent.Map, this.ExactProps.areaFillRadius);
				affectedCells = MGasEmitter.ScanArea(base.parent.Position, base.parent.Map, this.ExactProps.areaFillRadius);
			}
			MGasEmitter.DoSpawnCycle(this.affectedCells, ExactProps.gasType, ExactProps.countOfThingsToSpawnPerCell, base.parent.Map);
			//this.DoSpawnCycle(this.affectedCells);
		}
		public override void CompTickLong()
		{
			Log.Error("源石尘组件不应该使用long tickertype");
		}
		/*private void PulseThisTick()
		{
			this.updateRegionIntervel = this.ExactProps.ticksBetweenPulse;
			if (this.updateArea)
			{
				this.ScanArea(base.parent.Position, base.parent.Map, this.ExactProps.areaFillRadius);
			}
			this.updateArea = !this.updateArea;
			this.DoSpawnCycle(this.cellsInArea);
		}*/


		

		private List<IntVec3> affectedCells = new List<IntVec3>();

		private int updateRegionIntervel = 0;

	}
}
