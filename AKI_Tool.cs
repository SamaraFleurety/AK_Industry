﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AK_Industry
{
    public static class AKI_Tool
    {
        public static MapComp_AvoidGrid GetLocalAvoidGrid(this Map map)
        {
            MapComp_AvoidGrid comp = map.GetComponent<MapComp_AvoidGrid>();
            if (comp == null)
            {
                comp = new MapComp_AvoidGrid(map);
                map.components.Add(comp);
            }
            return comp;
        }
    }
}
