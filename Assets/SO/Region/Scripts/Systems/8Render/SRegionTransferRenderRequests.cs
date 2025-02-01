
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Map;

namespace SO.Region
{
    public class SRegionTransferRenderRequests : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsFilterInject<Inc<CMapModeCore, CActiveMapMode>> activeMapModeFilter = default;
        readonly EcsPoolInject<CMapModeCore> mapModePool = default;

        readonly EcsPoolInject<CRegionCore> rCPool = default;

        public void Run(IEcsSystems systems)
        {
            //Для каждого активного режима карты
            foreach (int mapModeEntity in activeMapModeFilter.Value)
            {
                //Берём режим карты
                ref CMapModeCore mapMode = ref mapModePool.Value.Get(mapModeEntity);

                //Передаём запросы обновления тонких граней
                RegionTransferUpdateThinEdgesRequests();

                //Передаём запросы обновления визуализации провинций
                RegionTransferUpdateProvinceRenderRequests(ref mapMode);

                //Передаём запросы подсветки наведения с регионов
                RegionTransferHoverHighlightRequests(ref mapMode);
            }
        }

        readonly EcsFilterInject<Inc<CRegionCore, SRUpdateThinEdges>> regionUpdateThinEdgesSRFilter = default;
        readonly EcsPoolInject<SRUpdateThinEdges> updateThinEdgesSRPool = default;
        void RegionTransferUpdateThinEdgesRequests()
        {
            //Для каждого региона с запросом обновления тонких граней
            foreach(int regionEntity in regionUpdateThinEdgesSRFilter.Value)
            {
                //Берём регион и запрос
                ref CRegionCore rC = ref rCPool.Value.Get(regionEntity);
                ref SRUpdateThinEdges requestComp = ref updateThinEdgesSRPool.Value.Get(regionEntity);

                //Для каждой провинции региона
                for(int a = 0; a < rC.provincePEs.Length; a++)
                {
                    //Берём сущность провинции
                    rC.provincePEs[a].Unpack(world.Value, out int provinceEntity);

                    //Создаём запрос обновления тонких граней для неё
                    MF.Map.MapModeData.UpdateThinEdgesRequest(
                        updateThinEdgesSRPool.Value,
                        provinceEntity,
                        requestComp.edgeIndex);
                }

                //Удаляем запрос с региона
                updateThinEdgesSRPool.Value.Del(regionEntity);
            }
        }

        readonly EcsFilterInject<Inc<CRegionCore, SRUpdateProvinceRender>> regionUpdateProvinceRenderValuesSRFilter = default;
        readonly EcsPoolInject<SRUpdateProvinceRender> updateProvinceRenderValuesSRPool = default;
        void RegionTransferUpdateProvinceRenderRequests(
            ref CMapModeCore mapMode)
        {
            //Для каждого региона с запросом обновления визуализации провинций 
            foreach (int regionEntity in regionUpdateProvinceRenderValuesSRFilter.Value)
            {
                //Берём регион и запрос
                ref CRegionCore rC = ref rCPool.Value.Get(regionEntity);
                ref SRUpdateProvinceRender requestComp = ref updateProvinceRenderValuesSRPool.Value.Get(regionEntity);

                //Для каждой провинции региона
                for (int a = 0; a < rC.provincePEs.Length; a++)
                {
                    //Берём сущность провинции
                    rC.provincePEs[a].Unpack(world.Value, out int provinceEntity);

                    //Создаём запрос обновления визуализации для неё
                    MF.Map.MapModeData.UpdateProvinceRenderRequestFull(
                        updateProvinceRenderValuesSRPool.Value,
                        //ref mapMode,
                        provinceEntity,
                        requestComp.displayedObjectPE,
                        requestComp.height,
                        requestComp.colorIndex);
                }

                //Удаляем запрос с региона
                updateProvinceRenderValuesSRPool.Value.Del(regionEntity);
            }
        }

        readonly EcsFilterInject<Inc<CRegionCore, SRShowMapHoverHighlight>> regionHoverHighlightSRFilter = default;
        readonly EcsPoolInject<SRShowMapHoverHighlight> showMapHoverHighlightSRPool = default;
        void RegionTransferHoverHighlightRequests(
            ref CMapModeCore mapMode)
        {
            //Для каждого региона с запросом подсветки наведения
            foreach(int regionEntity in regionHoverHighlightSRFilter.Value)
            {
                //Берём регион
                ref CRegionCore rC = ref rCPool.Value.Get(regionEntity);

                //Для каждой провинции региона
                for(int a = 0; a < rC.provincePEs.Length; a++)
                {
                    //Берём сущность провинции
                    rC.provincePEs[a].Unpack(world.Value, out int provinceEntity);

                    //Создаём запрос подсветки наведения для неё
                    MF.Map.MapModeData.ShowMapHoverHighlightRequest(
                        showMapHoverHighlightSRPool.Value,
                        ref mapMode,
                        provinceEntity);
                }

                //Удаляем запрос подсветки наведения с региона
                showMapHoverHighlightSRPool.Value.Del(regionEntity);
            }
        }
    }
}
