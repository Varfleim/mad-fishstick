
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace GS.UI
{
    public class SObjectMapPanelControl : IEcsRunSystem
    {
        readonly EcsWorldInject world = default;


        readonly EcsPoolInject<CObjectDisplayedMapPanels> objectDisplayedMapPanelsPool = default;


        readonly EcsFilterInject<Inc<RObjectMapPanelShow>> objectMapPanelShowRFilter = default;
        readonly EcsPoolInject<RObjectMapPanelShow> objectMapPanelShowRPool = default;

        readonly EcsFilterInject<Inc<RObjectMapPanelHide>> objectMapPanelHideRFilter = default;
        readonly EcsPoolInject<RObjectMapPanelHide> objectMapPanelHideRPool = default;

        public void Run(IEcsSystems systems)
        {
            //Отображаем панели карты объектов
            ShowObjectMapPanel();

            //Скрываем панели карты объектов
            HideObjectMapPanel();
        }

        void ShowObjectMapPanel()
        {
            //Для каждого запроса отображения панели карты объекта
            foreach (int requestEntity in objectMapPanelShowRFilter.Value)
            {
                //Берём запрос
                ref RObjectMapPanelShow requestComp = ref objectMapPanelShowRPool.Value.Get(requestEntity);

                //Если запрошенную панель требуется отобразить
                if (ObjectMapPanelExist(
                    requestComp.objectPE,
                    requestComp.objectMapPanelType) == false)
                {
                    //Отображаем панель
                    ObjectMapPanelShow(ref requestComp);
                }

                //Запрос передаётся дальше, переходя в модуль игрового интерфейса
            }
        }

        void ObjectMapPanelShow(
            ref RObjectMapPanelShow requestComp)
        {
            //Берём сущность объекта
            requestComp.objectPE.Unpack(world.Value, out int objectEntity);

            //Если у объекта нет компонента панелей карты
            if(objectDisplayedMapPanelsPool.Value.Has(objectEntity) == false)
            {
                //Создаём его
                ObjectDisplayedMapPanelsCreate(objectEntity);
            }

            //Берём компонент панелей карты объекта
            ref CObjectDisplayedMapPanels objectDisplayedMapPanels = ref objectDisplayedMapPanelsPool.Value.Get(objectEntity);

            //Создаём панель
            UIAObjectMapPanel.InstantiatePanel(
                requestComp.objectPE,
                ref objectDisplayedMapPanels,
                requestComp.objectMapPanelType);
        }

        void ObjectDisplayedMapPanelsCreate(
            int objectEntity)
        {
            //Назначаем объекту компонент панелей карты
            ref CObjectDisplayedMapPanels objectDisplayedMapPanels = ref objectDisplayedMapPanelsPool.Value.Add(objectEntity);

            //Заполняем основные данные компонента
            objectDisplayedMapPanels = new(0);
        }

        void HideObjectMapPanel()
        {
            //Для каждого запроса сокрытия панели карты объекта
            foreach(int requestEntity in objectMapPanelHideRFilter.Value)
            {
                //Берём запрос
                ref RObjectMapPanelHide requestComp = ref objectMapPanelHideRPool.Value.Get(requestEntity);

                //Если запрошенная панель есть у объекта
                if(ObjectMapPanelExist(
                    requestComp.objectPE,
                    requestComp.objectMapPanelType) == true)
                {
                    //Скрываем её
                    ObjectMapPanelHide(ref requestComp);
                }

                //Удаляем запрос
                objectMapPanelHideRPool.Value.Del(requestEntity);
            }
        }

        void ObjectMapPanelHide(
            ref RObjectMapPanelHide requestComp)
        {
            //Берём сущность объекта и компонент панелей карты
            requestComp.objectPE.Unpack(world.Value, out int objectEntity);
            ref CObjectDisplayedMapPanels objectDisplayedMapPanels = ref objectDisplayedMapPanelsPool.Value.Get(objectEntity);

            //Кэшируем запрошенную панель
            UIAObjectMapPanel.CachePanel(
                ref objectDisplayedMapPanels,
                requestComp.objectMapPanelType);

            //Если у объекта больше нет панелей карты
            if(objectDisplayedMapPanels.objectMapPanels.Count == 0)
            {
                //Удаляем компонент панелей карты
                ObjectDisplayedMapPanelsRemove(objectEntity);
            }
        }

        void ObjectDisplayedMapPanelsRemove(
            int objectEntity)
        {
            //Удаляем компонент панелей карты
            objectDisplayedMapPanelsPool.Value.Del(objectEntity);
        }

        bool ObjectMapPanelExist(
            EcsPackedEntity objectPE,
            string objectMapPanelType)
        {
            //Берём сущность объекта
            objectPE.Unpack(world.Value, out int objectEntity);

            //Если у объекта есть компонент панелей карты
            if (objectDisplayedMapPanelsPool.Value.Has(objectEntity) == true)
            {
                //Берём компонент
                ref CObjectDisplayedMapPanels objectDisplayedMapPanels = ref objectDisplayedMapPanelsPool.Value.Get(objectEntity);

                //Если у объекта есть панель запрошенного типа
                if (objectDisplayedMapPanels.objectMapPanels.ContainsKey(objectMapPanelType) == true)
                {
                    //Запрошенная панель уже есть у объекта
                    return true;
                }
                //Иначе
                else
                {
                    //Запрошенная панель карты отсутствует
                    return false;
                }
            }
            //Иначе
            else
            {
                //Запрошенная панель карты точно отсутствует
                return false;
            }
        }
    }
}
