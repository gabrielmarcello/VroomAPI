using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VroomAPI.Authentication
{
    public class ApiKeyAuthFilter : IAuthorizationFilter
    {

        private readonly IConfiguration _configuration;

        public ApiKeyAuthFilter(IConfiguration config)
        {
            _configuration = config;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("API Key não encontrada");
                return;
            }

            var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);
            if (!apiKey.Equals(extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("API Key inválida");
                return;
            }
        }
    }
}
