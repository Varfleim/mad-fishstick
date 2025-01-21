
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

                //Передаём запросы изменения визуализации с регионов
                RegionTransferSetMapRenderValuesRequests(ref mapMode);

                //Передаём запросы подсветки наведения с регионов
                RegionTransferHoverHighlightRequests(ref mapMode);
            }
        }

        readonly EcsFilterInject<Inc<CRegionCore, SRSetMapRenderValues>> regionSetMapRenderValuesSelfRequestFilter = default;
        readonly EcsPoolInject<SRSetMapRenderValues> setMapRenderValuesSelfRequestPool = default;
        void RegionTransferSetMapRenderValuesRequests(
            ref CMapModeCore mapMode)
        {
            //Для каждого региона с запросом изменения визуализации 
            foreach (int regionEntity in regionSetMapRenderValuesSelfRequestFilter.Value)
            {
                //Берём регион и запрос
                ref CRegionCore rC = ref rCPool.Value.Get(regionEntity);
                ref SRSetMapRenderValues requestComp = ref setMapRenderValuesSelfRequestPool.Value.Get(regionEntity);

                //Для каждой провинции региона
                for (int a = 0; a < rC.provincePEs.Length; a++)
                {
                    //Берём сущность провинции
                    rC.provincePEs[a].Unpack(world.Value, out int provinceEntity);

                    //Создаём запрос изменения визуализации для неё
                    MF.Map.MapModeData.SetMapRenderValuesRequestFull(
                        setMapRenderValuesSelfRequestPool.Value,
                        ref mapMode,
                        provinceEntity,
                        requestComp.displayedObjectPE,
                        requestComp.height,
                        requestComp.colorIndex);
                }

                //Удаляем запрос изменения визуализации с региона
                setMapRenderValuesSelfRequestPool.Value.Del(regionEntity);
            }
        }

        readonly EcsFilterInject<Inc<CRegionCore, SRShowMapHoverHighlight>> regionHoverHighlightSelfRequestFilter = default;
        readonly EcsPoolInject<SRShowMapHoverHighlight> showMapHoverHighlightSelfRequestPool = default;
        void RegionTransferHoverHighlightRequests(
            ref CMapModeCore mapMode)
        {
            //Для каждого региона с запросом подсветки наведения
            foreach(int regionEntity in regionHoverHighlightSelfRequestFilter.Value)
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
                        showMapHoverHighlightSelfRequestPool.Value,
                        ref mapMode,
                        provinceEntity);
                }

                //Удаляем запрос подсветки наведения с региона
                showMapHoverHighlightSelfRequestPool.Value.Del(regionEntity);
            }
        }
    }
}
