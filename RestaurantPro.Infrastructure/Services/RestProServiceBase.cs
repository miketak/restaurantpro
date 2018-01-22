﻿using System;
using System.Linq;
using RestaurantPro.Core;

namespace RestaurantPro.Infrastructure.Services
{
    public class RestProServiceBase
    {
        private readonly IUnitOfWork _unitOfWork;
        protected readonly InventoryTransactions InventoryTransactions;

        public RestProServiceBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            InventoryTransactions = new InventoryTransactions(unitOfWork);
        }
        protected string GenerateNewPurchaseOrderNumber()
        {
            var poNumbers = _unitOfWork
                .PurchaseOrders.GetAll().ToList();

            if (!poNumbers.Any())
                return "1000";

            var maxPoNumber = poNumbers
                .Select(a => a.PurchaseOrderNumber)
                .Select(int.Parse)
                .Max();

            var newPoNumber = maxPoNumber + 1;

            return newPoNumber.ToString();
        }



       
    }
}