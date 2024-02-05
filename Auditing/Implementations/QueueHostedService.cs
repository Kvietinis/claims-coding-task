using Claims.Auditing.Abstractions;
using EnsureThat;
using Microsoft.Extensions.Hosting;

namespace Claims.Auditing.Implementations
{
    public class QueueHostedService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;

        public QueueHostedService(IBackgroundTaskQueue taskQueue)
        {
            Ensure.That(taskQueue, nameof(taskQueue)).IsNotNull();

            _taskQueue = taskQueue;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var result = DoWork(stoppingToken);

            return result;
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var workItem = await _taskQueue.DequeueAsync().ConfigureAwait(false);

                    await workItem().ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    // Prevent throwing if stoppingToken was signaled
                }
            }
        }
    }
}
