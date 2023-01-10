namespace WorkTool.Core.Modules.Common.Services;

public class TaskCompletionSourceEnumerator : ITaskCompletionSourceEnumerator
{
    private TaskCompletionSource taskCompletionSource;
    private bool disposed;
    private Task current;

    public Task Current
    {
        get
        {
            if (disposed)
            {
                this.ThrowDisposedException();
            }

            return current;
        }
    }

    object IEnumerator.Current => Current;

    public TaskCompletionSourceEnumerator()
    {
        taskCompletionSource = new();
        current = taskCompletionSource.Task;
    }

    public Task MoveNextAndGetCurrent()
    {
        MoveNext();

        return Current;
    }

    public bool MoveNext()
    {
        if (disposed)
        {
            this.ThrowDisposedException();
        }

        taskCompletionSource.SetResult();
        taskCompletionSource = new TaskCompletionSource();
        current = taskCompletionSource.Task;

        return true;
    }

    public void Reset()
    {
        MoveNext();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
        {
            return;
        }

        if (disposing)
        {
            taskCompletionSource.SetResult();
            taskCompletionSource = null!;
            current = null!;
        }

        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
    }
}
