using YesPatchFrameworkForVRChatSdk.PatchApi;
using YetAnotherPatchForVRChatSdk.Patches;

[assembly: ExportYesPatch(typeof(RemoteConfigCachePatch))]
[assembly: ExportYesPatch(typeof(AlwaysAgreeCopyrightAgreementPatch))]