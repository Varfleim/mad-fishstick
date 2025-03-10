
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace SO.Initialization
{
    public class SRegionInitializationSecond : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;

        readonly EcsFilterInject<Inc<SRRegionInitializationSecond>> regionInitializationSecondSRFilter = default;
        readonly EcsPoolInject<SRRegionInitializationSecond> regionInitializationSecondSRPool = default;

        public void Init(IEcsSystems systems)
        {
            //Проводим вторичную инициализацию регионов
            RegionsInitializationEffects();
        }

        void RegionsInitializationEffects()
        {
            //Для каждого региона с вторичным инициализатором
            foreach(int regionRequestEntity in regionInitializationSecondSRFilter.Value)
            {
                //Берём запрос
                ref SRRegionInitializationSecond requestComp = ref regionInitializationSecondSRPool.Value.Get(regionRequestEntity);

                //Инициализируем регион
                RegionInitializationEffects(
                    ref requestComp,
                    regionRequestEntity);

                //Удаляем запрос
                regionInitializationSecondSRPool.Value.Del(regionRequestEntity);
            }
        }

        readonly EcsPoolInject<RIslandInitializationFirst> islandInitializationFirstRPool = default;
        void RegionInitializationEffects(
            ref SRRegionInitializationSecond requestComp,
            int regionEntity)
        {
            //Запрашиваем первичную инициализацию острова в регионе
            InitializerData.IslandInitializationFirstRequest(
                world.Value,
                islandInitializationFirstRPool.Value,
                world.Value.PackEntity(regionEntity),
                requestComp.ownerAgentPE);
            InitializerData.IslandInitializationFirstRequest(
                world.Value,
                islandInitializationFirstRPool.Value,
                world.Value.PackEntity(regionEntity),
                requestComp.ownerAgentPE);
            InitializerData.IslandInitializationFirstRequest(
                world.Value,
                islandInitializationFirstRPool.Value,
                world.Value.PackEntity(regionEntity),
                requestComp.ownerAgentPE);
            InitializerData.IslandInitializationFirstRequest(
                world.Value,
                islandInitializationFirstRPool.Value,
                world.Value.PackEntity(regionEntity),
                requestComp.ownerAgentPE);
            InitializerData.IslandInitializationFirstRequest(
                world.Value,
                islandInitializationFirstRPool.Value,
                world.Value.PackEntity(regionEntity),
                requestComp.ownerAgentPE);
            //InitializerData.IslandInitializationFirstRequest(
            //    world.Value,
            //    islandInitializationFirstRPool.Value,
            //    world.Value.PackEntity(regionEntity),
            //    requestComp.ownerAgentPE);
            //InitializerData.IslandInitializationFirstRequest(
            //    world.Value,
            //    islandInitializationFirstRPool.Value,
            //    world.Value.PackEntity(regionEntity),
            //    requestComp.ownerAgentPE);
            //InitializerData.IslandInitializationFirstRequest(
            //    world.Value,
            //    islandInitializationFirstRPool.Value,
            //    world.Value.PackEntity(regionEntity),
            //    requestComp.ownerAgentPE);
            //InitializerData.IslandInitializationFirstRequest(
            //    world.Value,
            //    islandInitializationFirstRPool.Value,
            //    world.Value.PackEntity(regionEntity),
            //    requestComp.ownerAgentPE);
            //InitializerData.IslandInitializationFirstRequest(
            //    world.Value,
            //    islandInitializationFirstRPool.Value,
            //    world.Value.PackEntity(regionEntity),
            //    requestComp.ownerAgentPE);
        }
    }
}
