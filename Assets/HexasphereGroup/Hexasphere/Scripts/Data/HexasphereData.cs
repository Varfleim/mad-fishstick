
using System.Collections.Generic;

using UnityEngine;

namespace HS
{
    public class HexasphereData : MonoBehaviour
    {
        //Константы
        internal const float PHI = 1.61803399f;

        internal const string chunksRootGOName = "ChunksRoot";
        internal static GameObject chunksRootGO;
        internal const string chunkGOName = "Chunk";
        internal const string provincesRootGOName = "ProvincesRoot";
        internal static GameObject provincesRootGO;

        internal const int maxVertexCountPerChunk = 65500;

        internal static readonly int[] hexagonIndices = new int[] {
            0, 1, 5,
            1, 2, 5,
            4, 5, 2,
            3, 4, 2
        };
        internal readonly int[] hexagonIndicesExtruded = new int[]
        {
            0, 1, 6,
            5, 0, 6,
            1, 2, 5,
            4, 5, 2,
            2, 3, 7,
            3, 4, 7
        };
        internal static readonly Vector2[] hexagonUVs = new Vector2[] {
            new Vector2 (0, 0.5f),
            new Vector2 (0.25f, 1f),
            new Vector2 (0.75f, 1f),
            new Vector2 (1f, 0.5f),
            new Vector2 (0.75f, 0f),
            new Vector2 (0.25f, 0f)
        };
        internal readonly Vector2[] hexagonUVsExtruded = new Vector2[]
        {
            new Vector2 (0, 0.5f),
            new Vector2 (0.25f, 1f),
            new Vector2 (0.75f, 1f),
            new Vector2 (1f, 0.5f),
            new Vector2 (0.75f, 0f),
            new Vector2 (0.25f, 0f),
            new Vector2 (0.25f, 0.5f),
            new Vector2 (0.75f, 0.5f)
        };

        internal static readonly int[] pentagonIndices = new int[] {
            0, 1, 4,
            1, 2, 4,
            3, 4, 2
        };
        internal readonly int[] pentagonIndicesExtruded = new int[]
        {
            0, 1, 5,
            4, 0, 5,
            1, 2, 4,
            2, 3, 6,
            3, 4, 6
        };
        internal static readonly Vector2[] pentagonUVs = new Vector2[] {
            new Vector2 (0, 0.33f),
            new Vector2 (0.25f, 1f),
            new Vector2 (0.75f, 1f),
            new Vector2 (1f, 0.33f),
            new Vector2 (0.5f, 0f),
        };
        internal readonly Vector2[] pentagonUVsExtruded = new Vector2[]
        {
            new Vector2 (0, 0.33f),
            new Vector2 (0.25f, 1f),
            new Vector2 (0.75f, 1f),
            new Vector2 (1f, 0.33f),
            new Vector2 (0.5f, 0f),
            new Vector2 (0.375f, 0.5f),
            new Vector2 (0.625f, 0.5f)
        };

        //Переменные для генерации
        internal int subdivisions;
        public float hexasphereScale;

        internal readonly Dictionary<DHexaspherePoint, DHexaspherePoint> points = new();

        //Переменные для визуализации
        public static float ExtrudeMultiplier;

        internal Vector3[][] chunksVertices;
        internal int[][] chunksIndices;
        internal Vector4[][] chunksUV2;
        internal Vector4[][] chunksUV;
        internal Color32[][] chunksColors;

        internal MeshFilter[] chunkMeshFilters;
        internal Mesh[] chunkMeshes;
        internal MeshRenderer[] chunkMeshRenderers;

        //Объекты
        public static GameObject HexasphereGO;
        internal static SphereCollider HexasphereCollider;

        //Материалы
        internal Material provinceMaterial;
        internal Material provinceColoredMaterial;

        internal Color defaultShadedColor = new Color(0.56f, 0.71f, 0.54f);

        internal float gradientIntensity;
        internal Color tileTintColor;
        internal Color ambientColor;
        internal float minimumLight;

        internal int uvChunkCount;

        internal Texture2D bevelNormals;
        internal Color[] bevelNormalsColors;

        internal Material hoverProvinceHighlightMaterial;
        internal Material currentProvinceHighlightMaterial;
    }
}
