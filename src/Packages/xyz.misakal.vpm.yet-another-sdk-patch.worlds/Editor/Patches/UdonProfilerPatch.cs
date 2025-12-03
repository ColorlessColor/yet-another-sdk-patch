using System;
using System.Collections.Generic;
using HarmonyLib;
using JetBrains.Annotations;
using Unity.Profiling;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using YesPatchFrameworkForVRChatSdk.PatchApi;
using UnityObject = UnityEngine.Object;

namespace YetAnotherPatchForVRChatSdk.Worlds.Patches;

internal sealed class UdonProfilerPatch : YesPatchBase
{
    public override string Id => "xyz.misakal.vpm.yet-another-sdk-patch.worlds.udon-profiler";
    public override string DisplayName => "Udon Profiler";

    public override string Description =>
        "Add more detail markers to Unity Profiler for Udon behaviours execution. "
        + "Warning: If your UdonBehaviour is not running on the main thread, it may cause an error!";

    private readonly Harmony _harmony = new("xyz.misakal.vpm.yet-another-sdk-patch.worlds.udon-profiler");

    public override void Patch()
    {
        _harmony.PatchAll(typeof(UdonProfilerPatch));
    }

    public override void UnPatch()
    {
        _harmony.UnpatchSelf();
    }

    [HarmonyPatch(typeof(UdonBehaviour), nameof(UdonBehaviour.RunProgram), typeof(uint))]
    [HarmonyPrefix]
    private static void UdonBehaviourRunProgramPrefix(
        uint entryPoint,
        UdonBehaviour __instance,
        ref IUdonProgram ____program,
        out ProfilerMarker __state)
    {
        __state = ProfilerMarkerScope.TryGetProfilerMarker(__instance, ____program, entryPoint);
        __state.Begin(__instance.gameObject);
    }

    [HarmonyPatch(typeof(UdonBehaviour), nameof(UdonBehaviour.RunProgram), typeof(uint))]
    [HarmonyFinalizer]
    public static void Finalizer(ref ProfilerMarker __state)
    {
        __state.End();
    }

    private readonly struct ProfilerMarkerScope : IDisposable
    {
        private readonly ProfilerMarker _marker;

        private ProfilerMarkerScope(ProfilerMarker profilerMarker, UnityObject obj)
        {
            _marker = profilerMarker;
            _marker.Begin(obj);
        }

        public void Dispose()
        {
            _marker.End();
        }

        private static readonly Dictionary<(int, uint), ProfilerMarker> ProfilerMarkerEventCache = new();

        [PublicAPI]
        public static ProfilerMarker TryGetProfilerMarker(
            UdonBehaviour udonBehaviour, IUdonProgram udonProgram, uint entryPoint)
        {
            var programID = udonBehaviour.programSource.GetInstanceID();
            var key = (programID, entryPoint);

            if (!ProfilerMarkerEventCache.TryGetValue(key, out var profilerMarker))
            {
                var programName = udonBehaviour.programSource.name;
                if (!udonProgram.EntryPoints.TryGetSymbolFromAddress(entryPoint, out var symbolName))
                {
                    symbolName = "[UNKNOWN]";
                }

                profilerMarker = new ProfilerMarker($"Udon {programName}.{symbolName}");
                ProfilerMarkerEventCache[key] = profilerMarker;
            }

            return profilerMarker;
        }

        [PublicAPI]
        public static ProfilerMarkerScope Auto(UdonBehaviour udonBehaviour, IUdonProgram udonProgram, uint entryPoint)
        {
            return new ProfilerMarkerScope
            (
                TryGetProfilerMarker(udonBehaviour, udonProgram, entryPoint), udonBehaviour.gameObject
            );
        }
    }
}