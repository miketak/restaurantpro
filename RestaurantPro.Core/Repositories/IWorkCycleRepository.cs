using System.Collections.Generic;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Core.Repositories
{
    public interface IWorkCycleRepository : IRepository<WorkCycle>
    {
        IEnumerable<WorkCycle> GetWorkCycles();
        void DeactivateWorkCycle(int id);
        void UpdateWorkCycle(WorkCycle workCycle);
        void AddWorkingCycle(WorkCycle workCycle);
        WorkCycle GetWorkCycleByWorkCycleName(string workCycleName, bool isActive);
        WorkCycle GetWorkCycleById(int workCycleId, bool isActive);
        bool CheckForActiveWorkCycles();


    }
}