
using Leopotam.EcsLite;

namespace SO.Colonization
{
    public struct CLandColony
    {
        public CLandColony(
            EcsPackedEntity selfPE)
        {
            this.selfPE = selfPE;

            ownerPE = new();

            colonizationProgress = 0;
        }

        public readonly EcsPackedEntity selfPE;

        public EcsPackedEntity ownerPE;

        public int ColonizationProgress
        {
            get
            {
                return colonizationProgress;
            }
        }
        int colonizationProgress;

        internal void AddProgress(
            int progressValue)
        {
            colonizationProgress += progressValue;
        }
    }
}
