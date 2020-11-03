using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardizedProject.Std.Auxillary {
    [System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class EntityTagAttribute : Attribute {
        private string displayName_;

        public EntityTagAttribute(string displayName) {
            this.displayName_ = displayName;
        }

        /// <summary>
        /// Entity/Property's Name in Database
        /// </summary>
        public string DisplayName {
            get { return displayName_; }
        }

        /// <summary>
        /// Property is Primary Key in Database
        /// </summary>
        public bool PK { get; set; }
    }
}
