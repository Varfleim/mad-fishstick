
using System.Collections.Generic;

using UnityEngine;

using MF;

namespace HS
{
    [CreateAssetMenu]
    public class HexasphereModule : MFModule
    {
        #region Hexasphere
        public float hexasphereScale;
        public float extrudeMultiplier;
        #endregion

        #region HexasphereRender

        public float gradientIntensity;
        public Color tileTintColor;
        public Color ambientColor;
        public float minimumLight;

        public Material provinceMaterial;
        public Material provinceColoredMaterial;

        public Material hoverProvinceHighlightMaterial;
        public Material currentProvinceHighlightMaterial;
        #endregion

        #region Camera
        public float rotationSpeed;
        public float minAngleX;
        public float maxAngleX;

        public float stickMinZoom;
        public float stickMaxZoom;
        public float swiwelMinZoom;
        public float swiwelMaxZoom;
        #endregion

        #region MapMode
        public string defaultMapModeName;
        public List<Color> defaultMapModeColors = new();
        #endregion

        public override void AddSystems(MFStartup startup)
        {
            //Добавляем системы инициализации
            #region PreInit
            //Генерация гексасферы по запросу
            startup.AddPreInitSystem(new SHexasphereGeneration());

            //Создание режимов карты
            startup.AddPreInitSystem(new SMapModesCreation());
            #endregion

            //Добавляем покадровые системы
            #region Frame
            //Ввод в режимах карты
            startup.AddFrameSystem(new SMapModesInput());

            //Преобразование запроса движения камеры
            startup.AddFrameSystem(new SICameraMovingRequest());
            //Движение камеры
            startup.AddFrameSystem(new SCameraMoving());
            #endregion

            //Добавляем системы рендеринга
            #region PreRender
            //Рассчёт визуализации режимов карты
            //Стандартный режим карты
            startup.AddPreRenderSystemGroup(
                defaultMapModeName,
                false,
                new SDefaultMapModeRender(),
                new SMTDefaultMapModeRender());
            #endregion
            #region PostRender
            //Рендер гексасферы при изменении
            startup.AddPostRenderSystem(new SHexasphereRender());
            #endregion

            //Добавляем потиковые системы

        }

        public override void InjectData(MFStartup startup)
        {
            //Создаём объект для данных гексасферы и назначаем ему их компонент
            HexasphereData hexasphereData = startup.AddDataObject().AddComponent<HexasphereData>();

            //Переносим в него данные
            //Сфера
            hexasphereData.hexasphereScale = hexasphereScale;
            HexasphereData.ExtrudeMultiplier = extrudeMultiplier;

            //GO
            HexasphereData.HexasphereGO = startup.mapObject;
            HexasphereData.HexasphereCollider = startup.mapCollider as SphereCollider;

            //Шейдеры
            hexasphereData.gradientIntensity = gradientIntensity;
            hexasphereData.tileTintColor = tileTintColor;
            hexasphereData.ambientColor = ambientColor;
            hexasphereData.minimumLight = minimumLight;

            hexasphereData.provinceMaterial = provinceMaterial;
            hexasphereData.provinceColoredMaterial = provinceColoredMaterial;

            hexasphereData.hoverProvinceHighlightMaterial = hoverProvinceHighlightMaterial;
            hexasphereData.hoverProvinceHighlightMaterial.shaderKeywords = null;//= new string[] { ShaderParameters.SKW_HIGHLIGHT_TINT_BACKGROUND };
            hexasphereData.hoverProvinceHighlightMaterial.SetFloat(ShaderParameters.ColorShift, 1f);

            hexasphereData.currentProvinceHighlightMaterial = currentProvinceHighlightMaterial;
            hexasphereData.currentProvinceHighlightMaterial.shaderKeywords = null;//= new string[] { ShaderParameters.SKW_HIGHLIGHT_TINT_BACKGROUND };
            hexasphereData.currentProvinceHighlightMaterial.SetFloat(ShaderParameters.ColorShift, 1f);

            //Вводим данные
            startup.InjectData(hexasphereData);

            //Создаём новый объект для данных камеры гексасферы и назначаем ему их компонент
            HexasphereCameraData hexasphereCameraData = startup.AddDataObject().AddComponent<HexasphereCameraData>();

            //Переносим в него данные
            //Объекты
            hexasphereCameraData.hexasphereCamera = startup.mapCamera;
            hexasphereCameraData.swiwel = startup.swiwel;
            hexasphereCameraData.stick = startup.stick;
            hexasphereCameraData.camera = startup.camera;

            //Переменные
            hexasphereCameraData.rotationSpeed = rotationSpeed;
            hexasphereCameraData.minAngleX = minAngleX;
            hexasphereCameraData.maxAngleX = maxAngleX;

            hexasphereCameraData.stickMinZoom = stickMinZoom;
            hexasphereCameraData.stickMaxZoom = stickMaxZoom;
            hexasphereCameraData.swiwelMinZoom = swiwelMinZoom;
            hexasphereCameraData.swiwelMaxZoom = swiwelMaxZoom;

            //Вводим данные
            startup.InjectData(hexasphereCameraData);

            //Создаём новый объект для данных режимов карты и назначаем ему их компонент
            MapModeData mapModeData = startup.AddDataObject().AddComponent<MapModeData>();

            //Переносим в него данные
            mapModeData.defaultMapModeName = defaultMapModeName;
            mapModeData.defaultMapModeColors = defaultMapModeColors;

            //Вводим данные
            startup.InjectData(mapModeData);
        }
    }
}
