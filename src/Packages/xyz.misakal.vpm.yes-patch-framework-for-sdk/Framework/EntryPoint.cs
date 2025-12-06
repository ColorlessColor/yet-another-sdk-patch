using UnityEditor;
using YesPatchFrameworkForVRChatSdk.PatchManagement;

namespace YesPatchFrameworkForVRChatSdk
{
    internal static class EntryPoint
    {
        [InitializeOnLoadMethod]
        public static void Main()
        {
            YesPatchManager.Instance.ApplyPatches();
        }
    }
}