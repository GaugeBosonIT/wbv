using System.Web.Mvc;
using Ninject;
using System.Collections.Generic;
using Ninject.Parameters;

namespace WBV
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {

            _kernel = kernel;
        }


        public object GetService(System.Type serviceType)
        {
            return _kernel.TryGet(serviceType, new IParameter[0]);
        }

        public IEnumerable<object> GetServices(System.Type serviceType)
        {
            return _kernel.GetAll(serviceType, new IParameter[0]);
        }
    }
}