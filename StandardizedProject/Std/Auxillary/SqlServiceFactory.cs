using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StandardizedProject.Std.Interface;
[jaljdlkajflad]
namespace StandardizedProject.Std.Auxillary {
    public class SqlServiceFactory<IService> : IServiceFactory<IService> {
        Dictionary<Type, IService> serviceCatch_;
        public string FindSelf() {
            return GetType().Assembly.Location;
        }
        public SqlServiceFactory() {
            serviceCatch_ = new Dictionary<Type, IService>();
        }
        public IEnumerable<string> ServiceList() {
            Assembly self = GetType().Assembly;
            return self.ExportedTypes.Select(p => p.FullName);
        }
        public Service CreateService<Service>() where Service : IService {
            Type type = typeof(Service);            
            if (serviceCatch_.ContainsKey(type)) { return (Service)serviceCatch_[type]; }
            dynamic service = GetType().Assembly.CreateInstance(type.FullName);
            serviceCatch_.Add(type, service);
            return service;
        }
    }
}
