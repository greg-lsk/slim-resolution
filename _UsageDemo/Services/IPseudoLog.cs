namespace _UsageDemo.Services;

internal interface IPseudoLog : IDisposable
{
    public void Log(string message);
}


internal class PseudoLog : IPseudoLog 
{
    private readonly Guid _id = Guid.NewGuid();
    private bool _disposed;

    public void Log(string message)
    {
        ThrowIfDisposed();
        Console.WriteLine($"Logger[{_id}] -> {message}");
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            // free managed resources here
        }
        // free unmanaged resources here (if any)
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}