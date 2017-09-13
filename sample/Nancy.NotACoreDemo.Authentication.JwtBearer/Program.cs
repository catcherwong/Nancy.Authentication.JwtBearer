namespace Nancy.NotACoreDemo.Authentication.JwtBearer
{
    using Nancy.Hosting.Self;
    using System;
    using System.Diagnostics;

    class Program
    {
        static void Main(string[] args)
        {
            using (var nancyHost = new NancyHost(new Uri("http://localhost:8888/nancy/")))
            {
                nancyHost.Start();

                Console.WriteLine("Nancy now listening - navigating to http://localhost:8888/nancy/. Press enter to stop");
                try
                {
                    Process.Start("http://localhost:8888/nancy/");
                }
                catch (Exception)
                {
                }
                Console.ReadKey();
            }

            Console.WriteLine("Stopped. Good bye!");
        }
    }
}
