
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using SO.Region;

namespace SO.Initialization
{
    public class SRegionInitializationFirst : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CMapRegions> mapRPool = default;

        readonly EcsFilterInject<Inc<RRegionInitializationFirst>> regionInitializationFirstRequestFilter = default;
        readonly EcsPoolInject<RRegionInitializationFirst> regionInitializationFirstRequestPool = default;

        readonly EcsPoolInject<SRRegionInitializationSecond> regionInitializationSecondSelfRequestPool = default;

        public void Init(IEcsSystems systems)
        {
            //Проводим первичную инициализацию регионов
            RegionsInitializationLocation();
        }

        void RegionsInitializationLocation()
        {
            //Для каждого первичного инициализатора региона
            foreach(int requestEntity in regionInitializationFirstRequestFilter.Value)
            {
                //Берём запрос
                ref RRegionInitializationFirst requestComp = ref regionInitializationFirstRequestPool.Value.Get(requestEntity);

                //Подбираем регион для инициализации
                RegionInitializationLocation(ref requestComp);

                //Удаляем запрос
                regionInitializationFirstRequestPool.Value.Del(requestEntity);
            }
        }

        void RegionInitializationLocation(
            ref RRegionInitializationFirst requestComp)
        {
            //Получаем сущность региона, соответствующего условиям
            int regionEntity = MapGetRegionEntity(ref requestComp);

            //Запрашиваем вторичную инициализацию найденного региона
            InitializerData.RegionInitializationSecondRequest(
                regionInitializationSecondSelfRequestPool.Value,
                regionEntity,
                ref requestComp);
        }

        int MapGetRegionEntity(
            ref RRegionInitializationFirst requestComp)
        {
            //Берём компонент регионов родительской карты
            requestComp.parentMapPE.Unpack(world.Value, out int mapEntity);
            ref CMapRegions mapR = ref mapRPool.Value.Get(mapEntity);

            //Берём сущность случайного региона
            mapR.GetRegionRandom().Unpack(world.Value, out int regionEntity);

            //Пока регион имеет запрос вторичной инициализации
            while (regionInitializationSecondSelfRequestPool.Value.Has(regionEntity) == true)
            {
                //Берём сущность случайного региона
                mapR.GetRegionRandom().Unpack(world.Value, out regionEntity);
            }

            //Возвращаем сущность итогового региона
            return regionEntity;
        }
    }
}
