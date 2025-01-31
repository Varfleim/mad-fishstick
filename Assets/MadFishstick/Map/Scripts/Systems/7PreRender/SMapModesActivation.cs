
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MF.Map
{
    public class SMapModesActivation : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CMapModeCore> mapModeCorePool = default;
        readonly EcsPoolInject<CActiveMapMode> activeMapModePool = default;


        readonly EcsPoolInject<RMapProvincesUpdate> mapProvincesUpdateRPool = default;

        readonly EcsFilterInject<Inc<CMapModeCore, SRMapModeUpdate>> mapModeUpdateSRFilter = default;
        readonly EcsPoolInject<SRMapModeUpdate> mapModeUpdateSRPool = default;

        public void Run(IEcsSystems systems)
        {
            //Активируем режим карты по запросу
            MapModesActivation();

        }

        readonly EcsFilterInject<Inc<RMapModeActivation>> mapModeActivationRFilter = default;
        readonly EcsPoolInject<RMapModeActivation> mapModeActivationRPool = default;
        void MapModesActivation()
        {
            //Для каждого запроса активации режима карты
            foreach(int requestEntity in mapModeActivationRFilter.Value)
            {
                //Берём запрос
                ref RMapModeActivation requestComp = ref mapModeActivationRPool.Value.Get(requestEntity);

                //Деактивируем активный режим карты
                bool isMapModeDeactivated = MapModeDeactivationCheck(ref requestComp);

                //Если режим карты деактивирован
                if(isMapModeDeactivated == true)
                {
                    //Активируем режим карты
                    MapModeActivation(ref requestComp);
                }

                //Удаляем запрос
                mapModeActivationRPool.Value.Del(requestEntity);
            }
        }

        void MapModeActivation(
            ref RMapModeActivation requestComp)
        {
            //Берём запрошенный режим карты
            requestComp.mapModePE.Unpack(world.Value, out int mapModeEntity);
            ref CMapModeCore mapMode = ref mapModeCorePool.Value.Get(mapModeEntity);

            //Назначаем ему компонент активного режима
            activeMapModePool.Value.Add(mapModeEntity);

            //Удаляем все запросы обновления режимов карты
            MapModeUpdatesCancel();

            //Запрашиваем обновление режима карты
            MapModeData.MapModeUpdateRequest(
                mapModeUpdateSRPool.Value,
                mapModeEntity);

            //Запрашиваем обновление провинций карты
            MapData.MapProvincesUpdateRequest(
                world.Value,
                mapProvincesUpdateRPool.Value,
                true, false, false);
        }

        readonly EcsFilterInject<Inc<CMapModeCore, CActiveMapMode>> activeMapModeFilter = default;
        bool MapModeDeactivationCheck(
            ref RMapModeActivation requestComp)
        {
            //Берём активный режим карты
            foreach(int activeMapModeEntity in activeMapModeFilter.Value)
            {
                ref CMapModeCore activeMapMode = ref mapModeCorePool.Value.Get(activeMapModeEntity);

                //Если это не тот режим карты, который требуется активировать
                if(activeMapMode.selfPE.EqualsTo(requestComp.mapModePE) == false)
                {
                    //Удаляем компонент активного режима
                    activeMapModePool.Value.Del(activeMapModeEntity);

                    //Возвращаем, что режим карты деактивирован
                    return true;
                }
                //Иначе
                else
                {
                    //Возвращаем, что режим карты не деактивирован
                    return false;
                }
            }

            //Возвращаем, что режим карты деактивирован
            return true;
        }

        /// <summary>
        /// Удаление всех существующих запросов обновления режима карты
        /// </summary>
        void MapModeUpdatesCancel()
        {
            //Для каждого запроса обновления режима карты
            foreach(int mapModeEntity in mapModeUpdateSRFilter.Value)
            {
                //Удаляем запрос
                mapModeUpdateSRPool.Value.Del(mapModeEntity);
            }
        }
    }
}
