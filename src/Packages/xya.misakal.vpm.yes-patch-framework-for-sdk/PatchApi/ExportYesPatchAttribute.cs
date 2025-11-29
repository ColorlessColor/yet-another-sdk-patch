using System;

namespace YesPatchFrameworkForVRChatSdk.PatchApi;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class ExportYesPatchAttribute : Attribute
{
    public Type ExportType { get; }

    public ExportYesPatchAttribute(Type exportPatchType)
    {
        if (!exportPatchType.IsSubclassOf(typeof(YesPatchBase)))
            throw new ArgumentException("Export Patch Type must inherited from YesPatchBase.", nameof(exportPatchType));

        ExportType = exportPatchType;
    }
}