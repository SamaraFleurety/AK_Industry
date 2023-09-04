using System;

namespace AK_Industry
{
    public class Func_OrgDustToOrgBlood : MathFunction
    {
        //x是严重度
        public override double Value(double x)
        {
            return Math.Min((50 - x) * 0.002 + 1, 1);
        }

    }
}
