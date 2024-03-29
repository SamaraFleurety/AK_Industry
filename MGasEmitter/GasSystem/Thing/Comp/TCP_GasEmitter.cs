﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace MGasEmitter
{
	public class TCP_GasEmitter : CompProperties
	{
		public int interval = 1;

		public float areaFillRadius = 3.5f;

		public GasDef gasDef;

		public int countOfThingsToSpawnPerCell = 1;

		public float fuelCostPerSpawn;

		public bool addToExsistingStacks;

		public float HPThreshold = 10f;

		public TCP_GasEmitter()
		{
			this.compClass = typeof(TC_GasEmitter);
		}
	}
}
