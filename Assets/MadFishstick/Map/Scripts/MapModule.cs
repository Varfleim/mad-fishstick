
using UnityEngine;

namespace MF.Map
{
    [CreateAssetMenu]
    internal class MapModule : MFModule
    {
        public GOProvince provinceGOPrefab;
        public GOProvinceHighlight provinceHighlightGOPrefab;

        public override void AddSystems(MFStartup startup)
        {
            //Добавляем системы инициализации
            #region PreInit
            //Создание карт и запросов генерации
            startup.AddPreInitSystem(new SMapCreation());
            #endregion
            #region Init
            //Создание главных компонентов провинций
            startup.AddInitSystem(new SProvinceCoreCreation());

            //Создание главных компонентов режимов карты
            startup.AddInitSystem(new SMainMapModesCreation());
            #endregion

            //Добавляем покадровые системы


            //Добавляем системы рендеринга
            #region PreRender
            //Управление картами
            startup.AddPreRenderSystem(new SMapControl());

            //Активация режимов карты
            startup.AddPreRenderSystem(new SMapModesActivation());
            //Включение группы систем визуализации режимов карты
            startup.AddPreRenderSystem(new SMapModeRenderStart());
            #endregion
            #region Render
            //Выключение группы систем визуализации режимов карты
            startup.AddRenderSystem(new SMapModeRenderEnd());
            #endregion
            #region PostRender
            //Изменение параметров рендера карты
            startup.AddPostRenderSystem(new SMapRender());
            #endregion

            //Добавляем потиковые системы
            #region PostTick
            //Запрос обновления активного режима карты
            startup.AddPostTickSystem(new SMapModeUpdate());
            #endregion
        }

        public override void InjectData(MFStartup startup)
        {
            //Создаём компонент данных карт
            MapData mapData = startup.AddDataObject().AddComponent<MapData>();

            //Вводим данные
            startup.InjectData(mapData);

            //Создаём компонент данных режимов карты
            MapModeData mapModeData = startup.AddDataObject().AddComponent<MapModeData>();

            //Вводим данные
            startup.InjectData(mapModeData);

            GOProvince.provinceGOPrefab = provinceGOPrefab;
            GOProvinceHighlight.provinceHighlightPrefab = provinceHighlightGOPrefab;
        }
    }
}
