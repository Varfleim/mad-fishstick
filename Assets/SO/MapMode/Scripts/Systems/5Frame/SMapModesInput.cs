
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Input;
using MF.Map;

namespace SO.MapMode
{
    public class SMapModesInput : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsFilterInject<Inc<CMap, CActiveMap>> activeMapFilter = default;
        readonly EcsPoolInject<CMap> mapPool = default;

        readonly EcsPoolInject<CMapModeCore> mapModeCorePool = default;

        readonly EcsPoolInject<CProvinceRender> pRPool = default;

        readonly EcsPoolInject<SRShowMapHoverHighlight> showMapHoverHighlightSelfRequestPool = default;

        public void Run(IEcsSystems systems)
        {
            //Для каждой активной карты
            foreach (int activeMapEntity in activeMapFilter.Value)
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
            //Обрабатываем ввод в политическом режиме карты
            LandOwnershipMapModeInput();
        }

        readonly EcsFilterInject<Inc<CMapModeCore, CPoliticalMapMode, CActiveMapMode>> activePoliticalMapModeFilter = default;
        void LandOwnershipMapModeInput()
        {
            //Для каждого активного политического режима карты 
            foreach (int mapModeEntity in activePoliticalMapModeFilter.Value)
            {
                //Берём режим карты
                ref CMapModeCore mapMode = ref mapModeCorePool.Value.Get(mapModeEntity);

                //Для каждого запроса проверки положения курсора на карте
                foreach (int requestEntity in mouseMapPositionCheckFilter.Value)
                {
                    //Берём запрос
                    ref RMouseMapPositionCheck requestComp = ref mouseMapPositionCheckPool.Value.Get(requestEntity);

                    //Обрабатываем положение курсора
                    PoliticalMapModeMousePositionCheck(
                        ref mapMode,
                        ref requestComp);

                    //Удаляем запрос
                    mouseMapPositionCheckPool.Value.Del(requestEntity);
                }
            }
        }

        void PoliticalMapModeMousePositionCheck(
            ref CMapModeCore mapMode,
            ref RMouseMapPositionCheck requestComp)
        {
            //Берём провинцию из запроса
            requestComp.currentProvincePE.Unpack(world.Value, out int provinceEntity);
            ref CProvinceRender pR = ref pRPool.Value.Get(provinceEntity);

            //Если отображаемый объект провинции не пуст
            if (pR.DisplayedObjectPE.Unpack(world.Value, out int landOwnerEntity))
            {
                //Запрашиваем для него подсветку наведения
                MF.Map.MapModeData.ShowMapHoverHighlightRequest(
                    showMapHoverHighlightSelfRequestPool.Value,
                    ref mapMode,
                    landOwnerEntity);
            }
        }
    }
}
