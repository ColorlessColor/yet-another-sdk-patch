using UnityEditor;
using UnityEngine.UIElements;

namespace YesPatchFrameworkForVRChatSdk.UserInterface.Controls.PatchManagement;

internal sealed class YesPatchListCategoryItem : VisualElement
{
    private readonly Label _categoryHeaderLabel;

    public YesPatchListCategoryItem(string categoryName)
    {
        var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
            "Packages/xyz.misakal.vpm.yes-patch-framework-for-sdk/Framework/UserInterface/Controls/PatchManagement/" +
            nameof(YesPatchListCategoryItem) + ".uxml");
        tree.CloneTree(this);

        _categoryHeaderLabel = this.Q<Label>("patch-category-header-text");

        _categoryHeaderLabel.text = categoryName;
        _categoryHeaderLabel.tooltip = categoryName;
    }
}