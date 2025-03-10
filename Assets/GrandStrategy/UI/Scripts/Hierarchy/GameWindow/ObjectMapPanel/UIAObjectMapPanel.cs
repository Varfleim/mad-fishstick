
using System.Collections.Generic;

using UnityEngine;

using Leopotam.EcsLite;

namespace GS.UI
{
    public abstract class UIAObjectMapPanel : MonoBehaviour
    {
        public static Dictionary<string, UIAObjectMapPanel> objectMapPanelPrefabs = new();
        public static Dictionary<string, List<UIAObjectMapPanel>> cachedObjectMapPanels = new();

        public EcsPackedEntity selfPE;

        public EcsPackedEntity parentProvincePE;

        public static void CachePanel(
            ref CObjectDisplayedMapPanels objectDisplayedMapPanels,
            string objectMapPanelType)
        {
            //Берём панель
            UIAObjectMapPanel currentObjectMapPanel = objectDisplayedMapPanels.objectMapPanels[objectMapPanelType];

            //Заносим панель в список кэшированных
            cachedObjectMapPanels[objectMapPanelType].Add(currentObjectMapPanel);

            //Скрываем панель и обнуляем родительский объект
            currentObjectMapPanel.gameObject.SetActive(false);
            currentObjectMapPanel.transform.SetParent(null);

            //Удаляем ссылку на панель
            objectDisplayedMapPanels.objectMapPanels.Remove(objectMapPanelType);
        }

        public static void InstantiatePanel(
            EcsPackedEntity objectPE,
            ref CObjectDisplayedMapPanels objectDisplayedMapPanels,
            string objectMapPanelType)
        {
            //Создаём пустую переменную для панели
            UIAObjectMapPanel currentObjectMapPanel;

            //Если список кэшированных панелей не пуст, то берём кэшированную
            if(cachedObjectMapPanels[objectMapPanelType].Count > 0)
            {
                //Берём список кэшированных панелей
                List<UIAObjectMapPanel> cachedPanels = cachedObjectMapPanels[objectMapPanelType];

                //Берём последнюю панель в списке и удаляем её из списка
                currentObjectMapPanel = cachedPanels[cachedPanels.Count - 1];
                cachedPanels.RemoveAt(cachedPanels.Count - 1);

            }
            //Иначе
            else
            {
                //Создаём новую панель
                currentObjectMapPanel = Instantiate(objectMapPanelPrefabs[objectMapPanelType]);
            }

            //Обновляем основные данные панели
            currentObjectMapPanel.UpdatePanel(objectPE);
            currentObjectMapPanel.parentProvincePE = new();

            //Отображаем новую панель
            currentObjectMapPanel.gameObject.SetActive(true);

            //Заносим её в словарь панелей объекта
            objectDisplayedMapPanels.objectMapPanels.Add(objectMapPanelType, currentObjectMapPanel);
        }

        public void UpdatePanel(
            EcsPackedEntity objectPE)
        {
            selfPE = objectPE;
        }
    }
}
