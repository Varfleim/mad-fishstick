
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Map;

namespace SO.LandOwnership
{
    public class SLandOwnerTransferRenderRequests : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsFilterInject<Inc<CMapModeCore, CActiveMapMode>> activeMapModeFilter = default;
        readonly EcsPoolInject<CMapModeCore> mapModePool = default;

        readonly EcsPoolInject<CAgentLandOwner> aLandOwnerPool = default;

        public void Run(IEcsSystems systems)
        {
            //Для каждого активного режима карты
            foreach(int mapModeEntity in activeMapModeFilter.Value)
            {
                //Берём режим карты
                ref CMapModeCore mapMode = ref mapModePool.Value.Get(mapModeEntity);

                //Передаём запросы изменения визуализации с владельцев земли
                LandOwnerTransferSetMapRenderValuesRequests(ref mapMode);

                //Передаём запросы подсветки наведения с владельцев земли
                LandOwnerTransferHoverHighlightRequests(ref mapMode);
            }
        }

        readonly EcsFilterInject<Inc<CAgentLandOwner, SRSetMapRenderValues>> aLandOwnerSetMapRenderValuesSelfRequestFilter = default;
        readonly EcsPoolInject<SRSetMapRenderValues> setMapRenderValuesSelfRequestPool = default;
        void LandOwnerTransferSetMapRenderValuesRequests(
            ref CMapModeCore mapMode)
        {
            //Для каждого владельца земли с запросом изменения визуализации 
            foreach (int landOwnerEntity in aLandOwnerSetMapRenderValuesSelfRequestFilter.Value)
            {
                //Берём владельца земли и запрос
                ref CAgentLandOwner aLandOwner = ref aLandOwnerPool.Value.Get(landOwnerEntity);
                ref SRSetMapRenderValues requestComp = ref setMapRenderValuesSelfRequestPool.Value.Get(landOwnerEntity);

                //Для каждой владеемой земли
                foreach(EcsPackedEntity landPE in aLandOwner.ownedLandPEs)
                {
                    //Берём сущность
                    landPE.Unpack(world.Value, out int landEntity);

                    //Создаём запрос изменения визуализации для неё
                    MF.Map.MapModeData.SetMapRenderValuesRequestFull(
                        setMapRenderValuesSelfRequestPool.Value,
                        ref mapMode,
                        landEntity,
                        requestComp.displayedObjectPE,
                        requestComp.height,
                        requestComp.colorIndex);
                }

                //Удаляем запрос изменения визуализации с владельца земли
                setMapRenderValuesSelfRequestPool.Value.Del(landOwnerEntity);
            }
        }

        readonly EcsFilterInject<Inc<CAgentLandOwner, SRShowMapHoverHighlight>> aLandOwnerHoverHighlightSelfRequestFilter = default;
        readonly EcsPoolInject<SRShowMapHoverHighlight> showMapHoverHighlightSelfRequestPool = default;
        void LandOwnerTransferHoverHighlightRequests(
            ref CMapModeCore mapMode)
        {
            //Для каждого владельца земли с запросом подсветки наведения
            foreach (int landOwnerEntity in aLandOwnerHoverHighlightSelfRequestFilter.Value)
            {
                //Берём владельца земли
                ref CAgentLandOwner aLandOwner = ref aLandOwnerPool.Value.Get(landOwnerEntity);

                //Для каждой владеемой земли
                foreach (EcsPackedEntity landPE in aLandOwner.ownedLandPEs)
                {
                    //Берём сущность
                    landPE.Unpack(world.Value, out int landEntity);

                    //Создаём запрос подсветки наведения для неё
                    MF.Map.MapModeData.ShowMapHoverHighlightRequest(
                        showMapHoverHighlightSelfRequestPool.Value,
                        ref mapMode,
                        landEntity);
                }

                //Удаляем запрос подсветки наведения с владельца земли
                showMapHoverHighlightSelfRequestPool.Value.Del(landOwnerEntity);
            }
        }
    }
}
