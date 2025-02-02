
using UnityEngine;

using MF;

namespace SO.Colonization
{
    [CreateAssetMenu]
    public class ColonizationModule : MFModule
    {
        public int colonizationProgressMax;

        public override void AddSystems(MFStartup startup)
        {
            //Добавляем системы рендеринга
            #region Render
            //Перенос запросов визуализации с владельцев колоний на колонии
            startup.AddRenderSystem(new SColoniesOwnerTransferRenderRequests());
            #endregion

            //Добавляем потиковые системы
            #region PreTick
            //Создание колоний
            startup.AddPreTickSystem(new SLandColonize());
            #endregion
            #region Tick
            //Подсчёт прогресса колонизации
            startup.AddTickSystem(new SMTColonizationProgressCalc());
            //Проверка завершения колонизации
            startup.AddTickSystem(new SColonizationEnd());
            #endregion
        }

        public override void InjectData(MFStartup startup)
        {
            //Создаём компонент данных колонизации
            ColonizationData colonizationData = startup.AddDataObject().AddComponent<ColonizationData>();

            //Переносим в него данные
            colonizationData.colonizationProgressMax = colonizationProgressMax;

            //Вводим данные
            startup.InjectData(colonizationData);
        }
    }
}
