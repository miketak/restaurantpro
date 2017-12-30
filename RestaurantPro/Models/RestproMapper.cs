using System.Collections.Generic;
using AutoMapper;
using RestaurantPro.Core.Domain;

namespace RestaurantPro.Models
{
    public static class RestproMapper
    {
        /// <summary>
        /// Maps WpfWorkCycle List to WorkCycle List
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<WpfWorkCycle> MapWorkCycleListToWpfWorkCycleList(List<WorkCycle> source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WorkCycle, WpfWorkCycle>();
            });

            IMapper iMapper = config.CreateMapper();

            var target = iMapper.Map<List<WorkCycle>, List<WpfWorkCycle>>(source);

            return target;
        }

        /// <summary>
        /// Maps WpfWorkCycle Type to WorkCycle
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static WorkCycle MapWpfWorkCycleToWorkCycle(WpfWorkCycle source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WpfWorkCycle, WorkCycle>();
            });

            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<WpfWorkCycle, WorkCycle>(source);
        }

        /// <summary>
        /// Maps WorkCycle type to WpfWorkCycle
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static WpfWorkCycle MapWorkCycleToWpfWorkCycle(WorkCycle source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WorkCycle, WpfWorkCycle>();
            });

            IMapper iMapper = config.CreateMapper();

            return iMapper.Map<WorkCycle, WpfWorkCycle>(source);
        }

        /// <summary>
        /// Maps WorkcycleLines to WpfWorkCycleLines
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<WpfWorkCycleLines> MapWorkCycleLinesToWpfWorkCycleList(List<WorkCycleLines> source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WorkCycleLines, WpfWorkCycleLines>()
                    .ForMember(dest => dest.RawMaterial, opt => opt.Ignore())
                    .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
                    .ForMember(dest => dest.Supplier, opt => opt.Ignore());
            });

            IMapper iMapper = config.CreateMapper();

            var target = iMapper.Map<List<WorkCycleLines>, List<WpfWorkCycleLines>>(source);

            return target;
        }

        /// <summary>
        /// Maps WpfWorkcycleLines to WorkCycleLines
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<WorkCycleLines> MapWpfWorkCycleLinesToWorkCycleLinesList(List<WpfWorkCycleLines> source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WpfWorkCycleLines, WorkCycleLines>();
            });

            IMapper iMapper = config.CreateMapper();

            var target = iMapper.Map<List<WpfWorkCycleLines>, List<WorkCycleLines>>(source);

            return target;
        }
        
    }
}