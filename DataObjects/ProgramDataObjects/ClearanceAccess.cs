using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.ProgramDataObjects
{
    public enum UserFeatures
    {
        TIMEENTRYAPP,
        EMPLOYEECENTRAL,
        ADMINCENTRAL,
        SCHRM,
        SCANNOUNCEMENTS
    }

    public class ClearanceAccess : ClearanceLevel
    {
        public int FeatureID { get; set; }

        public string FeatureName { get; set; }

        public bool IsBlocked { get; set; }

        public bool hasAccess { get; set; }

        public bool isEditable { get; set; }

        public bool isBlocked { get; set; } //for clearance access overrides

        public string UserRolesID { get; set; }

        

    }
}
