using System;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Infrastructure;

namespace RestaurantPro.Models
{
    /// <summary>
    /// Work Cycle Lines
    /// </summary>
    public class WpfWorkCycleLines : ValidatableBindableBase
    {
        private readonly IUnitOfWork _unitOfWork = new UnitOfWork(new RestProContext()); //shall be taken out

        public int WorkCycleId { get; set; }

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

        private float _plannedQuantity;
        public float PlannedQuantity
        {
            get { return _plannedQuantity; }
            set
            {
                SetProperty(ref _plannedQuantity, value);
                CalculateLineTotal();
            }
        }

        public string UnitOfMeasure { get; set; }

        public bool WasMoved { get; set; }

        public DateTime? MoveDate { get; set; }

        public string LocationId { get; set; }

        #region WorkCycleAddEditView DataGrid Elements

        private double _totalPrice;
        public double TotalPrice
        {
            get { return _totalPrice; }
            set { SetProperty(ref _totalPrice, value); }
        }

        public string RawMaterial
        {
            get
            {
                var rawMaterial = _unitOfWork.RawMaterials.Get(RawMaterialId);
                return rawMaterial != null ? rawMaterial.Name : NewRawMaterial;
            }
            set
            {
                var rawMaterialInDb = _unitOfWork
                .RawMaterials.ReturnRawMaterialIfExists(value);

                RawMaterialId = rawMaterialInDb != null ? rawMaterialInDb.Id : 0;
                NewRawMaterial = rawMaterialInDb == null ? value : null;
            }
        }

        public string Supplier
        {
            get
            {
                var supplier = _unitOfWork.Suppliers.Get(SupplierId);
                return supplier != null ? supplier.Name : NewSupplier;
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

        #endregion

        #region For Persistence
        public string NewSupplier { get; set; }
        public string NewRawMaterial { get; set; }

        #endregion


        #region CalculatedFields

        private void CalculateLineTotal()
        {
            TotalPrice = UnitPrice * PlannedQuantity;
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