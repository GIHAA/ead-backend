/*
 * File: CustomUserIdProvider.cs
 * Project: TechFixBackend.Services
 * Description: Custom implementation of the `IUserIdProvider` interface for SignalR.
 *              This class provides the logic to extract the user ID from the `NameIdentifier` claim 
 *              in a JWT during a SignalR connection, allowing the framework to identify users.
 */


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
