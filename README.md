[中文版](./README-zh.md)

# Nancy.Authentication.JwtBearer

![NuGet Version](https://img.shields.io/nuget/v/Nancy.Authentication.JwtBearer.svg)

[![Build status](https://ci.appveyor.com/api/projects/status/6jeqlrrjh8f5enjy?svg=true)](https://ci.appveyor.com/project/catcherwong/nancy-authentication-jwtbearer)

Nancy.Authentication.JwtBearer is a JwtBearer authentication provider for Nancy.

The newest version in NuGet support .NET Standard 2.0 and .NET Framework 4.5.2.

Visit this package in nuget :

<https://www.nuget.org/packages/Nancy.Authentication.JwtBearer>

# Quick Start

## Instatll the package at first

```
Install-Package Nancy.Authentication.JwtBearer
```

## Configure JwtBearer In Bootstrapper Class

```csharp
public class Bootstrapper : Nancy.DefaultNancyBootstrapper
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
            ValidIssuer = "https://github.com/hwqdt/",

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
```

## Add RequiresAuthentication in your modules

```csharp
public class MainModule : Nancy.NancyModule
{
    public MainModule()
    {
        this.RequiresAuthentication();

        Get("/", _ => 
        {
            return "From JwtBearer Authentication";
        });
    }
}
```
