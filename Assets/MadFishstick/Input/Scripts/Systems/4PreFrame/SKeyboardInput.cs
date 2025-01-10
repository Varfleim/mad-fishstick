
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MF.Input
{
    public class SKeyboardInput : IEcsRunSystem
    {
        readonly EcsCustomInject<InputData> inputData = default;

        public void Run(IEcsSystems systems)
        {
            //Обновляем состояние клавиш нампада
            UpdateKeypad();
        }

        void UpdateKeypad()
        {
            //Определяем состояние клавиш стрелок
            inputData.Value.rightArrowKeyPressed = UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightArrow);
            inputData.Value.leftArrowKeyPressed = UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftArrow);
            inputData.Value.upArrowKeyPressed = UnityEngine.Input.GetKey(UnityEngine.KeyCode.UpArrow);
            inputData.Value.downArrowKeyPressed = UnityEngine.Input.GetKey(UnityEngine.KeyCode.DownArrow);

            //Определяем состояние клавиш математических символов
            inputData.Value.keypadPlusPressed = UnityEngine.Input.GetKey(UnityEngine.KeyCode.KeypadPlus);
            inputData.Value.keypadMinusPressed = UnityEngine.Input.GetKey(UnityEngine.KeyCode.KeypadMinus);
        }
    }
}
