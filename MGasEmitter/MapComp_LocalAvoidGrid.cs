using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace MGasEmitter
{
    public class MapComp_AvoidGrid : MapComponent
    {
        public HashSet<int> orgDustCell = new HashSet<int>();
        public ByteGrid cachedAvoidGrid;
        //public ByteGrid cachedAvoidGrid_Full;

        public MapComp_AvoidGrid(Map map) : base(map)
        {
            cachedAvoidGrid = new ByteGrid(map);
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref orgDustCell, "orgDustLoc", LookMode.Value);
        }

        public override void FinalizeInit()
        {
            UpdateLocalAvoidGrid();
        }

        public void UpdateOriginiumDustCellAt(IntVec3 cell, bool isSpawn = true)
        {
            int index = CellIndicesUtility.CellToIndex(cell, map.Size.x);
            byte hatred = (byte)(isSpawn ? 50 : 0);
            if (isSpawn)
            {
                orgDustCell.Add(index);
            }
            else orgDustCell.Remove(index);

            cachedAvoidGrid[index] = hatred;
        }

        public void UpdateLocalAvoidGrid()
        {
            cachedAvoidGrid.Clear(0);
            foreach(int i in orgDustCell)
            {
                cachedAvoidGrid[i] = 50;
            }
        }
    }
}
