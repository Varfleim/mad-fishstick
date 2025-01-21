
using System.Collections.Generic;

using UnityEngine;

using MF;

namespace SO.Region
{
    [CreateAssetMenu]
    public class RegionModule : MFModule
    {
        #region MapMode
        public string regionMapModeName;
        #endregion
        public override void AddSystems(MFStartup startup)
        {
            //Добавляем системы инициализации
            #region PreInit
            //Создание режимов карты
            startup.AddPreInitSystem(new SMapModesCreation());
            #endregion
            #region Init
            //Запрос генерации регионов по запросу генерации карты
            startup.AddInitSystem(new SRegionsGenerationRequest());

            //Генерация регионов
            startup.AddInitSystem(new SRegionsGeneration());
            #endregion

            //Добавляем покадровые системы
            #region Frame
            //Ввод в режимах карты
            startup.AddFrameSystem(new SMapModesInput());
            #endregion

            //Добавляем системы рендеринга
            #region PreRender
            //Рассчёт визуализации режимов карты
            //Режим карты регионов
            startup.AddPreRenderSystemGroup(
                regionMapModeName,
                false,
                new SRegionMapModeRender(),
                new SMTRegionMapModeRender());
            #endregion
            #region Render
            //Перенос запросов визуализации с регионов на дочерние провинции
            startup.AddRenderSystem(new SRegionTransferRenderRequests());
            #endregion
        }

        public override void InjectData(MFStartup startup)
        {
            //Создаём новый объект для данных режимов карты и назначаем ему их компонент
            MapModeData mapModeData = startup.AddDataObject().AddComponent<MapModeData>();

            //Переносим в него данные
            mapModeData.regionMapModeName = regionMapModeName;

            //Вводим данные
            startup.InjectData(mapModeData);
        }
    }
}
