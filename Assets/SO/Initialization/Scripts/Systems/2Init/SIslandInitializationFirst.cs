
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using SO.Region;

namespace SO.Initialization
{
    public class SIslandInitializationFirst : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CRegionCore> rCPool = default;

        readonly EcsFilterInject<Inc<RIslandInitializationFirst>> islandInitializationFirstRFilter = default;
        readonly EcsPoolInject<RIslandInitializationFirst> islandInitializationFirstRPool = default;

        readonly EcsPoolInject<SRIslandInitializationSecond> islandInitializationSecondSRPool = default;

        public void Init(IEcsSystems systems)
        {
            //Проводим первичную инициализацию островов
            IslandsInitializationLocation();
        }

        void IslandsInitializationLocation()
        {
            //Для каждого первичного инициализатора острова
            foreach(int requestEntity in islandInitializationFirstRFilter.Value)
            {
                //Берём запрос
                ref RIslandInitializationFirst requestComp = ref islandInitializationFirstRPool.Value.Get(requestEntity);

                //Подбираем остров для инициализации
                IslandInitializationLocation(ref requestComp);

                //Удаляем запрос
                islandInitializationFirstRPool.Value.Del(requestEntity);
            }
        }

        void IslandInitializationLocation(
            ref RIslandInitializationFirst requestComp)
        {
            //Получаем сущность острова, соответствующего условиям
            int islandEntity = RegionGetProvinceEntity(ref requestComp);

            //Запрашиваем вторичную инициализацию найденного острова
            InitializerData.IslandInitializationSecondRequest(
                islandInitializationSecondSRPool.Value,
                islandEntity,
                ref requestComp);
        }

        int RegionGetProvinceEntity(
            ref RIslandInitializationFirst requestComp)
        {
            //Берём родительский регион
            requestComp.parentRegionPE.Unpack(world.Value, out int regionEntity);
            ref CRegionCore rC = ref rCPool.Value.Get(regionEntity);

            //Берём сущность случайной провинции региона
            rC.GetProvinceRandom().Unpack(world.Value, out int provinceEntity);

            //Пока провинция имеет запрос вторичной инициализации
            while(islandInitializationSecondSRPool.Value.Has(provinceEntity) == true)
            {
                //Берём сущность случайной провинции региона
                rC.GetProvinceRandom().Unpack(world.Value, out provinceEntity);
            }

            //Возвращаем сущность итоговой провинции
            return provinceEntity;
        }
    }
}
