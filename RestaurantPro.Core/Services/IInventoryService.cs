using RestaurantPro.Core.Domain;

namespace RestaurantPro.Core.Services
{
    public interface IInventoryService
    {
        void ConfirmWorkCycle(int workCycleId, User user);
    }
}