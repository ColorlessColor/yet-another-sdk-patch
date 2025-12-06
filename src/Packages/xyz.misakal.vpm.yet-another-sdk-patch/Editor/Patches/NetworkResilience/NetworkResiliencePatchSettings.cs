using System;
using UnityEditor;
using UnityEngine;
using YesPatchFrameworkForVRChatSdk.PatchApi;

namespace YetAnotherPatchForVRChatSdk.Patches.NetworkResilience
{
    internal sealed class NetworkResiliencePatchSettings : ScriptableObject
    {
        internal static NetworkResiliencePatchSettings GetOrCreateSettings()
        {
            var settingsPath =
                PatchSettingsHelper.CreateSettingsFolderIfNotExists(
                    "xyz.misakal.vpm.yet-another-sdk-patch.network-resilience",
                    nameof(NetworkResiliencePatchSettings) + ".asset");

            var settings = AssetDatabase.LoadAssetAtPath<NetworkResiliencePatchSettings>(settingsPath);
            if (settings != null)
                return settings;

            settings = CreateInstance<NetworkResiliencePatchSettings>();

            AssetDatabase.CreateAsset(settings, settingsPath);
            AssetDatabase.SaveAssets();
            return settings;
        }

        public ProxyMode proxyMode = ProxyMode.SystemProxy;
        public string customProxyAddress = string.Empty;

        public bool TryGetProxyUri(out Uri proxyUri)
        {
            if (!Uri.TryCreate(customProxyAddress, UriKind.Absolute, out proxyUri))
                return false;

            return proxyUri.Scheme == "http";
        }
    }

    internal enum ProxyMode
    {
        NoProxy,
        SystemProxy,
        CustomProxy
    }
}