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
    public class RecipeWorker_ExtractLimbOriginium : Recipe_Surgery
    {
        public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
        {
            if (!base.AvailableOnNow(thing, part)) return false;
            if (!(thing is Pawn pawn))
            {
                return false;
            }
            List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
            for (int i = 0; i < hediffs.Count; i++)
            {
                if (IsQualifiedOripathyCrystal(hediffs[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsQualifiedOripathyCrystal(Hediff h)
        {
            if (h.def == AKIDefOf.AKI_Hediff_OripathyCrystal || h.def == AKIDefOf.AKI_Hediff_OripathyCrystalLethal)
            {
                if (h.Severity >= 4) return true;
            }
            if (h.def == AKIDefOf.AKI_Hediff_OripathyCrystalTrans) return true;
            return false;
        }

        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
        {
            List<Hediff> allHediffs = pawn.health.hediffSet.hediffs;
            for (int i = 0; i < allHediffs.Count; i++)
            {
                if (IsQualifiedOripathyCrystal(allHediffs[i]))
                {
                    yield return allHediffs[i].Part;
                }
            }
        }

        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            bool flag = IsViolationOnPawn(pawn, part, Faction.OfPlayer);
            if (billDoer != null)
            {
                if (CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
                {
                    return;
                }
                TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
                if (!pawn.health.hediffSet.GetNotMissingParts().Contains(part))
                {
                    return;
                }

                pawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 99999f, 999f, -1f, null, part));

                GenSpawn.Spawn(AKIDefOf.AKI_Item_CleaningKitFloral, billDoer.Position, billDoer.Map);

            }
            if (flag)
            {
                ReportViolation(pawn, billDoer, pawn.HomeFaction, -70);
            }
        }

        public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
        {
            return "extractorgfrom".Translate();
        }
    }
}
