﻿using System;
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

            AddOrUpdateWorkingCycleLines(workCycle);
        }



        /// <summary>
        ///     Adds Working Cycles with lines
        /// </summary>
        /// <param name="workCycle"></param>
        public void AddWorkingCycle(WorkCycle workCycle)
        {
            _context.WorkCycles.Add(workCycle);
            _context.SaveChanges();

            var workCycleInDb = _context.WorkCycles
                .SingleOrDefault(wc => wc.Name == workCycle.Name);
            //if (workCycleInDb != null)
            //    workCycle.Lines
            //        .ToList()
            //        .ForEach(w => w.WorkCycleId = workCycleInDb.Id);

            //workCycle.Lines.ToList().ForEach(w => _context.WorkCycleLines.Add(w));

            try
            {
                AddOrUpdateWorkingCycleLines(workCycle);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);

                if (workCycleInDb != null)
                {
                    _context.WorkCycles.Remove(workCycleInDb);
                    _context.SaveChanges();
                }

                throw e;
            }
        }

        /// <summary>
        ///     Retrieve Work Cycles by Work Cycle Name
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
        ///     Retrieve Work Cycles by Work Cycle Id
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

                if (line.RawMaterialId == 0)
                    line.RawMaterialId = AddNewRawMaterialToRawMaterialTable(line);

                if (line.SupplierId == 0)
                    line.SupplierId = AddNewSupplierToSupplierTable(line);

                _context.WorkCycleLines.Add(line);

                _context.SaveChanges();
            }
        }

        private int AddNewSupplierToSupplierTable(WorkCycleLines line)
        {
            _context.Suppliers.Add(new Supplier { Name = line.SupplierStringTemp, Active = true });
            _context.SaveChanges();
            return _context.Suppliers.SingleOrDefault(s => s.Name == line.SupplierStringTemp).Id;
        }

        private int AddNewRawMaterialToRawMaterialTable(WorkCycleLines line)
        {
            _context.RawMaterials.Add(new RawMaterial { Name = line.RawMaterialStringTemp, RawMaterialCategoryId = 1 });
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