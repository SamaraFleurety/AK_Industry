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
    //Reactor Over Clocking 我嫌全称长
    public class TCP_ReactorOC : CompProperties_Power
    {
        public int powerOCOffset;
        public TCP_ReactorOC()
        {
            this.compClass = typeof(TC_ReactorOC);
        }
    }

    public class TC_ReactorOC : CompPowerPlant
    {
        bool overrided = false;

        TCP_ReactorOC ExactProps => props as TCP_ReactorOC;

        float originalPower
        {
            get
            {
                return parent.def.GetCompProperties<CompProperties_Power>().PowerConsumption;
            }
        }

        int PowerOffset => ExactProps.powerOCOffset;

        TC_GasEmitter GasEmitter => parent.GetComp<TC_GasEmitter>();

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            GasEmitter.manualSwitch = overrided;
        }

        public override void CompTick()
        {
            base.CompTick();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref overrided, "override", false);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            yield return new Command_Action
            {
                defaultLabel = "反应堆超载",
                defaultDesc = "超载desc",
                icon = ContentFinder<Texture2D>.Get("UI/Abilities/Medic/HealEnhance"),
                action = delegate ()
                {
                    overrided = !overrided;
                    GasEmitter.manualSwitch = true;
                }
            };
        }

        public override void UpdateDesiredPowerOutput()
        {
            base.UpdateDesiredPowerOutput();
            if(PowerOutput != 0f)
            {
                if (overrided)
                {
                    PowerOutput += PowerOffset;
                    GasEmitter.manualSwitch = true;
                }
                else GasEmitter.manualSwitch = false;
            }
            else
            {
                GasEmitter.manualSwitch = false;
            }
        }
    }

}
