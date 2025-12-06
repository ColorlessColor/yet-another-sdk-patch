using System;

namespace YesPatchFrameworkForVRChatSdk.Exceptions.Patch;

public sealed class YesPatchApplyFailedException : Exception
{
    internal YesPatchApplyFailedException(Exception innerException)
        : base("Failed to apply the patch.", innerException)
    {
    }
}