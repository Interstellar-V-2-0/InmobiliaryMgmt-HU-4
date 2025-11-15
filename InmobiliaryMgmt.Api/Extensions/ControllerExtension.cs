using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace InmobiliaryMgmt.Api.Extensions
{
    public static class ControllerExtensions
    {
        public static int GetUserId(this ControllerBase controller)
        {
            var claimValue = controller.User?.Claims?
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(claimValue))
                throw new UnauthorizedAccessException("The user Id was not found in the token.");

            if (!int.TryParse(claimValue, out int userId))
                throw new UnauthorizedAccessException("The user id in the token is invalid or does not exist any numeric digit");

            return userId;
        }
    }
}