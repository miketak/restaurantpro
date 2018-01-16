using System.Data.Entity;
using System.Linq;
using RestaurantPro.Core;
using RestaurantPro.Core.Repositories;
using RestaurantPro.Core.Services;

namespace RestaurantPro.Infrastructure.Services
{
    public class RestProService : IRestProService
    {
        protected readonly IUnitOfWork _unitOfWork;

        public RestProService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            InventoryService = new InventoryService(_unitOfWork);
        }

        public IInventoryService InventoryService { get; private set; }

        
    }
}