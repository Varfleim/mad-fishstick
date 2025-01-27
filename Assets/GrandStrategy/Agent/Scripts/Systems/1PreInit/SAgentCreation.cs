
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace GS.Agent
{
    public class SAgentCreation : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CAgent> agentPool = default;

        readonly EcsFilterInject<Inc<SRAgentCreation>> agentCreationSelfRequestFilter = default;
        readonly EcsPoolInject<SRAgentCreation> agentCreationSelfRequestPool = default;

        public void Init(IEcsSystems systems)
        {
            //Создаём агентов
            AgentsCreation();
        }

        void AgentsCreation()
        {
            //Для каждого запроса создания агента
            foreach(int agentRequestEntity in agentCreationSelfRequestFilter.Value)
            {
                //Берём запрос
                ref SRAgentCreation requestComp = ref agentCreationSelfRequestPool.Value.Get(agentRequestEntity);

                //Создаём агента
                AgentCreation(
                    ref requestComp,
                    agentRequestEntity);

                //Берём агента
                ref CAgent agent = ref agentPool.Value.Get(agentRequestEntity);

                UnityEngine.Debug.LogWarning(agent.selfName);

                //Удаляем запрос
                agentCreationSelfRequestPool.Value.Del(agentRequestEntity);
            }
        }

        void AgentCreation(
            ref SRAgentCreation requestComp,
            int agentEntity)
        {
            //Назначаем переданной сущности компонент агента
            ref CAgent agent = ref agentPool.Value.Add(agentEntity);

            //Заполняем основные данные агента
            agent = new(
                world.Value.PackEntity(agentEntity), requestComp.agentName);
        }
    }
}
