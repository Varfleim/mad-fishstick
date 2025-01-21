
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Input;
using MF.Map;

namespace SO.Region
{
    public class SMapModesInput : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsFilterInject<Inc<CMap, CActiveMap>> activeMapFilter = default;
        readonly EcsPoolInject<CMap> mapPool = default;

        readonly EcsPoolInject<CMapModeCore> mapModeCorePool = default;

        readonly EcsPoolInject<SRShowMapHoverHighlight> showMapHoverHighlightSelfRequestPool = default;

        public void Run(IEcsSystems systems)
        {
            //Для каждой активной карты
            foreach(int activeMapEntity in activeMapFilter.Value)
            {
                //Берём карту
                ref CMap activeMap = ref mapPool.Value.Get(activeMapEntity);

                //Обрабатываем ввод в режимах карты
                MapModesInput();
            }
        }

        readonly EcsFilterInject<Inc<RMouseMapPositionCheck>> mouseMapPositionCheckFilter = default;
        readonly EcsPoolInject<RMouseMapPositionCheck> mouseMapPositionCheckPool = default;
        void MapModesInput()
        {
            //Обрабатываем ввод в режиме карты регионов
            RegionMapModeInput();
        }

        readonly EcsFilterInject<Inc<CMapModeCore, CRegionMapMode, CActiveMapMode>> activeRegionMapModeFilter = default;
        void RegionMapModeInput()
        {
            //Для каждого активного режима карты регионов
            foreach(int regionEntity in activeRegionMapModeFilter.Value)
            {
                //Берём режим карты
                ref CMapModeCore mapMode = ref mapModeCorePool.Value.Get(regionEntity);

                //Для каждого запроса проверки положения курсора на карте
                foreach(int requestEntity in mouseMapPositionCheckFilter.Value)
                {
                    //Берём запрос
                    ref RMouseMapPositionCheck requestComp = ref mouseMapPositionCheckPool.Value.Get(requestEntity);

                    //Обрабатываем положение курсора
                    RegionMapModeMousePositionCheck(
                        ref mapMode,
                        ref requestComp);

                    //Удаляем запрос
                    mouseMapPositionCheckPool.Value.Del(requestEntity);
                }
            }
        }

        readonly EcsPoolInject<CProvinceRender> pRPool = default;
        void RegionMapModeMousePositionCheck(
            ref CMapModeCore mapMode,
            ref RMouseMapPositionCheck requestComp)
        {
            //Берём провинции из запроса
            requestComp.currentProvincePE.Unpack(world.Value, out int provinceEntity);
            ref CProvinceRender pR = ref pRPool.Value.Get(provinceEntity);

            //Берём отображаемый объект провинции
            pR.DisplayedObjectPE.Unpack(world.Value, out int regionEntity);

            //Запрашиваем для него подсветку наведения
            MF.Map.MapModeData.ShowMapHoverHighlightRequest(
                showMapHoverHighlightSelfRequestPool.Value,
                ref mapMode,
                regionEntity);
        }
    }
}
