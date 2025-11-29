using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
                throw new Exception($"YesPatchFramework: Duplicate patch ids found: {duplicateIdsString}");
            }

            var patchesFailedToApply = new List<YesPatchBase>();
            foreach (var patch in patches)
            {
                Debug.Log($"[YesPatchFramework] Applying patch: [{patch.Id}] {patch.DisplayName}");
                try
                {
                    patch.Patch();
                }
                catch (Exception ex)
                {
                    Debug.LogError(
                        $"[YesPatchFramework] Failed to apply patch: [{patch.Id}] {patch.DisplayName}");
                    Debug.LogException(ex);

                    patchesFailedToApply.Add(patch);
                }
            }

            var completedMessageBuilder = new StringBuilder();
            completedMessageBuilder.AppendLine(
                $"[YesPatchFramework] Patch process completed. Total: {patches.Length} Errors: {patchesFailedToApply.Count}");
            if (patchesFailedToApply.Count > 0)
            {
                completedMessageBuilder.AppendLine("The following patches failed to apply:");
                foreach (var failedPatch in patchesFailedToApply)
                {
                    completedMessageBuilder.AppendLine($"- [{failedPatch.Id}] {failedPatch.DisplayName}");
                }
            }

            Debug.Log(completedMessageBuilder.ToString());
        }
    }
}