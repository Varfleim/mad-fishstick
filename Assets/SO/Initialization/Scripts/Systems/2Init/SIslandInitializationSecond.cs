
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using SO.Island;
using SO.LandOwnership;

namespace SO.Initialization
{
    public class SIslandInitializationSecond : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsFilterInject<Inc<SRIslandInitializationSecond>> islandInitializationSecondSRFilter = default;
        readonly EcsPoolInject<SRIslandInitializationSecond> islandInitializationSecondSRPool = default;

        public void Init(IEcsSystems systems)
        {
            //Проводим вторичную инициализацию островов
            IslandsInitializationEffects();
        }

        void IslandsInitializationEffects()
        {
            //Для каждой провинции с вторичным инициализатором острова
            foreach(int provinceRequestEntity in islandInitializationSecondSRFilter.Value)
            {
                //Берём запрос
                ref SRIslandInitializationSecond requestComp = ref islandInitializationSecondSRPool.Value.Get(provinceRequestEntity);

                //Инициализируем остров
                IslandInitializationEffects(
                    ref requestComp,
                    provinceRequestEntity);

                //Удаляем запрос
                islandInitializationSecondSRPool.Value.Del(provinceRequestEntity);
            }
        }

        readonly EcsPoolInject<SRIslandCreation> islandCreationSRPool = default;
        readonly EcsPoolInject<RLandChangeOwner> landChangeOwnerRPool = default;
        void IslandInitializationEffects(
            ref SRIslandInitializationSecond requestComp,
            int provinceEntity)
        {
            //Назначаем сущности запрос создания острова
            ref SRIslandCreation creationRequestComp = ref islandCreationSRPool.Value.Add(provinceEntity);

            //Заполняем данные запроса
            creationRequestComp = new(
                "Test Island");

            //Запрашиваем изменение владельца земли (острова)
            LandOwnershipData.LandChangeOwnerRequest(
                world.Value,
                landChangeOwnerRPool.Value,
                requestComp.ownerAgentPE,
                world.Value.PackEntity(provinceEntity));
        }
    }
}
