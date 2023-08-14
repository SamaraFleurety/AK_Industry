using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AK_Industry
{
    public class Func_OrgDustToOrgBlood : MathFunction
    {
        public override double Value(double x)
        {
            return Math.Min((50 - x) * 0.002 + 1, 1);
        }

    }
}
