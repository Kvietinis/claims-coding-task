using Claims.Auditing.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claims.Auditing.Implementations
{
    public class QueueAuditer : IAuditer
    {
        //ConcurrentQueue<T> Class
        public Task AuditClaim(string id, string httpRequestType)
        {
            throw new NotImplementedException();
        }

        public Task AuditCover(string id, string httpRequestType)
        {
            throw new NotImplementedException();
        }
    }
}
