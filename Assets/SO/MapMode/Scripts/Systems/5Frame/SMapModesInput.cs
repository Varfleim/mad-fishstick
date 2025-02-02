
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Input;
using MF.Map;
using SO.Island;
using SO.LandOwnership;

namespace SO.MapMode
{
    public class SMapModesInput : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsFilterInject<Inc<CMap, CActiveMap>> activeMapFilter = default;
        readonly EcsPoolInject<CMap> mapPool = default;

        readonly EcsPoolInject<CMapModeCore> mapModeCorePool = default;

        readonly EcsPoolInject<CProvinceRender> pRPool = default;
        readonly EcsPoolInject<CIsland> islandPool = default;

        readonly EcsPoolInject<CAgentLandOwner> aLandOwnerPool = default;

        readonly EcsFilterInject<Inc<RMouseMapPositionCheck>> mouseMapPositionCheckRFilter = default;
        readonly EcsPoolInject<RMouseMapPositionCheck> mouseMapPositionCheckRPool = default;

        readonly EcsFilterInject<Inc<RMouseMapClickCheck>> mouseMapClickCheckRFilter = default;
        readonly EcsPoolInject<RMouseMapClickCheck> mouseMapClickCheckRPool = default;

        readonly EcsPoolInject<SRShowMapHoverHighlight> showMapHoverHighlightSRPool = default;


        readonly EcsCustomInject<MapModeData> mapModeData = default;

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
                foreach (int requestEntity in mouseMapPositionCheckRFilter.Value)
                {
                    //Берём запрос
                    ref RMouseMapPositionCheck requestComp = ref mouseMapPositionCheckRPool.Value.Get(requestEntity);

                    //Обрабатываем положение курсора
                    PoliticalMapModeMousePositionCheck(
                        ref mapMode,
                        ref requestComp);

                    //Удаляем запрос
                    mouseMapPositionCheckRPool.Value.Del(requestEntity);
                }

                //Для каждого запроса клика по карте
                foreach(int requestEntity in mouseMapClickCheckRFilter.Value)
                {
                    //Берём запрос
                    ref RMouseMapClickCheck requestComp = ref mouseMapClickCheckRPool.Value.Get(requestEntity);

                    //Обрабатываем клики по карте
                    PoliticalMapModeMouseClickCheck(
                        ref mapMode,
                        ref requestComp);

                    //Удаляем запрос
                    mouseMapClickCheckRPool.Value.Del(requestEntity);
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
                    showMapHoverHighlightSRPool.Value,
                    ref mapMode,
                    landOwnerEntity);
            }
            //Иначе, если провинция имеет компонент острова
            else if(islandPool.Value.Has(provinceEntity)) 
            {
                //Запрашиваем для неё подсветку наведения
                MF.Map.MapModeData.ShowMapHoverHighlightRequest(
                    showMapHoverHighlightSRPool.Value,
                    ref mapMode,
                    provinceEntity);
            }
        }

        readonly EcsPoolInject<SO.Colonization.RLandColonize> landColonizeRPool = default;
        void PoliticalMapModeMouseClickCheck(
            ref CMapModeCore mapMode,
            ref RMouseMapClickCheck requestComp)
        {
            //Берём провинцию из запроса
            requestComp.currentProvincePE.Unpack(world.Value, out int provinceEntity);
            ref CProvinceRender pR = ref pRPool.Value.Get(provinceEntity);

            //Если отображаемый объект провинции не пуст
            if (pR.DisplayedObjectPE.Unpack(world.Value, out int landOwnerEntity))
            {
                //Берём владельца земли
                ref CAgentLandOwner aLandOwner = ref aLandOwnerPool.Value.Get(landOwnerEntity);

                UnityEngine.Debug.LogWarning(aLandOwner.ownedLandPEs.Count + " !");
            }
            //Иначе, если провинция имеет компонент острова
            else if (islandPool.Value.Has(provinceEntity))
            {
                //Запрашиваем для неё колонизацию
                SO.Colonization.ColonizationData.LandColonizeRequest(
                    world.Value,
                    landColonizeRPool.Value,
                    mapModeData.Value.lastAgentPE,
                    requestComp.currentProvincePE);
            }
        }
    }
}
