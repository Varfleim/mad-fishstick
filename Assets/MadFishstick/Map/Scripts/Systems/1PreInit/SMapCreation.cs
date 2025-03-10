
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MF.Map
{
    public class SMapCreation : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CMap> mapPool = default;

        readonly EcsFilterInject<Inc<SRMapCreation>> mapCreationSRFilter = default;
        readonly EcsPoolInject<SRMapCreation> mapCreationSRPool = default;

        readonly EcsPoolInject<RMapActivation> mapActivationRPool = default;

        public void Init(IEcsSystems systems)
        {
            //Создаём карты
            MapsCreation();
        }

        void MapsCreation()
        {
            //Для каждого запроса создания карты
            foreach(int mapRequestEntity in mapCreationSRFilter.Value)
            {
                //Берём запрос
                ref SRMapCreation requestComp = ref mapCreationSRPool.Value.Get(mapRequestEntity);

                //Создаём карту
                MapCreation(
                    ref requestComp,
                    mapRequestEntity);

                //Берём карту
                ref CMap map = ref mapPool.Value.Get(mapRequestEntity);

                if (true)
                {
                    //Запрашиваем активацию карты
                    MapData.MapActivationRequest(
                        world.Value,
                        mapActivationRPool.Value,
                        map.selfPE);
                }

                //Удаляем запрос
                mapCreationSRPool.Value.Del(mapRequestEntity);
            }
        }

        void MapCreation(
            ref SRMapCreation requestComp,
            int mapEntity)
        {
            //Назначаем переданной сущности компонент карты
            ref CMap map = ref mapPool.Value.Add(mapEntity);

            //Заполняем основные данные карты
            map = new(
                world.Value.PackEntity(mapEntity), requestComp.mapName);
        }
    }
}
