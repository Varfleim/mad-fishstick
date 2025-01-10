
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MF.Map
{
    public class SMapControl : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CMap> mapPool = default;
        readonly EcsPoolInject<CActiveMap> activeMapPool = default;

        readonly EcsFilterInject<Inc<CProvinceCore, CProvinceRender>> pRFilter = default;
        readonly EcsPoolInject<CProvinceRender> pRPool = default;


        readonly EcsPoolInject<RMapRenderInitialization> mapRenderInitializationRequestPool = default;


        readonly EcsCustomInject<MapModeData> mapModeData = default;

        public void Run(IEcsSystems systems)
        {
            //Активируем карту по запросу
            MapsActivation();
        }

        readonly EcsFilterInject<Inc<RMapActivation>> mapActivationRequestFilter = default;
        readonly EcsPoolInject<RMapActivation> mapActivationRequestPool = default;
        void MapsActivation()
        {
            //Для каждого запроса активации карты
            foreach(int requestEntity in mapActivationRequestFilter.Value)
            {
                //Берём запрос
                ref RMapActivation requestComp = ref mapActivationRequestPool.Value.Get(requestEntity);

                //Деактивируем активную карту
                bool isMapDeactivated = MapDeactivationCheck(ref requestComp);

                //Если карта деактивирована
                if(isMapDeactivated == true)
                {
                    //Активируем карту
                    MapActivation(ref requestComp);
                }

                //Удаляем запрос
                mapActivationRequestPool.Value.Del(requestEntity);
            }
        }

        void MapActivation(
            ref RMapActivation requestComp)
        {
            //Берём запрошенную карту
            requestComp.mapPE.Unpack(world.Value, out int mapEntity);
            ref CMap map = ref mapPool.Value.Get(mapEntity);

            //Назначаем ей компонент активной карты
            activeMapPool.Value.Add(mapEntity);

            //Для каждой провинции карты
            for(int a = 0; a < map.provincePEs.Length; a++)
            {
                //Берём сущность провинции и назначаем ей компонент PR
                map.provincePEs[a].Unpack(world.Value, out int provinceEntity);
                ref CProvinceRender pR = ref pRPool.Value.Add(provinceEntity);

                //Заполняем данные PR
                pR = new(0);
            }

            //Запрашиваем инициализацию карты
            MapData.MapRenderInitializationRequest(
                world.Value,
                mapRenderInitializationRequestPool.Value);

            //Запрашиваем активацию стандартного режима карты
            MapModeDefaultActivation();
        }

        readonly EcsFilterInject<Inc<CMap, CActiveMap>> activeMapFilter = default;
        bool MapDeactivationCheck(
            ref RMapActivation requestComp)
        {
            //Берём активную карту
            foreach(int activeMapEntity in activeMapFilter.Value)
            {
                ref CMap activeMap = ref mapPool.Value.Get(activeMapEntity);

                //Если это не та карта, которую требуется активировать
                if(activeMap.selfPE.EqualsTo(in requestComp.mapPE) == false)
                {
                    //Для каждой провинции с компонентом PR
                    foreach(int provinceEntity in pRFilter.Value)
                    {
                        //Берём компонент PR

                        //Удаляем компонент с провинции
                        pRPool.Value.Del(provinceEntity);
                    }

                    //Удаляем компонент активной карты
                    activeMapPool.Value.Del(activeMapEntity);

                    //Возвращаем, что карта деактивирована
                    return true;
                }
                //Иначе
                else
                {
                    //Возвращаем, что карта не деактивирована
                    return false;
                }
            }

            //Возвращаем, что карта деактивирована
            return true;
        }

        readonly EcsPoolInject<RMapModeActivation> mapModeActivationRequestPool = default;
        void MapModeDefaultActivation()
        {
            //Запрашиваем активацию стандартного режима карты
            MapModeData.MapModeActivationRequest(
                world.Value,
                mapModeActivationRequestPool.Value,
                mapModeData.Value.defaultMapModePE);
        }
    }
}
