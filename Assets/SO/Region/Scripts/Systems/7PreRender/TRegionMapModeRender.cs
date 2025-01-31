
using Leopotam.EcsLite;
using Leopotam.EcsLite.Threads;

using MF.Map;

namespace SO.Region
{
    public struct TRegionMapModeRender : IEcsThread<
        CRegionCore, SRUpdateProvinceRender,
        CMapModeCore>
    {
        public EcsWorld world;

        public MF.Map.MapModeData mainMapModeData;

        int[] regionEntities;

        CRegionCore[] rCPool;
        int[] rCIndices;

        SRUpdateProvinceRender[] updateProvinceRenderSRPool;
        int[] updateProvinceRenderSRIndices;

        CMapModeCore[] mapModePool;
        int[] mapModeIndices;

        public void Init(
            int[] entities,
            CRegionCore[] pool1, int[] indices1,
            SRUpdateProvinceRender[] pool2, int[] indices2,
            CMapModeCore[] pool3, int[] indices3)
        {
            regionEntities = entities;

            rCPool = pool1;
            rCIndices = indices1;

            updateProvinceRenderSRPool = pool2;
            updateProvinceRenderSRIndices = indices2;

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
                //Берём регион и запрос обновления визуализации провинци
                int regionEntity = regionEntities[a];
                ref CRegionCore rC = ref rCPool[rCIndices[regionEntity]];
                ref SRUpdateProvinceRender requestComp = ref updateProvinceRenderSRPool[updateProvinceRenderSRIndices[regionEntity]];

                //Изменяем запрос, задавая ему цвет региона
                MF.Map.MapModeData.UpdateProvinceRenderRequestUpdate(
                    ref mapModeCore,
                    ref requestComp,
                    rC.selfPE,
                    0.01f,
                    rC.ColorIndex);
            }
        }
    }
}
