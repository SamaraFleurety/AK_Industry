using System;
using RimWorld;
using Verse;

namespace AK_Industry
{
    //并发症：技能点在-4~-8之间随机减少
    public class HCP_DecreaseSkills : HediffCompProperties
    {
        public HCP_DecreaseSkills()
        {
            this.compClass = typeof(HC_DecreaseSkills);
        }
    }

    public class HC_DecreaseSkills : HediffComp
    {
        private static Random random = new Random();
        public override void CompPostTick(ref float severityAdjustment)
        {
            SkillRecord skill = Pawn.skills.skills.RandomElement();
            int amount = random.Next(-8, -3);
            skill.Level += amount;
            parent.comps.Remove(this);
        }
    }
}
