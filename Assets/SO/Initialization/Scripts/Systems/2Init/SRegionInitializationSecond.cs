
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace SO.Initialization
{
    public class SRegionInitializationSecond : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;

        readonly EcsFilterInject<Inc<SRRegionInitializationSecond>> regionInitializationSecondSelfRequestFilter = default;
        readonly EcsPoolInject<SRRegionInitializationSecond> regionInitializationSecondSelfRequestPool = default;

        public void Init(IEcsSystems systems)
        {
            //Проводим вторичную инициализацию регионов
            RegionsInitializationEffects();
        }

        void RegionsInitializationEffects()
        {
            //Для каждого региона с вторичным инициализатором
            foreach(int regionRequestEntity in regionInitializationSecondSelfRequestFilter.Value)
            {
                //Берём запрос
                ref SRRegionInitializationSecond requestComp = ref regionInitializationSecondSelfRequestPool.Value.Get(regionRequestEntity);

                //Инициализируем регион
                RegionInitializationEffects(
                    ref requestComp,
                    regionRequestEntity);

                //Удаляем запрос
                regionInitializationSecondSelfRequestPool.Value.Del(regionRequestEntity);
            }
        }

        readonly EcsPoolInject<RIslandInitializationFirst> islandInitializationFirstRequestPool = default;
        void RegionInitializationEffects(
            ref SRRegionInitializationSecond requestComp,
            int regionEntity)
        {
            //Запрашиваем первичную инициализацию острова в регионе
            InitializerData.IslandInitializationFirstRequest(
                world.Value,
                islandInitializationFirstRequestPool.Value,
                world.Value.PackEntity(regionEntity),
                requestComp.ownerAgentPE);
            InitializerData.IslandInitializationFirstRequest(
                world.Value,
                islandInitializationFirstRequestPool.Value,
                world.Value.PackEntity(regionEntity),
                requestComp.ownerAgentPE);
            InitializerData.IslandInitializationFirstRequest(
                world.Value,
                islandInitializationFirstRequestPool.Value,
                world.Value.PackEntity(regionEntity),
                requestComp.ownerAgentPE);
            InitializerData.IslandInitializationFirstRequest(
                world.Value,
                islandInitializationFirstRequestPool.Value,
                world.Value.PackEntity(regionEntity),
                requestComp.ownerAgentPE);
            InitializerData.IslandInitializationFirstRequest(
                world.Value,
                islandInitializationFirstRequestPool.Value,
                world.Value.PackEntity(regionEntity),
                requestComp.ownerAgentPE);
            //InitializerData.IslandInitializationFirstRequest(
            //    world.Value,
            //    islandInitializationFirstRequestPool.Value,
            //    world.Value.PackEntity(regionEntity),
            //    requestComp.ownerAgentPE);
            //InitializerData.IslandInitializationFirstRequest(
            //    world.Value,
            //    islandInitializationFirstRequestPool.Value,
            //    world.Value.PackEntity(regionEntity),
            //    requestComp.ownerAgentPE);
            //InitializerData.IslandInitializationFirstRequest(
            //    world.Value,
            //    islandInitializationFirstRequestPool.Value,
            //    world.Value.PackEntity(regionEntity),
            //    requestComp.ownerAgentPE);
            //InitializerData.IslandInitializationFirstRequest(
            //    world.Value,
            //    islandInitializationFirstRequestPool.Value,
            //    world.Value.PackEntity(regionEntity),
            //    requestComp.ownerAgentPE);
            //InitializerData.IslandInitializationFirstRequest(
            //    world.Value,
            //    islandInitializationFirstRequestPool.Value,
            //    world.Value.PackEntity(regionEntity),
            //    requestComp.ownerAgentPE);
        }
    }
}
