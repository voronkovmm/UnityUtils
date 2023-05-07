using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Voronkovmm
{
    public class Grid<TGridObject>
    {
        public event Action<OnGridValueChanged> OnGridValueChange;
        public struct OnGridValueChanged
        {
            public int x, y;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public float Cellsize { get; private set; }

        private Vector3 _originPosition;
        private TGridObject[,] _gridArray;
        private TextMesh[,] _debugTextArray;

        private bool showDebug = true;

        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
        {
            Width = width;
            Height = height;
            Cellsize = cellSize;
            _originPosition = originPosition;
            _gridArray = new TGridObject[width, height];

            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    _gridArray[x, y] = createGridObject(this, x, y);
                }
            }

            if (showDebug)
            {
                _debugTextArray = new TextMesh[width, height];

                for (int x = 0; x < _gridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < _gridArray.GetLength(1); y++)
                    {
                        _debugTextArray[x, y] = WorldText.Create(_gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center, 100);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                    }
                }
                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(width, height), GetWorldPosition(width, 0), Color.white, 100f);

                OnGridValueChange += (OnGridValueChanged args) =>
                {
                    _debugTextArray[args.x, args.y].text = _gridArray[args.x, args.y]?.ToString();
                };
            }
        }


        public TGridObject GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                return _gridArray[x, y];
            }
            else
                return default;
        }

        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetGridObject(x, y);
        }

        public void SetGridObject(int x, int y, TGridObject value)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                _gridArray[x, y] = value;
                OnGridValueChange?.Invoke(new OnGridValueChanged { x = x, y = y });

                if (showDebug)
                    _debugTextArray[x, y].text = _gridArray[x, y].ToString();
            }
        }

        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetGridObject(x, y, value);
        }

        public Vector3 GetWorldPosition(int x, int y) => new Vector3(x, y) * Cellsize + _originPosition;

        public void TriggerGridObjectChanged(int x, int y)
        {
            OnGridValueChange?.Invoke(new OnGridValueChanged { x = x, y = y });
        }

        private void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / Cellsize);
            y = Mathf.FloorToInt((worldPosition - _originPosition).y / Cellsize);
        }
    }
}
