namespace Nancy.Demo.Authentication.JwtBearer
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Nancy.Owin;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseOwin(x => x.UseNancy(opt => opt.Bootstrapper = new JwtBearerAuthenticationBootstrapper()));
        }
    }
}
