
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MF.Map
{
    public class SMapRender : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsFilterInject<Inc<CMapModeCore, CActiveMapMode, SRMapModeUpdate>> activeMapModeFilter = default;
        readonly EcsPoolInject<CMapModeCore> mapModePool = default;

        readonly EcsPoolInject<CProvinceRender> pRPool = default;
        readonly EcsFilterInject<Inc<CProvinceRender, SRUpdateThinEdges>> provinceUpdateThinEdgesSRFilter = default;
        readonly EcsFilterInject<Inc<CProvinceRender>, Exc<SRUpdateThinEdges>> provinceWithoutUpdateThinEdgesSRFilter = default;
        readonly EcsPoolInject<SRUpdateThinEdges> updateThinEdgesSRPool = default;
        readonly EcsFilterInject<Inc<CProvinceRender, SRUpdateThickEdges>> provinceUpdateThickEdgesSRFilter = default;
        readonly EcsFilterInject<Inc<CProvinceRender>, Exc<SRUpdateThickEdges>> provinceWithoutUpdateThickEdgesSRFilter = default;
        readonly EcsPoolInject<SRUpdateThickEdges> updateThickEdgesSRPool = default;

        readonly EcsPoolInject<RMapEdgesUpdate> mapEdgesUpdateRPool = default;
        readonly EcsPoolInject<RMapProvincesUpdate> mapProvincesUpdateRPool = default;
        
        public void Run(IEcsSystems systems)
        {
            //Обновляем данные граней
            MapEdgesDataUpdate();

            //Для каждого активного режима карты с запросом обновления
            foreach(int activeMapModeEntity in activeMapModeFilter.Value)
            {
                //Берём режим карты
                ref CMapModeCore activeMapMode = ref mapModePool.Value.Get(activeMapModeEntity);

                //Обновляем данные визуализации провинций
                MapProvinceRenderDataUpdate(
                    ref activeMapMode,
                    out bool isHeightUpdated,
                    out bool isColorUpdated);

                //Запрашиваем обновление визуализации провинций карты
                MapData.MapProvincesUpdateRequest(
                    world.Value,
                    mapProvincesUpdateRPool.Value,
                    false, isHeightUpdated, isColorUpdated);
            }
        }

        void MapEdgesDataUpdate()
        {
            //Проверяем, какие грани требуется обновить
            bool isThinUpdated = false;
            bool isThickUpdated = false;

            //Если фильтр обновления тонких граней не пуст
            if (provinceUpdateThinEdgesSRFilter.Value.GetEntitiesCount() > 0)
            {
                //Обновляем тонкие грани
                MapThinEdgesDataUpdate(out isThinUpdated);
            }

            //Если фильтр обновления толстых граней не пуст
            if(provinceUpdateThickEdgesSRFilter.Value.GetEntitiesCount() > 0)
            {
                //Обновляем толстые грани
                MapThickEdgesDataUpdate(out isThickUpdated);
            }

            //Если какие-либо грани были обновлены
            if(isThinUpdated == true
                || isThickUpdated == true)
            {
                //Запрашиваем обновление граней карты
                MapData.MapEdgesUpdateRequest(
                    world.Value,
                    mapEdgesUpdateRPool.Value,
                    isThinUpdated, isThickUpdated);
            }
        }

        void MapThinEdgesDataUpdate(
            out bool isThinUpdated)
        {
            //Устанавливаем значение по умолчанию
            isThinUpdated = false;

            //Для каждой провинции без запроса обновления тонких граней
            foreach (int provinceEntity in provinceWithoutUpdateThinEdgesSRFilter.Value)
            {
                //Берём провинцию
                ref CProvinceRender pR = ref pRPool.Value.Get(provinceEntity);

                //Если индекс тонких граней был обновлён
                if(MapModeData.UpdateProvinceThinEdgesIndex(
                    ref pR,
                    -1) == true)
                {
                    //Отмечаем, что грани обновлены
                    isThinUpdated = true;
                }
            }

            //Если нет провинций с запросом
            if (provinceUpdateThinEdgesSRFilter.Value.GetEntitiesCount() == 0)
            {
                //Отмечаем, что требуется обновление карты
                isThinUpdated = true;
            }

            //Для каждой провинции с запросом
            foreach (int provinceEntity in provinceUpdateThinEdgesSRFilter.Value)
            {
                //Берём провинцию и запрос
                ref CProvinceRender pR = ref pRPool.Value.Get(provinceEntity);
                ref SRUpdateThinEdges requestComp = ref updateThinEdgesSRPool.Value.Get(provinceEntity);

                //Если индекс тонких граней был обновлён
                if (MapModeData.UpdateProvinceThinEdgesIndex(
                    ref pR,
                    requestComp.edgeIndex) == true)
                {
                    //Отмечаем, что грани обновлены
                    isThinUpdated = true;
                }

                //Удаляем запрос
                updateThinEdgesSRPool.Value.Del(provinceEntity);
            }
        }

        void MapThickEdgesDataUpdate(
            out bool isThickUpdated)
        {
            //Устанавливаем значение по умолчанию
            isThickUpdated = false;

            //Для каждой провинции без запроса обновления толстых граней
            foreach (int provinceEntity in provinceWithoutUpdateThickEdgesSRFilter.Value)
            {
                //Берём провинцию
                ref CProvinceRender pR = ref pRPool.Value.Get(provinceEntity);

                //Если индекс толстых граней был обновлён
                if (MapModeData.UpdateProvinceThickEdgesIndex(
                    ref pR,
                    -1) == true)
                {
                    //Отмечаем, что грани обновлены
                    isThickUpdated = true;
                }
            }

            //Если нет провинций с запросом
            if (provinceUpdateThickEdgesSRFilter.Value.GetEntitiesCount() == 0)
            {
                //Отмечаем, что требуется обновление карты
                isThickUpdated = true;
            }

            //Для каждой провинции с запросом
            foreach (int provinceEntity in provinceUpdateThickEdgesSRFilter.Value)
            {
                //Берём провинцию и запрос
                ref CProvinceRender pR = ref pRPool.Value.Get(provinceEntity);
                ref SRUpdateThickEdges requestComp = ref updateThickEdgesSRPool.Value.Get(provinceEntity);

                //Если индекс толстых граней был обновлён
                if (MapModeData.UpdateProvinceThickEdgesIndex(
                    ref pR,
                    requestComp.edgeIndex) == true)
                {
                    //Отмечаем, что грани обновлены
                    isThickUpdated = true;
                }

                //Удаляем запрос
                updateThickEdgesSRPool.Value.Del(provinceEntity);
            }
        }

        readonly EcsFilterInject<Inc<CProvinceRender, SRUpdateProvinceRender>> provinceUpdateProvinceRenderSRFilter = default;
        readonly EcsFilterInject<Inc<CProvinceRender>, Exc<SRUpdateProvinceRender>> provinceWithoutUpdateProvinceRenderSRFilter = default;
        readonly EcsPoolInject<SRUpdateProvinceRender> updateProvinceRenderSRPool = default;
        void MapProvinceRenderDataUpdate(
            ref CMapModeCore mapMode,
            out bool isHeightUpdated, out bool isColorUpdated)
        {
            //Устанавливаем значения по умолчанию
            isHeightUpdated = false;
            isColorUpdated = false;

            //Для каждой провинции без запроса обновления визуализации
            foreach (int provinceEntity in provinceWithoutUpdateProvinceRenderSRFilter.Value)
            {
                //Берём провинцию
                ref CProvinceRender pR = ref pRPool.Value.Get(provinceEntity);

                //Обновляем отображаемый объект провинции
                MapModeData.UpdateProvinceDisplayedObject(
                    ref mapMode,
                    ref pR,
                    new());

                //Изменяем параметры визуализации провинции
                //Если высота была обновлена
                if(MapModeData.UpdateProvinceHeight(
                    ref mapMode,
                    ref pR,
                    0.0f) == true)
                {
                    //Отмечаем, что высота обновлена
                    isHeightUpdated = true;
                }

                //Если индекс цвета был обновлён
                if(MapModeData.UpdateProvinceColorIndex(
                    ref mapMode,
                    ref pR,
                    -1) == true)
                {
                    //Отмечаем, что цвет обновлён
                    isColorUpdated = true;
                }
            }

            //Если нет провинций с запросом
            if(provinceUpdateProvinceRenderSRFilter.Value.GetEntitiesCount() == 0)
            {
                //Отмечаем, что требуется обновление карты
                isHeightUpdated = true;
                isColorUpdated = true;
            }

            //Для каждой провинции с запросом
            foreach (int provinceEntity in provinceUpdateProvinceRenderSRFilter.Value)
            {
                //Берём провинцию и запрос
                ref CProvinceRender pR = ref pRPool.Value.Get(provinceEntity);
                ref SRUpdateProvinceRender requestComp = ref updateProvinceRenderSRPool.Value.Get(provinceEntity);

                //Обновляем отображаемый объект провинции
                MapModeData.UpdateProvinceDisplayedObject(
                    ref mapMode,
                    ref pR,
                    requestComp.displayedObjectPE);

                //Изменяем параметры визуализации провинции
                //Если высота была обновлена
                if (MapModeData.UpdateProvinceHeight(
                    ref mapMode,
                    ref pR,
                    requestComp.height) == true)
                {
                    //Отмечаем, что высота обновлена
                    isHeightUpdated = true;
                }

                //Если индекс цвета был обновлён
                if (MapModeData.UpdateProvinceColorIndex(
                    ref mapMode,
                    ref pR,
                    requestComp.colorIndex) == true)
                {
                    //Отмечаем, что цвет обновлён
                    isColorUpdated = true;
                }

                //Удаляем запрос
                updateProvinceRenderSRPool.Value.Del(provinceEntity);
            }
        }
    }
}
