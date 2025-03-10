
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using GS.UI;
using SO.Island;

namespace SO.GameUI
{
    public class SIslandObjectSubpanelUpdate : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CIsland> islandPool = default;

        readonly EcsFilterInject<Inc<RObjectSubpanelTabShow, RObjectSubpanelTabUpdate>> objectSubpanelTabUpdateRFilter = default;
        readonly EcsPoolInject<RObjectSubpanelTabShow> objectSubpanelTabShowRPool = default;
        readonly EcsPoolInject<RObjectSubpanelTabUpdate> objectSubpanelTabUpdateRPool = default;


        readonly EcsCustomInject<UICore> uICore = default;

        readonly EcsCustomInject<GameUIData> gameUIData = default;

        public void Run(IEcsSystems systems)
        {
            //Проверяем, не требуется ли обновить подпанель острова
            UpdateIslandObjectSubpanel();
        }

        void UpdateIslandObjectSubpanel()
        {
            //Берём панель объекта
            UIObjectPanel objectPanel = uICore.Value.gameWindow.objectPanel;

            //Если активна подпанель острова
            if(objectPanel.activeSubpanel == gameUIData.Value.islandObjectSubpanel)
            {
                //Для каждого запроса обновления вкладки подпанели объекта
                foreach (int requestEntity in objectSubpanelTabUpdateRFilter.Value)
                {
                    //Берём запрос отображения
                    ref RObjectSubpanelTabShow showRequestComp = ref objectSubpanelTabShowRPool.Value.Get(requestEntity);

                    //Если запрашивается обновление подпанели острова
                    if(showRequestComp.objectSubpanelType == GameUIData.islandObjectSubpanelType)
                    {
                        //Берём запрос обновления
                        ref RObjectSubpanelTabUpdate updateRequestComp = ref objectSubpanelTabUpdateRPool.Value.Get(requestEntity);

                        //Обновляем подпанель острова
                        IslandObjectSubpanelUpdate(
                            ref showRequestComp, ref updateRequestComp,
                            objectPanel);

                        objectSubpanelTabShowRPool.Value.Del(requestEntity);
                        objectSubpanelTabUpdateRPool.Value.Del(requestEntity);
                    }
                }
            }
        }

        void IslandObjectSubpanelUpdate(
            ref RObjectSubpanelTabShow showRequestComp, ref RObjectSubpanelTabUpdate updateRequestComp,
            UIObjectPanel objectPanel)
        {
            //Берём остров
            showRequestComp.objectPE.Unpack(world.Value, out int islandEntity);
            ref CIsland island = ref islandPool.Value.Get(islandEntity);

            //Если была активна та же панель
            if (updateRequestComp.isSamePanel == true)
            {

            }
            //Иначе
            else
            {

            }

            //Если была активна та же подпанель 
            if (updateRequestComp.isSameSubpanel == true)
            {

            }
            //Иначе
            else
            {

            }

            //Если была активна та же вкладка
            if (updateRequestComp.isSameTab == true)
            {

            }
            //Иначе
            else
            {

            }

            //Если был активен тот же объект
            if (updateRequestComp.isSameObject == true)
            {

            }
            //Иначе
            else
            {
                //Отображаем название панели - название острова
                objectPanel.objectName.text = island.selfName;
            }
        }
    }
}
