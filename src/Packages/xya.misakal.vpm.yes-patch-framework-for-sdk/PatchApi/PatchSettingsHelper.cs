using System;
using System.IO;
using System.Linq;
using UnityEditor;

namespace YesPatchFrameworkForVRChatSdk.PatchApi;

public static class PatchSettingsHelper
{
    private const string FolderName = "YesPatchFrameworkForVRChatSdk";
    private const string FolderPath = "Assets/" + FolderName;

    public static string CreateSettingsFolderIfNotExists(string patchSettingsId)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        if (patchSettingsId.Any(c => invalidChars.Contains(c)))
            throw new ArgumentException("The patchSettingsId contains invalid characters.", nameof(patchSettingsId));

        if (!AssetDatabase.IsValidFolder(FolderPath))
            AssetDatabase.CreateFolder("Assets", FolderName);

        if (!AssetDatabase.IsValidFolder(FolderPath + "/" + patchSettingsId))
            AssetDatabase.CreateFolder(FolderPath, patchSettingsId);

        return FolderPath + "/" + patchSettingsId;
    }

    public static string CreateSettingsFolderIfNotExists(string patchSettingsId, string settingsFileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        if (settingsFileName.Any(c => invalidChars.Contains(c)))
            throw new ArgumentException("The settingsFileName contains invalid characters.", nameof(patchSettingsId));

        return CreateSettingsFolderIfNotExists(patchSettingsId) + "/" + settingsFileName;
    }
}