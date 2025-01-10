
using UnityEngine;

using Leopotam.EcsLite;

namespace MF.Map
{
    /// <summary>
    /// Компонент, хранящий базовые данные провинции
    /// </summary>
    public struct CProvinceCore
    {
        public CProvinceCore(
            EcsPackedEntity selfPE,
            EcsPackedEntity[] neighbourProvincePEs)
        {
            this.selfPE = selfPE;

            this.neighbourProvincePEs = neighbourProvincePEs;
        }

        public readonly EcsPackedEntity selfPE;

        #region ProvinceData
        public readonly EcsPackedEntity[] neighbourProvincePEs;
        #endregion
    }
}
