
using Leopotam.EcsLite;
using Leopotam.EcsLite.Threads;

using MF.Map;
using GS.Agent;
using SO.LandOwnership;

namespace SO.MapMode
{
    public struct TPoliticalMapModeThreads : IEcsThread<
        CAgent, CAgentLandOwner, SRUpdateProvinceRender,
        CMapModeCore>
    {
        public EcsWorld world;

        public MF.Map.MapModeData mainMapModeData;

        int[] agentEntities;

        CAgent[] agentPool;
        int[] agentIndices;

        CAgentLandOwner[] aLandOwnerPool;
        int[] aLandOwnerIndices;

        SRUpdateProvinceRender[] updateProvinceRenderSRPool;
        int[] updateProvinceRenderSRIndices;

        CMapModeCore[] mapModePool;
        int[] mapModeIndices;

        public void Init(
            int[] entities,
            CAgent[] pool1, int[] indices1,
            CAgentLandOwner[] pool2, int[] indices2,
            SRUpdateProvinceRender[] pool3, int[] indices3,
            CMapModeCore[] pool4, int[] indices4)
        {
            agentEntities = entities;

            agentPool = pool1;
            agentIndices = indices1;

            aLandOwnerPool = pool2;
            aLandOwnerIndices = indices2;

            updateProvinceRenderSRPool = pool3;
            updateProvinceRenderSRIndices = indices3;

            mapModePool = pool4;
            mapModeIndices = indices4;
        }

        public void Execute(int threadId, int fromIndex, int beforeIndex)
        {
            //Берём активный режим карты
            mainMapModeData.activeMapModePE.Unpack(world, out int mapModeEntity);
            ref CMapModeCore mapModeCore = ref mapModePool[mapModeIndices[mapModeEntity]];

            for (int a = fromIndex; a < beforeIndex; a++)
            {
                //Берём владельца земли и запрос обновления визуализации провинций
                int agentEntity = agentEntities[a];
                ref CAgent agent = ref agentPool[agentIndices[agentEntity]];
                ref CAgentLandOwner aLandOwner = ref aLandOwnerPool[aLandOwnerIndices[agentEntity]];
                ref SRUpdateProvinceRender requestComp = ref updateProvinceRenderSRPool[updateProvinceRenderSRIndices[agentEntity]];

                //Изменяем запрос, задавая ему цвет агента
                MF.Map.MapModeData.UpdateProvinceRenderRequestUpdate(
                    ref mapModeCore,
                    ref requestComp,
                    aLandOwner.selfPE,
                    0.01f,
                    agent.ColorIndex);
            }
        }
    }
}
