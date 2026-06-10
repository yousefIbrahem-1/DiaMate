namespace DiaMate.IServices
{
    public interface IUserCleanupService
    {
        Task DeleteExpiredUnconfirmedUsersAsync();
    }
}
