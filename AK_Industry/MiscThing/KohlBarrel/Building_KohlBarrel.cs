using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using UnityEngine;

namespace AK_Industry
{
    public class Building_KohlBarrel : Building
    {
        int beerCnt = 0;
        int beerCapacity = 25;
        int orgCnt = 0;
        int orgCapacity = 1;

        public float progress = 0;

        private static readonly Vector2 BarSize = new Vector2(0.55f, 0.1f);

        private static readonly Color BarZeroProgressColor = new Color(0.4f, 0.27f, 0.22f);

        private static readonly Color BarFermentedColor = new Color(0.9f, 0.85f, 0.2f);

        private static readonly Material BarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f));

        private Material barFilledCachedMat;
        private Material BarFilledMat
        {
            get
            {
                if (barFilledCachedMat == null)
                {
                    barFilledCachedMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.Lerp(BarZeroProgressColor, BarFermentedColor, progress));
                }
                return barFilledCachedMat;
            }
        }

        public bool PowerOn
        {
            get
            {
                CompPowerTrader comp = this.TryGetComp<CompPowerTrader>();
                if (comp != null && !comp.PowerOn) return false;
                return true;
            }
        }

        public int SpaceToFill => orgCapacity + beerCapacity - orgCnt - beerCnt;

        #region 内容物处理
        public bool FilledBeer()
        {
            if (beerCnt < beerCapacity) return false;
            return true;
        }

        public bool Filled()
        {
            if (beerCnt >= beerCapacity && orgCnt >= orgCapacity) return true;
            return false;
        }

        public void AddBeer(Thing beer)
        {
            int cnt = Math.Min(beerCapacity - beerCnt, beer.stackCount);
            if (cnt > 0)
            {
                beerCnt += cnt;
                beer.SplitOff(cnt).Destroy();
            }
        }
        public void AddOrg(Thing org)
        {
            int cnt = Math.Min(orgCapacity - orgCnt, org.stackCount);
            if (cnt > 0)
            {
                orgCnt += cnt;
                org.SplitOff(cnt).Destroy();
            }
        }

        public void Reset()
        {
            beerCnt = 0;
            orgCnt = 0;
            progress = 0;
        }
        #endregion

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref beerCnt, "beer", 0);
            Scribe_Values.Look(ref orgCnt, "org", 0);
            Scribe_Values.Look(ref progress, "progress", 0);
        }

        public override void TickLong()
        {
            base.TickLong();
            //fixme:改成5天后完成
            if (Filled() && progress < 1.005 && PowerOn) { progress += 0.1f; }
        }

        public override void Draw()
        {
            base.Draw();
            if (progress > 0.001)
            {
                Vector3 drawPos = DrawPos;
                drawPos.y += 3f / 74f;
                drawPos.z += 0.25f;
                GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
                r.center = drawPos;
                r.size = BarSize;
                r.fillPercent = progress;
                r.filledMat = BarFilledMat;
                r.unfilledMat = BarUnfilledMat;
                r.margin = 0.1f;
                r.rotation = Rot4.North;
                GenDraw.DrawFillableBar(r);
            }
        }

        public override string GetInspectString()
        {
            return base.GetInspectString();
        }

    }
}
