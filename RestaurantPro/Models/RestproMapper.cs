using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using AutoMapper;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Models
{
    /// <summary>
    ///     AutoMapper Class
    /// </summary>
    public static class RestproMapper
    {
        #region Work Cycle Mappings

        /// <summary>
        ///     Maps WpfWorkCycle List to WorkCycle List
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static List<WpfWorkCycle> MapWorkCycleListToWpfWorkCycleList(List<WorkCycle> source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WorkCycle, WpfWorkCycle>()
                    .ForMember(dest => dest.SubTotal, opt => opt.Ignore())
                    .ForMember(dest => dest.Tax, opt => opt.Ignore())
                    .ForMember(dest => dest.Total, opt => opt.Ignore())
                    .ForMember(dest => dest.FullName, opt => opt.Ignore())
                    .ForMember(dest => dest.DateBeginForView, opt => opt.Ignore())
                    .ForMember(dest => dest.DateEndForView, opt => opt.Ignore())
                    .ForMember(dest => dest.FirstName, opt => opt.Ignore())
                    .ForMember(dest => dest.LastName, opt => opt.Ignore())
                    .ForMember(dest => dest.Lines, opt => opt.Ignore());
            });

            var iMapper = config.CreateMapper();

            var target = iMapper.Map<List<WorkCycle>, List<WpfWorkCycle>>(source);

            return target;
        }

        /// <summary>
        ///     Maps WpfWorkCycle Type to WorkCycle
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static WorkCycle MapWpfWorkCycleToWorkCycle(WpfWorkCycle source)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<WpfWorkCycle, WorkCycle>(); });

            var iMapper = config.CreateMapper();

            return iMapper.Map<WpfWorkCycle, WorkCycle>(source);
        }

        /// <summary>
        ///     Maps WpfWorkCycle Type to WorkCycle and Handle Lines
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static WorkCycle MapWpfWorkCycleToWorkCycleAndHandleLines(WpfWorkCycle source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WpfWorkCycle, WorkCycle>()
                    .ForMember(dest => dest.Lines, opt => opt.Ignore());
            });

            var iMapper = config.CreateMapper();

            var domainWorkCycle = iMapper.Map<WpfWorkCycle, WorkCycle>(source);

            domainWorkCycle.Lines = MapWpfWorkCycleLinesToWorkCycleLinesList(source.Lines.ToList());

            return domainWorkCycle;
        }

        /// <summary>
        ///     Maps WorkCycle type to WpfWorkCycle
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static WpfWorkCycle MapWorkCycleToWpfWorkCycle(WorkCycle source)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<WorkCycle, WpfWorkCycle>(); });

            var iMapper = config.CreateMapper();

            return iMapper.Map<WorkCycle, WpfWorkCycle>(source);
        }

        /// <summary>
        ///     Maps WorkcycleLines to WpfWorkCycleLines
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static List<WpfWorkCycleLines> MapWorkCycleLinesToWpfWorkCycleList(List<WorkCycleLines> source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WorkCycleLines, WpfWorkCycleLines>()
                    .ForMember(dest => dest.RawMaterial, opt => opt.Ignore())
                    .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
                    .ForMember(dest => dest.Supplier, opt => opt.Ignore());
            });

            var iMapper = config.CreateMapper();

            var target = iMapper.Map<List<WorkCycleLines>, List<WpfWorkCycleLines>>(source);

            return target;
        }

        /// <summary>
        ///     Maps WpfWorkcycleLines to WorkCycleLines
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static List<WorkCycleLines> MapWpfWorkCycleLinesToWorkCycleLinesList(List<WpfWorkCycleLines> source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WpfWorkCycleLines, WorkCycleLines>()
                    .ForMember(dest => dest.RawMaterial, opt => opt.Ignore())
                    .ForMember(dest => dest.WorkCycle, opt => opt.Ignore())
                    .ForMember(dest => dest.Supplier, opt => opt.Ignore())
                    .ForMember(dest => dest.Location, opt => opt.Ignore())
                    .ForMember(dest => dest.RawMaterialStringTemp, opt => opt.MapFrom(src => src.NewRawMaterial))
                    .ForMember(dest => dest.SupplierStringTemp, opt => opt.MapFrom(src => src.NewSupplier));
            });

            var iMapper = config.CreateMapper();

            var target = iMapper.Map<List<WpfWorkCycleLines>, List<WorkCycleLines>>(source);

            return target;
        }

        #endregion

        #region Purchase Order Mappings

        /// <summary>
        ///     Maps Purchase Order to Wpf Purchase Order
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static WpfPurchaseOrder MapPurchaseOrderToWpfPurchaseOrder(PurchaseOrder source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseOrder, WpfPurchaseOrder>()
                    .ForMember(dest => dest.DateCreatedForView, opt => opt.Ignore())
                    .ForMember(dest => dest.FullName, opt => opt.Ignore())
                    .ForMember(dest => dest.WorkCycleName, opt => opt.Ignore())
                    .ForMember(dest => dest.Lines, opt => opt.Ignore());
            });

            var iMapper = config.CreateMapper();

            var target = iMapper.Map<PurchaseOrder, WpfPurchaseOrder>(source);

            if (source.Lines != null)
                target.Lines =
                    new BindingList<WpfPurchaseOrderLine>(
                        MapPurchaseOrderLineListToWpfPurchaseOrderLineList(source.Lines.ToList()));

            return target;
        }

        internal static List<WpfPurchaseOrder> MapPurchaseOrderListToWpfPurchaseOrderList(
            List<PurchaseOrder> purchaseOrders)
        {
            return purchaseOrders
                .Select(MapPurchaseOrderToWpfPurchaseOrder)
                .ToList();
        }

        private static WpfPurchaseOrderLine MapPurchaseOrderLineToWpfPurchaseOrderLine(PurchaseOrderLine source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseOrderLine, WpfPurchaseOrderLine>()
                    .ForMember(dest => dest.RawMaterial, opt => opt.Ignore())
                    .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
                    .ForMember(dest => dest.Supplier, opt => opt.Ignore());
            });

            var iMapper = config.CreateMapper();

            return iMapper.Map<PurchaseOrderLine, WpfPurchaseOrderLine>(source);
        }

        public static List<WpfPurchaseOrderLine> MapPurchaseOrderLineListToWpfPurchaseOrderLineList(
            List<PurchaseOrderLine> source)
        {
            return source
                .Select(MapPurchaseOrderLineToWpfPurchaseOrderLine)
                .ToList();
        }

        internal static PurchaseOrder MapWpfPurchaseOrderToPurchaseOrderWithLines(WpfPurchaseOrder source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WpfPurchaseOrder, PurchaseOrder>()
                    .ForMember(dest => dest.Lines, opt => opt.Ignore());
            });

            var iMapper = config.CreateMapper();

            var target = iMapper.Map<WpfPurchaseOrder, PurchaseOrder>(source);

            target.Lines = MapWpfPurchaseOrderLineListToPurchaseOrderLineList(source.Lines.ToList());

            return target;
        }

        private static PurchaseOrderLine MapWpfPurchaseOrderLineToPurchaseOrderLine(WpfPurchaseOrderLine source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WpfPurchaseOrderLine, PurchaseOrderLine>()
                    .ForMember(dest => dest.RawMaterial, opt => opt.Ignore())
                    .ForMember(dest => dest.PurchaseOrder, opt => opt.Ignore())
                    .ForMember(dest => dest.Supplier, opt => opt.Ignore())
                    .ForMember(dest => dest.RawMaterialStringTemp, opt => opt.MapFrom(src => src.NewRawMaterial))
                    .ForMember(dest => dest.SupplierStringTemp, opt => opt.MapFrom(src => src.NewSupplier));
            });

            var iMapper = config.CreateMapper();

            var target = iMapper.Map<WpfPurchaseOrderLine, PurchaseOrderLine>(source);

            return target;
        }

        private static List<PurchaseOrderLine> MapWpfPurchaseOrderLineListToPurchaseOrderLineList(
            List<WpfPurchaseOrderLine> source)
        {
            return source.Select(MapWpfPurchaseOrderLineToPurchaseOrderLine)
                .ToList();
        }

        #endregion

        #region Supplier Mappings

        internal static WpfSupplier MapSupplierToWpfSupplier(Supplier source)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Supplier, WpfSupplier>());

            var iMapper = config.CreateMapper();

            var target = iMapper.Map<Supplier, WpfSupplier>(source);

            return target;
        }

        internal static List<WpfSupplier> MapSupplierListToWpfSupplierList(List<Supplier> source)
        {
            return source
                .Select(MapSupplierToWpfSupplier)
                .ToList();
        }

        internal static Supplier MapWpfSupplierToSupplier(WpfSupplier source)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<WpfSupplier, Supplier>()
                    .ForMember(dest => dest.Active, opt => opt.Ignore()));

            var iMapper = config.CreateMapper();

            var target = iMapper.Map<WpfSupplier, Supplier>(source);

            return target;
        }

        internal static List<Supplier> MapWpfSupplierListToSupplierList(List<WpfSupplier> source)
        {
            return source
                .Select(MapWpfSupplierToSupplier)
                .ToList();
        }

        #endregion

        #region Raw Material Mappings

        internal static WpfRawMaterial MapRawMaterialToWpfRawMaterial(RawMaterial source)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<RawMaterial, WpfRawMaterial>()
                    .ForMember(dest => dest.Category, opt => opt.Ignore()));

            var iMapper = config.CreateMapper();

            var target = iMapper.Map<RawMaterial, WpfRawMaterial>(source);

            return target;
        }

        internal static RawMaterial MapWpfRawMaterialToRawMaterial(WpfRawMaterial source)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<WpfRawMaterial, RawMaterial>()
                    .ForMember(dest => dest.RawMaterialCategory, opt => opt.Ignore())
                    .ForMember(dest => dest.RawMaterialCatalog, opt => opt.Ignore())
                    .ForMember(dest => dest.PurchaseOrderLines, opt => opt.Ignore())
                    .ForMember(dest => dest.WorkCycleLines, opt => opt.Ignore()));

            var iMapper = config.CreateMapper();

            var target = iMapper.Map<WpfRawMaterial, RawMaterial>(source);

            return target;
        }

        internal static List<WpfRawMaterial> MapRawMaterialListToWpfRawMaterialList(List<RawMaterial> source)
        {
            return source
                .Select(MapRawMaterialToWpfRawMaterial)
                .ToList();
        }

        internal static List<RawMaterial> MapWpfRawMaterialLIstToRawMaterialList(List<WpfRawMaterial> source)
        {
            return source
                .Select(MapWpfRawMaterialToRawMaterial)
                .ToList();
        }

        #endregion

        #region Raw Material Categoies Mappings

        internal static WpfRawMaterialCategory MapRawMaterialCategoryToWpfRawMaterialCategory(
            RawMaterialCategory source)
        {
            var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<RawMaterialCategory, WpfRawMaterialCategory>());

            var iMapper = config.CreateMapper();

            var target = iMapper.Map<RawMaterialCategory, WpfRawMaterialCategory>(source);

            return target;
        }

        internal static RawMaterialCategory MapWpfRawMaterialCategoryToRawMaterialCategory(WpfRawMaterialCategory source)
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<WpfRawMaterialCategory, RawMaterialCategory>());

            var iMapper = config.CreateMapper();

            var target = iMapper.Map<WpfRawMaterialCategory, RawMaterialCategory>(source);

            return target;
        }

        internal static List<WpfRawMaterialCategory> MapRawMaterialCategoryListToWpfRawMaterialCategoryList(List<RawMaterialCategory> source)
        {
            return source
                .Select(MapRawMaterialCategoryToWpfRawMaterialCategory)
                .ToList();
        }        
        
        internal static List<RawMaterialCategory> MapWpfRawMaterialCategoryListToRawMaterialCategoryList(List<WpfRawMaterialCategory> source)
        {
            return source
                .Select(MapWpfRawMaterialCategoryToRawMaterialCategory)
                .ToList();
        }

        #endregion

        #region Location Mappings

        internal static WpfLocation MapLocationToWpfLocation(Location source)
        {
            var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<Location, WpfLocation>());

            var iMapper = config.CreateMapper();

            var target = iMapper.Map<Location, WpfLocation>(source);

            return target;
        }        
        
        internal static Location MapWpfLocationToLocation(WpfLocation source)
        {
            var config = new MapperConfiguration(cfg =>
            cfg.CreateMap<WpfLocation, Location>());

            var iMapper = config.CreateMapper();

            var target = iMapper.Map<WpfLocation, Location>(source);

            return target;
        }

        internal static List<WpfLocation> MapLocationListToWpfLocationList(List<Location> source)
        {
            return source
                .Select(MapLocationToWpfLocation).ToList();
        }

        internal static List<Location> MapWpfLocationListToLocationList(List<WpfLocation> source)
        {
            return source
                .Select(MapWpfLocationToLocation)
                .ToList();
        }

        #endregion


    }
}