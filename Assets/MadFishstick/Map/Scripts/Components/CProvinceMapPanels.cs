
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace MF.Map
{
    public struct CProvinceMapPanels
    {
        public CProvinceMapPanels(
            int a)
        {
            mapPanelGroup = null;
        }

        public static VerticalLayoutGroup mapPanelGroupPrefab;
        static List<VerticalLayoutGroup> cachedMapPanelGroups = new();

        public VerticalLayoutGroup mapPanelGroup;

        public static void CacheMapPanelGroup(
            ref CProvinceMapPanels pMP)
        {
            //Заносим группу в список кэшированных
            cachedMapPanelGroups.Add(pMP.mapPanelGroup);

            //Скрываем группу и обнуляем родительский объект
            pMP.mapPanelGroup.gameObject.SetActive(false);
            pMP.mapPanelGroup.transform.SetParent(null);

            //Удаляем ссылку на панель
            pMP.mapPanelGroup = null;
        }

        public static void InstantiateMapPanelGroup(
            ref CProvinceRender pR, ref CProvinceMapPanels pMP,
            Vector3 provinceCenter,
            float mapPanelAltitude)
        {
            //Создаём пустую переменную для группы панелей
            VerticalLayoutGroup mapPanelGroup;

            //Если список кэшированных групп не пуст, то берём кэшированную
            if(cachedMapPanelGroups.Count > 0)
            {
                //Берём последнюю группу в списке и удаляем её из списка
                mapPanelGroup = cachedMapPanelGroups[cachedMapPanelGroups.Count - 1];
                cachedMapPanelGroups.RemoveAt(cachedMapPanelGroups.Count - 1);
            }
            //Иначе
            else
            {
                //Создаём новую группу
                mapPanelGroup = Object.Instantiate(mapPanelGroupPrefab);
            }

            //Отображаем группу и присоединяем её к указанному роидителю
            mapPanelGroup.gameObject.SetActive(true);
            mapPanelGroup.transform.SetParent(pR.ProvinceGO.transform);
            pMP.mapPanelGroup = mapPanelGroup;

            //Задаём положение группы
            CalculateMapPanelGroupPosition(
                ref pMP,
                provinceCenter,
                mapPanelAltitude);
        }

        public static void CalculateMapPanelGroupPosition(
            ref CProvinceMapPanels pMP,
            Vector3 provinceCenter,
            float mapPanelAltitude)
        {
            Vector3 direction = provinceCenter.normalized * mapPanelAltitude;

            pMP.mapPanelGroup.transform.position = provinceCenter + direction;
        }
    }
}
