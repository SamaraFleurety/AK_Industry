using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using AK_DLL;

namespace AK_Industry
{
    public class HCP_LatentCrystal : HediffCompProperties
    {
        public HCP_LatentCrystal()
        {
            this.compClass = typeof(HC_LatentCrystal);
        }
    }

    public class HC_LatentCrystal : HediffComp
    {
        float transcendChance = 0.01f;
        public override void CompPostPostRemoved()
        {
            if (!shouldCrystallize()) return;
            BodyPartDef def = parent.Part.def;
            int rd = UnityEngine.Random.Range(0, 100);
            BodyPartRecord record = this.parent.Part;
            //Log.Message($"{rd} : {transcendChance * 100}");
            //激发结晶
            if (rd <= transcendChance * 100)
            {
                Log.Message($"激发 at {parent.Part.Label}");
                AbilityEffect_AddHediff.AddHediff(this.Pawn, AKIDefOf.AKI_Hediff_OripathyCrystalLethal, def, record, severity: -10); 
                AbilityEffect_AddHediff.AddHediff(this.Pawn, AKIDefOf.AKI_Hediff_OripathyCrystal, def, record, severity: -10);
                AbilityEffect_AddHediff.AddHediff(this.Pawn, AKIDefOf.AKI_Hediff_OripathyCrystalTrans, def, record, severity: 1);
            }
            else
            {
                //致死结晶
                if (def == BodyPartDefOf.Brain || def == BodyPartDefOf.Heart)
                {
                    AbilityEffect_AddHediff.AddHediff(this.Pawn, AKIDefOf.AKI_Hediff_OripathyCrystalLethal, def, record, severity: 1.1f);
                }
                //不致死结晶
                else
                {
                    AbilityEffect_AddHediff.AddHediff(this.Pawn, AKIDefOf.AKI_Hediff_OripathyCrystal, def, record, severity: 1.1f);
                }
            }
        }

        private bool shouldCrystallize()
        {
            if (Pawn.Dead) return false;
            foreach(Hediff h in Pawn.health.hediffSet.hediffs)
            {
                if (h.Part == this.parent.Part && h.def == AKIDefOf.AKI_Hediff_OripathyCrystalTrans) return false;
            }

            /*IEnumerable<Hediff> set = Pawn.health.hediffSet.hediffs.Where((Hediff h) => (h.Part == this.parent.Part && h.def == HediffDefof.AKI_Hediff_OripathyCrystalTrans));
            if (set.Count() > 0) return false;*/
            return true;
        }

    }


}
