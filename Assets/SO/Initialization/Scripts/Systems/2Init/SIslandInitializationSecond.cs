
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using SO.Island;
using SO.LandOwnership;

namespace SO.Initialization
{
    public class SIslandInitializationSecond : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsFilterInject<Inc<SRIslandInitializationSecond>> islandInitializationSecondSelfRequestFilter = default;
        readonly EcsPoolInject<SRIslandInitializationSecond> islandInitializationSecondSelfRequestPool = default;

        public void Init(IEcsSystems systems)
        {
            //Проводим вторичную инициализацию островов
            IslandsInitializationEffects();
        }

        void IslandsInitializationEffects()
        {
            //Для каждой провинции с вторичным инициализатором острова
            foreach(int provinceRequestEntity in islandInitializationSecondSelfRequestFilter.Value)
            {
                //Берём запрос
                ref SRIslandInitializationSecond requestComp = ref islandInitializationSecondSelfRequestPool.Value.Get(provinceRequestEntity);

                //Инициализируем остров
                IslandInitializationEffects(
                    ref requestComp,
                    provinceRequestEntity);

                //Удаляем запрос
                islandInitializationSecondSelfRequestPool.Value.Del(provinceRequestEntity);
            }
        }

        readonly EcsPoolInject<SRIslandCreation> islandCreationSelfRequestPool = default;
        readonly EcsPoolInject<RLandChangeOwner> landChangeOwnerRequestPool = default;
        void IslandInitializationEffects(
            ref SRIslandInitializationSecond requestComp,
            int provinceEntity)
        {
            //Назначаем сущности запрос создания острова
            ref SRIslandCreation creationRequestComp = ref islandCreationSelfRequestPool.Value.Add(provinceEntity);

            //Заполняем данные запроса
            creationRequestComp = new(
                "TestIsland");

            //Запрашиваем изменение владельца земли (острова)
            LandOwnershipData.LandChangeOwnerRequest(
                world.Value,
                landChangeOwnerRequestPool.Value,
                requestComp.ownerAgentPE,
                world.Value.PackEntity(provinceEntity));
        }
    }
}
