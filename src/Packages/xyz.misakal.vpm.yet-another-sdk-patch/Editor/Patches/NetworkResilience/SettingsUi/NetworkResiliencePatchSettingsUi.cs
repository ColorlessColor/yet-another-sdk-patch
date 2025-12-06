using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace YetAnotherPatchForVRChatSdk.Patches.NetworkResilience.SettingsUi;

internal sealed class NetworkResiliencePatchSettingsUi : VisualElement
{
    private readonly NetworkResiliencePatchSettings _settings = NetworkResiliencePatchSettings.GetOrCreateSettings();
    private readonly SerializedObject _settingsSerializedObject;

    private readonly VisualElement _customProxyUriValidationMessage;
    private readonly TextField _customProxyUriField;

    public NetworkResiliencePatchSettingsUi()
    {
        var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            "Packages/xyz.misakal.vpm.yet-another-sdk-patch/Editor/Patches/NetworkResilience/SettingsUi/" +
            nameof(NetworkResiliencePatchSettingsUi) + ".uxml");
        tree.CloneTree(this);

        _customProxyUriField = this.Q<TextField>("proxy-uri");
        _customProxyUriValidationMessage = this.Q<VisualElement>("custom-proxy-uri-valid-error");

        _settingsSerializedObject = new SerializedObject(_settings);

        UpdateUi();

        this.TrackSerializedObjectValue(_settingsSerializedObject, _ => UpdateUi());
        this.Bind(_settingsSerializedObject);
    }

    private void UpdateUi()
    {
        var isCustomProxySelected = _settings.proxyMode == ProxyMode.CustomProxy;

        _customProxyUriField.style.display = new StyleEnum<DisplayStyle>(
            isCustomProxySelected ? DisplayStyle.Flex : DisplayStyle.None
        );

        if (!isCustomProxySelected)
        {
            _customProxyUriValidationMessage.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            return;
        }

        var isCustomUriValid = _settings.TryGetProxyUri(out _);
        _customProxyUriValidationMessage.style.display = new StyleEnum<DisplayStyle>(
            isCustomUriValid ? DisplayStyle.None : DisplayStyle.Flex
        );
    }
}