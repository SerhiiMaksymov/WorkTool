namespace WorkTool.Core.XUnitTests.Services;

public class TestTaskCompletionSourceEnumerator : ITaskCompletionSourceEnumerator
{
    private TaskCompletionSource taskCompletionSource;
    private bool disposed;
    private Task current;

    public TaskCompletionSource WaitGetCurrent { get; private set; }

    public Task Current
    {
        get
        {
            try
            {
                if (disposed)
                {
                    this.ThrowDisposedException();
                }

                return current;
            }
            finally
            {
                WaitGetCurrent.SetResult();
            }
        }
    }

    object IEnumerator.Current => Current;

    public TestTaskCompletionSourceEnumerator()
    {
        WaitGetCurrent = new();
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
        WaitGetCurrent = new TaskCompletionSource();
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
