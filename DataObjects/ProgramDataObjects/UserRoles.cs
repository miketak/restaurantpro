using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class UserRoles
    {
        public string UserRolesId { get; set; }


        /// <summary>
        /// Job Position
        /// </summary>
        public string Name { get; set; }

        public string Description { get; set; }

        public string DepartmentId { get; set; }
    }
}
