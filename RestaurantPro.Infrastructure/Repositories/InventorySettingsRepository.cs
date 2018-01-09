using System.Data.Entity;
using System.Linq;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class InventorySettingsRepository : Repository<InventorySettings>, IInventorySettingsRepository
    {
        private readonly RestProContext context;


        public InventorySettingsRepository(DbContext context) 
            : base(context)
        {
            this.context = (RestProContext) context;
        }

        public void SetTax(decimal tax)
        {
            var taxInDb = context.InventorySettings.SingleOrDefault(x => x.Parameter == "Tax");

            if (taxInDb != null) 
                taxInDb.Value = tax;

            context.SaveChanges();
        }

        public decimal? GetTax()
        {
            var taxInDb = context.InventorySettings.SingleOrDefault(x => x.Parameter == "Tax");

            return taxInDb == null ? null : taxInDb.Value;
        }
    }
}