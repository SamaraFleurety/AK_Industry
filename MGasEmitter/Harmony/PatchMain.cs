using System.Reflection;
using HarmonyLib;
using Verse;

namespace MGasEmitter
{
    [StaticConstructorOnStartup]
    public class PatchMain
    {
        static PatchMain()
        {
            var instance = new Harmony("MGasEmitter");
            instance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
