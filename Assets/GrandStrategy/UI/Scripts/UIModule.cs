
using UnityEngine;

using MF;

namespace GS.UI
{
    [CreateAssetMenu]
    public class UIModule : MFModule
    {
        public override void AddSystems(MFStartup startup)
        {
            //Добавляем покадровые системы
            #region Frame
            //Ввод в панели объекта
            startup.AddFrameSystem(new SObjectPanelInput());
            #endregion

            //Добавляем системы рендеринга
            #region PreRender
            //Отображение панели объекта
            startup.AddPreRenderSystem(new SObjectPanelControl());

            //Отображение панелей карты объектов
            startup.AddPreRenderSystem(new SObjectMapPanelControl());
            #endregion
        }

        public override void InjectData(MFStartup startup)
        {
            //Берём компонент данных UI
            UIData uIData = startup.GetComponentInChildren<UIData>();

            //Вводим данные
            startup.InjectData(uIData);

            //Берём главный объект интерфейса
            UICore uICore = uIData.uICore;

            //Вводим данные
            startup.InjectData(uICore);
        }
    }
}
