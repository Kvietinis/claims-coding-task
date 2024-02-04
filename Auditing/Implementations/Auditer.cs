using Claims.Auditing.Abstractions;

namespace Claims.Auditing.Implementations
{
    public class Auditer : IAuditer
    {
        public Auditer()
        {
            
        }

        public void AuditClaim(string id, string httpRequestType)
        {
            throw new NotImplementedException();
        }

        public void AuditCover(string id, string httpRequestType)
        {
            throw new NotImplementedException();
        }
    }
}
