
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MF.Map
{
    public class SMapCreation : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CMap> mapPool = default;

        readonly EcsFilterInject<Inc<SRMapCreation>> mapCreationSelfRequestFilter = default;
        readonly EcsPoolInject<SRMapCreation> mapCreationSelfRequestPool = default;

        readonly EcsPoolInject<RMapActivation> mapActivationRequestPool = default;

        public void Init(IEcsSystems systems)
        {
            //Создаём карты
            MapsCreation();
        }

        void MapsCreation()
        {
            //Для каждого запроса создания карты
            foreach(int mapRequestEntity in mapCreationSelfRequestFilter.Value)
            {
                //Берём запрос
                ref SRMapCreation requestComp = ref mapCreationSelfRequestPool.Value.Get(mapRequestEntity);

                //Создаём карту
                MapCreation(
                    ref requestComp,
                    mapRequestEntity);

                //Берём карту
                ref CMap map = ref mapPool.Value.Get(mapRequestEntity);

                UnityEngine.Debug.LogWarning(map.selfName);

                if (true)
                {
                    //Запрашиваем активацию карты
                    MapData.MapActivationRequest(
                        world.Value,
                        mapActivationRequestPool.Value,
                        map.selfPE);
                }

                //Удаляем запрос
                mapCreationSelfRequestPool.Value.Del(mapRequestEntity);
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
