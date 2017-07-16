namespace Nancy.Authentication.JwtBearer
{
    using Nancy.Bootstrapper;

    public static class JwtBearerExtensions
    {
        /// <summary>
        /// Module requires JwtBearer authentication
        /// </summary>
        /// <param name="module">Module to enable</param>
        /// <param name="configuration">JwtBearer authentication configuration</param>
        public static void EnableJwtBearerAuthentication(this INancyModule module, JwtBearerAuthenticationConfiguration configuration)
        {
            JwtBearerAuthentication.Enable(module, configuration);
        }

        /// <summary>
        /// Module requires JwtBearer authentication
        /// </summary>
        /// <param name="pipeline">Bootstrapper to enable</param>
        /// <param name="configuration">JwtBearer authentication configuration</param>
        public static void EnableJwtBearerAuthentication(this IPipelines pipeline, JwtBearerAuthenticationConfiguration configuration)
        {
            JwtBearerAuthentication.Enable(pipeline, configuration);
        }
    }
}
