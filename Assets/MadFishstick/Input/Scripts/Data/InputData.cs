
using UnityEngine;

using Leopotam.EcsLite;

namespace MF.Input
{
    internal class InputData : MonoBehaviour
    {
        #region Mouse
        public bool leftMouseButtonClick;
        public bool leftMouseButtonPressed;
        public bool leftMouseButtonRelease;

        public bool rightMouseButtonClick;
        public bool rightMouseButtonPressed;
        public bool rightMouseButtonRelease;

        public bool isMouseOverMap;
        public EcsPackedEntity lastHitProvincePE;

        public static void MouseMapPositionCheckRequest(
            EcsWorld world,
            EcsPool<RMouseMapPositionCheck> requestPool,
            EcsPackedEntity currentProvincePE)
        {
            //Создаём новую сущность и назначаем ей запрос
            int requestEntity = world.NewEntity();
            ref RMouseMapPositionCheck requestComp = ref requestPool.Add(requestEntity);

            //Заполняем данные запроса
            requestComp = new(
                currentProvincePE);
        }

        public static void MouseMapClickCheckRequest(
            EcsWorld world,
            EcsPool<RMouseMapClickCheck> requestPool,
            EcsPackedEntity currentProvincePE,
            bool leftMouseButtonClick, bool rightMouseButtonClick)
        {
            //Создаём новую сущность и назначаем ей запрос
            int requestEntity = world.NewEntity();
            ref RMouseMapClickCheck requestComp = ref requestPool.Add(requestEntity);

            //Заполняем данные запроса
            requestComp = new(
                currentProvincePE,
                leftMouseButtonClick, rightMouseButtonClick);
        }
        #endregion

        #region Keyboard
        public bool rightArrowKeyPressed;
        public bool leftArrowKeyPressed;
        public bool upArrowKeyPressed;
        public bool downArrowKeyPressed;

        public bool keypadPlusPressed;
        public bool keypadMinusPressed;
        #endregion

        #region Camera
        public bool isCameraBlocked = false;
        #endregion
    }
}
