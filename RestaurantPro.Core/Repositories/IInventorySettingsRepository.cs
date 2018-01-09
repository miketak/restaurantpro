using RestaurantPro.Core.Domain;

namespace RestaurantPro.Core.Repositories
{
    public interface IInventorySettingsRepository : IRepository<InventorySettings>
    {
        void SetTax(decimal tax);

        decimal? GetTax();

    }
}