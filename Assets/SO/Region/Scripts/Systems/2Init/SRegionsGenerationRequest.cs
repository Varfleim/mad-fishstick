
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace SO.Region
{
    public class SRegionsGenerationRequest : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            //Запрашиваем генерацию регионов
            RegionsGenerationRequests();
        }

        readonly EcsFilterInject<Inc<MF.Map.SRMapGeneration>> mapGenerationSelfRequestFilter = default;
        readonly EcsPoolInject<MF.Map.SRMapGeneration> mapGenerationSelfRequestPool = default;
        void RegionsGenerationRequests()
        {
            //Для каждой карты с запросом генерации карты
            foreach (int mapEntity in mapGenerationSelfRequestFilter.Value)
            {
                //Берём запрос
                ref MF.Map.SRMapGeneration requestComp = ref mapGenerationSelfRequestPool.Value.Get(mapEntity);

                //Запрашиваем генерацию регионов
                RegionsGenerationRequest(
                    mapEntity,
                    ref requestComp);

                //Удаляем запрос
                mapGenerationSelfRequestPool.Value.Del(mapEntity);
            }
        }

        readonly EcsPoolInject<SRRegionsGeneration> regionsGenerationSelfRequestPool = default;
        void RegionsGenerationRequest(
            int mapEntity,
            ref MF.Map.SRMapGeneration mapGenerationRequest)
        {
            //Назначаем сущности карты запрос генерации регионов
            ref SRRegionsGeneration requestComp = ref regionsGenerationSelfRequestPool.Value.Add(mapEntity);

            //Заполняем данные запроса
            requestComp = new(
                50);
        }
    }
}
