using HarmonyLib;
using Kitchen;
using System.Reflection;

namespace PlateUpCompetitiveMode.Patches
{
    [HarmonyPatch]
    class LocalViewRouterPatch
    {
        static MethodBase TargetMethod()
        {
            return typeof(LocalViewRouter).GetMethod("HandleUpdate", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(typeof(MoneyDisplayView.ViewData));
        }

        //static void Prefix() { }

        static void Prefix(ViewIdentifier e, object data, MessageType type)
        {
            Mod.LogWarning($"== VIEW UPDATE: {e.Identifier}, {data.GetType()}, {type}");
        }
    }
}
