using RestaurantPro.Core;
using RestaurantPro.Infrastructure;

namespace RestaurantPro.Models
{
    public class WpfPurchaseOrderLine : ValidatableBindableBase
    {
        private IUnitOfWork _unitOfWork = new UnitOfWork(new RestProContext());

        public int PurchaseOrderId { get; set; }

        public int RawMaterialId { get; set; }

        public int SupplierId { get; set; }

        private double _unitPrice;
        public double UnitPrice
        {
            get { return _unitPrice; }
            set
            {
                SetProperty(ref _unitPrice, value);
                CalculateLineTotal();
            }
        }

        private float _quantity;
        public float Quantity
        {
            get { return _quantity; }
            set
            {
                SetProperty(ref _quantity, value);
                CalculateLineTotal();
            }
        }

        private string _unitOfMeasure;
        public string UnitOfMeasure
        {
            get { return _unitOfMeasure; }
            set { SetProperty(ref _unitOfMeasure, value); }
        }

        #region Purchase Order Lines View

        public string RawMaterial
        {
            get
            {
                var rawMaterial = _unitOfWork.RawMaterials.Get(RawMaterialId);
                return rawMaterial == null ? rawMaterial.Name : null;
            }
            set
            {
                var rawMaterialInDb = _unitOfWork
                    .RawMaterials
                    .SingleOrDefault(raw => raw.Name.ToLower() == value.ToLower());

                RawMaterialId = rawMaterialInDb != null ? rawMaterialInDb.Id : 0;
                NewRawMaterial = rawMaterialInDb == null ? value : null;
            }
        }

        public string Supplier
        {
            get
            {
                var supplier = _unitOfWork.Suppliers.Get(SupplierId);
                return supplier == null ? supplier.Name : null;
            }
            set
            {
                var supplierInDb = _unitOfWork
                    .Suppliers
                    .SingleOrDefault(sp => sp.Name.ToLower() == value.ToLower());

                SupplierId = supplierInDb != null ? supplierInDb.Id : 0;
                NewSupplier = supplierInDb == null ? value : null;
            }
        }

        private double _totalPrice;
        public double TotalPrice
        {
            get { return _totalPrice; }
            set { SetProperty(ref _totalPrice, value); }
        }
        #endregion

        #region For Persistence
        public string NewSupplier { get; set; }
        public string NewRawMaterial { get; set; }

        #endregion

        #region CalculatedFields

        private void CalculateLineTotal()
        {
            TotalPrice = UnitPrice * Quantity;
        }

        /// <summary>
        /// Sends line total to WpfWorkCycle Class for SubTotal Section
        /// </summary>
        /// <returns></returns>
        public double GetLineTotal()
        {
            CalculateLineTotal();
            return TotalPrice;
        }

        #endregion
    }
}