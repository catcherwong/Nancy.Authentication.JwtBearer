namespace Nancy.Authentication.JwtBearer
{
    using Microsoft.IdentityModel.Tokens;
    using Nancy.Bootstrapper;
    using Nancy.Security;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;

    public static class JwtBearerAuthentication
    {
        /// <summary>
        /// Enables JwtBearer authentication for the application
        /// </summary>
        /// <param name="pipelines">Pipelines to add handlers to (usually "this")</param>
        /// <param name="configuration">JwtBearer authentication configuration</param>
        public static void Enable(IPipelines pipelines, JwtBearerAuthenticationConfiguration configuration)
        {
            if (pipelines == null)
            {
                throw new ArgumentNullException("pipelines");
            }

            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            pipelines.BeforeRequest.AddItemToStartOfPipeline(GetLoadAuthenticationHook(configuration));
            pipelines.AfterRequest.AddItemToEndOfPipeline(GetAuthenticationPromptHook(configuration));
        }

        /// <summary>
        /// Enables JwtBearer authentication for a module
        /// </summary>
        /// <param name="module">Module to add handlers to (usually "this")</param>
        /// <param name="configuration">JwtBearer authentication configuration</param>
        public static void Enable(INancyModule module, JwtBearerAuthenticationConfiguration configuration)
        {
            if (module == null)
            {
                throw new ArgumentNullException("module");
            }

            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            module.RequiresAuthentication();
            module.Before.AddItemToStartOfPipeline(GetLoadAuthenticationHook(configuration));
            module.After.AddItemToEndOfPipeline(GetAuthenticationPromptHook(configuration));            
        }

        private static Func<NancyContext, Response> GetLoadAuthenticationHook(JwtBearerAuthenticationConfiguration configuration)
        {
            return context => 
            {
                Validate(context,configuration);
                return null;
            };
        }

        private static void Validate(NancyContext context, JwtBearerAuthenticationConfiguration configuration)
        {            
            //get the token from request header
            var jwtToken = context.Request.Headers["Authorization"].FirstOrDefault() ?? string.Empty;
           
            //whether the token value start with the challenge from configuration
            if (jwtToken.StartsWith(configuration.Challenge))
            {
                jwtToken = jwtToken.Substring(configuration.Challenge.Length + 1);
            }
            else
            {                                
                return;
            }
            
            //verify the token
            if (!string.IsNullOrWhiteSpace(jwtToken))
            {
                try
                {
                    SecurityToken validatedToken;
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validatedClaims = tokenHandler.ValidateToken(jwtToken, configuration.TokenValidationParameters, out validatedToken);
                    //var jwtSecurityToken = validatedToken as JwtSecurityToken;
                    context.CurrentUser = validatedClaims;
                }
                catch (Exception)
                {                                  
                }                                                                       
            }
        }

        private static Action<NancyContext> GetAuthenticationPromptHook(JwtBearerAuthenticationConfiguration configuration)
        {
            return context =>
            {
                if (context.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //add a response header 
                    context.Response.WithHeader(JwtBearerDefaults.WWWAuthenticate, configuration.Challenge);
                }
            };
        }
    }
}