using Claims.Auditing.Abstractions;
using EnsureThat;

namespace Claims.Auditing.Implementations
{
    public class AuditerQueueDecorator : IAuditer
    {
        private readonly IAuditer _auditer;
        private readonly IBackgroundTaskQueue _taskQueue;

        public AuditerQueueDecorator(IAuditer auditer, IBackgroundTaskQueue taskQueue)
        {
            Ensure.That(auditer, nameof(auditer)).IsNotNull();
            Ensure.That(taskQueue, nameof(taskQueue)).IsNotNull();

            _auditer = auditer;
            _taskQueue = taskQueue;
        }

        public async Task AuditClaim(string id, string httpRequestType)
        {
            await _taskQueue.Queue(async () =>
            {
                await _auditer.AuditClaim(id, httpRequestType).ConfigureAwait(false);
            })
            .ConfigureAwait(false);
        }

        public async Task AuditCover(string id, string httpRequestType)
        {
            await _taskQueue.Queue(async () =>
            {
                await _auditer.AuditCover(id, httpRequestType).ConfigureAwait(false);
            })
            .ConfigureAwait(false);
        }
    }
}
