
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using GS.Agent;

namespace SO.Initialization
{
    public class SAgentInitialization : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;

        public void Init(IEcsSystems systems)
        {
            //Инициализируем агентов
            AgentsInitialization();
        }

        readonly EcsFilterInject<Inc<SRAgentInitialization>> agentInitializationSRFilter = default;
        readonly EcsPoolInject<SRAgentInitialization> agentInitializationSRPool = default;
        void AgentsInitialization()
        {
            //Для каждого запроса инициализации агента
            foreach(int agentRequestEntity in agentInitializationSRFilter.Value)
            {
                //Берём запрос
                ref SRAgentInitialization requestComp = ref agentInitializationSRPool.Value.Get(agentRequestEntity);

                //Инициализируем агента
                AgentInitialization(
                    ref requestComp,
                    agentRequestEntity);

                //Удаляем запрос
                agentInitializationSRPool.Value.Del(agentRequestEntity);
            }
        }

        readonly EcsPoolInject<SRAgentCreation> agentCreationSRPool = default;
        readonly EcsPoolInject<RRegionInitializationFirst> regionInitializationFirstRPool = default;
        void AgentInitialization(
            ref SRAgentInitialization requestComp,
            int agentEntity)
        {
            //Назначаем сущности запрос создания агента
            ref SRAgentCreation creationRequestComp = ref agentCreationSRPool.Value.Add(agentEntity);

            //Заполняем данные запроса
            creationRequestComp = new(
                requestComp.agentName);

            //Запрашиваем первичную инициализацию стартового региона агента
            InitializerData.RegionInitializationFirstRequest(
                world.Value,
                regionInitializationFirstRPool.Value,
                requestComp.parentMapPE,
                world.Value.PackEntity(agentEntity));
        }
    }
}
