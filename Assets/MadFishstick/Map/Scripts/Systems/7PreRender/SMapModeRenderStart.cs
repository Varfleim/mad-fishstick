
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;

namespace MF.Map
{
    public class SMapModeRenderStart : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<EcsGroupSystemState> ecsGroupSystemStatePool = default;

        public void Run(IEcsSystems systems)
        {
            //Включаем системы визуализации режимов карты
            MapModesRenderSystemsActivation();
        }

        readonly EcsFilterInject<Inc<CMapModeCore, SRMapModeUpdate>> mapModeUpdateSRFilter = default;
        readonly EcsPoolInject<CMapModeCore> mapModeCorePool = default;
        void MapModesRenderSystemsActivation()
        {
            //Для каждого режима карты с запросом обновления
            foreach (int mapModeEntity in mapModeUpdateSRFilter.Value)
            {
                //Берём режим карты
                ref CMapModeCore mapMode = ref mapModeCorePool.Value.Get(mapModeEntity);

                //Создаём новую сущность и назначаем ей запрос переключения группы систем
                int requestEntity = world.Value.NewEntity();
                ref EcsGroupSystemState requestComp = ref ecsGroupSystemStatePool.Value.Add(requestEntity);

                //Заполняем данные запроса
                requestComp.Name = mapMode.selfName;
                requestComp.State = true;
            }
        }
    }
}
