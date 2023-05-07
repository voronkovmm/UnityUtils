using UnityEngine;
using Voronkovmm;

namespace Voronkovmm
{
    [RequireComponent(typeof(MeshFilter))]
    class HeatMapGenericVisual : MonoBehaviour
    {
        private Grid<HeatMapGridObject> _grid;
        private Mesh _mesh;

        private bool _updateMesh;

        private void Awake()
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
        }

        public void SetGrid(Grid<HeatMapGridObject> grid)
        {
            _grid = grid;
            UpdateHeatMapVisual();

            _grid.OnGridValueChange += Grid_OnGridValueChanged;
        }

        private void Grid_OnGridValueChanged(Grid<HeatMapGridObject>.OnGridValueChanged value) => _updateMesh = true;

        private void LateUpdate()
        {
            if (_updateMesh)
            {
                _updateMesh = false;
                UpdateHeatMapVisual();
            }
        }

        private void UpdateHeatMapVisual()
        {
            MeshUtils.CreateEmptyMeshArrays(_grid.Width * _grid.Height, out Vector3[] vertices, out Vector2[] uvs, out int[] triangles);

            for (int x = 0; x < _grid.Width; x++)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    int index = x * _grid.Height + y;
                    Vector3 quadSize = Vector3.one * _grid.Cellsize;

                    HeatMapGridObject gridObject = _grid.GetGridObject(x, y);
                    float gridValueNormalized = gridObject.GetNormalizedValue();
                    Vector2 gridValueUV = new Vector2(gridValueNormalized, 0);

                    MeshUtils.AddToMeshArrays(vertices, uvs, triangles, index, _grid.GetWorldPosition(x, y) + quadSize * 0.5f, 0f, quadSize, gridValueUV, gridValueUV);
                }
            }

            _mesh.vertices = vertices;
            _mesh.uv = uvs;
            _mesh.triangles = triangles;
        }
    } 
}

 