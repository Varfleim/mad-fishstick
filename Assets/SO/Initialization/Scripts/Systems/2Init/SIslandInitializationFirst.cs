
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using SO.Region;

namespace SO.Initialization
{
    public class SIslandInitializationFirst : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CRegionCore> rCPool = default;

        readonly EcsFilterInject<Inc<RIslandInitializationFirst>> islandInitializationFirstRequestFilter = default;
        readonly EcsPoolInject<RIslandInitializationFirst> islandInitializationFirstRequestPool = default;

        readonly EcsPoolInject<SRIslandInitializationSecond> islandInitializationSecondSelfRequestPool = default;

        public void Init(IEcsSystems systems)
        {
            //Проводим первичную инициализацию островов
            IslandsInitializationLocation();
        }

        void IslandsInitializationLocation()
        {
            //Для каждого первичного инициализатора острова
            foreach(int requestEntity in islandInitializationFirstRequestFilter.Value)
            {
                //Берём запрос
                ref RIslandInitializationFirst requestComp = ref islandInitializationFirstRequestPool.Value.Get(requestEntity);

                //Подбираем остров для инициализации
                IslandInitializationLocation(ref requestComp);

                //Удаляем запрос
                islandInitializationFirstRequestPool.Value.Del(requestEntity);
            }
        }

        void IslandInitializationLocation(
            ref RIslandInitializationFirst requestComp)
        {
            //Получаем сущность острова, соответствующего условиям
            int islandEntity = RegionGetProvinceEntity(ref requestComp);

            //Запрашиваем вторичную инициализацию найденного острова
            InitializerData.IslandInitializationSecondRequest(
                islandInitializationSecondSelfRequestPool.Value,
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
            while(islandInitializationSecondSelfRequestPool.Value.Has(provinceEntity) == true)
            {
                //Берём сущность случайной провинции региона
                rC.GetProvinceRandom().Unpack(world.Value, out provinceEntity);
            }

            //Возвращаем сущность итоговой провинции
            return provinceEntity;
        }
    }
}
