using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.Repositories
{
    public class WorkCycleRepository : Repository<WorkCycle>, IWorkCycleRepository
    {
        private readonly RestProContext _context;

        public WorkCycleRepository(DbContext context) 
            : base(context)
        {
            _context = (RestProContext) context;
        }

        public void DeactivateWorkCycle(int id)
        {
            var a = SingleOrDefault(x => x.Id == id);
            a.Active = false;
            _context.SaveChanges();
        }

        public void UpdateWorkCycle(WorkCycle workCycle)
        {
            var workCycleInDb = SingleOrDefault(x => x.Id == workCycle.Id);

            workCycleInDb.Name = workCycle.Name;
            workCycleInDb.DateBegin = workCycle.DateBegin;
            workCycleInDb.DateEnd = workCycle.DateEnd;
            workCycleInDb.Active = workCycle.Active;
            workCycleInDb.UserId = workCycle.UserId;

            _context.SaveChanges();
        }

        /// <summary>
        /// Adds Working Cycles with lines
        /// </summary>
        /// <param name="workCycle"></param>
        public void AddWorkingCyle(WorkCycle workCycle)
        {
            _context.WorkCycles.Add(workCycle);
            _context.SaveChanges();

            var workCycleInDb = _context.WorkCycles
                .SingleOrDefault(wc => wc.Name == workCycle.Name);

            if (workCycleInDb != null)
            {
                workCycle.Lines
                    .ToList()
                    .ForEach(w => w.WorkCycleId = workCycleInDb.Id);
            }

            workCycle.Lines.ToList().ForEach(w => _context.WorkCycleLines.Add(w));

            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                //Remove Working Cycle -- Reversal
                if (workCycleInDb != null)
                {
                    _context.WorkCycles.Remove(workCycleInDb);
                    _context.SaveChanges();
                }

                throw e;
            }
        }

        /// <summary>
        /// Retrieve Work Cycles by Work Cycle Name
        /// </summary>
        /// <param name="workCycleName"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public WorkCycle GetWorkCycleByWorkCycleName(string workCycleName, bool isActive)
        {
            return _context.WorkCycles
                .Include(po => po.WorkCycleLines)
                .Where(c => c.Active == isActive)
                .SingleOrDefault(c => c.Name == workCycleName);
        }

        /// <summary>
        /// Retrieve Work Cycles by Work Cycle Id
        /// </summary>
        /// <param name="workCycleId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public WorkCycle GetWorkCycleById(int workCycleId, bool isActive)
        {
            return _context.WorkCycles
                .Include(wc => wc.WorkCycleLines)
                .Where(w => w.Active == isActive)
                .SingleOrDefault(c => c.Id == workCycleId);
        }


    }
}