using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace AK_Industry 
{
    public class TCP_ReactorOverride : CompProperties
    {
        public int powerOffset;
        public TCP_ReactorOverride()
        {
            this.compClass = typeof(TC_ReactorOverride);
        }
    }

    public class TC_ReactorOverride : ThingComp
    {
        bool overrided = false;

        TCP_ReactorOverride ExactProps => props as TCP_ReactorOverride;

        float originalPower
        {
            get
            {
                return parent.def.GetCompProperties<CompProperties_Power>().PowerConsumption;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            //读档后发电量修复
            if (overrided && respawningAfterLoad) 
            {
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref overrided, "override", false);
        }
        private void AdjustPowerTrader()
        {
            CompPowerTrader comp = parent.GetComp<CompPowerTrader>();
            if (comp == null)
            {
                Log.Message("MIS. overriding non reactor building");
                return;
            }
            if (overrided)
            {
                comp.PowerOutput = originalPower + ExactProps.powerOffset;
            } 
            else
            {
                comp.PowerOutput = originalPower;
            }
        }
    }

}
