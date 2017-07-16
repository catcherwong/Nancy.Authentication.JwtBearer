namespace Nancy.Demo.Authentication.JwtBearer.AuthorizedServer
{
    using Nancy.TinyIoc;

    public class AuthorizedServerBootstrapper : Nancy.DefaultNancyBootstrapper
    {
        private readonly IAppConfiguration appConfig;

        public AuthorizedServerBootstrapper()
        {
        }

        public AuthorizedServerBootstrapper(IAppConfiguration appConfig)
        {
            this.appConfig = appConfig;
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register<IAppConfiguration>(appConfig);
        }
    }
}
