using System.Data.Entity;
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


        
    }
}