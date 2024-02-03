namespace Claims.Persistance.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IClaimsRepository GetClaims();

        ICoversRepository GetCovers();

        Task Commit();

        Task Reject();
    }
}
