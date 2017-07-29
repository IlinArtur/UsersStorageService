using Microsoft.Owin.Hosting;
using System;
using System.ServiceModel;
using UserStorageService.Read;

namespace UserStorageService.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = "http://localhost:51488";

            using (WebApp.Start<WebConfig>(host))
            {
                using (var readService = new ServiceHost(new UserInfoProvider(new LiteDbUserInfoDao(@"C:\Profiles.db")), new Uri[0]))
                {
                    readService.Open();
                    Console.ReadKey();
                    readService.Close();
                }
            }
        }
    }
}
