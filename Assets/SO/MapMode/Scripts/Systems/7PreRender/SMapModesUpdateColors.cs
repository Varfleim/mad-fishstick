
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Map;

namespace SO.MapMode
{
    public class SMapModesUpdateColors : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsCustomInject<MapModeData> mapModeData = default;

        public void Run(IEcsSystems systems)
        {
            //Первично обновляем списки цветов режимов карты
            MapModesCheckUpdateColorsListSecond();
        }

        readonly EcsFilterInject<Inc<RMapModeUpdateColorsListFirst>> mapModeUpdateColorsListFirstRFilter = default;
        readonly EcsPoolInject<RMapModeUpdateColorsListFirst> mapModeUpdateColorsListFirstRPool = default;
        readonly EcsPoolInject<RMapModeUpdateColorsListSecond> mapModeUpdateColorsListSecondRPool = default;
        void MapModesCheckUpdateColorsListSecond()
        {
            //Для каждого запроса первичного обновления списка цветов
            foreach(int requestEntity in mapModeUpdateColorsListFirstRFilter.Value)
            {
                //Берём запрос
                ref RMapModeUpdateColorsListFirst requestComp = ref mapModeUpdateColorsListFirstRPool.Value.Get(requestEntity);

                //Если запрашивается обновление списка цветов агентов
                if(requestComp.coloredObjectType == GS.Agent.AgentData.agentObjectType)
                {
                    //Запрашиваем вторичное обновление списка цветов политического режима карты
                    MF.Map.MapModeData.MapModeUpdateColorsListSecondRequest(
                        world.Value,
                        mapModeUpdateColorsListSecondRPool.Value,
                        mapModeData.Value.politicalMapModePE,
                        requestComp.objectColors, mapModeData.Value.politicalMapModeDefaultColor);
                }

                //Удаляем запрос
                mapModeUpdateColorsListFirstRPool.Value.Del(requestEntity);
            }
        }
    }
}
