namespace Nancy.Demo.Authentication.JwtBearer
{
    using Microsoft.IdentityModel.Tokens;
    using Nancy.Authentication.JwtBearer;
    using Nancy.Bootstrapper;
    using Nancy.TinyIoc;
    using System;
    using System.Text;

    public class JwtBearerAuthenticationBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

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

            var configuration = new JwtBearerAuthenticationConfiguration
            {
                TokenValidationParameters = tokenValidationParameters,
                Challenge = "Guest"//if not use this,default to Bearer
            };

            pipelines.EnableJwtBearerAuthentication(configuration);
        }
    }
}
