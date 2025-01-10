
using UnityEngine;

using MF;

namespace HS.Input
{
    [CreateAssetMenu]
    public class HexasphereInputModule : MFModule
    {
        public override void AddSystems(MFStartup startup)
        {
            //Добавляем системы инициализации

            //Добавляем покадровые системы
            #region PreFrame
            //Ввод с мыши
            startup.AddPreFrameSystem(new SMouseInput());
            #endregion

            //Добавляем системы рендеринга

            //Добавляем потиковые системы

        }

        public override void InjectData(MFStartup startup)
        {
            //Создаём новый объект для данных ввода гексасферы и назначаем ему их компонент
            HexasphereInputData hexasphereInputData = startup.AddDataObject().AddComponent<HexasphereInputData>();

            //Вводим данные
            startup.InjectData(hexasphereInputData);
        }
    }
}
