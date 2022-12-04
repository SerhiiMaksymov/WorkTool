namespace WorkTool.Core.Modules.Common.Extensions;

public static class TaskExtension
{
    public static async Task AfterAsync<TInput>(this Task<TInput> task, Action<TInput> action)
    {
        var input = await task;
        action.Invoke(input);
    }

    public static async Task AfterAsync<TInput>(this Task<TInput> task, Func<TInput, Task> action)
    {
        var input = await task;
        await action.Invoke(input);
    }

    public static async Task<TOutput> AfterAsync<TInput, TOutput>(this Task<TInput>           task,
                                                                  Func<TInput, Task<TOutput>> action)
    {
        var input  = await task;
        var result = await action.Invoke(input);

        return result;
    }

    public static async Task<TOutput> AfterAsync<TInput, TOutput>(this Task<TInput> task, Func<TInput, TOutput> action)
    {
        var input  = await task;
        var result = action.Invoke(input);

        return result;
    }
}