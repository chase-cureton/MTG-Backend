using MTGLambda.MTGLambda.Services.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTGLambda.MTGLambda.Services
{
    public class ServiceFactory
    {
        public static IService GetService(Type t)
        {
            IService service = (IService)Activator.CreateInstance(t);
            service.Init(new ServiceContext());
            return service;
        }

        public static T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }
    }
}
