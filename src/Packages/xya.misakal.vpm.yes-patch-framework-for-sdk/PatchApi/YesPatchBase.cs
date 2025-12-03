namespace YesPatchFrameworkForVRChatSdk.PatchApi;

public abstract class YesPatchBase
{
    public abstract string Id { get; }
    public abstract string DisplayName { get; }
    public virtual string Category { get; } = "Uncategorized";
    public virtual string Description { get; } = "No description provided.";

    public virtual bool IsDefaultEnabled { get; } = false;

    public virtual void Configure(IYesPatchConfigureOptions configureOptions)
    {
    }

    public abstract void Patch();
    public abstract void UnPatch();
}