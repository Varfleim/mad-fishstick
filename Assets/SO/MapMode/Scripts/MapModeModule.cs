
using System.Collections.Generic;

using UnityEngine;

using MF;

namespace SO.MapMode
{
    [CreateAssetMenu]
    public class MapModeModule : MFModule
    {
        #region MapMode
        public string politicalMapModeName;
        public Color politicalMapModeDefaultColor;
        #endregion

        public override void AddSystems(MFStartup startup)
        {
            //Добавляем системы инициализации
            #region PreInit
            //Создание режимов карты
            startup.AddPreInitSystem(new SMapModesCreation());
            #endregion
            #region PostInit
            //Создание постоянных границ
            startup.AddPostInitSystem(new SMapPermanentEdgesCreation());
            #endregion

            //Добавляем покадровые системы
            #region Frame
            //Ввод в режимах карты
            startup.AddFrameSystem(new SMapModesInput());
            #endregion

            //Добавляем системы рендеринга
            #region PreRender
            //Политический режим карты
            startup.AddPreRenderSystemGroup(
                politicalMapModeName,
                false,
                new SPoliticalMapModeRender(),
                new SMTPoliticalMapModeRender());

            //Обновление списков цветов режимов карты при обновлении списков цветов объектов
            startup.AddPreRenderSystem(new SMapModesUpdateColors());
            #endregion
        }

        public override void InjectData(MFStartup startup)
        {
            //Создаём компонент данных режимов карты
            MapModeData mapModeData = startup.AddDataObject().AddComponent<MapModeData>();

            //Переносим в него данные
            mapModeData.politicalMapModeName = politicalMapModeName;
            mapModeData.politicalMapModeDefaultColor = politicalMapModeDefaultColor;

            //Вводим данные
            startup.InjectData(mapModeData);
        }
    }
}
