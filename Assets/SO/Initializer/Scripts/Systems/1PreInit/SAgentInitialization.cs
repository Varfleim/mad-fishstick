
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using GS.Agent;

namespace SO.Initializer
{
    public class SAgentInitialization : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            //Инициализируем агентов
            AgentsInitialization();
        }

        readonly EcsFilterInject<Inc<SRAgentInitialization>> agentInitializationSelfRequestFilter = default;
        readonly EcsPoolInject<SRAgentInitialization> agentInitializationSelfRequestPool = default;
        void AgentsInitialization()
        {
            //Для каждого запроса инициализации агента
            foreach(int agentRequestEntity in agentInitializationSelfRequestFilter.Value)
            {
                //Берём запрос
                ref SRAgentInitialization requestComp = ref agentInitializationSelfRequestPool.Value.Get(agentRequestEntity);

                //Инициализируем агента
                AgentInitialization(
                    ref requestComp,
                    agentRequestEntity);

                //Удаляем запрос
                agentInitializationSelfRequestPool.Value.Del(agentRequestEntity);
            }
        }

        readonly EcsPoolInject<SRAgentCreation> agentCreationSelfRequestPool = default;
        void AgentInitialization(
            ref SRAgentInitialization requestComp,
            int agentEntity)
        {
            //Назначаем сущности запрос создания агента
            ref SRAgentCreation creationRequestComp = ref agentCreationSelfRequestPool.Value.Add(agentEntity);

            //Заполняем данные запроса
            creationRequestComp = new(
                requestComp.agentName);
        }
    }
}
