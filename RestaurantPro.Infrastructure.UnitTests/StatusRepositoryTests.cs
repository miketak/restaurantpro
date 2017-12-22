using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantPro.Core;

namespace RestaurantPro.Infrastructure.UnitTests
{
    [TestClass]
    public class StatusRepositoryTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RestProContext _context;

        public StatusRepositoryTests()
        {
            _context = new RestProContext();
            _unitOfWork = new UnitOfWork(_context);
        }

        [TestMethod]
        public void RetrieveAllStatuses()
        {
            var expectedNumber = 6;
            
            var count = _unitOfWork.Statuses.GetAll().Count();

            Assert.AreEqual(expectedNumber, count);
            
        }
    }
}