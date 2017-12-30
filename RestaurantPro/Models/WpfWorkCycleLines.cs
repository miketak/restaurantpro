using System;
using RestaurantPro.Core;
using RestaurantPro.Core.Domain;
using RestaurantPro.Infrastructure;

namespace RestaurantPro.Models
{
    public class WpfWorkCycleLines : ValidatableBindableBase
    {
        private readonly IUnitOfWork _unitOfWork = new UnitOfWork(new RestProContext()); //shall be taken out

        public int WorkCycleId { get; set; }

        public int RawMaterialId { get; set; }

        public int SupplierId { get; set; }

        public float? ActualQuantity { get; set; }

        public float? CurrentQuantity { get; set; }

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
                return rawMaterial != null ? rawMaterial.Name : null;
            }
            set
            {
                var rawMatFromDataGrid = _unitOfWork
                .RawMaterials
                .SingleOrDefault(raw => raw.Name == value);

                RawMaterialId = rawMatFromDataGrid != null ? rawMatFromDataGrid.Id : 0;
            }
        }

        public string Supplier
        {
            get
            {
                var supplier = _unitOfWork.Suppliers.Get(SupplierId);
                return supplier != null ? supplier.Name : null;
            }
            set
            {
                var supFromDataGrid = _unitOfWork
                    .Suppliers
                    .SingleOrDefault(sp => sp.Name == value);

                SupplierId = supFromDataGrid != null ? supFromDataGrid.Id : 0;
            }
        }

        #endregion

        #region CalculatedFields

        private void CalculateLineTotal()
        {
            TotalPrice = UnitPrice * PlannedQuantity;
        }

        #endregion


    }
}