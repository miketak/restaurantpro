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


            var workCycle = _unitOfWork.WorkCycles.SingleOrDefault(w => w.Id == 1);
            Assert.AreEqual(testCycle1.Name, workCycle.Name);

        }

        [TestMethod]
        public void UpdateWorkCycleNameByCycleNameTest()
        {
            string expectedNewCycleName = "New Cycle Name";

            var testInsertedWorkCycle = _unitOfWork.WorkCycles.SingleOrDefault(cycle => cycle.Name == "Cycle 1");

            testInsertedWorkCycle.Name = expectedNewCycleName;
            testInsertedWorkCycle.DateEnd = new DateTime(2017, 12, 25);

            _unitOfWork.Complete();

            Assert.AreEqual(expectedNewCycleName, testInsertedWorkCycle.Name);

            _unitOfWork.WorkCycles.Remove(testInsertedWorkCycle);

            _unitOfWork.Complete();
        }



        





    }
}