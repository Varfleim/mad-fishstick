
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace GS.UI
{
    public class SObjectPanelControl : IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<RObjectPanelHide>> objectPanelHideRFilter = default;
        readonly EcsPoolInject<RObjectPanelHide> objectPanelHideRPool = default;

        readonly EcsFilterInject<Inc<RObjectSubpanelTabShow>> objectSubpanelTabShowRFilter = default;
        readonly EcsPoolInject<RObjectSubpanelTabShow> objectSubpanelTabShowRPool = default;
        readonly EcsPoolInject<RObjectSubpanelTabUpdate> objectSubpanelTabUpdateRPool = default;


        readonly EcsCustomInject<UICore> uICore = default;

        public void Run(IEcsSystems systems)
        {
            //Отображаем вкладку подпанели объекта
            ShowObjectSubpanelTab();

            //Скрываем панель объекта
            HideObjectPanel();
        }

        void ShowObjectSubpanelTab()
        {
            //Для каждого запроса отображения вкладки подпанели объекта
            foreach(int requestEntity in objectSubpanelTabShowRFilter.Value)
            {
                //Берём запрос
                ref RObjectSubpanelTabShow requestComp = ref objectSubpanelTabShowRPool.Value.Get(requestEntity);

                //Отображаем вкладку
                ObjectSubpanelTabShow(
                    ref requestComp,
                    out bool isSamePanel,
                    out bool isSameSubpanel,
                    out bool isSameTab,
                    out bool isSameObject);

                //Запрашиваем обновление данных во вкладке
                UIData.UpdateObjectSubpanelTabRequest(
                    objectSubpanelTabUpdateRPool.Value,
                    requestEntity,
                    isSamePanel,
                    isSameSubpanel,
                    isSameTab,
                    isSameObject);
            }
        }

        /// <summary>
        /// Функция, активирующая запрошенную вкладку и проверяющая, была ли она уже активна и был ли активен запрошенный объект
        /// </summary>
        /// <param name="requestComp"></param>
        /// <param name="isSamePanel"></param>
        /// <param name="isSameSubpanel"></param>
        /// <param name="isSameTab"></param>
        /// <param name="isSameObject"></param>
        void ObjectSubpanelTabShow(
            ref RObjectSubpanelTabShow requestComp,
            out bool isSamePanel,
            out bool isSameSubpanel,
            out bool isSameTab,
            out bool isSameObject)
        {
            //Берём запрошенную подпанель объекта
            UIAObjectSubpanel objectSubpanel = UIData.objectSubpanels[requestComp.objectSubpanelType];

            //Активируем подпанель, если необходимо
            ObjectSubpanelShow(
                objectSubpanel,
                out isSamePanel,
                out isSameSubpanel);


            //Берём запрошенную вкладку
            UIAObjectSubpanelTab requestedTab = objectSubpanel.subpanelTabs[requestComp.objectSubpanelTabType];

            //Значения по умолчанию отрицательны
            isSameTab = false;
            isSameObject = false;

            //Если активна необходимая вкладка
            if (objectSubpanel.activeTab == requestedTab)
            {
                //Если была активна та же подпанель
                if(isSameSubpanel == true)
                {
                    //Сообщаем, что была активна та же вкладка
                    isSameTab = true;

                    //Если вкладка была активна для того же объекта
                    if(objectSubpanel.activeTab.objectPE.EqualsTo(requestComp.objectPE) == true)
                    {
                        //Сообщаем, что был активен тот же объект
                        isSameObject = true;
                    }
                }
            }

            //Если была активна та же панель
            if (isSamePanel == true)
            {

            }
            //Иначе
            else
            {

            }

            //Если была активна та же подпанель 
            if (isSameSubpanel == true)
            {

            }
            //Иначе
            else
            {

            }

            //Если была активна та же вкладка
            if (isSameTab == true)
            {

            }
            //Иначе
            else
            {
                //Активируем запрошенную вкладку
                objectSubpanel.tabGroup.OnTabSelected(requestedTab.selfTabButton);

                //Указываем её как активную вкладку
                objectSubpanel.activeTab = requestedTab;
            }

            //Если был активен тот же объект
            if (isSameObject == true)
            {

            }
            //Иначе
            else
            {
                //Указываем его как активный объект
                objectSubpanel.activeTab.objectPE = requestComp.objectPE;
            }
        }

        /// <summary>
        /// Функция, активирующая запрошенную подпанель и проверяющая, была ли она уже активирована
        /// </summary>
        /// <param name="requestedSubpanel"></param>
        /// <param name="isSamePanel"></param>
        /// <param name="isSameSubpanel"></param>
        void ObjectSubpanelShow(
            UIAObjectSubpanel requestedSubpanel,
            out bool isSamePanel,
            out bool isSameSubpanel)
        {
            //Берём панель объекта
            UIObjectPanel objectPanel = uICore.Value.gameWindow.objectPanel;

            //Активируем панель объекта, если необходимо
            uICore.Value.gameWindow.MainPanelActivation(
                objectPanel.gameObject,
                out isSamePanel);


            //Значение по умолчанию отрицательно
            isSameSubpanel = false;

            //Если активна необходимая подпанель
            if (objectPanel.activeSubpanel == requestedSubpanel)
            {
                //Если была активна та же панель
                if (isSamePanel == true)
                {
                    //Сообщаем, что активна та же подпанель
                    isSameSubpanel = true;
                }
            }

            //Если была активна та же панель
            if (isSamePanel == true)
            {

            }
            //Иначе
            else
            {

            }

            //Если была активна та же подпанель 
            if (isSameSubpanel == true)
            {

            }
            //Иначе
            else
            {
                //Активируем запрошенную подпанель

                //Если какая-либо подпанель была активна, скрываем её
                if (objectPanel.activeSubpanel != null)
                {
                    objectPanel.activeSubpanel.gameObject.SetActive(false);
                }

                //Делаем запрошенную подпанель активной
                requestedSubpanel.gameObject.SetActive(true);

                //Указываем её как активную подпанель
                objectPanel.activeSubpanel = requestedSubpanel;
            }
        }

        void HideObjectPanel()
        {
            //Для каждого запроса сокрытия панели объекта
            foreach(int requestEntity in objectPanelHideRFilter.Value)
            {
                //Берём запрос
                ref RObjectPanelHide requestComp = ref objectPanelHideRPool.Value.Get(requestEntity);

                //Скрываем панель
                ObjectPanelHide();

                //Удаляем запрос
                objectPanelHideRPool.Value.Del(requestEntity);
            }
        }

        void ObjectPanelHide()
        {
            //Берём панель объекта
            UIObjectPanel objectPanel = uICore.Value.gameWindow.objectPanel;

            //Скрываем активную вкладку активной подпанели
            objectPanel.activeSubpanel.HideActiveTab();

            //Скрываем активную подпанель
            objectPanel.HideActiveSubpanel();

            //Скрываем активную главную панель, то есть панель объекта
            uICore.Value.gameWindow.HideMainPanel();
        }
    }
}
