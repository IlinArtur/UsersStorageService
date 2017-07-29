using Microsoft.Owin.Hosting;
using System;

namespace UserStorageService.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = "http://localhost:51488";

            using (WebApp.Start<WebConfig>(host))
            {
                Console.ReadKey();
            }
        }
    }
}
