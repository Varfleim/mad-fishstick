
namespace GS.Agent
{
    public readonly struct SRAgentCreation
    {
        public SRAgentCreation(
            string agentName)
        {
            this.agentName = agentName;
        }

        public readonly string agentName;
    }
}
