namespace Nancy.Demo.Authentication.JwtBearer.AuthorizedServer.Modules
{
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;

    public class TokenModule : Nancy.NancyModule
    {
        public TokenModule(IAppConfiguration appConfig) : base("/api/token")
        {
            Get("/auth", _ =>
            {
                Parameters parameters = new Parameters
                {
                    client_id = this.Context.Request.Query["client_id"],
                    client_secret = this.Context.Request.Query["client_secret"],
                    grant_type = this.Context.Request.Query["grant_type"],
                    password = this.Context.Request.Query["password"],
                    username = this.Context.Request.Query["username"]
                };

                if (!CheckParameters(parameters))
                {
                    return Response.AsJson(new ResponseData
                    {
                        Code = "901",
                        Message = "null of parameters",
                        Data = null
                    });
                }

                if (parameters.grant_type == "password")
                {
                    return Response.AsJson(DoPassword(parameters, appConfig));
                }
                else
                {
                    return Response.AsJson(new ResponseData
                    {
                        Code = "904",
                        Message = "bad request",
                        Data = null
                    });
                }
            });
        }

        private bool CheckParameters(Parameters param)
        {
            return !string.IsNullOrWhiteSpace(param.client_id)
                && !string.IsNullOrWhiteSpace(param.client_secret)
                && !string.IsNullOrWhiteSpace(param.grant_type);
        }

        private ResponseData DoPassword(Parameters parameters, IAppConfiguration appConfig)
        {
            //validate the client_id/client_secret/username/password                                          
            var isValidated = UserInfo.GetAllUsers().Any(x => x.ClientId == parameters.client_id
                                    && x.ClientSecret == parameters.client_secret
                                    && x.UserName == parameters.username
                                    && x.Password == parameters.password);

            if (!isValidated)
            {
                return new ResponseData
                {
                    Code = "902",
                    Message = "invalid user infomation",
                    Data = null
                };
            }

            return new ResponseData
            {
                Code = "999",
                Message = "OK",
                Data = GetJwt(parameters.client_id,appConfig)
            };
        }

        private string GetJwt(string client_id, IAppConfiguration appConfig)
        {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                        new Claim(JwtRegisteredClaimNames.Sub, client_id),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
            };

            var symmetricKeyAsBase64 = appConfig.Audience.Secret;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var jwt = new JwtSecurityToken(
                issuer: appConfig.Audience.Iss,
                audience: appConfig.Audience.Aud,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)TimeSpan.FromMinutes(2).TotalSeconds
            };

            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented });
        }
    }
}
