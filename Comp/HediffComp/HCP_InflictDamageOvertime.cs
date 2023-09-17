using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AK_Industry
{
    //用于烈性源石尘的持续造成伤害
    public class HCP_InflictDamageOvertime : HediffCompProperties
    {
        public DamageDef damageDef;
        public float amount = 5;
        public int interval = 600;

        public HCP_InflictDamageOvertime()
        {
            this.compClass = typeof(HC_InflictDamageOvertime);
        }
    }

    public class HC_InflictDamageOvertime : HediffComp
    {
        private HCP_InflictDamageOvertime Props => props as HCP_InflictDamageOvertime;

        private int tick = 0;

        public override void CompPostTick(ref float severityAdjustment)
        {
            ++tick;
            if (tick >= Props.interval)
            {
                tick = 0;
                InflictDamage();
            }
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref tick, "tick");
        }

        private void InflictDamage()
        {
            HediffSet hediffSet = Pawn.health.hediffSet;
            IEnumerable<BodyPartRecord> source;

            source = from x in hediffSet.GetNotMissingParts()
                     where x.depth == BodyPartDepth.Outside || (x.depth == BodyPartDepth.Inside && x.def.IsSolid(x, hediffSet.hediffs))
                     select x;
            source = from x in source
                     where !Pawn.health.hediffSet.hediffs.Any((Hediff y) => y.Part == x && y.CurStage != null && y.CurStage.partEfficiencyOffset < 0f)
                     select x;

            BodyPartRecord bodyPartRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);

            float damageAmount = Props.amount;

            float statValue = Pawn.GetStatValue(StatDefOf.IncomingDamageFactor);
            if (statValue > 0f)
            {
                damageAmount = (int)((float)damageAmount / statValue);
            }

            DamageInfo dinfo = new DamageInfo(Props.damageDef, damageAmount, 999f, -1f, null, bodyPartRecord);
            dinfo.SetAllowDamagePropagation(val: false);
            Pawn.TakeDamage(dinfo);
        }
    }
}
