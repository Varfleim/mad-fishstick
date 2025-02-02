
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Map;

namespace SO.Colonization
{
    public class SColoniesOwnerTransferRenderRequests : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsFilterInject<Inc<CMapModeCore, CActiveMapMode>> activeMapModeFilter = default;
        readonly EcsPoolInject<CMapModeCore> mapModePool = default;

        readonly EcsPoolInject<CAgentColoniesOwner> aColoniesOwnerPool = default;

        public void Run(IEcsSystems systems)
        {
            //Для каждого активного режима карты
            foreach(int mapModeEntity in activeMapModeFilter.Value)
            {
                //Берём режим карты
                ref CMapModeCore mapMode = ref mapModePool.Value.Get(mapModeEntity);

                //Передаём запросы обновления визуализации провинций с владельцев колоний
                ColoniesOwnerTransferUpdateProvinceRenderRequests(ref mapMode);

                //Передаём запросы подсветки наведения с владельцев колоний
                ColoniesOwnerTransferHoverHighlightRequests(ref mapMode);
            }
        }

        readonly EcsFilterInject<Inc<CAgentColoniesOwner, SRUpdateProvinceRender>> aColoniesOwnerUpdateProvinceRenderSRFilter = default;
        readonly EcsPoolInject<SRUpdateProvinceRender> updateProvinceRenderSRPool = default;
        void ColoniesOwnerTransferUpdateProvinceRenderRequests(
            ref CMapModeCore mapMode)
        {
            //Для каждого владельца колоний с запросом обновления визуализации провинций
            foreach (int coloniesOwnerEntity in aColoniesOwnerUpdateProvinceRenderSRFilter.Value)
            {
                //Берём владельца колоний и запрос
                ref CAgentColoniesOwner aColoniesOwner = ref aColoniesOwnerPool.Value.Get(coloniesOwnerEntity);
                ref SRUpdateProvinceRender requestComp = ref updateProvinceRenderSRPool.Value.Get(coloniesOwnerEntity);

                //Для каждой владеемой колонии
                foreach (EcsPackedEntity colonyPE in aColoniesOwner.ownedColonyPEs)
                {
                    //Берём сущность
                    colonyPE.Unpack(world.Value, out int colonyEntity);

                    //Создаём запрос изменения визуализации для неё
                    MapModeData.UpdateProvinceRenderRequestFull(
                        updateProvinceRenderSRPool.Value,
                        //ref mapMode,
                        colonyEntity,
                        requestComp.displayedObjectPE,
                        requestComp.height,
                        requestComp.colorIndex);
                }
            }
        }

        readonly EcsFilterInject<Inc<CAgentColoniesOwner, SRShowMapHoverHighlight>> aColoniesOwnerHoverHighlightSRFilter = default;
        readonly EcsPoolInject<SRShowMapHoverHighlight> showMapHoverHighlightSRPool = default;
        void ColoniesOwnerTransferHoverHighlightRequests(
            ref CMapModeCore mapMode)
        {
            //Для каждого владельца колоний с запросом подсветки наведения
            foreach (int coloniesOwnerEntity in aColoniesOwnerHoverHighlightSRFilter.Value)
            {
                //Берём владельца колоний
                ref CAgentColoniesOwner aColoniesOwner = ref aColoniesOwnerPool.Value.Get(coloniesOwnerEntity);

                //Для каждой владеемой колонии
                foreach (EcsPackedEntity colonyPE in aColoniesOwner.ownedColonyPEs)
                {
                    //Берём сущность
                    colonyPE.Unpack(world.Value, out int colonyEntity);

                    //Создаём запрос подсветки наведения для неё
                    MapModeData.ShowMapHoverHighlightRequest(
                        showMapHoverHighlightSRPool.Value,
                        ref mapMode,
                        colonyEntity);
                }
            }
        }
    }
}
