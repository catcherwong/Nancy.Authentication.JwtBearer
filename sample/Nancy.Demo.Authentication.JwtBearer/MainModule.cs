namespace Nancy.Demo.Authentication.JwtBearer
{
    using Nancy.Security;
    using System.Text;

    public class MainModule : Nancy.NancyModule
    {
        public MainModule()
        {
            this.RequiresAuthentication();

            Get("/", _ => 
            {
                StringBuilder sb = new StringBuilder(1024);

                foreach (var item in this.Context.CurrentUser.Claims)
                {
                    sb.Append(item.Type + ":" + item.Value);
                    sb.AppendLine();
                }

                return sb.ToString();
            });
        }

    }
}
