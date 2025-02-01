
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

                //Передаём запросы обновления визуализации провинций с владельцев земли
                LandOwnerTransferUpdateProvinceRenderRequests(ref mapMode);

                //Передаём запросы подсветки наведения с владельцев земли
                LandOwnerTransferHoverHighlightRequests(ref mapMode);
            }
        }

        readonly EcsFilterInject<Inc<CAgentLandOwner, SRUpdateProvinceRender>> aLandOwnerUpdateProvinceRenderSRFilter = default;
        readonly EcsPoolInject<SRUpdateProvinceRender> updateProvinceRenderSRPool = default;
        void LandOwnerTransferUpdateProvinceRenderRequests(
            ref CMapModeCore mapMode)
        {
            //Для каждого владельца земли с запросом обновления визуализации провинций
            foreach (int landOwnerEntity in aLandOwnerUpdateProvinceRenderSRFilter.Value)
            {
                //Берём владельца земли и запрос
                ref CAgentLandOwner aLandOwner = ref aLandOwnerPool.Value.Get(landOwnerEntity);
                ref SRUpdateProvinceRender requestComp = ref updateProvinceRenderSRPool.Value.Get(landOwnerEntity);

                //Для каждой владеемой земли
                foreach(EcsPackedEntity landPE in aLandOwner.ownedLandPEs)
                {
                    //Берём сущность
                    landPE.Unpack(world.Value, out int landEntity);

                    //Создаём запрос изменения визуализации для неё
                    MF.Map.MapModeData.UpdateProvinceRenderRequestFull(
                        updateProvinceRenderSRPool.Value,
                        //ref mapMode,
                        landEntity,
                        requestComp.displayedObjectPE,
                        requestComp.height,
                        requestComp.colorIndex);
                }

                //Удаляем запрос изменения визуализации с владельца земли
                updateProvinceRenderSRPool.Value.Del(landOwnerEntity);
            }
        }

        readonly EcsFilterInject<Inc<CAgentLandOwner, SRShowMapHoverHighlight>> aLandOwnerHoverHighlightSRFilter = default;
        readonly EcsPoolInject<SRShowMapHoverHighlight> showMapHoverHighlightSRPool = default;
        void LandOwnerTransferHoverHighlightRequests(
            ref CMapModeCore mapMode)
        {
            //Для каждого владельца земли с запросом подсветки наведения
            foreach (int landOwnerEntity in aLandOwnerHoverHighlightSRFilter.Value)
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
                        showMapHoverHighlightSRPool.Value,
                        ref mapMode,
                        landEntity);
                }

                //Удаляем запрос подсветки наведения с владельца земли
                showMapHoverHighlightSRPool.Value.Del(landOwnerEntity);
            }
        }
    }
}
