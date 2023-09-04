
namespace AK_Industry
{
    public class Func_OrgBloodCauseCrystal : MathFunction
    {
        //x是严重度
        public override double Value(double x)
        {
            return 0.8 - x / 1440.0 * 0.8;
        }
    }
}
