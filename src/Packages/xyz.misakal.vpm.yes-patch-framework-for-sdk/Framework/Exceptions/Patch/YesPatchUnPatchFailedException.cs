using System;

namespace YesPatchFrameworkForVRChatSdk.Exceptions.Patch;

public sealed class YesPatchUnPatchFailedException : Exception
{
    internal YesPatchUnPatchFailedException(Exception innerException)
        : base("Failed to unpatch the patch.", innerException)
    {
    }
}