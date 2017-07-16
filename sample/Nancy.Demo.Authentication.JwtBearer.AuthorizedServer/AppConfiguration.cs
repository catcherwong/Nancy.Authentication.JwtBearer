namespace Nancy.Demo.Authentication.JwtBearer.AuthorizedServer
{
    public class AppConfiguration : IAppConfiguration
    {
        public Audience Audience { get; set; }
    }

    public interface IAppConfiguration
    {
        Audience Audience { get; }
    }

}