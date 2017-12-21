using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Core.Repositories;

namespace RestaurantPro.Infrastructure.UnitTests
{
    [TestClass]
    public class SupplierRepositoryTests
    {
        private IUnitOfWork _unitOfWork;
        private RestProContext _context;

        private Supplier testSupplier1 = new Supplier
        {
            Name = "Kofi and Co Foods",
            Address = "81 Miller AV SW",
            Telephone = "233245042434",
            Email = "ama@yahoo.com"
        };

        public SupplierRepositoryTests()
        {
            _context = new RestProContext();
            _unitOfWork = new UnitOfWork(_context);
        }

        [TestMethod]
        public void RetrieveSupplierTest()
        {
            _unitOfWork.Suppliers.Add(testSupplier1);
            _unitOfWork.Complete();


            var testSupplierInDb = _unitOfWork.Suppliers.SingleOrDefault(w => w.Name == testSupplier1.Name);
            Assert.AreEqual(testSupplier1.Name, testSupplierInDb.Name);

            _unitOfWork.Suppliers.Remove(testSupplierInDb);
            _unitOfWork.Complete();
        }

    }
}