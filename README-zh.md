[English](./README.md)

# Nancy.Authentication.JwtBearer

![NuGet Version](https://img.shields.io/nuget/v/Nancy.Authentication.JwtBearer.svg)

为Nancy提供JwtBearer授权验证的一个扩展。

这个组件基于.NET Standard 2.0，并且已经可以在NuGet上下载安装了！

<https://www.nuget.org/packages/Nancy.Authentication.JwtBearer/1.1.0>

# 快速上手

## 安装NuGet包

`Install-Package Nancy.Authentication.JwtBearer`

## 创建一个Bootstrapper类

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
```

## 在Module中添加授权验证

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