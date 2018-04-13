using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace NbPilot.Common.Dependencies
{
    public class DependencyResolver
    {
        public static void SetResolver(IDependencyResolver resolver)
        {
            _instance.InnerSetResolver(resolver);
        }
        private Func<IDependencyResolver> _resolverThunk;
        private object Create(Type type)
        {
            return _resolverThunk().GetService(type) ?? Activator.CreateInstance(type);
        }

    }

    internal class DefaultDependencyResolver : IDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            // Since attempting to create an instance of an interface or an abstract type results in an exception, immediately return null
            // to improve performance and the debugging experience with first-chance exceptions enabled.
            if (serviceType.IsInterface || serviceType.IsAbstract)
            {
                return null;
            }

            try
            {
                return Activator.CreateInstance(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Enumerable.Empty<object>();
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
            
        }
    }



    //internal class DelegateBasedDependencyResolver
    //{
    //    private Func<Type, object> _getService;
    //    private Func<Type, IEnumerable<object>> _getServices;

    //    public DelegateBasedDependencyResolver(Func<Type, object> getService, Func<Type, IEnumerable<object>> getServices)
    //    {
    //        _getService = getService;
    //        _getServices = getServices;
    //    }

    //    public object GetService(Type type)
    //    {
    //        try
    //        {
    //            return _getService.Invoke(type);
    //        }
    //        catch
    //        {
    //            return null;
    //        }
    //    }

    //    public IEnumerable<object> GetServices(Type type)
    //    {
    //        return _getServices(type);
    //    }
    //}
    //public class DefaultDependencyResolver : IDependencyResolver
    //{
    //    public void Dispose()
    //    {

    //    }

    //    public object GetService(Type serviceType)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IEnumerable<object> GetServices(Type serviceType)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IDependencyScope BeginScope()
    //    {
    //        return this;
    //    }
    //}
}
