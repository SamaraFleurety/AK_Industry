using System.Reflection;
using HarmonyLib;
using Verse;

namespace AK_Industry
{
    [StaticConstructorOnStartup]
    public class PatchMain
    {
        static PatchMain()
        {
            var instance = new Harmony("AK_Industry");
            instance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
