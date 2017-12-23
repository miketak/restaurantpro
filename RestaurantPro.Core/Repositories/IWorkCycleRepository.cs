using RestaurantPro.Core.Domain;

namespace RestaurantPro.Core.Repositories
{
    public interface IWorkCycleRepository : IRepository<WorkCycle>
    {
        void DeactivateWorkCycle(int id);
        void UpdateWorkCycle(WorkCycle workCycle);
        void AddWorkingCyle(WorkCycle workCycle);
        WorkCycle GetWorkCycleByWorkCycleName(string workCycleName, bool isActive);
        WorkCycle GetWorkCycleById(int workCycleId, bool isActive);


    }
}