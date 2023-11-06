using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AK_Industry
{
    /*public class TCP_RBLGlower : CompProperties_Glower
    {
        public TCP_RBLGlower()
        {
            compClass = typeof(TC_RBLGlower);
        }
    }*/

    public class TC_RBLGlower : CompGlower
    {
        ThingClass_RecruitBagLight ActualParent => parent as ThingClass_RecruitBagLight;
        int LightStatus => ActualParent.lightStatus;
        public override void Initialize(CompProperties props)
        {
            CompProperties_Glower templateComp = props as CompProperties_Glower;
            
            CompProperties_Glower actualComp = new CompProperties_Glower
            {
                glowRadius = GetLightRadius(),
                glowColor = templateComp.glowColor,
                colorPickerEnabled = false,
                darklightToggle = false
            };
            this.props = actualComp;
        }
        protected override bool ShouldBeLitNow
        {
            get
            {
                if (!base.ShouldBeLitNow) return false;
                if (LightStatus == 0) return false;
                return true;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            ChangeLightMode(ActualParent.lightStatus);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            yield break;
        }

        public void ChangeLightMode(int rarity)
        {
            //if (rarity == 0) 
            Props.glowRadius = GetLightRadius();
            SetGlowColorInternal(GetColor());
            //UpdateLit(parent.Map);
        }

        private int GetLightRadius()
        {
            int lightRadius = 1;
            switch (LightStatus)
            {
                case 1:
                    lightRadius = 8;
                    break;
                case 2:
                    lightRadius = 16;
                    break;
                case 3:
                    lightRadius = 24;
                    break;
                case 4:
                    lightRadius = 32;
                    break;
                default:
                    break;
            }
            return lightRadius;
        }

        private ColorInt GetColor()
        {
            switch (LightStatus)
            {
                case 1:
                    return new ColorInt(255, 255, 255,255);
                case 2:
                    return new ColorInt(162, 166, 250,255);
                case 3:
                    return new ColorInt(255, 254, 52, 255);
                case 4:
                    return new ColorInt(249, 169, 57, 255);
                default:
                    return new ColorInt(255, 255, 255,1);
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
        }
    }
}
