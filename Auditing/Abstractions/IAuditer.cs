namespace Claims.Auditing.Abstractions
{
    public interface IAuditer
    {
        Task AuditClaim(string id, string httpRequestType);

        Task AuditCover(string id, string httpRequestType);
    }
}
