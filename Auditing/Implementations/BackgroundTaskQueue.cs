using Claims.Auditing.Abstractions;
using EnsureThat;
using System.Threading.Channels;

namespace Claims.Auditing.Implementations
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Func<ValueTask>> _queue;

        public BackgroundTaskQueue()
        {
            var options = new BoundedChannelOptions(100)
            {
                FullMode = BoundedChannelFullMode.Wait
            };

            _queue = Channel.CreateBounded<Func<ValueTask>>(options);
        }

        public async ValueTask<Func<ValueTask>> DequeueAsync()
        {
            var workItem = await _queue.Reader.ReadAsync().ConfigureAwait(false);

            return workItem;
        }

        public async ValueTask Queue(Func<ValueTask> workItem)
        {
            Ensure.That(workItem).IsNotNull();

            await _queue.Writer.WriteAsync(workItem).ConfigureAwait(false);
        }
    }
}
