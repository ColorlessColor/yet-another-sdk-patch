using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using YesPatchFrameworkForVRChatSdk.UserInterface.Views.PatchManagement;

namespace YesPatchFrameworkForVRChatSdk.UserInterface.Views.ProjectSettingsProvider;

internal sealed class YesPatchManagementProjectSettingsProvider : SettingsProvider
{
    private YesPatchManagementProjectSettingsProvider(
        string path,
        SettingsScope scopes,
        IEnumerable<string>? keywords = null) : base(path, scopes, keywords)
    {
    }

    public override void OnActivate(string searchContext, VisualElement rootElement)
    {
        var view = new YesPatchManagementView
        {
            style =
            {
                height = new StyleLength(Length.Percent(100))
            }
        };

        rootElement.Add(view);
    }

    [SettingsProvider]
    public static SettingsProvider Create()
    {
        var provider = new YesPatchManagementProjectSettingsProvider(
            "Yes! Patch Framework for VRChat SDK/Patch Management",
            SettingsScope.Project);

        return provider;
    }
}