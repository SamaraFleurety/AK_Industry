﻿using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MGasEmitter
{
    public static class MGasEmitter
    {
		public static List<IntVec3> ScanArea(IntVec3 posistion, Map map, float areaFillRadius, RegionType filter = RegionType.Set_Passable)
		{
			List<IntVec3> affectedCells = new List<IntVec3>();

			//起点不允许是空
			if (!GenGrid.InBounds(posistion, map))
			{
				return null;
			}

			//起点可以不符合条件。这种情况下会从周围第一个合格的点开始扫描。
			IntVec3[] nearLoc = new IntVec3[] { posistion, new IntVec3(posistion.x + 1, posistion.y, posistion.z), new IntVec3(posistion.x - 1, posistion.y, posistion.z), new IntVec3(posistion.x, posistion.y, posistion.z + 1), new IntVec3(posistion.x, posistion.y, posistion.z - 1) };
			Region region = null;
			for (int i = 0; i < 5; ++i)
			{
				if (nearLoc[i].InBounds(map))
				{
					region = GridsUtility.GetRegion(nearLoc[i], map, RegionType.Set_Passable);
					if (region != null) goto Find_Region;
				}
			}

			//周围完全没有任何可能的区域
			if (region == null)
			{
				return null;
			}

		Find_Region:
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region traverseRegion) => traverseRegion.door == null, delegate (Region targetRegion)
			{
				foreach (IntVec3 item in targetRegion.Cells)
				{
					if (item.InHorDistOf(posistion, areaFillRadius))
					{
						affectedCells.Add(item);
					}
				}
				return false;
			}, 40, filter);
			return affectedCells;
		}

		public static void DoSpawnCycle(List<IntVec3> affectedCells, GasDef gasDef, int spawnPerCell ,Map map)
		{
			if (affectedCells == null || affectedCells.Count < 1 || gasDef == null || map == null)
			{
				return;
			}
			MapComp_AvoidGrid localAG = map.GetLocalAvoidGrid();
			for (int i = 0; i < affectedCells.Count; i++)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Mote_Stun, null);
				IntVec3 intVec = affectedCells[i];
				if (!DoesCellContain(intVec, map, gasDef))
				{
					thing = ThingMaker.MakeThing(gasDef, null);
					thing.stackCount = spawnPerCell;
					if (!thing.Spawned)
					{
						GenSpawn.Spawn(thing, intVec, map, 0);
						if (MGasEmitter_ModSettings.autoAvoidDangerGas && gasDef.isDanger)
                        {
							localAG.UpdateOriginiumDustCellAt(intVec);
						}
					}
				}
			}
		}

		private static bool DoesCellContain(IntVec3 cell, Map map, ThingDef thingToLookFor)
		{
			return GridsUtility.GetFirstThing(cell, map, thingToLookFor) != null;
		}
	}
}
