
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.Unity.Ugui;

namespace GS.UI
{
    public class SObjectPanelInput : IEcsInitSystem, IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<RObjectPanelHide> objectPanelHideRPool = default;

        EcsWorld uguiUIWorld;
        EcsFilter clickEventUIFilter;
        EcsPool<EcsUguiClickEvent> clickEventUIPool;


        readonly EcsCustomInject<UICore> uICore = default;

        public void Init(IEcsSystems systems)
        {
            uguiUIWorld = systems.GetWorld("uguiUIEventsWorld");

            clickEventUIPool = uguiUIWorld.GetPool<EcsUguiClickEvent>();
            clickEventUIFilter = uguiUIWorld.Filter<EcsUguiClickEvent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            //Проверяем клики в панели объекта
            ObjectPanelClickAction();
        }

        void ObjectPanelClickAction()
        {
            //Берём окно игры
            UIGameWindow gameWindow = uICore.Value.gameWindow;

            //Если активна панель объекта
            if (gameWindow.activeMainPanel == gameWindow.objectPanel.gameObject)
            {
                //Для каждого события клика по интерфейсу
                foreach (int clickEventUIEntity in clickEventUIFilter)
                {
                    //Берём событие
                    ref EcsUguiClickEvent clickEvent = ref clickEventUIPool.Get(clickEventUIEntity);

                    //Проверяем, было ли совершено какое-либо действие
                    bool isActionComplete = false;

                    //Если нажата кнопка закрытия панели объекта
                    if (clickEvent.WidgetName == "CloseObjectPanel")
                    {
                        //Запрашиваем сокрытие панели объекта
                        UIData.HideObjectPanelRequest(
                            world.Value,
                            objectPanelHideRPool.Value);

                        //Отмечаем, что действие было совершено
                        isActionComplete = true;
                    }

                    //Если действие было совершено
                    if (isActionComplete == true)
                    {
                        UnityEngine.Debug.LogWarning("Click! " + clickEvent.WidgetName);

                        //Удаляем событие
                        clickEventUIPool.Del(clickEventUIEntity);
                    }
                }
            }
        }
    }
}
