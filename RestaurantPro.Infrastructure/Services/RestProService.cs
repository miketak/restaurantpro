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
        }

        protected string GenerateNewPurchaseOrder()
        {
            var maxPurchaseOrderNumber = _unitOfWork
                .PurchaseOrders.GetAll()
                .Select(a => a.PurchaseOrderNumber)
                .Select(int.Parse)
                .Max();

            var newPoNumber = maxPurchaseOrderNumber + 1;

            return newPoNumber.ToString();
        }


        
    }
}