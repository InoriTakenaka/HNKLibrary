using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardizedProject.Std.Interface {
    public abstract class Entity {
        private bool logicDelete_;
        public virtual string Serialized() {
            return string.Empty;
        }
        public virtual Entity Deserialized(string source) {
            return null;
        }
        public void SetLogicDelete(bool flag) {
            logicDelete_ = flag;
        }
    }
}
