
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MF.Input
{
    public class SInputPreUpdate : IEcsRunSystem
    {
        readonly EcsCustomInject<InputData> inputData = default;

        public void Run(IEcsSystems systems)
        {
            //Обновляем состояние кнопок мыши
            UpdateMouseButtons();
        }

        void UpdateMouseButtons()
        {
            //Определяем состояние ЛКМ
            inputData.Value.leftMouseButtonClick = UnityEngine.Input.GetMouseButtonDown(0);
            inputData.Value.leftMouseButtonPressed = inputData.Value.leftMouseButtonClick || UnityEngine.Input.GetMouseButton(0);
            inputData.Value.leftMouseButtonRelease = UnityEngine.Input.GetMouseButtonUp(0);

            //Определяем состояние ПКМ
            inputData.Value.rightMouseButtonClick = UnityEngine.Input.GetMouseButtonDown(1);
            inputData.Value.rightMouseButtonPressed = inputData.Value.leftMouseButtonClick || UnityEngine.Input.GetMouseButton(1);
            inputData.Value.rightMouseButtonRelease = UnityEngine.Input.GetMouseButtonUp(1);
        }
    }
}
