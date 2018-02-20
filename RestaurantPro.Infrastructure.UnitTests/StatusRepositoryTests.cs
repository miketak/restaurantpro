using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using RestaurantPro.Core;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace RestaurantPro.Infrastructure.UnitTests
{
    [TestFixture]
    public class StatusRepositoryTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RestProContext _context;

        public StatusRepositoryTests()
        {
            _context = new RestProContext();
            _unitOfWork = new UnitOfWork(_context);
        }
    }
}