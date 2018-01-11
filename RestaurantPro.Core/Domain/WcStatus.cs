using System.Collections;
using System.Collections.Generic;

namespace RestaurantPro.Core.Domain
{
    public class WcStatus
    {
        public string Status { get; set; }

        public ICollection<WorkCycle> WorkCycle { get; set; }
    }
}