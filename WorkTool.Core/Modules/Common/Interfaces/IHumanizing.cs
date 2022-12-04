namespace WorkTool.Core.Modules.Common.Interfaces;

public interface IHumanizing<in TInput, out TOutput>
{
    TOutput Humanize(TInput input);
}