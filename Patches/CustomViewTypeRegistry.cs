using HarmonyLib;
using Kitchen;
using PlateUpCompetitiveMode.Views;
using UnityEngine;

namespace PlateUpCompetitiveMode.Patches
{
    [HarmonyPatch(typeof(LocalViewRouter), "GetPrefab")]
    class CustomViewTypeRegistry
    {
        static bool Prefix(ViewType view_type, ref GameObject __result)
        {
            if (CustomViewType.Prefabs.TryGetValue(view_type, out var value))
            {
                __result = value;
                return false;
            }

            return true;
        }
    }
}
