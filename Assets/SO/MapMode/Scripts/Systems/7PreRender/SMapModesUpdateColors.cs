
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

        readonly EcsFilterInject<Inc<RMapModeUpdateColorsListFirst>> mapModeUpdateColorsListFirstRequestFilter = default;
        readonly EcsPoolInject<RMapModeUpdateColorsListFirst> mapModeUpdateColorsListFirstRequestPool = default;
        readonly EcsPoolInject<RMapModeUpdateColorsListSecond> mapModeUpdateColorsListSecondRequestPool = default;
        void MapModesCheckUpdateColorsListSecond()
        {
            //Для каждого запроса первичного обновления списка цветов
            foreach(int requestEntity in mapModeUpdateColorsListFirstRequestFilter.Value)
            {
                //Берём запрос
                ref RMapModeUpdateColorsListFirst requestComp = ref mapModeUpdateColorsListFirstRequestPool.Value.Get(requestEntity);

                //Если запрашивается обновление списка цветов агентов
                if(requestComp.coloredObjectType == GS.Agent.AgentData.agentObjectType)
                {
                    //Запрашиваем вторичное обновление списка цветов политического режима карты
                    MF.Map.MapModeData.MapModeUpdateColorsListSecondRequest(
                        world.Value,
                        mapModeUpdateColorsListSecondRequestPool.Value,
                        mapModeData.Value.politicalMapModePE,
                        requestComp.objectColors, mapModeData.Value.politicalMapModeDefaultColor);
                }

                //Удаляем запрос
                mapModeUpdateColorsListFirstRequestPool.Value.Del(requestEntity);
            }
        }
    }
}
