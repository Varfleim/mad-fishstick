
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Threads;

using MF.Map;

namespace SO.Region
{
    public class SMTRegionMapModeRender : EcsThreadSystem<TRegionMapModeRender,
        CRegionCore, SRUpdateProvinceRender,
        CMapModeCore>
    {
        readonly EcsWorldInject world = default;

        readonly EcsCustomInject<MF.Map.MapModeData> mainMapModeData = default;

        protected override int GetChunkSize(IEcsSystems systems)
        {
            return 128;
        }

        protected override EcsWorld GetWorld(IEcsSystems systems)
        {
            return systems.GetWorld();
        }

        protected override EcsFilter GetFilter(EcsWorld world)
        {
            return world.Filter<CRegionCore>().Inc<SRUpdateProvinceRender>().End();
        }

        protected override void SetData(IEcsSystems systems, ref TRegionMapModeRender thread)
        {
            thread.world = world.Value;

            thread.mainMapModeData = mainMapModeData.Value;
        }
    }
}
