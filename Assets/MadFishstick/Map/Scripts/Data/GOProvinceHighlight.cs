
using System.Collections.Generic;

using UnityEngine;

namespace MF.Map
{
    public class GOProvinceHighlight : MonoBehaviour
    {
        public static GOProvinceHighlight provinceHighlightPrefab;
        static List<GOProvinceHighlight> cachedProvinceHighlights = new();

        public MeshRenderer meshRenderer;
        public MeshFilter meshFilter;

        public static void CacheProvinceHighlight(
            ref CProvinceHoverHighlight pHoverHighlight)
        {
            //Совершаем общие действия
            CacheProvinceHighlight(pHoverHighlight.highlight);

            //Удаляем ссылку на подсветку
            pHoverHighlight.highlight = null;
        }

        static void CacheProvinceHighlight(
            GOProvinceHighlight provinceHighlight)
        {
            //Скрываем подсветку и удаляем ссылку на родительский объект
            provinceHighlight.gameObject.SetActive(false);
            provinceHighlight.transform.SetParent(null);

            //Заносим подсветку в список кэшированных
            cachedProvinceHighlights.Add(provinceHighlight);
        }

        public static void InstantiateProvinceHighlight(
            ref CProvinceRender pR, ref CProvinceHoverHighlight pHoverHighlight,
            Material highlightMaterial)
        {
            //Совершаем общие действия
            GOProvinceHighlight provinceHighlight = InstantiateProvinceHighlight(
                ref pR, 
                highlightMaterial);

            //Присоединяем подсветку к указанному родителю
            pHoverHighlight.highlight = provinceHighlight;
        }

        static GOProvinceHighlight InstantiateProvinceHighlight(
            ref CProvinceRender pR,
            Material highlightMaterial)
        {
            //Создаём пустую переменную для подсветки
            GOProvinceHighlight provinceHighlight;

            //Если список кэшированных подсветок не пуст, то берём кэшированную
            if (cachedProvinceHighlights.Count > 0)
            {
                //Берём последнюю подсветку в списке и удаляем её из списка
                provinceHighlight = cachedProvinceHighlights[cachedProvinceHighlights.Count - 1];
                cachedProvinceHighlights.RemoveAt(cachedProvinceHighlights.Count - 1);
            }
            //Иначе
            else
            {
                //Создаём новую подсветку
                provinceHighlight = Instantiate(provinceHighlightPrefab);
            }

            //Отображаем подсветку и присоединяем её к GO провинции
            provinceHighlight.gameObject.SetActive(true);
            provinceHighlight.transform.SetParent(pR.ProvinceGO.transform);
            provinceHighlight.transform.localScale = Vector3.one;


            //Устанавливаем материал подсветки
            provinceHighlight.meshRenderer.material = highlightMaterial;

            //Возвращаем рендерер
            return provinceHighlight;
        }

        public static void UpdateProvinceHighlight(
            ref CProvinceRender pR,
            GOProvinceHighlight provinceHighlight,
            Material highlightMaterial)
        {
            //Обновляем материал подсветки
            provinceHighlight.meshRenderer.material = highlightMaterial;
        }
    }
}
