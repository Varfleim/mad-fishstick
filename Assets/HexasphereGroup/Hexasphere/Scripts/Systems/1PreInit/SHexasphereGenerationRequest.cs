
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace HS
{
    public class SHexasphereGenerationRequest : IEcsInitSystem
    {
        readonly EcsCustomInject<HexasphereData> hexasphereData = default;

        public void Init(IEcsSystems systems)
        {
            //Запрашиваем генерацию гексасфер
            HexasphereGenerationRequests();
        }

        readonly EcsFilterInject<Inc<MF.Map.SRMapGeneration>> mapGenerationSelfRequestFilter = default;
        readonly EcsPoolInject<MF.Map.SRMapGeneration> mapGenerationSelfRequestPool = default;
        void HexasphereGenerationRequests()
        {
            //Для каждой карты с запросом генерации карты
            foreach(int mapEntity in mapGenerationSelfRequestFilter.Value)
            {
                //Берём запрос
                ref MF.Map.SRMapGeneration requestComp = ref mapGenerationSelfRequestPool.Value.Get(mapEntity);

                //Запрашиваем генерацию гексасферы
                HexasphereGenerationRequest(
                    mapEntity,
                    ref requestComp);

                //Удаляем запрос
                mapGenerationSelfRequestPool.Value.Del(mapEntity);
            }
        }

        readonly EcsPoolInject<SRHexasphereGeneration> hexasphereGenerationSelfRequestPool = default;
        void HexasphereGenerationRequest(
            int mapEntity,
            ref MF.Map.SRMapGeneration mapGenerationRequest)
        {
            //Назначаем сущности карты запрос генерации гексасферы
            ref SRHexasphereGeneration requestComp = ref hexasphereGenerationSelfRequestPool.Value.Add(mapEntity);

            //Заполняем данные запроса
            requestComp = new(
                mapGenerationRequest.mapIndex,
                hexasphereData.Value.subdivisions);
        }
    }
}
