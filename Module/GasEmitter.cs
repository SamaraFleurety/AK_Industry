using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AK_Industry.Module
{
    public static class MGasEmitter
    {
		public static List<IntVec3> ScanArea(IntVec3 posistion, Map map, float areaFillRadius)
		{
			List<IntVec3> affectedCells = new List<IntVec3>();
			//this.affectedCells.Clear();
			if (!GenGrid.InBounds(posistion, map))
			{
				return null;
			}
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
			if (region == null)
			{
				return null;
			}

		Find_Region:
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region traverseRegion) => traverseRegion.door == null, delegate
				(Region targetRegion)
			{
				foreach (IntVec3 item in targetRegion.Cells)
				{
					if (item.InHorDistOf(posistion, areaFillRadius))
					{
						affectedCells.Add(item);
					}
				}
				return false;
			}, 16, RegionType.Set_Passable);
			return affectedCells;
		}

		public static void DoSpawnCycle(List<IntVec3> affectedCells, ThingDef gasType, int spawnPerCell ,Map map)
		{
			if (affectedCells.Count < 1 || gasType == null)
			{
				return;
			}
			for (int i = 0; i < affectedCells.Count; i++)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Mote_Stun, null);
				IntVec3 intVec = affectedCells[i];
				if (!DoesCellContain(intVec, map, gasType))
				{
					thing = ThingMaker.MakeThing(gasType, null);
					thing.stackCount = spawnPerCell;
					if (!thing.Spawned)
					{
						GenSpawn.Spawn(thing, intVec, map, 0);
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
