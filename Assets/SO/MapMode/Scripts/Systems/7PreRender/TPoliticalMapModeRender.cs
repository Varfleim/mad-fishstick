
using Leopotam.EcsLite;
using Leopotam.EcsLite.Threads;

using MF.Map;
using GS.Agent;
using SO.LandOwnership;

namespace SO.MapMode
{
    public struct TPoliticalMapModeRender : IEcsThread<
        CAgent, CAgentLandOwner, SRSetMapRenderValues,
        CMapModeCore>
    {
        public EcsWorld world;

        public MF.Map.MapModeData mainMapModeData;

        int[] agentEntities;

        CAgent[] agentPool;
        int[] agentIndices;

        CAgentLandOwner[] aLandOwnerPool;
        int[] aLandOwnerIndices;

        SRSetMapRenderValues[] setMapRenderValuesRequestPool;
        int[] setMapRenderValuesRequestIndices;

        CMapModeCore[] mapModePool;
        int[] mapModeIndices;

        public void Init(
            int[] entities,
            CAgent[] pool1, int[] indices1,
            CAgentLandOwner[] pool2, int[] indices2,
            SRSetMapRenderValues[] pool3, int[] indices3,
            CMapModeCore[] pool4, int[] indices4)
        {
            agentEntities = entities;

            agentPool = pool1;
            agentIndices = indices1;

            aLandOwnerPool = pool2;
            aLandOwnerIndices = indices2;

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

            for (int a = fromIndex; a < beforeIndex; a++)
            {
                //Берём владельца земли и запрос изменения визуализации
                int agentEntity = agentEntities[a];
                ref CAgent agent = ref agentPool[agentIndices[agentEntity]];
                ref CAgentLandOwner aLandOwner = ref aLandOwnerPool[aLandOwnerIndices[agentEntity]];
                ref SRSetMapRenderValues requestComp = ref setMapRenderValuesRequestPool[setMapRenderValuesRequestIndices[agentEntity]];

                //Изменяем запрос изменения визуализации, задавая ему цвет агента
                MF.Map.MapModeData.SetMapRenderValuesRequestUpdate(
                    ref mapModeCore,
                    ref requestComp,
                    aLandOwner.selfPE,
                    0.01f,
                    agent.ColorIndex);
            }
        }
    }
}
