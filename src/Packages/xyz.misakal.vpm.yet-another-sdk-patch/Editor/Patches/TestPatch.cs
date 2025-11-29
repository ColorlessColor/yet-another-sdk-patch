using HarmonyLib;
using UnityEngine;
using YesPatchFrameworkForVRChatSdk.PatchApi;

namespace YetAnotherPatchForVRChatSdk.Patches;

public sealed class TestPatch : YesPatchBase
{
    public override string Id => "com.example.test-patch";
    public override string DisplayName => "Test Patch";

    private readonly Harmony _harmonyInstance = new("com.example.test-patch");

    public override void Configure(IYesPatchConfigureOptions configureOptions)
    {
    }

    public override void Patch()
    {
        _harmonyInstance.PatchAll(typeof(TestPatch));
        Debug.Log("Patched!");
    }

    public override void UnPatch()
    {
        _harmonyInstance.UnpatchSelf();
        Debug.Log("UnPatched!");
    }

    [HarmonyPatch(typeof(VRCSdkControlPanel), "ShowControlPanel")]
    [HarmonyPrefix]
    private static void PatchShowControlPanel()
    {
        Debug.Log("VRCSdkControlPanel.ShowControlPanel() Patched!");
    }
}