using HarmonyLib;
using MessagePack.Resolvers;
using System;

namespace PlateUpCompetitiveMode.Patches
{
    [HarmonyPatch(typeof(DynamicUnionResolver), "BuildType")]
    class MessagePackPatch
    {
        static void Prefix(Type type)
        {
            Mod.LogInfo($"==== AutoUnion Init for type {type.FullName}");
        }
    }
}
