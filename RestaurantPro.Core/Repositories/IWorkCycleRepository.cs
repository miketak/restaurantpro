using RestaurantPro.Core.Domain;

namespace RestaurantPro.Core.Repositories
{
    public interface IWorkCycleRepository : IRepository<WorkCycle>
    {
        void DeactivateWorkCycle(int id);
    }
}