
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Input;
using MF.Map;

namespace HS
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

                //Проверяем ввод в режимах карты
                MapModesInput();
            }
        }

        readonly EcsFilterInject<Inc<RMouseMapPositionCheck>> mouseMapPositionCheckFilter = default;
        readonly EcsPoolInject<RMouseMapPositionCheck> mouseMapPositionCheckPool = default;
        void MapModesInput()
        {
            //Обрабатываем ввод в стандартном режиме карты
            DefaultMapModeInput();
        }

        readonly EcsFilterInject<Inc<CMapModeCore, CDefaultMapMode, CActiveMapMode>> activeDefaultMapModeFilter = default;
        void DefaultMapModeInput()
        {
            //Для каждого активного стандартного режима карты
            foreach (int activeMapModeEntity in activeDefaultMapModeFilter.Value)
            {
                //Берём активный режим карты
                ref CMapModeCore activeMapMode = ref mapModeCorePool.Value.Get(activeMapModeEntity);

                //Для каждого запроса проверки положения курсора на карте
                foreach (int requestEntity in mouseMapPositionCheckFilter.Value)
                {
                    //Берём запрос
                    ref RMouseMapPositionCheck requestComp = ref mouseMapPositionCheckPool.Value.Get(requestEntity);

                    //Проверяем положение курсора
                    DefaultMapModeMousePositionCheck(
                        ref activeMapMode,
                        ref requestComp);
                    
                    //Удаляем запрос
                    mouseMapPositionCheckPool.Value.Del(requestEntity);
                }
            }
        }

        readonly EcsPoolInject<CProvinceRender> pRPool = default;
        void DefaultMapModeMousePositionCheck(
            ref CMapModeCore mapMode,
            ref RMouseMapPositionCheck requestComp)
        {
            //Берём провинцию из запроса
            requestComp.currentProvincePE.Unpack(world.Value, out int provinceEntity);
            ref CProvinceRender pR = ref pRPool.Value.Get(provinceEntity);

            pR.DisplayedObjectPE.Unpack(world.Value, out provinceEntity);

            //Запрашиваем для неё подсветку наведения
            MF.Map.MapModeData.ShowMapHoverHighlightRequest(
                showMapHoverHighlightSelfRequestPool.Value,
                ref mapMode,
                provinceEntity);
        }
    }
}
