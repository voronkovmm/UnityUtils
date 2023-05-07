using UnityEngine;
using Voronkovmm;

namespace Voronkovmm
{
    public class HeatMapGridObject
    {
        private const int MIN = 0;
        private const int MAX = 100;

        private Grid<HeatMapGridObject> _grid;
        private int _x, _y;
        private int value;

        public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
        {
            _grid = grid;
            _x = x;
            _y = y;
        }

        public void AddValue(int addValue)
        {
            value += addValue;
            Mathf.Clamp(value, MIN, MAX);
            _grid.TriggerGridObjectChanged(_x, _y);
        }

        public float GetNormalizedValue() => (float)value / MAX;

        public override string ToString()
        {
            return value.ToString();
        }
    } 
}