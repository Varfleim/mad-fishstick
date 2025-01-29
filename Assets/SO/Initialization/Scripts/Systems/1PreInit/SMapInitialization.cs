
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Map;
using SO.Region;
using HS;

namespace SO.Initialization
{
    public class SMapInitialization : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;

        public void Init(IEcsSystems systems)
        {
            //Инициализируем карты
            MapsInitialization();
        }

        readonly EcsFilterInject<Inc<SRMapInitialization>> mapInitializationSelfRequestFilter = default;
        readonly EcsPoolInject<SRMapInitialization> mapInitializationSelfRequestPool = default;
        void MapsInitialization()
        {
            //Для каждого запроса инициализации карты
            foreach(int mapRequestEntity in mapInitializationSelfRequestFilter.Value)
            {
                //Берём запрос
                ref SRMapInitialization requestComp = ref mapInitializationSelfRequestPool.Value.Get(mapRequestEntity);

                //Инициализируем карту
                MapInitialization(
                    ref requestComp,
                    mapRequestEntity);

                //Удаляем запрос
                mapInitializationSelfRequestPool.Value.Del(mapRequestEntity);
            }
        }

        readonly EcsPoolInject<SRMapCreation> mapCreationSelfRequestPool = default;
        readonly EcsPoolInject<SRHexasphereGeneration> hexasphereGenerationSelfRequestPool = default;
        readonly EcsPoolInject<SRRegionsGeneration> regionsGenerationSelfRequestPool = default;
        void MapInitialization(
            ref SRMapInitialization requestComp,
            int mapEntity)
        {
            //Назначаем сущности запрос создания карты
            ref SRMapCreation mapCreationRequestComp = ref mapCreationSelfRequestPool.Value.Add(mapEntity);

            //Заполняем данные запроса
            mapCreationRequestComp = new(
                requestComp.mapName);

            //Назначаем сущности запрос генерации гексасферы
            ref SRHexasphereGeneration hexasphereGenerationRequestComp = ref hexasphereGenerationSelfRequestPool.Value.Add(mapEntity);

            //Заполняем данные запроса
            hexasphereGenerationRequestComp = new(
                world.Value.PackEntity(mapEntity),
                requestComp.hexasphereSubdivisions);

            //Назначаем сущности запрос генерации регионов
            ref SRRegionsGeneration regionsGenerationRequestComp = ref regionsGenerationSelfRequestPool.Value.Add(mapEntity);

            //Заполняем данные запроса
            regionsGenerationRequestComp = new(
                requestComp.averageProvincesPerRegion);
        }
    }
}
