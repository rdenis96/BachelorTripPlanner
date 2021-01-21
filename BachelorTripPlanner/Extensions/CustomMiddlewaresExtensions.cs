using Microsoft.AspNetCore.Builder;

namespace BachelorTripPlanner.Extensions
{
    public static class CustomMiddlewaresExtensions
    {
        public static IApplicationBuilder UseUserValidationMiddleware(
            this IApplicationBuilder builder)
        {
            return builder;
        }
    }
}