using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AK_Industry
{
    public class Func_OrgBloodCauseCrystal : MathFunction
    {
        public override double Value(double x)
        {
            return 0.8 - x / 1440.0 * 0.8;
        }
    }
}
