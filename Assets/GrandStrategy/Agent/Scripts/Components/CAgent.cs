
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
        }

        public readonly EcsPackedEntity selfPE;
        public readonly string selfName;
    }
}
