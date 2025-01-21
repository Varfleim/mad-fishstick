
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MF.Map
{
    public class SMainMapModesCreation : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;

        readonly EcsCustomInject<MapModeData> mapModeData = default;

        public void Init(IEcsSystems systems)
        {
            //Создаём режимы карты по запросам
            MapModesCreation();
        }

        readonly EcsFilterInject<Inc<SRMapModeCreation>> mapModeCreationSelfRequestFilter = default;
        readonly EcsPoolInject<SRMapModeCreation> mapModeCreationSelfRequestPool = default;
        void MapModesCreation()
        {
            //Для каждого запроса создания режима карты
            foreach(int mapModeEntity in mapModeCreationSelfRequestFilter.Value)
            {
                //Берём запрос
                ref SRMapModeCreation requestComp = ref mapModeCreationSelfRequestPool.Value.Get(mapModeEntity);

                //Создаём режим карты по запросу
                MapModeCreation(
                    mapModeEntity,
                    ref requestComp);

                //Берём режим карты
                ref CMapModeCore mapMode = ref mapModeCorePool.Value.Get(mapModeEntity);

                //Если данный режим карты указан как стандартный
                if(requestComp.defaultMapMode == true)
                {
                    //Сохраняем его как стандартный режим карты
                    mapModeData.Value.defaultMapModePE = mapMode.selfPE;
                }

                //Удаляем запрос
                mapModeCreationSelfRequestPool.Value.Del(mapModeEntity);
            }
        }

        readonly EcsPoolInject<CMapModeCore> mapModeCorePool = default;
        void MapModeCreation(
            int mapModeEntity,
            ref SRMapModeCreation requestComp)
        {
            //Назначаем сущности режима карты компонент режима карты
            ref CMapModeCore mapMode = ref mapModeCorePool.Value.Add(mapModeEntity);

            //Заполняем основные данные режима
            mapMode = new(
                world.Value.PackEntity(mapModeEntity), requestComp.name);
        }
    }
}
