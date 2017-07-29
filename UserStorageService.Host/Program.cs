using Autofac;
using System;
using System.Collections.Generic;
using UserStorageService.Host.Services;

namespace UserStorageService.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<RegistrationModule>();
            using (var container = builder.Build())
            {
                var services = container.Resolve<IEnumerable<IService>>();
                DoWithServices(services, x => x.Start());
                Console.WriteLine("Services started!");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                DoWithServices(services, x => x.Stop());
            }
        }

        private static void DoWithServices(IEnumerable<IService> services, Action<IService> execute)
        {
            foreach (var service in services)
            {
                execute(service);
            }
        }
    }
}
