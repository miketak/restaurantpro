using NUnit.Framework;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Infrastructure.UnitTests
{
    [TestFixture]
    public class SupplierRepositoryTests
    {
        private IUnitOfWork _unitOfWork;
        private RestProContext _context;

        public SupplierRepositoryTests()
        {
            _context = new RestProContext();
            _unitOfWork = new UnitOfWork(_context);
        }
    }
}