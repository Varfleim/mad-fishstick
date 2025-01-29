
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace SO.Region
{
    public class SMapModesCreation : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<MF.Map.SRMapModeCreation> mapModeCreationSelfRequestPool = default;
        
        
        readonly EcsCustomInject<MapModeData> mapModeData = default;

        public void Init(IEcsSystems systems)
        {
            //Создаём режим карты регионов
            RegionMapModeCreation();
        }

        readonly EcsPoolInject<CRegionMapMode> regionMapModePool = default;
        void RegionMapModeCreation()
        {
            //Создаём новую сущность и назначаем ей компонент режима карты регионов
            int mapModeEntity = world.Value.NewEntity();
            ref CRegionMapMode regionMapMode = ref regionMapModePool.Value.Add(mapModeEntity);

            //Сохраняем PE режима карты регионов
            mapModeData.Value.regionMapModePE = world.Value.PackEntity(mapModeEntity);

            //Запрашиваем назначение главного компонента режима карты
            MF.Map.MapModeData.MapModeCreationRequest(
                mapModeCreationSelfRequestPool.Value,
                mapModeEntity, mapModeData.Value.regionMapModeName,
                false);
        }
    }
}
