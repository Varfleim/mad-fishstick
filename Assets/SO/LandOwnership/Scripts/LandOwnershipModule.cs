
using UnityEngine;

using MF;

namespace SO.LandOwnership
{
    [CreateAssetMenu]
    public class LandOwnershipModule : MFModule
    {
        public override void AddSystems(MFStartup startup)
        {
            //Добавляем системы инициализации
            #region Init
            //Смена владельцев земли
            startup.AddInitSystem(new SLandChangeOwner());
            #endregion

            //Добавляем системы рендеринга
            #region Render
            //Перенос запросов визуализации с владельцев земли на землю
            startup.AddRenderSystem(new SLandOwnerTransferRenderRequests());
            #endregion
        }

        public override void InjectData(MFStartup startup)
        {
            //Создаём компонент данных владения землёй
            LandOwnershipData landOwnershipData = startup.AddDataObject().AddComponent<LandOwnershipData>();

            //Вводим данные
            startup.InjectData(landOwnershipData);
        }
    }
}
