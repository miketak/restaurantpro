using System;
using System.Linq;
using RestaurantPro.Core;

namespace RestaurantPro.Infrastructure.Services
{
    public class RestProServiceBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private static Random random = new Random();

        public RestProServiceBase(IUnitOfWork unitOfWork)
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

        protected int GenerateTrackingNumber()
        {
            var length = 6;
            const string chars = "0123456789";
            var val =  new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            var trackingNumbers = _unitOfWork.PurchaseOrderTransactions.GetAll()
                .Select(a => a.TrackingNumber).ToList();

            if (trackingNumbers.Contains(int.Parse(val)))
                GenerateTrackingNumber();

            return int.Parse(val);
        }

       
    }
}