using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using YesPatchFrameworkForVRChatSdk.PatchApi;

namespace YesPatchFrameworkForVRChatSdk
{
    internal static class EntryPoint
    {
        [InitializeOnLoadMethod]
        public static void Main()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var exportPatchTypes = assemblies
                .SelectMany(assembly => assembly.GetCustomAttributes())
                .Where(attribute => attribute is ExportYesPatchAttribute)
                .OfType<ExportYesPatchAttribute>()
                .Select(attribute => attribute.ExportType)
                .ToArray();

            var patches = exportPatchTypes
                .Select(Activator.CreateInstance)
                .OfType<YesPatchBase>()
                .ToArray();

            // check is id conflict
            var duplicateIds = patches
                .GroupBy(patch => patch.Id)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToArray();
            
            if (duplicateIds.Length > 0)
            {
                var duplicateIdsString = string.Join(", ", duplicateIds);
                throw new Exception($"YesPatchFrameworkForVRChatSdk: Duplicate patch ids found: {duplicateIdsString}");
            }

            foreach (var patch in patches)
            {
                Debug.Log(patch.Id);
                patch.Patch();
            }
        }
    }
}