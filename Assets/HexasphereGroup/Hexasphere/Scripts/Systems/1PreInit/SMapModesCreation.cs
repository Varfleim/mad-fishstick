
using System.Collections.Generic;

using UnityEngine;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace HS
{
    public class SMapModesCreation : IEcsInitSystem
    {
        readonly EcsWorldInject world = default;


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

            //Запрашиваем назначение главного компонента режима карты
            MapModeCreationRequest(
                mapModeEntity, mapModeData.Value.defaultMapModeName,
                mapModeData.Value.defaultMapModeColors,
                true);
        }

        readonly EcsPoolInject<MF.Map.SRMapModeCreation> mapModeCreationSelfRequestPool = default;
        void MapModeCreationRequest(
            int mapModeEntity, string mapModeName,
            List<Color> mapModeColors,
            bool defaultMapMode)
        {
            //Назначаем сущности запрос создания режима карты
            ref MF.Map.SRMapModeCreation requestComp = ref mapModeCreationSelfRequestPool.Value.Add(mapModeEntity);

            //Заполняем данные запроса
            requestComp = new(
                mapModeName,
                mapModeColors,
                defaultMapMode);
        }
    }
}
