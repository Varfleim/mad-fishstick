
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MF.Input;

namespace HS
{
    public class SICameraMovingRequest : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;

        public void Run(IEcsSystems systems)
        {
            //Преобразуем запрос движения камеры
            CameraMovingRequestTransform();
        }

        readonly EcsFilterInject<Inc<RCameraMoving>> cameraMovingRequestFilter = default;
        readonly EcsPoolInject<RCameraMoving> cameraMovingRequestPool = default;
        void CameraMovingRequestTransform()
        {
            //Для каждого запроса движения камеры
            foreach (int requestEntity in cameraMovingRequestFilter.Value)
            {
                //Берём запрос
                ref RCameraMoving requestComp = ref cameraMovingRequestPool.Value.Get(requestEntity);

                //Запрашиваем движения камеры гексасферы
                HexasphereCameraMovingRequest(
                    requestComp.isHorizontal, requestComp.isVertical, requestComp.isZoom,
                    requestComp.value);

                //Удаляем запрос
                cameraMovingRequestPool.Value.Del(requestEntity);
            }
        }

        readonly EcsPoolInject<RHexasphereCameraMoving> hexasphereCameraMovingRequestPool = default;
        void HexasphereCameraMovingRequest(
            bool isHorizontal, bool isVertical, bool isZoom,
            float value)
        {
            //Создаём новую сущность и назначаем ей запрос движения камеры гексасферы
            int requestEntity = world.Value.NewEntity();
            ref RHexasphereCameraMoving requestComp = ref hexasphereCameraMovingRequestPool.Value.Add(requestEntity);

            //Заполняем данные запроса
            requestComp = new(
                isHorizontal, isVertical, isZoom,
                value);
        }
    }
}
