
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace SO.Initialization
{
    public class SInitializationTest : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<SRMapInitialization> mapInitializationSRPool = default;

        readonly EcsPoolInject<SRAgentInitialization> agentInitializationSRPool = default;


        readonly EcsCustomInject<InitializerData> initializerData = default;

        public void Init(IEcsSystems systems)
        {
            //PE главной карты
            EcsPackedEntity mapPE = new();

            //Для каждой карты в списке
            for (int a = 0; a < initializerData.Value.mapNames.Length; a++)
            {
                //Создаём новую сущность и назначаем ей запрос инициализации карты
                int requestEntity = world.Value.NewEntity();
                ref SRMapInitialization requestComp = ref mapInitializationSRPool.Value.Add(requestEntity);

                //Заполняем данные запроса
                requestComp = new(
                    initializerData.Value.mapNames[a],
                    initializerData.Value.hexasphereSubdivisions,
                    initializerData.Value.averageProvincesPerRegion);

                mapPE = world.Value.PackEntity(requestEntity);
            }

            //Для каждого агента в списке
            for(int a = 0; a < initializerData.Value.agentNames.Length; a++)
            {
                //Создаём новую сущность и назначаем ей запрос инициализации агента
                int requestEntity = world.Value.NewEntity();
                ref SRAgentInitialization requestComp = ref agentInitializationSRPool.Value.Add(requestEntity);

                //Заполняем данные запроса
                requestComp = new(
                    initializerData.Value.agentNames[a],
                    mapPE);
            }
        }
    }
}
