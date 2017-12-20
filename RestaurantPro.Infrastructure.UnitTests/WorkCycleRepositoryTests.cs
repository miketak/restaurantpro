using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.UnitTests
{
    [TestClass]
    public class WorkCycleRepositoryTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private RestProContext _context;


        private WorkCycle testCycle1 = new WorkCycle
        {
            Name = "Cycle 600",
            DateBegin = new DateTime(2017, 12, 10),
            DateEnd = new DateTime(2017, 12, 15),
            Active = true,
            UserId = 1
        };

        public WorkCycleRepositoryTests()
        {
            _context = new RestProContext();
            _unitOfWork = new UnitOfWork(_context);
        }



        [TestMethod]
        public void RetrieveWorkCycleByWorkCycleIdTest()
        {
            _unitOfWork.WorkCycles.Add(testCycle1);
            _unitOfWork.Complete();


            var workCycle = _unitOfWork.WorkCycles.SingleOrDefault(w => w.Name == "Cycle 600");
            Assert.AreEqual(testCycle1.Name, workCycle.Name);

            _unitOfWork.WorkCycles.Remove(workCycle);
            _unitOfWork.Complete();

        }

        [TestMethod]
        public void DeactivateWorkCycleTest()
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
        public void UpdateWorkCycleNameByCycleNameTest()
        {
            string expectedNewCycleName = "New Cycle Name";
            _unitOfWork.WorkCycles.Add(testCycle1);
            _unitOfWork.Complete();

            var testInsertedWorkCycle = _unitOfWork.WorkCycles.SingleOrDefault(cycle => cycle.Name == testCycle1.Name);

            testInsertedWorkCycle.Name = expectedNewCycleName;
            testInsertedWorkCycle.DateEnd = new DateTime(2017, 12, 25);
            _unitOfWork.Complete();

            Assert.AreEqual(expectedNewCycleName, testInsertedWorkCycle.Name);

            _unitOfWork.WorkCycles.Remove(testInsertedWorkCycle);
            _unitOfWork.Complete();
        }

        [TestMethod]
        public void UpdateWorkCycleTest()
        {
            _unitOfWork.WorkCycles.Add(testCycle1);
            _unitOfWork.Complete();

            var workCycleInDb = _unitOfWork.WorkCycles.SingleOrDefault(w => w.Name == testCycle1.Name);

            var newWorkCycle = new WorkCycle
            {
                Id = workCycleInDb.Id,
                Name = "Lindador Cycle",
                DateBegin = new DateTime(2017, 10, 05),
                DateEnd = new DateTime(2018, 12, 15),
                Active = true,
                UserId = 2
            };
            _unitOfWork.WorkCycles.UpdateWorkCycle(newWorkCycle);

            var workCycleToRemove = _unitOfWork.WorkCycles.SingleOrDefault(w => w.Id == workCycleInDb.Id);

            Assert.AreEqual(newWorkCycle.Name, workCycleToRemove.Name);
            
            _unitOfWork.WorkCycles.Remove(workCycleToRemove);
            _unitOfWork.Complete();
        }





        





    }
}