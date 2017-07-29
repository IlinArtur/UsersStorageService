using Autofac;
using Autofac.Integration.Wcf;
using System;
using System.ServiceModel;
using UserStorageService.Read;

namespace UserStorageService.Host
{
    public class ReadService : IService, IDisposable
    {
        private readonly ILifetimeScope context;
        private readonly Uri address;
        private ServiceHost host;

        public ReadService(ILifetimeScope context, Uri address)
        {
            this.context = context;
            this.address = address;
        }

        public void Dispose()
        {
            host?.Close();
        }

        public void Start()
        {
            host = new ServiceHost(typeof(UserInfoProvider), address);
            host.AddDependencyInjectionBehavior<IUserInfoProvider>(context);
            host.Open();
        }

        public void Stop()
        {
            host?.Close();
        }
    }
}
