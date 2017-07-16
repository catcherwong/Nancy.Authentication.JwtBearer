namespace Nancy.Authentication.JwtBearer
{
    public static class JwtBearerDefaults
    {
        /// <summary>
        /// Default value for AuthenticationScheme property in the JwtBearerAuthenticationConfiguration
        /// </summary>
        public const string Scheme = "Bearer";

        /// <summary>
        /// Http header
        /// </summary>
        public const string WWWAuthenticate = "WWW-Authenticate";
    }
}