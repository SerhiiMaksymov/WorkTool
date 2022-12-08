namespace WorkTool.Core.Modules.Common.Interfaces;

public interface IHumanizing<in TInput, out TOutput> where TInput : notnull
{
    TOutput Humanize(TInput input);
}
