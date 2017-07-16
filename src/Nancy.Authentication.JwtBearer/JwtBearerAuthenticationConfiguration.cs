namespace Nancy.Authentication.JwtBearer
{
    using Microsoft.IdentityModel.Tokens;

    public class JwtBearerAuthenticationConfiguration
    {
        /// <summary>
        /// Gets or sets the parameters used to validate identity tokens.
        /// </summary>
        public TokenValidationParameters TokenValidationParameters { get; set; } = new TokenValidationParameters();

        /// <summary>
        /// Gets or sets the challenge to put in the "WWW-Authenticate" header.
        /// </summary>
        public string Challenge { get; set; } = JwtBearerDefaults.Scheme;
    }
}