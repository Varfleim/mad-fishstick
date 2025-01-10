
using Leopotam.EcsLite;
using Leopotam.EcsLite.Threads;

using MF.Map;

namespace HS
{
    public struct TDefaultMapModeRender : IEcsThread<
        CProvinceRender, CProvinceHexasphere, SRSetMapRenderValues,
        CMapModeCore>
    {
        public EcsWorld world;

        public MF.Map.MapModeData mainMapModeData;

        int[] provinceEntities;

        CProvinceRender[] pRPool;
        int[] pRIndices;

        CProvinceHexasphere[] pHSPool;
        int[] pHSIndices;

        SRSetMapRenderValues[] setMapRenderValuesRequestPool;
        int[] setMapRenderValuesRequestIndices;

        CMapModeCore[] mapModePool;
        int[] mapModeIndices;

        public void Init(
            int[] entities,
            CProvinceRender[] pool1, int[] indices1,
            CProvinceHexasphere[] pool2, int[] indices2,
            SRSetMapRenderValues[] pool3, int[] indices3,
            CMapModeCore[] pool4, int[] indices4)
        {
            provinceEntities = entities;

            pRPool = pool1;
            pRIndices = indices1;

            pHSPool = pool2;
            pHSIndices = indices2;

            setMapRenderValuesRequestPool = pool3;
            setMapRenderValuesRequestIndices = indices3;

            mapModePool = pool4;
            mapModeIndices = indices4;
        }

        public void Execute(int threadId, int fromIndex, int beforeIndex)
        {
            //Берём активный режим карты
            mainMapModeData.activeMapModePE.Unpack(world, out int mapModeEntity);
            ref CMapModeCore mapModeCore = ref mapModePool[mapModeIndices[mapModeEntity]];

            //Для каждой провинции в потоке
            for(int a = fromIndex; a < beforeIndex; a++)
            {
                //Берём провинцию и запрос изменения визуализации
                int provinceEntity = provinceEntities[a];
                ref CProvinceRender pR = ref pRPool[pRIndices[provinceEntity]];
                ref CProvinceHexasphere pHS = ref pHSPool[pHSIndices[provinceEntity]];
                ref SRSetMapRenderValues requestComp = ref setMapRenderValuesRequestPool[setMapRenderValuesRequestIndices[provinceEntity]];

                //Изменяем запрос изменения визуализации соответственно режиму карты
                MF.Map.MapModeData.SetMapRenderValuesRequestUpdate(
                    ref mapModeCore,
                    ref requestComp,
                    pHS.selfPE,
                    0.01f,
                    0);
            }
        }
    }
}
