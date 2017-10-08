[English](./README.md)

# Nancy.Authentication.JwtBearer

![NuGet Version](https://img.shields.io/nuget/v/Nancy.Authentication.JwtBearer.svg)

[![Build status](https://ci.appveyor.com/api/projects/status/6jeqlrrjh8f5enjy?svg=true)](https://ci.appveyor.com/project/catcherwong/nancy-authentication-jwtbearer)

为Nancy提供JwtBearer授权验证的一个扩展。

这个组件基于.NET Standard 2.0，并且已经可以在NuGet上下载安装了！

NuGet上最新的版本已经同时支持.NET Standard 2.0 和 .NET Framework 4.5.2了。

您可以通过下面的链接访问这个Package相关的信息：

<https://www.nuget.org/packages/Nancy.Authentication.JwtBearer>

# 快速上手

## 安装NuGet包

```
Install-Package Nancy.Authentication.JwtBearer
```

## 在Bootstrapper类中配置JwtBearer相关信息

在这一步主要是配置启用JwtBearer验证，需要实例化一个**JwtBearerAuthenticationConfiguration**对象，这个对象包括了JwtBearer验证的相关配置信息。

最后需要调用IPipelines的扩展方法去启用JwtBearer。

相关配置参数说明 

参数名 | 说明
---|---
TokenValidationParameters  | Token验证所需要的参数
Challenge  |  HTTP头部WWW-Authenticate字段的配置

示例代码如下：

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

只需要在Module中添加`this.RequiresAuthentication();`即可。

示例代码如下：

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