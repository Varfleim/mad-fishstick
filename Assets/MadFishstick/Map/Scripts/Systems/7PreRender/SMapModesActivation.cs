
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MF.Map
{
    public class SMapModesActivation : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CMapModeCore> mapModeCorePool = default;
        readonly EcsPoolInject<CActiveMapMode> activeMapModePool = default;


        readonly EcsPoolInject<RMapRenderUpdate> mapRenderUpdateRequestPool = default;

        readonly EcsFilterInject<Inc<CMapModeCore, SRMapModeUpdate>> mapModeUpdateSelfRequestFilter = default;
        readonly EcsPoolInject<SRMapModeUpdate> mapModeUpdateSelfRequestPool = default;

        public void Run(IEcsSystems systems)
        {
            //Активируем режим карты по запросу
            MapModesActivation();

            //Обновляем списки цветов режимов карты
            MapModesUpdateColorsList();
        }

        readonly EcsFilterInject<Inc<RMapModeActivation>> mapModeActivationRequestFilter = default;
        readonly EcsPoolInject<RMapModeActivation> mapModeActivationRequestPool = default;
        void MapModesActivation()
        {
            //Для каждого запроса активации режима карты
            foreach(int requestEntity in mapModeActivationRequestFilter.Value)
            {
                //Берём запрос
                ref RMapModeActivation requestComp = ref mapModeActivationRequestPool.Value.Get(requestEntity);

                //Деактивируем активный режим карты
                bool isMapModeDeactivated = MapModeDeactivationCheck(ref requestComp);

                //Если режим карты деактивирован
                if(isMapModeDeactivated == true)
                {
                    //Активируем режим карты
                    MapModeActivation(ref requestComp);
                }

                //Удаляем запрос
                mapModeActivationRequestPool.Value.Del(requestEntity);
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
                mapModeUpdateSelfRequestPool.Value,
                mapModeEntity);

            //Запрашиваем обновление материалов карты
            MapData.MapRenderUpdateRequest(
                world.Value,
                mapRenderUpdateRequestPool.Value,
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
            foreach(int mapModeEntity in mapModeUpdateSelfRequestFilter.Value)
            {
                //Удаляем запрос
                mapModeUpdateSelfRequestPool.Value.Del(mapModeEntity);
            }
        }

        readonly EcsFilterInject<Inc<RMapModeUpdateColorsList>> mapModeUpdateColorsListRequestFilter = default;
        readonly EcsPoolInject<RMapModeUpdateColorsList> mapModeUpdateColorsListRequestPool = default;
        void MapModesUpdateColorsList()
        {
            //Для каждого запроса обновления списка цветов режима карты
            foreach(int requestEntity in mapModeUpdateColorsListRequestFilter.Value)
            {
                //Берём запрос
                ref RMapModeUpdateColorsList requestComp = ref mapModeUpdateColorsListRequestPool.Value.Get(requestEntity);

                //Берём режим карты
                requestComp.mapModePE.Unpack(world.Value, out int mapModeEntity);
                ref CMapModeCore mapMode = ref mapModeCorePool.Value.Get(mapModeEntity);

                //Обновляем список цветов
                MapModeUpdateColorsList(
                    ref mapMode,
                    ref requestComp);

                //Удаляем запрос
                mapModeUpdateColorsListRequestPool.Value.Del(requestEntity);
            }
        }

        void MapModeUpdateColorsList(
            ref CMapModeCore mapMode,
            ref RMapModeUpdateColorsList requestComp)
        {
            //Очищаем список цветов режима карты
            mapMode.colors.Clear();

            //Для каждого цвета в запросе
            for(int a = 0; a < requestComp.mapModeColors.Count; a++)
            {
                //Заносим цвет в список
                mapMode.colors.Add(requestComp.mapModeColors[a]);
            }
        }
    }
}
