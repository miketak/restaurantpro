using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.UnitTests
{
    [TestClass]
    public class WorkCycleRepositoryTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RestProContext _context;

        public WorkCycleRepositoryTests()
        {
            _context = new RestProContext();
            _unitOfWork = new UnitOfWork(_context);
        }

        [TestMethod]
        public void TestAddAndRemoveWorkCycle()
        {
            _unitOfWork.WorkCycles.Add(testCycle1);
            _unitOfWork.Complete();
            var workCycle = _unitOfWork.WorkCycles.SingleOrDefault(w => w.Name == "Cycle 600");

            Assert.AreEqual(testCycle1.Name, workCycle.Name);

            _unitOfWork.WorkCycles.Remove(workCycle);
            _unitOfWork.Complete();
        }

        [TestMethod]
        public void TestWorkCycleDeactivation()
        {
            _unitOfWork.WorkCycles.Add(testCycle1);
            _unitOfWork.Complete();
            var workCycleFromDb = _unitOfWork.WorkCycles.SingleOrDefault(cycle => cycle.Name == testCycle1.Name);

            _unitOfWork.WorkCycles.DeactivateWorkCycle(workCycleFromDb.Id);

            Assert.IsFalse(workCycleFromDb.Active);

            _unitOfWork.WorkCycles.Remove(workCycleFromDb);
            _unitOfWork.Complete();
        }

        [TestMethod]
        public void TestWorkCycleFieldsUpdate()
        {
            _unitOfWork.WorkCycles.Add(testCycle1);
            _unitOfWork.Complete();
            const string expectedNewCycleName = "New Cycle Name";
            var testInsertedWorkCycle = _unitOfWork.WorkCycles.SingleOrDefault(cycle => cycle.Name == testCycle1.Name);

            testInsertedWorkCycle.Name = expectedNewCycleName;
            testInsertedWorkCycle.DateEnd = new DateTime(2017, 12, 25);
            _unitOfWork.Complete();

            Assert.AreEqual(expectedNewCycleName, testInsertedWorkCycle.Name);

            _unitOfWork.WorkCycles.Remove(testInsertedWorkCycle);
            _unitOfWork.Complete();
        }

        [TestMethod]
        public void TestAddWorkingCycleAndGetWorkCycleByName()
        {
            const string WorkCycleName = "Cycle Test A";
            var expectedLineCount = 5;
            var workCycleFromTest = GetWorkCycleWithLines(WorkCycleName, true);


            _unitOfWork.WorkCycles.AddWorkingCycle(workCycleFromTest);
            var workCycleInDb = _unitOfWork.WorkCycles.GetWorkCycleByWorkCycleName(WorkCycleName, true);
            var lineCountInDb = workCycleInDb.WorkCycleLines.Count;

            Assert.AreEqual(expectedLineCount, lineCountInDb);

            _unitOfWork.WorkCycles.Remove(workCycleInDb);
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
                throw new AssertFailedException("No suppliers in Database");
            if (wcs.Length == 0)
                throw new AssertFailedException("No Working Cycles in Database");
            if (rms.Length == 0)
                throw new AssertFailedException("No Raw Materials in Database");
            if (statuses.Length == 0)
                throw new AssertFailedException("No Statuses in Database");
            if (users.Length == 0)
                throw new AssertFailedException("No users in database");

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


    }
}