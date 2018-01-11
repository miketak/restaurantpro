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
        private RestProContext _context;

        public WorkCycleRepository(DbContext context)
            : base(context)
        {
            _context = (RestProContext) context;
        }

        /// <summary>
        /// Deactivates work cycle active flag
        /// </summary>
        /// <param name="id">Work Cycle Id</param>
        public void DeactivateWorkCycle(int id)
        {
            var a = SingleOrDefault(x => x.Id == id);

            if ( a == null)
                throw new ApplicationException("Database Error: Item not found");

            a.Active = false;
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates Work Cycle and associated lines
        /// </summary>
        /// <param name="workCycle">Work Cycle to be edited</param>
        public void UpdateWorkCycle(WorkCycle workCycle)
        {
            var workCycleInDb = SingleOrDefault(x => x.Id == workCycle.Id);

            workCycleInDb.Name = workCycle.Name;
            workCycleInDb.DateBegin = workCycle.DateBegin;
            workCycleInDb.DateEnd = workCycle.DateEnd;
            workCycleInDb.Active = workCycle.Active;
            workCycleInDb.UserId = workCycle.UserId;
            workCycleInDb.StatusId = workCycle.StatusId;
            _context.SaveChanges();

            AddOrUpdateWorkingCycleLines(workCycle);
        }

        /// <summary>
        ///  Adds Working Cycles with lines
        /// </summary>
        /// <param name="workCycle">Work Cycle to be added</param>
        public void AddWorkingCycle(WorkCycle workCycle)
        {
            _context.WorkCycles.Add(workCycle);
            _context.SaveChanges();

            var workCycleInDb = _context.WorkCycles
                .SingleOrDefault(wc => wc.Name == workCycle.Name);

            try
            {
                AddOrUpdateWorkingCycleLines(workCycle);
            }
            catch (Exception e)
            {
                if (workCycleInDb != null)
                {
                    _context.WorkCycles.Remove(workCycleInDb);
                    _context.SaveChanges();
                }

                throw new ApplicationException("Database Error: " + e.Message);
            }
        }

        /// <summary>
        ///     Retrieve Work Cycles by Work Cycle Name with lines
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
        ///     Retrieve Work Cycles by Work Cycle Id with lines
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


        #region Private Helper Methods

        private void AddOrUpdateWorkingCycleLines(WorkCycle workCycle)
        {
            if (!workCycle.Lines.Any())
                return;

            FlushWorkCycleLines(workCycle);

            foreach (var line in workCycle.Lines)
            {
                line.WorkCycleId = workCycle.Id;
                bool isOldRawMaterialUsed = false;


                if (line.RawMaterialId == 0)
                {
                    line.RawMaterialId = CheckForOldRawMaterialsToActivate(line);
                    if (line.RawMaterialId != 0)
                        isOldRawMaterialUsed = true;
                }

                if (line.RawMaterialId == 0 && !isOldRawMaterialUsed)
                    line.RawMaterialId = AddNewRawMaterialToRawMaterialTable(line);

                if (line.SupplierId == 0)
                    line.SupplierId = AddNewSupplierToSupplierTable(line);

                _context.WorkCycleLines.Add(line);

                _context.SaveChanges();
            }
        }

        private int CheckForOldRawMaterialsToActivate(WorkCycleLines line)
        {
            var rawMaterialInDb = _context.RawMaterials.SingleOrDefault(r => r.Name == line.RawMaterialStringTemp);

            if (rawMaterialInDb == null)
                return 0;

            rawMaterialInDb.Active = true;
            _context.SaveChanges();

            return rawMaterialInDb.Id;
        }

        private int AddNewSupplierToSupplierTable(WorkCycleLines line)
        {
            _context.Suppliers.Add(new Supplier { Name = line.SupplierStringTemp, Active = true });
            _context.SaveChanges();
            return _context.Suppliers.SingleOrDefault(s => s.Name == line.SupplierStringTemp).Id;
        }

        private int AddNewRawMaterialToRawMaterialTable(WorkCycleLines line)
        {
            _context.RawMaterials.Add(new RawMaterial { Name = line.RawMaterialStringTemp, RawMaterialCategoryId = 1, Active=true });
            _context.SaveChanges();
            return _context.RawMaterials.SingleOrDefault(r => r.Name == line.RawMaterialStringTemp).Id;
        }

        private void FlushWorkCycleLines(WorkCycle workCycle)
        {
            var linesToBeFlushed = _context.WorkCycleLines.Where(wc => wc.WorkCycleId == workCycle.Id).ToList();
            _context.WorkCycleLines.RemoveRange(linesToBeFlushed);
            _context.SaveChanges();
        }

        #endregion
    }
}