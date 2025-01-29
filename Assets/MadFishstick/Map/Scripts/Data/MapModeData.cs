
using System.Collections.Generic;

using UnityEngine;

using Leopotam.EcsLite;

namespace MF.Map
{
    public class MapModeData : MonoBehaviour
    {
        internal EcsPackedEntity defaultMapModePE;

        public EcsPackedEntity activeMapModePE;

        public static void MapModeCreationRequest(
            EcsPool<SRMapModeCreation> requestPool,
            int mapModeEntity, string mapModeName,
            bool defaultMapMode)
        {
            //Назначаем сущности запрос создания режима карты
            ref SRMapModeCreation requestComp = ref requestPool.Add(mapModeEntity);

            //Заполняем данные запроса
            requestComp = new(
                mapModeName,
                defaultMapMode);
        }

        public static void MapModeUpdateColorsListFirstRequest(
            EcsWorld world,
            EcsPool<RMapModeUpdateColorsListFirst> requestPool,
            string coloredObjectType,
            List<Color> objectColors)
        {
            //Создаём новую сущность и назначаем ей запрос
            int requestEntity = world.NewEntity();
            ref RMapModeUpdateColorsListFirst requestComp = ref requestPool.Add(requestEntity);

            //Заполняем данные запроса
            requestComp = new(
                coloredObjectType,
                objectColors);
        }

        public static void MapModeUpdateColorsListSecondRequest(
            EcsWorld world,
            EcsPool<RMapModeUpdateColorsListSecond> requestPool,
            EcsPackedEntity mapModePE,
            List<Color> mapModeColors, Color defaultColor)
        {
            //Создаём новую сущность и назначаем ей запрос
            int requestEntity = world.NewEntity();
            ref RMapModeUpdateColorsListSecond requestComp = ref requestPool.Add(requestEntity);

            //Заполняем данные запроса
            requestComp = new(
                mapModePE,
                mapModeColors, defaultColor);
        }

        internal static void MapModeActivationRequest(
            EcsWorld world,
            EcsPool<RMapModeActivation> requestPool,
            EcsPackedEntity mapModePE)
        {
            //Создаём новую сущность и назначаем ей запрос
            int requestEntity = world.NewEntity();
            ref RMapModeActivation requestComp = ref requestPool.Add(requestEntity);

            //Заполняем данные запроса
            requestComp = new(
                mapModePE);
        }

        internal static void MapModeUpdateRequest(
            EcsPool<SRMapModeUpdate> requestPool,
            int mapModeEntity)
        {
            //Назначаем сущности режима карты запрос 
            ref SRMapModeUpdate requestComp = ref requestPool.Add(mapModeEntity);

            //Заполняем данные запроса
            requestComp = new(0);
        }

         public static void SetMapRenderValuesRequestFull(
            EcsPool<SRSetMapRenderValues> requestPool,
            ref CMapModeCore mapMode,
            int targetEntity,
            EcsPackedEntity displayedObjectPE,
            float height,
            int colorIndex)
        {
            //Назначаем сущности запрос
            ref SRSetMapRenderValues requestComp = ref requestPool.Add(targetEntity);

            //Заполняем данные запроса
            requestComp = new(
                displayedObjectPE,
                height,
                colorIndex);
        }

        public static void SetMapRenderValuesRequestCreation(
            EcsPool<SRSetMapRenderValues> requestPool,
            int targetEntity)
        {
            //Назначаем сущности запрос
            ref SRSetMapRenderValues requestComp = ref requestPool.Add(targetEntity);

            //Заполняем данные запроса стандартными данными
            requestComp = new(0);
        }

        public static void SetMapRenderValuesRequestUpdate(
            ref CMapModeCore mapMode,
            ref SRSetMapRenderValues requestComp,
            EcsPackedEntity displayedObjectPE,
            float height,
            int colorIndex)
        {
            //Заполняем данные запроса
            requestComp = new(
                displayedObjectPE,
                height,
                colorIndex);
        }

        public static void ShowMapHoverHighlightRequest(
            EcsPool<SRShowMapHoverHighlight> requestPool,
            ref CMapModeCore mapMode,
            int targetEntity)
        {
            //Назначаем сущности запрос
            ref SRShowMapHoverHighlight requestComp = ref requestPool.Add(targetEntity);

            //Заполняем данные запроса
            requestComp = new(0);
        }

        internal static void UpdateProvinceDisplayedObject(
            ref CMapModeCore mapMode,
            ref CProvinceRender pR,
            EcsPackedEntity displayedObjectPE)
        {
            //Если отображаемый объект провинции не равен переданному
            if(pR.DisplayedObjectPE.EqualsTo(in displayedObjectPE) == false)
            {
                //Обновляем его
                pR.SetProvinceDisplayedObject(displayedObjectPE);
            }
        }

        internal static bool UpdateProvinceHeight(
            ref CMapModeCore mapMode,
            ref CProvinceRender pR,
            float newProvinceHeight)
        {
            //Если высота провинции не равна переданной
            if(pR.ProvinceHeight != newProvinceHeight)
            {
                //Обновляем её
                pR.SetProvinceHeight(newProvinceHeight);

                //Возвращаем, что высота была обновлена
                return true;
            }
            //Иначе
            else
            {
                //Возвращаем, что высота не была обновлена
                return false;
            }
        }

        internal static bool UpdateProvinceColorIndex(
            ref CMapModeCore mapMode,
            ref CProvinceRender pR,
            int newProvinceColorIndex)
        {
            //Если индекс цвета провинции не равен переданному
            if (pR.ProvinceColorIndex != newProvinceColorIndex)
            {
                //Обновляем его
                pR.SetProvinceColorIndex(newProvinceColorIndex);

                //Возвращаем, что индекс цвета был обновлён
                return true;
            }
            //Иначе
            else
            {
                //Возвращаем, что индекс цвета не был обновлён
                return false;
            }
        }
    }
}
