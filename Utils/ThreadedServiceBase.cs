namespace CS2Cheat.Utils;

public abstract class ThreadedServiceBase :
    IDisposable
{
    #region

    protected virtual string ThreadName => nameof(ThreadedServiceBase);

    protected virtual TimeSpan ThreadTimeout { get; set; } = new(0, 0, 0, 3);

    protected virtual TimeSpan ThreadFrameSleep { get; set; } = new(0, 0, 0, 0, 1);

    private Thread Thread { get; set; }

    #endregion

    #region

    protected ThreadedServiceBase()
    {
        Thread = new Thread(ThreadStart)
        {
            Name = ThreadName
        };
    }

    public virtual void Dispose()
    {
        // 1. Signal the thread to stop waiting and exit
        Thread.Interrupt(); 
        
        // 2. Wait for it to finish gracefully
        Thread.Join(ThreadTimeout); 

        // REMOVED: Thread.Abort() - This crashes the game/app in .NET 8
        
        Thread = default;
    }

    #endregion

    #region

    public void Start()
    {
        Thread.Start();
    }

    private void ThreadStart()
    {
        try
        {
            while (true)
            {
                FrameAction();
                Thread.Sleep(ThreadFrameSleep);
            }
        }
        catch (ThreadInterruptedException)
        {
            // This is expected when Dispose() calls Thread.Interrupt()
            // It allows the thread to exit the loop cleanly.
        }
        catch (NullReferenceException)
        {
            // Existing error handling
        }
    }

    protected abstract void FrameAction();

    #endregion
}