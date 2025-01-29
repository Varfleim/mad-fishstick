
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace HS
{
    public class SMapModesCreation : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<MF.Map.SRMapModeCreation> mapModeCreationSelfRequestPool = default;

        readonly EcsPoolInject<MF.Map.RMapModeUpdateColorsListSecond> mapModeUpdateColorsListSecondRequestPool = default;


        readonly EcsCustomInject<MapModeData> mapModeData = default;

        public void Init(IEcsSystems systems)
        {
            //Создаём стандартный режим карты
            DefaultMapModeCreation();
        }

        readonly EcsPoolInject<CDefaultMapMode> defaultMapModePool = default;
        void DefaultMapModeCreation()
        {
            //Создаём новую сущность и назначаем ей компонент стандартного режима карты
            int mapModeEntity = world.Value.NewEntity();
            ref CDefaultMapMode defaultMapMode = ref defaultMapModePool.Value.Add(mapModeEntity);

            //Сохраняем PE стандартного режима карты
            mapModeData.Value.defaultMapModePE = world.Value.PackEntity(mapModeEntity);

            //Запрашиваем назначение главного компонента режима карты
            MF.Map.MapModeData.MapModeCreationRequest(
                mapModeCreationSelfRequestPool.Value,
                mapModeEntity, mapModeData.Value.defaultMapModeName,
                false);

            //Запрашиваем вторичное обновление списка цветов режима карты
            MF.Map.MapModeData.MapModeUpdateColorsListSecondRequest(
                world.Value,
                mapModeUpdateColorsListSecondRequestPool.Value,
                mapModeData.Value.defaultMapModePE,
                mapModeData.Value.defaultMapModeColors, mapModeData.Value.defaultMapModeDefaultColor);
        }
    }
}
