
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

        readonly EcsFilterInject<Inc<SRMapInitialization>> mapInitializationSRFilter = default;
        readonly EcsPoolInject<SRMapInitialization> mapInitializationSRPool = default;
        void MapsInitialization()
        {
            //Для каждого запроса инициализации карты
            foreach(int mapRequestEntity in mapInitializationSRFilter.Value)
            {
                //Берём запрос
                ref SRMapInitialization requestComp = ref mapInitializationSRPool.Value.Get(mapRequestEntity);

                //Инициализируем карту
                MapInitialization(
                    ref requestComp,
                    mapRequestEntity);

                //Удаляем запрос
                mapInitializationSRPool.Value.Del(mapRequestEntity);
            }
        }

        readonly EcsPoolInject<SRMapCreation> mapCreationSRPool = default;
        readonly EcsPoolInject<SRHexasphereGeneration> hexasphereGenerationSRPool = default;
        readonly EcsPoolInject<SRRegionsGeneration> regionsGenerationSRPool = default;
        void MapInitialization(
            ref SRMapInitialization requestComp,
            int mapEntity)
        {
            //Назначаем сущности запрос создания карты
            ref SRMapCreation mapCreationRequestComp = ref mapCreationSRPool.Value.Add(mapEntity);

            //Заполняем данные запроса
            mapCreationRequestComp = new(
                requestComp.mapName);

            //Назначаем сущности запрос генерации гексасферы
            ref SRHexasphereGeneration hexasphereGenerationRequestComp = ref hexasphereGenerationSRPool.Value.Add(mapEntity);

            //Заполняем данные запроса
            hexasphereGenerationRequestComp = new(
                world.Value.PackEntity(mapEntity),
                requestComp.hexasphereSubdivisions);

            //Назначаем сущности запрос генерации регионов
            ref SRRegionsGeneration regionsGenerationRequestComp = ref regionsGenerationSRPool.Value.Add(mapEntity);

            //Заполняем данные запроса
            regionsGenerationRequestComp = new(
                requestComp.averageProvincesPerRegion);
        }
    }
}
