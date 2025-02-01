
using UnityEngine;

using MF;

namespace SO.Region
{
    [CreateAssetMenu]
    internal class RegionModule : MFModule
    {
        #region MapMode
        public string regionMapModeName;
        public Color regionMapModeDefaultColor;
        #endregion

        public override void AddSystems(MFStartup startup)
        {
            //Добавляем системы инициализации
            #region PreInit
            //Создание режимов карты
            startup.AddPreInitSystem(new SMapModesCreation());

            //Генерация регионов по запросу
            startup.AddPreInitSystem(new SRegionsGeneration());
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
            //Создаём компонент данных регионов
            RegionData regionData = startup.AddDataObject().AddComponent<RegionData>();

            //Вводим данные
            startup.InjectData(regionData);

            //Создаём компонент данных режимов карты
            MapModeData mapModeData = startup.AddDataObject().AddComponent<MapModeData>();

            //Переносим в него данные
            mapModeData.regionMapModeName = regionMapModeName;
            mapModeData.regionMapModeDefaultColor = regionMapModeDefaultColor;

            //Вводим данные
            startup.InjectData(mapModeData);
        }
    }
}
