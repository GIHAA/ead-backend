using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

public class CustomUserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        // Extract the user ID from the NameIdentifier claim in the JWT
        var userId = connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Optionally, log or handle cases where userId is null
        if (userId == null)
        {
            // Handle the case where there is no valid user ID
            Console.WriteLine("UserIdentifier is null.");
        }

        return userId;
    }
}
