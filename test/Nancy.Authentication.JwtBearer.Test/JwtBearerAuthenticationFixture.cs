namespace Nancy.Authentication.JwtBearer.Test
{
    using Microsoft.IdentityModel.Tokens;
    using Nancy.Bootstrapper;
    using System;
    using System.Text;

    public class JwtBearerAuthenticationFixture
    {
        private readonly JwtBearerAuthenticationConfiguration config;
        private readonly IPipelines hooks;

        public JwtBearerAuthenticationFixture()
        {
            var keyByteArray = Encoding.ASCII.GetBytes("Y2F0Y2hlciUyMHdvbmclMjBsb3ZlJTIwLm5ldA==");
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = "http://www.c-sharpcorner.com/members/catcher-wong",

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = "Catcher Wong",

                // Validate the token expiry
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            this.config = new JwtBearerAuthenticationConfiguration()
            {
                TokenValidationParameters = tokenValidationParameters
            };
            this.hooks = new Pipelines();
            JwtBearerAuthentication.Enable(this.hooks, this.config);
        }
    
        public void Test()
        {            
        }
    }
}
