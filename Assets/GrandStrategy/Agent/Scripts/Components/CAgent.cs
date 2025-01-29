
using Leopotam.EcsLite;

namespace GS.Agent
{
    public struct CAgent
    {
        public CAgent(
            EcsPackedEntity selfPE, string selfName)
        {
            this.selfPE = selfPE;
            this.selfName = selfName;

            colorIndex = -1;
        }

        public readonly EcsPackedEntity selfPE;
        public readonly string selfName;

        public int ColorIndex
        {
            get
            {
                return colorIndex;
            }
        }
        int colorIndex;

        public void SetColorIndex(
            int value)
        {
            colorIndex = value;
        }
    }
}
