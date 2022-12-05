namespace WorkTool.Core.Modules.Common.Services;

public class ToStringHumanizing<TInput> : IHumanizing<TInput, string>
{
    public string Humanize(TInput input)
    {
        return input?.ToString();
    }
}
