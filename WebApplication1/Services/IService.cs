using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Services
{
    public interface IService
    {
        Guid getScoped();
        Guid getSingletone();
        Guid getTransient();
        void MakeWork();
    }

    public class ServiceA: IService
    {
       private readonly ILogger<ServiceA> logger;
        private readonly TransientService transientService;
        private readonly ScopedService scopedService;
        private readonly SingletonService singletonService;

        public ServiceA(ILogger<ServiceA> logger, TransientService transientService, ScopedService scopedService, SingletonService singletonService) 
        {
            this.logger = logger;
            this.transientService = transientService;
            this.scopedService = scopedService;
            this.singletonService = singletonService;
        }

        public Guid getTransient() { return transientService.Guid; }
        public Guid getScoped() { return scopedService.Guid; }
        public Guid getSingletone() { return singletonService.Guid; }

        public void MakeWork() 
        {
           
        }
    }

    public class ServiceB : IService
    {
        public Guid getScoped()
        {
            throw new NotImplementedException();
        }

        public Guid getSingletone()
        {
            throw new NotImplementedException();
        }

        public Guid getTransient()
        {
            throw new NotImplementedException();
        }

        public void MakeWork()
        {
            
        }
    }

    public class TransientService
    {
        public Guid Guid = Guid.NewGuid();
    }

    public class ScopedService
    {
        public Guid Guid = Guid.NewGuid();
    }

    public class SingletonService
    {
        public Guid Guid = Guid.NewGuid();
    }
}
