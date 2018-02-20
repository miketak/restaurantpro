using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.UnitTests
{
    [TestFixture]
    public class WorkCycleRepositoryTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RestProContext _context;

        public WorkCycleRepositoryTests()
        {
            _context = new RestProContext();
            _unitOfWork = new UnitOfWork(_context);
        }


        [Test]
        public void DeactivateWorkCycle_WhenCalled_SetsActiveBitToFalse()
        {
            var workCycleFromDb = AddAndGetTestWorkCycle();

            _unitOfWork.WorkCycles.DeactivateWorkCycle(workCycleFromDb.Id);

            Assert.IsFalse(workCycleFromDb.Active);
        }

        [Test]
        public void AddWorkingCycle_WhenCalled_CorrectlyAddsWorkCycleWithLines()
        {
            const string WorkCycleName = "Cycle Test A";
            var workCycleToDb = GetWorkCycleWithLines(WorkCycleName, true);


            _unitOfWork.WorkCycles.AddWorkingCycle(workCycleToDb);

            var workCycleInDb = _unitOfWork.WorkCycles.GetWorkCycleByWorkCycleName(WorkCycleName, true);

            Assert.That(workCycleInDb.WorkCycleLines.Count, Is.EqualTo(workCycleToDb.Lines.Count()));
        }

        [TearDown]
        public void TearDown()
        {
            RemoveTestWorkCycle();
            RemoveTestWorkCycle2();
        }

        #region Helper Methods

        private WorkCycle AddAndGetTestWorkCycle()
        {
            var cycleInDb = _unitOfWork.WorkCycles.SingleOrDefault(cycle => cycle.Name == testCycle1.Name);

            if (cycleInDb != null)
            {
                _unitOfWork.WorkCycles.Remove(cycleInDb);
                _unitOfWork.Complete();
            }

            _unitOfWork.WorkCycles.Add(testCycle1);

            _unitOfWork.Complete();

            return _unitOfWork.WorkCycles.SingleOrDefault(cycle => cycle.Name == testCycle1.Name);
        }

        private void RemoveTestWorkCycle()
        {
            var workCycleInDb = _unitOfWork.WorkCycles.GetWorkCycleByWorkCycleName(testCycle1.Name, true) ??
                                _unitOfWork.WorkCycles.GetWorkCycleByWorkCycleName(testCycle1.Name, false); 

            if (workCycleInDb == null)
                return;

            _unitOfWork.WorkCycles.Remove(workCycleInDb);
            _unitOfWork.Complete();

        }

        private void RemoveTestWorkCycle2()
        {
            var workCycleInDb2 = _unitOfWork.WorkCycles.GetWorkCycleByWorkCycleName("Cycle Test A", true);

            if (workCycleInDb2 == null)
                return;

            _unitOfWork.WorkCycles.Remove(workCycleInDb2);
            _unitOfWork.Complete();
        }


        private WorkCycle GetWorkCycleWithLines(string workCycleName, bool isActive)
        {
            var wcs = _unitOfWork.WorkCycles.GetAll().ToArray();
            var supps = _unitOfWork.Suppliers.GetAll().ToArray();
            var rms = _unitOfWork.RawMaterials.GetAll().ToArray();
            var statuses = _unitOfWork.Statuses.GetAll().ToArray();
            var users = _unitOfWork.Users.GetAll().ToArray();

            if (supps.Length == 0)
                throw new AssertionException("No suppliers in Database");
            if (wcs.Length == 0)
                throw new AssertionException("No Working Cycles in Database");
            if (rms.Length == 0)
                throw new AssertionException("No Raw Materials in Database");
            if (statuses.Length == 0)
                throw new AssertionException("No Statuses in Database");
            if (users.Length == 0)
                throw new AssertionException("No users in database");

            var wc = new WorkCycle
            {
                Name = workCycleName,
                DateBegin = new DateTime(2017, 12, 10),
                DateEnd = new DateTime(2017, 12, 23),
                Active = isActive,
                UserId = users[0].Id,
                StatusId = WorkCycleStatus.Draft.ToString()
            };

            var wcLines = new List<WorkCycleLines>
            {
                new WorkCycleLines { RawMaterialId = rms[0].Id, SupplierId = supps[0].Id, UnitPrice = 125, PlannedQuantity = 91, UnitOfMeasure = "crates" },
                new WorkCycleLines { RawMaterialId = rms[1].Id, SupplierId = supps[1].Id, UnitPrice = 125, PlannedQuantity = 91, UnitOfMeasure = "crates" },
                new WorkCycleLines { RawMaterialId = rms[2].Id, SupplierId = supps[2].Id, UnitPrice = 125, PlannedQuantity = 91, UnitOfMeasure = "crates" },
                new WorkCycleLines { RawMaterialId = rms[3].Id, SupplierId = supps[3].Id, UnitPrice = 125, PlannedQuantity = 91, UnitOfMeasure = "crates" },
                new WorkCycleLines { RawMaterialId = rms[4].Id, SupplierId = supps[1].Id, UnitPrice = 125, PlannedQuantity = 91, UnitOfMeasure = "crates" },
            };
            wc.Lines = wcLines;

            return wc;
        }

        private readonly WorkCycle testCycle1 = new WorkCycle
        {
            Name = "Cycle 600",
            DateBegin = new DateTime(2017, 12, 10),
            DateEnd = new DateTime(2017, 12, 15),
            Active = true,
            UserId = 1,
            StatusId = WorkCycleStatus.Draft.ToString()
        };

        #endregion

        
    }
}