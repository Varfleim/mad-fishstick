
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;

namespace MF.Map
{
    public class SMapModeRenderEnd : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<EcsGroupSystemState> ecsGroupSystemStatePool = default;

        public void Run(IEcsSystems systems)
        {
            //Выключаем системы визуализации режимов карты
            MapModesRenderSystemsDeactivation();
        }

        readonly EcsFilterInject<Inc<CMapModeCore, SRMapModeUpdate>> mapModeUpdateSRFilter = default;
        readonly EcsPoolInject<CMapModeCore> mapModeCorePool = default;
        readonly EcsPoolInject<SRMapModeUpdate> mapModeUpdateSRPool = default;
        void MapModesRenderSystemsDeactivation()
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
                requestComp.State = false;

                //Удаляем запрос обновления режима карты
                mapModeUpdateSRPool.Value.Del(mapModeEntity);
            }
        }
    }
}
