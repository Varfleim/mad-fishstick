
using Leopotam.EcsLite;
using Leopotam.EcsLite.Threads;

using MF.Map;

namespace SO.Region
{
    public struct TRegionMapModeRender : IEcsThread<
        CRegionCore, SRSetMapRenderValues,
        CMapModeCore>
    {
        public EcsWorld world;

        public MF.Map.MapModeData mainMapModeData;

        int[] regionEntities;

        CRegionCore[] rCPool;
        int[] rCIndices;

        SRSetMapRenderValues[] setMapRenderValuesRequestPool;
        int[] setMapRenderValuesRequestIndices;

        CMapModeCore[] mapModePool;
        int[] mapModeIndices;

        public void Init(
            int[] entities,
            CRegionCore[] pool1, int[] indices1,
            SRSetMapRenderValues[] pool2, int[] indices2,
            CMapModeCore[] pool3, int[] indices3)
        {
            regionEntities = entities;

            rCPool = pool1;
            rCIndices = indices1;

            setMapRenderValuesRequestPool = pool2;
            setMapRenderValuesRequestIndices = indices2;

            mapModePool = pool3;
            mapModeIndices = indices3;
        }

        public void Execute(int threadId, int fromIndex, int beforeIndex)
        {
            //Берём активный режим карты
            mainMapModeData.activeMapModePE.Unpack(world, out int mapModeEntity);
            ref CMapModeCore mapModeCore = ref mapModePool[mapModeIndices[mapModeEntity]];

            for(int a = fromIndex; a < beforeIndex; a++)
            {
                //Берём регион и запрос изменения визуализации
                int regionEntity = regionEntities[a];
                ref CRegionCore rC = ref rCPool[rCIndices[regionEntity]];
                ref SRSetMapRenderValues requestComp = ref setMapRenderValuesRequestPool[setMapRenderValuesRequestIndices[regionEntity]];

                //Изменяем запрос изменения визуализации, задавая ему цвет региона
                MF.Map.MapModeData.SetMapRenderValuesRequestUpdate(
                    ref mapModeCore,
                    ref requestComp,
                    rC.selfPE,
                    0.01f,
                    rC.ColorIndex);
            }
        }
    }
}
