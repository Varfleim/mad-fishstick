
using UnityEngine;

namespace GS.UI
{
    public class UIGameWindow : MonoBehaviour
    {
        public GameObject activeMainPanel;

        public UIObjectPanel objectPanel;

        public void HideMainPanel()
        {
            //Скрываем активную главную панель
            activeMainPanel.SetActive(false);

            //Указываем, что активной главной панели нет
            activeMainPanel = null;
        }

        public void MainPanelActivation(
            GameObject requestedPanel,
            out bool isSamePanel)
        {
            //Значение по умолчанию отрицательно
            isSamePanel = false;

            //Если активна необходимая панель
            if(activeMainPanel == requestedPanel)
            {
                //Сообщаем, что была активна та же панель
                isSamePanel = true;
            }

            //Если была активна та же панель
            if(isSamePanel == true)
            {
                
            }
            //Иначе
            else
            {
                //Активируем запрошенную панель

                //Если какая-либо панель была активна, скрываем её
                if(activeMainPanel != null)
                {
                    activeMainPanel.SetActive(false);
                }

                //Делаем запрошенную панель активной
                requestedPanel.SetActive(true);

                //Указываем её как активную панель
                activeMainPanel = requestedPanel;
            }
        }
    }
}
