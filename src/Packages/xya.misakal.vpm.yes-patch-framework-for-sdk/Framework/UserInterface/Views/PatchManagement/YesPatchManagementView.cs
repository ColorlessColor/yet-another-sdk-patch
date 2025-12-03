using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using YesPatchFrameworkForVRChatSdk.PatchManagement;
using YesPatchFrameworkForVRChatSdk.UserInterface.Controls.PatchManagement;
using YesPatchFrameworkForVRChatSdk.UserInterface.StateManagement;

namespace YesPatchFrameworkForVRChatSdk.UserInterface.Views.PatchManagement;

internal sealed class YesPatchManagementView : VisualElement
{
    private readonly YesPatchManagerStateManager _patchManagerStateManager = YesPatchManagerStateManager.Instance;
    private readonly YesPatchManager _yesPatchManager = YesPatchManager.Instance;

    private readonly VisualElement _contentContainer;
    private readonly VisualElement _welcomePageContainer;

    private VisualElement? _patchSettingsUi;

    public YesPatchManagementView()
    {
        var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            "Packages/xyz.misakal.vpm.yes-patch-framework-for-sdk/Framework/UserInterface/Views/PatchManagement/" +
            nameof(YesPatchManagementView) + ".uxml");
        tree.CloneTree(this);

        _contentContainer = this.Q<VisualElement>("patch-management-content-container");
        _welcomePageContainer = this.Q<VisualElement>("welcome-page-container");

        var patchesListView = this.Q<ListView>("patches-list-view");

        var patches = _yesPatchManager.Patches.ToList();
        patchesListView.makeItem = () => new VisualElement();
        patchesListView.bindItem = (view, index) =>
        {
            var item = new YesPatchListItem(patches[index], _patchManagerStateManager);
            view.Add(item);
        };

        patchesListView.fixedItemHeight = 67;
        patchesListView.itemsSource = patches;

        patchesListView.selectionChanged += objects =>
        {
            if (objects.FirstOrDefault(obj => obj is YesPatch) is not YesPatch selectedPatch)
                return;

            CreateSettingsUi(selectedPatch);
        };
    }

    private void CreateSettingsUi(YesPatch patch)
    {
        _welcomePageContainer.style.display = new StyleEnum<DisplayStyle>(StyleKeyword.None);

        if (_patchSettingsUi is not null && _contentContainer.Contains(_patchSettingsUi))
            _contentContainer.Remove(_patchSettingsUi);

        _patchSettingsUi = new YesPatchSettingsUi(patch);
        _patchSettingsUi.style.height = new StyleLength(Length.Percent(100));

        _contentContainer.Add(_patchSettingsUi);
    }
}