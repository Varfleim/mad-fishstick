
using Leopotam.EcsLite;
using Leopotam.EcsLite.Threads;

namespace SO.Colonization
{
    public class SMTColonizationProgressCalc : EcsThreadSystem<TColonizationProgressCalc,
        CLandColony>
    {
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
            return world.Filter<CLandColony>().End();
        }

        protected override void SetData(IEcsSystems systems, ref TColonizationProgressCalc thread)
        {

        }
    }
}
