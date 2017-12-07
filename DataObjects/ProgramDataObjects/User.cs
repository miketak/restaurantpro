using DataObjects.ProgramDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class User
    {

        public int UserId { get; set; }
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string OtherNames { get; set; }

        public string DepartmentId { get; set; }

        public string Department { get; set; }

        public string PasswordHash { get; set; }

        public string SSNo { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string PicUrl { get; set; }

        public bool isEmployed { get; set; }

        public bool isBlocked { get; set; }

        public string UserRolesId { get; set; }

        public string JobDesignation { get; set; }

        public int ClearanceLevelId { get; set; }

        public string ClearanceLevel { get; set; }

        public List<ClearanceAccess> ClearanceAccess { get; set; }

    }
}
