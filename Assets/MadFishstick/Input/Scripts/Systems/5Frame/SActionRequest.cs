
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MF.Input
{
    public enum ActionType : byte
    {
        CameraRight,
        CameraLeft,
        CameraUp,
        CameraDown,
        CameraZoomIn,
        CameraZoomOut
    }

    public class SActionRequest : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;

        readonly EcsCustomInject<InputData> inputData = default;

        public void Run(IEcsSystems systems)
        {
            //Проверяем клавиши, связанные с нампадом
            KeypadActionRequests();
        }

        void KeypadActionRequests()
        {
            if(inputData.Value.rightArrowKeyPressed == true)
            {
                CheckActionType(ActionType.CameraRight);
            }

            if (inputData.Value.leftArrowKeyPressed == true)
            {
                CheckActionType(ActionType.CameraLeft);
            }

            if (inputData.Value.upArrowKeyPressed == true)
            {
                CheckActionType(ActionType.CameraUp);
            }

            if (inputData.Value.downArrowKeyPressed == true)
            {
                CheckActionType(ActionType.CameraDown);
            }

            if (inputData.Value.keypadPlusPressed == true)
            {
                CheckActionType(ActionType.CameraZoomIn);
            }

            if (inputData.Value.keypadMinusPressed == true)
            {
                CheckActionType(ActionType.CameraZoomOut);
            }
        }

        void CheckActionType(ActionType actionType)
        {
            switch(actionType)
            {
                case ActionType.CameraRight:
                    CameraMovingRequest(
                        true, false, false,
                        1);
                    break;
                case ActionType.CameraLeft:
                    CameraMovingRequest(
                        true, false, false,
                        -1);
                    break;
                case ActionType.CameraUp:
                    CameraMovingRequest(
                        false, true, false,
                        1);
                    break;
                case ActionType.CameraDown:
                    CameraMovingRequest(
                        false, true, false,
                        -1);
                    break;
                case ActionType.CameraZoomIn:
                    CameraMovingRequest(
                        false, false, true,
                        1);
                    break;
                case ActionType.CameraZoomOut:
                    CameraMovingRequest(
                        false, false, true,
                        -1);
                    break;
            }
        }

        readonly EcsPoolInject<RCameraMoving> cameraMovingRPool = default;
        void CameraMovingRequest(
            bool isHorizontal, bool isVertical, bool isZoom,
            float value)
        {
            //Если камера не заблокирована
            if(inputData.Value.isCameraBlocked == false)
            {
                //Создаём новую сущность и назначаем ей запрос движения камеры
                int requestEntity = world.Value.NewEntity();
                ref RCameraMoving requestComp = ref cameraMovingRPool.Value.Add(requestEntity);

                //Заполняем данные запроса
                requestComp = new RCameraMoving(
                    isHorizontal, isVertical, isZoom,
                    value);
            }
        }
    }
}
