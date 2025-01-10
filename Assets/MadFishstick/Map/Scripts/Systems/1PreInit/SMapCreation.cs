
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MF.Map
{
    public class SMapCreation : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;

        readonly EcsPoolInject<CMap> mapPool = default;

        readonly EcsPoolInject<RMapActivation> mapActivationRequestPool = default;

        readonly EcsCustomInject<MapData> mapData = default;

        public void Init(IEcsSystems systems)
        {
            //Создаём карты
            MapsCreation();
        }

        void MapsCreation()
        {
            //Для каждой карты
            for(int a = 0; a < mapData.Value.mapNames.Length; a++)
            {
                //Создаём карту
                int mapEntity = MapCreation(
                    a, mapData.Value.mapNames[a]);

                //Берём карту
                ref CMap map = ref mapPool.Value.Get(mapEntity);

                //Запрашиваем генерацию карты
                MapGenerationRequest(
                    mapEntity,
                    map.selfIndex);

                //Если это первая карта
                if(a == 0)
                {
                    //Запрашиваем активацию карты
                    MapData.MapActivationRequest(
                        world.Value,
                        mapActivationRequestPool.Value,
                        map.selfPE);
                }
            }
        }

        int MapCreation(
            int mapIndex, string mapName)
        {
            //Создаём новую сущность и назначаем ей компонент карты
            int mapEntity = world.Value.NewEntity();
            ref CMap map = ref mapPool.Value.Add(mapEntity);

            //Заполняем основные данные карты
            map = new(
                world.Value.PackEntity(mapEntity), mapIndex, mapName);

            return mapEntity;
        }

        readonly EcsPoolInject<SRMapGeneration> mapGenerationSelfRequestPool = default;
        void MapGenerationRequest(
            int mapEntity,
            int mapIndex)
        {
            //Назначаем сущности карты самозапрос генерации
            ref SRMapGeneration requestComp = ref mapGenerationSelfRequestPool.Value.Add(mapEntity);

            //Заполняем данные запроса
            requestComp = new(
                mapIndex);
        }
    }
}
