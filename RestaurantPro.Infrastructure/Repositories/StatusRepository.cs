using System;
using System.Data.Entity;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class StatusRepository : Repository<PoStatus>, IStatusRepository
    {
        public StatusRepository(DbContext context) 
            : base(context)
        {
        }
        

    }


}