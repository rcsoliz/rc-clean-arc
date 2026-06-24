using Application.Interfaces;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotificationAsync(int toUserId, string type, string message, int? postId, CancellationToken cancellationToken = default)
        {
            await _hubContext.Clients
                .Group($"user-{toUserId}")
                .SendAsync("ReceiveNotification", new
                {
                    type,
                    message,
                    postId,
                    createdAt = DateTime.UtcNow
                }, cancellationToken);
        }
    }
}