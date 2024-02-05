namespace Claims.Auditing.Abstractions
{
    public interface IBackgroundTaskQueue
    {
        ValueTask Queue(Func<ValueTask> workItem);

        ValueTask<Func<ValueTask>> DequeueAsync();
    }
}
