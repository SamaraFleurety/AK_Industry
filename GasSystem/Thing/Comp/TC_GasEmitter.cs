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
				updateRegionIntervel = ExactProps.intervel;
				if (lowerThanHPThreshold())
                {
					affectedCells = MGasEmitter.ScanArea(base.parent.Position, base.parent.Map, this.ExactProps.areaFillRadius);
					MGasEmitter.DoSpawnCycle(this.affectedCells, ExactProps.gasType, ExactProps.countOfThingsToSpawnPerCell, base.parent.Map);
				}
				//this.ScanArea(base.parent.Position, base.parent.Map, this.ExactProps.areaFillRadius);
			}
			//this.DoSpawnCycle(this.affectedCells);
		}
		public override void CompTickLong()
		{
			CompTickRare();
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

		public bool lowerThanHPThreshold()
        {
			if ((float)parent.HitPoints / (float)parent.MaxHitPoints <= ExactProps.HPThreshold) return true;
			return false;
        }
		

		private List<IntVec3> affectedCells = new List<IntVec3>();

		private int updateRegionIntervel = 0;

	}
}
