
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MF.Map
{
    public class SMapRender : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsFilterInject<Inc<CMapModeCore, CActiveMapMode, SRMapModeUpdate>> activeMapModeFilter = default;
        readonly EcsPoolInject<CMapModeCore> mapModePool = default;


        readonly EcsPoolInject<RMapRenderUpdate> mapRenderUpdateRequestPool = default;

        public void Run(IEcsSystems systems)
        {
            //Для каждого активного режима карты с запросом обновления
            foreach(int activeMapModeEntity in activeMapModeFilter.Value)
            {
                //Берём режим карты
                ref CMapModeCore activeMapMode = ref mapModePool.Value.Get(activeMapModeEntity);

                //Устанавливаем параметры визуализации провинций на карте
                ProvinceSetMapRenderValues(
                    ref activeMapMode,
                    out bool isHeightUpdated,
                    out bool isColorUpdated);

                //Запрашиваем обновление карты
                MapData.MapRenderUpdateRequest(
                    world.Value,
                    mapRenderUpdateRequestPool.Value,
                    false, isHeightUpdated, isColorUpdated);
            }
        }

        readonly EcsFilterInject<Inc<CProvinceRender, SRSetMapRenderValues>> provinceSetMapRenderValuesFilter = default;
        readonly EcsFilterInject<Inc<CProvinceRender>, Exc<SRSetMapRenderValues>> provinceWithoutSetMapRenderValuesFilter = default;
        readonly EcsPoolInject<CProvinceRender> pRPool = default;
        readonly EcsPoolInject<SRSetMapRenderValues> setMapRenderValuesSelfRequestPool = default;
        void ProvinceSetMapRenderValues(
            ref CMapModeCore mapMode,
            out bool isHeightUpdated, out bool isColorUpdated)
        {
            //Устанавливаем значения по умолчанию
            isHeightUpdated = false;
            isColorUpdated = false;

            //Для каждой провинции без запроса изменения визуализации
            foreach (int provinceEntity in provinceWithoutSetMapRenderValuesFilter.Value)
            {
                //Берём провинцию
                ref CProvinceRender pR = ref pRPool.Value.Get(provinceEntity);

                //Обновляем отображаемый объект провинции
                MapModeData.UpdateProvinceDisplayedObject(
                    ref mapMode,
                    ref pR,
                    new());

                //Изменяем параметры визуализации провинции
                isHeightUpdated = MapModeData.UpdateProvinceHeight(
                    ref mapMode,
                    ref pR,
                    0.0f);

                isColorUpdated = MapModeData.UpdateProvinceColorIndex(
                    ref mapMode,
                    ref pR,
                    -1);
            }

            //Если нет провинций с изменением запроса визуализации
            if(provinceSetMapRenderValuesFilter.Value.GetEntitiesCount() == 0)
            {
                //Отмечаем, что требуется обновление карты
                isHeightUpdated = true;
                isColorUpdated = true;
            }

            //Для каждой провинции с запросом изменения визуализации
            foreach (int provinceEntity in provinceSetMapRenderValuesFilter.Value)
            {
                //Берём провинцию и запрос
                ref CProvinceRender pR = ref pRPool.Value.Get(provinceEntity);
                ref SRSetMapRenderValues requestComp = ref setMapRenderValuesSelfRequestPool.Value.Get(provinceEntity);

                //Обновляем отображаемый объект провинции
                MapModeData.UpdateProvinceDisplayedObject(
                    ref mapMode,
                    ref pR,
                    requestComp.displayedObjectPE);

                //Изменяем параметры визуализации провинции
                isHeightUpdated = MapModeData.UpdateProvinceHeight(
                    ref mapMode,
                    ref pR,
                    requestComp.height);

                isColorUpdated = MapModeData.UpdateProvinceColorIndex(
                    ref mapMode,
                    ref pR,
                    requestComp.colorIndex);

                //Удаляем запрос
                setMapRenderValuesSelfRequestPool.Value.Del(provinceEntity);
            }
        }
    }
}
