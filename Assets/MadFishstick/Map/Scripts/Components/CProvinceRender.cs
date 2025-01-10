
using Leopotam.EcsLite;

namespace MF.Map
{
    /// <summary>
    /// Компонент, хранящий данные провинции, использующиеся для универсальной визуализации
    /// </summary>
    public struct CProvinceRender
    {
        public CProvinceRender(int a)
        {
            displayedObjectPE = new();

            provinceHeight = 0f;

            provinceColorIndex = -1;

            provinceGO = null;
        }

        public EcsPackedEntity DisplayedObjectPE 
        {
            get
            {
                return displayedObjectPE;
            }
        }
        EcsPackedEntity displayedObjectPE;

        public float ProvinceHeight
        {
            get
            {
                return provinceHeight;
            }
        }
        float provinceHeight;

        public int ProvinceColorIndex
        {
            get
            {
                return provinceColorIndex;
            }
        }
        int provinceColorIndex;

        public GOProvince ProvinceGO
        {
            get
            {
                return provinceGO;
            }
        }
        GOProvince provinceGO;

        public void SetProvinceDisplayedObject(
            EcsPackedEntity displayedObjectPE)
        {
            this.displayedObjectPE = displayedObjectPE;
        }

        public void SetProvinceHeight(
            float provinceHeight)
        {
            this.provinceHeight = provinceHeight;
        }

        public void SetProvinceColorIndex(
            int provinceColorIndex)
        {
            this.provinceColorIndex = provinceColorIndex;
        }

        public void SetProvinceGO(
            GOProvince provinceGO)
        {
            this.provinceGO = provinceGO;
        }
    }
}
