﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;
using RimWorld;

namespace AK_Industry
{
    [HarmonyPatch(typeof(PawnUtility), "GetAvoidGrid")]
    public class PatchAvoidGrid
    {
        [HarmonyPostfix]
        public static void postfix(Pawn p, ref ByteGrid __result)
        {
            if (!AKI_ModSettings.autoAvoidDangerGas) return;
            if (p.AnimalOrWildMan() || (p.Faction != null && p.Faction.def.canUseAvoidGrid))
            {
                MapComp_AvoidGrid comp = p.Map.GetLocalAvoidGrid();
                if (__result == null)
                {
                    __result = comp.cachedAvoidGrid;
                }
                else
                {
                    foreach(int i in comp.orgDustCell)
                    {
                        __result[i] = (byte)Math.Min(255, __result[i] + 50);
                    }
                }
            }
        }
    }
}
