namespace Application.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(int toUserId, string type, string message, int? postId, CancellationToken cancellationToken = default);

    }
}
