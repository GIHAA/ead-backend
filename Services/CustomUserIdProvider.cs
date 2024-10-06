using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace TechFixBackend.Services
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            // Extract the user ID from the NameIdentifier claim
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
