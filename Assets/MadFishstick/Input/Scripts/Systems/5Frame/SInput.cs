
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MF.Input
{
    public class SInput : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<RMouseMapPositionCheck> mouseMapPositionCheckRequestPool = default;


        readonly EcsCustomInject<InputData> inputData = default;

        public void Run(IEcsSystems systems)
        {
            //Обновляем положение курсора мыши по запросу из подмодуля взаимодействия с картой
            MousePositionChangeRequests();
        }

        readonly EcsFilterInject<Inc<RMousePositionChange>> mousePositionChangeRequestFilter = default;
        readonly EcsPoolInject<RMousePositionChange> mousePositionChangeRequestPool = default;
        void MousePositionChangeRequests()
        {
            //Для каждого запроса изменения положения курсора
            foreach(int requestEntity in mousePositionChangeRequestFilter.Value)
            {
                //Берём запрос
                ref RMousePositionChange requestComp = ref mousePositionChangeRequestPool.Value.Get(requestEntity);

                //Обновляем положение курсора мыши
                MousePositionChangeRequest(ref requestComp);

                //Если курсор находится над картой
                if (inputData.Value.isMouseOverMap == true)
                {
                    //Запрашиваем проверку положения курсора на карте
                    InputData.MouseMapPositionCheckRequest(
                        world.Value,
                        mouseMapPositionCheckRequestPool.Value,
                        inputData.Value.lastHitProvincePE);
                }

                //Удаляем запрос
                mousePositionChangeRequestPool.Value.Del(requestEntity);
            }
        }

        void MousePositionChangeRequest(
            ref RMousePositionChange requestComp)
        {
            //Переносим данные из запроса
            inputData.Value.isMouseOverMap = requestComp.isMouseOverMap;
            inputData.Value.lastHitProvincePE = requestComp.lastHitProvincePE;
        }
    }
}
