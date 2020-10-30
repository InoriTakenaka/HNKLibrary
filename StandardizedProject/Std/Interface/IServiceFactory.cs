using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardizedProject.Std.Interface {
    public interface IServiceFactory<IService>{
        IEnumerable<string> ServiceList();
        Service CreateService<Service>() where Service : IService;
    }
}
