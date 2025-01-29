
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Threads;

using MF.Map;
using GS.Agent;
using SO.LandOwnership;

namespace SO.MapMode
{
    public class SMTPoliticalMapModeRender : EcsThreadSystem<TPoliticalMapModeRender,
        CAgent, CAgentLandOwner, SRSetMapRenderValues,
        CMapModeCore>
    {
        readonly EcsWorldInject world = default;

        readonly EcsCustomInject<MF.Map.MapModeData> mainMapModeData = default;

        protected override int GetChunkSize(IEcsSystems systems)
        {
            return 16;
        }

        protected override EcsWorld GetWorld(IEcsSystems systems)
        {
            return systems.GetWorld();
        }

        protected override EcsFilter GetFilter(EcsWorld world)
        {
            return world.Filter<CAgent>().Inc<CAgentLandOwner>().Inc<SRSetMapRenderValues>().End();
        }

        protected override void SetData(IEcsSystems systems, ref TPoliticalMapModeRender thread)
        {
            thread.world = world.Value;

            thread.mainMapModeData = mainMapModeData.Value;
        }
    }
}
