
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MF.Map
{
    public class SMapModesUpdateColors : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CMapModeCore> mapModeCorePool = default;

        readonly EcsFilterInject<Inc<RMapModeUpdateColorsListSecond>> mapModeUpdateColorsListSecondRFilter = default;
        readonly EcsPoolInject<RMapModeUpdateColorsListSecond> mapModeUpdateColorsListSecondRPool = default;

        public void Run(IEcsSystems systems)
        {
            //Вторично обновляем списки цветов режимов карты
            MapModesUpdateColorsListSecond();
        }

        void MapModesUpdateColorsListSecond()
        {
            //Для каждого запроса вторичного обновления списка цветов режима карты
            foreach (int requestEntity in mapModeUpdateColorsListSecondRFilter.Value)
            {
                //Берём запрос
                ref RMapModeUpdateColorsListSecond requestComp = ref mapModeUpdateColorsListSecondRPool.Value.Get(requestEntity);

                //Берём режим карты
                requestComp.mapModePE.Unpack(world.Value, out int mapModeEntity);
                ref CMapModeCore mapMode = ref mapModeCorePool.Value.Get(mapModeEntity);

                //Обновляем список цветов
                MapModeUpdateColorsList(
                    ref mapMode,
                    ref requestComp);

                //Удаляем запрос
                mapModeUpdateColorsListSecondRPool.Value.Del(requestEntity);
            }
        }

        void MapModeUpdateColorsList(
            ref CMapModeCore mapMode,
            ref RMapModeUpdateColorsListSecond requestComp)
        {
            //Очищаем список цветов режима карты
            mapMode.colors.Clear();

            //Для каждого цвета в запросе
            for (int a = 0; a < requestComp.mapModeColors.Count; a++)
            {
                //Заносим цвет в список
                mapMode.colors.Add(requestComp.mapModeColors[a]);
            }

            //Обновляем стандартный цвет
            mapMode.defaultColor = requestComp.defaultColor;
        }
    }
}
