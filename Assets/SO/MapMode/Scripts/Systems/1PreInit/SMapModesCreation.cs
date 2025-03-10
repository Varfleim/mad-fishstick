
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace SO.MapMode
{
    public class SMapModesCreation : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<MF.Map.SRMapModeCreation> mapModeCreationSRPool = default;


        readonly EcsCustomInject<MapModeData> mapModeData = default;

        public void Init(IEcsSystems systems)
        {
            //Создаём политический режим карты
            PoliticalMapModeCreation();
        }

        readonly EcsPoolInject<CPoliticalMapMode> politicalMapModePool = default;
        void PoliticalMapModeCreation()
        {
            //Создаём новую сущность и назначаем ей компонент политического режима карты
            int mapModeEntity = world.Value.NewEntity();
            ref CPoliticalMapMode politicalMapMode = ref politicalMapModePool.Value.Add(mapModeEntity);

            //Сохраняем PE политического режима карты
            mapModeData.Value.politicalMapModePE = world.Value.PackEntity(mapModeEntity);

            //Запрашиваем назначение главного компонента режима карты
            MF.Map.MapModeData.MapModeCreationRequest(
                mapModeCreationSRPool.Value,
                mapModeEntity, mapModeData.Value.politicalMapModeName,
                true);
        }
    }
}
