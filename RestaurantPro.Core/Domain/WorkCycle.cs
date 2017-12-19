using System;

namespace RestaurantPro.Core.Domain
{
    public class WorkCycle
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateBegin { get; set; }

        public DateTime DateEnd { get; set; }

        public bool Active { get; set; }

        public virtual  User User { get; set; }
    }
}